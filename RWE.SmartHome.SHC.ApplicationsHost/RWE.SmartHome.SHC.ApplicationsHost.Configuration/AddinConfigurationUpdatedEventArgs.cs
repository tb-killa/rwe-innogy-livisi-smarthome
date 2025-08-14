using System;

namespace RWE.SmartHome.SHC.ApplicationsHost.Configuration;

public class AddinConfigurationUpdatedEventArgs : EventArgs
{
	public AddinsConfigurationCache AddinsCache { get; set; }

	public string[] AffectedAppIds { get; set; }
}
