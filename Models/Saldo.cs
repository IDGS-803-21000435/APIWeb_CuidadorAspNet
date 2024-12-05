using System;
using System.Collections.Generic;

namespace Cuidador.Models;

public partial class Saldo
{
    public int IdSaldo { get; set; }

    public int UsuarioId { get; set; }

    public decimal? SaldoActual { get; set; }

    public int Estatusid { get; set; }

    public DateTime FechaRegistro { get; set; }

    public int UsuarioRegistro { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public int? UsuarioModifico { get; set; }

    public virtual Estatus Estatus { get; set; } = null!;

    public virtual ICollection<TransaccionesSaldo> TransaccionesSaldos { get; set; } = new List<TransaccionesSaldo>();

    public virtual Usuario Usuario { get; set; } = null!;
}
