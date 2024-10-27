namespace Cuidador.Dto.Contract.RegistrarDatoContrato
{
    public class TareasContratoDTO
    {
        public int? idTarea {  get; set; }  
        public string? tituloTarea { get; set; }

        public string? descripcionTarea { get; set; }

        public string? tipoTarea { get; set; }

        public int estatusId { get; set; }

        public DateTime? fechaARealizar { get; set; }
    }
}
