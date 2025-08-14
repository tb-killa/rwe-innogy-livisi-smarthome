using System;
using System.Collections.Generic;
using System.Linq;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.BidCos.Tasks;

public class ConfigBidCosTask : BaseBidCosTask
{
	public override BidCosTaskType Type => BidCosTaskType.Configuration;

	public IDictionary<byte, byte> Params { get; set; }

	public ConfigBidCosTask(byte channel, byte[] sourceAddress, byte[] destinationAddress)
		: base(channel, sourceAddress, destinationAddress)
	{
		Params = new Dictionary<byte, byte>();
	}

	public ConfigBidCosTask(byte[] data)
		: base(data)
	{
	}

	public override int GetHashCode()
	{
		int hashCode = base.GetHashCode();
		return (hashCode * 397) ^ Params.GetHashCode();
	}

	public override bool Equals(object obj)
	{
		ConfigBidCosTask configBidCosTask = obj as ConfigBidCosTask;
		if (base.Equals((object)configBidCosTask) && Params != null && Params != null)
		{
			return Params.SequenceEqual(configBidCosTask.Params);
		}
		return false;
	}

	public override BaseBidCosTask GetDiffTask(BaseBidCosTask previousTask)
	{
		ConfigBidCosTask configBidCosTask = previousTask as ConfigBidCosTask;
		ConfigBidCosTask configBidCosTask2 = new ConfigBidCosTask(base.Channel, base.SourceAddress, base.DestinationAddress);
		Dictionary<byte, byte> dictionary = new Dictionary<byte, byte>();
		foreach (KeyValuePair<byte, byte> item in Params)
		{
			if (configBidCosTask == null || !configBidCosTask.Params.ContainsKey(item.Key) || configBidCosTask.Params[item.Key] != item.Value)
			{
				dictionary[item.Key] = item.Value;
			}
		}
		configBidCosTask2.Params = dictionary;
		return configBidCosTask2;
	}

	public override byte[] GetBytes()
	{
		byte[] bytes = base.GetBytes();
		int num = bytes.Length;
		int num2 = Params.Count * 2 + num;
		byte[] array = new byte[num2];
		Array.Copy(bytes, array, num);
		int num3 = num;
		foreach (KeyValuePair<byte, byte> item in Params)
		{
			array[num3++] = item.Key;
			array[num3++] = item.Value;
		}
		return array;
	}

	public byte[] GetParams()
	{
		int num = Params.Count * 2;
		byte[] array = new byte[num];
		int num2 = 0;
		foreach (KeyValuePair<byte, byte> item in Params)
		{
			array[num2++] = item.Key;
			array[num2++] = item.Value;
		}
		return array;
	}

	protected override int PopulateFromBytes(byte[] data)
	{
		Params = new Dictionary<byte, byte>();
		int num = base.PopulateFromBytes(data);
		for (int i = num; i + 1 < data.Length; i += 2)
		{
			Params[data[i]] = data[i + 1];
		}
		return data.Length;
	}
}
