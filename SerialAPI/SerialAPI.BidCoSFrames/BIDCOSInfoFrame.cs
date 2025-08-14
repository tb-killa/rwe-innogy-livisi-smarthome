using System.Collections.Generic;
using System.Text;

namespace SerialAPI.BidCoSFrames;

internal class BIDCOSInfoFrame : BIDCOSMessage
{
	private byte m_infoType;

	private byte m_channel;

	private byte m_currentValue;

	private bool m_lowBat;

	private bool m_clock;

	private byte m_condition;

	private byte m_channelError;

	private bool m_dutyCycle;

	private byte[,] m_partners;

	public byte InfoType
	{
		get
		{
			return m_infoType;
		}
		set
		{
			m_infoType = value;
		}
	}

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

	public byte CurrentValue
	{
		get
		{
			return m_currentValue;
		}
		set
		{
			m_currentValue = value;
		}
	}

	public bool LowBat
	{
		get
		{
			return m_lowBat;
		}
		set
		{
			m_lowBat = value;
		}
	}

	public bool Clock
	{
		get
		{
			return m_clock;
		}
		set
		{
			m_clock = value;
		}
	}

	public byte Condition
	{
		get
		{
			return m_condition;
		}
		set
		{
			m_condition = value;
		}
	}

	public byte ChannelError
	{
		get
		{
			return m_channelError;
		}
		set
		{
			m_channelError = value;
		}
	}

	public bool BiDi
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public bool DutyCycle
	{
		get
		{
			return m_dutyCycle;
		}
		set
		{
			m_dutyCycle = value;
		}
	}

	public byte[,] Partners
	{
		get
		{
			return m_partners;
		}
		set
		{
			m_partners = value;
		}
	}

	public BIDCOSInfoFrame(BIDCOSHeader header)
		: base(header)
	{
	}

	public bool Parse(List<byte> message)
	{
		if (message.Count >= 4)
		{
			m_data = message;
			m_infoType = message[0];
			if (m_infoType == 1)
			{
				m_partners = new byte[2, 4];
				for (int i = 0; i < 2; i++)
				{
					for (int j = 0; j < 4 && 1 + i * 4 + j < message.Count; j++)
					{
						m_partners[i, j] = message[1 + i * 4 + j];
					}
				}
			}
			else
			{
				m_channel = message[1];
				m_currentValue = message[2];
				m_lowBat = (message[3] & 0x80) == 128;
				m_clock = (message[3] & 0x40) == 64;
				m_dutyCycle = (message[3] & 1) == 1;
				m_condition = (byte)((message[3] >> 4) & 3);
				m_channelError = (byte)((message[3] >> 1) & 7);
			}
			return true;
		}
		return false;
	}

	public override void GenerateSIPCOSMessage(ref CORESTACKHeader header, ref List<byte> data)
	{
		base.GenerateSIPCOSMessage(ref header, ref data);
		data.Clear();
		byte b = 18;
		if (m_infoType == 1)
		{
			b = 17;
		}
		if ((m_header.HeaderBits & BIDCOSHeaderBitField.Bidi) == BIDCOSHeaderBitField.Bidi)
		{
			b |= 0x80;
		}
		if ((m_header.HeaderBits & BIDCOSHeaderBitField.WakeUp) == BIDCOSHeaderBitField.WakeUp)
		{
			b |= 0x40;
		}
		data.Add(b);
		data.Add(m_header.FrameCounter);
		if (m_infoType == 1)
		{
			data.Add(1);
			data.Add(10);
			data.Add(128);
			for (int i = 0; i < m_partners.Length / 4; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					data.Add(m_partners[i, j]);
				}
			}
		}
		else
		{
			data.Add(1);
			data.Add(m_channel);
			data.Add(m_currentValue);
			data.Add(m_data[3]);
			data.Add(128);
		}
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("Serial Api: BIDCOSInfoFrame: ");
		if (m_header != null)
		{
			stringBuilder.Append(m_header.ToString());
		}
		else
		{
			stringBuilder.Append("Header is null");
		}
		stringBuilder.Append($" Channel: {m_channel}, Current Value: {m_currentValue}, Battery Low: {m_lowBat}");
		return stringBuilder.ToString();
	}
}
