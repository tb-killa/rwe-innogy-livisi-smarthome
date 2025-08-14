using System;

namespace SerialAPI.BidCosLayer.CommandFrames;

public class VirtualTestSoundCommand : BaseVirtualCommand
{
	public byte SoundId { get; set; }

	public byte CurrentSoundId { get; set; }

	public int DelayMs { get; set; }

	public VirtualTestSoundCommand(byte channel, byte[] destinationAddress)
		: base(channel, destinationAddress, VirtualCommandType.TestSound)
	{
	}

	public VirtualTestSoundCommand(byte[] message)
		: base(message)
	{
		int headerLength = GetHeaderLength();
		if (message == null || message.Length < headerLength + 2 + 4)
		{
			throw new ArgumentException("Invalid message in VirtualTestSoundCommand");
		}
		SoundId = message[headerLength];
		CurrentSoundId = message[headerLength + 1];
		DelayMs = BitConverter.ToInt32(message, headerLength + 2);
	}

	public override byte[] ToArray()
	{
		byte[] array = base.ToArray();
		byte[] bytes = BitConverter.GetBytes(DelayMs);
		int num = array.Length + 2 + bytes.Length;
		byte[] array2 = new byte[num];
		Array.Copy(array, array2, array.Length);
		array2[array.Length] = SoundId;
		array2[array.Length + 1] = CurrentSoundId;
		Array.Copy(bytes, 0, array2, array.Length + 2, bytes.Length);
		return array2;
	}
}
