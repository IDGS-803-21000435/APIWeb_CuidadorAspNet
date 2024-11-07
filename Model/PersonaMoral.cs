using System;
using System.Collections.Generic;

namespace Cuidador.Model;

public partial class PersonaMoral
{
    public int IdPersonamoral { get; set; }

    public string RazonSocial { get; set; } = null!;

    public string? NombreComercial { get; set; }

    public string Rfc { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string CorreoElectronico { get; set; } = null!;

    public int DireccionId { get; set; }

    public DateTime FechaRegistro { get; set; }

    public int UsuarioRegistro { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public int? UsuarioModifico { get; set; }

    public virtual Domicilio Direccion { get; set; } = null!;
}
