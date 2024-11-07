using System;
using System.Collections.Generic;

namespace Cuidador.Model;

public partial class Documentacion
{
    public int IdDocumentacion { get; set; }

    public int PersonaId { get; set; }

    public string? TipoDocumento { get; set; }

    public string? NombreDocumento { get; set; }

    public string UrlDocumento { get; set; } = null!;

    public DateOnly? FechaEmision { get; set; }

    public DateOnly? FechaExpiracion { get; set; }

    public decimal Version { get; set; }

    public int EstatusId { get; set; }

    public DateTime FechaRegistro { get; set; }

    public int UsuarioRegistro { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public int? UsuarioModifico { get; set; }

    public virtual ICollection<CertificacionesExperiencium> CertificacionesExperiencia { get; set; } = new List<CertificacionesExperiencium>();

    public virtual Estatus Estatus { get; set; } = null!;

    public virtual PersonaFisica Persona { get; set; } = null!;
}
