using System;
using System.Collections.Generic;

namespace Cuidador.Models;

public partial class SalarioCuidador
{
    public int IdSueldonivel { get; set; }

    public int Usuarioid { get; set; }

    public decimal PrecioPorHora { get; set; }

    public DateTime FechaRegistro { get; set; }

    public int UsuarioRegistro { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public int? UsuarioModifico { get; set; }

    public int? Concurrencia { get; set; }

    public virtual Usuario Usuario { get; set; } = null!;
}
