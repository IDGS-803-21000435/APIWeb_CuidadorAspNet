namespace Cuidador.Dto.User.ListarCuidador
{
    public class OUTCertificacionExpDTO
    {
        public int id_certificacion { get; set; }

        public string tipo_certificacion { get; set; } = null!;

        public string institucion_emisora { get; set; } = null!;

        public DateOnly fecha_certificacion { get; set; }

        public bool vigente { get; set; }

        public int ExperienciaAnios { get; set; }//----

        public string? descripcion { get; set; }

        public DateTime fecha_registro { get; set; }

        public int usuario_registro { get; set; }

        public DateTime? fecha_modificacion { get; set; }

        public int? usuario_modifico { get; set; }

        public int persona_id { get; set; }

        public int? documento_id { get; set; }
    }
}
