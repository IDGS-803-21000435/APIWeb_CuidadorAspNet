namespace Cuidador.Dto.User.ListarCuidador
{
    public class OUTDocumentacionDTO
    {
        public int id_documentacion { get; set; }

        public int persona_id { get; set; }

        public string? tipo_documento { get; set; }

        public string? nombre_documento { get; set; }

        public string url_documento { get; set; } = null!;

        public DateOnly? fecha_emision { get; set; }

        public DateOnly? fecha_expiracion { get; set; }

        public decimal version { get; set; }

        public int estatus_id { get; set; }

        public DateTime fecha_registro { get; set; }

        public int usuario_registro { get; set; }

        public DateTime? fecha_modificacion { get; set; }

        public int? usuario_modifico { get; set; }
    }
}
