using Cuidador.Dto.Contract.DetalleVistaCliente;
using Cuidador.Dto.Contract.ListarContrato;
using Cuidador.Dto.Contract.RegistrarDatoContrato;
using Cuidador.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cuidador.Dto;
using Cuidador.Dto.Contract.FechasCuidador;
using System.Data;
using Dapper;
using System.Data.Common;

namespace Cuidador.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContratoItemController : ControllerBase
    {
        private readonly DbAaaabeCuidadorContext _baseDatos;
        private readonly IDbConnection _connection;

        public ContratoItemController(DbAaaabeCuidadorContext baseDatos)
        {
            this._baseDatos = baseDatos;
            this._connection = _baseDatos.Database.GetDbConnection();
        }

        [HttpGet]
        [Route("fechasOcupadasCuidador/{idPersona}")]
        public async Task<IActionResult> FechasAOcupadarCuidador(int idPersona)
        {
            var listaContratos = await _baseDatos.Contratos.Where(c => c.PersonaidCuidador == idPersona).ToListAsync();
            var listaFechas = new List<OUTFechasOcupadasCuidador>();
            
            if (listaContratos != null)
            {
                foreach (var contrato in listaContratos)
                {
                    var contratoItm = await _baseDatos.ContratoItems.Where(c => c.ContratoId == contrato.IdContrato).ToListAsync();
                    var fechaActual = DateTime.Now;

                    var filtroContrato = contratoItm.Where(c => (c.EstatusId == 7 || c.EstatusId == 18 || c.EstatusId == 19)
                                    && c.HorarioInicioPropuesto.HasValue
                                    && c.HorarioInicioPropuesto.Value >= fechaActual).ToList();

                    if (filtroContrato == null)
                    {
                        return BadRequest(new { error = "sin valores actuales para mostrar" });
                    }
                    else
                    {
                        // Procesar los contratos filtrados
                        foreach (var i in filtroContrato)
                        {
                            var aux = new OUTFechasOcupadasCuidador
                            {
                                horario_inicio_propuesto = i.HorarioInicioPropuesto,
                                horario_fin_propuesto = i.HorarioFinPropuesto
                            };

                            listaFechas.Add(aux);
                        }
                    }

                    
                }
                return Ok(listaFechas);
            }

            return BadRequest(new { error = "no se encontro los contratos"});
        }

        [HttpGet("detalleVistaCliente/{idContrato}")]
        public async Task<IActionResult> DetalleVistaCliente(int idContrato)
        {
            try
            {
                // Obtener el contrato
                var contrato = await _baseDatos.Contratos.SingleOrDefaultAsync(c => c.IdContrato == idContrato);
                if (contrato == null)
                {
                    return NotFound("Contrato no encontrado.");
                }

                // Obtener la información del cuidador
                var listPersonaCuidador = await _baseDatos.PersonaFisicas
                    .Where(p => p.IdPersona == contrato.PersonaidCuidador)
                    .ToListAsync();

                // Obtener en un solo lote domicilios, certificaciones y comentarios de los cuidadores
                var cuidadorIds = listPersonaCuidador.Select(p => p.IdPersona).ToList();
                var domicilios = await _baseDatos.Domicilios
                    .Where(d => listPersonaCuidador.Select(p => p.DomicilioId).Contains(d.IdDomicilio))
                    .ToListAsync();

                var certificacionesExperiencia = await _baseDatos.CertificacionesExperiencia
                    .Where(ce => cuidadorIds.Contains(ce.PersonaId))
                    .ToListAsync();

                var comentariosUsuarios = await _baseDatos.ComentariosUsuarios
                    .Where(cm => cuidadorIds.Contains(cm.PersonaReceptorid))
                    .ToListAsync();

                // Crear la lista de cuidadores
                var outListCuidador = listPersonaCuidador.Select(personaCuidador => new PersonaFisica
                {
                    IdPersona = personaCuidador.IdPersona,
                    Nombre = personaCuidador.Nombre,
                    ApellidoPaterno = personaCuidador.ApellidoPaterno,
                    ApellidoMaterno = personaCuidador.ApellidoMaterno,
                    CorreoElectronico = personaCuidador.CorreoElectronico,
                    FechaNacimiento = personaCuidador.FechaNacimiento,
                    Genero = personaCuidador.Genero,
                    EstadoCivil = personaCuidador.EstadoCivil,
                    Domicilio = domicilios.SingleOrDefault(d => d.IdDomicilio == personaCuidador.DomicilioId),
                    AvatarImage = personaCuidador.AvatarImage,
                    CertificacionesExperiencia = certificacionesExperiencia
                        .Where(ce => ce.PersonaId == personaCuidador.IdPersona)
                        .ToList(),
                    ComentariosUsuarioPersonaReceptors = comentariosUsuarios
                        .Where(cm => cm.PersonaReceptorid == personaCuidador.IdPersona)
                        .ToList()
                }).ToList();

                // Obtener todos los items de contrato y las tareas relacionadas
                var contratoItems = await _baseDatos.ContratoItems
                    .Where(c => c.ContratoId == contrato.IdContrato)
                    .ToListAsync();

                var contratoItemIds = contratoItems.Select(ci => ci.IdContratoitem).ToList();
                var tareasContratos = await _baseDatos.TareasContratos
                    .Where(t => contratoItemIds.Contains(t.ContratoitemId))
                    .ToListAsync();

                var estatusIds = contratoItems.Select(ci => ci.EstatusId).Distinct().ToList();
                var estatusList = await _baseDatos.Estatuses
                    .Where(e => estatusIds.Contains(e.IdEstatus))
                    .ToListAsync();

                // Crear la lista de contrato items
                var outContratoitm = contratoItems.Select(contratoitm => new ContratoItem
                {
                    IdContratoitem = contratoitm.IdContratoitem,
                    Estatus = estatusList.SingleOrDefault(e => e.IdEstatus == contratoitm.EstatusId),
                    Observaciones = contratoitm.Observaciones,
                    HorarioInicioPropuesto = contratoitm.HorarioInicioPropuesto,
                    HorarioFinPropuesto = contratoitm.HorarioFinPropuesto,
                    FechaAceptacion = contratoitm.FechaAceptacion,
                    FechaInicioCuidado = contratoitm.FechaInicioCuidado,
                    FechaFinCuidado = contratoitm.FechaFinCuidado,
                    ImporteTotal = contratoitm.ImporteTotal,
                    TareasContratos = tareasContratos
                        .Where(t => t.ContratoitemId == contratoitm.IdContratoitem)
                        .ToList()
                }).ToList();

                // Preparar la salida final
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
            var personaUsuario = await _baseDatos.PersonaFisicas
                .Where(p => p.UsuarioId == idUsuario)
                .ToListAsync();

            List<int> personaIds = personaUsuario.Select(p => p.IdPersona).ToList();

            List<Contrato> contratos;
            if (tipousuarioid == 1) // 1 = cuidador | 2 = cliente
            {
                contratos = await _baseDatos.Contratos
                    .Where(c => personaIds.Contains(c.PersonaidCuidador))
                    .ToListAsync();
            }
            else 
            {
                contratos = await _baseDatos.Contratos
                    .Where(c => personaIds.Contains(c.PersonaidCliente))
                    .ToListAsync();
            }

            var contratoIds = contratos.Select(c => c.IdContrato).ToList();

            var contratoItems = await _baseDatos.ContratoItems
                .Where(ci => contratoIds.Contains(ci.ContratoId))
                .ToListAsync();

            var tareasContratos = await _baseDatos.TareasContratos
                .Where(tc => contratoItems.Select(ci => ci.IdContratoitem).Contains(tc.ContratoitemId))
                .ToListAsync();

            var estatusIds = contratoItems.Select(ci => ci.EstatusId).Distinct().ToList();
            var estatusList = await _baseDatos.Estatuses
                .Where(e => estatusIds.Contains(e.IdEstatus))
                .ToListAsync();

            var cuidadorIds = contratos.Select(c => c.PersonaidCuidador).Distinct().ToList();
            var clienteIds = contratos.Select(c => c.PersonaidCliente).Distinct().ToList();

            var cuidadores = await _baseDatos.PersonaFisicas
                .Where(p => cuidadorIds.Contains(p.IdPersona))
                .ToListAsync();

            var clientes = await _baseDatos.PersonaFisicas
                .Where(p => clienteIds.Contains(p.IdPersona))
                .ToListAsync();

            var outListaContrato = new List<OUTListarContrato>();

            // Mapear los contratos con sus items y detalles
            foreach (var contrato in contratos)
            {
                var persona_cuidador = cuidadores.SingleOrDefault(p => p.IdPersona == contrato.PersonaidCuidador);
                var persona_cliente = clientes.SingleOrDefault(p => p.IdPersona == contrato.PersonaidCliente);

                var contratoItemsFiltrados = contratoItems.Where(ci => ci.ContratoId == contrato.IdContrato).ToList();

                foreach (var i in contratoItemsFiltrados)
                {
                    var estatus = estatusList.SingleOrDefault(e => e.IdEstatus == i.EstatusId);
                    var numero_de_tareas = tareasContratos.Count(tc => tc.ContratoitemId == i.IdContratoitem);

                    var obj = new OUTListarContrato
                    {
                        id_contrato = contrato.IdContrato,
                        horario_inicio = i.HorarioInicioPropuesto ?? DateTime.MinValue,
                        horario_fin = i.HorarioFinPropuesto ?? DateTime.MinValue,
                        estatus = estatus,
                        persona_cuidador = persona_cuidador,
                        persona_cliente = persona_cliente,
                        importe_cuidado = i.ImporteTotal ?? 0,
                        numero_de_tareas = numero_de_tareas
                    };
                    outListaContrato.Add(obj);
                }
            }

            return Ok(outListaContrato);
        }


        [HttpGet]
        [Route("listarContratoDapper/{idUsuario}")]
        public async Task<IActionResult> listarContratoDapper(int idUsuario)
        {
            var sql = @"

                SELECT
                    id_persona              AS IdPersona
		            ,nombre                 AS Nombre
		            ,apellido_paterno       AS ApellidoPaterno
                    ,apellido_materno       AS ApellidoMaterno
                    ,correo_electronico     AS CorreoElectronico
                    ,fecha_nacimiento       AS FechaNacimiento
                    ,genero                 AS Genero
                    ,estado_civil           AS EstadoCivil
                    ,rfc                    AS RFC
                    ,curp                   AS Curp
                    ,telefono_particular    AS TelefonoParticular
                    ,telefono_movil         AS TelefonoMovil
                    ,telefono_emergencia    AS TelefonoEmergencia
                    ,nombrecompleto_familiar AS NombrecompletoFamiliar
                    ,domicilio_id           AS DomicilioId
                    ,datos_medicosid        AS DatosMedicosid
                    ,fecha_registro         AS FechaRegistro
                    ,usuario_registro       AS UsuarioRegistro
                    ,fecha_modificacion     AS FechaModificacion
                    ,usuario_modifico       AS UsuarioModifico
                    ,usuario_id             AS UsuarioId
                    ,avatar_image           AS AvatarImage
                    ,estatus_id             AS EstatusId
                    ,esFamiliar             AS EsFamiliar
                FROM persona_fisica WHERE usuario_id = @IdUsuario;
            
                SELECT 
                    c.id_contrato			AS IdContrato
                    ,c.personaid_cuidador	AS PersonaidCuidador
                    ,c.personaid_cliente	AS PersonaidCliente
                    ,c.estatus_id			AS EstatusId
                    ,c.usuario_registro	    AS UsuarioRegistro
                    ,c.fecha_registro		AS FechaRegistro
                    ,c.usuario_modifico	    AS UsuarioModifico
                    ,c.fecha_modifico		AS FechaModifico
                FROM contrato c
                LEFT JOIN persona_fisica p ON 
                    (c.personaid_cuidador = p.id_persona)
                    OR (c.personaid_cliente = p.id_persona)
                WHERE p.usuario_id = @IdUsuario;

            ";

            using (var multi = await _connection.QueryMultipleAsync(sql, new
            {
                IdUsuario = idUsuario
            }))
            {
                // Cargar todas las personas relacionadas al usuario
                var personaUsuario = (await multi.ReadAsync<PersonaFisica>()).ToList();

                if (!personaUsuario.Any())
                {
                    return NotFound(new { error = "No se encontraron personas asociadas al usuario" });
                }

                // Obtener la segunda consulta
                var contratos = (await multi.ReadAsync<Contrato>()).ToList();

                if (!contratos.Any())
                {
                    return NotFound(new { error = "No se encontraron contratos" });
                }

                // Obtener los IDs de los contratos para las consultas siguientes
                var contratoIds = contratos.Select(c => c.IdContrato).ToList();

                // Obtener los items de los contratos
                var contratoItems = await _connection.QueryAsync<ContratoItem>(
                    @"
                        SELECT  *
                        FROM contrato_item 
                        WHERE contrato_id IN @ContratoIds
                    ",
                    new { ContratoIds = contratoIds });

                // Obtener los estatus relacionados con los contrato items usando los IDs de contrato obtenidos
                var estatusIds = contratoItems.Select(ci => ci.EstatusId).Distinct().ToList();
                var estatuses = await _connection.QueryAsync<Estatus>(
                    @"
                        SELECT * FROM estatus WHERE id_estatus IN @EstatusIds;
                    ",
                    new { EstatusIds = estatusIds });

                // Obtener las tareas relacionadas con los contrato items
                var contratoItemIds = contratoItems.Select(ci => ci.IdContratoitem).ToList();
                var tareasContratos = await _connection.QueryAsync<TareasContrato>(
                    @"
                        SELECT * FROM tareas_contrato WHERE contratoitem_id IN @ContratoItemIds
                    ",
                    new { ContratoItemIds = contratoItemIds });

                // Armar la lista final de salida
                var outListaContrato = new List<OUTListarContrato>();

                foreach (var contrato in contratos)
                {
                    var items = contratoItems.Where(ci => ci.ContratoId == contrato.IdContrato).ToList();
                    Console.WriteLine("--------------------------" + contrato.IdContrato);
                    foreach (var item in items)
                    {
                        var estatus = estatuses.SingleOrDefault(e => e.IdEstatus == item.EstatusId);
                        var numeroDeTareas = tareasContratos.Count(t => t.ContratoitemId == item.IdContratoitem);

                        var obj = new OUTListarContrato
                        {
                            id_contrato = contrato.IdContrato,
                            horario_inicio = item.HorarioInicioPropuesto ?? DateTime.MinValue,
                            horario_fin = item.HorarioFinPropuesto ?? DateTime.MinValue,
                            estatus = estatus,
                            persona_cuidador = contrato.PersonaidCuidadorNavigation,
                            persona_cliente = contrato.PersonaidClienteNavigation,
                            importe_cuidado = item.ImporteTotal ?? 0,
                            numero_de_tareas = numeroDeTareas
                        };

                        outListaContrato.Add(obj);
                    }
                }
                

                return Ok(outListaContrato);
            }
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
                    if (change.id_estatus == 7)
                    {
                        item.FechaInicioCuidado = DateTime.Now;
                    }
                    else if (change.id_estatus == 8)
                    {
                        item.FechaFinCuidado = DateTime.Now;
                    }
                    else if (change.id_estatus == 19)
                    {
                        item.FechaInicioCuidado = DateTime.Now;
                    }

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
