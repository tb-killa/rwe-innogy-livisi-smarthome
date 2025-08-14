using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ConfigurationTransformation;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.RepositoryOperations;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.VirtualDevices;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.CoreApiConverters;
using SmartHome.SHC.API.Configuration;
using SmartHome.SHC.API.Configuration.Services;

namespace RWE.SmartHome.SHC.ApplicationsHost.Configuration;

internal class ConfigurationProvider : IConfigurationProvider, IConfigurationValidator
{
	private const string LoggingSource = "ConfigurationProvider";

	private readonly string appId;

	private readonly IRepository configurationRepository;

	private readonly IRepositorySync repositorySync;

	private readonly IConfigurationValidation addinConfigurationValidator;

	private readonly AddinsConfigurationValidator addinsConfigurationValidator;

	private readonly IAddinsConfigurationRepository addinsConfigurationRepository;

	private AddinConfiguration configuration;

	public event EventHandler<ConfigurationChangedEventArgs> ConfigurationChanged;

	public ConfigurationProvider(string appId, IRepository configurationRepository, IRepositorySync repositorySync, AddinsConfigurationValidator addinsConfigurationValidator, IConfigurationValidation addinConfigurationValidator, IAddinsConfigurationRepository addinsConfigurationRepository)
	{
		this.appId = appId;
		this.addinsConfigurationRepository = addinsConfigurationRepository;
		configuration = addinsConfigurationRepository.GetConfiguration(this.appId);
		this.configurationRepository = configurationRepository;
		this.repositorySync = repositorySync;
		if (addinConfigurationValidator != null)
		{
			this.addinConfigurationValidator = addinConfigurationValidator;
		}
		this.addinsConfigurationValidator = addinsConfigurationValidator;
		if (addinsConfigurationValidator != null && addinConfigurationValidator != null)
		{
			this.addinsConfigurationValidator.AddAddinValidator(appId, this);
		}
		this.addinsConfigurationRepository.ConfigurationChanged += OnConfigurationChanged;
	}

	public ConfigurationUpdateResponse UpdateConfiguration(IEnumerable<Device> devicesToSet, IEnumerable<Capability> capabilitiesToSet, IEnumerable<Guid> deviceIdsToDelete, IEnumerable<Guid> capabilitiesIdsToDelete)
	{
		Log.InformationFormat(Module.ApplicationsHost, "ConfigurationProvider", true, "ConfigurationUpdate requested by application {0}.", appId);
		ConfigurationUpdateResponse ret = ConfigurationUpdateResponse.ConfigurationFailure;
		AddinConfigurationUpdateTranslator addinConfigurationUpdateTranslator = new AddinConfigurationUpdateTranslator(appId, configurationRepository);
		AddinConfigurationUpdate addinConfigurationUpdate = addinConfigurationUpdateTranslator.Update(devicesToSet, capabilitiesToSet, deviceIdsToDelete, capabilitiesIdsToDelete);
		if (addinConfigurationUpdate.IsValid)
		{
			RepositoryLockContext repositoryLockContext = null;
			ConfigurationProcessingStatusUpdateEvent value = delegate(ConfigurationProcessingStatusEventArgs args)
			{
				if (args.Status == ConfigurationProcessingStatus.Processed)
				{
					Log.InformationFormat(Module.ApplicationsHost, "ConfigurationProvider", true, "ConfigurationUpdate requested by application {0}; configuration processed successfully.", appId);
					ret = ConfigurationUpdateResponse.Success;
				}
				if (args.Status == ConfigurationProcessingStatus.Failed)
				{
					Log.WarningFormat(Module.ApplicationsHost, "ConfigurationProvider", true, "ConfigurationUpdate requested by application {0}; configuration processing failed.", appId);
					ret = ConfigurationUpdateResponse.ConfigurationFailure;
				}
			};
			try
			{
				repositoryLockContext = repositorySync.GetLock("UpdateConfiguration/" + appId, new RepositoryUpdateContextData(appId));
				repositorySync.OnConfigurationStatusUpdate += value;
				addinConfigurationUpdate.BaseDevices.ForEach(configurationRepository.SetBaseDevice);
				addinConfigurationUpdate.LogicalDevices.ForEach(configurationRepository.SetLogicalDevice);
				addinConfigurationUpdate.DeleteBaseDevicesIds.ForEach(configurationRepository.DeleteBaseDevice);
				addinConfigurationUpdate.DeleteLogicalDevicesIds.ForEach(configurationRepository.DeleteLogicalDevice);
				repositoryLockContext.Commit = true;
				Log.InformationFormat(Module.ApplicationsHost, "ConfigurationProvider", true, "ConfigurationUpdate requested by application {0}; processing.", appId);
				repositoryLockContext.Dispose();
				repositoryLockContext = null;
				repositorySync.OnConfigurationStatusUpdate -= value;
			}
			catch (RepositoryLockedException)
			{
				Log.WarningFormat(Module.ApplicationsHost, "ConfigurationProvider", true, "ConfigurationUpdate requested by application {0}; configuration in progress.", appId);
				ret = ConfigurationUpdateResponse.ConfigurationInProgress;
			}
			finally
			{
				repositoryLockContext?.Dispose();
			}
		}
		else
		{
			Log.WarningFormat(Module.ApplicationsHost, "ConfigurationProvider", true, "ConfigurationUpdate requested by application {0}; request not valid.", appId);
		}
		Log.InformationFormat(Module.ApplicationsHost, "ConfigurationProvider", true, "ConfigurationUpdate requested by application {0} finalized with status: {1}.", appId, ret);
		return ret;
	}

	public IEnumerable<Capability> GetCapabilities()
	{
		return configuration.Capabilities;
	}

	public IEnumerable<Device> GetDevices()
	{
		return configuration.Devices;
	}

	public Capability GetCapability(Guid capabilityId)
	{
		return GetCapabilities().FirstOrDefault((Capability c) => c.Id == capabilityId);
	}

	public Device GetDevice(Guid deviceId)
	{
		return GetDevices().FirstOrDefault((Device d) => d.Id == deviceId);
	}

	public Device GetShcDevice()
	{
		Property geolocation = GetGeolocation();
		return configurationRepository.GetOriginalBaseDevices().FirstOrDefault((BaseDevice d) => d.Id == configurationRepository.GetShcBaseDeviceId()).ToApiBaseDevice(new List<Property> { geolocation });
	}

	private Property GetGeolocation()
	{
		HomeSetup homeSetup = configurationRepository.GetHomeSetups().FirstOrDefault();
		string value = string.Empty;
		if (homeSetup != null)
		{
			StringProperty stringProperty = homeSetup.Properties.OfType<StringProperty>().FirstOrDefault((StringProperty pp) => pp.Name == "GeoLocation");
			if (stringProperty != null)
			{
				value = stringProperty.Value;
			}
		}
		return new StringProperty("GeoLocation", value);
	}

	public IEnumerable<Capability> GetDeviceCapabilities(Guid deviceId)
	{
		return from c in GetCapabilities()
			where c.DeviceId == deviceId
			select c;
	}

	public IEnumerable<Trigger> GetTriggers()
	{
		return configuration.Triggers;
	}

	public IEnumerable<CustomTrigger> GetCustomTriggers()
	{
		return configuration.CustomTriggers;
	}

	public void OnConfigurationChanged(object sender, AddinConfigurationUpdatedEventArgs args)
	{
		configuration = args.AddinsCache.GetAddinConfiguration(appId);
		if (this.ConfigurationChanged != null && args.AffectedAppIds.Contains(appId))
		{
			try
			{
				this.ConfigurationChanged(this, new ConfigurationChangedEventArgs());
			}
			catch (Exception ex)
			{
				Log.Exception(Module.ApplicationsHost, "ConfigurationProvider", ex, "OnConfigurationChanged error occured in {0}.", appId);
			}
		}
	}

	public IEnumerable<ErrorEntry> GetConfigurationErrors(IRepository configuration, RepositoryUpdateContextData updateContextData)
	{
		List<ErrorEntry> list = new List<ErrorEntry>();
		if (addinConfigurationValidator == null || updateContextData.AppId == appId)
		{
			return list;
		}
		AddinConfiguration validationConfiguration = addinsConfigurationRepository.GetValidationConfiguration(appId);
		IEnumerable<ConfigurationError> source = addinConfigurationValidator.OnConfigurationChanging(validationConfiguration.Capabilities, validationConfiguration.Devices, validationConfiguration.ActionDescriptions, validationConfiguration.Triggers);
		list.AddRange(source.Select((ConfigurationError ce) => ce.ToCoreErrorEntry(appId)));
		return list;
	}

	internal void Uninitialize()
	{
		if (addinsConfigurationValidator != null)
		{
			addinsConfigurationValidator.RemoveAddinValidator(appId);
		}
		if (addinsConfigurationRepository != null)
		{
			addinsConfigurationRepository.ConfigurationChanged -= OnConfigurationChanged;
		}
	}
}
