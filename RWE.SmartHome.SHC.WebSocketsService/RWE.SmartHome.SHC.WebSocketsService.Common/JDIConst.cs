namespace RWE.SmartHome.SHC.WebSocketsService.Common;

public static class JDIConst
{
	public enum ByteOrder
	{
		Network,
		BigEndian,
		LittleEndian
	}

	public const int MicrosecondsPerSecond = 1000000;

	public const int MillisecondsPerSecond = 1000;
}
