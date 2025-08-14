namespace RWE.SmartHome.SHC.DeviceCommunication.SipcosCommandHandlerExtensions;

public class SwitchCommand
{
	public bool ActivationTime { get; set; }

	public byte KeyChannelNumber { get; set; }

	public byte KeyStrokeCounter { get; set; }

	public SwitchCommand(byte[] buffer)
	{
		ActivationTime = (buffer[0] & 0x40) == 64;
		KeyChannelNumber = (byte)(buffer[0] & 0x3F);
		KeyStrokeCounter = buffer[1];
	}

	public SwitchCommand()
	{
	}

	public virtual byte[] ToArray()
	{
		byte[] array = new byte[2];
		byte b = KeyChannelNumber;
		if (ActivationTime)
		{
			b |= 0x40;
		}
		array[0] = b;
		array[1] = KeyStrokeCounter;
		return array;
	}
}
