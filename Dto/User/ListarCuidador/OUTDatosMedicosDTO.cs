namespace Cuidador.Dto.User.ListarCuidador
{
    public class OUTDatosMedicosDTO
    {
        public int id_datosmedicos { get; set; }

        public string? antecedentes_medicos { get; set; }

        public string? alergias { get; set; }

        public string? tipo_sanguineo { get; set; }

        public string? nombre_medicofamiliar { get; set; }

        public string? telefono_medicofamiliar { get; set; }

        public string? observaciones { get; set; }

        public DateTime fecha_registro { get; set; }

        public int usuario_registro { get; set; }

        public DateTime? fecha_modificacion { get; set; }

        public int? usuario_modifico { get; set; }
    }
}
