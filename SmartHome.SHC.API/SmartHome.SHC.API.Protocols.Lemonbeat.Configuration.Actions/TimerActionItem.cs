namespace SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.Actions;

public class TimerActionItem : ActionItem
{
	public TimerActionType ActionType { get; set; }

	public uint TimerId { get; set; }
}
