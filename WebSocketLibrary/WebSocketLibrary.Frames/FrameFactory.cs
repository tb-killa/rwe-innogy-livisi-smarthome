using WebSocketLibrary.Helpers;

namespace WebSocketLibrary.Frames;

public static class FrameFactory
{
	public static FrameIdentifier GetBinaryFrameIdentifier(ulong bytesCount, bool maskedFrame)
	{
		return GetFrameIdentifier(FrameType.Binary, bytesCount, maskedFrame);
	}

	public static FrameIdentifier GetTextFrameIdentifier(ulong payloadLength, bool maskedFrame)
	{
		return GetFrameIdentifier(FrameType.Text, payloadLength, maskedFrame);
	}

	public static FrameIdentifier GetPingFrameIdentifier(ulong payloadLenght, bool maskedFrame)
	{
		return GetFrameIdentifier(FrameType.Ping, payloadLenght, maskedFrame);
	}

	public static FrameIdentifier GetPongFrameIdentifier(ulong payloadLength, bool maskedFrame)
	{
		return GetFrameIdentifier(FrameType.Pong, payloadLength, maskedFrame);
	}

	public static FrameIdentifier GetCloseFrameIdentifier(ulong payloadLength, bool maskedFrame)
	{
		return GetFrameIdentifier(FrameType.Close, payloadLength, maskedFrame);
	}

	public static FrameIdentifier GetFrameIdentifier(FrameType frameType, ulong payloadLength, bool maskedFrame)
	{
		byte headerLength = FrameLengthHelper.GetHeaderLength(payloadLength);
		FrameHeader finalFrameHeader = FrameHeaderHelper.GetFinalFrameHeader(frameType, headerLength, maskedFrame);
		uint newMaskingKey = FrameMaskHelper.GetNewMaskingKey();
		return new FrameIdentifier(finalFrameHeader, payloadLength, newMaskingKey);
	}
}
