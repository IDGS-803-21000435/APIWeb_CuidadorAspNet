using Cuidador.Models;

namespace Cuidador.Dto.User.IniciarSesion
{
    public class OutLoginCuidador
    {
        public Domicilio domicilio {  get; set; }
        public DatosMedico datosMedico {  get; set; }
        public List<Padecimiento> padecimientos { get; set; }
        public Usuario usuario { get; set; }
        public PersonaFisica personaFisica { get; set; }
        public List<Documentacion> documentaciones { get; set; }
        public List<CertificacionesExperiencium> certificaciones { get; set; }
    }
}
