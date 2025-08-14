using System.Collections.Generic;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Actions;

public class LemonbeatAction : ConfigurationItem
{
	public List<ActionItem> Items { get; set; }

	public LemonbeatAction()
	{
		Items = new List<ActionItem>();
	}

	public override bool Equals(ConfigurationItem other)
	{
		if (other is LemonbeatAction lemonbeatAction && lemonbeatAction.Id == base.Id)
		{
			return Items.Compare(lemonbeatAction.Items);
		}
		return false;
	}
}
