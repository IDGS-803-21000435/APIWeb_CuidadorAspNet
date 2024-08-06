using Cuidador.Models;

namespace Cuidador.Dto.Contract.RegistrarDatoContrato
{
    public class ContratoItemDTO
    {
        public string observaciones {  get; set; }
        public DateTime? horario_inicio_propuesto { get; set; }
        public DateTime? horario_fin_propuesto { get; set; }
        public List<TareasContratoDTO> tareas_contrato { get; set; }
    }
}
