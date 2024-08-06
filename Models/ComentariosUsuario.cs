using System;
using System.Collections.Generic;

namespace Cuidador.Models;

public partial class ComentariosUsuario
{
    public int IdComentarios { get; set; }

    public int PersonaReceptorid { get; set; }

    public int PersonaEmisorid { get; set; }

    public short Calificacion { get; set; }

    public string? Comentario { get; set; }

    public DateTime FechaRegistro { get; set; }

    public int UsuarioRegistro { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public int? UsuarioModifico { get; set; }

    public virtual PersonaFisica PersonaEmisor { get; set; } = null!;

    public virtual PersonaFisica PersonaReceptor { get; set; } = null!;
}
