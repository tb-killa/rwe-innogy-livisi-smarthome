namespace RWE.SmartHome.SHC.DeviceCommunication.SipcosCommandHandlerExtensions;

public class ActionDataRampStart
{
	public byte SwitchLevel { get; set; }

	public ushort RampTime { get; set; }

	public ushort? OnTime { get; set; }

	public ActionDataRampStart(byte[] buffer)
	{
		SwitchLevel = buffer[0];
		RampTime = Converter.ReadUShort(buffer, 1);
		if (buffer.Length > 3)
		{
			OnTime = Converter.ReadUShort(buffer, 3);
		}
	}

	public ActionDataRampStart()
	{
	}

	public byte[] ToArray()
	{
		bool flag = false;
		int num = 3;
		if (OnTime.HasValue)
		{
			ushort? onTime = OnTime;
			if (onTime.GetValueOrDefault() != 0 || !onTime.HasValue)
			{
				flag = true;
				num = 5;
			}
		}
		byte[] array = new byte[num];
		array[0] = SwitchLevel;
		byte[] bytes = Converter.GetBytes(RampTime);
		array[1] = bytes[0];
		array[2] = bytes[1];
		if (flag)
		{
			bytes = Converter.GetBytes(OnTime.Value);
			array[3] = bytes[0];
			array[4] = bytes[1];
		}
		return array;
	}
}
