using System;

namespace RWE.SmartHome.SHC.RuleEngine.Triggers;

internal interface IDeviceTriggerData : ITriggerData
{
	Guid LogicalDeviceId { get; set; }
}
