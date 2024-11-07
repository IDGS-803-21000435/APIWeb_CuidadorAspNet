using Cuidador.Dto.Comentarios;
using Cuidador.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cuidador.Controllers
{
	
	[Route("api/[controller]")]
	[ApiController]
	
	public class ComentariosController : ControllerBase
	{
		
		private readonly DbAae280CuidadorContext _context;
		
		public ComentariosController(DbAae280CuidadorContext context)
		{
			_context = context;
		}
		
		[HttpGet("{idPersona}")]
		public async Task<ActionResult<SalarioCuidador>> GetComentarios(int idPersona)
		{
			var comentarios = await _context.ComentariosUsuarios
				.Where(h => h.PersonaReceptorid == idPersona)
				.ToListAsync();
			
			if(comentarios == null || !comentarios.Any())
			{
				return BadRequest();
			}
			
			return Ok(comentarios);
		}
		
		[HttpPost("agregarComentario")]
		public async Task<ActionResult<SalarioCuidador>> AgregarComentario(NuevoComentario comentario)
		{
			var comentarioNuevo = new ComentariosUsuario
			{
				PersonaEmisorid = comentario.idPersonaEmisor,
				PersonaReceptorid = comentario.idPersonaReceptor,
				Comentario = comentario.comentario,
				Calificacion = comentario.calificacion,
				FechaRegistro = DateTime.Now,
				UsuarioRegistro = comentario.idPersonaEmisor
			};
			
			_context.ComentariosUsuarios.Add(comentarioNuevo);
			await _context.SaveChangesAsync();
			
			return Ok(comentarioNuevo);
		}
		
		[HttpPost("actualizarComentario")]
		public async Task<ActionResult<SalarioCuidador>> ActualizarComentario(UpdateComentario comentario)
		{
			var comentarioActualizado = await _context.ComentariosUsuarios
				.Where(h => h.IdComentarios == comentario.idComentario)
				.FirstOrDefaultAsync();
			
			if(comentarioActualizado == null)
			{
				return BadRequest();
			}
			
			comentarioActualizado.Comentario = comentario.comentario;
			comentarioActualizado.Calificacion = comentario.calificacion;
			comentarioActualizado.FechaModificacion = DateTime.Now;
			
			_context.Entry(comentarioActualizado).State = EntityState.Modified;
			await _context.SaveChangesAsync();
			
			return Ok(comentarioActualizado);
		}
		
		[HttpDelete("{idComentario}")]
		public async Task<ActionResult<SalarioCuidador>> EliminarComentario(int idComentario)
		{
			var comentario = await _context.ComentariosUsuarios
				.Where(h => h.IdComentarios == idComentario)
				.FirstOrDefaultAsync();
			
			if(comentario == null)
			{
				return BadRequest();
			}
			
			_context.ComentariosUsuarios.Remove(comentario);
			await _context.SaveChangesAsync();
			
			return Ok(comentario);
		}
		
	}
	
}