using System;
using System.Collections.Generic;

namespace Cuidador.Models;

public partial class MetodoPago
{
    public int IdMetodopago { get; set; }

    public string Nombre { get; set; } = null!;

    public DateTime FechaRegistro { get; set; }

    public int UsuarioRegistro { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public int? UsuarioModifico { get; set; }

    public virtual ICollection<MovimientoCuentum> MovimientoCuenta { get; set; } = new List<MovimientoCuentum>();

    public virtual ICollection<TransaccionesSaldo> TransaccionesSaldos { get; set; } = new List<TransaccionesSaldo>();
}
