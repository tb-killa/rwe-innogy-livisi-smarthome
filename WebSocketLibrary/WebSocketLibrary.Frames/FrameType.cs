namespace WebSocketLibrary.Frames;

public enum FrameType : byte
{
	Continue = 0,
	Text = 1,
	Binary = 2,
	Close = 8,
	Ping = 9,
	Pong = 10
}
