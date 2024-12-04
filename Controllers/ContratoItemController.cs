using Cuidador.Dto.Contract.DetalleVistaCliente;
using Cuidador.Dto.Contract.ListarContrato;
using Cuidador.Dto.Contract.RegistrarDatoContrato;
using Cuidador.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cuidador.Dto;
using Cuidador.Dto.Contract.FechasCuidador;
using System.Data;
using Dapper;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Cuidador.Data;
using System.IO.Compression;

namespace Cuidador.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ContratoItemController : ControllerBase
	{
		private readonly DbAae280CuidadorContext _baseDatos;
		private readonly DataContext _connection;

		public ContratoItemController(DbAae280CuidadorContext baseDatos, DataContext dataContext)
		{
			this._baseDatos = baseDatos;
			this._connection = dataContext;
		}

		[HttpGet]
		[Route("fechasOcupadasCuidador/{idPersona}")]
		public async Task<IActionResult> FechasAOcupadarCuidador(int idPersona)
		{
			var listaContratos = await _baseDatos.Contratos.Where(c => c.PersonaidCuidador == idPersona).ToListAsync();
			var listaFechas = new List<OUTFechasOcupadasCuidador>();
			
			if (listaContratos != null)
			{
				foreach (var contrato in listaContratos)
				{
					var contratoItm = await _baseDatos.ContratoItems.Where(c => c.ContratoId == contrato.IdContrato).ToListAsync();
					var fechaActual = DateTime.Now;

					var filtroContrato = contratoItm.Where(c => (c.EstatusId == 7 || c.EstatusId == 18 || c.EstatusId == 19)
									&& c.HorarioInicioPropuesto.HasValue
									&& c.HorarioInicioPropuesto.Value >= fechaActual).ToList();

					if (filtroContrato == null)
					{
						return BadRequest(new { error = "sin valores actuales para mostrar" });
					}
					else
					{
						// Procesar los contratos filtrados
						foreach (var i in filtroContrato)
						{
							var aux = new OUTFechasOcupadasCuidador
							{
								horarioInicioPropuesto = i.HorarioInicioPropuesto,
								horarioFinPropuesto = i.HorarioFinPropuesto
							};

							listaFechas.Add(aux);
						}
					}

					
				}
				return Ok(listaFechas);
			}

			return BadRequest(new { error = "no se encontro los contratos"});
		}

		[HttpGet("detalleVistaCliente/{idContrato}")]
		public async Task<IActionResult> DetalleVistaCliente(int idContrato)
		{
			try
			{
				ContratoItem contratoitm = new ContratoItem();
				// Obtener el contrato
				contratoitm = await _baseDatos.ContratoItems.Where(c => c.IdContratoItem == idContrato).Include(c => c.Contrato).FirstOrDefaultAsync();
				if (contratoitm == null)
				{
					return BadRequest("Contrato no encontrado.");
				}

				// Obtener la información del cuidador
				var listPersonaCuidador = await _baseDatos.PersonaFisicas
					.Where(p => p.IdPersona == contratoitm.Contrato.PersonaidCuidador)
					.ToListAsync();

				// Obtener en un solo lote domicilios, certificaciones y comentarios de los cuidadores
				var cuidadorIds = listPersonaCuidador.Select(p => p.IdPersona).ToList();
				var domicilios = await _baseDatos.Domicilios
					.Where(d => listPersonaCuidador.Select(p => p.DomicilioId).Contains(d.IdDomicilio))
					.ToListAsync();

				var certificacionesExperiencia = await _baseDatos.CertificacionesExperiencia
					.Where(ce => cuidadorIds.Contains(ce.PersonaId))
					.ToListAsync();

				var comentariosUsuarios = await _baseDatos.ComentariosUsuarios
					.Where(cm => cuidadorIds.Contains(cm.PersonaReceptorid))
					.ToListAsync();

				// Crear la lista de cuidadores
				var outListCuidador = listPersonaCuidador.Select(personaCuidador => new PersonaFisica
				{
					IdPersona = personaCuidador.IdPersona,
					Nombre = personaCuidador.Nombre,
					ApellidoPaterno = personaCuidador.ApellidoPaterno,
					ApellidoMaterno = personaCuidador.ApellidoMaterno,
					CorreoElectronico = personaCuidador.CorreoElectronico,
					FechaNacimiento = personaCuidador.FechaNacimiento,
					Genero = personaCuidador.Genero,
					EstadoCivil = personaCuidador.EstadoCivil,
					Domicilio = domicilios.SingleOrDefault(d => d.IdDomicilio == personaCuidador.DomicilioId),
					AvatarImage = personaCuidador.AvatarImage,
					CertificacionesExperiencia = certificacionesExperiencia
						.Where(ce => ce.PersonaId == personaCuidador.IdPersona)
						.ToList(),
					ComentariosUsuarioPersonaReceptors = comentariosUsuarios
						.Where(cm => cm.PersonaReceptorid == personaCuidador.IdPersona)
						.ToList()
				}).ToList();

				//var contratoItemIds = contrato.Select(ci => ci.IdContratoitem).ToList();
				var tareasContratos = await _baseDatos.TareasContratos
					.Where(t => t.ContratoitemId == contratoitm.IdContratoItem)
					.ToListAsync();

				var estatusList = await _baseDatos.Estatuses
					.Where(e => e.IdEstatus == contratoitm.EstatusId)
					.ToListAsync();

				List<ContratoItem> outContratoitm = new List<ContratoItem>();
				// Crear la lista de contrato items
				var outcont = new ContratoItem
				{
					IdContratoItem = contratoitm.IdContratoItem,
					Estatus = estatusList.SingleOrDefault(e => e.IdEstatus == contratoitm.EstatusId),
					Observaciones = contratoitm.Observaciones,
					HorarioInicioPropuesto = contratoitm.HorarioInicioPropuesto,
					HorarioFinPropuesto = contratoitm.HorarioFinPropuesto,
					FechaAceptacion = contratoitm.FechaAceptacion,
					FechaInicioCuidado = contratoitm.FechaInicioCuidado,
					FechaFinCuidado = contratoitm.FechaFinCuidado,
					ImporteTotal = contratoitm.ImporteTotal,
					TareasContratos = tareasContratos
						.Where(t => t.ContratoitemId == contratoitm.IdContratoItem)
						.ToList()
				};

				outContratoitm.Add(outcont);

				// Preparar la salida final
				var cont = new OUTContratoDetalle
				{
					idContrato = contratoitm.Contrato.IdContrato,
					personaCuidador = outListCuidador,
					contratoItem = outContratoitm
				};

				return Ok(cont);
			}
			catch (Exception ex)
			{
				// Log the exception (ex) here using your logging framework of choice
				return StatusCode(500, $"Ocurrió un error al procesar la solicitud: {ex.Message}");
			}
		}



		[HttpGet]
		[Route("listarContrato/{idUsuario}/{tipousuarioid}")]
		public async Task<IActionResult> ListarContrato(int idUsuario, int tipousuarioid)
		{
			var personaUsuario = await _baseDatos.PersonaFisicas.Where(p => p.UsuarioId == idUsuario).ToListAsync();

			List<int> personaIds = personaUsuario.Select(p => p.IdPersona).ToList();

			List<Contrato> contratos;
			if (tipousuarioid == 1) // 1 = cuidador | 2 = cliente
			{
				contratos = await _baseDatos.Contratos
					.Where(c => personaIds.Contains(c.PersonaidCuidador))
					.ToListAsync();
			}
			else 
			{
				contratos = await _baseDatos.Contratos
					.Where(c => personaIds.Contains(c.PersonaidCliente))
					.ToListAsync();
			}

			var contratoIds = contratos.Select(c => c.IdContrato).ToList();

			var contratoItems = await _baseDatos.ContratoItems
				.Where(ci => contratoIds.Contains(ci.ContratoId))
				.ToListAsync();

			var tareasContratos = await _baseDatos.TareasContratos
				.Where(tc => contratoItems.Select(ci => ci.IdContratoItem).Contains(tc.ContratoitemId))
				.ToListAsync();

			var estatusIds = contratoItems.Select(ci => ci.EstatusId).Distinct().ToList();
			var estatusList = await _baseDatos.Estatuses
				.Where(e => estatusIds.Contains(e.IdEstatus))
				.ToListAsync();

			var cuidadorIds = contratos.Select(c => c.PersonaidCuidador).Distinct().ToList();
			var clienteIds = contratos.Select(c => c.PersonaidCliente).Distinct().ToList();

			var cuidadores = await _baseDatos.PersonaFisicas
				.Where(p => cuidadorIds.Contains(p.IdPersona))
				.ToListAsync();

			var clientes = await _baseDatos.PersonaFisicas
				.Where(p => clienteIds.Contains(p.IdPersona))
				.ToListAsync();

			var outListaContrato = new List<OUTListarContrato>();

			// Mapear los contratos con sus items y detalles
			foreach (var contrato in contratos)
			{
				var persona_cuidador = cuidadores.SingleOrDefault(p => p.IdPersona == contrato.PersonaidCuidador);
				var persona_cliente = clientes.SingleOrDefault(p => p.IdPersona == contrato.PersonaidCliente);

				var contratoItemsFiltrados = contratoItems.Where(ci => ci.ContratoId == contrato.IdContrato).ToList();

				foreach (var i in contratoItemsFiltrados)
				{
					var estatus = estatusList.SingleOrDefault(e => e.IdEstatus == i.EstatusId);
					var numero_de_tareas = tareasContratos.Count(tc => tc.ContratoitemId == i.IdContratoItem);

					var obj = new OUTListarContrato
					{
						idContrato = contrato.IdContrato,
						horarioInicio = i.HorarioInicioPropuesto ?? DateTime.MinValue,
						horarioFin = i.HorarioFinPropuesto ?? DateTime.MinValue,
						estatus = estatus,
						personaCuidador = persona_cuidador,
						personaCliente = persona_cliente,
						importeCuidado = i.ImporteTotal ?? 0,
						numeroTarea = numero_de_tareas,
						idContratoItem = i.IdContratoItem
					};
					outListaContrato.Add(obj);
				}
			}

			return Ok(outListaContrato);
		}

		[HttpGet]
		[Route("listarContratoByPersonId/{idPersona}/{tipousuarioid}")]
		public async Task<IActionResult> ListarContratoByPersonId(int idPersona, int tipousuarioid)
		{
			try
			{
				List<OUTListarContrato> contratos = new List<OUTListarContrato>();
				string condition = "personaid_cuidador = @idPersona";
				if (tipousuarioid == 2)
				{
					condition = "personaid_cliente = @idPersona";
				}
				
				string selectContratoItems = @"
					SELECT 
					cuidador.id_persona as idCuidador,
					cliente.id_persona as idCliente,
					citm.estatus_id as estatusidContratoItem,
					ROW_NUMBER() OVER (ORDER BY citm.id_contratoitem) as numeroContrato,
					task.numeroTareas as numeroTarea,
					importe_total as importeCuidado,
					horario_inicio_propuesto as horarioInicio,
					horario_fin_propuesto as horarioFin,
					citm.id_contratoitem as idContratoItem
				FROM contrato_item citm
					INNER JOIN contrato on id_contrato = contrato_id
					INNER JOIN persona_fisica as cuidador on personaid_cuidador = cuidador.id_persona
					INNER JOIN persona_fisica as cliente on personaid_cliente = cliente.id_persona
					INNER JOIN (
						SELECT contratoitem_id, COUNT(*) numeroTareas FROM tareas_contrato GROUP BY contratoitem_id
					) task on citm.id_contratoitem = task.contratoitem_id
				" + $"WHERE horario_inicio_propuesto >= GETDATE() AND {condition}";
				
				DynamicParameters dynamicParameters = new DynamicParameters();
				dynamicParameters.Add("@idPersona", idPersona);
				
				var contratoItems = new List<dynamic>();
				using (var connection = _connection.createConnection())
				{
					contratoItems = (await connection.QueryAsync<dynamic>(selectContratoItems, dynamicParameters)).ToList();
				}
				
				List<OUTListarContrato> tempContratos = new List<OUTListarContrato>();
				foreach (dynamic item in contratoItems)
				{
					int idCuidador = (int)item.idCuidador;
					int idCliente = (int)item.idCliente;
					int idEstatusContratoItem = (int)item.estatusidContratoItem;
					int idContratoItem = (int)item.idContratoItem;
					
					PersonaFisica cuidador = await _baseDatos.PersonaFisicas.Where(p => p.IdPersona == idCuidador).FirstOrDefaultAsync() ?? new PersonaFisica();
					PersonaFisica cliente = await _baseDatos.PersonaFisicas.Where(p => p.IdPersona == idCliente).FirstOrDefaultAsync() ?? new PersonaFisica();
					
					Estatus estatus = await _baseDatos.Estatuses.Where(e => e.IdEstatus == idEstatusContratoItem).FirstOrDefaultAsync() ?? new Estatus();

					tempContratos.Add(new OUTListarContrato
					{
						idContrato = item.numeroContrato as int? ?? 0,
						horarioInicio = item.horarioInicio,
						horarioFin = item.horarioFin,
						estatus = estatus,
						personaCuidador = cuidador,
						personaCliente = cliente,
						importeCuidado = item.importeCuidado as decimal? ?? 0,
						numeroTarea = item.numeroTarea as int? ?? 0,
						idContratoItem = idContratoItem
					});
				}
				contratos.AddRange(tempContratos);
				return Ok(contratos);
			}
			catch(Exception ex)
			{
				return BadRequest(new { error = ex.Message });
			}
		}

		[HttpPost]
		[Route("guardarContrato")]
		public async Task<IActionResult> guardarContrato([FromBody] RegisterContractDTO data )
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			using (var transaction = await _baseDatos.Database.BeginTransactionAsync())
			{
				try
				{
					// validacion de fechas
					DateTime minSqlDate = new DateTime(1753, 1, 1);
					DateTime maxSqlDate = new DateTime(9999, 12, 31);
					// hay más campos para registrar
					var contrato = new Contrato
					{
						PersonaidCuidador = data.personaCuidadorId,
						PersonaidCliente = data.personaClienteId,
						EstatusId = 18,
						FechaRegistro = DateTime.Now,
						FechaModifico = null
					};

					_baseDatos.Contratos.Add(contrato);
					await _baseDatos.SaveChangesAsync();

					SalarioCuidador salario_coidador = new SalarioCuidador();
					decimal minutos = 0;
					foreach (var contratoitemData in data.ContratoItem)
					{
						// Asegúrate de que 'diferencia' no sea nulo antes de intentar acceder a sus propiedades
						TimeSpan? diferencia = (contratoitemData.horarioFinPropuesto - contratoitemData.horarioInicioPropuesto);

						if (diferencia.HasValue)
						{
							// Calcula los minutos sólo si 'diferencia' tiene un valor
							minutos = diferencia.Value.Days * 24 * 60 + diferencia.Value.Hours * 60 + diferencia.Value.Minutes;
							var persona_cuidador = await _baseDatos.PersonaFisicas.FirstOrDefaultAsync(p => p.IdPersona == data.personaCuidadorId) ?? new PersonaFisica();
							
							var culture = new System.Globalization.CultureInfo("es-ES");
							string currentDay = DateTime.Now.ToString("dddd", culture).ToUpper();
							
							salario_coidador = await _baseDatos.SalarioCuidadors.FirstOrDefaultAsync(
								s => s.Usuarioid == persona_cuidador.UsuarioId && s.DiaSemana == currentDay
							) ?? new SalarioCuidador();
							
							var contratoitem = new ContratoItem
							{
								ContratoId = contrato.IdContrato, // id que se establecerá después de SaveChangesAsync
								EstatusId = 18,
								Observaciones = contratoitemData.observacion,
								HorarioInicioPropuesto = contratoitemData.horarioInicioPropuesto,
								HorarioFinPropuesto = contratoitemData.horarioFinPropuesto,
								ImporteTotal = Math.Round((decimal)(minutos * (salario_coidador.PrecioPorHora / 60)), 2),
								// Asignando null a las fechas anulables
								FechaAceptacion = null,         // Esto es para DateTime? FechaAceptacion
								FechaInicioCuidado = null,      // Esto es para DateTime? FechaInicioCuidado
								FechaFinCuidado = null
							};
							
							_baseDatos.Add(contratoitem);
							await _baseDatos.SaveChangesAsync();

							if (contratoitemData.tareaContrato != null)
							{
								foreach (var tareas_contratoData in contratoitemData.tareaContrato)
								{
									if (tareas_contratoData.fechaARealizar.Value >= minSqlDate && tareas_contratoData.fechaARealizar.Value <= maxSqlDate)
									{
										var tareaContrato = new TareasContrato
										{
											ContratoitemId = contratoitem.IdContratoItem, // id que se establecerá después de SaveChangesAsync
											TituloTarea = tareas_contratoData.tituloTarea,
											DescripcionTarea = tareas_contratoData.descripcionTarea,
											TipoTarea = tareas_contratoData.tipoTarea,
											EstatusId = 7,
											FechaARealizar = tareas_contratoData?.fechaARealizar,
										};
										_baseDatos.Add(tareaContrato);
										await _baseDatos.SaveChangesAsync();
									}
								}
							}
						}
					}

					var persona = await _baseDatos.PersonaFisicas.Where(p => p.IdPersona == data.personaClienteId).SingleOrDefaultAsync();

					var importe = Math.Round((decimal)(minutos * (salario_coidador.PrecioPorHora / 60)), 2);
					Saldo eSaldo = await _baseDatos.Saldos
								.Where(s => s.UsuarioId == persona.UsuarioId)
								.FirstOrDefaultAsync() ?? new Saldo();

					TransaccionesSaldo mov = new TransaccionesSaldo
					{
						SaldoId = eSaldo.IdSaldo,
						ConceptoTransaccion = "Contratacion de cuidados",
						MetodoPagoid = 7,
						TipoMovimiento = "Cargo",
						FechaTransaccion = DateTime.Now,
						ImporteTransaccion = contrato.ContratoItems.Sum((e) => e.ImporteTotal) ?? 0,
						SaldoActual = eSaldo.SaldoActual - (contrato.ContratoItems.Sum((e) => e.ImporteTotal) ?? 0),
						SaldoAnterior = eSaldo.SaldoActual - (contrato.ContratoItems.Sum((e) => e.ImporteTotal) ?? 0),
						FechaRegistro = DateTime.Now,
						UsuarioRegistro = eSaldo.UsuarioId
					};

					_baseDatos.TransaccionesSaldos.Add(mov);

					eSaldo.SaldoActual = eSaldo.SaldoActual- (contrato.ContratoItems.Sum((e) => e.ImporteTotal) ?? 0);
					_baseDatos.Entry(eSaldo).State = EntityState.Modified;
					
					
					// Aquí hacemos una única llamada a SaveChangesAsync, lo que asegura que todos los objetos se guarden de una vez.
					await _baseDatos.SaveChangesAsync();

					// Si todo va bien, confirmamos la transacción
					await transaction.CommitAsync();

					var res = new
					{
						successful = "Se agregó correctamente"
					};

					return Ok(res);
				}
				catch (DbUpdateException dbEx)
				{
					// Captura la excepción interna para obtener más detalles
					var innerException = dbEx.InnerException;
					return BadRequest(new { error = dbEx.Message, innerError = innerException?.Message });
				}
				catch (Exception ex)
				{
					// En caso de error, deshacemos la transacción
					await transaction.RollbackAsync();
					return BadRequest(new { error = ex.Message });
				}
			}
		}

		// codigo kev

		[HttpPost]
		[Route("cambiarEstatusContratoItem")]
		public async Task<IActionResult> CambiarEstatusContratoItem(OUTChangeEstatus change)
		{
			
			try
			{
				ContratoItem contratoItem = await _baseDatos.ContratoItems.Where(c => c.IdContratoItem == change.idContratoItem).FirstOrDefaultAsync() ?? new ContratoItem();
				Contrato contrato = await _baseDatos.Contratos.Where(c => c.IdContrato == contratoItem.ContratoId).FirstOrDefaultAsync() ?? new Contrato();
				
				DynamicParameters dynamicParameters = new DynamicParameters();
				dynamicParameters.Add("@idContratoItem", change.idContratoItem);
				dynamicParameters.Add("@idEstatus", change.idEstatus);
				
				string statusContractItemUpdate = """
					UPDATE c SET c.estatus_id = @idEstatus
					FROM contrato_item c
					WHERE id_contratoitem = @idContratoItem
				""";
				
				using (var connection = _connection.createConnection()) // Se actualiza el estatus del contrato item
				{
					var rowsAffected = await connection.ExecuteAsync(statusContractItemUpdate, dynamicParameters);
					if (rowsAffected == 0)
					{
						return BadRequest(new { error = "No se pudo actualizar el estatus del contrato item" });
					}
				}
			
				string dateChange = "UPDATE contrato_item SET fecha_fin_cuidado = GETDATE() WHERE id_contratoitem = @idContratoItem";
				
				switch(change.idEstatus)
				{
					case 7:
						dateChange = "UPDATE contrato_item SET fecha_aceptacion = GETDATE() WHERE id_contratoitem = @idContratoItem";
						break;
					case 19:
						dateChange = "UPDATE contrato_item SET fecha_inicio_cuidado = GETDATE() WHERE id_contratoitem = @idContratoItem";
						break;
				}
				
				using (var connection = _connection.createConnection()) // Se actualiza la fecha de aceptacion, inicio o fin de cuidado
				{
					var rowsAffected = await connection.ExecuteAsync(dateChange, dynamicParameters);
					if (rowsAffected == 0)
					{
						return BadRequest(new { error = "No se pudo actualizar la fecha de aceptación, inicio o fin de cuidado" });
					}
				}
				
				
				if(change.idEstatus == 8) // Operacion para regresar dinero al cliente
				{
					PersonaFisica persona = await _baseDatos.PersonaFisicas.Where(p => p.IdPersona == contrato.PersonaidCliente).FirstOrDefaultAsync() ?? new PersonaFisica();
					Saldo saldo = await _baseDatos.Saldos.Where(s => s.UsuarioId == persona.UsuarioId).FirstOrDefaultAsync() ?? new Saldo();
					
					DynamicParameters transactionParam = new DynamicParameters();
					transactionParam.Add("@saldoId", saldo.IdSaldo);
					transactionParam.Add("@importeTransaccion", contratoItem.ImporteTotal);
					transactionParam.Add("@saldoActual", saldo.SaldoActual + contratoItem.ImporteTotal);
					transactionParam.Add("@saldoAnterior", saldo.SaldoActual);
					
					string insertTransaccion = @"
						INSERT INTO transacciones_saldos (saldo_id, concepto_transaccion, metodo_pagoid, tipo_movimiento, fecha_transaccion, importe_transaccion, saldo_actual, saldo_anterior, fecha_registro, usuario_registro)
						VALUES (@saldoId, 'Cancelación de Contrato', 7, 'Abono', GETDATE(), @importeTransaccion, @saldoActual, @saldoAnterior, GETDATE(), 1)
					";
					
					string updateSaldo = "UPDATE saldos SET saldo_actual = @saldoActual WHERE id_saldo = @saldoId";
					
					using (var connection = _connection.createConnection())
					{
						int insertTransaccionSuccess = await connection.ExecuteAsync(insertTransaccion, transactionParam);
						if(insertTransaccionSuccess == 0)  return BadRequest(new { error = "No se pudo registrar la transacción" });
						int updateSaldoSuccess = await connection.ExecuteAsync(updateSaldo, transactionParam);
						if(updateSaldoSuccess == 0) return BadRequest(new { error = "No se pudo actualizar el saldo" });
					}
				}
				else if (change.idEstatus == 9) //Operación para añadir saldo al cuidador por contrato concluido
				{
					PersonaFisica persona = await _baseDatos.PersonaFisicas.Where(p => p.IdPersona == contrato.PersonaidCuidador).FirstOrDefaultAsync() ?? new PersonaFisica();
					Saldo saldo = await _baseDatos.Saldos.Where(s => s.UsuarioId == persona.UsuarioId).FirstOrDefaultAsync() ?? new Saldo();
					CuentaBancarium cuentaBancarium = await _baseDatos.CuentaBancaria.Where(c => c.UsuarioId == persona.UsuarioId).FirstOrDefaultAsync() ?? new CuentaBancarium();
					
					DynamicParameters movimientoParam = new DynamicParameters();
					movimientoParam.Add("@cuentaBancariaId", cuentaBancarium.IdCuentabancaria);
					movimientoParam.Add("@importeMovimiento", contratoItem.ImporteTotal);
					movimientoParam.Add("@saldoActual", saldo.SaldoActual + contratoItem.ImporteTotal);
					movimientoParam.Add("@saldoAnterior", saldo.SaldoActual);
					movimientoParam.Add("@saldoId", saldo.IdSaldo);
					
					string insertMovimiento = @"
						INSERT INTO movimiento_cuenta (cuentabancaria_id, concepto_movimiento, metodo_pagoid, tipo_movimiento, numeroseguimiento_banco, fecha_movimiento, importe_movimiento, saldo_actual, saldo_anterior)
						VALUES (@cuentaBancariaId, 'Pago por Contrato', 7, 'Abono', '0000000000', GETDATE(), @importeMovimiento, @saldoActual, @saldoAnterior)
					";
					
					string updateSaldo = "UPDATE saldos SET saldo_actual = @saldoActual WHERE id_saldo = @saldoId";
					
					using (var connection = _connection.createConnection())
					{
						int insertMovimientoSuccess = await connection.ExecuteAsync(insertMovimiento, movimientoParam);
						if(insertMovimientoSuccess == 0)  return BadRequest(new { error = "No se pudo registrar el movimiento" });
						int updateSaldoSuccess = await connection.ExecuteAsync(updateSaldo, movimientoParam);
						if(updateSaldoSuccess == 0) return BadRequest(new { error = "No se pudo actualizar el saldo" });
					}
				}
			}
			catch(Exception ex)
			{
				return BadRequest(new { error = ex.Message });
			}
			return Ok(new { successful = "Se actualizó correctamente" });
		}
				
		[HttpGet("verEstatusContratoItem/{idContratoItem}")]
		public async Task<IActionResult> GetEstatusContratoItem(int idContratoItem)
		{
			//se usará dapper
			string query1 = """
				SELECT horario_inicio_propuesto as horarioInicioPropuesto,
						horario_fin_propuesto as horarioFinPropuesto,
						importe_total as importeTotal,
						DATEDIFF(MINUTE, horario_inicio_propuesto, horario_fin_propuesto) tiempoContratado,
						fecha_aceptacion as fechaAceptacion,
						fecha_inicio_cuidado as fechaInicioCuidado,
						fecha_fin_cuidado as fechaFinCuidado
				FROM contrato_item
				WHERE id_contratoitem = @idContratoItem
			""";
			
			string query2 = """
				SELECT 
					titulo_tarea as tituloTarea, 
					descripcion_tarea as descripcionTarea,
					estatus_id as estatusId,
					nombre nombreEstatus, 
					CASE estatus_id 
							WHEN 7 THEN fecha_a_realizar 
							WHEN 8 THEN null
							WHEN 9 THEN fecha_finalizacion
							WHEN 18 THEN fecha_pospuesta
							ELSE fecha_inicio
					END fechaEstatusTarea
			FROM tareas_contrato
					INNER JOIN estatus on id_estatus = estatus_id
			WHERE contratoitem_id = @idContratoItem
			""";
			
			DynamicParameters dynamicParameters = new DynamicParameters();
			dynamicParameters.Add("@idContratoItem", idContratoItem);
			
			using (var connection = (_connection.createConnection()))
			{
				var contratoItem = await connection.QueryFirstOrDefaultAsync<EstatusContratoItemCliente>(query1, dynamicParameters);
				var estatusTareas = await connection.QueryAsync<EstatusTareasContratoItem>(query2, dynamicParameters);
				
				contratoItem.estatusTareas = estatusTareas.ToList();
				
				if (contratoItem == null)
				{
					return BadRequest("Contrato no encontrado");
				}
				
				contratoItem.estatusTareas = estatusTareas.ToList();
				return Ok(contratoItem);
			}
			
		}		
				
	}
}
