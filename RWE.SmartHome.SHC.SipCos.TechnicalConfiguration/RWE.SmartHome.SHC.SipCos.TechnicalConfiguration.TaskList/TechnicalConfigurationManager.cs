using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ConfigurationTransformation.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Database;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.DataAccessInterfaces.TechnicalConfiguration;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.DeviceFirmwareUpdate;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.DeviceHandler;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Events;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.ErrorHandling;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.TaskList;

public class TechnicalConfigurationManager : ITechnicalConfigurationManager
{
	private const string LoggingSource = "TechnicalConfiguration";

	private IDeviceManager deviceManager;

	private readonly IEventManager eventManager;

	private readonly ITechnicalConfigurationPersistence persistence;

	private SubscriptionToken deviceFactoryResetDetectedToken;

	private readonly Dictionary<Guid, ConfigurationEntry> tasks = new Dictionary<Guid, ConfigurationEntry>();

	private readonly Dictionary<Guid, Guid> pendingChanges = new Dictionary<Guid, Guid>();

	private readonly DeviceConfigurationStates deviceConfigStates = new DeviceConfigurationStates();

	private bool persistenceLoaded;

	public TechnicalConfigurationManager(IDeviceManager deviceManager, IEventManager eventManager, ITechnicalConfigurationPersistence persistence)
	{
		this.deviceManager = deviceManager;
		this.deviceManager.SequenceFinished += SequenceFinished;
		this.eventManager = eventManager;
		this.persistence = persistence;
		deviceFactoryResetDetectedToken = eventManager.GetEvent<DeviceInclusionStateChangedEvent>().Subscribe(DeviceFactoryReset, (DeviceInclusionStateChangedEventArgs args) => args.DeviceInclusionState == DeviceInclusionState.FactoryReset, ThreadOption.PublisherThread, null);
	}

	~TechnicalConfigurationManager()
	{
		if (deviceFactoryResetDetectedToken != null)
		{
			eventManager.GetEvent<DeviceInclusionStateChangedEvent>().Unsubscribe(deviceFactoryResetDetectedToken);
			deviceFactoryResetDetectedToken = null;
		}
		if (deviceManager != null)
		{
			deviceManager.SequenceFinished -= SequenceFinished;
			deviceManager = null;
		}
	}

	private void DeviceFactoryReset(DeviceInclusionStateChangedEventArgs eventArgs)
	{
		if (!tasks.ContainsKey(eventArgs.DeviceId))
		{
			return;
		}
		tasks.Remove(eventArgs.DeviceId);
		using DatabaseConnection databaseConnection = persistence.OpenDatabaseTransaction();
		try
		{
			persistence.Delete(eventArgs.DeviceId);
			databaseConnection.CommitTransaction();
		}
		catch (Exception ex)
		{
			LogError(ErrorCode.DeviceFactoryResetPersistenceFailed, eventArgs.DeviceId, ex);
			databaseConnection.RollbackTransaction();
		}
	}

	private void SequenceFinished(object sender, SequenceFinishedEventArgs e)
	{
		lock (deviceManager.DeviceList.SyncRoot)
		{
			if (!pendingChanges.TryGetValue(e.CorrelationId, out var value))
			{
				return;
			}
			pendingChanges.Remove(e.CorrelationId);
			if (!tasks.TryGetValue(value, out var value2) || !value2.PendingChanges.TryGetValue(e.CorrelationId, out var value3))
			{
				return;
			}
			value2.PendingChanges.Remove(e.CorrelationId);
			Log.Debug(Module.ConfigurationTransformation, "TechnicalConfiguration", OutputFinishedChange(value2.Address, value3, e.State));
			if (e.State == SequenceState.Success)
			{
				value3.ApplyTo(value2.CurrentConfig);
			}
			if (value2.PendingChanges.Count == 0)
			{
				PersistConfigurationEntry(value, value2);
				IDeviceInformation deviceInformation = deviceManager.DeviceList[value];
				if (value2.LinkUpdatePendingConfig != null && e.State == SequenceState.Success && deviceInformation.UpdateState == CosIPDeviceUpdateState.UpToDate)
				{
					ClearLinkUpdatePendingOnLinkPartners(value, value2);
					value2.LinkUpdatePendingConfig = null;
				}
				deviceConfigStates.ReceivedNewConfigState(value);
				eventManager.GetEvent<DeviceConfigurationFinishedEvent>().Publish(new DeviceConfigurationFinishedEventArgs(value, e.State == SequenceState.Success));
			}
		}
	}

	private void PersistConfigurationEntry(Guid deviceId, ConfigurationEntry entry)
	{
		using DatabaseConnection databaseConnection = persistence.OpenDatabaseTransaction();
		try
		{
			persistence.Save(GetDatabaseEntity(new KeyValuePair<Guid, ConfigurationEntry>(deviceId, entry)));
			databaseConnection.CommitTransaction();
		}
		catch (Exception ex)
		{
			LogError(ErrorCode.PersistConfigurationFailed, deviceId, ex);
			databaseConnection.RollbackTransaction();
		}
	}

	private static TechnicalConfigurationEntity GetDatabaseEntity(KeyValuePair<Guid, ConfigurationEntry> entry)
	{
		using MemoryStream memoryStream = new MemoryStream();
		using BinaryWriter writer = new BinaryWriter(memoryStream);
		entry.Value.CurrentConfig.Save(writer);
		TechnicalConfigurationEntity technicalConfigurationEntity = new TechnicalConfigurationEntity();
		technicalConfigurationEntity.Id = entry.Key;
		technicalConfigurationEntity.Data = memoryStream.ToArray();
		return technicalConfigurationEntity;
	}

	public void LoadAllFromPersistence()
	{
		IEnumerable<TechnicalConfigurationEntity> enumerable = persistence.LoadAll();
		deviceManager.RemoveAllPendingConfigurationUpdates();
		tasks.Clear();
		pendingChanges.Clear();
		foreach (TechnicalConfigurationEntity item in enumerable)
		{
			using MemoryStream input = new MemoryStream(item.Data);
			using BinaryReader reader = new BinaryReader(input);
			DeviceConfiguration currentConfig = new DeviceConfiguration(reader);
			tasks.Add(item.Id, new ConfigurationEntry
			{
				CurrentConfig = currentConfig
			});
		}
	}

	public void LoadFromPersistence(IEnumerable<Guid> ids)
	{
		List<Guid> idsList = ids.ToList();
		List<TechnicalConfigurationEntity> list = (from m in persistence.LoadAll()
			where idsList.Contains(m.Id)
			select m).ToList();
		deviceManager.RemoveAllPendingConfigurationUpdates();
		tasks.Clear();
		pendingChanges.Clear();
		foreach (TechnicalConfigurationEntity item in list)
		{
			using MemoryStream input = new MemoryStream(item.Data);
			using BinaryReader reader = new BinaryReader(input);
			DeviceConfiguration currentConfig = new DeviceConfiguration(reader);
			tasks.Add(item.Id, new ConfigurationEntry
			{
				CurrentConfig = currentConfig
			});
		}
	}

	public void SetConfiguration(IEnumerable<Guid> devicesToRemove, IList<TechnicalConfigurationTask> allDeviceConfigurations)
	{
		lock (deviceManager.DeviceList.SyncRoot)
		{
			if (!persistenceLoaded)
			{
				LoadFromPersistence(allDeviceConfigurations.Select((TechnicalConfigurationTask m) => m.DeviceId));
				persistenceLoaded = true;
			}
			if (pendingChanges.Count > 0)
			{
				Log.Debug(Module.ConfigurationTransformation, "TechnicalConfiguration", OutputPendingChanges($"{pendingChanges.Count} unconfigured sequences from last commit:"));
			}
			deviceManager.RemoveAllPendingConfigurationUpdates();
			SetAllConfigurations(devicesToRemove, allDeviceConfigurations);
		}
	}

	private void ExcludeRemovedDevices(IEnumerable<Guid> devicesToRemoveByUiConfig, IEnumerable<TechnicalConfigurationTask> allDeviceConfigurations)
	{
		if (devicesToRemoveByUiConfig == null || devicesToRemoveByUiConfig.Count() == 0)
		{
			return;
		}
		List<Guid> devicesToKeep = allDeviceConfigurations.Select((TechnicalConfigurationTask c) => c.DeviceId).ToList();
		List<Guid> collection = (from m in deviceManager.DeviceList
			where m.ProtocolType == ProtocolType.BidCos && m.ManufacturerDeviceType == 22
			select m.DeviceId).ToList();
		devicesToKeep.AddRange(collection);
		IEnumerable<IDeviceInformation> source = deviceManager.DeviceList.Where((IDeviceInformation d) => d.DeviceInclusionState != DeviceInclusionState.Found);
		IEnumerable<Guid> first = from configurationEntry in tasks
			where !devicesToKeep.Contains(configurationEntry.Key)
			select configurationEntry.Key;
		if (devicesToRemoveByUiConfig != null)
		{
			first = first.Union(devicesToRemoveByUiConfig);
		}
		first = first.Union(from device in source
			where !devicesToKeep.Contains(device.DeviceId)
			select device.DeviceId).ToList();
		if (first.Count() == 0)
		{
			return;
		}
		foreach (Guid item in first)
		{
			tasks.Remove(item);
			if (deviceManager.DeviceList.Contains(item))
			{
				deviceManager.ExcludeDevice(item);
			}
		}
		using DatabaseConnection databaseConnection = persistence.OpenDatabaseTransaction();
		try
		{
			foreach (Guid item2 in first)
			{
				persistence.Delete(item2);
			}
			databaseConnection.CommitTransaction();
		}
		catch (Exception ex)
		{
			LogError(ErrorCode.DeletePersistedConfigurationFailed, Guid.Empty, ex);
			databaseConnection.RollbackTransaction();
			throw;
		}
	}

	private void SetAllConfigurations(IEnumerable<Guid> devicesToRemove, IList<TechnicalConfigurationTask> allDeviceConfigurations)
	{
		ExcludeRemovedDevices(devicesToRemove, allDeviceConfigurations);
		PerformanceMonitoring.PrintMemoryUsage("Removed devices excluded");
		if (allDeviceConfigurations == null)
		{
			return;
		}
		pendingChanges.Clear();
		List<KeyValuePair<Guid, ConfigurationEntry>> list = new List<KeyValuePair<Guid, ConfigurationEntry>>();
		foreach (TechnicalConfigurationTask allDeviceConfiguration in allDeviceConfigurations)
		{
			SetDeviceConfiguration(allDeviceConfiguration, list);
		}
		PerformanceMonitoring.PrintMemoryUsage("device configuration set");
		foreach (KeyValuePair<Guid, ConfigurationEntry> item in list)
		{
			SaveLinkConfigUpdatePendingConfigurations(item.Key, item.Value);
		}
		PerformanceMonitoring.PrintMemoryUsage("link configuration set");
		List<Guid> list2 = new List<Guid>();
		ISipcosConfigurator sipcosConfigurator = deviceManager.CreateSipcosConfigurator();
		using (DatabaseConnection databaseConnection = persistence.OpenDatabaseTransaction())
		{
			try
			{
				foreach (KeyValuePair<Guid, ConfigurationEntry> task in tasks)
				{
					ConfigurationEntry value = task.Value;
					DeviceConfigurationDiff deviceConfigurationDiff = value.ReferenceToCurrentDiff ?? value.CurrentConfig.CreateDiffAndMark(value.ReferenceConfig);
					DeviceConfigurator deviceConfigurator = new DeviceConfigurator(deviceManager, sipcosConfigurator, pendingChanges, task.Value.PendingChanges, task.Key);
					deviceConfigurator.ConfigureDevice(deviceConfigurationDiff);
					foreach (KeyValuePair<byte, ConfigurationChannelDiff> channelDiff in deviceConfigurationDiff.ChannelDiffs)
					{
						if (channelDiff.Value.ToChange.Count > 0 || channelDiff.Value.ToCreate.Count > 0 || channelDiff.Value.ToDelete.Count > 0)
						{
							list2.Add(task.Key);
							try
							{
								persistence.Save(GetDatabaseEntity(new KeyValuePair<Guid, ConfigurationEntry>(task.Key, task.Value)));
							}
							catch (Exception ex)
							{
								LogError(ErrorCode.PersistConfigurationFailed, task.Key, ex);
								throw;
							}
							break;
						}
					}
				}
				databaseConnection.CommitTransaction();
			}
			catch (Exception)
			{
				databaseConnection.RollbackTransaction();
				throw;
			}
		}
		deviceConfigStates.ClearAllStates();
		if (list2.Any())
		{
			ThreadPool.QueueUserWorkItem(MarkDevicesPending, list2);
		}
		sipcosConfigurator.FlushToSendQueue();
		PerformanceMonitoring.PrintMemoryUsage("configuration updates enqueued");
	}

	private void MarkDevicesPending(object param)
	{
		IEnumerable<Guid> enumerable = (IEnumerable<Guid>)param;
		foreach (Guid item in enumerable)
		{
			IDeviceInformation deviceInformation = deviceManager.DeviceList[item];
			if (deviceInformation != null)
			{
				if (!deviceConfigStates.IsReceivedConfigState(item))
				{
					deviceInformation.DeviceConfigurationState = DeviceConfigurationState.Pending;
				}
				else
				{
					Log.Information(Module.ConfigurationTransformation, $"The device(id: {item}) received already a state and it was not set to configuration pending");
				}
			}
		}
	}

	private void SetDeviceConfiguration(TechnicalConfigurationTask configTask, ICollection<KeyValuePair<Guid, ConfigurationEntry>> eventListenerDevices)
	{
		IDeviceInformation deviceInformation = deviceManager.DeviceList[configTask.DeviceId];
		if (deviceInformation == null)
		{
			LogError(ErrorCode.DeviceDoesNotExistForConfig, Guid.Empty, configTask.DeviceId, configTask.DeviceAddress.ToReadable());
			return;
		}
		if (configTask.ReferenceConfiguration == null)
		{
			throw ExceptionFactory.GetException(ErrorCode.ReferenceConfigurationNull, configTask.DeviceId);
		}
		if (!tasks.TryGetValue(configTask.DeviceId, out var value))
		{
			value = new ConfigurationEntry();
			if (deviceInformation.DeviceInclusionState != DeviceInclusionState.FactoryReset)
			{
				tasks.Add(configTask.DeviceId, value);
			}
			if (configTask.DefaultConfiguration == null)
			{
				throw ExceptionFactory.GetException(ErrorCode.DefaultConfigurationNull, configTask.DeviceId);
			}
			value.CurrentConfig = configTask.DefaultConfiguration;
		}
		value.ReferenceConfig = configTask.ReferenceConfiguration;
		value.Address = configTask.DeviceAddress;
		value.LinkUpdatePendingConfig = null;
		value.PendingChanges.Clear();
		if (DeviceInclusionState.Included != deviceInformation.DeviceInclusionState && DeviceInclusionState.FactoryReset != deviceInformation.DeviceInclusionState)
		{
			deviceManager.IncludeDevice(configTask.DeviceId);
		}
		if (LinkUpdatePendingType(value.ReferenceConfig, ChannelType.SensorNeedsFlag))
		{
			value.ReferenceToCurrentDiff = value.CurrentConfig.CreateDiffAndMark(value.ReferenceConfig);
			if (IsLinkConfigUpdatePendingFlagRequired(deviceInformation, value, configTask.SensorConfigurationChangedByAckDelegate))
			{
				eventListenerDevices.Add(new KeyValuePair<Guid, ConfigurationEntry>(configTask.DeviceId, value));
			}
		}
		else
		{
			value.ReferenceToCurrentDiff = null;
		}
	}

	private static bool IsLinkConfigUpdatePendingFlagRequired(IDeviceInformation device, ConfigurationEntry eventListener, bool sensorConfigurationChangedByAckDelegate)
	{
		if (device.UpdateState != CosIPDeviceUpdateState.UpToDate)
		{
			return true;
		}
		foreach (KeyValuePair<byte, ConfigurationChannelDiff> channelDiff in eventListener.ReferenceToCurrentDiff.ChannelDiffs)
		{
			Dictionary<LinkPartner, ConfigurationLink> links = eventListener.CurrentConfig.Channels[channelDiff.Key].Links;
			int num = links.Count((KeyValuePair<LinkPartner, ConfigurationLink> link) => link.Key != LinkPartner.Empty && !link.Value.IsUnknownState);
			if (channelDiff.Value.ToCreate.Count > 0 && num > 0)
			{
				return true;
			}
			if (channelDiff.Value.ToChange.Count > 0)
			{
				return true;
			}
			if (channelDiff.Value.ToDelete.Count > 0 && sensorConfigurationChangedByAckDelegate)
			{
				return true;
			}
		}
		return false;
	}

	private static bool LinkUpdatePendingType(DeviceConfiguration referenceConfig, ChannelType type)
	{
		return referenceConfig.Channels.Any((KeyValuePair<byte, ConfigurationChannel> channel) => channel.Value.ChannelType == type);
	}

	private void SaveLinkConfigUpdatePendingConfigurations(Guid deviceId, ConfigurationEntry eventListener)
	{
		eventListener.LinkUpdatePendingConfig = new List<LinkUpdatePendingPatch>();
		foreach (byte key in eventListener.CurrentConfig.Channels.Keys)
		{
			LinkPartner eventListenerPartner = new LinkPartner(deviceId, eventListener.Address, key);
			if (!GuaranteeSensorNotification(eventListener, eventListener.CurrentConfig.Channels[key], eventListenerPartner))
			{
				GuaranteeSensorNotification(eventListener, eventListener.ReferenceConfig.Channels[key], eventListenerPartner);
			}
		}
	}

	private bool GuaranteeSensorNotification(ConfigurationEntry eventListener, ConfigurationChannel configurationChannel, LinkPartner eventListenerPartner)
	{
		foreach (KeyValuePair<LinkPartner, ConfigurationLink> link in configurationChannel.Links)
		{
			if (link.Key == LinkPartner.Empty)
			{
				continue;
			}
			if (link.Key.DeviceId == Guid.Empty)
			{
				return true;
			}
			LinkPartner key = link.Key;
			if (tasks.TryGetValue(key.DeviceId, out var value) && value.Address.Compare(link.Key.Address))
			{
				ConfigurationChannel configurationChannel2 = value.ReferenceConfig.Channels[key.Channel];
				if (configurationChannel2.ChannelType == ChannelType.ActuatorHasFlag && (!link.Value.IsUnknownState || value.CurrentConfig.Channels[key.Channel].Links.ContainsKey(eventListenerPartner)) && configurationChannel2.Links.TryGetValue(eventListenerPartner, out var value2))
				{
					value2.ApplyDiff(configurationChannel2.SetLinkConfigUpdatePendingConfiguration);
					eventListener.LinkUpdatePendingConfig.Add(new LinkUpdatePendingPatch
					{
						ChannelIndex = eventListenerPartner.Channel,
						Partner = key
					});
					return true;
				}
			}
		}
		return false;
	}

	private void ClearLinkUpdatePendingOnLinkPartners(Guid eventListenerId, ConfigurationEntry eventListener)
	{
		IDeviceInformation deviceInformation = deviceManager.DeviceList[eventListenerId];
		if (deviceInformation == null)
		{
			return;
		}
		byte allOperationModes = deviceInformation.AllOperationModes;
		Log.Debug(Module.ConfigurationTransformation, "TechnicalConfiguration", $"Clearing LinkConfigUpdatePending flags for link partners of event listener {deviceManager.DeviceList.LogInfoByAddress(eventListener.Address)}");
		ISipcosConfigurator sipcosConfigurator = deviceManager.CreateSipcosConfigurator();
		foreach (LinkUpdatePendingPatch item in eventListener.LinkUpdatePendingConfig)
		{
			Guid deviceId = item.Partner.DeviceId;
			if (tasks.TryGetValue(deviceId, out var value))
			{
				LinkPartner linkPartner = new LinkPartner(eventListenerId, eventListener.Address, item.ChannelIndex);
				ConfigurationChannel configurationChannel = value.ReferenceConfig.Channels[item.Partner.Channel];
				ConfigurationLink configurationLink = configurationChannel.Links[linkPartner];
				ConfigurationLink clearLinkConfigUpdatePendingConfiguration = configurationChannel.ClearLinkConfigUpdatePendingConfiguration;
				configurationLink.ApplyDiff(clearLinkConfigUpdatePendingConfiguration);
				DeviceConfigurator deviceConfigurator = new DeviceConfigurator(deviceManager, sipcosConfigurator, pendingChanges, value.PendingChanges, deviceId);
				deviceConfigurator.ConfigureLink(item.Partner.Channel, linkPartner, allOperationModes, clearLinkConfigUpdatePendingConfiguration, withCreate: false);
			}
		}
		sipcosConfigurator.FlushToSendQueue();
	}

	private string OutputPendingChanges(string prefix)
	{
		StringBuilder stringBuilder = new StringBuilder(prefix);
		foreach (KeyValuePair<Guid, Guid> pendingChange in pendingChanges)
		{
			stringBuilder.AppendLine();
			ConfigurationChange value2;
			if (!tasks.TryGetValue(pendingChange.Value, out var value))
			{
				stringBuilder.AppendFormat("Device for sequence not found: {0}", new object[1] { deviceManager.DeviceList.LogInfoByGuid(pendingChange.Value) });
			}
			else if (!value.PendingChanges.TryGetValue(pendingChange.Key, out value2))
			{
				stringBuilder.AppendFormat("Change for sequence not found, device ID: {0}", new object[1] { deviceManager.DeviceList.LogInfoByGuid(pendingChange.Value) });
			}
			else
			{
				stringBuilder.Append(value2.ToString(value.Address.ToReadable()));
			}
		}
		return stringBuilder.ToString();
	}

	private static string OutputFinishedChange(byte[] address, ConfigurationChange change, SequenceState state)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendFormat((state == SequenceState.Success) ? "SequenceFinished successful: " : "SequenceFinished '{0}': ", new object[1] { state });
		stringBuilder.Append(change.ToString(address.ToReadable()));
		return stringBuilder.ToString();
	}

	private void LogError(ErrorCode errorCode, Guid deviceId, params object[] values)
	{
		ResourceManager resourceManager = new ResourceManager(typeof(ErrorStrings));
		string text = resourceManager.GetString(errorCode.ToString());
		string message = (string.IsNullOrEmpty(text) ? errorCode.ToString() : string.Format(text, new object[1] { deviceManager.DeviceList.LogInfoByGuid(deviceId) }.Concat(values).ToArray()));
		Log.Error(Module.ConfigurationTransformation, "TechnicalConfiguration", message);
	}
}
