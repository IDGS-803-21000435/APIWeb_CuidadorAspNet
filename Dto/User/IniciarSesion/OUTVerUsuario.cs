using Cuidador.Models;
using System.Text.Json.Serialization;

namespace Cuidador.Dto.User.IniciarSesion
{
    public class OUTVerUsuario
    {
        public int id_usuario {  get; set; }
        public string usuario { get; set; }
        public string nivelUsuario { get; set; }
        public List<SalarioCuidador> horariosCuidador { get; set; }
        public string tipo_usuario { get; set; }
        public int cuidadosRealizados { get; set; }
        public List<PersonaFisica> personaFisica { get; set; }
        public List<ComentariosUsuario> comentariosUsuarioPersonaReceptor { get; set; }
        [JsonIgnore]
        public List<Domicilio> domicilio { get; set; }
    }
}
