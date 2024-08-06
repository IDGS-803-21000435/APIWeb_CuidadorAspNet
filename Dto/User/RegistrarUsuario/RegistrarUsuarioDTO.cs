namespace Cuidador.Dto.User.RegistrarUsuario
{
    public class RegistrarUsuarioDTO
    {
        public DomicilioDTO Domicilio { get; set; }
        public DatosMedicosDTO DatosMedicos { get; set; }
        public List<PadecimientoDTO> Padecimientos { get; set; }
        public UsuarioDTO Usuario { get; set; }
        public PersonaDTO Persona { get; set; }
        public List<DocumentacionDTO> Documentacion { get; set; }
        public List<CertificacionExperienciaDTO> CertificacionesExperiencia { get; set; }
    }
}
