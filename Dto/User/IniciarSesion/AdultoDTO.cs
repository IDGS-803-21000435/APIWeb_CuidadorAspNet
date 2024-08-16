using Cuidador.Models;
using System.Text.Json.Serialization;

namespace Cuidador.Dto.User.IniciarSesion
{
    public class AdultoDTO
    {
        public Domicilio domicilio {  get; set; }
        public DatosMedico DatosMedico { get; set; }
        public List<Padecimiento> Padecimiento { get; set; }
        public PersonaFisica PersonaFisica { get; set; }
        public List<Documentacion> documentacion { get; set; }

    }
}
