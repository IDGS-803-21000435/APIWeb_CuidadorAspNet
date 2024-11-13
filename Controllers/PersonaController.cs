using Cuidador.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cuidador.Controllers
{
    [Route("api/crm/[controller]")]
    [ApiController]
    public class PersonaController : ControllerBase
    {
        private readonly DbAae280CuidadorContext _baseDatos;

        public PersonaController(DbAae280CuidadorContext baseDatos)
        {
            this._baseDatos = baseDatos;
        }

        [HttpGet]
        [Route("getPersona/{idUsuario}")]
        public async Task<IActionResult> getPersona(int idUsuario)
        {
            Usuario usuario = null;
            usuario = await _baseDatos.Usuarios.Where(u => u.IdUsuario == idUsuario)
                .Include(u => u.PersonaFisicas)
                .FirstOrDefaultAsync();

            if (usuario == null)
            {
                return BadRequest(new { res = "No encontrado" });
            }

            return Ok(usuario.PersonaFisicas);

        }

        [HttpGet]
        [Route("getUsuario/{idUsuario}")]
        public async Task<IActionResult> getUsuario(int idUsuario)
        {
            Usuario usuario = null;
            usuario = await _baseDatos.Usuarios.Where(u => u.IdUsuario == idUsuario).SingleOrDefaultAsync();

            if (usuario == null)
            {
                return BadRequest(new { res = "No encontrado" });
            }

            return Ok(usuario);
        }

        [HttpGet]
        [Route("getDatoMedico/{idUsuario}")]
        public async Task<IActionResult> getDatoMedico(int idUsuario)
        {
            Usuario usuario = null;
            usuario = await _baseDatos.Usuarios.Where(u => u.IdUsuario == idUsuario)
                .Include(u => u.PersonaFisicas)
                .FirstOrDefaultAsync();

            if (usuario == null)
            {
                return BadRequest(new { res = "No encontrado" });
            }

            PersonaFisica personaFisica =  usuario.PersonaFisicas.First();
            var persona = await _baseDatos.PersonaFisicas.Where(p => p.IdPersona == personaFisica.IdPersona)
                .Include(p => p.DatosMedicos)
                .SingleOrDefaultAsync();

            return Ok(persona.DatosMedicos);
        }

        [HttpGet]
        [Route("getDomicilio/{idUsuario}")]
        public async Task<IActionResult> getDomicilio(int idUsuario)
        {
            Usuario usuario = null;
            usuario = await _baseDatos.Usuarios.Where(u => u.IdUsuario == idUsuario)
                .Include(u => u.PersonaFisicas)
                .FirstOrDefaultAsync();

            if (usuario == null)
            {
                return BadRequest(new { res = "No encontrado" });
            }

            PersonaFisica personaFisica = usuario.PersonaFisicas.First();
            var persona = await _baseDatos.PersonaFisicas.Where(p => p.IdPersona == personaFisica.IdPersona)
                .Include(p => p.Domicilio)
                .SingleOrDefaultAsync();

            return Ok(persona.Domicilio);
        }

        [HttpGet]
        [Route("getDocumento/{idUsuario}")]
        public async Task<IActionResult> getDocumento(int idUsuario)
        {
            Usuario usuario = null;
            usuario = await _baseDatos.Usuarios.Where(u => u.IdUsuario == idUsuario)
                .Include(u => u.PersonaFisicas)
                .FirstOrDefaultAsync();

            if (usuario == null)
            {
                return BadRequest(new { res = "No encontrado" });
            }

            PersonaFisica personaFisica = usuario.PersonaFisicas.First();
            var persona = await _baseDatos.PersonaFisicas.Where(p => p.IdPersona == personaFisica.IdPersona)
                .Include(p => p.Documentacions)
                .SingleOrDefaultAsync();

            return Ok(persona.Documentacions);
        }
    }
}
