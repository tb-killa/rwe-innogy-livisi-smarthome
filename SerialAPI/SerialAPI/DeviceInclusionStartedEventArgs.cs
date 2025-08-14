using System;

namespace SerialAPI;

public class DeviceInclusionStartedEventArgs : EventArgs
{
	public byte[] DeviceAddress { get; private set; }

	public DeviceInclusionStartedEventArgs(byte[] address)
	{
		DeviceAddress = address;
	}
}
