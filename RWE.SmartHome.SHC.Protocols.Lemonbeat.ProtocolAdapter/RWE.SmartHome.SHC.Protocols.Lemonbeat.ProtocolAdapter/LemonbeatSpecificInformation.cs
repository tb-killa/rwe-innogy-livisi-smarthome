using System.Collections.Generic;
using RWE.SmartHome.SHC.Lemonbeat.ProtocolAdapter.Persistence;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter;

public class LemonbeatSpecificInformation
{
	public DeviceInformationEntity DongleParameters { get; set; }

	public List<DeviceInformationEntity> Devices { get; set; }
}
