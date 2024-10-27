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

            foreach (var feedback in listaFeedback) {
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
                    Estatus = feedback.Estatus,
                    UsuarioidRemitenteNavigation = feedback.UsuarioidRemitenteNavigation
                };

                listaGetFeedback.Add(getFeedback);
            }

            return Ok(listaGetFeedback);
        }
    }
}
