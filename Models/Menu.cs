using System;
using System.Collections.Generic;

namespace Cuidador.Models;

public partial class Menu
{
    public int IdMenu { get; set; }

    public int EstatusId { get; set; }

    public string NombreMenu { get; set; } = null!;

    public string? DescripcionMenu { get; set; }

    public string RutaMenu { get; set; } = null!;

    public string Endpoint { get; set; } = null!;

    public DateTime FechaRegistro { get; set; }

    public int UsuarioRegistro { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public int? UsuarioModifico { get; set; }

    public virtual Estatus Estatus { get; set; } = null!;

    public virtual ICollection<TipousuarioMenu> TipousuarioMenus { get; set; } = new List<TipousuarioMenu>();
}
