using Cuidador.Models;

namespace Cuidador.Dto.Contract.ListarContrato
{
    public class OUTListarContrato
    {
        public int id_contrato { get; set; }
        public PersonaFisica persona_cuidador { get; set; }
        public PersonaFisica persona_cliente { get; set; }
        public Estatus estatus { get; set; }
        public int numeroContratos { get; set; }
        public int numero_de_tareas { get; set; }
        public decimal importe_cuidado { get; set; }
        public DateTime horario_inicio { get; set; }
        public DateTime horario_fin { get; set; }   

    }
}
