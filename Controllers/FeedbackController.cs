using Cuidador.Dto.Feedback;
using Cuidador.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cuidador.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : Controller
    {
        private readonly DbAae280CuidadorContext _context;

        public FeedbackController(DbAae280CuidadorContext context) 
        { 
            this._context = context;
        }

        [HttpPost]
        [Route("insert")]
        public async Task<IActionResult> insert([FromBody] insertFeedback feedback)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var newFeedback = new Feedback
                {
                    UsuarioidRemitente = feedback.UsuarioIdRemitente,
                    Categoria = feedback.Categoria,
                    Cuerpo = feedback.Cuerpo,
                    Fecha = DateOnly.FromDateTime(DateTime.Now),
                    FechaResolucion = null,
                    Estatusid = 20,
                    UsuarioRegistro = feedback.UsuarioRegistro,
                    FechaRegistro = DateOnly.FromDateTime(DateTime.Now)
                };

                _context.Feedbacks.Add(newFeedback);
                await _context.SaveChangesAsync();

                return Ok(new { res = "Se registro exitosamente" });
            }
            catch(Exception ex)
            {
                return BadRequest($"Failed to insert {ex.Message}");
            }
        }

        [HttpGet]
        [Route("getFeedback")]
        public async Task<IActionResult> getFeedback()
        {
            List<getFeedback> listaGetFeedback = new List<getFeedback>();
            var listaFeedback = await _context.Feedbacks.ToListAsync();
            var listaUsuarios = await _context.Usuarios.ToListAsync();
            var listaEstatus = await _context.Estatuses.ToListAsync();

            foreach (var feedback in listaFeedback) {
                var usuario = listaUsuarios.Where(lu => lu.IdUsuario == feedback.UsuarioidRemitente);
                var estatus = listaEstatus.Where(e => e.IdEstatus == feedback.Estatusid);
                var getFeedback = new getFeedback
                {
                    IdFeedback = feedback.IdFeedback,
                    UsuarioIdRemitente = feedback.UsuarioidRemitente,
                    Categoria = feedback.Categoria,
                    Cuerpo = feedback.Cuerpo,
                    Fecha = feedback.Fecha,
                    FechaResolucion = feedback.FechaResolucion,
                    EstatusId = feedback.Estatusid,
                    UsuarioRegistro = feedback.UsuarioRegistro,
                    FechaRegistro = feedback.FechaRegistro,
                    Estatus = estatus != null ? estatus.SingleOrDefault(): null,
                    UsuarioIdRemitenteNavigation = usuario != null ? usuario.SingleOrDefault(): null
                };

                listaGetFeedback.Add(getFeedback);
            }

            return Ok(listaGetFeedback);
        }

        [HttpGet]
        [Route("getFeedbackFilted/{id}")]
        public async Task<IActionResult> getFeedbackPorId(int id)
        {
            List<getFeedback> listaGetFeedback = new List<getFeedback>();
            var listaFeedback = await _context.Feedbacks.Where(u => u.UsuarioidRemitente == id).ToListAsync();
            
            if (listaFeedback != null && listaFeedback.Count > 0) 
            {
                var listaUsuarios = await _context.Usuarios.ToListAsync();
                var listaEstatus = await _context.Estatuses.ToListAsync();

                foreach (var feedback in listaFeedback)
                {
                    var usuario = listaUsuarios.SingleOrDefault(lu => lu.IdUsuario == feedback.UsuarioidRemitente);
                    var estatus = listaEstatus.SingleOrDefault(e => e.IdEstatus == feedback.Estatusid);
                    var getFeedback = new getFeedback
                    {
                        IdFeedback = feedback.IdFeedback,
                        UsuarioIdRemitente = feedback.UsuarioidRemitente,
                        Categoria = feedback.Categoria,
                        Cuerpo = feedback.Cuerpo,
                        Fecha = feedback.Fecha,
                        FechaResolucion = feedback.FechaResolucion,
                        EstatusId = feedback.Estatusid,
                        UsuarioRegistro = feedback.UsuarioRegistro,
                        FechaRegistro = feedback.FechaRegistro,
                        Estatus = estatus != null ? estatus : null,
                        UsuarioIdRemitenteNavigation = usuario != null ? usuario : null
                    };

                    listaGetFeedback.Add(getFeedback);
                }

                return Ok(listaGetFeedback);
            }
            else
            {
                return BadRequest(new { res = "no encontrado" });
            };
        }


        [HttpPost("aceptarFeedback/{id}/{idEstatus}")]
        public async Task<IActionResult> aceptarFeedback(int id, int idEstatus)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null)
            {
                return NotFound();
            }

            feedback.Estatusid = idEstatus;
            feedback.FechaResolucion = DateOnly.FromDateTime(DateTime.Now);
            _context.Entry(feedback).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { res = "Se actualizo exitosamente" });
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest("Failed to update");
            }
        }
    }
}
