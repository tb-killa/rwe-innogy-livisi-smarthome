using System;

namespace WebSocketLibrary.Frames;

public class FrameIdentifier
{
	public uint Mask { get; private set; }

	public FrameHeader Header { get; private set; }

	public ulong PayloadLength { get; private set; }

	public FrameType Type => Header.Opcode;

	public ulong RemainedBytesToProcess { get; private set; }

	public FrameIdentifier(FrameHeader header, ulong payloadLength, uint mask)
	{
		Mask = (header.IsMasked ? mask : 0u);
		Header = header;
		PayloadLength = payloadLength;
		RemainedBytesToProcess = payloadLength;
	}

	public void ProcessBytesCount(int count)
	{
		RemainedBytesToProcess = Math.Max(0uL, RemainedBytesToProcess - (ulong)count);
	}
}
