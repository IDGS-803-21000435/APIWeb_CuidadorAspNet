namespace Cuidador.Dto.CRM
{
	public class PendingUsers
	{
		public int idUsuario { get; set; }
		public string nivelUsuario { get; set; }
		public string tipoUsuario { get; set; }
		public string estatusUsuario { get; set; }	
		public string usuario { get; set; }
		public string contrasenia { get; set; }
		public List<PendingPerson> personaFisica { get; set; }
	}
	
	public class PendingPerson
	{
		public int idPersona { get; set; }
		public string nombre { get; set; }
		public string apellidoPaterno { get; set; }
		public string apellidoMaterno { get; set; }
		public DateTime fechaNacimiento { get; set; }
		public string correoElectronico { get; set; }
		public string genero { get; set; }
		public string estadoCivil { get; set; }
		public string rfc { get; set; }
		public string curp { get; set; }
		public string telefonoParticular { get; set; }
		public string telefonoMovil { get; set; }
		public string telefonoEmergencia { get; set; }
		public string nombreCompletoFamiliar { get; set; }
		public int esFamiliar { get; set; }
	}
}