using Cuidador.Models;

namespace Cuidador.Dto.User.ListarCuidador
{
    public class OUTVerCliente
    {
        public int idUsuario { get; set; }
        public string usuario { get; set; }
        public string nivel_usuario { get; set; }
        public string tipo_usuario { get; set; }
        public List<PersonaFisica> personaFisica { get; set; }
    }
}
