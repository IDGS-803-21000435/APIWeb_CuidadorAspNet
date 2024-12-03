using Cuidador.Data;
using Cuidador.Dto.CRM;
using Cuidador.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace Cuidador.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CuidadorCRMController : ControllerBase
	{
	
		private readonly DataContext _dapperContext;
		
		public CuidadorCRMController(DataContext context)
		{
			this._dapperContext = context;
		}
		
		[HttpGet("dashboardCRM")]
		public async Task<IActionResult> DashboardCRM()
		{
			DashboardModel dashboard = new DashboardModel();
			string userRegister = """
				WITH usuariosHoy as (
					select COUNT(*) as usuarios_registrados_hoy 
					from usuario
					WHERE CAST(fecha_registro as date) = CAST(GETDATE() as date)
				),
				usuariosAnteriores as ( 
					select COUNT(*) as usuarios_registrados_anteriormente 
					from usuario
					WHERE CAST(fecha_registro as date) <= CAST(GETDATE() -1 as date)
				)
				SELECT *
				FROM usuariosHoy, usuariosAnteriores
			""";
			
			string rechargeSald = """
				WITH saldoRecargadoHoy as (
					select SUM(saldo_actual) as saldo_recargado_hoy
					from saldos
				),
				saldoRecargadoAnterior as (
					select SUM(saldo_actual) as saldo_recargado_anterior
					from saldos
					WHERE CAST(fecha_registro as date) <= CAST(GETDATE() -1 as date)
				)
				SELECT * FROM saldoRecargadoHoy, saldoRecargadoAnterior
			""";
			
			string moneyPerContracts = """
				WITH dineroContratosGeneradoHoy as (
					SELECT SUM(ISNULL(importe_total, 0)) as dinero_generado_hoy
					FROM contrato_item
					WHERE CAST(horario_inicio_propuesto as date) = CAST(GETDATE() as date)
				),
				dineroContratosGeneradoAnterior as (
					SELECT SUM(importe_total) as dinero_generado_anterior
					FROM contrato_item
					WHERE CAST(horario_inicio_propuesto as date) <= CAST(GETDATE() -1 as date)
				)
				SELECT ISNULL(dinero_generado_hoy, 0) dinero_generado_hoy, ISNULL(dinero_generado_anterior, 0) dinero_generado_anterior 
				FROM dineroContratosGeneradoHoy, dineroContratosGeneradoAnterior
			""";
			
			string newContracts = """
				WITH contratosHoy as (
					SELECT COUNT(*) as contratos_hoy
					FROM contrato_item
					WHERE CAST(horario_inicio_propuesto as date) = CAST(GETDATE() as date)
				),
				contratosAnteriores as (
					SELECT COUNT(*) as contratos_anteriores
					FROM contrato_item
					WHERE CAST(horario_inicio_propuesto as date) <= CAST(GETDATE() -1 as date)
				)
				SELECT * FROM contratosHoy, contratosAnteriores
			""";
			
			string moneyContractsPerMonth = """
				SELECT 
				pen.mes,
				SUM(rea.importe_total) importeConcluidos,
				ISNULL(pen.importe, 0) importePendientes
			FROM contrato_item rea
				RIGHT JOIN (
					SELECT SUM(importe_total) importe, MONTH(horario_inicio_propuesto) mes
					FROM contrato_item
					WHERE estatus_id IN (7, 18, 19)
					GROUP BY MONTH(horario_inicio_propuesto)
				) pen ON mes = pen.mes
			WHERE estatus_id = (9)
			GROUP BY pen.mes, pen.importe
			""";
			
			string usersTypeClient_usersTypeCuidador = """
				WITH cte_clientes AS (
					SELECT MONTH(fecha_registro) AS mes, COUNT(*) AS clientes
					FROM usuario
					WHERE tipo_usuarioid = 1
					GROUP BY MONTH(fecha_registro)
				),
				cte_cuidadores AS (
					SELECT MONTH(fecha_registro) AS mes, COUNT(*) AS cuidadores
					FROM usuario
					WHERE tipo_usuarioid = 2
					GROUP BY MONTH(fecha_registro)
				)
				SELECT 
					ISNULL(cte_clientes.mes, cte_cuidadores.mes) AS mes,
					ISNULL(cte_clientes.clientes, 0) AS clientes,
					ISNULL(cte_cuidadores.cuidadores, 0) AS cuidadores
				FROM cte_clientes
				FULL OUTER JOIN cte_cuidadores
					ON cte_clientes.mes = cte_cuidadores.mes;
			""";
			
			string newContractCurrentMonth = """
				SELECT  
					citm.id_contratoitem AS id_contratoitem,
					CONCAT(cliente.nombre, ' ' ,cliente.apellido_paterno, ' ' ,cliente.apellido_materno) AS cliente,
					citm.importe_total AS importe_total,
					CONCAT(cuidador.nombre, ' ', cuidador.apellido_paterno, ' ',cuidador.apellido_materno) AS cuidador,
					e.nombre as estatus,
					DATEDIFF(HOUR, horario_inicio_propuesto, horario_fin_propuesto) as horasContratadas
				FROM contrato
					INNER JOIN contrato_item citm on id_contrato = contrato_id
					INNER JOIN estatus e on id_estatus = citm.estatus_id
					INNER JOIN persona_fisica cliente on cliente.id_persona = personaid_cliente
					INNER JOIN persona_fisica cuidador on cuidador.id_persona = personaid_cuidador
				WHERE MONTH(GETDATE()) = MONTH(horario_inicio_propuesto) AND YEAR(GETDATE()) = YEAR(horario_inicio_propuesto)
				ORDER BY horario_inicio_propuesto DESC
			""";
			
			using(var conn = _dapperContext.createConnection())
			{
				dashboard.userRegister = await conn.QueryFirstOrDefaultAsync<UserRegister>(userRegister) ?? new UserRegister();
				dashboard.rechargeSald = await conn.QueryFirstOrDefaultAsync<RechargeSald>(rechargeSald) ?? new RechargeSald();
				dashboard.moneyPerContracts = await conn.QueryFirstOrDefaultAsync<MoneyPerContracts>(moneyPerContracts) ?? new MoneyPerContracts();
				dashboard.newContracts = await conn.QueryFirstOrDefaultAsync<NewContracts>(newContracts) ?? new NewContracts();
				dashboard.moneyContractsPerMonth = (await conn.QueryAsync<MoneyContractsPerMonth>(moneyContractsPerMonth) ?? new List<MoneyContractsPerMonth>()).ToList();
				dashboard.usersTypeClient_Cuidador = (await conn.QueryAsync<UsersTypeClient_Cuidador>(usersTypeClient_usersTypeCuidador) ?? new List<UsersTypeClient_Cuidador>()).ToList();
				dashboard.newContractsPerMonth = (await conn.QueryAsync<NewContractsPerMonth>(newContractCurrentMonth) ?? new List<NewContractsPerMonth>()).ToList();
			}
			return dashboard.userRegister != null ? Ok(dashboard) : BadRequest("Error al obtener los datos");
		}
	
		[HttpGet("getAllUsers")]
		public async Task<IActionResult> GetAllUsers()
		{
			List<UserCRM> users = new List<UserCRM>();
			
			string usuarioSelect = """
				SELECT
				id_usuario,
				usuario,
				tipo.nombre_tipo AS tipoUsuario,
				estatusUsuario.nombre AS estatusUsuario,
				nivel_usuario.nombre_nivel AS nivelUsuario,
				u.fecha_registro,
				MAX(avatar_image) AS avatar_image -- Puedes cambiar esto según tus necesidades
			FROM usuario u
				INNER JOIN estatus estatusUsuario ON id_estatus = u.estatusid
				INNER JOIN tipo_usuario tipo ON id_tipousuario = tipo_usuarioid
				INNER JOIN nivel_usuario ON id_nivelusuario = usuarionivel_id
				INNER JOIN persona_fisica ON usuario_id = id_usuario
			GROUP BY
				id_usuario,
				usuario,
				tipo.nombre_tipo,
				estatusUsuario.nombre,
				nivel_usuario.nombre_nivel,
				u.fecha_registro;

			""";
			
			using (var conn = _dapperContext.createConnection())
			{
				users = (await conn.QueryAsync<UserCRM>(usuarioSelect)).ToList();
			}
			return users != null ? Ok(users) : BadRequest("Error al obtener los datos");
		}
		
		[HttpGet("getPersonById/{id}")]
		public async Task<IActionResult> GetPersonById(int id)
		{
			List<PersonaCRM> persona = new List<PersonaCRM>();
			
			string personaSelect = """
				SELECT 
				id_persona
				, p.nombre
				, apellido_paterno
				, apellido_materno
				, fecha_nacimiento
				, avatar_image
				, genero
				, estado_civil
				, rfc
				, curp
				, telefono_particular
				, telefono_movil
				, id_domicilio
				, calle
				, colonia
				, numero_interior
				, numero_exterior
				, ciudad
				, estado
				, pais
				, estatus.nombre as estatusPersona
				, esFamiliar
			FROM persona_fisica p
				INNER JOIN domicilio on id_domicilio = domicilio_id
				INNER JOIN estatus On id_estatus = p.estatus_id
			WHERE usuario_id = @id
			""";
			DynamicParameters parameters = new DynamicParameters();
			using(var conn = _dapperContext.createConnection())
			{
				parameters.Add("@id", id);
				persona = (await conn.QueryAsync<PersonaCRM>(personaSelect, parameters)).ToList();
			}
			return persona != null ? Ok(persona) : BadRequest("Error al obtener los datos");
		}
	
		[HttpGet("getDocumentsByPersonId/{id}")]
		public async Task<IActionResult> GetDocumentsByPersonId(int id)
		{
			List<Documentos> documentos = new List<Documentos>();
			
			string documentosSelect = """
				SELECT 
				id_documentacion as idDocumentacion
				, tipo_documento as tipoDocumento
				, nombre_documento as nombreDocumento
				, url_documento as urlDocumento
				, fecha_emision AS fechaEmision
				, fecha_expiracion AS fechaExpiracion
				, version AS version
				, nombre as estatusDocumento
			FROM documentacion
				INNER JOIN estatus on id_estatus = estatus_id
			WHERE persona_id = @id
			""";
			DynamicParameters parameters = new DynamicParameters();
			using(var conn = _dapperContext.createConnection())
			{
				parameters.Add("@id", id);
				documentos = (await conn.QueryAsync<Documentos>(documentosSelect, parameters)).ToList();
			}
			return documentos != null ? Ok(documentos) : BadRequest("Error al obtener los datos");
		}
		
		[HttpGet("getMedicalDataByPersonId/{id}")]
		public async Task<IActionResult> GetMedicalDataByPersonId(int id)
		{
			DatosMedicos datosMedicos = new DatosMedicos();
			
			string datosMedicosSelect = """
				SELECT  
					id_datosmedicos as idDatosmedicos
					, antecedentes_medicos as antecedentesMedicos
					, alergias
					, tipo_sanguineo as tipoSanguineo
					, nombre_medicofamiliar as nombreMedicofamiliar
					, telefono_medicofamiliar as telefonoMedicofamiliar
					, observaciones
				FROM datos_medicos
				WHERE id_datosmedicos = @id
			""";
			
			string padecimientosSelect = """
				SELECT 
					id_padecimiento as idPadecimiento
					, nombre
					, descripcion
					, padece_desde as padeceDesde
				FROM padecimientos
				WHERE datosmedicos_id = @id
			""";
			
			DynamicParameters parameters = new DynamicParameters();
			using(var conn = _dapperContext.createConnection())
			{
				parameters.Add("@id", id);
				datosMedicos = await conn.QueryFirstOrDefaultAsync<DatosMedicos>(datosMedicosSelect, parameters) ?? new DatosMedicos();
				datosMedicos.padecimientos = (await conn.QueryAsync<Padecimientos>(padecimientosSelect, parameters)).ToList();
				
			}
			return datosMedicos != null ? Ok(datosMedicos) : BadRequest("Error al obtener los datos");
		}	
	
		[HttpPut("blockOrEnableUser/{id}/{estatus}")]
		public async Task<IActionResult> BlockUser(int id, int estatus)
		{
			string blockUser = """
				UPDATE usuario
				SET estatusid = @estatus
				WHERE id_usuario = @id
			""";
			DynamicParameters parameters = new DynamicParameters();
			using(var conn = _dapperContext.createConnection())
			{
				parameters.Add("@id", id);
				parameters.Add("@estatus", estatus);
				await conn.ExecuteAsync(blockUser, parameters);
			}
			return Ok("Usuario bloqueado");
		}
	
		[HttpGet("getPersonaUsuario/{id}/{tipo}")]
		public async Task<IActionResult> GetPersonaUsuario(int id, int tipo)
		{
			AllUsers user = new AllUsers();
			
			string usuarioSelect = "";
			string personaSelect = "";
			string datosMedicosSelect = "";
			
			if(tipo == 1)// personaid
			{
				usuarioSelect = """
					SELECT DISTINCT
					id_usuario,
					usuario,
					contrasenia,
					tipo.nombre_tipo as tipoUsuario,
					estatusUsuario.nombre as estatusUsuario,
					nivel_usuario.nombre_nivel as nivelUsuario,
					u.fecha_registro,
					avatar_image
				FROM usuario u
					INNER JOIN estatus estatusUsuario on id_estatus = u.estatusid
					INNER JOIN tipo_usuario tipo on id_tipousuario = tipo_usuarioid
					INNER JOIN nivel_usuario on id_nivelusuario = usuarionivel_id
					INNER JOIN persona_fisica on usuario_id = id_usuario
				WHERE id_usuario = (SELECT usuario_id FROM persona_fisica WHERE id_persona = @id)
				""";
				
				personaSelect = """
					SELECT 
					id_persona
					, p.nombre
					, apellido_paterno
					, apellido_materno
					, fecha_nacimiento
					, avatar_image
					, correo_electronico
					, genero
					, estado_civil
					, rfc
					, curp
					, telefono_particular
					, telefono_movil
					, id_domicilio
					, calle
					, colonia
					, numero_interior
					, numero_exterior
					, ciudad
					, estado
					, pais
					, estatus.nombre as estatusPersona
					, esFamiliar
				FROM persona_fisica p
					INNER JOIN domicilio on id_domicilio = domicilio_id
					INNER JOIN estatus On id_estatus = p.estatus_id
				WHERE id_persona = @id
				""";
				
				datosMedicosSelect = """
					SELECT  
						id_datosmedicos as idDatosmedicos
						, antecedentes_medicos as antecedentesMedicos
						, alergias
						, tipo_sanguineo as tipoSanguineo
						, nombre_medicofamiliar as nombreMedicofamiliar
						, telefono_medicofamiliar as telefonoMedicofamiliar
						, observaciones
					FROM datos_medicos
					WHERE id_datosmedicos = (SELECT datos_medicosid FROM persona_fisica WHERE id_persona = @id)
				""";
				
			}
			else // cuidadorid
			{
				usuarioSelect = """
					SELECT DISTINCT
					id_usuario,
					usuario,
					contrasenia,
					tipo.nombre_tipo as tipoUsuario,
					estatusUsuario.nombre as estatusUsuario,
					nivel_usuario.nombre_nivel as nivelUsuario,
					u.fecha_registro,
					avatar_image
				FROM usuario u
					INNER JOIN estatus estatusUsuario on id_estatus = u.estatusid
					INNER JOIN tipo_usuario tipo on id_tipousuario = tipo_usuarioid
					INNER JOIN nivel_usuario on id_nivelusuario = usuarionivel_id
					INNER JOIN persona_fisica on usuario_id = id_usuario
				WHERE id_usuario = @id
				""";
				
				personaSelect = """
					SELECT TOP 1
					id_persona
					, p.nombre
					, apellido_paterno
					, apellido_materno
					, fecha_nacimiento
					, avatar_image
					, correo_electronico
					, genero
					, estado_civil
					, rfc
					, curp
					, telefono_particular
					, telefono_movil
					, id_domicilio
					, calle
					, colonia
					, numero_interior
					, numero_exterior
					, ciudad
					, estado
					, pais
					, estatus.nombre as estatusPersona
					, esFamiliar
				FROM persona_fisica p
					INNER JOIN domicilio on id_domicilio = domicilio_id
					INNER JOIN estatus On id_estatus = p.estatus_id
				WHERE usuario_id = @id
				""";
				
				datosMedicosSelect = """
					SELECT  
						id_datosmedicos as idDatosmedicos
						, antecedentes_medicos as antecedentesMedicos
						, alergias
						, tipo_sanguineo as tipoSanguineo
						, nombre_medicofamiliar as nombreMedicofamiliar
						, telefono_medicofamiliar as telefonoMedicofamiliar
						, observaciones
					FROM datos_medicos
					WHERE id_datosmedicos = (SELECT TOP 1 datos_medicosid FROM persona_fisica WHERE usuario_id = @id)
				""";
			}
			
			string padecimientosSelect = """
				SELECT 
					id_padecimiento as idPadecimiento
					, nombre
					, descripcion
					, padece_desde as padeceDesde
				FROM padecimientos
				WHERE datosmedicos_id = @id
			""";
			
			DynamicParameters parameters = new DynamicParameters();
			DynamicParameters datosM = new DynamicParameters();
			using(var conn = _dapperContext.createConnection())
			{
				parameters.Add("@id", id);
				user.persona = await conn.QueryFirstOrDefaultAsync<PersonaCRM>(personaSelect, parameters) ?? new PersonaCRM();
				user.user = await conn.QueryFirstOrDefaultAsync<UserCRM>(usuarioSelect, parameters) ?? new UserCRM();
				user.datosMedicos = await conn.QueryFirstOrDefaultAsync<DatosMedicos>(datosMedicosSelect, parameters) ?? new DatosMedicos();
				datosM.Add("@id", user.datosMedicos.idDatosmedicos);
				user.datosMedicos.padecimientos = (await conn.QueryAsync<Padecimientos>(padecimientosSelect, datosM)).ToList();
			}
			return user != null ? Ok(user) : BadRequest("Error al obtener los datos");
		}
		
		[HttpPost("updateUser")]
		public async Task<IActionResult> UpdateUser(AllUsers user)
		{
			string updatePersona = """
				UPDATE persona_fisica
				SET nombre = @nombre
				, apellido_paterno = @apellidoPaterno
				, apellido_materno = @apellidoMaterno
				, fecha_nacimiento = @fechaNacimiento
				, avatar_image = @avatarImage
				, correo_electronico = @correoElectronico
				, genero = @genero
				, estado_civil = @estadoCivil
				, rfc = @rfc
				, curp = @curp
				, telefono_particular = @telefonoParticular
				, telefono_movil = @telefonoMovil
				, esFamiliar = @esFamiliar
				WHERE id_persona = @idPersona
			""";
			
			string updateDomicilio = """
				UPDATE domicilio
				SET calle = @calle
				, colonia = @colonia
				, numero_interior = @numeroInterior
				, numero_exterior = @numeroExterior
				, ciudad = @ciudad
				, estado = @estado
				, pais = @pais
				WHERE id_domicilio = @idDomicilio
			""";
			
			string updateUser = """
				UPDATE usuario
				SET usuario = @usuario
				, contrasenia = @contrasenia
				WHERE id_usuario = @idUsuario
			""";
			
			string updateDatosMedicos = """
				UPDATE datos_medicos
				SET antecedentes_medicos = @antecedentesMedicos
				, alergias = @alergias
				, tipo_sanguineo = @tipoSanguineo
				, nombre_medicofamiliar = @nombreMedicofamiliar
				, telefono_medicofamiliar = @telefonoMedicofamiliar
				, observaciones = @observaciones
				WHERE id_datosmedicos = @idDatosMedicos
			""";
			
			DynamicParameters parameters = new DynamicParameters();
			try
			{
				using(var conn = _dapperContext.createConnection())
				{
					parameters.Add("@nombre", user.persona.nombre);
					parameters.Add("@apellidoPaterno", user.persona.apellido_paterno);
					parameters.Add("@apellidoMaterno", user.persona.apellido_materno);
					parameters.Add("@fechaNacimiento", user.persona.fecha_nacimiento);
					parameters.Add("@avatarImage", user.persona.avatar_image);
					parameters.Add("@correoElectronico", user.persona.correo_electronico);
					parameters.Add("@genero", user.persona.genero);
					parameters.Add("@estadoCivil", user.persona.estado_civil);
					parameters.Add("@rfc", user.persona.rfc);
					parameters.Add("@curp", user.persona.curp);
					parameters.Add("@telefonoParticular", user.persona.telefono_particular);
					parameters.Add("@telefonoMovil", user.persona.telefono_movil);
					parameters.Add("@esFamiliar", user.persona.esFamiliar);
					parameters.Add("@idPersona", user.persona.id_persona);
					
					await conn.ExecuteAsync(updatePersona, parameters);
					
					parameters.Add("@calle", user.persona.calle);
					parameters.Add("@colonia", user.persona.colonia);
					parameters.Add("@numeroInterior", user.persona.numero_interior);
					parameters.Add("@numeroExterior", user.persona.numero_exterior);
					parameters.Add("@ciudad", user.persona.ciudad);
					parameters.Add("@estado", user.persona.estado);
					parameters.Add("@pais", user.persona.pais);
					parameters.Add("@idDomicilio", user.persona.id_domicilio);
					
					await conn.ExecuteAsync(updateDomicilio, parameters);
					
					parameters.Add("@usuario", user.user.usuario);
					parameters.Add("@contrasenia", user.user.contrasenia);
					parameters.Add("@idUsuario", user.user.id_usuario);
					
					await conn.ExecuteAsync(updateUser, parameters);
					
					parameters.Add("@antecedentesMedicos", user.datosMedicos.antecedentesMedicos);
					parameters.Add("@alergias", user.datosMedicos.alergias);
					parameters.Add("@tipoSanguineo", user.datosMedicos.tipoSanguineo);
					parameters.Add("@nombreMedicofamiliar", user.datosMedicos.nombreMedicofamiliar);
					parameters.Add("@telefonoMedicofamiliar", user.datosMedicos.telefonoMedicofamiliar);
					parameters.Add("@observaciones", user.datosMedicos.observaciones);
					parameters.Add("@idDatosMedicos", user.datosMedicos.idDatosmedicos);
					
					await conn.ExecuteAsync(updateDatosMedicos, parameters);
				}
			}
			catch(Exception ex)
			{
				return BadRequest(ex.Message);
			}
			return Ok("Usuario actualizado");
		}
	
		[HttpGet("getOrganizations")]
		public async Task<IActionResult> GetOrganizations()
		{
			List<Organizations> organizations = new List<Organizations>();
			
			string organizationsSelect = """
				SELECT 
				id_personamoral as id_personamoral
				, razon_social as razon_social
				, nombre_comercial as nombre_comercial
				, rfc as rfc
				, telefono as telefono
				, correo_electronico as correo_electronico
				, id_domicilio as id_domicilio
				, calle as calle
				, colonia as colonia
				, numero_interior as numero_interior
				, numero_exterior as numero_exterior
				, ciudad as ciudad
				, estado as estado
				, pais as pais
				, referencias as referenciasDomicilio
			FROM persona_moral
				INNER JOIN domicilio on id_domicilio = direccion_id
			""";
			
			using(var conn = _dapperContext.createConnection())
			{
				organizations = (await conn.QueryAsync<Organizations>(organizationsSelect)).ToList();
			}
			return organizations != null ? Ok(organizations) : BadRequest("Error al obtener los datos");
		}
	
		[HttpPost("newOrganization")]
		public async Task<IActionResult> NewOrganization(Organizations organization)
		{
			string insertOrganization = """
				INSERT INTO persona_moral
				(razon_social, nombre_comercial, rfc, telefono, correo_electronico, direccion_id, fecha_registro, usuario_registro)
				VALUES
				(@razonSocial, @nombreComercial, @rfc, @telefono, @correoElectronico, @idDomicilio, GETDATE(), 1)
			""";
			
			string insertDomicilio = """
				INSERT INTO domicilio
				(calle, colonia, numero_interior, numero_exterior, ciudad, estado, pais, referencias, estatus_id, fecha_registro, usuario_registro)
				OUTPUT INSERTED.id_domicilio
				VALUES
				(@calle, @colonia, @numeroInterior, @numeroExterior, @ciudad, @estado, @pais, @referencias, 10, GETDATE(), 1)
			""";
			
			DynamicParameters parameters = new DynamicParameters();
			try
			{
				using(var conn = _dapperContext.createConnection())
				{
					parameters.Add("@calle", organization.calle);
					parameters.Add("@colonia", organization.colonia);
					parameters.Add("@numeroInterior", organization.numero_interior);
					parameters.Add("@numeroExterior", organization.numero_exterior);
					parameters.Add("@ciudad", organization.ciudad);
					parameters.Add("@estado", organization.estado);
					parameters.Add("@pais", organization.pais);
					parameters.Add("@referencias", organization.referenciasDomicilio);
					
					var domicilioId = await conn.QuerySingleAsync<int>(insertDomicilio, parameters);
					parameters.Add("@idDomicilio", domicilioId);
					
					parameters.Add("@razonSocial", organization.razon_social);
					parameters.Add("@nombreComercial", organization.nombre_comercial);
					parameters.Add("@rfc", organization.rfc);
					parameters.Add("@telefono", organization.telefono);
					parameters.Add("@correoElectronico", organization.correo_electronico);
					
					await conn.ExecuteAsync(insertOrganization, parameters);
					
				}
			}
			catch(Exception ex)
			{
				return BadRequest(ex.Message);
			}
			return Ok("Organización registrada");
		}
	
		[HttpPut("updateOrganization")]
		public async Task<IActionResult> UpdateOrganization(Organizations organization)
		{
			string updateOrganization = """
				UPDATE persona_moral
				SET razon_social = @razonSocial
				, nombre_comercial = @nombreComercial
				, rfc = @rfc
				, telefono = @telefono
				, correo_electronico = @correoElectronico
				WHERE id_personamoral = @idPersonaMoral
			""";
			
			string updateDomicilio = """
				UPDATE domicilio
				SET calle = @calle
				, colonia = @colonia
				, numero_interior = @numeroInterior
				, numero_exterior = @numeroExterior
				, ciudad = @ciudad
				, estado = @estado
				, pais = @pais
				, referencia = @referencias
				WHERE id_domicilio = @idDomicilio
			""";
			
			DynamicParameters parameters = new DynamicParameters();
			try
			{
				using(var conn = _dapperContext.createConnection())
				{
					parameters.Add("@calle", organization.calle);
					parameters.Add("@colonia", organization.colonia);
					parameters.Add("@numeroInterior", organization.numero_interior);
					parameters.Add("@numeroExterior", organization.numero_exterior);
					parameters.Add("@ciudad", organization.ciudad);
					parameters.Add("@estado", organization.estado);
					parameters.Add("@pais", organization.pais);
					parameters.Add("@referencias", organization.referenciasDomicilio);
					parameters.Add("@idDomicilio", organization.id_domicilio);
					
					await conn.ExecuteAsync(updateDomicilio, parameters);
					
					parameters.Add("@razonSocial", organization.razon_social);
					parameters.Add("@nombreComercial", organization.nombre_comercial);
					parameters.Add("@rfc", organization.rfc);
					parameters.Add("@telefono", organization.telefono);
					parameters.Add("@correoElectronico", organization.correo_electronico);
					parameters.Add("@idPersonaMoral", organization.id_personamoral);
					
					await conn.ExecuteAsync(updateOrganization, parameters);
					
				}
			}
			catch(Exception ex)
			{
				return BadRequest(ex.Message);
			}
			return Ok("Organización actualizada");
		}
	
		[HttpDelete("deleteOrganization/{id}")]
		public async Task<IActionResult> DeleteOrganization(int id)
		{
			string deleteOrganization = """
				DELETE FROM persona_moral
				WHERE id_personamoral = @id
			""";
			
			DynamicParameters parameters = new DynamicParameters();
			try
			{
				using(var conn = _dapperContext.createConnection())
				{
					parameters.Add("@id", id);
					await conn.ExecuteAsync(deleteOrganization, parameters);
				}
			}
			catch(Exception ex)
			{
				return BadRequest(ex.Message);
			}
			return Ok("Organización eliminada");
		}
	
		[HttpGet("getOrganizationById/{id}")]
		public async Task<IActionResult> GetOrganizationById(int id)
		{
			Organizations organization = new Organizations();
			
			string organizationSelect = """
				SELECT 
				id_personamoral as id_personamoral
				, razon_social as razon_social
				, nombre_comercial as nombre_comercial
				, rfc as rfc
				, telefono as telefono
				, correo_electronico as correo_electronico
				, id_domicilio as id_domicilio
				, calle as calle
				, colonia as colonia
				, numero_interior as numero_interior
				, numero_exterior as numero_exterior
				, ciudad as ciudad
				, estado as estado
				, pais as pais
				, referencias as referenciasDomicilio
			FROM persona_moral
				INNER JOIN domicilio on id_domicilio = direccion_id
			WHERE id_personamoral = @id
			""";
			
			DynamicParameters parameters = new DynamicParameters();
			using(var conn = _dapperContext.createConnection())
			{
				parameters.Add("@id", id);
				organization = await conn.QueryFirstOrDefaultAsync<Organizations>(organizationSelect, parameters) ?? new Organizations();
			}
			return organization != null ? Ok(organization) : BadRequest("Error al obtener los datos");
		}

		[HttpGet("getContracts")]
		public async Task<IActionResult> GetContracts()
		{
			List<ContratoCRM> contracts = new List<ContratoCRM>();
			
			string contractsSelect = """
				SELECT 
					id_contratoitem as idContratoItem
					, horario_inicio_propuesto as horarioInicioPropuesto
					, horario_fin_propuesto as horarioFinPropuesto
					, importe_total as importeTotal
					,DATEDIFF(HOUR, horario_fin_propuesto, horario_inicio_propuesto) as horasContratadas
					, IIF(horario_inicio_propuesto < GETDATE(), 'VENCIDO', estatus.nombre) as estatusContratoItem
					, CONCAT(cuidador.nombre, ' ', cuidador.apellido_paterno, ' ', cuidador.apellido_materno) as nombreCuidador
					, CONCAT(paciente.nombre, ' ', paciente.apellido_paterno, ' ', paciente.apellido_materno) as nombreCliente
					, cuidador.avatar_image as avatarCuidador
					, paciente.avatar_image as avatarCliente
				FROM contrato
					INNER JOIN contrato_item on id_contrato = contrato_id
					INNER JOIN persona_fisica cuidador on cuidador.id_persona = personaid_cuidador
					INNER JOIN persona_fisica paciente on paciente.id_persona = personaid_cliente 
					INNER JOIN estatus on id_estatus = contrato_item.estatus_id
			""";
			
			using(var conn = _dapperContext.createConnection())
			{
				contracts = (await conn.QueryAsync<ContratoCRM>(contractsSelect)).ToList();
			}
			return contracts != null ? Ok(contracts) : BadRequest("Error al obtener los datos");
		}
	
		[HttpGet("getPendingUsers/{tipoUsuario}/{esFamiliar}/{estatusid}")]
		public async Task<IActionResult> getPendingUser(int tipoUsuario, int esFamiliar, int estatusid)
		{
			List<PendingUsers> pendingUsers = new List<PendingUsers>();
			
			string pendingUsersSelect = """
				SELECT 
					id_usuario as idUsuario,
					nombre_nivel as nivelUsuario,
					nombre_tipo as tipoUsuario,
					nombre as estatusUsuario,
					estatusid as estatusId,
					usuario,
					contrasenia
				FROM usuario
					INNER JOIN nivel_usuario on usuarionivel_id = id_nivelusuario
					INNER JOIN tipo_usuario on id_tipousuario = tipo_usuarioid
					INNER JOIN estatus on id_estatus = estatusid
				WHERE estatusid = @estatusid AND tipo_usuarioid = @tipoUsuario
			""";
			
			string pendingPersonSelect = """
				SELECT 
					nombre
					, apellido_paterno as apellidoPaterno
					, apellido_materno as apellidoMaterno
					, correo_electronico as correoElectronico
					, fecha_nacimiento as fechaNacimiento
					, genero
					, estado_civil as estadoCivil
					, rfc
					, curp
					, telefono_particular as telefonoParticular
					, telefono_emergencia as telefonoEmergencia
					, telefono_movil as telefonoMovil	
					, nombrecompleto_familiar as nombreCompletoFamiliar
					, avatar_image as avatarImage
					, esFamiliar 
				FROM persona_fisica
				WHERE usuario_id = @usuarioId AND esFamiliar = @esFamiliar
			""";
			
			DynamicParameters parameters = new DynamicParameters();
			
			using(var conn = _dapperContext.createConnection())
			{
				parameters.Add("@tipoUsuario", tipoUsuario);
				parameters.Add("@estatusid", estatusid);
			
				pendingUsers = (await conn.QueryAsync<PendingUsers>(pendingUsersSelect, parameters)).ToList();
				
				foreach(var user in pendingUsers)
				{
					parameters.Add("@usuarioId", user.idUsuario);
					parameters.Add("@esFamiliar", esFamiliar);
					user.personaFisica = (await conn.QueryAsync<PendingPerson>(pendingPersonSelect, parameters)).ToList();
				}
			}
			return pendingUsers != null ? Ok(pendingUsers) : BadRequest("Error al obtener los datos");	
		}
	
		[HttpGet("getPersonByPersonaId/{id}")]
		public async Task<IActionResult> GetPersonByPersonaId(int id)
		{
			List<PersonaCRM> persona = new List<PersonaCRM>();
			
			string personaSelect = """
				SELECT 
				id_persona
				, p.nombre
				, apellido_paterno
				, apellido_materno
				, fecha_nacimiento
				, avatar_image
				, genero
				, estado_civil
				, rfc
				, curp
				, telefono_particular
				, telefono_movil
				, id_domicilio
				, calle
				, colonia
				, numero_interior
				, numero_exterior
				, ciudad
				, estado
				, pais
				, estatus.nombre as estatusPersona
				, esFamiliar
			FROM persona_fisica p
				INNER JOIN domicilio on id_domicilio = domicilio_id
				INNER JOIN estatus On id_estatus = p.estatus_id
			WHERE id_persona = @id
			""";
			DynamicParameters parameters = new DynamicParameters();
			using(var conn = _dapperContext.createConnection())
			{
				parameters.Add("@id", id);
				persona = (await conn.QueryAsync<PersonaCRM>(personaSelect, parameters)).ToList();
			}
			return persona != null ? Ok(persona) : BadRequest("Error al obtener los datos");
		}
	
	}
	
}