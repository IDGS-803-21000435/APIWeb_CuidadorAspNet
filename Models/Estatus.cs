using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Cuidador.Models;

public partial class Estatus
{
    public int IdEstatus { get; set; }

    public int TipoestatusId { get; set; }

    public string Codigo { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string? Color { get; set; }

    public DateTime FechaRegistro { get; set; }

    public int UsuarioRegistro { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public int? UsuarioModifico { get; set; }

    public virtual ICollection<CapacitacionUsuario> CapacitacionUsuarios { get; set; } = new List<CapacitacionUsuario>();

    public virtual ICollection<Capacitacion> Capacitacions { get; set; } = new List<Capacitacion>();

    [JsonIgnore]
    public virtual ICollection<ContratoItem> ContratoItems { get; set; } = new List<ContratoItem>();

    [JsonIgnore]
    public virtual ICollection<Contrato> Contratos { get; set; } = new List<Contrato>();

    public virtual ICollection<Documentacion> Documentacions { get; set; } = new List<Documentacion>();

    public virtual ICollection<Domicilio> Domicilios { get; set; } = new List<Domicilio>();

    [JsonIgnore]
    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<Menu> Menus { get; set; } = new List<Menu>();

    public virtual ICollection<Notificacione> Notificaciones { get; set; } = new List<Notificacione>();

    public virtual ICollection<PersonaFisica> PersonaFisicas { get; set; } = new List<PersonaFisica>();

    public virtual ICollection<Saldo> Saldos { get; set; } = new List<Saldo>();

    [JsonIgnore]
    public virtual ICollection<TareasContrato> TareasContratos { get; set; } = new List<TareasContrato>();

    public virtual TipoEstatus Tipoestatus { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
