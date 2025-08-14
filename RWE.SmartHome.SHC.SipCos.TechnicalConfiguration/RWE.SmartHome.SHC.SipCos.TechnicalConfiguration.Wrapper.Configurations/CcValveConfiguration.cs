using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Actuators;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.ErrorHandling;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Channels;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Links;
using RWE.SmartHome.SHC.SipCosProtocolAdapterInterfaces;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Configurations;

public class CcValveConfiguration : CcActuatorConfiguration
{
	private readonly ValveType? valveType;

	public CcValveChannel ValveChannel { get; private set; }

	public CcValveConfiguration(LogicalDevice logicalDevice, byte[] address)
		: base(logicalDevice, address)
	{
		if (!(logicalDevice is ValveActuator valveActuator))
		{
			throw new ArgumentException("LogicalDevice type doesn't match ValveConfiguration: " + base.LogicalDeviceContract.GetType().Name);
		}
		if (logicalDevice.BaseDevice.GetBuiltinDeviceDeviceType() == BuiltinPhysicalDeviceType.RST || logicalDevice.BaseDevice.GetBuiltinDeviceDeviceType() == BuiltinPhysicalDeviceType.RST2)
		{
			if (valveActuator.ValveIndex != 0)
			{
				throw new ArgumentException($"ValveActuator.ValveIndex must be zero for {logicalDevice.BaseDevice.GetBuiltinDeviceDeviceType()}!");
			}
			if (valveActuator.ValveType.HasValue)
			{
				throw new ArgumentException($"ValveActuator.ValveType must be null for {logicalDevice.BaseDevice.GetBuiltinDeviceDeviceType()}!");
			}
			ValveChannel = new CcValveChannel(8, 1);
			return;
		}
		if (valveActuator.ValveIndex == 0)
		{
			throw new ArgumentException("ValveActuator.ValveIndex can't be zero for FSC8!");
		}
		if (!valveActuator.ValveType.HasValue)
		{
			throw new ArgumentException("ValveActuator.ValveType cannot be null for FSC8!");
		}
		ValveChannel = new CcValveChannel((byte)valveActuator.ValveIndex, 1);
		valveType = valveActuator.ValveType;
		ValveChannel.ValveType = ValveType.NormalClose;
		ValveChannel.ControlMode = null;
	}

	public override void SaveConfiguration(DeviceConfiguration configuration)
	{
		IDictionary<byte, ConfigurationChannel> channels = configuration.Channels;
		if (base.LogicalDeviceContract.BaseDevice != null && base.LogicalDeviceContract.BaseDevice.GetBuiltinDeviceDeviceType() == BuiltinPhysicalDeviceType.FSC8)
		{
			if (!channels.ContainsKey(base.GlobalStatusInfo.ChannelIndex))
			{
				base.GlobalStatusInfo.SaveConfiguration(configuration.Channels);
			}
			ValveActuator valveActuator = base.LogicalDeviceContract as ValveActuator;
			ValveChannel.ValveType = valveActuator.ValveType ?? ValveType.NormalClose;
			ValveChannel.ControlMode = valveActuator.ControlMode;
			EnsureAllActuatorChannelsExist(channels);
		}
		ValveChannel.SaveConfiguration(channels);
	}

	public override IEnumerable<int> GetUsedChannels(ActionDescription action)
	{
		return new int[0];
	}

	internal override IEnumerable<LinkPartner> CreateLinks(LinkPartner sensor, ActionDescription action, ProfileAction switchEvent, ProfileAction aboveEvent, ProfileAction belowEvent, int? comparisonValuePercent, Rule rule)
	{
		IEnumerable<LinkPartner> result = new LinkPartner[0];
		if (action.ActionType == "ControlValve")
		{
			ValveChannel.AddLink(sensor, new BaseLink());
			result = new LinkPartner[1]
			{
				new LinkPartner(base.PhysicalDeviceId, base.PhysicalAddress, ValveChannel.ChannelIndex)
			};
		}
		return result;
	}

	internal override IEnumerable<LinkPartner> CreateInternalLinks(LinkPartner sensor, InternalLinkType linkType)
	{
		if (linkType != InternalLinkType.CcSensor)
		{
			throw new IncompatibleLinkTypeException();
		}
		ValveChannel.AddLink(sensor, new BaseLink());
		return new LinkPartner[1]
		{
			new LinkPartner(base.PhysicalDeviceId, base.PhysicalAddress, ValveChannel.ChannelIndex)
		};
	}

	public override void LinkWithOtherDevice(CcActuatorConfiguration otherDevice)
	{
	}

	public override void LinkWithMasterDevice(CcActuatorConfiguration masterDevice)
	{
	}

	private static void EnsureAllActuatorChannelsExist(IDictionary<byte, ConfigurationChannel> channels)
	{
		for (byte b = 1; b < 9; b++)
		{
			if (!channels.ContainsKey(b))
			{
				ConfigurationChannel configurationChannel = new ConfigurationChannel();
				configurationChannel.DefaultLink = new ConfigurationLink();
				ConfigurationChannel configurationChannel2 = configurationChannel;
				new BaseLink().SaveConfiguration(configurationChannel2.DefaultLink);
				channels.Add(b, configurationChannel2);
				CcValveChannel ccValveChannel = new CcValveChannel(b, 1);
				ccValveChannel.ValveType = ValveType.NormalClose;
				ccValveChannel.ControlMode = ControlMode.Heating;
				ccValveChannel.SaveConfiguration(channels);
			}
		}
	}
}
