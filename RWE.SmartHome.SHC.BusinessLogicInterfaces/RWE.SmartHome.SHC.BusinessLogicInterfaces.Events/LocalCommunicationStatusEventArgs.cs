using System;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;

public class LocalCommunicationStatusEventArgs
{
	public Guid DeviceId { get; set; }

	public bool Status { get; set; }

	public LocalCommunicationStatusEventArgs(Guid deviceId, bool status)
	{
		DeviceId = deviceId;
		Status = status;
	}
}
