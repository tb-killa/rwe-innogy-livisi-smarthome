using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Sensors;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Configurations;

public class LuminanceSensorConfiguration : IndirectSensorConfiguration
{
	public LuminanceSensorConfiguration(LogicalDevice logicalDevice, byte[] address)
		: base(logicalDevice, address)
	{
		if (!(logicalDevice is LuminanceSensor))
		{
			throw new ArgumentException("LogicalDevice type doesn't match LuminanceSensorConfiguration: " + logicalDevice.GetType().Name);
		}
	}
}
