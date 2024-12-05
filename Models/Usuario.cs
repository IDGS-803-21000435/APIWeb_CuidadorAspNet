using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Cuidador.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public int UsuarionivelId { get; set; }

    public int TipoUsuarioid { get; set; }

    public int Estatusid { get; set; }

    public string Usuario1 { get; set; } = null!;

    public string Contrasenia { get; set; } = null!;

    public DateTime FechaRegistro { get; set; }

    public int UsuarioRegistro { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public int? UsuarioModifico { get; set; }

    public virtual ICollection<CapacitacionUsuario> CapacitacionUsuarios { get; set; } = new List<CapacitacionUsuario>();

    public virtual ICollection<CuentaBancarium> CuentaBancaria { get; set; } = new List<CuentaBancarium>();

    public virtual Estatus Estatus { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<MetodoPagoUsuario> MetodoPagoUsuarios { get; set; } = new List<MetodoPagoUsuario>();

    [JsonIgnore]
    public virtual ICollection<PersonaFisica> PersonaFisicas { get; set; } = new List<PersonaFisica>();

    [JsonIgnore]
    public virtual ICollection<SalarioCuidador> SalarioCuidadors { get; set; } = new List<SalarioCuidador>();

    public virtual ICollection<Saldo> Saldos { get; set; } = new List<Saldo>();

    public virtual ICollection<SesionesUsuario> SesionesUsuarios { get; set; } = new List<SesionesUsuario>();

    public virtual ICollection<TarjetaUsuario> TarjetaUsuarios { get; set; } = new List<TarjetaUsuario>();

    public virtual TipoUsuario TipoUsuario { get; set; } = null!;

    public virtual NivelUsuario Usuarionivel { get; set; } = null!;

    public virtual ICollection<VersionesMovil> VersionesMovils { get; set; } = new List<VersionesMovil>();
}
