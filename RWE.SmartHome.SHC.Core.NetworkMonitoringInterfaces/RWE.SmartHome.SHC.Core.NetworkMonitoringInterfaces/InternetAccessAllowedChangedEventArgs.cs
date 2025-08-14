using System;

namespace RWE.SmartHome.SHC.Core.NetworkMonitoringInterfaces;

public class InternetAccessAllowedChangedEventArgs : EventArgs
{
	public bool InternetAccessAllowed { get; set; }
}
