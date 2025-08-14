using System;
using System.Diagnostics;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Enums;

namespace RWE.SmartHome.SHC.DeviceManager.Calibration;

internal sealed class CalibrationTarget : ICalibrationTarget
{
	public Guid Id { get; private set; }

	public CalibrationState State { get; set; }

	public DateTime FirstTime { get; private set; }

	public DateTime LastTime { get; private set; }

	public Stopwatch MulticastStopWatch { get; private set; }

	internal CalibrationTarget(Guid id)
	{
		State = CalibrationState.None;
		Id = id;
		FirstTime = DateTime.MinValue;
		LastTime = DateTime.MinValue;
		MulticastStopWatch = new Stopwatch();
	}

	public void Store(DateTime time)
	{
		if (DateTime.MinValue == FirstTime)
		{
			FirstTime = time;
		}
		LastTime = time;
	}

	public int CalculateTotalTime()
	{
		return Convert.ToInt32(LastTime.Subtract(FirstTime).TotalMilliseconds * 0.01);
	}

	public void Reset()
	{
		if (State != CalibrationState.None)
		{
			State = CalibrationState.Initialized;
		}
		ResetSequenceVariables();
	}

	internal void ResetSequenceVariables()
	{
		FirstTime = DateTime.MinValue;
		LastTime = DateTime.MinValue;
	}
}
