using System;
using System.Collections.Generic;
using System.Linq;
using CommonFunctionality.Encryption;

namespace SerialAPI.BidCosLayer.DevicesSupport.Wsd2;

internal class Wsd2Comm : DeviceComm
{
	public Wsd2Comm(BidCosHandlerRef bidcosHandler)
		: base(bidcosHandler)
	{
	}

	public bool ConfigureNodeKey(byte[] destination, byte[] currentKey, byte[] targetKey)
	{
		bool flag = false;
		byte[] data = BuildWsd2KeyChangePayload(1, 0, targetKey, currentKey);
		flag = SendBidcos(destination, 4, data, SendMode.Normal);
		if (!flag)
		{
			return flag;
		}
		byte[] data2 = BuildWsd2KeyChangePayload(1, 1, targetKey, currentKey);
		return SendBidcos(destination, 4, data2, SendMode.Normal);
	}

	public static byte[] BuildWsd2KeyChangePayload(byte keyIndex, byte keyPart, byte[] newKey, byte[] oldKey)
	{
		byte[] array = new byte[16]
		{
			1,
			(byte)((keyIndex << 1) | (keyPart & 1)),
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0
		};
		newKey.Skip((keyPart != 0) ? 8 : 0).Take(8).ToArray()
			.CopyTo(array, 2);
		byte[] array2 = new byte[2];
		new Random().NextBytes(array2);
		array2.CopyTo(array, 10);
		new byte[4] { 126, 41, 111, 165 }.CopyTo(array, 12);
		Aes aes = new Aes(KeySize.Bits128, oldKey);
		byte[] output = new byte[16];
		aes.Cipher(array, out output);
		return output;
	}

	public List<byte> BuildWsd2ConditionalSwitchMessage(byte[] source, byte[] destination, byte[] message, byte[] counter, byte[] key)
	{
		byte header = 20;
		byte command = 65;
		AesConditionalSwitch aesConditionalSwitch = new AesConditionalSwitch(key);
		byte[] collection = aesConditionalSwitch.CalculateAesSignature(source, destination, counter, header, message);
		List<byte> list = new List<byte>();
		list.AddRange(message);
		list.Add(counter[0]);
		list.Add(counter[1]);
		list.AddRange(collection);
		return BuildBidCosMessageWsd2Static(header, source, destination, command, list.ToArray(), SendMode.Normal, counter[2]);
	}

	private static List<byte> BuildBidCosMessageWsd2Static(byte header, byte[] sender, byte[] address, byte Command, byte[] data, SendMode mode, byte specialCounter)
	{
		List<byte> list = new List<byte>();
		byte item = specialCounter;
		list.Add(item);
		if (mode != SendMode.Normal)
		{
			header |= 0x10;
		}
		list.Add(header);
		list.Add(Command);
		list.AddRange(sender);
		list.AddRange(address);
		list.AddRange(data);
		return list;
	}
}
