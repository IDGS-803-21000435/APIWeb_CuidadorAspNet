namespace Cuidador.Dto.User.RegistrarUsuario
{
    public class PersonaDTO
    {
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string CorreoElectronico { get; set; }
        public DateOnly FechaNacimiento { get; set; }
        public string Genero { get; set; }
        public string EstadoCivil { get; set; }
        public string RFC { get; set; }
        public string CURP { get; set; }
        public string TelefonoParticular { get; set; }
        public string TelefonoMovil { get; set; }
        public string TelefonoEmergencia { get; set; }
        public string NombreCompletoFamiliar { get; set; }
        public int DomicilioId { get; set; }
        public int DatosMedicosId { get; set; }
        public string AvatarImage { get; set; }
        public int EstatusId { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int UsuarioRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
        public int UsuarioModifico { get; set; }
    }
}
