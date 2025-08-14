namespace WebSocketLibrary.Helpers;

public static class FrameLengthHelper
{
	public static PayloadLengthType GetPayloadLengthType(byte headerPayloadLength)
	{
		if (headerPayloadLength < 126)
		{
			return PayloadLengthType.Byte;
		}
		if (headerPayloadLength == 126)
		{
			return PayloadLengthType.Short;
		}
		return PayloadLengthType.Long;
	}

	public static byte GetHeaderLength(ulong length)
	{
		if (length < 126)
		{
			return (byte)length;
		}
		if (length >= 126 && length <= 65535)
		{
			return 126;
		}
		return 127;
	}
}
