using System;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Enums;

namespace RWE.SmartHome.SHC.DeviceManagerInterfaces.Events;

public class DeviceCalibrationStateChangedEventArgs
{
	public Guid DeviceId { get; set; }

	public CalibrationState CalibrationState { get; set; }

	public int? Value { get; set; }

	public DeviceCalibrationStateChangedEventArgs(Guid deviceId, CalibrationState calibrationState, int? value)
	{
		DeviceId = deviceId;
		CalibrationState = calibrationState;
		Value = value;
	}
}
