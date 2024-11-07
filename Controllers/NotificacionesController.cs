using Cuidador.Dto.Notificaciones;
using Cuidador.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cuidador.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	
	public class NotificacionesController : ControllerBase
	{
		
		private readonly DbAae280CuidadorContext _context;
		
		public NotificacionesController(DbAae280CuidadorContext context)
		{
			_context = context;
		}
		
		[HttpGet("{idPersona}")]
		public async Task<ActionResult<SalarioCuidador>> GetNotificaciones(int idPersona)
		{
			var notificaciones = await _context.Notificaciones
				.Where(h => h.PersonaidNoti == idPersona)
				.ToListAsync();
			
			if(notificaciones == null || !notificaciones.Any())
			{
				return BadRequest();
			}
			
			return Ok(notificaciones);
		}
		
		[HttpPost("agregarNotificacion")]
		public async Task<ActionResult<SalarioCuidador>> AgregarNotificacion(NuevaNotificacion notificaciones)
		{
			var notificacion = new Notificacione
			{
				PersonaidNoti = notificaciones.personaIdNoti,
				RutaMenu = notificaciones.rutaMenu,
				TituloNoti = notificaciones.tituloNoti,
				DescripcionNoti = notificaciones.descripcionNoti,
				FechaRegistro = DateTime.Now
			};
			
			_context.Notificaciones.Add(notificacion);
			await _context.SaveChangesAsync();
			
			return Ok(notificacion);
		}
		
		[HttpDelete("{idNotificacion}")]
		public async Task<ActionResult<SalarioCuidador>> EliminarNotificacion(int idNotificacion)
		{
			var notificacion = await _context.Notificaciones
				.Where(h => h.IdNotificacion == idNotificacion)
				.FirstOrDefaultAsync();
			
			if(notificacion == null)
			{
				return BadRequest();
			}
			
			_context.Notificaciones.Remove(notificacion);
			await _context.SaveChangesAsync();
			
			return Ok(notificacion);
		}
		
		
	}
	
}