using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceInclusion;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.Scheduler;
using RWE.SmartHome.SHC.Core.Scheduler.Tasks;
using RWE.SmartHome.SHC.ExternalCommandDispatcherInterfaces;

namespace RWE.SmartHome.SHC.BusinessLogic.DeviceInclusion;

public class DiscoveryController : IDiscoveryController
{
	private const string LoggingSource = "DiscoveryController";

	public static readonly TimeSpan DefaultDiscoveredDeviceMaxAge = new TimeSpan(1, 0, 0);

	private readonly IScheduler taskScheduler;

	private readonly IProtocolMultiplexer protocolMultiplexer;

	private readonly object discoveryActivationSyncRoot = new object();

	private readonly object activationTaskSyncRoot = new object();

	private Guid? activationTaskId;

	private readonly DiscoveryNotificationsDispatcher notificationsDispatcher;

	private readonly DiscoveredDevicesHandler discoveredDevices;

	public DiscoveryController(IRepository repository, IEventManager eventManager, INotificationHandler notificationHandler, IScheduler taskScheduler, IProtocolMultiplexer protocolMultiplexer, IApplicationsHost appHost, TimeSpan discoveredDeviceMaxAge)
	{
		notificationsDispatcher = new DiscoveryNotificationsDispatcher(notificationHandler);
		discoveredDevices = new DiscoveredDevicesHandler(notificationsDispatcher, eventManager, repository, taskScheduler, protocolMultiplexer, appHost, discoveredDeviceMaxAge);
		this.taskScheduler = taskScheduler;
		this.protocolMultiplexer = protocolMultiplexer;
	}

	public BaseDevice GetDiscoveredBaseDevice(Guid id)
	{
		return discoveredDevices.GetBaseDevice(id);
	}

	public void StartDiscovery(List<string> appIds)
	{
		lock (activationTaskSyncRoot)
		{
			Log.Debug(Module.BusinessLogic, "DiscoveryController", "Device discovery starting.");
			RemoveActivateDeviceDiscoveryTask();
			AddActivateDeviceDiscoveryTask(appIds);
		}
	}

	public void StopDiscovery()
	{
		lock (activationTaskSyncRoot)
		{
			lock (discoveryActivationSyncRoot)
			{
				RemoveActivateDeviceDiscoveryTask();
				StopDiscoveryInternal();
			}
		}
	}

	private void AddActivateDeviceDiscoveryTask(IEnumerable<string> appIds)
	{
		activationTaskId = Guid.NewGuid();
		List<string> taskParams = appIds?.ToList();
		taskScheduler.AddSchedulerTask(new FixedTimeSpanSchedulerTask(activationTaskId.Value, delegate
		{
			StartDiscoveryInternal(taskParams);
		}, new TimeSpan(0, 0, 0), runOnce: true));
	}

	private void RemoveActivateDeviceDiscoveryTask()
	{
		if (activationTaskId.HasValue)
		{
			taskScheduler.RemoveSchedulerTask(activationTaskId.Value);
			activationTaskId = null;
		}
	}

	private void StartDiscoveryInternal(List<string> appIds)
	{
		lock (discoveryActivationSyncRoot)
		{
			if (discoveredDevices.DiscoveryActive)
			{
				StopDiscoveryInternal();
			}
			protocolMultiplexer.ActivateDeviceDiscovery(appIds);
			notificationsDispatcher.NotifyDeviceDiscoveryStateChanged(active: true, appIds);
			notificationsDispatcher.NotifyDiscoveredDevices(discoveredDevices.DiscoveredDevices);
			discoveredDevices.DiscoveryActive = true;
		}
	}

	private void StopDiscoveryInternal()
	{
		Log.Debug(Module.BusinessLogic, "DiscoveryController", "Device discovery stopping.");
		protocolMultiplexer.DeactivateDeviceDiscovery();
		discoveredDevices.DiscoveryActive = false;
		notificationsDispatcher.NotifyDeviceDiscoveryStateChanged(active: false, null);
	}
}
