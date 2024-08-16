namespace Cuidador.Dto.User.ListarCuidador
{
    public class OUTPadecimientosDTO
    {
        public int id_padecimiento { get; set; }

        public int? datosmedicos_id { get; set; }

        public string? nombre { get; set; }

        public string? descripcion { get; set; }

        public DateOnly? padeceDesde { get; set; }

        public DateTime fecha_registro { get; set; }

        public int usuario_registro { get; set; }

        public DateTime? fecha_modificacion { get; set; }

        public int? usuario_modifico { get; set; }
    }
}
