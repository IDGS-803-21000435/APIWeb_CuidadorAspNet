using System;
using System.Collections.Generic;

namespace Cuidador.Models;

public partial class ContratoItem
{
    public int IdContratoitem { get; set; }

    public int ContratoId { get; set; }

    public int EstatusId { get; set; }

    public string? Observaciones { get; set; }

    public DateTime? HorarioInicioPropuesto { get; set; }

    public DateTime? HorarioFinPropuesto { get; set; }

    public DateTime? FechaAceptacion { get; set; }

    public DateTime? FechaInicioCuidado { get; set; }

    public DateTime? FechaFinCuidado { get; set; }

    public virtual Contrato Contrato { get; set; } = null!;

    public virtual Estatus Estatus { get; set; } = null!;

    public virtual ICollection<TareasContrato> TareasContratos { get; set; } = new List<TareasContrato>();
}
