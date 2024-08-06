using System;
using System.Collections.Generic;

namespace Cuidador.Models;

public partial class TipousuarioMenu
{
    public int IdUsuariomenu { get; set; }

    public int MenuId { get; set; }

    public int TipousuarioId { get; set; }

    public virtual Menu Menu { get; set; } = null!;

    public virtual TipoUsuario Tipousuario { get; set; } = null!;
}
