using Cuidador.Models;

namespace Cuidador.Dto.Contract.DetalleVistaCliente
{
    public class OUTContratoDetalle
    {
        public int idContrato {  get; set; }
        public List<PersonaFisica> personaCuidador { get; set; }
        public List<ContratoItem> contratoItem { get; set; }
    }
}
