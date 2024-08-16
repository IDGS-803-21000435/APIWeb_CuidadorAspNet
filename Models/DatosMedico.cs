using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Cuidador.Models;

public partial class DatosMedico
{
    public int IdDatosmedicos { get; set; }

    public string? AntecedentesMedicos { get; set; }

    public string? Alergias { get; set; }

    public string? TipoSanguineo { get; set; }

    public string? NombreMedicofamiliar { get; set; }

    public string? TelefonoMedicofamiliar { get; set; }

    public string? Observaciones { get; set; }

    public DateTime FechaRegistro { get; set; }

    public int UsuarioRegistro { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public int? UsuarioModifico { get; set; }

    public virtual ICollection<Padecimiento> Padecimientos { get; set; } = new List<Padecimiento>();
    [JsonIgnore]
    public virtual ICollection<PersonaFisica> PersonaFisicas { get; set; } = new List<PersonaFisica>();
}
