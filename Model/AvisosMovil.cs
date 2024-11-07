using System;
using System.Collections.Generic;

namespace Cuidador.Model;

public partial class AvisosMovil
{
    public int Idaviso { get; set; }

    public string Encabezado { get; set; } = null!;

    public string? Detalle { get; set; }

    public int TipoUsuarioDirigido { get; set; }

    public DateTime VisibleHasta { get; set; }

    public bool Esdisponible { get; set; }

    public virtual TipoUsuario TipoUsuarioDirigidoNavigation { get; set; } = null!;
}
