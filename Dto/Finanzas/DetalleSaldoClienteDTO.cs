using Cuidador.Models;

namespace Cuidador.Dto.Finanzas
{
    public class DetalleSaldoClienteDTO
    {
        public decimal saldoActual { get; set; }
        public List<MetodoPagoUsuario> metodoPagoUsuario { get; set; }
        public List<TransaccionesSaldo> transacciones { get; set; }

    }
}
