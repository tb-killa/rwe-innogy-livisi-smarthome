using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.DeviceInclusion;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.Scheduler;
using RWE.SmartHome.SHC.Core.Scheduler.Tasks;

namespace RWE.SmartHome.SHC.BusinessLogic.DeviceInclusion;

internal class DiscoveredDevicesHandler
{
	private const string LoggingSource = "DiscoveredDevicesHandler";

	private readonly object syncRoot = new object();

	private readonly List<DiscoveredDevice> discoveredDevices = new List<DiscoveredDevice>();

	private readonly TimeSpan cleanupTaskTimeSpan = new TimeSpan(0, 10, 0);

	private Guid? devicesCleanupTaskId;

	private readonly TimeSpan discoveredDeviceMaxAge;

	private readonly IRepository repo;

	private readonly IProtocolMultiplexer protocolMultiplexer;

	private readonly IScheduler taskScheduler;

	private readonly DiscoveryNotificationsDispatcher notificationsDispatcher;

	private readonly IApplicationsHost appHost;

	private bool discoveryActive;

	public bool DiscoveryActive
	{
		get
		{
			return discoveryActive;
		}
		set
		{
			if (value)
			{
				StartTracking();
			}
			else
			{
				StopTracking();
			}
			discoveryActive = value;
		}
	}

	public List<DiscoveredDevice> DiscoveredDevices => discoveredDevices.ToList();

	public DiscoveredDevicesHandler(DiscoveryNotificationsDispatcher notificationsDispatcher, IEventManager eventManager, IRepository repository, IScheduler taskScheduler, IProtocolMultiplexer protocolMultiplexer, IApplicationsHost appHost, TimeSpan discoveredDeviceMaxAge)
	{
		this.notificationsDispatcher = notificationsDispatcher;
		this.taskScheduler = taskScheduler;
		this.protocolMultiplexer = protocolMultiplexer;
		repo = repository;
		this.appHost = appHost;
		eventManager.GetEvent<DeviceFoundEvent>().Subscribe(OnDeviceFound, null, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<DeviceInclusionStateChangedEvent>().Subscribe(OnDeviceInclusionStateChanged, null, ThreadOption.PublisherThread, null);
		appHost.ApplicationStateChanged += AppHost_ApplicationStateChanged;
		this.discoveredDeviceMaxAge = discoveredDeviceMaxAge;
		StartCleanupTask();
	}

	public BaseDevice GetBaseDevice(Guid id)
	{
		lock (syncRoot)
		{
			return discoveredDevices.FirstOrDefault((DiscoveredDevice x) => x.Device.Id.Equals(id))?.Device;
		}
	}

	public void OnDeviceFound(DeviceFoundEventArgs eventArgs)
	{
		if (eventArgs == null || eventArgs.FoundDevice == null)
		{
			Log.DebugFormat(Module.BusinessLogic, "DiscoveredDevicesHandler", true, (eventArgs == null) ? "DeviceFound event fired with null args" : "DeviceFound event fired with null FoundDevice");
			return;
		}
		if (eventArgs.State == DeviceFoundState.MaximumNumberOfDevicesReached)
		{
			DropDiscoveredDevices();
		}
		lock (syncRoot)
		{
			if (ShouldTrackDevice(eventArgs))
			{
				TrackDevice(eventArgs);
			}
			if (DiscoveryActive)
			{
				notificationsDispatcher.NotifyDeviceDiscovered(eventArgs.FoundDevice, eventArgs.State);
			}
		}
	}

	public void OnDeviceInclusionStateChanged(DeviceInclusionStateChangedEventArgs args)
	{
		if (args.DeviceInclusionState != DeviceInclusionState.InclusionPending && args.DeviceInclusionState != DeviceInclusionState.Included && args.DeviceInclusionState != DeviceInclusionState.ExclusionPending)
		{
			return;
		}
		lock (syncRoot)
		{
			discoveredDevices.RemoveAll((DiscoveredDevice dd) => dd.Device.Id == args.DeviceId);
		}
	}

	private void StartTracking()
	{
		lock (syncRoot)
		{
			Log.Debug(Module.BusinessLogic, "DiscoveredDevicesHandler", "Device discovery tracking started...");
		}
	}

	private void StopTracking()
	{
		lock (syncRoot)
		{
			Log.Debug(Module.BusinessLogic, "DiscoveredDevicesHandler", "Device discovery tracking stopped.");
			List<Guid> list = discoveredDevices.Select((DiscoveredDevice x) => x.Device.Id).ToList();
			foreach (Guid item in list)
			{
				if (repo.GetBaseDevice(item) != null)
				{
					Guid bdid = item;
					discoveredDevices.RemoveAll((DiscoveredDevice x) => x.Device.Id == bdid);
				}
			}
		}
	}

	private bool ShouldTrackDevice(DeviceFoundEventArgs eventArgs)
	{
		if (eventArgs.State == DeviceFoundState.ReadyForInclusion)
		{
			return discoveredDevices.All((DiscoveredDevice x) => x.Device.Id != eventArgs.FoundDevice.Id);
		}
		return false;
	}

	private void TrackDevice(DeviceFoundEventArgs eventArgs)
	{
		Log.DebugFormat(Module.BusinessLogic, "DiscoveredDevicesHandler", true, "New device found {0}.", FormatDeviceForLogging(eventArgs.FoundDevice));
		discoveredDevices.Add(new DiscoveredDevice(ShcDateTime.UtcNow, eventArgs.FoundDevice));
	}

	private void StartCleanupTask()
	{
		if (!devicesCleanupTaskId.HasValue)
		{
			devicesCleanupTaskId = Guid.NewGuid();
			taskScheduler.AddSchedulerTask(new FixedTimeSpanSchedulerTask(devicesCleanupTaskId.Value, DropOldDevices, cleanupTaskTimeSpan));
		}
	}

	private void DropOldDevices()
	{
		DateTime now = ShcDateTime.UtcNow;
		List<DiscoveredDevice> list = discoveredDevices.Where((DiscoveredDevice x) => now.Subtract(x.TimeOfDiscovery) >= discoveredDeviceMaxAge).ToList();
		lock (syncRoot)
		{
			foreach (DiscoveredDevice item in list)
			{
				discoveredDevices.Remove(item);
			}
		}
		protocolMultiplexer.DropDiscoveredDevices(list.Select((DiscoveredDevice x) => x.Device).ToArray());
	}

	private string FormatDeviceForLogging(BaseDevice baseDevice)
	{
		return $"[{baseDevice.DeviceType} with serial: {baseDevice.SerialNumber} ID: {baseDevice.Id}]";
	}

	private void AppHost_ApplicationStateChanged(ApplicationLoadStateChangedEventArgs args)
	{
		if (args == null || args.Application == null)
		{
			Log.DebugFormat(Module.BusinessLogic, "DiscoveredDevicesHandler", true, (args == null) ? "ApplicationLoadStateChangedEvent event fired with null args" : "ApplicationLoadStateChangedEvent event fired with null Application");
		}
		else
		{
			if (args.ApplicationState != ApplicationStates.ApplicationDeactivated && args.ApplicationState != ApplicationStates.ApplicationUpdated && args.ApplicationState != ApplicationStates.ApplicationsUninstalled)
			{
				return;
			}
			lock (syncRoot)
			{
				discoveredDevices.RemoveAll((DiscoveredDevice x) => x.Device.AppId == args.Application.ApplicationId);
			}
		}
	}

	private void DropDiscoveredDevices()
	{
		discoveredDevices.Clear();
	}
}
