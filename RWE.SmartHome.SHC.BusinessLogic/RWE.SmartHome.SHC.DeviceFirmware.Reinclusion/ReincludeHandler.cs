using System;
using System.Collections.Generic;
using Microsoft.Practices.Mobile.ContainerModel;
using RWE.SmartHome.Common.ControlNodeSHCContracts;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration.DevicesDefinitions;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.RepositoryOperations;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.RepositoryOperations.Enums;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.Scheduler;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Events;

namespace RWE.SmartHome.SHC.DeviceFirmware.Reinclusion;

public class ReincludeHandler : IDisposable
{
	private class DeviceUpdateInfo
	{
		public Guid DeviceId { get; set; }

		public string FirmwareVersion { get; set; }
	}

	private const string LoggingSource = "ReincludeHandler";

	private const int MaxLockWait = 20000;

	private readonly IScheduler scheduler;

	private readonly IRepository repository;

	private readonly IRepositorySync repositorySync;

	private readonly IEventManager eventManager;

	private Container container;

	private ReincludeTask pendingReincludeTask;

	private readonly object pendingReincludeTaskSync = new object();

	private bool disposed;

	private readonly IDeviceDefinitionsProvider deviceDefinitionsProvider;

	private readonly List<DeviceUpdateInfo> deviceUpdateInfos = new List<DeviceUpdateInfo>();

	private readonly object commitTaskSync = new object();

	public int CommitDelaySeconds { get; set; }

	public ReincludeHandler(Container container, IDeviceDefinitionsProvider deviceDefinitionsProvider)
	{
		CommitDelaySeconds = 20;
		this.container = container;
		eventManager = container.Resolve<IEventManager>();
		eventManager.GetEvent<DeviceUpdateStateChangedEvent>().Subscribe(OnDeviceUpdateStateChanged, null, ThreadOption.PublisherThread, null);
		this.deviceDefinitionsProvider = deviceDefinitionsProvider;
		scheduler = container.Resolve<IScheduler>();
		repository = container.Resolve<IRepository>();
		repositorySync = container.Resolve<IRepositorySync>();
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		if (disposed)
		{
			return;
		}
		if (disposing)
		{
			lock (pendingReincludeTaskSync)
			{
				if (scheduler != null && pendingReincludeTask != null)
				{
					scheduler.RemoveSchedulerTask(pendingReincludeTask.TaskId);
				}
			}
			eventManager.GetEvent<DeviceUpdateStateChangedEvent>().Unsubscribe(OnDeviceUpdateStateChanged);
		}
		disposed = true;
	}

	private void OnDeviceUpdateStateChanged(DeviceUpdateStateChangedEventArgs args)
	{
		if ((args.OldDeviceUpdateState != DeviceUpdateState.UpToDate || args.NewDeviceUpdateState == DeviceUpdateState.UpToDate || args.NewDeviceUpdateState == DeviceUpdateState.TransferInProgress || args.NewDeviceUpdateState == DeviceUpdateState.ImageTransferred) && (args.OldDeviceUpdateState == DeviceUpdateState.UpToDate || args.OldDeviceUpdateState == DeviceUpdateState.TransferInProgress || args.OldDeviceUpdateState == DeviceUpdateState.ImageTransferred || args.NewDeviceUpdateState != DeviceUpdateState.UpToDate))
		{
			return;
		}
		lock (commitTaskSync)
		{
			if (args.OldDeviceUpdateState != DeviceUpdateState.UpToDate && args.NewDeviceUpdateState == DeviceUpdateState.UpToDate)
			{
				deviceUpdateInfos.Add(new DeviceUpdateInfo
				{
					DeviceId = args.DeviceId,
					FirmwareVersion = args.FirmwareVersion
				});
			}
			ScheduleCommitTask();
		}
	}

	private void ReincludeDevices()
	{
		lock (commitTaskSync)
		{
			using (RepositoryLockContext repositoryLockContext = repositorySync.WaitForLock("ReincludeHandler::ReincludeDevices", new RepositoryUpdateContextData(CoreConstants.CoreAppId, ForcePushDeviceConfiguration.Yes)))
			{
				foreach (DeviceUpdateInfo deviceUpdateInfo in deviceUpdateInfos)
				{
					UpdateDevice(deviceUpdateInfo.DeviceId, deviceUpdateInfo.FirmwareVersion);
				}
				deviceUpdateInfos.Clear();
				repositoryLockContext.Commit = true;
			}
			pendingReincludeTask = null;
		}
	}

	private void UpdateDevice(Guid deviceId, string newFirmwareVersion)
	{
		BaseDevice baseDevice = repository.GetBaseDevice(deviceId);
		if (baseDevice == null)
		{
			Log.ErrorFormat(Module.BusinessLogic, "FirmwareUpdate ReincludeHandler", true, "Device {0} marked for update not found.", deviceId);
			throw new Exception("Device marked for update not found");
		}
		FirmwareVersion newFirmwareVersion2 = GetNewFirmwareVersion(baseDevice, newFirmwareVersion);
		if (newFirmwareVersion2 == null)
		{
			return;
		}
		BaseDeviceDefinition deviceDefinition = deviceDefinitionsProvider.GetDeviceDefinition(null, baseDevice.DeviceType, newFirmwareVersion2);
		if (deviceDefinition == null)
		{
			return;
		}
		baseDevice.DeviceVersion = deviceDefinition.GetAttribute<StringPropertyDefinition>("Version").DefaultValue;
		List<Property> collection = deviceDefinition.ConfigurationProperties.GenerateMissingProperties(baseDevice.Properties);
		baseDevice.Properties.AddRange(collection);
		repository.SetBaseDevice(baseDevice);
		foreach (Guid logicalDeviceId in baseDevice.LogicalDeviceIds)
		{
			LogicalDevice logicalDevice = repository.GetLogicalDevice(logicalDeviceId);
			LogicalDeviceDefinition logicalDeviceDefinition = deviceDefinition.LogicalDevices.Find((LogicalDeviceDefinition logicalDeviceDef) => logicalDeviceDef.DeviceType == logicalDevice.DeviceType);
			if (logicalDevice != null)
			{
				List<Property> collection2 = logicalDeviceDefinition.ConfigurationProperties.GenerateMissingProperties(logicalDevice.GetAllProperties());
				logicalDevice.Properties.AddRange(collection2);
				repository.SetLogicalDevice(logicalDevice);
			}
		}
	}

	private FirmwareVersion GetNewFirmwareVersion(BaseDevice device, string newFirmwareVersion)
	{
		FirmwareVersion result = null;
		ProtocolIdentifier protocolId = device.ProtocolId;
		if (protocolId == ProtocolIdentifier.Cosip)
		{
			result = new SipcosFirmwareVersion(newFirmwareVersion);
		}
		return result;
	}

	private void ScheduleCommitTask()
	{
		if (pendingReincludeTask == null)
		{
			pendingReincludeTask = new ReincludeTask(Guid.NewGuid(), ReincludeDevices, runOnce: true)
			{
				DueTime = DateTime.Now.AddSeconds(CommitDelaySeconds)
			};
			scheduler.AddSchedulerTask(pendingReincludeTask);
		}
		else
		{
			pendingReincludeTask.DueTime = DateTime.Now.AddSeconds(CommitDelaySeconds);
		}
	}
}
