namespace WebSocketLibrary.Managers.Frames;

public class ReceivedResult
{
	public bool IsFinalData { get; set; }

	public ResultDataType Type { get; set; }

	public int ReadBytes { get; set; }
}
