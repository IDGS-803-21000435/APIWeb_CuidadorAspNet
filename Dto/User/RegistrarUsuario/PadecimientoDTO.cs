namespace Cuidador.Dto.User.RegistrarUsuario
{
    public class PadecimientoDTO
    {
        public int DatosMedicosId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateOnly PadeceDesde { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int UsuarioRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
        public int UsuarioModifico { get; set; }
    }
}
