using System;
using System.Collections.Generic;

namespace Cuidador.Models;

public partial class TipoEstatus
{
    public int IdTipoestatus { get; set; }

    public string NombreTipoestatus { get; set; } = null!;

    public virtual ICollection<Estatus> Estatuses { get; set; } = new List<Estatus>();
}
