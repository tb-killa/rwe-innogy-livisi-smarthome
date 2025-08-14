using System;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapterInterfaces.Events;

public class SmokeDetectorStateChangeTriggeredEventArgs
{
	public Guid ActuatorId { get; private set; }

	public SmokeDetectorStateChangeTriggeredEventArgs(Guid actuatorId)
	{
		ActuatorId = actuatorId;
	}
}
