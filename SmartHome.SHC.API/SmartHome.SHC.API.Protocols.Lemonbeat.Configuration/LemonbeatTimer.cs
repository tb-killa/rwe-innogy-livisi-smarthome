namespace SmartHome.SHC.API.Protocols.Lemonbeat.Configuration;

public class LemonbeatTimer
{
	public uint TimerId { get; set; }

	public uint Delay { get; set; }

	public uint? CalculationId { get; set; }

	public uint? ActionId { get; set; }

	public LemonbeatTimer()
	{
	}

	public LemonbeatTimer(uint timerId, uint delay, uint? calculationId, uint? actionId)
	{
		TimerId = timerId;
		Delay = delay;
		CalculationId = calculationId;
		ActionId = actionId;
	}
}
