using System;
using System.Collections.Generic;

namespace Cuidador.Models;

public partial class CuentaBancarium
{
    public int IdCuentabancaria { get; set; }

    public int UsuarioId { get; set; }

    public decimal NumeroCuenta { get; set; }

    public decimal ClabeInterbancaria { get; set; }

    public string Nombrebanco { get; set; } = null!;

    public DateTime FechaRegistro { get; set; }

    public int UsuarioRegistro { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public int? UsuarioModifico { get; set; }

    public virtual ICollection<MovimientoCuentum> MovimientoCuenta { get; set; } = new List<MovimientoCuentum>();

    public virtual Usuario Usuario { get; set; } = null!;
}
