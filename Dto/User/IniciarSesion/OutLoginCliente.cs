using Cuidador.Models;

namespace Cuidador.Dto.User.IniciarSesion
{
    public class OutLoginCliente
    {
        public Domicilio domicilio { get; set; }
        public DatosMedico datosMedico { get; set; }
        public List<Padecimiento> padecimiento { get; set; }
        public Usuario usuario { get; set; }
        public PersonaFisica personaFisica { get; set; }
        public List<Documentacion> documentacion { get; set; }
        public List<AdultoDTO> adulto {get;set;}
    }
}
