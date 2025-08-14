using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Lemonbeat.ProtocolAdapter.Interfaces;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Events;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Interfaces;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Configuration;

internal class ConfigurationController : IConfigurationController
{
	private readonly object syncRoot = new object();

	private bool persistenceLoaded;

	private readonly IDictionary<Guid, ConfigurationInformation> configurations = new SortedList<Guid, ConfigurationInformation>();

	private readonly IDeviceList deviceList;

	private readonly IInclusionController deviceInclusionController;

	private readonly IConfigurationAccess configurationAccess;

	private readonly ILemonbeatPersistence LemonbeatPersistence;

	public ConfigurationController(IDeviceList deviceList, IInclusionController deviceInclusionController, IConfigurationAccess deviceConfigurationController, ILemonbeatPersistence LemonbeatPersistence)
	{
		this.deviceList = deviceList;
		this.deviceInclusionController = deviceInclusionController;
		configurationAccess = deviceConfigurationController;
		this.LemonbeatPersistence = LemonbeatPersistence;
		deviceList.DeviceInclusionStateChanged += OnDeviceInclusionStateChanged;
		configurationAccess.GetDynamicConfigurationCompleted += OnCurrentConfigurationReceived;
		configurationAccess.DeployConfigurationChangesCompleted += OnDeploymentFinished;
		configurationAccess.GetStaticConfigurationCompleted += OnStaticConfigurationReceived;
		deviceList.DeviceReachabilityChanged += OnReachableChanged;
	}

	public void ConfigureDevices(IEnumerable<Guid> devicesToRemove, IEnumerable<Guid> devicesToInclude, IDictionary<Guid, PhysicalConfiguration> targetConfigurations)
	{
		if (!persistenceLoaded)
		{
			LoadCurrentConfigurationFromPersistence();
			persistenceLoaded = true;
		}
		IncludeDevices(devicesToInclude.Where((Guid deviceId) => !targetConfigurations.ContainsKey(deviceId)));
		ExcludeRemovedDevices(devicesToRemove);
		ApplyTargetConfigurations(targetConfigurations);
	}

	public void ResetDevicePartners(Guid deviceId, IRepository configRepository)
	{
		DeviceInformation deviceInformation = deviceList[deviceId];
		if (deviceInformation == null || deviceInformation.Identifier == null)
		{
			return;
		}
		lock (syncRoot)
		{
			foreach (KeyValuePair<Guid, ConfigurationInformation> configuration in configurations)
			{
				List<Partner> partnerToRemove = new List<Partner>();
				if (configuration.Value.Current == null)
				{
					continue;
				}
				foreach (Partner partner in configuration.Value.Current.Partners)
				{
					if (partner.Identifier == deviceInformation.Identifier)
					{
						partnerToRemove.Add(partner);
					}
				}
				configuration.Value.Current.Partners.RemoveAll((Partner p) => partnerToRemove.Contains(p));
			}
		}
	}

	private PhysicalConfigurationDifference GetConfigDifference(Guid deviceId)
	{
		lock (syncRoot)
		{
			if (!configurations.ContainsKey(deviceId))
			{
				return null;
			}
			return configurations[deviceId].DifferencesToBeDeployed;
		}
	}

	private void IncludeDevices(IEnumerable<Guid> devicesToInclude)
	{
		foreach (Guid item in devicesToInclude)
		{
			DeviceInformation deviceInformation = deviceList[item];
			if (deviceInformation != null)
			{
				SynchronizeConfig(deviceInformation);
			}
		}
	}

	private void ApplyTargetConfigurations(IDictionary<Guid, PhysicalConfiguration> targetConfigurations)
	{
		foreach (KeyValuePair<Guid, PhysicalConfiguration> targetConfiguration in targetConfigurations)
		{
			Guid key = targetConfiguration.Key;
			DeviceInformation deviceInformation = deviceList[key];
			if (deviceInformation != null)
			{
				lock (syncRoot)
				{
					if (!configurations.TryGetValue(key, out var value))
					{
						value = new ConfigurationInformation();
						configurations.Add(key, value);
					}
					value.Target = targetConfiguration.Value;
				}
				SynchronizeConfig(deviceInformation);
			}
			else
			{
				Log.Warning(Module.LemonbeatProtocolAdapter, "SetPhysicalConfigurations() was invoked for a device which is no longer in the device list");
			}
		}
	}

	private void SynchronizeConfig(DeviceInformation device)
	{
		if (device.DeviceInclusionState == LemonbeatDeviceInclusionState.Included)
		{
			if (IsStaticConfigAvailable(device))
			{
				bool flag;
				ConfigurationInformation value;
				lock (syncRoot)
				{
					flag = configurations.TryGetValue(device.DeviceId, out value);
				}
				if (flag && value.Current != null)
				{
					if (value.DifferencesToBeDeployed.IsEmpty())
					{
						device.DeviceConfigurationState = DeviceConfigurationState.Complete;
						return;
					}
					device.DeviceConfigurationState = DeviceConfigurationState.Pending;
					DeployConfigurationDifference(device);
				}
				else
				{
					device.DeviceConfigurationState = DeviceConfigurationState.Pending;
					configurationAccess.GetDynamicConfigurationAsync(device);
				}
			}
			else
			{
				device.DeviceConfigurationState = DeviceConfigurationState.Pending;
				configurationAccess.GetStaticConfigurationAsync(device);
			}
		}
		else
		{
			deviceInclusionController.IncludeAsync(device);
		}
	}

	private void OnReachableChanged(object sender, DeviceReachabilityChangedEventArgs args)
	{
		lock (syncRoot)
		{
			if (!configurations.TryGetValue(args.Device.DeviceId, out var _))
			{
				return;
			}
		}
		if (args.IsReachable)
		{
			SynchronizeConfig(args.Device);
		}
	}

	private void OnDeviceInclusionStateChanged(object sender, LemonbeatDeviceInclusionStateChangedEventArgs args)
	{
		if (args.DeviceInclusionState == LemonbeatDeviceInclusionState.Included)
		{
			DeviceInformation deviceInformation = deviceList[args.DeviceId];
			if (deviceInformation == null)
			{
				return;
			}
			SynchronizeConfig(deviceInformation);
		}
		if (args.DeviceInclusionState != LemonbeatDeviceInclusionState.FactoryReset)
		{
			return;
		}
		lock (syncRoot)
		{
			if (configurations.ContainsKey(args.DeviceId))
			{
				RemoveConfiguration(args.DeviceId);
			}
		}
	}

	private void OnCurrentConfigurationReceived(object sender, CurrentConfigurationReceivedEventArgs configurationReceivedEventArgs)
	{
		Guid deviceId = configurationReceivedEventArgs.DeviceId;
		DeviceInformation deviceInformation = deviceList[deviceId];
		lock (syncRoot)
		{
			if (deviceInformation == null || !configurations.TryGetValue(deviceId, out var value))
			{
				Log.Warning(Module.LemonbeatProtocolAdapter, $"Received current configuration for device with id {deviceId} but its is no longer in the list.");
				deviceInformation.DeviceConfigurationState = DeviceConfigurationState.Complete;
				return;
			}
			value.Current = configurationReceivedEventArgs.Configuration;
			LemonbeatPersistence.SaveConfiguration(deviceId, value.Current, suppressEvent: false);
		}
		SynchronizeConfig(deviceInformation);
	}

	private void DeployConfigurationDifference(DeviceInformation device)
	{
		if (device.IsReachable)
		{
			configurationAccess.DeployConfigurationAsync(device, GetConfigDifference);
		}
		else
		{
			Log.Debug(Module.LemonbeatProtocolAdapter, $"ConfigurationController.DeployConfigurationDifference(): Device is not reachable, the config deployment is postponed for {device.ToString()}");
		}
	}

	private void OnDeploymentFinished(object sender, DeployConfigurationChangesCompletedEventArgs sequenceFinishedEventArgs)
	{
		DeviceInformation deviceInformation = deviceList[sequenceFinishedEventArgs.DeviceId];
		ConfigurationInformation configurationInformation;
		lock (syncRoot)
		{
			if (!configurations.ContainsKey(sequenceFinishedEventArgs.DeviceId) || configurations[sequenceFinishedEventArgs.DeviceId].Current == null)
			{
				Log.Warning(Module.LemonbeatProtocolAdapter, "ConfigurationController.OnDeploymentFinished called for a device which is not in the dictionary or whose current configuration was not retrieved yet. Possibly because of factory reset");
				return;
			}
			configurationInformation = configurations[sequenceFinishedEventArgs.DeviceId];
			configurationInformation.UpdateCurrentConfiguration(sequenceFinishedEventArgs.DeployedServices, sequenceFinishedEventArgs.DeployedDifference);
		}
		LemonbeatPersistence.SaveConfiguration(sequenceFinishedEventArgs.DeviceId, configurationInformation.Current, suppressEvent: false);
		deviceInformation.DeviceConfigurationState = DeviceConfigurationState.Complete;
	}

	private void OnStaticConfigurationReceived(object sender, StaticConfigurationReceivedEventArgs e)
	{
		DeviceInformation deviceInformation = deviceList[e.DeviceId];
		if (deviceInformation != null && deviceInformation.DeviceInclusionState == LemonbeatDeviceInclusionState.Included)
		{
			deviceInformation.ValueDescriptions = e.ValueDescriptions;
			deviceInformation.MemoryInformation = e.MemoryInformation;
			deviceInformation.ServiceDescriptions = e.ServiceDescriptions;
			deviceInformation.TimezoneOffset = Convert.ToInt32(TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).TotalHours * 60.0 * 60.0);
			configurationAccess.UpdateDeviceTimeZone(deviceInformation);
			LemonbeatPersistence.SaveInTransaction(deviceInformation, suppressEvent: false);
			SynchronizeConfig(deviceInformation);
		}
		else
		{
			Log.Warning(Module.LemonbeatProtocolAdapter, "ConfigurationController.OnStaticConfigurationReceived called for a device which is not included");
		}
	}

	private bool IsStaticConfigAvailable(DeviceInformation device)
	{
		if (device.ServiceDescriptions != null && (!device.ServiceDescriptions.Any((ServiceDescription sd) => sd.ServiceType == ServiceType.MemoryDescription) || device.MemoryInformation != null))
		{
			if (device.ServiceDescriptions.Any((ServiceDescription sd) => sd.ServiceType == ServiceType.ValueDescription))
			{
				return device.ValueDescriptions != null;
			}
			return true;
		}
		return false;
	}

	private void LoadCurrentConfigurationFromPersistence()
	{
		IEnumerable<KeyValuePair<Guid, PhysicalConfiguration>> enumerable = LemonbeatPersistence.LoadAllConfigurations();
		lock (syncRoot)
		{
			foreach (KeyValuePair<Guid, PhysicalConfiguration> item in enumerable)
			{
				configurations.Add(item.Key, new ConfigurationInformation
				{
					Current = item.Value
				});
			}
		}
	}

	private void ExcludeRemovedDevices(IEnumerable<Guid> devicesToRemove)
	{
		lock (syncRoot)
		{
			Guid[] array = (from configuration in configurations
				where devicesToRemove.Contains(configuration.Key)
				select configuration.Key).ToArray();
			Guid[] array2 = array;
			foreach (Guid deviceId in array2)
			{
				RemoveConfiguration(deviceId);
			}
		}
		List<DeviceInformation> list = deviceList.SyncWhere((DeviceInformation device) => device.DeviceInclusionState != LemonbeatDeviceInclusionState.Found && device.DeviceInclusionState != LemonbeatDeviceInclusionState.PublicKeyReceived && devicesToRemove.Contains(device.DeviceId));
		foreach (DeviceInformation item in list)
		{
			deviceInclusionController.ExcludeAsync(item);
		}
	}

	private void RemoveConfiguration(Guid deviceId)
	{
		configurations.Remove(deviceId);
		LemonbeatPersistence.DeleteConfiguration(deviceId, suppressEvent: false);
	}
}
