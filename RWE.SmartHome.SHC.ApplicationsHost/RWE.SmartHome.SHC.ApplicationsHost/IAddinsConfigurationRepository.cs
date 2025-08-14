using System;
using RWE.SmartHome.SHC.ApplicationsHost.Configuration;

namespace RWE.SmartHome.SHC.ApplicationsHost;

public interface IAddinsConfigurationRepository
{
	event EventHandler<AddinConfigurationUpdatedEventArgs> ConfigurationChanged;

	AddinConfiguration GetConfiguration(string appid);

	AddinConfiguration GetValidationConfiguration(string appId);
}
