using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Cuidador.Models;

public partial class MovimientoCuentum
{
    public int IdMovimientocuenta { get; set; }

    public int CuentabancariaId { get; set; }

    public string ConceptoMovimiento { get; set; } = null!;

    public int MetodoPagoid { get; set; }

    public string TipoMovimiento { get; set; } = null!;

    public decimal? NumeroseguimientoBanco { get; set; }

    public DateTime FechaMovimiento { get; set; }

    public decimal ImporteMovimiento { get; set; }

    public decimal SaldoActual { get; set; }

    public decimal SaldoAnterior { get; set; }

    [JsonIgnore]
    public virtual CuentaBancarium Cuentabancaria { get; set; } = null!;

    public virtual MetodoPago MetodoPago { get; set; } = null!;
}
