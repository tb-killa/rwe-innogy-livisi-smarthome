using System.Collections.Generic;

namespace SerialAPI;

public class SIPCOSMessage : CORESTACKMessage
{
	protected SIPcosHeader m_header = new SIPcosHeader();

	protected List<byte> m_message = new List<byte>();

	public new SIPcosHeader Header
	{
		get
		{
			return m_header;
		}
		set
		{
			m_header = value;
		}
	}

	public new List<byte> Data
	{
		get
		{
			return m_message;
		}
		set
		{
			m_message = value;
		}
	}

	public bool ExpectReply { get; set; }

	public SIPCOSMessage()
	{
	}

	public SIPCOSMessage(SIPCOSMessage Message)
		: this(Message.m_header, Message.m_message, Message.m_mode)
	{
	}

	public SIPCOSMessage(CORESTACKMessage Message)
		: this(new SIPcosHeader(Message.Header), Message.Data, Message.Mode)
	{
	}

	public SIPCOSMessage(SIPcosHeader header)
		: this(header, new List<byte>())
	{
	}

	public SIPCOSMessage(SIPcosHeader header, byte[] message)
		: this(header, message, SendMode.Normal)
	{
	}

	public SIPCOSMessage(SIPcosHeader header, List<byte> message)
		: this(header, message, SendMode.Normal)
	{
	}

	public SIPCOSMessage(SIPcosHeader header, byte[] message, SendMode Mode)
	{
		m_header = header;
		if (message != null)
		{
			m_message.AddRange(message);
		}
		m_mode = Mode;
	}

	public SIPCOSMessage(SIPcosHeader header, List<byte> message, SendMode Mode)
	{
		m_header = header;
		if (message != null)
		{
			m_message = message;
		}
		m_mode = Mode;
	}
}
