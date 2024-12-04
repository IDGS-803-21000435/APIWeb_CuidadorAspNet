using Cuidador.Dto.MyProfile;
using Cuidador.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cuidador.Controllers
{
	
	[Route("api/[controller]")]
	[ApiController]
	public class MyProfileController : ControllerBase
	{
		
		private readonly DbAae280CuidadorContext _baseDatos;
		
		public MyProfileController(DbAae280CuidadorContext baseDatos)
		{
			this._baseDatos = baseDatos;
		}
		
		[HttpGet]
		[Route("getProfile/{idPersona}")]
		public async Task<IActionResult> getProfile(int idPersona)
		{
			PersonaFisica personaActual = new PersonaFisica();
			
			personaActual = await _baseDatos.PersonaFisicas.Where(p => p.IdPersona == idPersona)
				.FirstOrDefaultAsync() ?? new PersonaFisica();
			
			if(personaActual == null) return BadRequest(new { res = "Persona no encontrada" });
			
			Usuario usuario = await _baseDatos.Usuarios
				.Include(u => u.Usuarionivel)
				.Include(un => un.TipoUsuario)
				.Where(u => u.IdUsuario == personaActual.UsuarioId)
				.SingleOrDefaultAsync() ?? new Usuario();
				
			// usuario.PersonaFisicas.Add(personaActual);
			
			return Ok(new 
			{
				idUsuario = usuario.IdUsuario,
				tipoUsuario = usuario.TipoUsuario.NombreTipo,
				usuarioNivel = usuario.Usuarionivel.NombreNivel,
				usuario1  = usuario.Usuario1,
				contrasenia = usuario.Contrasenia,
				personaFisica = usuario.PersonaFisicas.ToList()
			});
		}
		
		[HttpGet]
		[Route("getDomicilio/{idPersona}")]
		public async Task<IActionResult> getDomicilio(int idPersona)
		{
			PersonaFisica personaActual = new PersonaFisica();
			
			personaActual = await _baseDatos.PersonaFisicas.Where(p => p.IdPersona == idPersona)
				.FirstOrDefaultAsync() ?? new PersonaFisica(); 
			
			if(personaActual == null) return BadRequest(new { res = "Persona no encontrada" });
			
			Domicilio domicilio = await _baseDatos.Domicilios
				.Where(d => d.IdDomicilio == personaActual.DomicilioId)
				.SingleOrDefaultAsync() ?? new Domicilio();
				
			return Ok(domicilio);
		}
		
		[HttpGet]
		[Route("getDatosMedicos/{idPersona}")]
		public async Task<IActionResult> getDatosMedicos(int idPersona)
		{
			PersonaFisica personaActual = new PersonaFisica();
			
			personaActual = await _baseDatos.PersonaFisicas.Where(p => p.IdPersona == idPersona)
				.FirstOrDefaultAsync() ?? new PersonaFisica();
			
			if(personaActual == null) return BadRequest(new { res = "Persona no encontrada" });
			
			DatosMedico datosMedico = await _baseDatos.DatosMedicos
				.Where(dm => dm.IdDatosmedicos == personaActual.DatosMedicosid)
				.FirstOrDefaultAsync() ?? new DatosMedico();
				
				
			return Ok(datosMedico);
		}
		
		[HttpGet]
		[Route("getPadecimientos/{idPersona}")]
		public async Task<IActionResult> getPadecimientos(int idPersona)
		{
			Usuario usuario = new Usuario();
			
			var datosMedicosId = await _baseDatos.PersonaFisicas
				.Where(p => p.IdPersona == idPersona)
				.Select(p => p.DatosMedicosid)
				.FirstOrDefaultAsync();
			
			if(usuario == null) return BadRequest(new { res = "Usuario no encontrado" });
			
			List<Padecimiento> padecimientos = await _baseDatos.Padecimientos
				.Where(p => p.DatosmedicosId == datosMedicosId)
				.ToListAsync() ?? new List<Padecimiento>();
				
			return Ok(padecimientos);
		}
		
		[HttpPost]
		[Route("updateProfile")]
		public async Task<IActionResult> updateProfile(UpdateProfile profile)
		{
			PersonaFisica personaActual = await _baseDatos.PersonaFisicas
				.Where(p => p.IdPersona == profile.idPersona)
				.SingleOrDefaultAsync() ?? new PersonaFisica();
				
			if(personaActual == null) return BadRequest(new { res = "Persona no encontrada" });
			
			personaActual.TelefonoParticular = profile.telefonoParticular;
			personaActual.TelefonoMovil = profile.telefonoMovil;
			personaActual.TelefonoEmergencia = profile.telefonoEmergencia;
			personaActual.NombrecompletoFamiliar = profile.nombreCompletoFamiliar;
			personaActual.CorreoElectronico = profile.correoElectronico;
			
			await _baseDatos.SaveChangesAsync();
			
			return Ok(personaActual);
		}
		
		[HttpPost]
		[Route("updateDomicilio")]
		public async Task<IActionResult> updateDomicilio(UpdateDomicilio domicilio)
		{
			Domicilio domicilioActual = await _baseDatos.Domicilios
				.Where(d => d.IdDomicilio == domicilio.idDomicilio)
				.SingleOrDefaultAsync() ?? new Domicilio();
				
			if(domicilioActual == null) return BadRequest(new { res = "Domicilio no encontrado" });
			
			domicilioActual.Calle = domicilio.calle;
			domicilioActual.Colonia = domicilio.colonia;
			domicilioActual.NumeroExterior = domicilio.numeroExterior;
			domicilioActual.NumeroInterior = domicilio.numeroInterior;
			domicilioActual.Ciudad = domicilio.ciudad;
			domicilioActual.Estado = domicilio.estado;
			domicilioActual.Pais = domicilio.pais;
			
			
			await _baseDatos.SaveChangesAsync();
			
			return Ok(domicilioActual);
		}
			
		[HttpPost]
		[Route("updateDatosMedicos")]
		public async Task<IActionResult> updateDatosMedicos(UpdateDatosMedicos datosMedicos)
		{
			DatosMedico datosMedicosActual = await _baseDatos.DatosMedicos
				.Where(dm => dm.IdDatosmedicos == datosMedicos.idDatosMedicos)
				.SingleOrDefaultAsync() ?? new DatosMedico();
				
			if(datosMedicosActual == null) return BadRequest(new { res = "Datos medicos no encontrados" });
			
			datosMedicosActual.NombreMedicofamiliar = datosMedicos.nombreMedicoFamiliar;
			datosMedicosActual.TelefonoMedicofamiliar = datosMedicos.telefonoMedicoFamiliar;
			datosMedicosActual.AntecedentesMedicos = datosMedicos.antecedentesMedicos;
			datosMedicosActual.Alergias = datosMedicos.alergias;
			
			
			await _baseDatos.SaveChangesAsync();
			
			return Ok(datosMedicosActual);
		}
		
		[HttpPost]
		[Route("updatePadecimiento")]
		public async Task<IActionResult> updatePadecimiento(List<UpdatePadecimientos> padecimiento)
		{
			foreach(UpdatePadecimientos padecimientoActual in padecimiento)
			{
				//delete all padecimientos
				Padecimiento padecimientos = await _baseDatos.Padecimientos
					.Where(p => p.IdPadecimiento == padecimientoActual.idPadecimiento)
					.SingleOrDefaultAsync() ?? new Padecimiento();
				
				if(padecimientos == null)
				{
					return BadRequest(new { res = "Padecimientos no encontrados" });
				}
				
				padecimientos.Nombre = padecimientoActual.nombrePadecimiento;
				padecimientos.PadeceDesde = DateOnly.TryParse(padecimientoActual.padeceDesde, out var date) ? date : null;
				
				await _baseDatos.SaveChangesAsync();
				
			}
			return Ok(new { res = "Padecimientos actualizados" });
		}
			
		[HttpDelete]
		[Route("deletePadecimiento/{idPadecimiento}")]
		public async Task<IActionResult> deletePadecimiento(int idPadecimiento)
		{
			Padecimiento padecimientos = await _baseDatos.Padecimientos
				.Where(p => p.IdPadecimiento == idPadecimiento)
				.SingleOrDefaultAsync() ?? new Padecimiento();
				
			if(padecimientos == null)
			{
				return BadRequest(new { res = "Padecimientos no encontrados" });
			}
			
			_baseDatos.Padecimientos.Remove(padecimientos);
			await _baseDatos.SaveChangesAsync();
			
			return Ok(new { res = "Padecimiento eliminado" });
		}		
				
		[HttpPost]	
		[Route("addPadecimiento")]
		public async Task<IActionResult> addPadecimiento(AddPadecimiento padecimiento)
		{
			int datosMedicosId = await _baseDatos.PersonaFisicas
				.Where(p => p.IdPersona == padecimiento.idPersona)
				.Select(p => p.DatosMedicosid)
				.FirstOrDefaultAsync() ?? 0;
			
			Padecimiento padecimientos = new Padecimiento();
			
			padecimientos.Nombre = padecimiento.nombrePadecimiento;
			padecimientos.DatosmedicosId = datosMedicosId;
			padecimientos.Descripcion = "";
			padecimientos.PadeceDesde = DateOnly.TryParse(padecimiento.padeceDesde, out var date) ? date : null;
			padecimientos.FechaRegistro = DateTime.Now;
			padecimientos.UsuarioRegistro = 1;
			
			await _baseDatos.Padecimientos.AddAsync(padecimientos);
			await _baseDatos.SaveChangesAsync();
			
			return Ok(new { res = "Padecimiento agregado" });
		}	
				
		[HttpPost]	
		[Route("updatePassword")]
		public async Task<IActionResult> updatePassword(UpdatePassword password)
		{
			Usuario usuario = await _baseDatos.Usuarios
				.Where(u => u.IdUsuario == password.idUsuario)
				.SingleOrDefaultAsync() ?? new Usuario();
				
			if(usuario == null)
			{
				return BadRequest(new { res = "Usuario no encontrado" });
			}
			
			usuario.Contrasenia = password.password;
			usuario.FechaModificacion = DateTime.Now;
			usuario.UsuarioModifico = password.idUsuario;
			
			await _baseDatos.SaveChangesAsync();
			
			return Ok(new { res = "Contraseña actualizada" });
		}	
			
		[HttpPost]
		[Route("updateAvatar")]
		public async Task<IActionResult> UpdateAvatarImage (UpdateAvatar avatar)
		{
			PersonaFisica personaActual = await _baseDatos.PersonaFisicas.Where(p => p.IdPersona == avatar.idPersona).SingleOrDefaultAsync() ?? new PersonaFisica();
				
			if(personaActual == null)
			{
				return BadRequest(new { res = "Persona no encontrada" });
			}
			
			personaActual.AvatarImage = avatar.avatarImage;
			
			await _baseDatos.SaveChangesAsync();
			
			return Ok(new { res = "Avatar actualizado" });
		}
			
				
	}
	
	public class UpdateAvatar
	{
		public int idPersona { get; set; }
		public string avatarImage { get; set; }
	}
	
}