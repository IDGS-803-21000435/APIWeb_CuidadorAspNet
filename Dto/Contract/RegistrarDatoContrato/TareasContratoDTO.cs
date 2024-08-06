namespace Cuidador.Dto.Contract.RegistrarDatoContrato
{
    public class TareasContratoDTO
    {
        public string? TituloTarea { get; set; }

        public string? DescripcionTarea { get; set; }

        public string? TipoTarea { get; set; }

        public int EstatusId { get; set; }

        public DateTime? FechaARealizar { get; set; }

        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaFinalizacion { get; set; }

        public DateTime? FechaPospuesta { get; set; }
    }
}
