namespace RWE.SmartHome.SHC.WebSocketsService.Client;

public enum WSFrameType : byte
{
	Continuation = 0,
	Text = 1,
	Binary = 2,
	Reserved = 3,
	Reserved04 = 4,
	Reserved05 = 5,
	Reserved06 = 6,
	Reserved07 = 7,
	Close = 8,
	Ping = 9,
	Pong = 10,
	Reserved0B = 11,
	Reserved0C = 12,
	Reserved0D = 13,
	Reserved0E = 14,
	Reserved0F = 15,
	Undefined = byte.MaxValue
}
