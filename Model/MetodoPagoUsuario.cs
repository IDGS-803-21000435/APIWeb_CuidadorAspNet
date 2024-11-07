using System;
using System.Collections.Generic;

namespace Cuidador.Model;

public partial class MetodoPagoUsuario
{
    public int IdMetodousuario { get; set; }

    public int UsuarioId { get; set; }

    public string NombreBeneficiario { get; set; } = null!;

    public string NumeroTarjeta { get; set; } = null!;

    public DateOnly FechaVencimiento { get; set; }

    public string Ccv { get; set; } = null!;

    public int VecesUsada { get; set; }

    public int UsuarioRegistro { get; set; }

    public DateTime FechaRegistro { get; set; }

    public int? UsuarioModifico { get; set; }

    public DateTime? FechaModifico { get; set; }

    public virtual Usuario Usuario { get; set; } = null!;
}
