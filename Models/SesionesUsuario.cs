using System;
using System.Collections.Generic;

namespace Cuidador.Models;

public partial class SesionesUsuario
{
    public int IdSesion { get; set; }

    public int UsuarioId { get; set; }

    public DateTime? UltimoinicioSesion { get; set; }

    public DateTime? UltimocambioContrasenia { get; set; }

    public long? Ipsesion { get; set; }

    public string? NombreEquipo { get; set; }

    public string? SistemaOperativo { get; set; }

    public bool IsSesionactiva { get; set; }

    public virtual Usuario Usuario { get; set; } = null!;
}
