namespace Cuidador.Dto.User.ListarCuidador
{
    public class OUTDomicilioDTO
    {
        public int id_domicilio { get; set; }

        public string calle { get; set; } = null!;

        public string colonia { get; set; } = null!;

        public string? numero_interior { get; set; }

        public string? numero_exterior { get; set; }

        public string ciudad { get; set; } = null!;

        public string estado { get; set; } = null!;

        public string pais { get; set; } = null!;

        public string? referencias { get; set; }

        public int? estatus_id { get; set; }

        public DateTime? fecha_registro { get; set; }

        public int usuario_registro { get; set; }

        public DateTime? fecha_modificacion { get; set; }

        public int? usuario_modifico  { get; set; }
    }
}
