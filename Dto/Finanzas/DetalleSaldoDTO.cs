using Cuidador.Models;

namespace Cuidador.Dto.Finanzas
{
    public class DetalleSaldoDTO
    {
        public int saldoId { get; set; }
        public decimal saldoActual { get; set; }
        public decimal saldoRetirado { get; set; }
        public SalarioCuidador salarioCuidador { get; set; }
        public CuentaBancarium cuentaBancaria { get; set; }
        public List<MovimientoCuentum> movimientoCuenta { get; set; }
    }
}
