using System;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;
using SipcosCommandHandler;

namespace RWE.SmartHome.SHC.DeviceManager;

public static class ProtocolTypeExtension
{
	public static ProtocolType ToProtocolType(this DeviceInfoProtocolType me)
	{
		return me switch
		{
			DeviceInfoProtocolType.SIPcos => ProtocolType.SipCos, 
			DeviceInfoProtocolType.BIDcos => ProtocolType.BidCos, 
			_ => throw new ArgumentOutOfRangeException("protocolType"), 
		};
	}
}
