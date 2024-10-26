using Cuidador.Models;

namespace Cuidador.Dto.Contract.RegistrarDatoContrato
{
    public class RegisterContractDTO
    {
        public int personaCuidadorId { get; set; }
        public int personaClienteId {  get; set; }
        public List<ContratoItemDTO> ContratoItem {  get; set; }

    }
}
