using System;

namespace RWE.SmartHome.SHC.DeviceManagerInterfaces.Events;

public class SequenceFinishedEventArgs : EventArgs
{
	public Guid CorrelationId { get; private set; }

	public SequenceState State { get; private set; }

	public SequenceFinishedEventArgs(Guid correlationId, SequenceState state)
	{
		CorrelationId = correlationId;
		State = state;
	}
}
