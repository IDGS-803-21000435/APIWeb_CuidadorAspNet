using Cuidador.Models;

namespace Cuidador.Dto.User.ListarDatoUsuario
{
    public class OUTUsCuidador
    {
        public int IdUsuario { get; set; }
        public string Usuario1 { get; set; } = null!;
        public int nivelUsuario { get; set; }
        public List<ComentariosUsuario> comentarioUsuario { get; set; }
        public List<CertificacionesExperiencium> certifiaciones { get; set; }
        public List<PersonaFisica> personasFisicas { get; set; }
    }
}
