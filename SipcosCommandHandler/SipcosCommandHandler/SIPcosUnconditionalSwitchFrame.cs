using System.Collections.Generic;
using SerialAPI;

namespace SipcosCommandHandler;

public class SIPcosUnconditionalSwitchFrame : SIPCOSMessage
{
	private byte m_channel;

	private bool m_longPress;

	private byte m_count;

	public byte Channel
	{
		get
		{
			return m_channel;
		}
		set
		{
			m_channel = value;
		}
	}

	public bool LongPress
	{
		get
		{
			return m_longPress;
		}
		set
		{
			m_longPress = value;
		}
	}

	public byte Count
	{
		get
		{
			return m_count;
		}
		set
		{
			m_count = value;
		}
	}

	public SIPcosUnconditionalSwitchFrame(SIPcosHeader header)
		: base(header)
	{
	}

	internal void parse(ref List<byte> message)
	{
		if (message.Count >= 2)
		{
			m_message.Clear();
			m_message.AddRange(message);
			m_longPress = (message[0] & 0x80) == 128;
			m_channel = (byte)(message[0] & 0x7F);
			m_count = message[1];
		}
	}
}
