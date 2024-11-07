namespace Cuidador.Dto.Horarios
{
	
	public class NuevoHorario
	{
		public int idUsuario {get; set;}
		public List<HorariosAdd> horarios {get; set;}
		
	}
	
	public class HorariosAdd
	{
		public string diaSemana {get; set;}
		public TimeOnly horaInicio {get; set;}
		public TimeOnly horaFin {get; set;}
		public decimal precioPorhora {get; set;}
	}
	
}