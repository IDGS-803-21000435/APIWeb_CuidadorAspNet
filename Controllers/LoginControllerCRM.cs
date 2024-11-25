using Cuidador.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cuidador.Controllers
{
    [Route("api/crm")]
    [ApiController]
    public class LoginControllerCRM : ControllerBase
    {
        // VARIABLE DE COTEXTO DE BD
        private readonly DbAae280CuidadorContext _baseDatos;

        public LoginControllerCRM(DbAae280CuidadorContext baseDatos)
        {
            this._baseDatos = baseDatos;
        }

        public class LoginBoody
        {
            public string usuario { get; set; }
            public string contrasenia { get; set; }
        }

        [HttpPost]
        [Route("admin/login")]
        public async Task<IActionResult> Login([FromBody] LoginBoody bodyLogin)
        {
            try
            {
                Estatus estatus = new Estatus();
                PersonaFisica personaFisica = new PersonaFisica();
                Usuario usuario = new Usuario();
                usuario = await _baseDatos.Usuarios.SingleOrDefaultAsync(u => u.Usuario1 == bodyLogin.usuario);

                if(usuario != null)
                {
                    if(usuario.Contrasenia == bodyLogin.contrasenia)
                    {
                        
                        personaFisica = await _baseDatos.PersonaFisicas.SingleOrDefaultAsync(p => p.UsuarioId == usuario.IdUsuario);
                        estatus = await _baseDatos.Estatuses.SingleOrDefaultAsync(e => e.IdEstatus == usuario.Estatusid);

                        var outObject = new
                        {
                            IdUsuario = usuario.IdUsuario,
                            UsuarionivelId = usuario.UsuarionivelId,
                            TipoUsuarioid = usuario.TipoUsuarioid,
                            Usuario1 = usuario.Usuario1,
                            Contrasenia = usuario.Contrasenia,
                            FechaRegistro = usuario.FechaRegistro,
                            UsuarioRegistro = usuario.UsuarioRegistro,
                            FechaModificacion = usuario.FechaModificacion,
                            UsuarioModifico = usuario.UsuarioModifico,
                            personaFisica = personaFisica,
                            estatus = estatus,
                        };

                        return Ok(outObject);
                        
                    }
                    else
                    {
                        return BadRequest(new { res = "Credenciales incorrectas" });
                    }
                }
                else
                {
                    return BadRequest(new { res = "No encontrado" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Fallo en el try: "+ex.Message);
            }
        }
    }
}
