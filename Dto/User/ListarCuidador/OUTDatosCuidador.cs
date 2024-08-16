using Cuidador.Dto.User.ListarDatoUsuario;
using Cuidador.Models;

namespace Cuidador.Dto.User.ListarCuidador
{
    public class OUTDatosCuidador
    {
        public OUTDomicilioDTO Domicilio { get; set; }
        public OUTDatosMedicosDTO DatosMedicos { get; set; }
        public List<OUTPadecimientosDTO> Padecimientos { get; set; }
        public OUTUsuarioDTO Usuario { get; set; }
        public OUTPersonaFisicaWebDTO Persona { get; set; }
        public List<OUTDocumentacionDTO> Documentacion { get; set; }
        public List<OUTCertificacionYDocumentoOUT> CertificacionesExperiencia { get; set; }
    }
}
