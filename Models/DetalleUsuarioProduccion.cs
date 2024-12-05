using System;
using System.Collections.Generic;

namespace Cuidador.Models;

public partial class DetalleUsuarioProduccion
{
    public long IdDetalle { get; set; }

    public int? IdUsuario { get; set; }

    public string Accion { get; set; } = null!;

    public DateTime? Fecha { get; set; }
}
