using System;
using System.Collections.Generic;

namespace Cuidador.Models;

public partial class TipoUsuario
{
    public int IdTipousuario { get; set; }

    public string NombreTipo { get; set; } = null!;

    public DateTime FechaRegistro { get; set; }

    public int UsuarioRegistro { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public int? UsuarioModifico { get; set; }

    public virtual ICollection<AvisosMovil> AvisosMovils { get; set; } = new List<AvisosMovil>();

    public virtual ICollection<TipousuarioMenu> TipousuarioMenus { get; set; } = new List<TipousuarioMenu>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
