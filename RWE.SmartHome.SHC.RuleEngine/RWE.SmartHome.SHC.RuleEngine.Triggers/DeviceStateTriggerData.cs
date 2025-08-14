using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;

namespace RWE.SmartHome.SHC.RuleEngine.Triggers;

public class DeviceStateTriggerData : IDeviceTriggerData, ITriggerData
{
	public Guid LogicalDeviceId { get; set; }

	public LogicalDeviceState NewState { get; set; }

	public LogicalDeviceState OldState { get; set; }

	public List<Property> GetChangedProperties()
	{
		List<Property> list = new List<Property>();
		if (OldState == null)
		{
			if (NewState != null)
			{
				return (from p in NewState.GetProperties()
					where p.GetValueAsComparable() != null
					select p).ToList();
			}
			return list;
		}
		if (NewState == null)
		{
			return OldState.GetProperties();
		}
		foreach (Property property in NewState.GetProperties())
		{
			if (!OldState.GetProperties().Contains(property))
			{
				list.Add(property);
			}
		}
		return list;
	}
}
