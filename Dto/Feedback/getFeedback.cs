using Cuidador.Models;

namespace Cuidador.Dto.Feedback
{
    public class getFeedback
    {
        public int IdFeedback { get; set; }

        public int UsuarioIdRemitente { get; set; }

        public string Categoria { get; set; } = null!;

        public string Cuerpo { get; set; } = null!;

        public DateOnly Fecha { get; set; }

        public DateOnly? FechaResolucion { get; set; }

        public int EstatusId { get; set; }

        public int UsuarioRegistro { get; set; }

        public DateOnly FechaRegistro { get; set; }

        public virtual Estatus Estatus { get; set; } = null!;

        public virtual Usuario UsuarioidRemitenteNavigation { get; set; } = null!;
    }
}
