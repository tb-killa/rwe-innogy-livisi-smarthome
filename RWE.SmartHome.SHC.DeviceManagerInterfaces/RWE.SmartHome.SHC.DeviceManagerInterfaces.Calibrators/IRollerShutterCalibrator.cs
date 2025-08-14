using System;
using System.Collections.Generic;

namespace RWE.SmartHome.SHC.DeviceManagerInterfaces.Calibrators;

public interface IRollerShutterCalibrator
{
	void SetCalibrationTargets(IEnumerable<Guid> deviceIds);

	void ResetCalibration(Guid deviceId);
}
