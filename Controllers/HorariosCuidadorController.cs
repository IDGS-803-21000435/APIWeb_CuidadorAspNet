using Cuidador.Dto.Horarios;
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
		
		[HttpPost("actualizarHorario")]
		public async Task<ActionResult<SalarioCuidador>> ActualizarHorario(List<UpdateHorario> horarios)
		{
			foreach(var horario in horarios)
			{
				var horarioCuidador = await _context.SalarioCuidadors
					.Where(h => h.IdSueldonivel == horario.idSueldoNivel)
					.FirstOrDefaultAsync();
				
				if(horarioCuidador == null)
				{
					return BadRequest();
				}
				
				horarioCuidador.DiaSemana = horario.diaSemana;
				horarioCuidador.HoraInicio = horario.horaInicio;
				horarioCuidador.HoraFin = horario.horaFin;
				horarioCuidador.PrecioPorHora = horario.precioPorhora;
				horarioCuidador.Estatusid = horario.estatus;
				
				_context.Entry(horarioCuidador).State = EntityState.Modified;
			}
			
			await _context.SaveChangesAsync();
			
			return Ok();
		}
		
		[HttpPost("nuevoHorario")]
		public async Task<ActionResult<SalarioCuidador>> AgregarHorario(NuevoHorario horario)
		{
			
			foreach(var horarioAdd in horario.horarios)
			{
				var horarioCuidador = new SalarioCuidador
				{
					Usuarioid = horario.idUsuario,
					DiaSemana = horarioAdd.diaSemana,
					HoraInicio = horarioAdd.horaInicio,
					HoraFin = horarioAdd.horaFin,
					PrecioPorHora = horarioAdd.precioPorhora,
					FechaRegistro = DateTime.Now,
					UsuarioRegistro = 1,
					Estatusid = 1
				};
				
				_context.SalarioCuidadors.Add(horarioCuidador);
			}
			
			await _context.SaveChangesAsync();
			
			return Ok();
		}
		
		[HttpPut("cambiarEstatusHorario/{idSueldoNivel}/{estatus}")]
		public async Task<ActionResult<SalarioCuidador>> DesactivarHorario(int idSueldoNivel, int estatus)
		{
			var horarioCuidador = await _context.SalarioCuidadors
				.Where(h => h.IdSueldonivel == idSueldoNivel)
				.FirstOrDefaultAsync();
			
			if(horarioCuidador == null)
			{
				return BadRequest();
			}
			
			horarioCuidador.Estatusid = byte.Parse(estatus.ToString());
			horarioCuidador.FechaModificacion = DateTime.Now;
			horarioCuidador.UsuarioModifico = horarioCuidador.Usuarioid;
			
			_context.Entry(horarioCuidador).State = EntityState.Modified;
			
			await _context.SaveChangesAsync();
			
			return Ok();
		}
		
	}
	
}