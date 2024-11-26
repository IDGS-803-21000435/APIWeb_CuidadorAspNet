using Cuidador.Dto.User;
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
            var persona = await _baseDatos.PersonaFisicas
                .Where(p => p.IdPersona == personaFisica.IdPersona)
                .Include(p => p.DatosMedicos)
                    .ThenInclude(dm => dm.Padecimientos)
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

        [HttpGet]
        [Route("getCuidadoresPendientes/{tipoUsuarioId}/{estatusId}")]
        public async Task<IActionResult> GetCuidadoresPendientes(int tipoUsuarioId, int estatusId)
        {
            if (tipoUsuarioId < 0 || estatusId < 0)
            {
                return BadRequest(new { res = "Los parámetros tipoUsuarioId y estatusId deben ser mayores a 0 y válidos." });
            }

            var usuarios = await _baseDatos.Usuarios
                .Where(u => u.TipoUsuarioid == tipoUsuarioId && u.Estatusid == estatusId && u.PersonaFisicas.Any(p => p.EsFamiliar == 0))
                .Include(u => u.PersonaFisicas)
                .ToListAsync();

            List<UsuarioOUT> listUsuario = new List<UsuarioOUT>();

            foreach(var u in usuarios)
            {
                UsuarioOUT usuario = new UsuarioOUT
                {
                    IdUsuario = u.IdUsuario,
                    TipoUsuarioid = u.TipoUsuarioid,
                    UsuarionivelId = u.UsuarionivelId,
                    Usuario1 = u.Usuario1,
                    Contrasenia = u.Contrasenia,
                    Estatusid = u.Estatusid,
                    UsuarioModifico = u.UsuarioModifico,
                    FechaModificacion = u.FechaModificacion,
                    UsuarioRegistro = u.UsuarioRegistro,
                    FechaRegistro = u.FechaRegistro,
                    PersonaFisicas = u.PersonaFisicas.ToList(),
                };

                listUsuario.Add(usuario);
            }


            if (listUsuario.Count > 0)
            {
                return Ok(listUsuario);
            }
            else
            {
                return BadRequest(new { res = "No hay datos a mostrar." });
            }            
        }


        [HttpGet]
        [Route("getFamiliaresPendientes/{tipoUsuarioId}/{estatusId}")]
        public async Task<IActionResult> getFamiliaresPendientes(int tipoUsuarioId, int estatusId)
        {
            if (tipoUsuarioId < 0 || estatusId < 0)
            {
                return BadRequest(new { res = "Los parámetros tipoUsuarioId y estatusId deben ser mayores a 0 y válidos." });
            }

            var usuarios = await _baseDatos.Usuarios
                .Where(u => u.TipoUsuarioid == tipoUsuarioId && u.Estatusid == estatusId && u.PersonaFisicas.Any(p => p.EsFamiliar == 1))
                .Include(u => u.PersonaFisicas)
                .ToListAsync();

            List<UsuarioOUT> listUsuario = new List<UsuarioOUT>();

            foreach (var u in usuarios)
            {
                UsuarioOUT usuario = new UsuarioOUT
                {
                    IdUsuario = u.IdUsuario,
                    TipoUsuarioid = u.TipoUsuarioid,
                    UsuarionivelId = u.UsuarionivelId,
                    Usuario1 = u.Usuario1,
                    Contrasenia = u.Contrasenia,
                    Estatusid = u.Estatusid,
                    UsuarioModifico = u.UsuarioModifico,
                    FechaModificacion = u.FechaModificacion,
                    UsuarioRegistro = u.UsuarioRegistro,
                    FechaRegistro = u.FechaRegistro,
                    PersonaFisicas = u.PersonaFisicas.ToList(),
                };

                listUsuario.Add(usuario);
            }


            if (listUsuario.Count > 0)
            {
                return Ok(listUsuario);
            }
            else
            {
                return BadRequest(new { res = "No hay datos a mostrar." });
            }
        }
    }
}
