namespace Cuidador.Dto.CRM
{
	public class DashboardModel
	{
		public UserRegister userRegister { get; set; }
		public RechargeSald rechargeSald { get; set; }
		public MoneyPerContracts moneyPerContracts { get; set; }
		public NewContracts newContracts { get; set; }
		public List<MoneyContractsPerMonth> moneyContractsPerMonth { get; set; }
		public List<UsersTypeClient_Cuidador> usersTypeClient_Cuidador { get; set; }
		public List<NewContractsPerMonth> newContractsPerMonth { get; set; }
	}
	
	public class UserRegister
	{
		public int usuarios_registrados_hoy { get; set; }
		public int usuarios_registrados_anteriormente { get; set; }
	}
	
	public class RechargeSald
	{
		public decimal saldo_recargado_hoy { get; set; }
		public decimal saldo_recargado_anterior { get; set; }
	}
	
	public class MoneyPerContracts
	{
		public decimal dinero_generado_hoy { get; set; }
		public decimal dinero_generado_anterior { get; set; }
	}
	
	public class NewContracts 
	{
		public int contratos_hoy { get; set; }
		public int contratos_anteriores { get; set; }
	}
	
	public class MoneyContractsPerMonth
	{
		public int mes { get; set; }
		public decimal importeConcluidos { get; set; }
		public decimal importePendientes { get; set; }	
	}
	
	public class UsersTypeClient_Cuidador 
	{
		public int mes { get; set; }
		public decimal clientes { get; set; }
		public decimal cuidadores { get; set; }
	}
	
	public class NewContractsPerMonth
	{
		public int id_contratoitem { get; set; }
		public string cliente { get; set; }
		public decimal importe_total { get; set; }
		public string cuidador { get; set; }
		public string estatus { get; set; }
		public int horasContratadas { get; set; }
	}
	
}

