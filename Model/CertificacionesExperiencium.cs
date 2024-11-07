using System;
using System.Collections.Generic;

namespace Cuidador.Model;

public partial class CertificacionesExperiencium
{
    public int IdCertificacion { get; set; }

    public string TipoCertificacion { get; set; } = null!;

    public string InstitucionEmisora { get; set; } = null!;

    public DateOnly FechaCertificacion { get; set; }

    public bool Vigente { get; set; }

    public int ExperienciaAnios { get; set; }

    public string? Descripcion { get; set; }

    public DateTime FechaRegistro { get; set; }

    public int UsuarioRegistro { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public int? UsuarioModifico { get; set; }

    public int PersonaId { get; set; }

    public int? DocumentoId { get; set; }

    public virtual Documentacion? Documento { get; set; }

    public virtual PersonaFisica Persona { get; set; } = null!;
}
