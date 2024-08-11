namespace Cuidador.Dto.Contract.RegistrarDatoContrato
{
    public class TareasContratoDTO
    {
        public int? idTarea {  get; set; }  
        public string? TituloTarea { get; set; }

        public string? DescripcionTarea { get; set; }

        public string? TipoTarea { get; set; }

        public int EstatusId { get; set; }

        public DateTime? FechaARealizar { get; set; }
    }
}
