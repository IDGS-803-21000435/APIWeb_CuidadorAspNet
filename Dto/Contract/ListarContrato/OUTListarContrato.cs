using Cuidador.Models;

namespace Cuidador.Dto.Contract.ListarContrato
{
    public class OUTListarContrato
    {
        public int idContrato { get; set; }
        public PersonaFisica personaCuidador { get; set; }
        public PersonaFisica personaCliente { get; set; }
        public Estatus estatus { get; set; }
        public int numeroContrato { get; set; }
        public int numeroTarea { get; set; }
        public decimal importeCuidado { get; set; }
        public DateTime horarioInicio { get; set; }
        public DateTime horarioFin { get; set; }   

    }
}
