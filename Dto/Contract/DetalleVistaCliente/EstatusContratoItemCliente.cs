namespace Cuidador.Dto.Contract.DetalleVistaCliente
{
	
	public class EstatusContratoItemCliente
	{
		public DateTime horarioInicioPropuesto { get; set; }
		public DateTime horarioFinPropuesto { get; set; }
		public decimal importeTotal { get; set; }
		public int tiempoContratado { get; set; }
		public DateTime fechaAceptacion { get; set; }
		public DateTime fechaInicioCuidado { get; set; }
		public DateTime fechaFinCuidado { get; set; }
		public List<EstatusTareasContratoItem> estatusTareas { get; set; }
	}
	
	public class EstatusTareasContratoItem
	{
		public string tituloTarea { get; set; }
		public string descripcionTarea { get; set; }
		public int estatusId { get; set; }
		public string nombreEstatus { get; set; }
		public DateTime fechaEstatusTarea { get; set; }
	}
	
}