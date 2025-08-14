using System.Collections.Generic;

namespace RWE.SmartHome.SHC.CommonFunctionality.Interfaces;

public class EmailSettings
{
	public string ServerAddress { get; set; }

	public int ServerPortNumber { get; set; }

	public string EmailUsername { get; set; }

	public string EmailPassword { get; set; }

	public List<string> Recipients { get; set; }

	public bool NotificationDeviceLowBattery { get; set; }

	public bool NotificationsDeviceUnreachable { get; set; }
}
