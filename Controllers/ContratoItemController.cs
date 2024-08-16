using Cuidador.Dto.Contract.DetalleVistaCliente;
using Cuidador.Dto.Contract.ListarContrato;
using Cuidador.Dto.Contract.RegistrarDatoContrato;
using Cuidador.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cuidador.Dto;

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

        [HttpGet("detalleVistaCliente/{idContrato}")]
        public async Task<IActionResult> detalleVistaCliente(int idContrato)
        {
            try
            {
                var contrato = await _baseDatos.Contratos.SingleOrDefaultAsync(c => c.IdContrato.Equals(idContrato));
                if (contrato == null)
                {
                    return NotFound("Contrato no encontrado.");
                }

                var list_persona_cuidador = await _baseDatos.PersonaFisicas.Where(p => p.IdPersona == contrato.PersonaidCuidador).ToListAsync();

                var outListCuidador = new List<PersonaFisica>();

                foreach (var persona_cuidador in list_persona_cuidador)
                {
                    var domicilio = await _baseDatos.Domicilios.SingleOrDefaultAsync(d => d.IdDomicilio == persona_cuidador.DomicilioId);
                    var certificacionesExp = await _baseDatos.CertificacionesExperiencia.Where(cp => cp.PersonaId == persona_cuidador.IdPersona).ToListAsync();
                    var comentariosUs = await _baseDatos.ComentariosUsuarios.Where(cm => cm.PersonaReceptorid == persona_cuidador.IdPersona).ToListAsync();

                    var outPersona = new PersonaFisica
                    {
                        IdPersona = persona_cuidador.IdPersona,
                        Nombre = persona_cuidador.Nombre,
                        ApellidoPaterno = persona_cuidador.ApellidoPaterno,
                        ApellidoMaterno = persona_cuidador.ApellidoMaterno,
                        CorreoElectronico = persona_cuidador.CorreoElectronico,
                        FechaNacimiento = persona_cuidador.FechaNacimiento,
                        Genero = persona_cuidador.Genero,
                        EstadoCivil = persona_cuidador.EstadoCivil,
                        Domicilio = domicilio,
                        AvatarImage = persona_cuidador.AvatarImage,
                        CertificacionesExperiencia = certificacionesExp,
                        ComentariosUsuarioPersonaReceptors = comentariosUs
                    };

                    outListCuidador.Add(outPersona);
                }

                var lista_contratositem = await _baseDatos.ContratoItems.Where(c => c.ContratoId == contrato.IdContrato).ToListAsync();

                var outContratoitm = new List<ContratoItem>();
                foreach (var contratoitm in lista_contratositem)
                {
                    var tareas = await _baseDatos.TareasContratos.Where(t => t.ContratoitemId == contratoitm.ContratoId).ToListAsync();
                    var estatus = await _baseDatos.Estatuses.SingleOrDefaultAsync(e => e.IdEstatus == contratoitm.EstatusId);
                    var conitm = new ContratoItem
                    {
                        IdContratoitem = contratoitm.IdContratoitem,
                        Estatus = estatus,
                        Observaciones = contratoitm.Observaciones,
                        HorarioInicioPropuesto = contratoitm.HorarioInicioPropuesto,
                        HorarioFinPropuesto = contratoitm.HorarioFinPropuesto,
                        FechaAceptacion = contratoitm.FechaAceptacion,
                        FechaInicioCuidado = contratoitm.FechaInicioCuidado,
                        FechaFinCuidado = contratoitm.FechaFinCuidado,
                        ImporteTotal = contratoitm.ImporteTotal,
                        TareasContratos = tareas != null ? tareas : null
                    };

                    outContratoitm.Add(conitm);
                }

                var cont = new OUTContratoDetalle
                {
                    id_contrato = contrato.IdContrato,
                    persona_cuidador = outListCuidador,
                    contrato_item = outContratoitm
                };

                return Ok(cont);
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here using your logging framework of choice
                return StatusCode(500, $"Ocurrió un error al procesar la solicitud: {ex.Message}");
            }

        }


        [HttpGet]
        [Route("listarContrato/{idUsuario}/{tipousuarioid}")]
        public async Task<IActionResult> ListarContrato(int idUsuario, int tipousuarioid)
        {

            var personaUsuario = await _baseDatos.PersonaFisicas.Where(p => p.UsuarioId == idUsuario).ToListAsync();
            var contratos = new List<Contrato>();

            if (tipousuarioid == 1) // cuidador
            {
                foreach (var listpersona in personaUsuario)
                {
                    var contrato = await _baseDatos.Contratos.Where(c => c.PersonaidCuidador == listpersona.IdPersona).ToListAsync();
                    contratos.AddRange(contrato);
                }
            }
            else
            {
                foreach (var listpersona in personaUsuario)
                {
                    var contrato = await _baseDatos.Contratos.Where(c => c.PersonaidCliente == listpersona.IdPersona).ToListAsync();
                    contratos.AddRange(contrato);
                }
            }

            var outListaContrato = new List<OUTListarContrato>();

            foreach (var listasContrato in contratos)
            {
                var persona_cuidador = await _baseDatos.PersonaFisicas.SingleOrDefaultAsync(p => p.IdPersona == listasContrato.PersonaidCuidador);
                var persona_cliente = await _baseDatos.PersonaFisicas.SingleOrDefaultAsync(p => p.IdPersona == listasContrato.PersonaidCliente);


                var contratoitem = await _baseDatos.ContratoItems.Where(c => c.ContratoId == listasContrato.IdContrato).ToListAsync();

                foreach (var i in contratoitem)
                {
                    var estatus = await _baseDatos.Estatuses.SingleOrDefaultAsync(e => e.IdEstatus == i.EstatusId);
                    var obj = new OUTListarContrato
                    {
                        id_contrato = listasContrato.IdContrato,
                        id_contrato_item = i.IdContratoitem,
                        horario_inicio = i.HorarioInicioPropuesto ?? DateTime.MinValue,
                        horario_fin = i.HorarioFinPropuesto ?? DateTime.MinValue,
                        estatus = estatus,
                        persona_cuidador = persona_cuidador,
                        persona_cliente = persona_cliente,
                        importe_cuidado = i.ImporteTotal ?? 0,
                        numero_de_tareas = await _baseDatos.TareasContratos.Where(t => t.ContratoitemId == i.IdContratoitem).CountAsync()
                    };
                    outListaContrato.Add(obj);
                }

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
                        // Asegúrate de que 'diferencia' no sea nulo antes de intentar acceder a sus propiedades
                        TimeSpan? diferencia = (contratoitemData.horario_fin_propuesto - contratoitemData.horario_inicio_propuesto);

                        if (diferencia.HasValue)
                        {
                            // Calcula los minutos sólo si 'diferencia' tiene un valor
                            decimal minutos = diferencia.Value.Days * 24 * 60 + diferencia.Value.Hours * 60 + diferencia.Value.Minutes;
                            var persona_cuidador = await _baseDatos.PersonaFisicas.FirstOrDefaultAsync(p => p.IdPersona == data.persona_cuidador_id);
                            var salario_coidador = await _baseDatos.SalarioCuidadors.FirstOrDefaultAsync(s => s.Usuarioid == persona_cuidador.UsuarioId);
                            var contratoitem = new ContratoItem
                            {
                                ContratoId = contrato.IdContrato, // id que se establecerá después de SaveChangesAsync
                                EstatusId = 18,
                                Observaciones = contratoitemData.observaciones,
                                HorarioInicioPropuesto = contratoitemData.horario_inicio_propuesto,
                                HorarioFinPropuesto = contratoitemData.horario_fin_propuesto,
                                ImporteTotal = Math.Round((decimal)(minutos * (salario_coidador.PrecioPorHora / 60)), 2)
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
                                        EstatusId = 7,
                                        FechaARealizar = tareas_contratoData?.FechaARealizar,
                                    };
                                    _baseDatos.Add(tareaContrato);
                                    await _baseDatos.SaveChangesAsync();
                                }
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

        // codigo kev

        [HttpPost]
        [Route("cambiarEstatusContratoItem")]
        public async Task<IActionResult> CambiarEstatusContratoItem(OUTChangeEstatus change)
        {
            using (var transaction = await _baseDatos.Database.BeginTransactionAsync())
            {
                try
                {
                    ContratoItem? item = await _baseDatos.ContratoItems.FindAsync(change.id_contrato_item);
                    if (item == null) return BadRequest("Id de item no encontrado");

                    item.EstatusId = change.id_estatus;
                    item.FechaAceptacion = DateTime.Now;
                    _baseDatos.ContratoItems.Update(item);
                    await _baseDatos.SaveChangesAsync();

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
    }
}
