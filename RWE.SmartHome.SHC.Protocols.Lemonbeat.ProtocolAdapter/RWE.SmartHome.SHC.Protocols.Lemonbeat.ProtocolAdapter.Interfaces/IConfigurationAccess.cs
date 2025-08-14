using System;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Configuration;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Events;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Interfaces;

internal interface IConfigurationAccess
{
	event EventHandler<StaticConfigurationReceivedEventArgs> GetStaticConfigurationCompleted;

	event EventHandler<CurrentConfigurationReceivedEventArgs> GetDynamicConfigurationCompleted;

	event EventHandler<DeployConfigurationChangesCompletedEventArgs> DeployConfigurationChangesCompleted;

	void GetDynamicConfigurationAsync(DeviceInformation device);

	void DeployConfigurationAsync(DeviceInformation device, ConfigDifferenceProvider configDiffProvider);

	void GetStaticConfigurationAsync(DeviceInformation device);

	void UpdateDeviceTimeZone(DeviceInformation device);
}
