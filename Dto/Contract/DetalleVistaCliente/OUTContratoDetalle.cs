using Cuidador.Models;

namespace Cuidador.Dto.Contract.DetalleVistaCliente
{
    public class OUTContratoDetalle
    {
        public int id_contrato {  get; set; }
        public List<PersonaFisica> persona_cuidador { get; set; }
        public List<ContratoItem> contrato_item { get; set; }
    }
}
