using System;
using System.Collections.Generic;

namespace Cuidador.Models;

public partial class CapacitacionUsuario
{
    public int Usuarioid { get; set; }

    public int Capacitacionid { get; set; }

    public decimal Calificacion { get; set; }

    public int Estatusid { get; set; }

    public DateTime? FechaInicio { get; set; }

    public DateTime? FechaFinalizacion { get; set; }

    public virtual Capacitacion Capacitacion { get; set; } = null!;

    public virtual Estatus Estatus { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
