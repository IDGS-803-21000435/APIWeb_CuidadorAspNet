using System.ComponentModel.DataAnnotations;

namespace Cuidador.Dto.Feedback
{
    public class insertFeedback
    {
        [Required]
        public int UsuarioIdRemitente { get; set; }

        public string Categoria { get; set; } = null!;
        [Required]
        public string Cuerpo { get; set; } = null!;

        public int UsuarioRegistro { get; set; }

        public DateOnly FechaRegistro { get; set; }

    }
}
