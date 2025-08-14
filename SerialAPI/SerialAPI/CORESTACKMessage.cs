using System.Collections.Generic;

namespace SerialAPI;

public class CORESTACKMessage
{
	protected CORESTACKHeader m_core_header = new CORESTACKHeader();

	protected List<byte> m_core_message = new List<byte>();

	protected SendMode m_mode;

	public CORESTACKHeader Header
	{
		get
		{
			return m_core_header;
		}
		set
		{
			m_core_header = value;
		}
	}

	public List<byte> Data
	{
		get
		{
			return m_core_message;
		}
		set
		{
			m_core_message = value;
		}
	}

	public SendMode Mode
	{
		get
		{
			return m_mode;
		}
		set
		{
			m_mode = value;
		}
	}

	public CORESTACKMessage()
	{
	}

	public CORESTACKMessage(CORESTACKMessage Message)
		: this(Message.m_core_header, Message.m_core_message, Message.m_mode)
	{
	}

	public CORESTACKMessage(CORESTACKHeader header, List<byte> message)
		: this(header, message, SendMode.Normal)
	{
	}

	public CORESTACKMessage(CORESTACKHeader header, List<byte> message, SendMode Mode)
	{
		m_core_header = header;
		m_core_message = message;
		m_mode = Mode;
	}
}
