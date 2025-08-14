using System;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;

public class SoftwareUpdateAvailableEventArgs
{
	public DateTime UpdateDeadline { get; set; }

	public SoftwareUpdateType UpdateType { get; set; }

	public string Version { get; private set; }

	public SoftwareUpdateAvailableEventArgs(SoftwareUpdateType updateType, DateTime updateDeadline, string version)
	{
		UpdateType = updateType;
		UpdateDeadline = updateDeadline;
		Version = version;
	}
}
