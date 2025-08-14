using System;
using System.Linq;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.BidCos.Tasks;

public abstract class BaseBidCosTask
{
	public byte Channel { get; set; }

	public byte[] SourceAddress { get; set; }

	public byte[] DestinationAddress { get; set; }

	public abstract BidCosTaskType Type { get; }

	public BaseBidCosTask(byte channel, byte[] sourceAddress, byte[] destinationAddress)
	{
		Channel = channel;
		SourceAddress = sourceAddress;
		DestinationAddress = destinationAddress;
	}

	public BaseBidCosTask(byte[] data)
	{
		PopulateFromBytes(data);
	}

	public abstract BaseBidCosTask GetDiffTask(BaseBidCosTask previousTask);

	public override int GetHashCode()
	{
		int num = 0;
		num = (num * 397) ^ Channel;
		num = (num * 397) ^ SourceAddress.GetHashCode();
		num = (num * 397) ^ DestinationAddress.GetHashCode();
		return (num * 397) ^ (int)Type;
	}

	public override bool Equals(object obj)
	{
		BaseBidCosTask baseBidCosTask = obj as BaseBidCosTask;
		if (baseBidCosTask == this)
		{
			return true;
		}
		return true && baseBidCosTask != null && baseBidCosTask.Channel == Channel && baseBidCosTask.DestinationAddress != null && DestinationAddress != null && baseBidCosTask.DestinationAddress.SequenceEqual(DestinationAddress) && baseBidCosTask.SourceAddress != null && SourceAddress != null && baseBidCosTask.SourceAddress.SequenceEqual(SourceAddress) && baseBidCosTask.Type == Type;
	}

	public virtual byte[] GetBytes()
	{
		int num = 2 + SourceAddress.Length + DestinationAddress.Length;
		byte[] array = new byte[num];
		array[0] = (byte)Type;
		array[1] = Channel;
		Array.Copy(SourceAddress, 0, array, 2, SourceAddress.Length);
		Array.Copy(DestinationAddress, 0, array, 2 + SourceAddress.Length, DestinationAddress.Length);
		return array;
	}

	protected virtual int PopulateFromBytes(byte[] data)
	{
		SourceAddress = new byte[3];
		DestinationAddress = new byte[3];
		_ = data[0];
		Channel = data[1];
		Array.Copy(data, 2, SourceAddress, 0, SourceAddress.Length);
		Array.Copy(data, 2 + SourceAddress.Length, DestinationAddress, 0, DestinationAddress.Length);
		return 2 + SourceAddress.Length + DestinationAddress.Length;
	}
}
