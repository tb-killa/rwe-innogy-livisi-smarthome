using System;
using System.Net;

namespace SmartHome.SHC.API.Protocols.Lemonbeat.Gateway;

public class MessageReceivedEventArgs : EventArgs
{
	public LemonbeatServiceId ServiceId { get; private set; }

	public IPAddress Address { get; private set; }

	public string Message { get; private set; }

	public MessageReceivedEventArgs(LemonbeatServiceId serviceId, IPAddress address, string message)
	{
		ServiceId = serviceId;
		Address = address;
		Message = message;
	}
}
