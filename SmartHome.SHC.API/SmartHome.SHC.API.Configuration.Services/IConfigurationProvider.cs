using System;
using System.Collections.Generic;

namespace SmartHome.SHC.API.Configuration.Services;

public interface IConfigurationProvider
{
	event EventHandler<ConfigurationChangedEventArgs> ConfigurationChanged;

	Capability GetCapability(Guid capabilityId);

	IEnumerable<Capability> GetCapabilities();

	IEnumerable<Capability> GetDeviceCapabilities(Guid deviceId);

	Device GetDevice(Guid deviceId);

	Device GetShcDevice();

	IEnumerable<Device> GetDevices();

	IEnumerable<Trigger> GetTriggers();

	IEnumerable<CustomTrigger> GetCustomTriggers();

	ConfigurationUpdateResponse UpdateConfiguration(IEnumerable<Device> devicesToSet, IEnumerable<Capability> capabilitiesToSet, IEnumerable<Guid> deviceIdsToDelete, IEnumerable<Guid> capabilitiesIdsToDelete);
}
