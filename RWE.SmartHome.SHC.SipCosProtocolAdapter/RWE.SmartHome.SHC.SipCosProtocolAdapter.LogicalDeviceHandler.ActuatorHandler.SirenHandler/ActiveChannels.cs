namespace RWE.SmartHome.SHC.SipCosProtocolAdapter.LogicalDeviceHandler.ActuatorHandler.SirenHandler;

public class ActiveChannels
{
	private readonly bool[] channelStates;

	public ActiveChannels(int numChannels)
	{
		channelStates = new bool[numChannels];
		DeactivateChannels();
	}

	public void ChannelStateChange(byte channel, bool state)
	{
		channelStates[channel - 1] = state;
	}

	public byte? GetActiveChannel()
	{
		for (int num = (byte)(channelStates.Length - 1); num >= 0; num--)
		{
			if (channelStates[num])
			{
				return (byte)(num + 1);
			}
		}
		return null;
	}

	public byte[] GetAllChannelsWithActivePriority()
	{
		int num = channelStates.Length;
		byte[] array = new byte[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = (byte)(num - i);
		}
		byte b = 0;
		for (int j = 0; j < num; j++)
		{
			if (channelStates[array[j] - 1])
			{
				if (b != j)
				{
					byte b2 = array[b];
					array[b] = array[j];
					array[j] = b2;
				}
				b++;
			}
		}
		return array;
	}

	public void DeactivateChannels()
	{
		for (int i = 0; i < channelStates.Length; i++)
		{
			channelStates[i] = false;
		}
	}
}
