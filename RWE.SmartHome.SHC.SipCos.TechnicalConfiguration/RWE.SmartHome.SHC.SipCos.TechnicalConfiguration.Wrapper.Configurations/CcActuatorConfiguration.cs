using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Configurations;

public abstract class CcActuatorConfiguration : ActuatorConfiguration
{
	protected CcActuatorConfiguration(LogicalDevice logicalDevice, byte[] address)
		: base(logicalDevice, address)
	{
	}

	public abstract void LinkWithMasterDevice(CcActuatorConfiguration masterDevice);

	public abstract void LinkWithOtherDevice(CcActuatorConfiguration otherDevice);

	public void LinkWithOtherDevices(IEnumerable<CcActuatorConfiguration> otherDevices)
	{
		foreach (CcActuatorConfiguration otherDevice in otherDevices)
		{
			LinkWithOtherDevice(otherDevice);
		}
	}
}
