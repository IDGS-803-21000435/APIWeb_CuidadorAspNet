using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Cuidador.Models;

public partial class Domicilio
{
    public int IdDomicilio { get; set; }

    public string Calle { get; set; } = null!;

    public string Colonia { get; set; } = null!;

    public string? NumeroInterior { get; set; }

    public string? NumeroExterior { get; set; }

    public string Ciudad { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public string Pais { get; set; } = null!;

    public string? Referencias { get; set; }

    public int? EstatusId { get; set; }

    public DateTime FechaRegistro { get; set; }

    public int UsuarioRegistro { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public int? UsuarioModifico { get; set; }

    public virtual Estatus? Estatus { get; set; }
    [JsonIgnore]
    public virtual ICollection<PersonaFisica> PersonaFisicas { get; set; } = new List<PersonaFisica>();

    public virtual ICollection<PersonaMoral> PersonaMorals { get; set; } = new List<PersonaMoral>();
}
