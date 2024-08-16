using Cuidador.Dto.User.RegistrarUsuario;

namespace Cuidador.Dto.User.RegistrarFamiliar
{
    public class RegistrarFamiliar
    {
        public DomicilioDTO domicilio { get; set; }
        public List<PadecimientoDTO> padecimientos { get; set; }
        public DatosMedicosDTO datos_medicos { get; set; }        
        public UsuarioDTO usuario { get; set; }
        public PersonaDTO persona { get; set; }
        public List<DocumentacionDTO> documentacion { get; set; }
    }
}
