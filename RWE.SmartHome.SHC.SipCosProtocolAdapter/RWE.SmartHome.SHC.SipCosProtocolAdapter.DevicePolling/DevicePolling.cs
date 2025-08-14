using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ConfigurationTransformation.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.Scheduler;
using RWE.SmartHome.SHC.Core.Scheduler.Tasks;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.DeviceHandler;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Events;
using RWE.SmartHome.SHC.SipCosProtocolAdapter.LogicalDeviceHandler;
using RWE.SmartHome.SHC.SipCosProtocolAdapterInterfaces;
using RWE.SmartHome.SHC.StartupLogicInterfaces.Events;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter.DevicePolling;

public class DevicePolling : IDevicePolling
{
	private class DevicePollingInfo
	{
		internal IEnumerable<byte> StatusInfoChannels { get; set; }

		internal int MinPollingInterval { get; set; }

		internal DateTime LastPolledTime { get; set; }
	}

	private const int DEVICE_POLLING_FREQUENCY = 120;

	private readonly ILogicalDeviceHandlerCollection logicalDeviceHandlerCollection;

	private readonly IRepository configurationRepository;

	private readonly IDeviceManager deviceManager;

	private readonly Dictionary<IDeviceInformation, DevicePollingInfo> pollingDeviceInformation = new Dictionary<IDeviceInformation, DevicePollingInfo>();

	private int pollingIndex;

	private Guid pollingTaskId;

	private readonly IScheduler scheduler;

	private readonly object syncPolling = new object();

	private bool isTrafficSuspended;

	private readonly BidcosAdvancedPolling bidcosAdvancedPolling;

	internal DevicePolling(IDeviceManager deviceManager, IRepository configurationRepository, IEventManager eventManager, IScheduler scheduler, ILogicalDeviceHandlerCollection logicalDeviceHandlers)
	{
		this.configurationRepository = configurationRepository;
		this.scheduler = scheduler;
		this.deviceManager = deviceManager;
		logicalDeviceHandlerCollection = logicalDeviceHandlers;
		pollingTaskId = Guid.NewGuid();
		FixedTimeSpanSchedulerTask schedulerTask = new FixedTimeSpanSchedulerTask(pollingTaskId, RefreshDeviceStatus, TimeSpan.FromSeconds(120.0));
		scheduler.AddSchedulerTask(schedulerTask);
		eventManager.GetEvent<ShcStartupCompletedEvent>().Subscribe(RefreshPollingDeviceList, (ShcStartupCompletedEventArgs args) => args.Progress == StartupProgress.CompletedRound1, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<ConfigurationProcessedEvent>().Subscribe(OnConfigurationChanged, (ConfigurationProcessedEventArgs args) => args.ConfigurationPhase == ConfigurationProcessedPhase.CompletedInternally, ThreadOption.BackgroundThread, null);
		eventManager.GetEvent<CosIPTrafficControlEvent>().Subscribe(OnCosIPTrafficControlEvent, null, ThreadOption.PublisherThread, null);
		isTrafficSuspended = false;
		bidcosAdvancedPolling = new BidcosAdvancedPolling(eventManager, deviceManager, scheduler, logicalDeviceHandlers);
	}

	public void HandleDeviceChanges()
	{
		RescheduleDevicePolling(120);
	}

	public void RefreshPollingDeviceList(ShcStartupCompletedEventArgs args)
	{
		RefreshPollingDeviceList();
	}

	private void OnCosIPTrafficControlEvent(CosIPTrafficControlEventArgs args)
	{
		switch (args.TrafficState)
		{
		}
	}

	private void RefreshPollingDeviceList()
	{
		List<LogicalDevice> list = new List<LogicalDevice>(configurationRepository.GetLogicalDevices());
		lock (syncPolling)
		{
			pollingDeviceInformation.Clear();
			foreach (LogicalDevice item in list)
			{
				if (logicalDeviceHandlerCollection.GetLogicalDeviceHandler(item) is IActuatorHandler actuatorHandler && actuatorHandler.GetIsPeriodicStatusPollingActive(item))
				{
					IEnumerable<byte> statusInfoChannels = actuatorHandler.StatusInfoChannels;
					Guid baseDeviceId = item.BaseDeviceId;
					IDeviceInformation deviceInformation = deviceManager.DeviceList[baseDeviceId];
					if (deviceInformation != null && (deviceInformation.DeviceInclusionState == DeviceInclusionState.Included || deviceInformation.DeviceInclusionState == DeviceInclusionState.InclusionPending) && !pollingDeviceInformation.ContainsKey(deviceInformation))
					{
						pollingDeviceInformation.Add(deviceInformation, new DevicePollingInfo
						{
							StatusInfoChannels = statusInfoChannels,
							MinPollingInterval = actuatorHandler.MinStatusRequestPollingIterval
						});
					}
				}
			}
		}
	}

	private void RescheduleDevicePolling(int sec)
	{
		if (pollingTaskId != Guid.Empty)
		{
			scheduler.RemoveSchedulerTask(pollingTaskId);
			pollingTaskId = Guid.NewGuid();
			FixedTimeSpanSchedulerTask schedulerTask = new FixedTimeSpanSchedulerTask(pollingTaskId, RefreshDeviceStatus, TimeSpan.FromSeconds(sec));
			scheduler.AddSchedulerTask(schedulerTask);
			if (sec != 120)
			{
				Log.Information(Module.SipCosProtocolAdapter, $"Device polling: Polling frequency changed to {sec} secs.");
			}
		}
	}

	private void RefreshDeviceStatus()
	{
		IDeviceController deviceController = null;
		IDeviceInformation deviceInformation = null;
		IEnumerable<byte> enumerable = null;
		lock (syncPolling)
		{
			if (isTrafficSuspended)
			{
				return;
			}
			if (pollingDeviceInformation.Count() != 0)
			{
				if (pollingIndex >= pollingDeviceInformation.Count())
				{
					pollingIndex = 0;
				}
				deviceInformation = pollingDeviceInformation.Keys.ElementAt(pollingIndex);
				deviceController = deviceManager[deviceInformation];
				enumerable = pollingDeviceInformation.Values.ElementAt(pollingIndex).StatusInfoChannels;
				if (pollingDeviceInformation.Values.ElementAt(pollingIndex).MinPollingInterval > 0)
				{
					if (DateTime.Now - pollingDeviceInformation.Values.ElementAt(pollingIndex).LastPolledTime < TimeSpan.FromSeconds(pollingDeviceInformation.Values.ElementAt(pollingIndex).MinPollingInterval))
					{
						Log.Debug(Module.SipCosProtocolAdapter, $"Skip polling for {deviceInformation.ToString()}, minimum allowed polling time didn't pass yet!");
						pollingIndex++;
						return;
					}
					pollingDeviceInformation.Values.ElementAt(pollingIndex).LastPolledTime = DateTime.Now;
					Log.Debug(Module.SipCosProtocolAdapter, $"Time to poll device {deviceInformation.ToString()}");
				}
				pollingIndex++;
			}
		}
		if (deviceController == null || enumerable == null || deviceInformation == null || deviceInformation.DeviceInclusionState != DeviceInclusionState.Included || !DeviceReachableOrBidcos(deviceInformation))
		{
			return;
		}
		foreach (byte item in enumerable)
		{
			deviceController.RequestStatusInfo(item);
		}
	}

	private bool DeviceReachableOrBidcos(IDeviceInformation deviceInformation)
	{
		if (deviceInformation.ProtocolType != ProtocolType.BidCos)
		{
			return !deviceInformation.DeviceUnreachable;
		}
		return true;
	}

	private void OnConfigurationChanged(ConfigurationProcessedEventArgs args)
	{
		RefreshPollingDeviceList();
	}
}
