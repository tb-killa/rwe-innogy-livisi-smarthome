using System;
using System.Collections.Generic;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Interfaces;

public interface ILemonbeatCommunication
{
	event Action<ServiceType, DeviceIdentifier, string> MessageReceived;

	event Action<int, bool> GatewayAvailabilityUpdated;

	void SendMessage(DeviceIdentifier destination, ServiceType serviceId, string message, TransportType preferredTransportType);

	string SendRequest(DeviceIdentifier destination, ServiceType serviceId, string request, TransportType preferredTransportType);

	ReachabilityState Ping(DeviceIdentifier destination);

	void SetMulticastSubscriptions(List<DeviceIdentifier> subscriptions);
}
