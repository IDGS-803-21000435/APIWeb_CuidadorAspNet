namespace Cuidador.Dto.User.RegistrarUsuario
{
    public class RegistrarUsuarioDTO
    {
        public DomicilioDTO domicilio { get; set; }
        public DatosMedicosDTO datos_medicos { get; set; }
        public List<PadecimientoDTO> padecimientos { get; set; }
        public UsuarioDTO usuario { get; set; }
        public PersonaDTO persona { get; set; }
        public List<DocumentacionDTO> documentacion { get; set; }
        public CertificacionExperienciaAUXDTO CertificacionesExperiencia { get; set; }
    }
}
