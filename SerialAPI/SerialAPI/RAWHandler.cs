using System;
using System.Collections.Generic;

namespace SerialAPI;

public class RAWHandler : SerialHandler
{
	public delegate void ReceiveRawData(byte[] Message);

	public event ReceiveRawData ReceiveData;

	public RAWHandler(Core core)
		: base(SerialHandlerType.RAW_HANDLER, core)
	{
	}

	protected override void HandleData(List<byte> data, DateTime receiveTime)
	{
		if (this.ReceiveData != null)
		{
			byte[] array = new byte[data.Count];
			data.CopyTo(array);
			this.ReceiveData(array);
		}
	}

	public SendStatus SendMessage(byte[] message)
	{
		List<byte> list = new List<byte>();
		list.AddRange(message);
		return BroadcastFrameToAir(list, SendMode.Normal);
	}

	public SendStatus SendMessageDefaultSync(byte[] message)
	{
		List<byte> list = new List<byte>();
		list.AddRange(message);
		return writeDefaultSync(list, SendMode.Normal);
	}

	public void EnableRead()
	{
		List<byte> data = new List<byte>();
		register(data);
	}
}
