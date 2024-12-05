using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Cuidador.Models;

public partial class TarjetaUsuario
{
    public int IdTarjeta { get; set; }

    public int UsuarioId { get; set; }

    public string? Beneficiario { get; set; }

    public string? NumeroTarjeta { get; set; }

    public DateTime? Vencimiento { get; set; }

    public string? Ccv { get; set; }

    public string? Banco { get; set; }

    public string? TipoTarjeta { get; set; }

    public int? ConcurrenciaUso { get; set; }

    [JsonIgnore]
    public virtual Usuario Usuario { get; set; } = null!;
}
