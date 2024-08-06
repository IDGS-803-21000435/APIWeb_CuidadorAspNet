using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Cuidador.Models;

public partial class NivelUsuario
{
    public int IdNivelusuario { get; set; }

    public string NombreNivel { get; set; } = null!;

    public DateTime VigenciaHasta { get; set; }

    public string ColorNivel { get; set; } = null!;

    public string? ImagenurlNivel { get; set; }

    public DateTime FechaRegistro { get; set; }

    public int UsuarioRegistro { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public int? UsuarioModifico { get; set; }
    [JsonIgnore]
    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
