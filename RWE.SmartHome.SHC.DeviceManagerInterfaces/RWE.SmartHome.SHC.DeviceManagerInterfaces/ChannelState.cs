namespace RWE.SmartHome.SHC.DeviceManagerInterfaces;

public class ChannelState
{
	public byte KeyChannelNumber { get; set; }

	public int Value { get; set; }

	public byte ChannelError { get; set; }

	public DeviceCondition Condition { get; set; }

	public int KeystrokeCounter { get; set; }

	public bool IsLongPress { get; set; }
}
