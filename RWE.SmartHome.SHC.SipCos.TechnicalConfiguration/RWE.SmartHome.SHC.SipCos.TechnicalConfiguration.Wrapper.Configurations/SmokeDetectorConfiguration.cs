using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Configurations;

public class SmokeDetectorConfiguration : SensorConfiguration
{
	public SmokeDetectorConfiguration(LogicalDevice logicalDevice, byte[] address)
		: base(logicalDevice, address)
	{
	}

	public override bool CreateLinks(Trigger trigger, ActionDescription action, ActuatorConfiguration actuatorConfiguration, Rule rule)
	{
		return true;
	}

	public override IEnumerable<int> GetUsedChannels(Trigger trigger)
	{
		return new int[0];
	}

	public override void SaveConfiguration(DeviceConfiguration configuration)
	{
	}
}
