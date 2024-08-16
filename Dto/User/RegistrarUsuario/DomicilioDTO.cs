namespace Cuidador.Dto.User.RegistrarUsuario
{
    public class DomicilioDTO
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
}
