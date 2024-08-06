﻿namespace Cuidador.Dto.User.RegistrarUsuario
{
    public class DocumentacionDTO
    {
        public int PersonaId { get; set; }
        public string TipoDocumento { get; set; }
        public string NombreDocumento { get; set; }
        public string UrlDocumento { get; set; }
        public DateOnly FechaEmision { get; set; }
        public DateOnly FechaExpiracion { get; set; }
        public int Version { get; set; }
        public int EstatusId { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int UsuarioRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
        public int UsuarioModifico { get; set; }
    }
}