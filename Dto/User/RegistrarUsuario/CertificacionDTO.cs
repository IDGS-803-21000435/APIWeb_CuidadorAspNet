namespace Cuidador.Dto.User.RegistrarUsuario
{
    public class CertificacionDTO
    {
        public string TipoCertificacion { get; set; }
        public string InstitucionEmisora { get; set; }
        public DateOnly FechaCertificacion { get; set; }
        public bool Vigente { get; set; }
        public string Descripcion { get; set; }
    }
}
