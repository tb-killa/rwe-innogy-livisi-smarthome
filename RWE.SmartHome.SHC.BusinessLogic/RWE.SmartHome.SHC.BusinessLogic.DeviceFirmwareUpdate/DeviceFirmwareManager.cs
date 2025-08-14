using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces.Events;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.DeviceUpdate;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolSpecific;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.Scheduler;
using RWE.SmartHome.SHC.Core.Scheduler.Tasks;
using RWE.SmartHome.SHC.DeviceFirmware.Reinclusion;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Events;
using RWE.SmartHome.SHC.StartupLogicInterfaces.Events;

namespace RWE.SmartHome.SHC.BusinessLogic.DeviceFirmwareUpdate;

public class DeviceFirmwareManager : IDeviceFirmwareManager, IService
{
	private const string LoggingSource = "DeviceFirmwareManager";

	private readonly IEventManager eventManager;

	private readonly IApplicationsHost applicationHost;

	private readonly IScheduler taskScheduler;

	private readonly Configuration configuration;

	private SubscriptionToken shcStartupCompletedToken;

	private SubscriptionToken deviceExcludedToken;

	private SubscriptionToken deviceIncludedToken;

	private SubscriptionToken deviceReachabilityChangedToken;

	private SubscriptionToken deviceUpdateStateChangedToken;

	private readonly IDeviceFirmwareImagesService firmwareImagesService;

	private Action<ApplicationLoadStateChangedEventArgs> appActivatedEventHandler;

	private Action<ApplicationLoadStateChangedEventArgs> appUnloadedEventHandler;

	private readonly List<IProtocolSpecificDeviceUpdater> deviceUpdaters = new List<IProtocolSpecificDeviceUpdater>();

	private readonly Dictionary<Guid, byte> deviceUpdateRetries = new Dictionary<Guid, byte>();

	private bool checkAlreadyRunning;

	private readonly Dictionary<Guid, IProtocolSpecificDeviceUpdater> updaterCache = new Dictionary<Guid, IProtocolSpecificDeviceUpdater>();

	private bool shcStartupCompleted;

	private readonly Dictionary<Guid, RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware.DeviceUpdateInfo> exclusionCache = new Dictionary<Guid, RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware.DeviceUpdateInfo>();

	private readonly ReincludeHandler reincludeHandler;

	private readonly object syncLock = new object();

	private readonly object deviceUpdateRetriesLock = new object();

	public DeviceFirmwareManager(IEventManager eventManager, IApplicationsHost appHost, IScheduler taskScheduler, Configuration configuration, IDeviceFirmwareImagesService firmwareImagesService, ReincludeHandler reincludeHandler)
	{
		this.eventManager = eventManager;
		applicationHost = appHost;
		this.taskScheduler = taskScheduler;
		this.configuration = configuration;
		this.firmwareImagesService = firmwareImagesService;
		this.reincludeHandler = reincludeHandler;
	}

	private void SubscribeToEvents()
	{
		try
		{
			shcStartupCompletedToken = eventManager.GetEvent<ShcStartupCompletedEvent>().Subscribe(OnShcStartupCompleted, (ShcStartupCompletedEventArgs args) => args.Progress == StartupProgress.CompletedRound2, ThreadOption.BackgroundThread, null);
			deviceIncludedToken = eventManager.GetEvent<DeviceInclusionStateChangedEvent>().Subscribe(OnDeviceIncluded, (DeviceInclusionStateChangedEventArgs args) => args.DeviceInclusionState == DeviceInclusionState.Included, ThreadOption.BackgroundThread, null);
			deviceExcludedToken = eventManager.GetEvent<DeviceInclusionStateChangedEvent>().Subscribe(OnDeviceExcluded, (DeviceInclusionStateChangedEventArgs args) => args.DeviceInclusionState == DeviceInclusionState.Excluded || args.DeviceInclusionState == DeviceInclusionState.ExclusionPending || args.DeviceInclusionState == DeviceInclusionState.FactoryReset, ThreadOption.BackgroundThread, null);
			firmwareImagesService.FirmwareDownloadFinished += OnFirmwareDownloadComplete;
			deviceReachabilityChangedToken = eventManager.GetEvent<DeviceUnreachableChangedEvent>().Subscribe(OnDeviceReachabilityChanged, null, ThreadOption.BackgroundThread, null);
			deviceUpdateStateChangedToken = eventManager.GetEvent<DeviceUpdateStateChangedEvent>().Subscribe(OnDeviceUpdateStateChanged, null, ThreadOption.BackgroundThread, null);
			appActivatedEventHandler = OnApplicationActivated;
			appUnloadedEventHandler = OnApplicationUnloaded;
			applicationHost.ApplicationStateChanged += appActivatedEventHandler;
			applicationHost.ApplicationStateChanged += appUnloadedEventHandler;
		}
		catch (Exception ex)
		{
			Log.Exception(Module.BusinessLogic, ex, "DeviceFirmwareManager", "Unexpected error while subscribing to events.");
		}
	}

	private void UnsubscribeFromEvents()
	{
		try
		{
			deviceUpdaters.ForEach(delegate(IProtocolSpecificDeviceUpdater upd)
			{
				upd.UpdateFailed -= HandleFailedDeviceUpdate;
			});
			if (shcStartupCompletedToken != null)
			{
				eventManager.GetEvent<ShcStartupCompletedEvent>().Unsubscribe(shcStartupCompletedToken);
				shcStartupCompletedToken = null;
			}
			if (deviceIncludedToken != null)
			{
				eventManager.GetEvent<DeviceInclusionStateChangedEvent>().Unsubscribe(deviceIncludedToken);
				deviceIncludedToken = null;
			}
			if (deviceExcludedToken != null)
			{
				eventManager.GetEvent<DeviceInclusionStateChangedEvent>().Unsubscribe(deviceExcludedToken);
				deviceExcludedToken = null;
			}
			if (deviceUpdateStateChangedToken != null)
			{
				eventManager.GetEvent<DeviceUpdateStateChangedEvent>().Unsubscribe(deviceUpdateStateChangedToken);
				deviceUpdateStateChangedToken = null;
			}
			if (deviceReachabilityChangedToken != null)
			{
				eventManager.GetEvent<DeviceUnreachableChangedEvent>().Unsubscribe(deviceReachabilityChangedToken);
				deviceReachabilityChangedToken = null;
			}
			firmwareImagesService.FirmwareDownloadFinished -= OnFirmwareDownloadComplete;
			if (appUnloadedEventHandler != null)
			{
				applicationHost.ApplicationStateChanged -= appUnloadedEventHandler;
				applicationHost.ApplicationStateChanged -= appUnloadedEventHandler;
				appUnloadedEventHandler = null;
			}
			if (appActivatedEventHandler != null)
			{
				applicationHost.ApplicationStateChanged -= appActivatedEventHandler;
				appUnloadedEventHandler = null;
			}
		}
		catch (Exception ex)
		{
			Log.Exception(Module.BusinessLogic, ex, "DeviceFirmwareManager", "Unexpected error while unsubscribing from events.");
		}
	}

	private void OnDeviceUpdateStateChanged(DeviceUpdateStateChangedEventArgs args)
	{
		try
		{
			if ((args.OldDeviceUpdateState != DeviceUpdateState.UpToDate && args.NewDeviceUpdateState == DeviceUpdateState.UpToDate) || (args.OldDeviceUpdateState != DeviceUpdateState.ImageTransferred && args.NewDeviceUpdateState == DeviceUpdateState.ImageTransferred))
			{
				EvaluateDevice(args.DeviceId);
				RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware.DeviceUpdateInfo deviceUpdateInfo = GetDeviceUpdateInfo(args.DeviceId);
				if (deviceUpdateInfo != null)
				{
					bool occurred = IsDeviceReadyForUpdate(deviceUpdateInfo);
					RaiseDeviceReadyForUpdateEvent(deviceUpdateInfo, occurred);
				}
			}
		}
		catch (Exception ex)
		{
			Log.Exception(Module.BusinessLogic, ex, "DeviceFirmwareManager", "Error occurred while handling device update state changed");
		}
	}

	private void OnApplicationActivated(ApplicationLoadStateChangedEventArgs args)
	{
		try
		{
			if (args != null && args.Application != null && shcStartupCompleted && args.ApplicationState == ApplicationStates.ApplicationActivated)
			{
				CheckAppDevicesUpdateStatusChanges(args.Application.ApplicationId);
			}
		}
		catch (Exception ex)
		{
			Log.Exception(Module.BusinessLogic, ex, "DeviceFirmwareManager", "Error occurred while handling applciation load.");
		}
	}

	private void OnApplicationUnloaded(ApplicationLoadStateChangedEventArgs args)
	{
		try
		{
			if (args != null && args.Application != null && (args.ApplicationState == ApplicationStates.ApplicationsUninstalled || args.ApplicationState == ApplicationStates.ApplicationDeactivated))
			{
				NotifyAppDeviceGroupReady(args.Application.ApplicationId);
			}
		}
		catch (Exception ex)
		{
			Log.Exception(Module.BusinessLogic, ex, "DeviceFirmwareManager", "Error occurred while handling application unload.");
		}
	}

	private void OnDeviceReachabilityChanged(DeviceUnreachableChangedEventArgs args)
	{
		try
		{
			if (args.Unreachable)
			{
				RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware.DeviceUpdateInfo deviceUpdateInfo = GetDeviceUpdateInfo(args.DeviceId);
				if (deviceUpdateInfo != null)
				{
					if (deviceUpdateInfo.UpdateState != DeviceUpdateState.Updating)
					{
						GetDeviceUpdater(args.DeviceId).AbortUpdate(args.DeviceId);
						ResetDeviceUpdateCounter(args.DeviceId);
					}
					else
					{
						Log.Warning(Module.BusinessLogic, $"Cannot abort updating process: {deviceUpdateInfo.ToString()}");
					}
				}
			}
			else
			{
				EvaluateDevice(args.DeviceId);
			}
		}
		catch (Exception ex)
		{
			Log.Exception(Module.BusinessLogic, ex, "DeviceFirmwareManager", "Error occurred while handling device reachability changed.");
		}
	}

	private void ResetDeviceUpdateCounter(Guid deviceId)
	{
		lock (deviceUpdateRetriesLock)
		{
			deviceUpdateRetries.Remove(deviceId);
		}
	}

	private void IncrementDeviceUpdateCounter(Guid deviceId)
	{
		lock (deviceUpdateRetriesLock)
		{
			if (deviceUpdateRetries.ContainsKey(deviceId))
			{
				deviceUpdateRetries[deviceId]++;
			}
			else
			{
				deviceUpdateRetries.Add(deviceId, 1);
			}
		}
	}

	private void OnFirmwareDownloadComplete(object sender, FirmwareDownloadFinishedEventArgs args)
	{
		try
		{
			foreach (IProtocolSpecificDeviceUpdater deviceUpdater in deviceUpdaters)
			{
				List<RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware.DeviceUpdateInfo> list = (from di in deviceUpdater.GetDeviceInfo()
					where di.Manufacturer == args.DeviceInfo.Manufacturer && di.ProductId == args.DeviceInfo.ProductId
					select di).ToList();
				if (list.Any())
				{
					list.ForEach(HandleDeviceUpdateInfoUpdateStateChanged);
					break;
				}
			}
		}
		catch (Exception ex)
		{
			Log.Exception(Module.BusinessLogic, ex, "DeviceFirmwareManager", "Error occurred while handling firmware download complete.");
		}
	}

	private void OnDeviceIncluded(DeviceInclusionStateChangedEventArgs args)
	{
		try
		{
			if (args != null)
			{
				Log.Debug(Module.BusinessLogic, "DeviceFirmwareManager", "Evaluating newly included device for update.");
				EvaluateDevice(args.DeviceId);
				CacheForExclusion(GetDeviceUpdateInfo(args.DeviceId));
			}
		}
		catch (Exception ex)
		{
			Log.Exception(Module.BusinessLogic, ex, "DeviceFirmwareManager", "Error occurred while handling device inclusion.");
		}
	}

	private void OnDeviceExcluded(DeviceInclusionStateChangedEventArgs args)
	{
		try
		{
			GetDeviceUpdater(args.DeviceId)?.AbortUpdate(args.DeviceId);
			updaterCache.Remove(args.DeviceId);
			ResetDeviceUpdateCounter(args.DeviceId);
			RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware.DeviceUpdateInfo value;
			lock (exclusionCache)
			{
				if (!exclusionCache.TryGetValue(args.DeviceId, out value))
				{
					Log.Debug(Module.BusinessLogic, $"Device not found in the exclusion cache: {args.DeviceId}");
					return;
				}
				exclusionCache.Remove(args.DeviceId);
			}
			List<RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware.DeviceUpdateInfo> deviceGroup = GetDeviceGroup(value);
			if (deviceGroup.Count == 0)
			{
				if (value == null)
				{
					Log.Warning(Module.BusinessLogic, "Unable to find device info on exclusion, leaking an alert");
				}
				else
				{
					RaiseDeviceReadyForUpdateEvent(value, occurred: false);
				}
			}
			else
			{
				NotifyGroupReadyForUpdate(deviceGroup);
			}
		}
		catch (Exception ex)
		{
			Log.Exception(Module.BusinessLogic, ex, "DeviceFirmwareManager", "Error occurred while handling device exclusion.");
		}
	}

	private List<RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware.DeviceUpdateInfo> GetDeviceGroup(RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware.DeviceUpdateInfo deviceInfo)
	{
		if (deviceInfo != null)
		{
			foreach (IProtocolSpecificDeviceUpdater deviceUpdater in deviceUpdaters)
			{
				List<RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware.DeviceUpdateInfo> list = deviceUpdater.GetDeviceInfo().FindAll((RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware.DeviceUpdateInfo di) => di.Manufacturer == deviceInfo.Manufacturer && di.ProductId == deviceInfo.ProductId && di.HardwareVersion == deviceInfo.HardwareVersion && di.AddInVersion == deviceInfo.AddInVersion);
				if (list.Any())
				{
					return list;
				}
			}
		}
		return new List<RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware.DeviceUpdateInfo>();
	}

	private void OnShcStartupCompleted(ShcStartupCompletedEventArgs args)
	{
		try
		{
			shcStartupCompleted = true;
			Log.Information(Module.BusinessLogic, "DeviceFirmwareManager", "Device FW update check at startup...");
			taskScheduler.AddSchedulerTask(new FixedTimeSpanSchedulerTask(Guid.NewGuid(), CheckDeviceUpdates, TimeSpan.FromHours(Convert.ToDouble(configuration.DeviceFirmwareCheckTime)).Add(TimeSpan.FromMinutes(new Random().Next(30)))));
			CheckDeviceUpdates();
			deviceUpdaters.ForEach(delegate(IProtocolSpecificDeviceUpdater updater)
			{
				List<RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware.DeviceUpdateInfo> deviceInfo = updater.GetDeviceInfo();
				foreach (RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware.DeviceUpdateInfo item in deviceInfo)
				{
					CacheForExclusion(item);
				}
				foreach (RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware.DeviceUpdateInfo item2 in deviceInfo.Where((RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware.DeviceUpdateInfo di) => di.IsReachable))
				{
					HandleDeviceUpdateInfoUpdateStateChanged(item2);
				}
				IEnumerable<IGrouping<short, RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware.DeviceUpdateInfo>> enumerable = from di in deviceInfo
					group di by di.Manufacturer;
				foreach (IGrouping<short, RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware.DeviceUpdateInfo> item3 in enumerable)
				{
					IEnumerable<IGrouping<uint, RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware.DeviceUpdateInfo>> enumerable2 = from di in item3
						group di by di.ProductId;
					foreach (IGrouping<uint, RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware.DeviceUpdateInfo> item4 in enumerable2)
					{
						NotifyGroupReadyForUpdate(item4.ToList());
					}
				}
			});
		}
		catch (Exception ex)
		{
			Log.Exception(Module.BusinessLogic, ex, "DeviceFirmwareManager", "Error occurred while handling startup completed.");
		}
	}

	private void CacheForExclusion(RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware.DeviceUpdateInfo device)
	{
		if (device != null)
		{
			lock (exclusionCache)
			{
				exclusionCache[device.DeviceId] = device;
			}
		}
	}

	private IProtocolSpecificDeviceUpdater GetDeviceUpdater(Guid deviceId)
	{
		IProtocolSpecificDeviceUpdater value;
		lock (syncLock)
		{
			if (!updaterCache.TryGetValue(deviceId, out value))
			{
				foreach (IProtocolSpecificDeviceUpdater deviceUpdater in deviceUpdaters)
				{
					RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware.DeviceUpdateInfo deviceInfo = deviceUpdater.GetDeviceInfo(deviceId);
					if (deviceInfo != null)
					{
						updaterCache.Add(deviceId, deviceUpdater);
						value = deviceUpdater;
						break;
					}
				}
			}
		}
		if (value == null)
		{
			Log.Debug(Module.BusinessLogic, $"Could not found device updater for device with ID={deviceId}");
		}
		return value;
	}

	private RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware.DeviceUpdateInfo GetDeviceUpdateInfo(Guid deviceId)
	{
		IProtocolSpecificDeviceUpdater deviceUpdater = GetDeviceUpdater(deviceId);
		if (deviceUpdater != null)
		{
			RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware.DeviceUpdateInfo deviceInfo = deviceUpdater.GetDeviceInfo(deviceId);
			if (deviceInfo != null)
			{
				return deviceInfo;
			}
		}
		return null;
	}

	private List<RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware.DeviceUpdateInfo> GetDeviceGroup(Guid deviceId)
	{
		RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware.DeviceUpdateInfo deviceUpdateInfo = GetDeviceUpdateInfo(deviceId);
		return GetDeviceGroup(deviceUpdateInfo);
	}

	public void PerformUpdate(List<Guid> deviceList)
	{
		foreach (IProtocolSpecificDeviceUpdater deviceUpdater in deviceUpdaters)
		{
			deviceUpdater.PerformUpdate(deviceList);
			foreach (Guid device in deviceList)
			{
				NotifyGroupReadyForUpdate(GetDeviceGroup(device));
			}
		}
	}

	public void RegisterUpdater(IProtocolSpecificDeviceUpdater updater)
	{
		if (!deviceUpdaters.Contains(updater))
		{
			deviceUpdaters.Add(updater);
			updater.UpdateFailed += HandleFailedDeviceUpdate;
		}
	}

	public void Initialize()
	{
		SubscribeToEvents();
	}

	public void Uninitialize()
	{
		UnsubscribeFromEvents();
	}

	private void HandleFailedDeviceUpdate(object sender, UpdateFailedEventArgs args)
	{
		IncrementDeviceUpdateCounter(args.DeviceId);
		if (args.UpdateStep == FailedUpdateStep.Update)
		{
			RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware.DeviceUpdateInfo deviceUpdateInfo = GetDeviceUpdateInfo(args.DeviceId);
			if (deviceUpdateInfo != null && deviceUpdateInfo.IsAppIdValid())
			{
				eventManager.GetEvent<DeviceUpdateFailedEvent>().Publish(new DeviceUpdateFailedEventArgs
				{
					DeviceId = args.DeviceId,
					AppId = deviceUpdateInfo.AppID,
					PhysicalDeviceType = deviceUpdateInfo.DeviceType
				});
			}
		}
	}

	private void HandleDeviceUpdateInfoUpdateStateChanged(RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware.DeviceUpdateInfo deviceInfo)
	{
		if (deviceInfo != null)
		{
			IProtocolSpecificDeviceUpdater deviceUpdater = GetDeviceUpdater(deviceInfo.DeviceId);
			switch (deviceInfo.UpdateState)
			{
			case DeviceUpdateState.UpdatePending:
				Log.Debug(Module.BusinessLogic, "DeviceFirmwareManager", "Trying to apply pending FW update for device:" + deviceInfo);
				deviceUpdater.PerformUpdate(new List<Guid> { deviceInfo.DeviceId });
				break;
			default:
				StartNewFirmwareTransfer(deviceUpdater, deviceInfo);
				break;
			case DeviceUpdateState.ImageTransferred:
			case DeviceUpdateState.Updating:
				break;
			}
		}
	}

	private void StartNewFirmwareTransfer(IProtocolSpecificDeviceUpdater deviceUpdater, RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware.DeviceUpdateInfo deviceUpdateInfo)
	{
		DeviceFirmwareDescriptor firmware = firmwareImagesService.GetFirmware(deviceUpdateInfo.GetDescriptor());
		if (firmware != null)
		{
			Log.Debug(Module.BusinessLogic, "DeviceFirmwareManager", $"Found downloaded FW for device: {deviceUpdateInfo} {firmware}");
			deviceUpdater.EnqueueFirmwareTransfer(deviceUpdateInfo.DeviceId, firmware);
			IncrementDeviceUpdateCounter(deviceUpdateInfo.DeviceId);
		}
	}

	private void EvaluateDevice(Guid deviceId)
	{
		lock (deviceUpdateRetriesLock)
		{
			if (deviceUpdateRetries.ContainsKey(deviceId) && deviceUpdateRetries[deviceId] > 3)
			{
				return;
			}
		}
		RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware.DeviceUpdateInfo deviceUpdateInfo = GetDeviceUpdateInfo(deviceId);
		if (deviceUpdateInfo != null && deviceUpdateInfo.IsReachable)
		{
			HandleDeviceUpdateInfoUpdateStateChanged(deviceUpdateInfo);
		}
	}

	private void NotifyGroupReadyForUpdate(List<RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware.DeviceUpdateInfo> deviceGroup)
	{
		foreach (RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware.DeviceUpdateInfo item in deviceGroup)
		{
			bool occurred = IsDeviceReadyForUpdate(item);
			RaiseDeviceReadyForUpdateEvent(item, occurred);
		}
	}

	private bool IsDeviceReadyForUpdate(RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware.DeviceUpdateInfo deviceUpdateInfo)
	{
		if (deviceUpdateInfo.IsReachable)
		{
			return deviceUpdateInfo.UpdateState == DeviceUpdateState.ImageTransferred;
		}
		return false;
	}

	private void NotifyAppDeviceGroupReady(string appId)
	{
		foreach (IProtocolSpecificDeviceUpdater deviceUpdater in deviceUpdaters)
		{
			List<RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware.DeviceUpdateInfo> deviceGroup = deviceUpdater.GetDeviceInfo().FindAll((RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware.DeviceUpdateInfo di) => di.AppID == appId);
			NotifyGroupReadyForUpdate(deviceGroup);
		}
	}

	private void CheckAppDevicesUpdateStatusChanges(string appId)
	{
		IEnumerable<List<RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware.DeviceUpdateInfo>> enumerable = from updater in deviceUpdaters
			select updater.GetDeviceInfo().FindAll((RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware.DeviceUpdateInfo di) => di.AppID == appId) into appDeviceGroup
			where appDeviceGroup.Count > 0
			select appDeviceGroup;
		foreach (List<RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware.DeviceUpdateInfo> item in enumerable)
		{
			item.ForEach(HandleDeviceUpdateInfoUpdateStateChanged);
		}
	}

	private void RaiseDeviceReadyForUpdateEvent(RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware.DeviceUpdateInfo deviceUpdateInfo, bool occurred)
	{
		if (deviceUpdateInfo.IsAppIdValid())
		{
			eventManager.GetEvent<DeviceReadyForUpdateEvent>().Publish(new DeviceReadyForUpdateEventArgs
			{
				DeviceId = deviceUpdateInfo.DeviceId,
				PhysicalDeviceType = deviceUpdateInfo.DeviceType,
				AppId = deviceUpdateInfo.AppID,
				Occurred = occurred
			});
		}
	}

	private void CheckDeviceUpdates()
	{
		if (checkAlreadyRunning)
		{
			return;
		}
		if (firmwareImagesService == null)
		{
			checkAlreadyRunning = false;
			Log.Error(Module.BusinessLogic, "Device firmware update service client not initialized.");
			return;
		}
		try
		{
			Log.Information(Module.BusinessLogic, "DeviceFirmwareManager", "Checking BE for device updates...");
			checkAlreadyRunning = true;
			QueueUpDeviceChecks();
		}
		catch (Exception ex)
		{
			Log.Exception(Module.BusinessLogic, "DeviceFirmwareManager", ex, "Unexpected error while checking for device update");
		}
		finally
		{
			checkAlreadyRunning = false;
		}
	}

	private void QueueUpDeviceChecks()
	{
		List<DeviceDescriptor> list = new List<DeviceDescriptor>();
		foreach (IProtocolSpecificDeviceUpdater deviceUpdater in deviceUpdaters)
		{
			List<RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware.DeviceUpdateInfo> deviceInfo = deviceUpdater.GetDeviceInfo();
			list.AddRange(from di in deviceInfo
				select di.GetDescriptor() into x
				group x by new { x.ProductId, x.Manufacturer, x.HardwareVersion, x.FirmwareVersion, x.AddInVersion } into obj
				select obj.First());
		}
		foreach (DeviceDescriptor item in list)
		{
			Log.Debug(Module.BusinessLogic, "DeviceFirmwareManager", "Checking updates for device group: " + item.FriendlyTrace());
			firmwareImagesService.CheckDeviceUpdate(item);
		}
		CleanupDownloadedUpdates(list);
	}

	private void CleanupDownloadedUpdates(List<DeviceDescriptor> includedDeviceGroups)
	{
		try
		{
			List<DeviceDescriptor> neededFirmwareImages = includedDeviceGroups.Select((DeviceDescriptor x) => new DeviceDescriptor
			{
				ProductId = x.ProductId,
				Manufacturer = x.Manufacturer,
				HardwareVersion = x.HardwareVersion,
				FirmwareVersion = x.FirmwareVersion
			}).ToList();
			firmwareImagesService.RemoveUnneededImages(neededFirmwareImages);
		}
		catch (Exception ex)
		{
			Log.Exception(Module.BusinessLogic, "DeviceFirmwareManager", ex, "Unexpected error encountered while cleaning up firmware repository.");
		}
	}
}
