using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

namespace RWE.SmartHome.SHC.RuleEngine.Triggers;

public class EventContext
{
	public List<Property> CurrentStateProperties { get; set; }

	public List<Property> OldStateProperties { get; set; }
}
