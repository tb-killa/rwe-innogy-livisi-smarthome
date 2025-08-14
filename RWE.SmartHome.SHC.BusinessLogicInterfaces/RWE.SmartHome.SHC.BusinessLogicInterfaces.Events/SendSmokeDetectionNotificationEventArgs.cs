using System;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;

public class SendSmokeDetectionNotificationEventArgs
{
	public DateTime Date { get; set; }

	public Guid DeviceId { get; set; }

	public bool Occurred { get; set; }
}
