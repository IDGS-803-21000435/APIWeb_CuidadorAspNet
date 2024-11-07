using System;
using System.Collections.Generic;

namespace Cuidador.Model;

public partial class Contrato
{
    public int IdContrato { get; set; }

    public int PersonaidCuidador { get; set; }

    public int PersonaidCliente { get; set; }

    public int EstatusId { get; set; }

    public int UsuarioRegistro { get; set; }

    public DateTime FechaRegistro { get; set; }

    public DateTime? UsuarioModifico { get; set; }

    public DateTime? FechaModifico { get; set; }

    public virtual ICollection<ContratoItem> ContratoItems { get; set; } = new List<ContratoItem>();

    public virtual Estatus Estatus { get; set; } = null!;

    public virtual PersonaFisica PersonaidClienteNavigation { get; set; } = null!;

    public virtual PersonaFisica PersonaidCuidadorNavigation { get; set; } = null!;
}
