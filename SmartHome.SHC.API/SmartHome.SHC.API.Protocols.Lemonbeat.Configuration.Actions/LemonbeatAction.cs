using System.Collections.Generic;

namespace SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.Actions;

public class LemonbeatAction
{
	public uint ActionId { get; set; }

	public List<ActionItem> Items { get; set; }

	public LemonbeatAction(uint actionId, List<ActionItem> items)
	{
		ActionId = actionId;
		Items = items;
	}

	public LemonbeatAction()
	{
		Items = new List<ActionItem>();
	}
}
