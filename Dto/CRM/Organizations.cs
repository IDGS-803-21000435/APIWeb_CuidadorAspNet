namespace Cuidador.Dto.CRM
{
	public class Organizations
	{
		public int id_personamoral { get; set; }
		public string razon_social { get; set; }
		public string nombre_comercial { get; set; }
		public string rfc { get; set; }
		public string telefono { get; set; }
		public string correo_electronico { get; set; }
		public int id_domicilio { get; set; }
		public string calle { get; set; }
		public string colonia { get; set; }
		public string numero_interior { get; set; }
		public string numero_exterior { get; set; }
		public string ciudad { get; set; }
		public string estado { get; set; }
		public string pais { get; set; }
		public string referenciasDomicilio { get; set; }
	}
}