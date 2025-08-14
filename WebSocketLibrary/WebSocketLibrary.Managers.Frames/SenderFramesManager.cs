using System;
using System.Text;
using WebSocketLibrary.Common;
using WebSocketLibrary.Exceptions;
using WebSocketLibrary.Frames;
using WebSocketLibrary.Handlers.Frames;
using WebSocketLibrary.Helpers;

namespace WebSocketLibrary.Managers.Frames;

public class SenderFramesManager
{
	private const int MaxFramePayloadSize = 1500;

	private readonly IFrameHandler frameHandler;

	private readonly PingPongWatcher pingPongWatcher;

	private readonly byte[] buffer = new byte[1500];

	private readonly object sync = new object();

	private readonly bool isServerSide;

	public SenderFramesManager(IFrameHandler frameHandler, PingPongWatcher pingPongWatcher, bool isServerSide)
	{
		this.isServerSide = isServerSide;
		this.frameHandler = frameHandler;
		this.pingPongWatcher = pingPongWatcher;
	}

	public void SendData(ArraySegment<byte> data)
	{
		lock (sync)
		{
			byte[] array = data.Array;
			int num = data.Count;
			int num2 = data.Offset;
			bool flag = true;
			while (num > 0)
			{
				int num3 = Math.Min(num, 1500);
				bool isFinal = num3 >= num;
				FrameIdentifier binaryFrameIdentifier = FrameFactory.GetBinaryFrameIdentifier((ulong)num3, !isServerSide);
				binaryFrameIdentifier.Header.IsFinal = isFinal;
				binaryFrameIdentifier.Header.Opcode = (flag ? FrameType.Binary : FrameType.Continue);
				ArraySegment<byte> data2 = new ArraySegment<byte>(array, num2, num3);
				frameHandler.SendFrameIdentifier(binaryFrameIdentifier);
				frameHandler.SendFramePayload(binaryFrameIdentifier, data2);
				num -= num3;
				num2 += num3;
				flag = false;
			}
		}
	}

	public void SendText(string text)
	{
		lock (sync)
		{
			int val = ((Encoding.UTF8.GetByteCount(text) <= 1500) ? 1500 : 375);
			int length = text.Length;
			int num = 0;
			int num2 = length;
			bool flag = true;
			while (num2 > 0)
			{
				int num3 = Math.Min(val, num2);
				int bytes = Encoding.UTF8.GetBytes(text, num, num3, buffer, 0);
				bool isFinal = num3 >= num2;
				FrameIdentifier textFrameIdentifier = FrameFactory.GetTextFrameIdentifier((ulong)bytes, !isServerSide);
				textFrameIdentifier.Header.IsFinal = isFinal;
				textFrameIdentifier.Header.Opcode = (flag ? FrameType.Text : FrameType.Continue);
				ArraySegment<byte> data = new ArraySegment<byte>(buffer, 0, bytes);
				frameHandler.SendFrameIdentifier(textFrameIdentifier);
				frameHandler.SendFramePayload(textFrameIdentifier, data);
				num2 -= num3;
				num += num3;
				flag = false;
			}
		}
	}

	public void SendCloseConnection(CloseFrameStatusCode code, string text)
	{
		ulong payloadLength = CloseFrameHelper.GetPayloadLength((ushort)code, text);
		if (payloadLength > 125)
		{
			throw new FrameSizeException($"Close frame payload is bigger than maximum size allowed, size: {payloadLength}");
		}
		lock (sync)
		{
			int num = 0;
			ConverterHelper.PopulateBufferWithUShort((ushort)code, buffer, 0);
			num += 2;
			num += Encoding.UTF8.GetBytes(text, 0, text.Length, buffer, 2);
			FrameIdentifier closeFrameIdentifier = FrameFactory.GetCloseFrameIdentifier((ulong)num, !isServerSide);
			ArraySegment<byte> data = new ArraySegment<byte>(buffer, 0, num);
			frameHandler.SendFrameIdentifier(closeFrameIdentifier);
			frameHandler.SendFramePayload(closeFrameIdentifier, data);
		}
	}

	public void SendPing(string text)
	{
		int byteCount = Encoding.UTF8.GetByteCount(text);
		if (byteCount > 125)
		{
			throw new FrameSizeException($"Ping frame payload is bigger than maximum size allowed, size: {byteCount}");
		}
		lock (sync)
		{
			int bytes = Encoding.UTF8.GetBytes(text, 0, text.Length, buffer, 0);
			FrameIdentifier pingFrameIdentifier = FrameFactory.GetPingFrameIdentifier((ulong)bytes, !isServerSide);
			ArraySegment<byte> data = new ArraySegment<byte>(buffer, 0, bytes);
			frameHandler.SendFrameIdentifier(pingFrameIdentifier);
			frameHandler.SendFramePayload(pingFrameIdentifier, data);
			pingPongWatcher.SendPing();
		}
	}

	public void SendPong(ArraySegment<byte> data)
	{
		if (data.Count > 125)
		{
			throw new FrameSizeException("Pong frame payload cannot be greater than 125 bytes");
		}
		lock (sync)
		{
			FrameIdentifier pongFrameIdentifier = FrameFactory.GetPongFrameIdentifier((ulong)data.Count, !isServerSide);
			frameHandler.SendFrameIdentifier(pongFrameIdentifier);
			frameHandler.SendFramePayload(pongFrameIdentifier, data);
		}
	}
}
