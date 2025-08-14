using System.Collections.Generic;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;

public class DeviceConfigurationDiff
{
	public Dictionary<byte, ConfigurationChannelDiff> ChannelDiffs { get; private set; }

	public DeviceConfigurationDiff()
	{
		ChannelDiffs = new Dictionary<byte, ConfigurationChannelDiff>();
	}
}
