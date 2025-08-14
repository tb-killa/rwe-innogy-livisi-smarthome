using System;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;

public class UpdateFailedEventArgs : EventArgs
{
	public Guid DeviceId { get; set; }

	public FailedUpdateStep UpdateStep { get; set; }
}
