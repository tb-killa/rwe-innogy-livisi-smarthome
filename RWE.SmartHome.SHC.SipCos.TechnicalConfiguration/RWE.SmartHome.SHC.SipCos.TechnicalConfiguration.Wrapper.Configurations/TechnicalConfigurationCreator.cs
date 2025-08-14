using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Channels;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Configurations;

public abstract class TechnicalConfigurationCreator
{
	public Guid PhysicalDeviceId { get; private set; }

	public byte[] PhysicalAddress { get; private set; }

	public LogicalDevice LogicalDeviceContract { get; private set; }

	public GlobalStatusInfoChannel GlobalStatusInfo { get; private set; }

	public IRepository ConfigurationRepository { get; set; }

	protected TechnicalConfigurationCreator(LogicalDevice logicalDevice, byte[] address)
	{
		GlobalStatusInfo = new GlobalStatusInfoChannel(0);
		PhysicalDeviceId = ((logicalDevice == null || logicalDevice.BaseDevice == null) ? Guid.Empty : logicalDevice.BaseDevice.Id);
		PhysicalAddress = address;
		LogicalDeviceContract = logicalDevice;
	}

	public virtual void InitializeConfiguration(IList<byte[]> shcAddresses, IDictionary<Guid, SensorConfiguration> sensors, IDictionary<Guid, ActuatorConfiguration> actuators)
	{
	}

	public abstract void SaveConfiguration(DeviceConfiguration configuration);
}
