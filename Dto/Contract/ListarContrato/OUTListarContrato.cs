using Cuidador.Models;

namespace Cuidador.Dto.Contract.ListarContrato
{
    public class OUTListarContrato
    {
        public int id_contrato {  get; set; }
        public PersonaFisica persona_cuidador { get; set; }
        public PersonaFisica persona_cliente { get; set; }
        public Estatus estatus { get; set; }
    }
}
