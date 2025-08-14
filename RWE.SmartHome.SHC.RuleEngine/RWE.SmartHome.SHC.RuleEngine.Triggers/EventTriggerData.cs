using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

namespace RWE.SmartHome.SHC.RuleEngine.Triggers;

internal class EventTriggerData : IDeviceTriggerData, ITriggerData
{
	public Guid LogicalDeviceId { get; set; }

	public List<Property> EventDetails { get; set; }
}
