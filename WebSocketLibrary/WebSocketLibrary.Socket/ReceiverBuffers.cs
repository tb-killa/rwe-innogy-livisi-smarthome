using System;
using System.Collections.Generic;

namespace WebSocketLibrary.Socket;

public class ReceiverBuffers
{
	private readonly List<ArraySegment<byte>> buffers = new List<ArraySegment<byte>>();

	private readonly object sync = new object();

	public bool ExistsBuffers => buffers.Count > 0;

	public void AddBuffer(ArraySegment<byte> buffer)
	{
		lock (sync)
		{
			buffers.Add(buffer);
		}
	}

	public int PopulateFromBuffer(byte[] array, int offset, int count)
	{
		int result = 0;
		if (buffers.Count > 0)
		{
			ArraySegment<byte> arraySegment = buffers[0];
			buffers.RemoveAt(0);
			int num = PopulateFromSegment(arraySegment, array, offset, count);
			ArraySegment<byte> remainedBuffer = GetRemainedBuffer(arraySegment, num);
			if (remainedBuffer.Count > 0)
			{
				buffers.Insert(0, remainedBuffer);
			}
			result = num;
		}
		return result;
	}

	private int PopulateFromSegment(ArraySegment<byte> bufferSegment, byte[] array, int offset, int count)
	{
		int num = Math.Min(bufferSegment.Count, count);
		Array.Copy(bufferSegment.Array, bufferSegment.Offset, array, offset, num);
		return num;
	}

	private ArraySegment<byte> GetRemainedBuffer(ArraySegment<byte> buffer, int removedBytes)
	{
		int count = buffer.Count - removedBytes;
		int offset = buffer.Offset + removedBytes;
		return new ArraySegment<byte>(buffer.Array, offset, count);
	}
}
