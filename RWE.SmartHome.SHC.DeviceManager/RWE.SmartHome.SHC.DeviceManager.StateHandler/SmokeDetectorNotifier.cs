using System;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.DeviceManager.StateHandler;

internal class SmokeDetectorNotifier
{
	private bool smokeNotificationSet;

	private readonly IEventManager eventManager;

	public SmokeDetectorNotifier(IEventManager eventManager)
	{
		this.eventManager = eventManager;
	}

	public void SendStatusUpdate(Guid deviceId, SmokeDetectedState smokeState)
	{
		Log.Information(Module.ExternalCommandDispatcher, string.Format("Smoke detector event occured: {0}, base deivce: {1}", (smokeState == SmokeDetectedState.SmokeDetected) ? "ON" : "OFF", deviceId));
		bool flag = smokeState == SmokeDetectedState.SmokeDetected;
		if (flag != smokeNotificationSet)
		{
			eventManager.GetEvent<SendSmokeDetectionNotificationEvent>().Publish(new SendSmokeDetectionNotificationEventArgs
			{
				Date = ShcDateTime.Now,
				DeviceId = deviceId,
				Occurred = flag
			});
			smokeNotificationSet = flag;
		}
	}
}
