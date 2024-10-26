using Cuidador.Dto.Documentos;
using Cuidador.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace Cuidador.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoocumentosController : ControllerBase
    {
        private readonly DbAaaabeCuidadorContext _baseDatos;

        public DoocumentosController(DbAaaabeCuidadorContext baseDatos)
        {
            this._baseDatos = baseDatos;
        }

        [HttpPut("updateDocumento")]
        public async Task<IActionResult> ModificarDocumento([FromBody] List<DocumentoDTO> documentacions)
        {
            if (documentacions == null || !documentacions.Any())
            {
                return BadRequest(new { error = "La lista de documentos no puede estar vacía." });
            }

            foreach (var documento in documentacions)
            {
                var documentoExistente = await _baseDatos.Documentacions.FindAsync(documento.IdDocumento);
                if (documentoExistente == null)
                {
                    return BadRequest(new {error = $"No se encontró el documento con ID {documento.IdDocumento}." });
                }

                documentoExistente.TipoDocumento = documento.TipoDocumento;
                documentoExistente.NombreDocumento = documento.NombreDocumento;
                documentoExistente.UrlDocumento = documento.UrlDocumento;
                documentoExistente.FechaEmision = documento.FechaEmision;
                documentoExistente.FechaExpiracion = documento.FechaExpiracion;
                documentoExistente.Version = documento.Version;
                documentoExistente.EstatusId = documento.EstatusId;
                documentoExistente.FechaModificacion = DateTime.Now;
                documentoExistente.UsuarioModifico = documento.UsuarioModifico; ;

                // actualizar la fecha de expiración a un año más tarde
                if (documento.FechaExpiracion.HasValue)
                {
                    documento.FechaExpiracion = documento.FechaExpiracion.Value.AddYears(1);
                }
            }

            try
            {
                await _baseDatos.SaveChangesAsync();
                return Ok(new {success = "informacion actualizada"});
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al actualizar los documentos: {ex.Message}");
            }
        }
    }
}
