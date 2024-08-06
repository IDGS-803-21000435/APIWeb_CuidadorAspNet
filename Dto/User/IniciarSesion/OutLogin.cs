using Cuidador.Models;

namespace Cuidador.Dto.User.IniciarSesion
{
    public class OutLogin
    {
        public Usuario usuario;
        public List<PersonaFisica> personas;
        public List<Menu> menus;

        public int IdUsuario { get; set; }

        public int UsuarionivelId { get; set; }

        public int TipoUsuarioid { get; set; }

        public int Estatusid { get; set; }

        public string Usuario1 { get; set; } = null!;

        public string Contrasenia { get; set; } = null!;

        public virtual ICollection<PersonaFisica> PersonaFisicas { get; set; } = new List<PersonaFisica>();

        public virtual ICollection<Menu> Menu { get; set; } = new List<Menu>();
    }
}
