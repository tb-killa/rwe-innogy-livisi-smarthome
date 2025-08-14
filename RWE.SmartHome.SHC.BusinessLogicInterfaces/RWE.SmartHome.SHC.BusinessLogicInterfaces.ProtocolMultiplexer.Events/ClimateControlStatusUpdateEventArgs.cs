using System;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;

public class ClimateControlStatusUpdateEventArgs : EventArgs
{
	public bool Occurred { get; set; }

	public StatusEventType StatusType { get; set; }

	public Guid DeviceId { get; set; }

	public ClimateControlStatusUpdateEventArgs(Guid deviceId, StatusEventType statusType, bool occurred)
	{
		Occurred = occurred;
		DeviceId = deviceId;
		StatusType = statusType;
	}
}
