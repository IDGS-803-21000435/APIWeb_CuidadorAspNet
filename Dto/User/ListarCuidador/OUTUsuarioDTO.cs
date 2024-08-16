namespace Cuidador.Dto.User.ListarCuidador
{
    public class OUTUsuarioDTO
    {
        public int id_usuario { get; set; }

        public int usuarionivel_id { get; set; }

        public int tipo_usuarioid { get; set; }

        public int estatusid { get; set; }

        public string usuario { get; set; } = null!;

        public string contrasenia { get; set; } = null!;

        public DateTime fecha_registro { get; set; }

        public int usuario_registro { get; set; }

        public DateTime? fecha_modificacion { get; set; }

        public int? usuario_modifico { get; set; }
    }
}
