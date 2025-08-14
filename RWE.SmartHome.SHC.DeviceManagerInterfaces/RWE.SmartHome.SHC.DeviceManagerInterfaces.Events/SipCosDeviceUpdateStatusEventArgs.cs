using System;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.DeviceHandler;

namespace RWE.SmartHome.SHC.DeviceManagerInterfaces.Events;

public class SipCosDeviceUpdateStatusEventArgs : EventArgs
{
	public Guid DeviceId { get; private set; }

	public CosIPFirmwareUpdateStatusCode StatusCode { get; private set; }

	public int NextFrame { get; private set; }

	public SipCosDeviceUpdateStatusEventArgs(Guid deviceId, CosIPFirmwareUpdateStatusCode statusCode, int nextFrame)
	{
		DeviceId = deviceId;
		StatusCode = statusCode;
		NextFrame = nextFrame;
	}
}
