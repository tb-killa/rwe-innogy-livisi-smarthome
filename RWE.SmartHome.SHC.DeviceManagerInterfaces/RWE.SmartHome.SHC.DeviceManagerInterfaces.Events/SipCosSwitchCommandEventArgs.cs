using System;

namespace RWE.SmartHome.SHC.DeviceManagerInterfaces.Events;

public class SipCosSwitchCommandEventArgs : EventArgs
{
	public byte[] DeviceAddress { get; private set; }

	public byte KeyChannelNumber { get; private set; }

	public byte KeyStrokeCounter { get; private set; }

	public bool IsLongPress { get; private set; }

	public byte? DecisionValue { get; private set; }

	public SipCosSwitchCommandEventArgs(byte[] deviceAddress, byte keyChannelNumber, byte keyStrokeCounter, bool isLongPress, byte? decisionValue)
	{
		DeviceAddress = deviceAddress;
		KeyChannelNumber = keyChannelNumber;
		KeyStrokeCounter = keyStrokeCounter;
		IsLongPress = isLongPress;
		DecisionValue = decisionValue;
	}
}
