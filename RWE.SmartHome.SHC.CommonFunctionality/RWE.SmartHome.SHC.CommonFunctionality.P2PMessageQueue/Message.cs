namespace RWE.SmartHome.SHC.CommonFunctionality.P2PMessageQueue;

public class Message
{
	private byte[] mBytes;

	public bool IsAlert { get; set; }

	public virtual byte[] MessageBytes
	{
		get
		{
			return mBytes;
		}
		set
		{
			mBytes = value;
		}
	}

	public Message()
		: this(null, isAlert: false)
	{
	}

	public Message(byte[] bytes)
		: this(bytes, isAlert: false)
	{
	}

	public Message(byte[] bytes, bool isAlert)
	{
		mBytes = bytes;
		IsAlert = isAlert;
	}
}
