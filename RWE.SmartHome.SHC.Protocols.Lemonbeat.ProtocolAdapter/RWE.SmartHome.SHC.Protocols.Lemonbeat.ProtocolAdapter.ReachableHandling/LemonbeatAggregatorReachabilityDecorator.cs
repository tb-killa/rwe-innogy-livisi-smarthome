using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.Scheduler;
using RWE.SmartHome.SHC.Core.Scheduler.Tasks;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Interfaces;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Interfaces;
using SmartHome.SHC.API.Protocols.Lemonbeat;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.ReachableHandling;

internal sealed class LemonbeatAggregatorReachabilityDecorator : ILemonbeatCommunication, IDeviceReachability
{
	private const int granularitySeconds = 60;

	private readonly ILemonbeatCommunication implementation;

	private readonly IDeviceList deviceList;

	private readonly IEventManager eventManager;

	private readonly IScheduler scheduler;

	private readonly IApplicationsHost applicationHost;

	private readonly List<int> activeGatewayTypes;

	private Guid reachabilityTaskId;

	public event Action<ServiceType, DeviceIdentifier, string> MessageReceived;

	public event Action<int, bool> GatewayAvailabilityUpdated;

	public LemonbeatAggregatorReachabilityDecorator(ILemonbeatCommunication implementation, IDeviceList deviceList, IScheduler scheduler, IApplicationsHost applicationHost, IEventManager eventManager)
	{
		activeGatewayTypes = new List<int>();
		this.implementation = implementation;
		this.eventManager = eventManager;
		this.deviceList = deviceList;
		this.scheduler = scheduler;
		this.applicationHost = applicationHost;
		ILemonbeatCommunication lemonbeatCommunication = this.implementation;
		Action<ServiceType, DeviceIdentifier, string> value = delegate(ServiceType service, DeviceIdentifier devIdentifier, string messageReceived)
		{
			Action<ServiceType, DeviceIdentifier, string> messageReceived2 = this.MessageReceived;
			if (messageReceived2 != null)
			{
				UpdateReachability(devIdentifier, reachable: true);
				messageReceived2(service, devIdentifier, messageReceived);
			}
		};
		lemonbeatCommunication.MessageReceived += value;
		this.implementation.GatewayAvailabilityUpdated += OnGatewayAvailabilityUpdated;
	}

	public void SendMessage(DeviceIdentifier destination, ServiceType serviceId, string message, TransportType preferredTransportType)
	{
		try
		{
			implementation.SendMessage(destination, serviceId, message, preferredTransportType);
			if (preferredTransportType == TransportType.Connection)
			{
				UpdateReachability(destination, reachable: true);
			}
		}
		catch (Exception)
		{
			ForceReachabilityCheck(destination);
			throw;
		}
	}

	public string SendRequest(DeviceIdentifier destination, ServiceType serviceId, string request, TransportType preferredTransportType)
	{
		string empty = string.Empty;
		try
		{
			empty = implementation.SendRequest(destination, serviceId, request, preferredTransportType);
			UpdateReachability(destination, reachable: true);
			return empty;
		}
		catch (Exception)
		{
			ForceReachabilityCheck(destination);
			throw;
		}
	}

	public ReachabilityState Ping(DeviceIdentifier destination)
	{
		return implementation.Ping(destination);
	}

	public void SetMulticastSubscriptions(List<DeviceIdentifier> subscriptions)
	{
		implementation.SetMulticastSubscriptions(subscriptions);
	}

	public void UpdateReachability(DeviceIdentifier remoteEndPoint, bool reachable)
	{
		IEnumerable<DeviceInformation> devicesByIPAddress = deviceList.GetDevicesByIPAddress(remoteEndPoint.IPAddress);
		if (devicesByIPAddress == null)
		{
			return;
		}
		foreach (DeviceInformation item in devicesByIPAddress)
		{
			item.LastDeviceReachabilityTestedTime = DateTime.UtcNow;
			item.IsReachable = reachable;
		}
	}

	public void ForceReachabilityCheck(DeviceIdentifier remoteEndPoint)
	{
		IEnumerable<DeviceInformation> devicesByIPAddress = deviceList.GetDevicesByIPAddress(remoteEndPoint.IPAddress);
		if (devicesByIPAddress != null)
		{
			foreach (DeviceInformation item in devicesByIPAddress)
			{
				if (!ValidateDeviceInclusion(item))
				{
					Log.Debug(Module.LemonbeatProtocolAdapter, "Trying to force ping to not included device @ " + remoteEndPoint.ToString());
					return;
				}
			}
		}
		ThreadPool.QueueUserWorkItem(delegate
		{
			UpdateReachability(remoteEndPoint, Ping(remoteEndPoint) == ReachabilityState.Reachable);
		});
	}

	private void ForceReachabilityRefresh()
	{
		IEnumerable<DeviceInformation> source = deviceList.SyncWhere((DeviceInformation d) => ValidateDeviceInclusion(d));
		foreach (DeviceIdentifier item in source.Select((DeviceInformation dev) => dev.Identifier).Distinct())
		{
			ForceReachabilityCheck(item);
		}
	}

	private void TestReachability()
	{
		try
		{
			IEnumerable<DeviceInformation> source = deviceList.SyncWhere((DeviceInformation d) => RequiresReachabilityTest(d));
			foreach (DeviceIdentifier item in source.Select((DeviceInformation dev) => dev.Identifier))
			{
				UpdateReachability(item, Ping(item) == ReachabilityState.Reachable);
				Thread.Sleep(2000);
			}
		}
		catch (Exception ex)
		{
			Log.Error(Module.LemonbeatProtocolAdapter, "Unable to perform reachability testing: " + ex.ToString());
		}
	}

	private bool ValidateDeviceInclusion(DeviceInformation device)
	{
		if (device == null)
		{
			return false;
		}
		if (device.DeviceInclusionState != LemonbeatDeviceInclusionState.Included)
		{
			return device.DeviceInclusionState == LemonbeatDeviceInclusionState.ExclusionPending;
		}
		return true;
	}

	private bool RequiresReachabilityTest(DeviceInformation device)
	{
		if (!ValidateDeviceInclusion(device))
		{
			return false;
		}
		if (!device.ReachabilityParametersReadFromAddin)
		{
			IDeviceHandler lemonbeatDeviceHandler = applicationHost.GetLemonbeatDeviceHandler(device.DeviceTypeIdentifier);
			if (lemonbeatDeviceHandler == null)
			{
				return false;
			}
			device.PollingInterval = lemonbeatDeviceHandler.GetReachabilityPollingInterval(device.DeviceId);
			device.ReachabilityParametersReadFromAddin = true;
		}
		if (!device.PollingInterval.HasValue)
		{
			return false;
		}
		return DateTime.UtcNow - device.LastDeviceReachabilityTestedTime > device.PollingInterval.Value;
	}

	private void OnGatewayAvailabilityUpdated(int gwid, bool val)
	{
		bool flag = false;
		lock (activeGatewayTypes)
		{
			if (val)
			{
				if (!activeGatewayTypes.Contains(gwid))
				{
					activeGatewayTypes.Add(gwid);
					if (activeGatewayTypes.Count == 1)
					{
						StartReachabilityCheck();
					}
					flag = true;
				}
			}
			else if (activeGatewayTypes.Remove(gwid))
			{
				if (activeGatewayTypes.Count == 0)
				{
					SuspendReachabilityCheck();
				}
				flag = true;
			}
		}
		Action<int, bool> gatewayAvailabilityUpdated = this.GatewayAvailabilityUpdated;
		if (gatewayAvailabilityUpdated != null)
		{
			this.GatewayAvailabilityUpdated(gwid, val);
		}
		if (!flag)
		{
			return;
		}
		List<DeviceInformation> list = deviceList.SyncWhere((DeviceInformation d) => ValidateDeviceInclusion(d) && d.Identifier.GatewayId == gwid);
		if (list == null)
		{
			return;
		}
		foreach (DeviceInformation item in list)
		{
			Thread.Sleep(2000);
			if (!ValidateDeviceInclusion(item))
			{
				Log.Debug(Module.LemonbeatProtocolAdapter, "Trying to force ping to not included device @ " + item.Identifier.ToString());
				continue;
			}
			bool flag2 = false;
			try
			{
				flag2 = Ping(item.Identifier) == ReachabilityState.Reachable;
			}
			catch
			{
			}
			UpdateReachability(item.Identifier, flag2);
			if (!flag2)
			{
				item.LastDeviceReachabilityTestedTime = DateTime.UtcNow - TimeSpan.FromDays(1.0);
			}
		}
	}

	private void StartReachabilityCheck()
	{
		if (reachabilityTaskId != Guid.Empty)
		{
			return;
		}
		Random random = new Random();
		IEnumerable<DeviceInformation> enumerable = deviceList.SyncWhere((DeviceInformation d) => RequiresReachabilityTest(d));
		foreach (DeviceInformation item in enumerable)
		{
			item.LastDeviceReachabilityTestedTime = DateTime.UtcNow - TimeSpan.FromSeconds(random.Next(600));
		}
		reachabilityTaskId = Guid.NewGuid();
		scheduler.AddSchedulerTask(new FixedTimeSpanSchedulerTask(reachabilityTaskId, TestReachability, TimeSpan.FromSeconds(60.0)));
	}

	private void SuspendReachabilityCheck()
	{
		if (reachabilityTaskId != Guid.Empty)
		{
			scheduler.RemoveSchedulerTask(reachabilityTaskId);
			reachabilityTaskId = Guid.Empty;
			Log.Information(Module.LemonbeatProtocolAdapter, "Device ping suspended while dongle is unplugged.");
		}
	}
}
