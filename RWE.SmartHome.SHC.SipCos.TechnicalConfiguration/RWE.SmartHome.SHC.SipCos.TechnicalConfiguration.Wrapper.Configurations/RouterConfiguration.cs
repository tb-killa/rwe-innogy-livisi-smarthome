using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Configurations;

public class RouterConfiguration : SensorConfiguration
{
	private bool routerInitialConfPerformed;

	public RouterConfiguration(LogicalDevice logicalDevice, byte[] address)
		: base(logicalDevice, address)
	{
		routerInitialConfPerformed = false;
	}

	public override void SaveConfiguration(DeviceConfiguration configuration)
	{
		base.GlobalStatusInfo.SaveConfiguration(configuration.Channels);
		if (!routerInitialConfPerformed)
		{
			return;
		}
		if (configuration.Channels.TryGetValue(0, out var value))
		{
			if (value.Links.TryGetValue(LinkPartner.Empty, out var value2))
			{
				ParameterList parameterList = value2[11];
				parameterList[0] = 19;
				parameterList[1] = byte.MaxValue;
				parameterList[2] = byte.MaxValue;
				parameterList[3] = byte.MaxValue;
				ParameterList parameterList2 = value2[12];
				parameterList2[0] = 22;
				parameterList2[1] = byte.MaxValue;
				parameterList2[2] = byte.MaxValue;
				parameterList2[3] = byte.MaxValue;
				ParameterList parameterList3 = value2[13];
				parameterList3[0] = 23;
				parameterList3[1] = byte.MaxValue;
				parameterList3[2] = byte.MaxValue;
				parameterList3[3] = byte.MaxValue;
			}
			else
			{
				Log.Error(Module.TechnicalConfiguration, $"No valid link defined for the global configuration channel of the router with address: {base.PhysicalAddress.ToReadable()}");
			}
		}
		else
		{
			Log.Error(Module.TechnicalConfiguration, $"The global configuration channel is not defined for the router with address: {base.PhysicalAddress.ToReadable()}");
		}
	}

	public override bool CreateLinks(Trigger trigger, ActionDescription action, ActuatorConfiguration actuatorConfiguration, Rule rule)
	{
		return true;
	}

	public override IEnumerable<int> GetUsedChannels(Trigger trigger)
	{
		return new int[0];
	}

	public override void InitializeConfiguration(IList<byte[]> shcAddresses, IDictionary<Guid, SensorConfiguration> sensors, IDictionary<Guid, ActuatorConfiguration> actuators)
	{
		base.InitializeConfiguration(shcAddresses, sensors, actuators);
		base.GlobalStatusInfo.DutyLimit = 0;
		routerInitialConfPerformed = true;
	}
}
