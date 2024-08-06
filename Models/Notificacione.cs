using System;
using System.Collections.Generic;

namespace Cuidador.Models;

public partial class Notificacione
{
    public int IdNotificacion { get; set; }

    public int PersonaidNoti { get; set; }

    public string? RutaMenu { get; set; }

    public string TituloNoti { get; set; } = null!;

    public string? DescripcionNoti { get; set; }

    public int Estatusid { get; set; }

    public int UsuarioRegistro { get; set; }

    public DateTime FechaRegistro { get; set; }

    public int? UsuarioModifico { get; set; }

    public DateTime? FechaModifico { get; set; }

    public virtual Estatus Estatus { get; set; } = null!;

    public virtual PersonaFisica PersonaidNotiNavigation { get; set; } = null!;
}
