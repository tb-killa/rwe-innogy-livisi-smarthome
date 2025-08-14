using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Configurations;

public class AlarmActuatorConfiguration : ShcActuatorConfiguration
{
	public AlarmActuatorConfiguration(LogicalDevice logicalDevice)
		: base(logicalDevice)
	{
	}

	public override IEnumerable<int> GetUsedChannels(ActionDescription action)
	{
		return new int[1] { 1 };
	}
}
