using Cuidador.Models;

namespace Cuidador.Dto.User.ListarDatoUsuario
{
    public class DashboardDTO
    {
        public fechasConContratos fechasConContratos { get; set; }
        public horasPorMes horasPorMes { get; set; }
        public ContratoItem contratoEnCurso { get; set; }
    }

    public class fechasConContratos
    {
        public DateTime? horarioInicioPropuesto { get; set; }
        public DateTime? horarioFinPropuesto { get; set; }
        public string? nombreCliente { get; set; }
    }

    public class horasPorMes
    {
        public string mes { get; set; }
        public int horas { get; set; }
    }

}
