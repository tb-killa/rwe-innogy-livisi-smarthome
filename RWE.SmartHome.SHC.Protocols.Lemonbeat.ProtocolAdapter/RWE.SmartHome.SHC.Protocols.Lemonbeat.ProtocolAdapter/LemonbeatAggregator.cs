using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces.Events;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Interfaces;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;
using SmartHome.SHC.API.Protocols.Lemonbeat.Gateway;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter;

internal class LemonbeatAggregator : ILemonbeatCommunication, IGatewayRegistrar
{
	internal class GatewayDescriptor
	{
		public int GatewayId;

		public ILemonbeatGateway Gateway;

		public EventHandler<MessageReceivedEventArgs> MessageReceivedHandler;

		public EventHandler<GatewayAvailabilityEventArgs> GatewayAvailabilityHandler;

		public bool Available;

		public GatewayDescriptor(ILemonbeatGateway gw)
		{
			Gateway = gw;
			GatewayId = gw.GatewayId;
			Available = true;
		}
	}

	internal readonly List<GatewayDescriptor> gatewayDescriptors;

	public event Action<ServiceType, DeviceIdentifier, string> MessageReceived;

	public event Action<int, bool> GatewayAvailabilityUpdated;

	public LemonbeatAggregator(IApplicationsHost appHost)
	{
		gatewayDescriptors = new List<GatewayDescriptor>(8);
		appHost.ApplicationStateChanged += OnApplicationActivated;
		appHost.ApplicationStateChanged += OnApplicationRemoved;
	}

	public void RegisterGateway(ILemonbeatGateway gateway)
	{
		if (gateway != null)
		{
			AddGateway(gateway);
		}
	}

	private void OnApplicationRemoved(ApplicationLoadStateChangedEventArgs obj)
	{
		if ((obj.ApplicationState == ApplicationStates.ApplicationDeactivated || obj.ApplicationState == ApplicationStates.ApplicationsUninstalled) && obj.Application is ILemonbeatGateway gateway)
		{
			RemoveGateway(gateway);
		}
	}

	private void RemoveGateway(ILemonbeatGateway gateway)
	{
		int gatewayId = gateway.GatewayId;
		GatewayDescriptor gatewayDescriptor;
		lock (gatewayDescriptors)
		{
			gatewayDescriptor = gatewayDescriptors.FirstOrDefault((GatewayDescriptor g) => g.GatewayId == gatewayId);
			if (gatewayDescriptor.Gateway != gateway)
			{
				Log.Debug(Module.LemonbeatProtocolAdapter, $"Trying to remove gateway with ID: {gateway.GatewayId} that was already upgraded");
				return;
			}
			gatewayDescriptors.Remove(gatewayDescriptor);
		}
		if (gatewayDescriptor != null)
		{
			gatewayDescriptor.Gateway.MessageReceived -= gatewayDescriptor.MessageReceivedHandler;
			gatewayDescriptor.Gateway.GatewayAvailabilityUpdated -= gatewayDescriptor.GatewayAvailabilityHandler;
			Log.Debug(Module.LemonbeatProtocolAdapter, $"Removed gateway with ID {gatewayId}.");
		}
		else
		{
			Log.Warning(Module.LemonbeatProtocolAdapter, $"Gateway with ID {gatewayId} was not included in the aggregator");
		}
	}

	private void OnApplicationActivated(ApplicationLoadStateChangedEventArgs obj)
	{
		if (obj.ApplicationState == ApplicationStates.ApplicationActivated && obj.Application is ILemonbeatGateway gateway)
		{
			AddGateway(gateway);
		}
	}

	private void AddGateway(ILemonbeatGateway gateway)
	{
		int gatewayId = gateway.GatewayId;
		GatewayDescriptor desc = new GatewayDescriptor(gateway);
		lock (gatewayDescriptors)
		{
			GatewayDescriptor gatewayDescriptor = gatewayDescriptors.FirstOrDefault((GatewayDescriptor g) => g.GatewayId == gatewayId);
			if (gatewayDescriptor != null)
			{
				Log.Warning(Module.LemonbeatProtocolAdapter, $"Gateway with ID {gateway.GatewayId} already exists in the system");
				RemoveGateway(gatewayDescriptor.Gateway);
			}
			gatewayDescriptors.Add(desc);
		}
		desc.MessageReceivedHandler = delegate(object obj, MessageReceivedEventArgs evArgs)
		{
			HandleOnMessageReceived(evArgs.ServiceId, evArgs.Address, gatewayId, evArgs.Message);
		};
		desc.Gateway.MessageReceived += desc.MessageReceivedHandler;
		desc.GatewayAvailabilityHandler = delegate(object obj, GatewayAvailabilityEventArgs evArgs)
		{
			desc.Available = evArgs.Available;
			if (this.GatewayAvailabilityUpdated != null)
			{
				this.GatewayAvailabilityUpdated(desc.GatewayId, evArgs.Available);
			}
		};
		desc.Gateway.GatewayAvailabilityUpdated += desc.GatewayAvailabilityHandler;
	}

	private void HandleOnMessageReceived(LemonbeatServiceId serviceId, IPAddress address, int gatewayId, string message)
	{
		this.MessageReceived?.Invoke(serviceId.ToServiceType(), ToDeviceIdentifier(address, gatewayId), message);
	}

	public void SendMessage(DeviceIdentifier destination, ServiceType serviceId, string message, RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.TransportType preferredTransportType)
	{
		GatewayDescriptor gatewayDescriptor;
		lock (gatewayDescriptors)
		{
			gatewayDescriptor = gatewayDescriptors.FirstOrDefault((GatewayDescriptor g) => g.GatewayId == destination.GatewayId);
		}
		if (gatewayDescriptor != null && (gatewayDescriptor.Available || serviceId == ServiceType.NetworkManagement || serviceId == ServiceType.Value))
		{
			gatewayDescriptor.Gateway.SendMessage(destination.IPAddress, serviceId.ToLemonbeatServiceId(), message, preferredTransportType.ToApiTransport());
		}
	}

	public string SendRequest(DeviceIdentifier destination, ServiceType serviceId, string request, RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.TransportType preferredTransportType)
	{
		GatewayDescriptor gatewayDescriptor;
		lock (gatewayDescriptors)
		{
			gatewayDescriptor = gatewayDescriptors.FirstOrDefault((GatewayDescriptor g) => g.GatewayId == destination.GatewayId);
		}
		if (gatewayDescriptor == null || (!gatewayDescriptor.Available && serviceId != ServiceType.DeviceDescription))
		{
			return string.Empty;
		}
		return gatewayDescriptor.Gateway.SendRequest(destination.IPAddress, serviceId.ToLemonbeatServiceId(), request, preferredTransportType.ToApiTransport());
	}

	public ReachabilityState Ping(DeviceIdentifier destination)
	{
		GatewayDescriptor gatewayDescriptor;
		lock (gatewayDescriptors)
		{
			gatewayDescriptor = gatewayDescriptors.FirstOrDefault((GatewayDescriptor g) => g.GatewayId == destination.GatewayId);
		}
		if (gatewayDescriptor == null || !gatewayDescriptor.Available)
		{
			return ReachabilityState.GatewayUnavailable;
		}
		if (!gatewayDescriptor.Gateway.Ping(destination.IPAddress))
		{
			return ReachabilityState.Unreachable;
		}
		return ReachabilityState.Reachable;
	}

	public void SetMulticastSubscriptions(List<DeviceIdentifier> subscriptions)
	{
		List<GatewayDescriptor> list;
		lock (gatewayDescriptors)
		{
			list = gatewayDescriptors.Where((GatewayDescriptor gwDesc) => subscriptions.Any((DeviceIdentifier di) => di.GatewayId == gwDesc.GatewayId)).ToList();
		}
		list.ForEach(delegate(GatewayDescriptor desc)
		{
			desc.Gateway.SetMulticastSubscriptions(from di in subscriptions
				where di.GatewayId == desc.GatewayId
				select di.IPAddress);
		});
	}

	private static DeviceIdentifier ToDeviceIdentifier(IPAddress address, int gatewayId)
	{
		return new DeviceIdentifier(address, null, gatewayId);
	}
}
