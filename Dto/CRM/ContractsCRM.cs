namespace  Cuidador.Dto.CRM
{
	
	public class ContratoCRM
	{
		public int idContratoItem { get; set; }
		public DateTime horarioInicioPropuesto { get; set; }
		public DateTime horarioFinPropuesto { get; set; }
		public decimal importeTotal { get; set; }
		public int horasContratadas { get; set; }
		public string estatusContratoItem { get; set; }
		public string nombreCuidador { get; set; }
		public string nombreCliente { get; set; }
		public string avatarCuidador { get; set; }
		public string avatarCliente { get; set; }
	}
	
}