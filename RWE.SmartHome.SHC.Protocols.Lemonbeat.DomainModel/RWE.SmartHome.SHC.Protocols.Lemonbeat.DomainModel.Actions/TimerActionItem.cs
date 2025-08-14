namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Actions;

public class TimerActionItem : ActionItem
{
	public TimerActionType ActionType { get; set; }

	public uint TimerId { get; set; }

	public override bool Equals(ActionItem other)
	{
		if (other is TimerActionItem timerActionItem && timerActionItem.TimerId == TimerId)
		{
			return timerActionItem.ActionType == ActionType;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return TimerId.GetHashCode();
	}
}
