namespace Cuidador.Dto.Horarios
{
	
	public class UpdateHorario
	{
		public int idSueldoNivel { get; set; }
		public string diaSemana {get; set;}
		public TimeOnly horaInicio {get; set;}
		public TimeOnly horaFin {get; set;}
		public decimal precioPorhora {get; set;}
		public byte estatus {get; set;}
		
	}

}