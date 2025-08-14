using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;
using RWE.SmartHome.SHC.SipCosProtocolAdapterInterfaces;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Configurations;

public abstract class ActuatorConfiguration : TechnicalConfigurationCreator
{
	public virtual int? MaxTimePointsPerWeekday => null;

	protected ActuatorConfiguration(LogicalDevice logicalDevice, byte[] address)
		: base(logicalDevice, address)
	{
	}

	public override void InitializeConfiguration(IList<byte[]> shcAddresses, IDictionary<Guid, SensorConfiguration> sensors, IDictionary<Guid, ActuatorConfiguration> actuators)
	{
		base.InitializeConfiguration(shcAddresses, sensors, actuators);
		base.GlobalStatusInfo.StatusInfoDestinationAddress = shcAddresses[0];
	}

	internal abstract IEnumerable<LinkPartner> CreateLinks(LinkPartner sensor, ActionDescription action, ProfileAction switchEvent, ProfileAction aboveEvent, ProfileAction belowEvent, int? comparisonValuePercent, Rule rule);

	internal virtual IEnumerable<LinkPartner> CreateInternalLinks(LinkPartner sensor, InternalLinkType linkType)
	{
		return new LinkPartner[0];
	}

	public virtual bool AddDeviceSetpoint(TimeSpan timeOfDay, byte weekdays, ActionDescription action)
	{
		return false;
	}

	public abstract IEnumerable<int> GetUsedChannels(ActionDescription action);

	protected int? GetActionParameterValue(ActionDescription action, string paramName)
	{
		Parameter parameter = action.Data.FirstOrDefault((Parameter p) => p.Name == paramName);
		if (parameter == null)
		{
			return null;
		}
		if (!(parameter.Value is ConstantNumericBinding constantNumericBinding))
		{
			return null;
		}
		return (int)constantNumericBinding.Value;
	}
}
