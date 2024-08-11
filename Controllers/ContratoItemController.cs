using Cuidador.Dto.Contract.ListarContrato;
using Cuidador.Dto.Contract.RegistrarDatoContrato;
using Cuidador.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cuidador.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContratoItemController : ControllerBase
    {
        private readonly DbAaaabeCuidadorContext _baseDatos;

        public ContratoItemController(DbAaaabeCuidadorContext baseDatos)
        {
            this._baseDatos = baseDatos;
        }

        [HttpGet]
        [Route("fechasAOcupar")]
        public async Task<IActionResult> FechasAOcupadarCuidador()
        {
            var listaUsuario = await _baseDatos.ContratoItems
                .Where(item => item.EstatusId == 7)
                .ToListAsync();
            return Ok(listaUsuario);
        }

        [HttpGet]
        [Route("listarContrato")]
        public async Task<IActionResult> ListarContrato()
        {
            var contratos = await _baseDatos.Contratos.ToListAsync();
            var outListaContrato = new List<OUTListarContrato>();
            
            foreach (var listasContrato in contratos)
            {
                var persona_cuidador = await _baseDatos.PersonaFisicas.SingleOrDefaultAsync(p => p.IdPersona == listasContrato.PersonaidCuidador);
                var persona_cliente = await _baseDatos.PersonaFisicas.SingleOrDefaultAsync(p => p.IdPersona == listasContrato.PersonaidCliente);
                var estatus = await _baseDatos.Estatuses.SingleOrDefaultAsync(e => e.IdEstatus == listasContrato.EstatusId);

                var outObject = new OUTListarContrato
                {
                    id_contrato = listasContrato.IdContrato,
                    persona_cliente = persona_cliente,
                    persona_cuidador = persona_cuidador,
                    estatus = estatus
                };
                outListaContrato.Add(outObject);
            }

            return Ok(outListaContrato);
        }

        [HttpPost]
        [Route("guardarContrato")]
        public async Task<IActionResult> guardarContrato([FromBody] RegisterContractDTO data )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (var transaction = await _baseDatos.Database.BeginTransactionAsync())
            {
                try
                {
                    // hay más campos para registrar
                    var contrato = new Contrato
                    {
                        PersonaidCuidador = data.persona_cuidador_id,
                        PersonaidCliente = data.persona_cliente_id,
                        EstatusId = 18,
                        FechaRegistro = DateTime.Now,
                        FechaModifico = null
                    };

                    _baseDatos.Contratos.Add(contrato);
                    await _baseDatos.SaveChangesAsync();

                    foreach (var contratoitemData in data.contrato_item)
                    {
                        var contratoitem = new ContratoItem
                        {
                            ContratoId = contrato.IdContrato, // id que se establecerá después de SaveChangesAsync
                            EstatusId = 18,
                            Observaciones = contratoitemData.observaciones,
                            HorarioInicioPropuesto = contratoitemData.horario_inicio_propuesto,
                            HorarioFinPropuesto = contratoitemData.horario_fin_propuesto
                        };

                        _baseDatos.Add(contratoitem);
                        await _baseDatos.SaveChangesAsync();

                        if (contratoitemData.tareas_contrato != null)
                        {
                            foreach (var tareas_contratoData in contratoitemData.tareas_contrato)
                            {
                                var tareaContrato = new TareasContrato
                                {
                                    ContratoitemId = contratoitem.IdContratoitem, // id que se establecerá después de SaveChangesAsync
                                    TituloTarea = tareas_contratoData.TituloTarea,
                                    DescripcionTarea = tareas_contratoData.DescripcionTarea,
                                    TipoTarea = tareas_contratoData.TipoTarea,
                                    EstatusId = 19,
                                    FechaARealizar = tareas_contratoData?.FechaARealizar,
                                };
                                _baseDatos.Add(tareaContrato);
                                await _baseDatos.SaveChangesAsync();
                            }
                        }
                    }

                    // Aquí hacemos una única llamada a SaveChangesAsync, lo que asegura que todos los objetos se guarden de una vez.
                    await _baseDatos.SaveChangesAsync();

                    // Si todo va bien, confirmamos la transacción
                    await transaction.CommitAsync();

                    var res = new
                    {
                        successful = "Se agregó correctamente"
                    };

                    return Ok(res);
                }
                catch (DbUpdateException dbEx)
                {
                    // Captura la excepción interna para obtener más detalles
                    var innerException = dbEx.InnerException;
                    return BadRequest(new { error = dbEx.Message, innerError = innerException?.Message });
                }
                catch (Exception ex)
                {
                    // En caso de error, deshacemos la transacción
                    await transaction.RollbackAsync();
                    return BadRequest(new { error = ex.Message });
                }
            }
        }
    }
}
