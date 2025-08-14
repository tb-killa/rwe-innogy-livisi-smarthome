using System;
using WebSocketLibrary.Frames;

namespace WebSocketLibrary.Helpers;

public static class FrameHeaderHelper
{
	public static FrameHeader GetFrameHeader(ushort data)
	{
		byte b = (byte)((data >> 8) & 0xFF);
		byte b2 = (byte)(data & 0xFF);
		FrameHeader frameHeader = new FrameHeader();
		frameHeader.IsFinal = ((b >> 7) & 1) == 1;
		frameHeader.Reserved1 = ((b >> 6) & 1) == 1;
		frameHeader.Reserved2 = ((b >> 5) & 1) == 1;
		frameHeader.Reserved3 = ((b >> 4) & 1) == 1;
		frameHeader.Opcode = (FrameType)(b & 0xF);
		frameHeader.IsMasked = ((b2 >> 7) & 1) == 1;
		frameHeader.Length = (byte)(b2 & 0x7F);
		return frameHeader;
	}

	public static void PopulateHeaderToBuffer(FrameHeader header, byte[] buffer)
	{
		if (buffer.Length < 2)
		{
			throw new ArgumentException($"PopulateHeaderToBuffer buffer size insuficient: {buffer.Length}");
		}
		byte b = (byte)(header.Opcode & (FrameType)15);
		buffer[0] = (byte)((header.IsFinal ? 128 : 0) | (header.Reserved1 ? 64 : 0) | (header.Reserved2 ? 32 : 0) | (header.Reserved3 ? 16 : 0) | b);
		buffer[1] = (byte)((header.IsMasked ? 128 : 0) | (header.Length & 0x7F));
	}

	public static FrameHeader GetFinalFrameHeader(FrameType frameType, byte headerLength, bool masked)
	{
		FrameHeader frameHeader = new FrameHeader();
		frameHeader.IsFinal = true;
		frameHeader.IsMasked = masked;
		frameHeader.Length = headerLength;
		frameHeader.Opcode = frameType;
		frameHeader.Reserved1 = false;
		frameHeader.Reserved2 = false;
		frameHeader.Reserved3 = false;
		return frameHeader;
	}
}
