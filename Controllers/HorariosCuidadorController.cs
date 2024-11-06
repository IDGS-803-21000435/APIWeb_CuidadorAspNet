using Cuidador.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cuidador.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	
	public class HorariosCuidadorController : ControllerBase
	{
		
		private readonly DbAae280CuidadorContext _context;
		
		public HorariosCuidadorController(DbAae280CuidadorContext context)
		{
			_context = context;
		}
		
		
		[HttpGet("{idUsuario}")]
		public async Task<ActionResult<SalarioCuidador>> GetHorariosCuidador(int idUsuario)
		{
			var horariosCuidador = await _context.SalarioCuidadors
				.Where(h => h.Usuarioid == idUsuario)
				.ToListAsync();
			
			if(horariosCuidador == null || !horariosCuidador.Any())
			{
				return BadRequest();
			}
			
			return Ok(horariosCuidador);
		}
		
	}
	
}