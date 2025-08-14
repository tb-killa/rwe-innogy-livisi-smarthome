namespace SmartHome.SHC.API.Protocols.Lemonbeat.Configuration;

public class Transaction
{
	public uint? CalculationId { get; set; }

	public uint? ActionId { get; set; }

	public uint? GotoStateId { get; set; }

	public Transaction()
	{
	}

	public Transaction(uint? calculationId, uint? actionId, uint? gotoStateId)
	{
		CalculationId = calculationId;
		ActionId = actionId;
		GotoStateId = gotoStateId;
	}
}
