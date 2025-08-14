using SipcosCommandHandler;

namespace RWE.SmartHome.SHC.DeviceManagerInterfaces;

public class DeviceStatusInfo
{
	public bool LowBat { get; set; }

	public bool Clock { get; set; }

	public bool DutyCycle { get; set; }

	public byte Rssi { get; set; }

	public bool? Freeze { get; set; }

	public bool? Mold { get; set; }

	public DeviceStatusInfo()
	{
	}

	public DeviceStatusInfo(SIPcosStatusFrame statusFrame)
	{
		LowBat = statusFrame.LowBat;
		Clock = statusFrame.Clock;
		DutyCycle = statusFrame.DutyCycle;
		Rssi = statusFrame.RSSI;
		if (statusFrame.Type == SIPcosStatusType.STATUS_FRAME_CC_SENSOR)
		{
			Freeze = statusFrame.Freeze;
			Mold = statusFrame.Mold;
		}
	}

	public DeviceStatusInfo(SIPcosAnswerFrame answerFrame)
	{
		LowBat = answerFrame.LowBat;
		Clock = answerFrame.Clock;
		DutyCycle = answerFrame.DutyCycle;
		Rssi = answerFrame.RSSI;
	}

	public void Update(DeviceStatusInfo deviceStatusInfo)
	{
		LowBat = deviceStatusInfo.LowBat;
		Clock = deviceStatusInfo.Clock;
		DutyCycle = deviceStatusInfo.DutyCycle;
		Rssi = deviceStatusInfo.Rssi;
		if (deviceStatusInfo.Freeze.HasValue)
		{
			Freeze = deviceStatusInfo.Freeze;
		}
		if (deviceStatusInfo.Mold.HasValue)
		{
			Mold = deviceStatusInfo.Mold;
		}
	}
}
