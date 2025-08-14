using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Configurations;

public abstract class IndirectSensorConfiguration : SensorConfiguration
{
	private readonly IDictionary<Guid, List<int>> linkPartners = new Dictionary<Guid, List<int>>();

	protected IndirectSensorConfiguration(LogicalDevice logicalDevice, byte[] address)
		: base(logicalDevice, address)
	{
	}

	public override bool CreateLinks(Trigger trigger, ActionDescription action, ActuatorConfiguration actuatorConfiguration, Rule rule)
	{
		if (action == null)
		{
			return true;
		}
		List<int> list = actuatorConfiguration.GetUsedChannels(action).ToList();
		if (linkPartners.TryGetValue(new Guid(action.Target.EntityId), out var value))
		{
			foreach (int item in list)
			{
				if (value.Contains(item))
				{
					throw ConversionHelpers.GetDuplicateSensorActuatorCombinationException(trigger.Entity.EntityIdAsGuid().ToString("N"), action.Target.EntityIdAsGuid().ToString("N"));
				}
				value.Add(item);
			}
		}
		else
		{
			linkPartners.Add(new Guid(action.Target.EntityId), list);
		}
		return true;
	}

	public override void SaveConfiguration(DeviceConfiguration configuration)
	{
	}

	public override IEnumerable<int> GetUsedChannels(Trigger trigger)
	{
		return new int[0];
	}
}
