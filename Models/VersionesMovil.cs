using System;
using System.Collections.Generic;

namespace Cuidador.Models;

public partial class VersionesMovil
{
    public int IdVersiones { get; set; }

    public string NumeroVersion { get; set; } = null!;

    public string? DescripcionVersion { get; set; }

    public string? HashCommit { get; set; }

    public int UsuarioDesplego { get; set; }

    public DateTime FechaDespliegue { get; set; }

    public string? ObservacionesPost { get; set; }

    public virtual Usuario UsuarioDesplegoNavigation { get; set; } = null!;
}
