using System.Collections.Generic;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Persistence;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter;

public class SipCosSpecificInformation
{
	public SIPCosNetworkParameter NetworkParameter { get; set; }

	public List<DeviceInformationEntity> Entities { get; set; }
}
