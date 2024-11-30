namespace Cuidador.Dto.CRM
{
	
	public class AllUsers
	{
		public UserCRM user { get; set; }
		public PersonaCRM persona { get; set; }
		public DatosMedicos datosMedicos { get; set; }
	}
	
	public class UserCRM
	{
		public int id_usuario { get; set; }
		public string usuario { get; set; }
		public string contrasenia { get; set; }
		public string tipoUsuario { get; set; }
		public string estatusUsuario { get; set; }
		public string nivelUsuario { get; set; }
		public DateTime fecha_registro { get; set; }
		public string avatar_image { get; set; }
	}
	
	public class PersonaCRM
	{
		public int id_persona { get; set; }
		public string nombre { get; set; }
		public string apellido_paterno { get; set; }
		public string apellido_materno { get; set; }
		public DateTime fecha_nacimiento { get; set; }
		public string avatar_image { get; set; }
		public string correo_electronico { get; set; }
		public string genero { get; set; }
		public string estado_civil { get; set; }
		public string rfc { get; set; }
		public string curp { get; set; }
		public string telefono_particular { get; set; }
		public string telefono_movil { get; set; }
		public int id_domicilio { get; set; }
		public string calle { get; set; }
		public string colonia { get; set; }
		public string numero_interior { get; set; }
		public string numero_exterior { get; set; }
		public string ciudad { get; set; }
		public string estado { get; set; }
		public string pais { get; set; }
		public string estatusPersona { get; set; }
		public int esFamiliar 	{ get; set; }
	}
	
	public class Documentos 
	{
		public int idDocumentacion { get; set; }
		public string tipoDocumento { get; set; }
		public string nombreDocumento { get; set; }
		public string urlDocumento { get; set; }
		public string fechaEmision { get; set; }
		public string fechaExpiracion { get; set; }
		public int version { get; set; }
		public string estatusDocumento { get; set; }
	}
	
	public class DatosMedicos
	{
		public int idDatosmedicos { get; set; }
		public string antecedentesMedicos { get; set; }
		public string alergias { get; set; }
		public string tipoSanguineo { get; set; }
		public string nombreMedicofamiliar { get; set; }
		public string telefonoMedicofamiliar { get; set; }
		public string observaciones { get; set; }
		public List<Padecimientos> padecimientos { get; set; }
	}
	
	public class Padecimientos 
	{
		public int idPadecimiento { get; set; }
		public string nombre { get; set; }
		public string descripcion { get; set; }
		public string padeceDesde { get; set; }
	}
	
}