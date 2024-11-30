namespace Cuidador.Dto.User.RegistrarAdulto
{
    public class DomicilioDTORA
    {
        public string Calle { get; set; }
        public string Colonia { get; set; }
        public string NumeroInterior { get; set; }
        public string NumeroExterior { get; set; }
        public string Ciudad { get; set; }
        public string Estado { get; set; }
        public string Pais { get; set; }
        public string Referencias { get; set; }
        public int EstatusId { get; set; }
        public int UsuarioRegistro { get; set; }
    }

    public class DatosMedicosDTORA
    {
        public string AntecedentesMedicos { get; set; }
        public string Alergias { get; set; }
        public string TipoSanguineo { get; set; }
        public string NombreMedicoFamiliar { get; set; }
        public string TelefonoMedicoFamiliar { get; set; }
        public string Observaciones { get; set; }
    }

    public class PadecimientoDTORA
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateOnly PadeceDesde { get; set; }
    }

    public class PersonaDTORA
    {
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }

        public string NombreCompletoFamiliar { get; set; }
        public string CorreoElectronico { get; set; }
        public DateOnly FechaNacimiento { get; set; }
        public string Genero { get; set; }
        public string EstadoCivil { get; set; }
        public string Rfc { get; set; }
        public string Curp { get; set; }
        public string TelefonoMovil { get; set; }
        public string AvatarImage { get; set; }
        public int EstatusId { get; set; }
        public int UsuarioId { get; set; }
        public int EsFamiliar { get; set; }
    }

    public class DocumentacionDTORA
    {
        public string TipoDocumento { get; set; }
        public string NombreDocumento { get; set; }
        public string UrlDocumento { get; set; }
        public DateOnly FechaEmision { get; set; }
        public DateOnly FechaExpiracion { get; set; }
        public int Version { get; set; }
        public int EstatusId { get; set; }
    }

    public class RequestRegAdultoDTORA
    {
        public int idUsuario { get; set; }
        public DomicilioDTORA Domicilio { get; set; }
        public DatosMedicosDTORA DatosMedicos { get; set; }
        public List<PadecimientoDTORA> Padecimientos { get; set; }
        public PersonaDTORA Persona { get; set; }
        public List<DocumentacionDTORA> Documentacion { get; set; }
    }
}
