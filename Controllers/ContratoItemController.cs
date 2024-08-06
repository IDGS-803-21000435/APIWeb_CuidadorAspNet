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
        private readonly SysCuidadorV2Context _baseDatos;

        public ContratoItemController(SysCuidadorV2Context baseDatos)
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
            var listarContrato = await _baseDatos.Contratos
                .Include(con => con.PersonaidCuidador)
                .Include(con => con.PersonaidCliente)
                .Include(con => con.Estatus)
                .ToListAsync();


            return Ok(listarContrato);
        }

        [HttpPost]
        [Route("guardarContrato")]
        public async Task<IActionResult> guardarContrato([FromBody] RegisterContractDTO data )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                //hay mas campos para registrar
                var contrato = new Contrato
                {
                    PersonaidCuidador = data.persona_cuidador_id,
                    PersonaidCliente = data.persona_cliente_id,
                    EstatusId = 18
                };

                _baseDatos.Contratos.Add(contrato);
                await _baseDatos.SaveChangesAsync();

                foreach (var contratoitemData in data.contrato_item)
                {
                    var contratoitem = new ContratoItem
                    {
                        ContratoId = contrato.IdContrato, // id Recuperado despues de la insercion
                        EstatusId = 18,
                        Observaciones = contratoitemData.observaciones,
                        HorarioInicioPropuesto = contratoitemData.horario_inicio_propuesto,
                        HorarioFinPropuesto = contratoitemData.horario_fin_propuesto
                    };

                    _baseDatos.Add(contratoitem);
                    await _baseDatos.SaveChangesAsync();

                    foreach (var tareas_contratoData in contratoitemData.tareas_contrato)
                    {
                        var tareaContrato = new TareasContrato
                        {
                            ContratoitemId = contratoitem.IdContratoitem, // id Recuperado
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

                var res = new
                {
                    successful = "Se agrego correctamente"
                };

                return Ok(res); 
            }
            catch (Exception ex) {
                return BadRequest(ex);
            }
        }
    }
}
