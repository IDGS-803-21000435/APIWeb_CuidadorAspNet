using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Cuidador.Models;

public partial class Padecimiento
{
    public int IdPadecimiento { get; set; }

    public int? DatosmedicosId { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public DateOnly? PadeceDesde { get; set; }

    public DateTime FechaRegistro { get; set; }

    public int UsuarioRegistro { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public int? UsuarioModifico { get; set; }
    
    public virtual DatosMedico? Datosmedicos { get; set; }
}
