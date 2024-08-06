namespace Cuidador.Dto.User.RegistrarUsuario
{
    public class CertificacionDTO
    {
        public int IdCertificacion { get; set; }
        public string TipoCertificacion { get; set; }
        public string InstitucionEmisora { get; set; }
        public DateOnly FechaCertificacion { get; set; }
        public bool Vigente { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int UsuarioRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
        public int UsuarioModifico { get; set; }
        public int PersonaId { get; set; }
        public int DocumentoId { get; set; }
    }
}
