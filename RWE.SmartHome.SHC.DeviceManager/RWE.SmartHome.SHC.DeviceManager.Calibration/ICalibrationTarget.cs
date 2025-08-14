using System;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Enums;

namespace RWE.SmartHome.SHC.DeviceManager.Calibration;

internal interface ICalibrationTarget
{
	Guid Id { get; }

	CalibrationState State { get; set; }

	DateTime FirstTime { get; }

	DateTime LastTime { get; }

	void Store(DateTime time);

	int CalculateTotalTime();

	void Reset();
}
