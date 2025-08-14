using System;
using System.Collections.Generic;

namespace SmartHome.SHC.API.Configuration;

public class TargetLogicalConfiguration
{
	public IEnumerable<Capability> Capabilities { get; set; }

	public Device Device { get; set; }

	public IEnumerable<ActionDescription> ActionDescriptions { get; set; }

	public IEnumerable<Trigger> Triggers { get; set; }

	public Dictionary<Guid, List<TimeInteractionSetPoint>> TimeInteractionSetpoints { get; set; }
}
