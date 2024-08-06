using System;
using System.Collections.Generic;

namespace Cuidador.Models;

public partial class Cuidahora
{
    public int IdCuidahoras { get; set; }

    public decimal ValorActual { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime FechaVigencia { get; set; }

    public DateTime FechaRegistro { get; set; }

    public int UsuarioRegistro { get; set; }

    public int EstatusId { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public int? UsuarioModifico { get; set; }

    public virtual Estatus Estatus { get; set; } = null!;

    public virtual ICollection<Saldo> Saldos { get; set; } = new List<Saldo>();
}
