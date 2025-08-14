using System;
using WebSocketLibrary.Frames;
using WebSocketLibrary.Helpers;
using WebSocketLibrary.Socket;

namespace WebSocketLibrary.Handlers.Frames;

public class FrameHandler : IFrameHandler
{
	private readonly IReceiverSocket receiver;

	private readonly ISenderSocket sender;

	private readonly FrameParser frameParser = new FrameParser();

	private readonly object sendFrameSync = new object();

	private readonly object receiveFrameSync = new object();

	private readonly byte[] buffer = new byte[8];

	private FrameType? lastFrameType = null;

	public FrameHandler(IReceiverSocket receiver, ISenderSocket sender)
	{
		this.receiver = receiver;
		this.sender = sender;
	}

	public void SendFrameIdentifier(FrameIdentifier frameIdentifier)
	{
		lock (sendFrameSync)
		{
			FrameHeaderHelper.PopulateHeaderToBuffer(frameIdentifier.Header, buffer);
			sender.Send(buffer, 0, 2);
			ulong payloadLength = frameIdentifier.PayloadLength;
			byte length = frameIdentifier.Header.Length;
			switch (FrameLengthHelper.GetPayloadLengthType(length))
			{
			case PayloadLengthType.Short:
				ConverterHelper.PopulateBufferWithUShort((ushort)payloadLength, buffer, 0);
				sender.Send(buffer, 0, 2);
				break;
			case PayloadLengthType.Long:
				ConverterHelper.PopulateBufferWithULong(payloadLength, buffer, 0);
				sender.Send(buffer, 0, 8);
				break;
			}
			if (frameIdentifier.Header.IsMasked)
			{
				ConverterHelper.PopulateBufferWithUInt(frameIdentifier.Mask, buffer, 0);
				sender.Send(buffer, 0, 4);
			}
		}
	}

	public void SendFramePayload(FrameIdentifier frameIdentifier, ArraySegment<byte> data)
	{
		lock (sendFrameSync)
		{
			int num = (int)Math.Min(Math.Min(frameIdentifier.RemainedBytesToProcess, (ulong)data.Count), 2147483647uL);
			if (num > 0)
			{
				sender.Send(data.Array, data.Offset, num, frameIdentifier.Mask);
				frameIdentifier.ProcessBytesCount(num);
			}
		}
	}

	public FrameIdentifier ReceiveNextFrameIdentifier()
	{
		lock (receiveFrameSync)
		{
			ushort uShortNumber = receiver.GetUShortNumber();
			FrameHeader headerFromData = frameParser.GetHeaderFromData(uShortNumber);
			ulong payloadLength = frameParser.GetPayloadLength(headerFromData, receiver);
			uint maskIfExist = frameParser.GetMaskIfExist(headerFromData, receiver);
			FrameType currentFrameType = (headerFromData.Opcode = GetFrameType(headerFromData, lastFrameType));
			UpdateFrameType(currentFrameType);
			return new FrameIdentifier(headerFromData, payloadLength, maskIfExist);
		}
	}

	public int ReceivePayload(FrameIdentifier frameIdentifier, ArraySegment<byte> data)
	{
		lock (receiveFrameSync)
		{
			int num = (int)Math.Min(Math.Min(frameIdentifier.RemainedBytesToProcess, (ulong)data.Count), 2147483647uL);
			if (num > 0)
			{
				ArraySegment<byte> segmentData = new ArraySegment<byte>(data.Array, data.Offset, num);
				receiver.GetBytes(segmentData, frameIdentifier.Mask);
				frameIdentifier.ProcessBytesCount(num);
			}
			return num;
		}
	}

	private FrameType GetFrameType(FrameHeader header, FrameType? previousFrameType)
	{
		if (header.Opcode == FrameType.Continue && !previousFrameType.HasValue)
		{
			throw new ArgumentException("Cannot create WS Frame with Continue type opcode and no previous frame present");
		}
		if (header.Opcode != FrameType.Continue)
		{
			return header.Opcode;
		}
		return previousFrameType.Value;
	}

	private void UpdateFrameType(FrameType currentFrameType)
	{
		if (currentFrameType == FrameType.Binary || currentFrameType == FrameType.Text)
		{
			lastFrameType = currentFrameType;
		}
	}
}
