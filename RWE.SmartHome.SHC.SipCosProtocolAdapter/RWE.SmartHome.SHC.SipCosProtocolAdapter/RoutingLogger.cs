using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Events;
using SipcosCommandHandler;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter;

public class RoutingLogger
{
	private struct IpAddress
	{
		public byte FirstByte { private get; set; }

		public byte SecondByte { private get; set; }

		public byte ThirdByte { private get; set; }
	}

	private class RouteEntryInfo
	{
		public RouteManagementAddressFamilyIdentifier AddressFamily { get; set; }

		public byte[] Next { get; set; }

		public byte Distance { get; set; }

		public byte RSSI_from { get; set; }

		public byte RSSI_received { get; set; }

		public DateTime TimeStamp { get; set; }
	}

	private const double RetentionTime = 10000.0;

	private readonly object syncRoot = new object();

	private readonly IEventManager eventManager;

	private SubscriptionToken routingMessagesSubscription;

	private readonly Dictionary<IpAddress, RouteEntryInfo> routingTableHolder = new Dictionary<IpAddress, RouteEntryInfo>();

	public object SyncRoot => syncRoot;

	public RoutingLogger(IEventManager eventManager)
	{
		this.eventManager = eventManager;
	}

	public void Initialize()
	{
		routingMessagesSubscription = eventManager.GetEvent<RoutingMessageReceivedEvent>().Subscribe(OnRoutingMessageReceived, null, ThreadOption.BackgroundThread, null);
	}

	public void Uninitialize()
	{
		if (routingMessagesSubscription != null)
		{
			eventManager.GetEvent<RoutingMessageReceivedEvent>().Unsubscribe(routingMessagesSubscription);
			routingMessagesSubscription = null;
		}
		routingTableHolder.Clear();
	}

	private void OnRoutingMessageReceived(RoutingMessageReceivedEventArgs args)
	{
		lock (SyncRoot)
		{
			try
			{
				if (args.RouteManagementFrame.Address != null && args.RouteManagementFrame.Address.Length > 2)
				{
					IpAddress key = new IpAddress
					{
						FirstByte = args.RouteManagementFrame.Address[0],
						SecondByte = args.RouteManagementFrame.Address[1],
						ThirdByte = args.RouteManagementFrame.Address[2]
					};
					if (routingTableHolder.TryGetValue(key, out var value))
					{
						value.AddressFamily = args.RouteManagementFrame.AddressFamily;
						value.Next = args.RouteManagementFrame.Next;
						value.Distance = args.RouteManagementFrame.Distance;
						value.RSSI_from = args.RouteManagementFrame.RSSI_from;
						value.RSSI_received = args.RouteManagementFrame.RSSI_received;
						value.TimeStamp = args.TimeStamp;
					}
					else
					{
						routingTableHolder.Add(key, new RouteEntryInfo
						{
							AddressFamily = args.RouteManagementFrame.AddressFamily,
							Next = args.RouteManagementFrame.Next,
							Distance = args.RouteManagementFrame.Distance,
							RSSI_from = args.RouteManagementFrame.RSSI_from,
							RSSI_received = args.RouteManagementFrame.RSSI_received,
							TimeStamp = args.TimeStamp
						});
					}
				}
				PerformRouteManagementCleanup();
			}
			catch (Exception ex)
			{
				Log.Error(Module.BusinessLogic, $"Could not store routing message: {ex.Message}.\n{ex.StackTrace}");
			}
		}
	}

	private void PerformRouteManagementCleanup()
	{
		List<IpAddress> list = (from kvp in routingTableHolder
			where DateTime.UtcNow.Subtract(kvp.Value.TimeStamp).TotalMilliseconds >= 10000.0
			select kvp.Key).ToList();
		foreach (IpAddress item in list)
		{
			routingTableHolder.Remove(item);
		}
	}
}
