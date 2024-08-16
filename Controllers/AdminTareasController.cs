using Cuidador.Dto.Contract.AdminTareas;
using Cuidador.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace AdminTareasController.Hubs
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminTareasController : ControllerBase
    {

        private readonly DbAaaabeCuidadorContext _baseDatos;
        private readonly IHubContext<HubHelper> _hubContext;

        public AdminTareasController(DbAaaabeCuidadorContext baseDatos, IHubContext<HubHelper> hubContext)
        {
            this._baseDatos = baseDatos;
            this._hubContext = hubContext;
        }

        [HttpPost]
        [Route("cambiarEstatusTarea")]
        public async Task<IActionResult> CambiarEstatusTarea(ChangeEstatusDto change)
        {
            using (var transaction = await _baseDatos.Database.BeginTransactionAsync())
            {
                try
                {
                    TareasContrato? tarea = await _baseDatos.TareasContratos.FindAsync(change.id1);
                    if (tarea == null) return BadRequest("Id de tarea no encontrado");

                    tarea.EstatusId = change.id2;
                    switch (change.id2)
                    {
                        case 19: // EN CURSO
                            tarea.FechaInicio = DateTime.Now;
                            break;
                        case 18: // EN ESPERA
                            tarea.FechaPospuesta = DateTime.Now;
                            break;
                        case 9: // CONCLUIDA
                            tarea.FechaFinalizacion = DateTime.Now;
                            break;
                    }

                    _baseDatos.TareasContratos.Update(tarea);
                    await _baseDatos.SaveChangesAsync();

                    await _hubContext.Clients.All.SendAsync("ReceiveMessage", "Admin", $"Se ha cambiado el estatus de la tarea {change.id1} a {change.id2}");

                    await transaction.CommitAsync(); 

                    return Ok(new { success = true });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(); 
                    return StatusCode(500, $"Ocurrió un error: {ex.Message}");
                }
            }
        }


        [HttpPost]
        [Route("concluirContratoItem")]
        public async Task<IActionResult> ConcluirContratoItem(ChangeEstatusDto change)
        {
            ContratoItem? item = await _baseDatos.ContratoItems.FindAsync(change.id1, change.id2);
            if (item == null) return BadRequest("Id de item no encontrado");

            item.EstatusId = 9;
            item.FechaFinCuidado = DateTime.Now;
            _baseDatos.ContratoItems.Update(item);
            await _baseDatos.SaveChangesAsync();

            //Si todos los item del contrato estan concluidos, se concluye el contrato
            if (_baseDatos.ContratoItems.All(ci => ci.ContratoId == change.id1 && ci.EstatusId == 9))
            {
                Contrato? contrato = await _baseDatos.Contratos.FindAsync(change.id1);
                contrato.EstatusId = 9;
                _baseDatos.Contratos.Update(contrato);
                await _baseDatos.SaveChangesAsync();
            }

            await _hubContext.Clients.All.SendAsync("ReceiveMessage", "Admin", $"Se ha concluido el item {change.id2} del contrato {change.id1}");

            return Ok(new { success = true });
        }

        [HttpGet]
        [Route("procesoContrato/{contratoitem}")]
        public async Task<IActionResult> getProcesoContrato(int contratoitem)
        {
            ContratoItem? item = await _baseDatos.ContratoItems.FindAsync(contratoitem);

            if (item == null) return BadRequest("Id de item no encontrado");

            List<OUTEventosContrato> eventos = new List<OUTEventosContrato>();

            //Se añaden eventos del contrato item
            eventos.Add(new OUTEventosContrato
            {
                id = item.IdContratoitem,
                esTarea = false,
                titulo = "Aceptación del cuidado",
                fecha = item.FechaAceptacion,
                estatus = item.FechaAceptacion == null ? "Pendiente" : "Aceptado"
            });
            

            eventos.Add(new OUTEventosContrato
            {
                id = item.IdContratoitem,
                esTarea = false,
                titulo = "Inicio del cuidado",
                fecha = item.FechaInicioCuidado,
                estatus = item.FechaInicioCuidado == null ? "Pendiente" : "Iniciado"
            });

            foreach (TareasContrato tarea in await _baseDatos.TareasContratos.Where(t => t.ContratoitemId == contratoitem).ToListAsync())
            {
                Estatus? estatus = await _baseDatos.Estatuses.FindAsync(tarea.EstatusId);
                eventos.Add(new OUTEventosContrato
                {
                    id = tarea.IdTareas,
                    esTarea = true,
                    titulo = tarea.TituloTarea ?? "",
                    fecha = tarea.EstatusId == 7 ? tarea.FechaInicio ?? DateTime.MinValue : tarea.EstatusId == 8 || tarea.EstatusId == 9 ? tarea.FechaFinalizacion ?? DateTime.MinValue : tarea.FechaFinalizacion ?? DateTime.MinValue,
                    estatus = estatus.Nombre
                });
            }

            eventos.Add(new OUTEventosContrato
            {
                id = item.IdContratoitem,
                esTarea = false,
                titulo = "Fin del cuidado",
                fecha = item.FechaFinCuidado,
                estatus = item.FechaFinCuidado == null ? "Pendiente" : "Finalizado"
            });

            return Ok(eventos);
        }

        [HttpGet]
        [Route("listaTareasTest/{contratoitem}")]
        public async Task<IActionResult> getTareasTest(int contratoitem)
        {
            //lista con un join en el estatus de la tarea del contrato item
            var tareas = await _baseDatos.TareasContratos.Where(t => t.ContratoitemId == contratoitem)
                .Include(t => t.Estatus)
                .Select(t => new
                {
                    t.IdTareas,
                    t.TituloTarea,
                    t.DescripcionTarea,
                    t.FechaARealizar,
                    t.FechaInicio,
                    t.FechaPospuesta,
                    t.FechaFinalizacion,
                    t.EstatusId,
                    t.Estatus.Nombre,
                }).ToListAsync();
            return Ok(tareas);
        }

    }

    public class ChangeEstatusDto
    {
        public int id1 { get; set; }
        public int id2 { get; set; }
    }

}