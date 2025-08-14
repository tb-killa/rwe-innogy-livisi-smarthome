namespace WebSocketLibrary.Frames;

public class FrameHeader
{
	public bool IsFinal { get; set; }

	public bool Reserved1 { get; set; }

	public bool Reserved2 { get; set; }

	public bool Reserved3 { get; set; }

	public FrameType Opcode { get; set; }

	public bool IsMasked { get; set; }

	public byte Length { get; set; }
}
