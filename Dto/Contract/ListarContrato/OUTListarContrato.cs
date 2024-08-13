using Cuidador.Models;

namespace Cuidador.Dto.Contract.ListarContrato
{
    public class OUTListarContrato
    {
        public int id_contrato { get; set; }
        public PersonaFisica persona_cuidador { get; set; }
        public PersonaFisica persona_cliente { get; set; }
        public Estatus estatus { get; set; }
        public int numero_de_contratos { get; set; }
        public int numero_de_tareas { get; set; }
        public DateTime? fecha_primer_contrato { get; set; } = null;
        public DateTime? fecha_ultimp_contrato { get; set; } = null;
    }
}
