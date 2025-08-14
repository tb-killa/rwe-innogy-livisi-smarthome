using System;

namespace SerialAPI.BidCosLayer.CommandFrames;

public abstract class BaseVirtualCommand
{
	public byte Channel { get; set; }

	public byte[] DestinationAddress { get; set; }

	public VirtualCommandType Type { get; set; }

	public BaseVirtualCommand(byte channel, byte[] destinationAddress, VirtualCommandType commandType)
	{
		Channel = channel;
		DestinationAddress = destinationAddress;
		Type = commandType;
	}

	public BaseVirtualCommand(byte[] message)
	{
		ParseMessage(message);
	}

	public virtual byte[] ToArray()
	{
		if (DestinationAddress == null || DestinationAddress.Length != GetDestinationAddrLength())
		{
			throw new Exception("Invlaid destination address");
		}
		byte[] array = new byte[GetHeaderLength()];
		array[0] = (byte)Type;
		array[1] = Channel;
		Array.Copy(DestinationAddress, 0, array, 2, DestinationAddress.Length);
		return array;
	}

	protected int GetDestinationAddrLength()
	{
		return 3;
	}

	protected int GetHeaderLength()
	{
		return 2 + GetDestinationAddrLength();
	}

	private void ParseMessage(byte[] message)
	{
		if (message == null || message.Length < GetHeaderLength())
		{
			throw new ArgumentException("Invalid message");
		}
		DestinationAddress = new byte[GetDestinationAddrLength()];
		Type = (VirtualCommandType)message[0];
		Channel = message[1];
		Array.Copy(message, 2, DestinationAddress, 0, DestinationAddress.Length);
	}
}
