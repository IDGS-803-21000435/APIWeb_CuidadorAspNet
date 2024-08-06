using Cuidador.Models;
using System.Text.Json.Serialization;

namespace Cuidador.Dto.User.IniciarSesion
{
    public class OUTVerUsuario
    {
        public int id_usuario {  get; set; }
        public string usuario { get; set; }
        public string nivelUsuario { get; set; }
        public decimal salario_cuidador { get; set; }
        public string tipo_usuario { get; set; }
        public int cuidados_realizados { get; set; }
        public List<PersonaFisica> personaFisica { get; set; }
        public List<ComentariosUsuario> comentarios_usuario { get; set; }
        [JsonIgnore]
        public List<Domicilio> domicilio { get; set; }
    }
}
