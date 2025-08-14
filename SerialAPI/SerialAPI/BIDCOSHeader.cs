using System.Collections.Generic;
using System.Text;

namespace SerialAPI;

internal class BIDCOSHeader
{
	private byte m_frameCounter;

	private BIDCOSHeaderBitField m_headerBits;

	private BIDCOSFrameType m_frameType;

	private byte[] m_sender;

	private byte[] m_receiver;

	public byte FrameCounter
	{
		get
		{
			return m_frameCounter;
		}
		set
		{
			m_frameCounter = value;
		}
	}

	public BIDCOSHeaderBitField HeaderBits
	{
		get
		{
			return m_headerBits;
		}
		set
		{
			m_headerBits = value;
		}
	}

	public BIDCOSFrameType FrameType
	{
		get
		{
			return m_frameType;
		}
		set
		{
			m_frameType = value;
		}
	}

	public byte[] Sender
	{
		get
		{
			return m_sender;
		}
		set
		{
			m_sender = value;
		}
	}

	public byte[] Receiver
	{
		get
		{
			return m_receiver;
		}
		set
		{
			m_receiver = value;
		}
	}

	public bool Parse(ref List<byte> message)
	{
		if (message.Count >= 9)
		{
			m_frameCounter = message[0];
			m_headerBits = (BIDCOSHeaderBitField)message[1];
			m_frameType = (BIDCOSFrameType)message[2];
			m_sender = new byte[3];
			m_sender[0] = message[3];
			m_sender[1] = message[4];
			m_sender[2] = message[5];
			m_receiver = new byte[3];
			m_receiver[0] = message[6];
			m_receiver[1] = message[7];
			m_receiver[2] = message[8];
			message.RemoveRange(0, 9);
			if ((m_headerBits & BIDCOSHeaderBitField.Bidi) != BIDCOSHeaderBitField.Bidi && (m_headerBits & BIDCOSHeaderBitField.Broadcast) == BIDCOSHeaderBitField.Broadcast && (m_receiver[0] != 0 || m_receiver[1] != 0 || m_receiver[2] != 0))
			{
				byte[] sender = m_sender;
				m_sender = m_receiver;
				m_receiver = sender;
			}
			return true;
		}
		return false;
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append($"Sender: {Sender[0]:X2} {Sender[1]:X2} {Sender[2]:X2}, ");
		stringBuilder.Append($"Receiver: {Receiver[0]:X2} {Receiver[1]:X2} {Receiver[2]:X2},");
		return stringBuilder.ToString();
	}
}
