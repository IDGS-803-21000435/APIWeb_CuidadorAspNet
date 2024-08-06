using System;
using System.Collections.Generic;

namespace Cuidador.Models;

public partial class Capacitacion
{
    public int IdCapacitacion { get; set; }

    public string NombreCapacitacion { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public string? UrlDocumento { get; set; }

    public int Estatusid { get; set; }

    public decimal? CalificacionMaxima { get; set; }

    public decimal? CalificacionAprov { get; set; }

    public DateTime FechaExpiracion { get; set; }

    public DateTime FechaInicio { get; set; }

    public int UsuarioRegistro { get; set; }

    public DateTime FechaRegistro { get; set; }

    public int? UsuarioModifico { get; set; }

    public DateTime? FechaModifico { get; set; }

    public virtual ICollection<CapacitacionUsuario> CapacitacionUsuarios { get; set; } = new List<CapacitacionUsuario>();

    public virtual Estatus Estatus { get; set; } = null!;
}
