namespace Cuidador.Dto.Comentarios
{
	
	public class NuevoComentario
	{
		public int idPersonaReceptor {get; set;}
		public int idPersonaEmisor {get; set;}
		public short calificacion {get; set;}
		public string comentario {get; set;}
	}
	
}