using System;
using System.Collections.Generic;

namespace Cuidador.Model;

public partial class TareasContrato
{
    public int IdTareas { get; set; }

    public int ContratoitemId { get; set; }

    public string? TituloTarea { get; set; }

    public string? DescripcionTarea { get; set; }

    public string? TipoTarea { get; set; }

    public int EstatusId { get; set; }

    public DateTime? FechaARealizar { get; set; }

    public DateTime? FechaInicio { get; set; }

    public DateTime? FechaFinalizacion { get; set; }

    public DateTime? FechaPospuesta { get; set; }

    public virtual ContratoItem Contratoitem { get; set; } = null!;

    public virtual Estatus Estatus { get; set; } = null!;
}
