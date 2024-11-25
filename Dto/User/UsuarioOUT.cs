using Cuidador.Models;

namespace Cuidador.Dto.User
{
    public class UsuarioOUT
    {
        public int IdUsuario { get; set; }

        public int UsuarionivelId { get; set; }

        public int TipoUsuarioid { get; set; }

        public int Estatusid { get; set; }

        public string Usuario1 { get; set; } = null!;

        public string Contrasenia { get; set; } = null!;

        public DateTime FechaRegistro { get; set; }

        public int UsuarioRegistro { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public int? UsuarioModifico { get; set; }
        public List<PersonaFisica> PersonaFisicas { get; set; }
    }
}
