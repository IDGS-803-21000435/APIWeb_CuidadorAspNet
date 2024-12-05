using Cuidador.Dto.Finanzas;
using Cuidador.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cuidador.Controllers
{

	[Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
	[ApiController]
	public class FinanzasController : ControllerBase
	{

		private readonly DbAae280CuidadorContext _context;

		public FinanzasController(DbAae280CuidadorContext context)
		{
			_context = context;
		}

		[HttpGet("finanzasUsuarioCuidador/{idUsuario}")]
		public async Task<IActionResult> FinanzasUsuarioCuidador(int idUsuario)
		{
			DetalleSaldoDTO detalleSaldoDTO = new DetalleSaldoDTO();
			try
			{
				detalleSaldoDTO.saldoId = await _context.Saldos.Where(s => s.UsuarioId == idUsuario).Select(s => s.IdSaldo).FirstOrDefaultAsync();
				detalleSaldoDTO.saldoActual = (decimal)await _context.Saldos.Where(s => s.UsuarioId == idUsuario).Select(s => s.SaldoActual).FirstOrDefaultAsync();
				detalleSaldoDTO.saldoRetirado = await _context.CuentaBancaria
					.Join(_context.MovimientoCuenta, c => c.IdCuentabancaria, m => m.CuentabancariaId, (c, m) => new { c, m })
					.Where(cm => cm.c.UsuarioId == idUsuario && cm.m.TipoMovimiento == "Retiro")
					.OrderByDescending(cm => cm.m.IdMovimientocuenta)
					.Select(cm => cm.m.ImporteMovimiento).SumAsync();
				detalleSaldoDTO.salarioCuidador = await _context.SalarioCuidadors.Where(s => s.Usuarioid == idUsuario).FirstOrDefaultAsync() ?? new SalarioCuidador();
				detalleSaldoDTO.cuentaBancaria = await _context.CuentaBancaria.Where(c => c.UsuarioId == idUsuario).FirstOrDefaultAsync() ?? new CuentaBancarium();
				detalleSaldoDTO.movimientoCuenta = await _context.MovimientoCuenta.Where(m => m.Cuentabancaria.UsuarioId == idUsuario).ToListAsync() ?? new List<MovimientoCuentum>();

			}
			catch(Exception ex)
			{
				return BadRequest("Error Interno del Servidor");
			}
			return Ok(detalleSaldoDTO);
		}

		[HttpGet("finanzasUsuarioCliente/{idUsuario}")]
		public async Task<IActionResult> FinanzasUsuarioCliente(int idUsuario)
		{
			DetalleSaldoClienteDTO detalleSaldoDTO = new DetalleSaldoClienteDTO();
			try
			{
				detalleSaldoDTO.saldoActual = await _context.Saldos.Where(s => s.UsuarioId == idUsuario).Select(s => s.SaldoActual).FirstOrDefaultAsync() ?? 0;
				detalleSaldoDTO.metodoPagoUsuario = await _context.MetodoPagoUsuarios.Where(m => m.UsuarioId == idUsuario).ToListAsync() ?? new List<MetodoPagoUsuario>();
				detalleSaldoDTO.transaccionSaldo = await _context.Saldos
					.Join(_context.TransaccionesSaldos, c => c.IdSaldo, m => m.SaldoId, (c, m) => new { c, m })
					.Where(cm => cm.c.UsuarioId == idUsuario)
					.OrderByDescending(cm => cm.m.FechaTransaccion)
					.Select(cm => cm.m).ToListAsync() ?? new List<TransaccionesSaldo>();
			}
			catch(Exception ex)
			{
				return BadRequest("Error Interno del Servidor");
			}
			return Ok(detalleSaldoDTO);
		}

		[HttpPost("añadirTarjeta")]
		public async Task<IActionResult> AñadirTarjeta(AñadirTarjeta add)
		{
			try
			{
				MetodoPagoUsuario metodoPagoUsuario = new MetodoPagoUsuario();

				metodoPagoUsuario.NumeroTarjeta = add.NumeroTarjeta;
				metodoPagoUsuario.FechaVencimiento = add.FechaVencimiento;
				metodoPagoUsuario.Ccv = add.Cvv;
				metodoPagoUsuario.NombreBeneficiario = add.NombreTitular;
				metodoPagoUsuario.UsuarioId = add.UsuarioId;
				metodoPagoUsuario.FechaRegistro = DateTime.Now;
				metodoPagoUsuario.UsuarioRegistro = add.UsuarioId;
				metodoPagoUsuario.VecesUsada = 0;
				metodoPagoUsuario.UsuarioModifico = null;
				metodoPagoUsuario.FechaModifico = null;

				_context.MetodoPagoUsuarios.Add(metodoPagoUsuario);
				await _context.SaveChangesAsync();

				if(metodoPagoUsuario.IdMetodousuario == 0)
				{
					return BadRequest("Error al añadir la tarjeta");
				}
			}
			catch(Exception ex)
			{
				return BadRequest("Error Interno del Servidor");
			}
			return Ok();
		}
		
		[HttpPost("registrarCuentaBancaria")]
		public async Task<IActionResult> RegistrarCuentaBancaria (RegistrarCuentaBancaria cuenta)
		{
			try
			{
				CuentaBancarium cuentaBancarium = new CuentaBancarium
				{
					UsuarioId = cuenta.usuarioid,
					NumeroCuenta = cuenta.numeroCuenta,
					ClabeInterbancaria = cuenta.clabeInterbancaria,
					Nombrebanco = cuenta.nombreBanco,
					FechaRegistro = DateTime.Now,
					UsuarioRegistro = cuenta.usuarioid
				};
				
				_context.CuentaBancaria.Add(cuentaBancarium);
				await _context.SaveChangesAsync();

				
			} catch(Exception e)
			{
				return BadRequest("Error al registrar cuenta bancaria:" + e);
			}
			return Ok();
		}
		

		[HttpPost("recargarSaldo")]
		public async Task<IActionResult> RecargarSaldo(RecargarSaldo recargar)
		{
			using(var transaction = await _context.Database.BeginTransactionAsync())
			{
				try
				{
					Saldo eSaldo = await _context.Saldos
						.Where(s => s.UsuarioId == recargar.idUsuario)
						.FirstOrDefaultAsync() ?? new Saldo();

					eSaldo.SaldoActual += recargar.importe;
					_context.Entry(eSaldo).State = EntityState.Modified;
					await _context.SaveChangesAsync();

					TransaccionesSaldo mov = new TransaccionesSaldo
					{
						SaldoId = eSaldo.IdSaldo,
						ConceptoTransaccion = "Abono De Saldo Desde App",
						MetodoPagoid = recargar.idMetodoPago,
						TipoMovimiento = "Abono",
						FechaTransaccion = DateTime.Now,
						ImporteTransaccion = recargar.importe,
						SaldoActual = eSaldo.SaldoActual,
						SaldoAnterior = eSaldo.SaldoActual - recargar.importe,
						FechaRegistro = DateTime.Now,
						UsuarioRegistro = eSaldo.UsuarioId
					};

					_context.TransaccionesSaldos.Add(mov);
					await _context.SaveChangesAsync();

					await transaction.CommitAsync();

					return Ok("Saldo recargado exitosamente");
				}
				catch(Exception ex)
				{
					await transaction.RollbackAsync();
					return BadRequest("Error Interno del Servidor " + ex);
				}
			}
		}

		[HttpPost("retirarSaldo")]
		public async Task<IActionResult> RetirarSaldo(RetirarSaldo retirar) 
		{
			using(var transaction = await _context.Database.BeginTransactionAsync())
			{
				try
				{
					Saldo eSaldo = await _context.Saldos
						.Where(s => s.UsuarioId == retirar.usuarioId)
						.FirstOrDefaultAsync() ?? new Saldo();

					if(eSaldo.SaldoActual < retirar.importe)
					{
						return BadRequest("Saldo Insuficiente");
					}

					eSaldo.SaldoActual -= retirar.importe;
					_context.Entry(eSaldo).State = EntityState.Modified;
					await _context.SaveChangesAsync();

					MovimientoCuentum mov = new MovimientoCuentum
					{
						CuentabancariaId = retirar.idCuentaBancaria,
						ConceptoMovimiento = "Retiro de Saldo",
						MetodoPagoid = 6,
						TipoMovimiento = "Retiro",
						NumeroseguimientoBanco = Decimal.Parse(DateTime.Now.ToString("yyyyMMddHHmmssffff")),
						FechaMovimiento = DateTime.Now,
						ImporteMovimiento = retirar.importe,
						SaldoActual = eSaldo.SaldoActual ??0,
						SaldoAnterior = eSaldo.SaldoActual + retirar.importe ?? 0
					};

					_context.MovimientoCuenta.Add(mov);
					await _context.SaveChangesAsync();

					await transaction.CommitAsync();

					return Ok("Saldo retirado exitosamente");
				}
				catch(Exception)
				{
					await transaction.RollbackAsync();
					return BadRequest("Error Interno del Servidor");
				}
			}
		}

		[HttpPost("modificarCuentaBancaria")]
		public async Task<IActionResult> ModificarCuentaBancaria(ModifyCuentaBancaria m)
		{
			try
			{
				CuentaBancarium cuenta = await _context.CuentaBancaria.Where(c => c.IdCuentabancaria == m.idCuentaBancaria).FirstOrDefaultAsync() ?? new CuentaBancarium();
				cuenta.NumeroCuenta = Decimal.Parse(m.numeroCuenta);
				cuenta.ClabeInterbancaria = m.clabeInterbancaria;
				cuenta.Nombrebanco = m.nombreBanco;

				_context.Entry(cuenta).State = EntityState.Modified;
				await _context.SaveChangesAsync();

				return Ok("Cuenta Modificada Con Exito!");
			}
			catch(Exception ex)
			{
				return BadRequest("Error Interno del Servidor");
			}
		}

		[HttpGet("salarioCuidador/{idUsuario}")]
		public async Task<IActionResult> SalarioCuidador(int idUsuario)
		{
			SalarioCuidador salario = await _context.SalarioCuidadors.Where(s => s.Usuarioid == idUsuario).FirstOrDefaultAsync() ?? new SalarioCuidador();
			return Ok(salario);
		}


	}

	public class AñadirTarjeta
	{
		public string NumeroTarjeta { get; set; }
		public DateOnly FechaVencimiento { get; set; }
		public string Cvv { get; set; }
		public string NombreTitular { get; set; }
		public int UsuarioId { get; set; }
	}

	public class RecargarSaldo
	{
		public int idUsuario { get; set; }
		public int idMetodoPago { get; set; }
		public decimal importe { get; set; }
	}

	public class RetirarSaldo
	{
		public int idCuentaBancaria { get; set; }
		public decimal importe { get; set; }
		public int idSaldo { get; set;}
		public int usuarioId { get; set; }
	}

	public class ModifyCuentaBancaria
	{
		public int idCuentaBancaria { get; set; }
		public string numeroCuenta { get; set; }
		public decimal clabeInterbancaria { get; set; }
		public string nombreBanco { get; set; }
	}
	
	public class RegistrarCuentaBancaria
	{
		public int usuarioid {get; set;}
		public decimal numeroCuenta {get; set;}
		public decimal clabeInterbancaria {get; set;}
		public string nombreBanco {get; set;}
	}


}
