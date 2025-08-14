using System.Collections.Generic;
using System.Threading;

namespace SerialAPI.BidCosLayer.DevicesSupport.Wsd2;

internal class AdapterFrameHandlerConditionalSwitch : AdapterFrameHandler<Wsd2Adapter>
{
	public AdapterFrameHandlerConditionalSwitch(Wsd2Adapter deviceAdapter)
		: base(deviceAdapter)
	{
	}

	public override bool CanHandle(BidCosMessageForSend message)
	{
		return message.header.FrameType == SIPcosFrameType.CONDITIONAL_SWITCH_COMMAND;
	}

	public override SendStatus Handle(BidCosMessageForSend message)
	{
		if (base.DeviceAdapter.GroupIp == null)
		{
			return SendStatus.MULTI_CAST;
		}
		byte[] message2 = TranslateWsd1ToWsd2CondSwitchMessage(message.message);
		byte[] counter = new byte[3]
		{
			base.DeviceAdapter.SequenceCounterWsd2HighByte,
			base.DeviceAdapter.SequenceCounterWsd2MidByte,
			base.DeviceAdapter.SequenceCounterWsd2LowByte
		};
		List<byte> data = BuildWsd2ConditionalSwitchMessage(base.DeviceAdapter.GroupIp, base.DeviceAdapter.bidCosHandler.DefaultIP, message2, counter, base.DeviceAdapter.Wsd2LocalKey);
		for (int i = 0; i < 6; i++)
		{
			base.DeviceAdapter.bidCosHandler.BroadcastFrameToAir(data, SendMode.Burst);
			Thread.Sleep(200);
		}
		base.DeviceAdapter.IncrementSequenceCounter();
		return SendStatus.MULTI_CAST;
	}

	private byte[] TranslateWsd1ToWsd2CondSwitchMessage(byte[] message)
	{
		byte[] array = new byte[message.Length];
		message.CopyTo(array, 0);
		array[1] = 1;
		if (array[2] == 200)
		{
			array[2] = 198;
		}
		if (array[2] == 1)
		{
			array[2] = 0;
		}
		array[1] = base.DeviceAdapter.SequenceCounterWsd2LowByte;
		return array;
	}

	private static List<byte> BuildWsd2ConditionalSwitchMessage(byte[] source, byte[] destination, byte[] message, byte[] counter, byte[] key)
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
