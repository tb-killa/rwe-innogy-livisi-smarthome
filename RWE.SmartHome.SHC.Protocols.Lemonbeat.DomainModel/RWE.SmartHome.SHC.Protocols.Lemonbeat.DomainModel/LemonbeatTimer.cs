namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

public class LemonbeatTimer : ConfigurationItem
{
	public uint Delay { get; set; }

	public uint? CalculationId { get; set; }

	public uint? ActionId { get; set; }

	public override bool Equals(ConfigurationItem other)
	{
		if (other is LemonbeatTimer lemonbeatTimer && base.Id == lemonbeatTimer.Id && Delay == lemonbeatTimer.Delay && CalculationId == lemonbeatTimer.CalculationId)
		{
			return ActionId == lemonbeatTimer.ActionId;
		}
		return false;
	}
}
