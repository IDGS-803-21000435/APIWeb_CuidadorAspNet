using System;
using System.Collections.Generic;

namespace Cuidador.Model;

public partial class TransaccionesSaldo
{
    public int IdTransaccionsaldo { get; set; }

    public int SaldoId { get; set; }

    public string ConceptoTransaccion { get; set; } = null!;

    public int MetodoPagoid { get; set; }

    public string TipoMovimiento { get; set; } = null!;

    public DateTime FechaTransaccion { get; set; }

    public decimal ImporteTransaccion { get; set; }

    public decimal SaldoActual { get; set; }

    public decimal SaldoAnterior { get; set; }

    public DateTime FechaRegistro { get; set; }

    public int UsuarioRegistro { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public int? UsuarioModifico { get; set; }

    public virtual MetodoPago MetodoPago { get; set; } = null!;

    public virtual Saldo Saldo { get; set; } = null!;
}
