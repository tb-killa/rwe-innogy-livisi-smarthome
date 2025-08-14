using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceMonitoring;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.PacketReceivers;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.PacketSender;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Services;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Events;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.HardwareSupport;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Interfaces;
using RWE.SmartHome.SHC.StartupLogicInterfaces.Events;
using SmartHome.SHC.API.Protocols.Lemonbeat;
using SmartHome.SHC.API.Protocols.Lemonbeat.Gateway;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter;

internal class LemonbeatUsbDongle : ILemonbeatGateway
{
	public static int LemonbeatUSBDongleGatewayID = 1;

	private readonly IExiCompressor exiCompressor;

	private bool dongleConnected;

	private readonly IPacketSender tcpPacketSender;

	private readonly IPacketSender udpPacketSender;

	private readonly UdpPacketReceiver valueReceiver;

	private readonly List<UdpPacketReceiver> packetReceivers;

	private readonly RasConnectionManager connectionManager;

	private readonly IEventManager eventManager;

	private readonly IInclusionController deviceInclusionController;

	private readonly IDeviceList deviceList;

	private readonly DriverSupport hardwareSupport;

	private readonly IPv6InterfaceManagement ipv6InterfaceManagement = new IPv6InterfaceManagement();

	public int GatewayId => LemonbeatUSBDongleGatewayID;

	public event EventHandler<GatewayAvailabilityEventArgs> GatewayAvailabilityUpdated;

	public event EventHandler<MessageReceivedEventArgs> MessageReceived;

	public LemonbeatUsbDongle(IEventManager eventManager, IExiCompressor compressor, IDeviceMonitor deviceMonitor, IInclusionController deviceInclusionController, IDeviceList deviceList, RasConnectionManager connectionManager)
	{
		this.eventManager = eventManager;
		exiCompressor = compressor;
		this.deviceInclusionController = deviceInclusionController;
		this.deviceList = deviceList;
		hardwareSupport = new DriverSupport();
		this.connectionManager = connectionManager;
		tcpPacketSender = new TcpPacketSender
		{
			IsMessageComplete = MessageCheck.IsComplete
		};
		udpPacketSender = new UdpPacketSender
		{
			IsMessageComplete = MessageCheck.IsComplete
		};
		packetReceivers = new List<UdpPacketReceiver>(4);
		AddPacketReceiver(LemonbeatServicePort.Value, LemonbeatServiceId.Value);
		AddPacketReceiver(LemonbeatServicePort.DeviceDescription, LemonbeatServiceId.DeviceDescription);
		AddPacketReceiver(LemonbeatServicePort.Status, LemonbeatServiceId.Status);
		valueReceiver = packetReceivers.First();
		deviceMonitor.DeviceConnected += DeviceMonitorDeviceConnected;
		eventManager.GetEvent<ShcStartupCompletedEvent>().Subscribe(StartupCompleted, null, ThreadOption.PublisherThread, null);
		deviceInclusionController.DongleInitialized += DongleInitializedHandler;
	}

	private void StartupCompleted(ShcStartupCompletedEventArgs eventArgs)
	{
		if (eventArgs.Progress == StartupProgress.CompletedRound1)
		{
			FireGatewayAvailabilityUpdated(available: false);
			new Thread((ThreadStart)delegate
			{
				Start(TimeSpan.FromSeconds(5.0));
			}).Start();
		}
	}

	private void DeviceMonitorDeviceConnected(BasicDriverInformation basicDriverInformation)
	{
		try
		{
			if (!hardwareSupport.IsSupportedDevice(basicDriverInformation.Key))
			{
				return;
			}
			if (basicDriverInformation.Active)
			{
				new Thread((ThreadStart)delegate
				{
					Start(TimeSpan.FromSeconds(15.0));
				}).Start();
			}
			else
			{
				Stop();
			}
		}
		catch (Exception arg)
		{
			Log.Information(Module.LemonbeatProtocolAdapter, $"Failed to start Lemonbeat services {arg}");
		}
	}

	private void Stop()
	{
		dongleConnected = false;
		RaiseUsbDeviceConnectionChangedEvent(connected: false);
		AlertDongleFailure(toSet: false);
		try
		{
			packetReceivers.ForEach(delegate(UdpPacketReceiver receiver)
			{
				receiver.Stop();
			});
		}
		catch (Exception ex)
		{
			Log.Error(Module.LemonbeatProtocolAdapter, "Unable to close the packet receivers: " + ex.ToString());
		}
		FireGatewayAvailabilityUpdated(available: false);
	}

	private void Start(TimeSpan initialTimeOut)
	{
		Thread.Sleep((int)initialTimeOut.TotalMilliseconds);
		int num = 0;
		while (!dongleConnected && num < 5)
		{
			num++;
			Log.Debug(Module.LemonbeatProtocolAdapter, "Trying to initialize dongle " + num + " of 5");
			try
			{
				if (!connectionManager.IsDongleConnected())
				{
					break;
				}
				connectionManager.Connect();
				tcpPacketSender.LocalEndpoint = new IPEndPoint(connectionManager.LocalAddress, 0);
				udpPacketSender.LocalEndpoint = new IPEndPoint(connectionManager.LocalAddress, 0);
				if (connectionManager.IsConnected && deviceInclusionController.InitializeDongle(connectionManager.PeerAddress))
				{
					RaiseUsbDeviceConnectionChangedEvent(connected: true);
					AlertDongleFailure(toSet: false);
					dongleConnected = true;
					packetReceivers.ForEach(delegate(UdpPacketReceiver receiver)
					{
						receiver.Start();
					});
					ipv6InterfaceManagement.DeleteEmacb1Interface();
				}
			}
			catch (Exception ex)
			{
				Log.Information(Module.LemonbeatProtocolAdapter, $"Failed to start Lemonbeat services {ex.Message}");
				Log.Debug(Module.LemonbeatProtocolAdapter, ex.ToString());
				AlertDongleFailure(toSet: true);
			}
			if (!dongleConnected)
			{
				Thread.Sleep(5000);
			}
		}
	}

	private void DongleInitializedHandler()
	{
		try
		{
			FireGatewayAvailabilityUpdated(available: true);
		}
		catch (Exception ex)
		{
			Log.Warning(Module.LemonbeatProtocolAdapter, "Failed during GatewayAvailabilityUpdate: " + ex);
		}
	}

	private void FireGatewayAvailabilityUpdated(bool available)
	{
		EventHandler<GatewayAvailabilityEventArgs> gatewayAvailabilityUpdated = this.GatewayAvailabilityUpdated;
		if (gatewayAvailabilityUpdated == null)
		{
			return;
		}
		try
		{
			gatewayAvailabilityUpdated(this, new GatewayAvailabilityEventArgs(available));
			if (available)
			{
				eventManager.GetEvent<LemonbeatCoreGatewayReadyEvent>().Publish(new LemonbeatCoreGatewayReadyEventArgs());
			}
		}
		catch (Exception ex)
		{
			Log.Error(Module.LemonbeatProtocolAdapter, "Gateway availability update failure: " + ex.ToString());
		}
	}

	private void AlertDongleFailure(bool toSet)
	{
		eventManager.GetEvent<LemonbeatDongleFailedEvent>().Publish(new LemonbeatDongleFailedEventArgs
		{
			DongleFailure = toSet
		});
	}

	private void RaiseUsbDeviceConnectionChangedEvent(bool connected)
	{
		eventManager.GetEvent<UsbDeviceConnectionChangedEvent>().Publish(new UsbDeviceConnectionChangedEventArgs
		{
			ProtocolIdentifier = ProtocolIdentifier.Lemonbeat,
			Connected = connected
		});
	}

	private void AddPacketReceiver(LemonbeatServicePort port, LemonbeatServiceId serviceId)
	{
		UdpPacketReceiver udpPacketReceiver = new UdpPacketReceiver((int)port);
		packetReceivers.Add(udpPacketReceiver);
		udpPacketReceiver.ProcessIncomingMessage = delegate(IPEndPoint ep, byte[] msg)
		{
			ProcessIncomingMessage(port, serviceId, ep, msg);
			return (byte[])null;
		};
	}

	private void ProcessIncomingMessage(LemonbeatServicePort port, LemonbeatServiceId serviceId, IPEndPoint endPoint, byte[] message)
	{
		Log.Debug(Module.LemonbeatProtocolAdapter, $"Incoming message received from IP: {endPoint.Address.ToString()}");
		string text = null;
		EventHandler<MessageReceivedEventArgs> messageReceived = this.MessageReceived;
		try
		{
			if (messageReceived != null)
			{
				text = exiCompressor.DecompressMessage(message, port);
				messageReceived(this, new MessageReceivedEventArgs(serviceId, endPoint.Address, text));
			}
		}
		catch (Exception arg)
		{
			Log.Error(Module.LemonbeatProtocolAdapter, $"The received message from {endPoint.Address.ToString()} caused an error. Message: {message.ToReadable()}, Error:{arg}");
		}
	}

	public bool Ping(IPAddress destination)
	{
		return LemonbeatPing.Ping(destination);
	}

	public void SendMessage(IPAddress destination, LemonbeatServiceId serviceId, string message, global::SmartHome.SHC.API.Protocols.Lemonbeat.Transport preferredTransportType)
	{
		if (dongleConnected)
		{
			try
			{
				IPacketSender packetSender = ((preferredTransportType == global::SmartHome.SHC.API.Protocols.Lemonbeat.Transport.Tcp) ? tcpPacketSender : udpPacketSender);
				LemonbeatServicePort servicePort = GetServicePort(serviceId);
				byte[] compressedMessage = exiCompressor.GetCompressedMessage(message, servicePort);
				IPEndPoint remoteEndPoint = new IPEndPoint(destination, (int)servicePort);
				packetSender.Send(remoteEndPoint, compressedMessage, responseExpected: false);
			}
			catch (Exception ex)
			{
				Log.Error(Module.LemonbeatProtocolAdapter, "Failure while sending request: " + ex);
				throw;
			}
		}
	}

	private LemonbeatServicePort GetServicePort(LemonbeatServiceId serviceId)
	{
		return serviceId switch
		{
			LemonbeatServiceId.PublicKey => LemonbeatServicePort.PublicKey, 
			LemonbeatServiceId.MemoryDescription => LemonbeatServicePort.MemoryInformation, 
			LemonbeatServiceId.DeviceDescription => LemonbeatServicePort.DeviceDescription, 
			LemonbeatServiceId.ValueDescription => LemonbeatServicePort.ValueDescription, 
			LemonbeatServiceId.Value => LemonbeatServicePort.Value, 
			LemonbeatServiceId.PartnerInformation => LemonbeatServicePort.PartnerInformation, 
			LemonbeatServiceId.Action => LemonbeatServicePort.Action, 
			LemonbeatServiceId.Calculation => LemonbeatServicePort.Calculation, 
			LemonbeatServiceId.Timer => LemonbeatServicePort.Timer, 
			LemonbeatServiceId.Calendar => LemonbeatServicePort.Calendar, 
			LemonbeatServiceId.Statemachine => LemonbeatServicePort.StateMachine, 
			LemonbeatServiceId.FirmwareUpdate => LemonbeatServicePort.FirmwareUpdate, 
			LemonbeatServiceId.ChannelScan => LemonbeatServicePort.ChannelScan, 
			LemonbeatServiceId.Status => LemonbeatServicePort.Status, 
			LemonbeatServiceId.Configuration => LemonbeatServicePort.Configuration, 
			LemonbeatServiceId.NetworkManagement => LemonbeatServicePort.NetworkManagement, 
			LemonbeatServiceId.ServiceDescription => LemonbeatServicePort.ServiceDescription, 
			_ => throw new InvalidCastException("Unmatched service: " + serviceId), 
		};
	}

	public string SendRequest(IPAddress destination, LemonbeatServiceId serviceId, string request, global::SmartHome.SHC.API.Protocols.Lemonbeat.Transport preferredTransportType)
	{
		IPacketSender packetSender = ((preferredTransportType == global::SmartHome.SHC.API.Protocols.Lemonbeat.Transport.Tcp) ? tcpPacketSender : udpPacketSender);
		LemonbeatServicePort servicePort = GetServicePort(serviceId);
		try
		{
			byte[] compressedMessage = exiCompressor.GetCompressedMessage(request, servicePort);
			IPEndPoint remoteEndPoint = new IPEndPoint(destination, (int)servicePort);
			byte[] array = packetSender.Send(remoteEndPoint, compressedMessage, responseExpected: true);
			if (array != null)
			{
				return exiCompressor.DecompressMessage(array, servicePort);
			}
		}
		catch (Exception ex)
		{
			Log.Error(Module.LemonbeatProtocolAdapter, "Failure while sending request: " + ex);
			throw;
		}
		return string.Empty;
	}

	public void SetMulticastSubscriptions(IEnumerable<IPAddress> subscriptions)
	{
		valueReceiver.SetAddresses(subscriptions.ToList());
	}
}
