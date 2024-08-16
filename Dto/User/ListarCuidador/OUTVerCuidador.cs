using Cuidador.Models;

namespace Cuidador.Dto.User.ListarCuidador
{
    public class OUTVerCuidador
    {
        public int idUsuario {  get; set; }
        public string usuario { get; set; }
        public string nivelUsuario { get; set; }
        public List<ComentariosUsuario> comentariosUsuario { get; set; }
        public List<CertificacionesExperiencium> certificaciones {  get; set; }
        public List<OUTPersonaFisicaDTO> personaFisica { get; set; }
        public int cuidadosrealizados { get; set; }
        public decimal salarioCuidador {  get; set; }
    }
}
