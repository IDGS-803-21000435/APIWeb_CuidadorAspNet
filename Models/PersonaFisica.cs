using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Cuidador.Models;

public partial class PersonaFisica
{
    public int IdPersona { get; set; }

    public string Nombre { get; set; } = null!;

    public string ApellidoPaterno { get; set; } = null!;

    public string ApellidoMaterno { get; set; } = null!;

    public string CorreoElectronico { get; set; } = null!;

    public DateOnly FechaNacimiento { get; set; }

    public string Genero { get; set; } = null!;

    public string EstadoCivil { get; set; } = null!;

    public string Rfc { get; set; } = null!;

    public string Curp { get; set; } = null!;

    public string? TelefonoParticular { get; set; }

    public string TelefonoMovil { get; set; } = null!;

    public string? TelefonoEmergencia { get; set; }

    public string? NombrecompletoFamiliar { get; set; }

    public int DomicilioId { get; set; }

    public int? DatosMedicosid { get; set; }

    public DateTime FechaRegistro { get; set; }

    public int UsuarioRegistro { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public int? UsuarioModifico { get; set; }

    public int UsuarioId { get; set; }

    public string? AvatarImage { get; set; }

    public int? EstatusId { get; set; }

    public short? EsFamiliar { get; set; }
    public virtual ICollection<CertificacionesExperiencium> CertificacionesExperiencia { get; set; } = new List<CertificacionesExperiencium>();

    public virtual ICollection<ComentariosUsuario> ComentariosUsuarioPersonaEmisors { get; set; } = new List<ComentariosUsuario>();

    public virtual ICollection<ComentariosUsuario> ComentariosUsuarioPersonaReceptors { get; set; } = new List<ComentariosUsuario>();

    public virtual ICollection<Contrato> ContratoPersonaidClienteNavigations { get; set; } = new List<Contrato>();

    public virtual ICollection<Contrato> ContratoPersonaidCuidadorNavigations { get; set; } = new List<Contrato>();

    public virtual DatosMedico? DatosMedicos { get; set; }

    public virtual ICollection<Documentacion> Documentacions { get; set; } = new List<Documentacion>();
   
    public virtual Domicilio Domicilio { get; set; } = null!;

    public virtual Estatus? Estatus { get; set; }

    public virtual Usuario Usuario { get; set; } = null!;
}
