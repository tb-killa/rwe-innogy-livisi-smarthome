using System;

namespace WebSocketLibrary.Managers.Frames;

public class ReceiverBuffer
{
	public ArraySegment<byte> Data { get; set; }

	public ResultDataType Type { get; set; }

	public bool IsAllBytesRead { get; set; }

	public bool IsFinalFrame { get; set; }

	public int ReadBytes { get; set; }
}
