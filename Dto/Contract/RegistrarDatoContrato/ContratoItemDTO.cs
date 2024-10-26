using Cuidador.Models;

namespace Cuidador.Dto.Contract.RegistrarDatoContrato
{
    public class ContratoItemDTO
    {
        public string observacion {  get; set; }
        public DateTime? horarioInicioPropuesto{ get; set; }
        public DateTime? horarioFinPropuesto{ get; set; }
        public List<TareasContratoDTO> tareaContrato { get; set; }
    }
}
