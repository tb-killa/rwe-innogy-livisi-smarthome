using System;
using System.Threading;
using WebSocketLibrary.Helpers;

namespace WebSocketLibrary.Socket;

public class ReceiverSocket : IReceiverSocket
{
	private readonly IBaseSocket socket;

	private readonly byte[] numbersBuffer = new byte[8];

	private readonly ReceiverBuffers buffers = new ReceiverBuffers();

	private readonly object sync = new object();

	public ReceiverSocket(IBaseSocket socket)
	{
		this.socket = socket;
	}

	public void GetBytes(ArraySegment<byte> segmentData, uint mask)
	{
		byte[] array = segmentData.Array;
		int num = segmentData.Offset;
		int num2 = segmentData.Count;
		while (num2 > 0)
		{
			int num3 = ReceiveFromSocket(array, num, num2);
			num2 -= num3;
			num += num3;
			if (num3 == 0)
			{
				Thread.Sleep(10);
			}
		}
		FrameMaskHelper.ApplyMaskOnBuffer(array, 0uL, (ulong)array.Length, mask);
	}

	public int ReceiveBytes(ArraySegment<byte> segmentData, uint mask)
	{
		int num = ReceiveFromSocket(segmentData.Array, segmentData.Offset, segmentData.Count);
		FrameMaskHelper.ApplyMaskOnBuffer(segmentData.Array, (ulong)segmentData.Offset, (ulong)num, mask);
		return num;
	}

	public void InsertBufferForReceive(byte[] buffer, int offset, int count)
	{
		byte[] array = new byte[count];
		Array.Copy(buffer, offset, array, 0, count);
		buffers.AddBuffer(new ArraySegment<byte>(array));
	}

	public ushort GetUShortNumber()
	{
		ushort num = 0;
		int num2 = 2;
		lock (sync)
		{
			GetBytes(new ArraySegment<byte>(numbersBuffer, 0, num2), 0u);
			for (int i = 0; i < num2; i++)
			{
				num <<= 8;
				num |= numbersBuffer[i];
			}
			return num;
		}
	}

	public uint GetUIntNumber()
	{
		uint num = 0u;
		int num2 = 4;
		lock (sync)
		{
			GetBytes(new ArraySegment<byte>(numbersBuffer, 0, num2), 0u);
			for (int i = 0; i < num2; i++)
			{
				num <<= 8;
				num |= numbersBuffer[i];
			}
			return num;
		}
	}

	public ulong GetULongNumber()
	{
		ulong num = 0uL;
		int num2 = 8;
		lock (sync)
		{
			GetBytes(new ArraySegment<byte>(numbersBuffer, 0, num2), 0u);
			for (int i = 0; i < num2; i++)
			{
				num <<= 8;
				num |= numbersBuffer[i];
			}
			return num;
		}
	}

	private int ReceiveFromSocket(byte[] array, int offset, int count)
	{
		if (buffers.ExistsBuffers)
		{
			return buffers.PopulateFromBuffer(array, offset, count);
		}
		return socket.ReceiveBytes(array, offset, count);
	}
}
