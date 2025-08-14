using System;
using System.Collections.Generic;
using System.Net;

namespace SmartHome.SHC.API.Protocols.Lemonbeat.Gateway;

public interface ILemonbeatGateway
{
	int GatewayId { get; }

	event EventHandler<MessageReceivedEventArgs> MessageReceived;

	event EventHandler<GatewayAvailabilityEventArgs> GatewayAvailabilityUpdated;

	void SendMessage(IPAddress destination, LemonbeatServiceId serviceId, string message, Transport preferredTransportType);

	string SendRequest(IPAddress destination, LemonbeatServiceId serviceId, string request, Transport preferredTransportType);

	bool Ping(IPAddress destination);

	void SetMulticastSubscriptions(IEnumerable<IPAddress> subscriptions);
}
