using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;
using RWE.SmartHome.SHC.SipCosProtocolAdapterInterfaces;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Configurations;

public class ShcActuatorConfiguration : ActuatorConfiguration
{
	private LinkPartner shcLinkPartner;

	public ShcActuatorConfiguration(LogicalDevice logicalDevice)
		: base(logicalDevice, null)
	{
	}

	public override void InitializeConfiguration(IList<byte[]> shcAddresses, IDictionary<Guid, SensorConfiguration> sensors, IDictionary<Guid, ActuatorConfiguration> actuators)
	{
		shcLinkPartner = new LinkPartner(Guid.Empty, shcAddresses[1], 63);
	}

	internal override IEnumerable<LinkPartner> CreateLinks(LinkPartner sensor, ActionDescription action, ProfileAction switchEvent, ProfileAction aboveEvent, ProfileAction belowEvent, int? comparisonValuePercent, Rule rule)
	{
		return new LinkPartner[1] { shcLinkPartner };
	}

	public override IEnumerable<int> GetUsedChannels(ActionDescription action)
	{
		return new int[0];
	}

	public override void SaveConfiguration(DeviceConfiguration configuration)
	{
	}
}
