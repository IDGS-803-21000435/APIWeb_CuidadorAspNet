namespace Cuidador.Dto.User.ListarCuidador
{
    public class OUTPersonaFisicaWebDTO
    {
        public int id_persona { get; set; }

        public string nombre { get; set; } = null!;

        public string apellido_paterno { get; set; } = null!;

        public string apellido_materno { get; set; } = null!;

        public string correo_electronico { get; set; } = null!;

        public DateOnly fecha_nacimiento { get; set; }

        public string genero { get; set; } = null!;

        public string estado_Civil { get; set; } = null!;

        public string rfc { get; set; } = null!;

        public string curp { get; set; } = null!;

        public string? telefono_particular { get; set; }

        public string telefono_movil { get; set; } = null!;

        public string? telefono_emergencia { get; set; }

        public string? nombrecompleto_familiar { get; set; }

        public int domicilio_id { get; set; }

        public int? datos_medicosid { get; set; }

        public int usuario_id { get; set; }

        public string? avatar_image { get; set; }

        public int? estatus_id { get; set; }

        public short? EsFamiliar { get; set; }
        public DateTime fecha_registro { get; set; }

        public int usuario_registro { get; set; }

        public DateTime? fecha_modificacion { get; set; }

        public int? usuario_modifico { get; set; }
    }
}