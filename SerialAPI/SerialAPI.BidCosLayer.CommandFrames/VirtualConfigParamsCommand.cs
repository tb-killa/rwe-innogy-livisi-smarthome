using System;

namespace SerialAPI.BidCosLayer.CommandFrames;

public class VirtualConfigParamsCommand : BaseVirtualCommand
{
	public byte[] Parameters { get; set; }

	public VirtualConfigParamsCommand(byte channel, byte[] destinationAddress)
		: base(channel, destinationAddress, VirtualCommandType.ConfigParameter)
	{
	}

	public VirtualConfigParamsCommand(byte[] message)
		: base(message)
	{
		Parameters = new byte[message.Length - GetHeaderLength()];
		Array.Copy(message, GetHeaderLength(), Parameters, 0, Parameters.Length);
	}

	public override byte[] ToArray()
	{
		Parameters = Parameters ?? new byte[0];
		byte[] array = base.ToArray();
		int num = array.Length + Parameters.Length;
		byte[] array2 = new byte[num];
		Array.Copy(array, array2, array.Length);
		Array.Copy(Parameters, 0, array2, array.Length, Parameters.Length);
		return array2;
	}
}
