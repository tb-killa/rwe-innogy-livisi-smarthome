using System.Collections.Generic;
using System.Net;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.PacketReceivers;

public interface IMulticastListener
{
	void SetAddresses(IList<IPAddress> addresses);
}
