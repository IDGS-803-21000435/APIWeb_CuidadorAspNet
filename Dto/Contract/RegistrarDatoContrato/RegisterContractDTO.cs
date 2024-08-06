using Cuidador.Models;

namespace Cuidador.Dto.Contract.RegistrarDatoContrato
{
    public class RegisterContractDTO
    {
        public int persona_cuidador_id { get; set; }
        public int persona_cliente_id {  get; set; }
        public List<ContratoItemDTO> contrato_item {  get; set; }

    }
}
