using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Sensors;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Logging;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.IntegrityManagement;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LogicalDeviceStateRepository;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LogicalDeviceStateRepository.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.CommonFunctionality.Interfaces;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.DataAccessInterfaces.Applications;
using RWE.SmartHome.SHC.StartupLogicInterfaces.Events;

namespace RWE.SmartHome.SHC.BusinessLogic.LogicalDeviceStateRepository;

public class LogicalDeviceStateRepository : ILogicalDeviceStateRepository
{
	private const string LoggerSource = "LogicalDeviceStateRepository";

	private const string WdsPersistenceIsolatedStorageId = "sh://core.builtin.WdsStatePersistence";

	private readonly Dictionary<Guid, LogicalDeviceState> deviceStates;

	private readonly IEventManager eventManager;

	private readonly IRepository configurationRepository;

	private readonly IApplicationsHost applicationsHost;

	private readonly IDateTimeProvider dateTimeProvider;

	private IApplicationsSettings isolatedStorage;

	public LogicalDeviceStateRepository(IEventManager eventManager, IRepository configurationRepository, IApplicationsHost applicationsHost, IApplicationsSettings isolatedStorage, IDateTimeProvider dateTimeProvider)
	{
		this.eventManager = eventManager;
		this.configurationRepository = configurationRepository;
		this.applicationsHost = applicationsHost;
		this.isolatedStorage = isolatedStorage;
		this.dateTimeProvider = dateTimeProvider;
		deviceStates = new Dictionary<Guid, LogicalDeviceState>();
		eventManager.GetEvent<DeviceUnreachableChangedEvent>().Subscribe(OnDeviceReachableChanged, null, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<RawLogicalDeviceStateChangedEvent>().Subscribe(OnDeviceStateUpdateReceived, null, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<SoftwareUpdateProgressEvent>().Subscribe(OnSoftwareUpdate, null, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<ShcStartupCompletedEvent>().Subscribe(OnStartupCompleted, (ShcStartupCompletedEventArgs args) => args.Progress == StartupProgress.CompletedRound1, ThreadOption.BackgroundThread, null);
		eventManager.GetEvent<ShutdownEvent>().Subscribe(OnShutdown, null, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<ConfigurationDevicesDeletedEvent>().Subscribe(OnDevicesDeleted, null, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<DeviceInclusionStateChangedEvent>().Subscribe(OnDeviceInclusionStateChanged, null, ThreadOption.PublisherThread, null);
		if (this.applicationsHost != null)
		{
			this.applicationsHost.ApplicationStateChanged += OnApplicationDeactivated;
		}
	}

	private void OnSoftwareUpdate(SoftwareUpdateProgressEventArgs eventArgs)
	{
		if (eventArgs.State == SoftwareUpdateState.Started)
		{
			PersistWdsState();
		}
		if (eventArgs.State == SoftwareUpdateState.Success)
		{
			RetrieveWdsState();
		}
	}

	private void PersistWdsState()
	{
		try
		{
			IEnumerable<Guid> wdsLogicalDevices = from dev in configurationRepository.GetLogicalDevices()
				where dev.DeviceType == "WindowDoorSensor"
				select dev.Id;
			List<LogicalDeviceState> list;
			lock (deviceStates)
			{
				list = deviceStates.Values.Where((LogicalDeviceState s) => wdsLogicalDevices.Contains(s.LogicalDeviceId)).ToList();
			}
			foreach (LogicalDeviceState item in list)
			{
				BooleanProperty booleanProperty = item.GetProperties().FirstOrDefault((Property prop) => prop.Name == "IsOpen") as BooleanProperty;
				isolatedStorage.SetValue(new ConfigurationItem
				{
					ApplicationId = "sh://core.builtin.WdsStatePersistence",
					Name = item.LogicalDeviceId.ToString(),
					Value = ((booleanProperty != null) ? booleanProperty.ValueStr : "null")
				});
				Log.Debug(Module.BusinessLogic, "LogicalDeviceStateRepository: Saved state of WDS  " + item.LogicalDeviceId);
			}
		}
		catch (Exception ex)
		{
			Log.Error(Module.BusinessLogic, "LogicalDeviceStateRepository: Failed to persist the state of a windows sensor\n" + ex.ToString());
		}
	}

	private void RetrieveWdsState()
	{
		try
		{
			List<ConfigurationItem> allSettings = isolatedStorage.GetAllSettings("sh://core.builtin.WdsStatePersistence");
			lock (deviceStates)
			{
				foreach (ConfigurationItem item in allSettings)
				{
					Guid guid = new Guid(item.Name);
					if (configurationRepository.GetLogicalDevice(guid) is WindowDoorSensor windowDoorSensor)
					{
						GenericDeviceState genericDeviceState = new GenericDeviceState();
						genericDeviceState.LogicalDeviceId = guid;
						genericDeviceState.Properties = new List<Property>
						{
							new BooleanProperty
							{
								Name = "IsOpen",
								ValueStr = item.Value,
								UpdateTimestamp = ShcDateTime.UtcNow
							}
						};
						GenericDeviceState value = genericDeviceState;
						deviceStates.Add(guid, value);
						Log.Debug(Module.BusinessLogic, "LogicalDeviceStateRepository: Successfully retrieved state of WDS named " + windowDoorSensor.Name);
					}
				}
			}
			isolatedStorage.RemoveAllApplicationSettings("sh://core.builtin.WdsStatePersistence");
		}
		catch (Exception ex)
		{
			Log.Error(Module.BusinessLogic, "LogicalDeviceStateRepository: Failed to retrieve the state of a windows sensor\n" + ex.ToString());
		}
	}

	private void OnApplicationDeactivated(ApplicationLoadStateChangedEventArgs eventArgs)
	{
		if (eventArgs.ApplicationState != ApplicationStates.ApplicationDeactivated && eventArgs.ApplicationState != ApplicationStates.ApplicationsUninstalled)
		{
			return;
		}
		lock (deviceStates)
		{
			List<LogicalDevice> list = configurationRepository.GetOriginalLogicalDevices().FindAll((LogicalDevice d) => d.BaseDevice != null && d.BaseDevice.AppId == eventArgs.Application.ApplicationId && deviceStates.ContainsKey(d.Id)).ToList();
			if (list != null && list.Count > 0)
			{
				list.ForEach(delegate(LogicalDevice s)
				{
					Log.Debug(Module.BusinessLogic, $"Need to remove state for [{s.Name}]=({s.Id})");
				});
				list.ForEach(delegate(LogicalDevice logDev)
				{
					RemoveLogicalState(logDev.Id);
				});
			}
		}
	}

	private void OnDeviceStateUpdateReceived(RawLogicalDeviceStateChangedEventArgs args)
	{
		try
		{
			if (args == null)
			{
				Log.Warning(Module.BusinessLogic, "NULL arg in OnDeviceStateUpdateReceived");
				return;
			}
			LogicalDeviceState newDeviceState = args.LogicalDeviceState;
			if (newDeviceState == null || newDeviceState.LogicalDevice == null)
			{
				return;
			}
			if (SetDeviceState(newDeviceState.LogicalDeviceId, ref newDeviceState, out var oldDeviceState))
			{
				StateChangeLogInfo(oldDeviceState, newDeviceState);
				LogicalDeviceState logicalDeviceState = newDeviceState.Clone();
				logicalDeviceState?.GetProperties()?.ForEach(delegate(Property prop)
				{
					prop.UpdateTimestamp = null;
				});
				LogicalDeviceStateChangedEvent logicalDeviceStateChangedEvent = eventManager.GetEvent<LogicalDeviceStateChangedEvent>();
				logicalDeviceStateChangedEvent.Publish(new LogicalDeviceStateChangedEventArgs(newDeviceState.LogicalDeviceId, oldDeviceState, logicalDeviceState));
				StateChangeLogInfo(oldDeviceState, newDeviceState);
			}
			if (newDeviceState != null && newDeviceState.LogicalDevice != null && newDeviceState.LogicalDevice.Id != newDeviceState.LogicalDeviceId)
			{
				SetDeviceState(newDeviceState.LogicalDevice.Id, ref newDeviceState, out oldDeviceState);
			}
		}
		catch (Exception ex)
		{
			Log.Error(Module.BusinessLogic, "LogicalDeviceStateRepository", ex.ToString());
		}
	}

	private void OnStartupCompleted(ShcStartupCompletedEventArgs args)
	{
		RetrieveWdsState();
	}

	private void OnShutdown(ShutdownEventArgs args)
	{
		PersistWdsState();
	}

	private void StateChangeLogInfo(LogicalDeviceState oldState, LogicalDeviceState newState)
	{
		if (ModuleInfos.GetLogLevel(Module.BusinessLogic) != Severity.Debug)
		{
			return;
		}
		Guid guid = Guid.Empty;
		string arg = "";
		if (oldState != null)
		{
			if (oldState.LogicalDevice != null)
			{
				arg = oldState.LogicalDevice.Name;
			}
			guid = oldState.LogicalDeviceId;
			oldState.ToString();
		}
		if (newState != null)
		{
			if (newState.LogicalDevice != null)
			{
				arg = newState.LogicalDevice.Name;
			}
			guid = newState.LogicalDeviceId;
			newState.ToString();
		}
		Log.Debug(Module.BusinessLogic, $"Received device status update from device {arg} with id {guid}");
	}

	private void OnDeviceReachableChanged(DeviceUnreachableChangedEventArgs args)
	{
		if (args.Unreachable)
		{
			RemoveLogicalStateForDevice(args.DeviceId);
		}
	}

	private void OnDeviceInclusionStateChanged(DeviceInclusionStateChangedEventArgs args)
	{
		if (args.DeviceInclusionState == DeviceInclusionState.FactoryReset)
		{
			RemoveLogicalStateForDevice(args.DeviceId);
		}
	}

	private void RemoveLogicalStateForDevice(Guid physicalDeviceId)
	{
		lock (deviceStates)
		{
			List<LogicalDevice> list = (from dev in GetAssociatedLogicalDevices(physicalDeviceId)
				where deviceStates.ContainsKey(dev.Id)
				select dev).ToList();
			if (list.Count > 0)
			{
				list.ForEach(delegate(LogicalDevice s)
				{
					Log.Debug(Module.BusinessLogic, $"Need to remove LogicalState for [{s.Name}]=({s.Id})");
				});
				list.ForEach(delegate(LogicalDevice logDev)
				{
					RemoveLogicalState(logDev.Id);
				});
			}
		}
	}

	private void RemoveLogicalState(Guid logicalDeviceId)
	{
		lock (deviceStates)
		{
			LogicalDeviceStateChangedEvent logicalDeviceStateChangedEvent = eventManager.GetEvent<LogicalDeviceStateChangedEvent>();
			if (deviceStates.ContainsKey(logicalDeviceId))
			{
				LogicalDeviceState logicalDeviceState = deviceStates[logicalDeviceId];
				Log.Debug(Module.BusinessLogic, string.Format("Removing LogicalState for [{0}]=({1})", (logicalDeviceState.LogicalDevice == null) ? "deleted device" : logicalDeviceState.LogicalDevice.Name, logicalDeviceId));
				deviceStates.Remove(logicalDeviceId);
				StateChangeLogInfo(logicalDeviceState, null);
				logicalDeviceStateChangedEvent.Publish(new LogicalDeviceStateChangedEventArgs(logicalDeviceId, logicalDeviceState, null));
				Log.Debug(Module.BusinessLogic, string.Format("Removed LogicalState for [{0}]=({1})", (logicalDeviceState.LogicalDevice == null) ? "deleted3 device" : logicalDeviceState.LogicalDevice.Name, logicalDeviceId));
			}
		}
	}

	private bool SetDeviceState(Guid logicalDeviceId, ref LogicalDeviceState newDeviceState, out LogicalDeviceState oldDeviceState)
	{
		bool result = false;
		oldDeviceState = null;
		lock (deviceStates)
		{
			if (deviceStates.ContainsKey(logicalDeviceId))
			{
				oldDeviceState = deviceStates[logicalDeviceId];
				LogicalDeviceState logicalDeviceState = oldDeviceState.Clone();
				logicalDeviceState.UpdateFrom(newDeviceState, dateTimeProvider.UtcNow);
				newDeviceState = logicalDeviceState;
				if (!oldDeviceState.Equals(newDeviceState))
				{
					deviceStates.Remove(logicalDeviceId);
					deviceStates.Add(logicalDeviceId, newDeviceState);
					result = true;
				}
			}
			else
			{
				deviceStates.Add(logicalDeviceId, newDeviceState);
				result = true;
			}
		}
		return result;
	}

	private void OnDevicesDeleted(ConfigurationDevicesDeletedEventArgs args)
	{
		lock (deviceStates)
		{
			args.DeletedDeviceList.ForEach(delegate(Guid devId)
			{
				try
				{
					LogicalDevice originalLogicalDevice = configurationRepository.GetOriginalLogicalDevice(devId);
					Log.Debug(Module.BusinessLogic, string.Format("Need to remove LogicalState for [{0}] = [{1}]", (originalLogicalDevice != null) ? originalLogicalDevice.Name : "", devId));
					RemoveLogicalState(devId);
				}
				catch
				{
					Log.Error(Module.BusinessLogic, "LogicalDeviceStateRepository", $"Failed to discard LogicalState for device {devId}");
				}
			});
		}
	}

	private IEnumerable<LogicalDevice> GetAssociatedLogicalDevices(Guid physicalDeviceId)
	{
		return configurationRepository.GetOriginalLogicalDevices().FindAll((LogicalDevice d) => d.BaseDevice != null && d.BaseDevice.Id == physicalDeviceId);
	}

	public LogicalDeviceState GetLogicalDeviceState(Guid logicalDeviceId)
	{
		lock (deviceStates)
		{
			return (!deviceStates.ContainsKey(logicalDeviceId)) ? null : deviceStates[logicalDeviceId];
		}
	}

	public List<LogicalDeviceState> GetAllLogicalDeviceStates(params string[] deviceIds)
	{
		List<LogicalDeviceState> list = new List<LogicalDeviceState>();
		lock (deviceStates)
		{
			if (deviceIds != null && deviceIds.Length > 0)
			{
				list.AddRange(from deviceId in deviceIds
					where deviceStates.ContainsKey(deviceId.ToGuid())
					select deviceStates[deviceId.ToGuid()]);
			}
			else
			{
				list = deviceStates.Values.ToList();
			}
		}
		return list;
	}
}
