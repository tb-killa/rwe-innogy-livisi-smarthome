using System;
using System.Net;
using System.Xml.Serialization;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Interfaces;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Services;

public abstract class LemonbeatService<T> : ILemonbeatService
{
	private readonly XmlSerializer deserializer;

	private readonly XmlSerializerNamespaces namespaces;

	private readonly XmlSerializer serializer;

	private readonly ILemonbeatCommunication aggregator;

	public ServiceType ServiceType { get; private set; }

	protected LemonbeatService(ILemonbeatCommunication aggregator, ServiceType serviceId, string defaultNamespace, ServiceCommunicationType communicationType)
	{
		this.aggregator = aggregator;
		ServiceType = serviceId;
		namespaces = new XmlSerializerNamespaces();
		namespaces.Add("", defaultNamespace);
		deserializer = new XmlSerializer(typeof(T), defaultNamespace);
		serializer = new XmlSerializer(typeof(T));
		if (communicationType == ServiceCommunicationType.Bidirectional)
		{
			aggregator.MessageReceived += OnMessageReceived;
		}
	}

	private void OnMessageReceived(ServiceType service, DeviceIdentifier deviceIdentifier, string message)
	{
		if (ServiceType != service)
		{
			return;
		}
		Log.Debug(Module.LemonbeatProtocolAdapter, $"Incoming message received from device {deviceIdentifier}");
		try
		{
			T message2 = deserializer.Deserialize<T>(message);
			Handle(deviceIdentifier.GatewayId, deviceIdentifier.IPAddress, message2);
		}
		catch (Exception ex)
		{
			Log.Error(Module.LemonbeatProtocolAdapter, $"The received message from {deviceIdentifier} caused an error. Error: {ex.Message} Message: {message}");
		}
	}

	private T SendRequest(DeviceIdentifier deviceAddress, T request, RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.TransportType transportType)
	{
		string request2 = serializer.Serialize(request, namespaces);
		try
		{
			string text = aggregator.SendRequest(deviceAddress, ServiceType, request2, transportType);
			if (string.IsNullOrEmpty(text))
			{
				Log.Error(Module.LemonbeatProtocolAdapter, "SendRequest: Invalid response received");
				return default(T);
			}
			return deserializer.Deserialize<T>(text);
		}
		catch (Exception ex)
		{
			Log.Error(Module.LemonbeatProtocolAdapter, "Error during SendRequest: " + ex.ToString());
			throw;
		}
	}

	protected T SendRequest(DeviceIdentifier deviceAddress, T request)
	{
		return SendRequest(deviceAddress, request, RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.TransportType.Connection);
	}

	protected T SendRequestDatagram(DeviceIdentifier deviceAddress, T request)
	{
		return SendRequest(deviceAddress, request, RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.TransportType.Datagram);
	}

	protected void SendMessage(DeviceIdentifier deviceAddress, T message, RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.TransportType transportType)
	{
		string message2 = serializer.Serialize(message, namespaces);
		try
		{
			aggregator.SendMessage(deviceAddress, ServiceType, message2, transportType);
		}
		catch (Exception ex)
		{
			Log.Error(Module.LemonbeatProtocolAdapter, "Error during SendMessage: " + ex.ToString());
			throw;
		}
	}

	protected abstract void Handle(int gatewayId, IPAddress address, T message);
}
