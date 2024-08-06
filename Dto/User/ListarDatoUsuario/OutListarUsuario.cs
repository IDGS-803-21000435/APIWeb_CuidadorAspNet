using Cuidador.Dto.User.ListarDatoUsuario;
using Cuidador.Models;

namespace Cuidador.Dto.User.ListarDatoUsuario
{
    public class OutListarUsuario
    {
        public Domicilio Domicilio { get; set; }
        public DatosMedico DatosMedicos { get; set; }
        public List<Padecimiento> Padecimientos { get; set; }
        public Usuario Usuario { get; set; }
        public PersonaFisica Persona { get; set; }
        public List<Documentacion> Documentacion { get; set; }
        public List<CertificacionExperienciaListarDTO> CertificacionesExperiencia { get; set; }
    }
}
