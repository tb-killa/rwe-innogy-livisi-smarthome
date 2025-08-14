using System.Collections.Generic;

namespace SerialAPI.BidCoSFrames;

internal class BIDCOSSwitchFrame : BIDCOSMessage
{
	private bool m_lowBat;

	private bool m_duration;

	private byte m_channelNumber;

	private byte m_keyCounter;

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

	public bool Duration
	{
		get
		{
			return m_duration;
		}
		set
		{
			m_duration = value;
		}
	}

	public byte ChannelNumber
	{
		get
		{
			return m_channelNumber;
		}
		set
		{
			m_channelNumber = value;
		}
	}

	public byte KeyCounter
	{
		get
		{
			return m_keyCounter;
		}
		set
		{
			m_keyCounter = value;
		}
	}

	public BIDCOSSwitchFrame(BIDCOSHeader header)
		: base(header)
	{
	}

	public bool Parse(List<byte> message)
	{
		if (message.Count >= 2)
		{
			m_data = message;
			m_lowBat = (message[0] & 0x80) == 128;
			m_duration = (message[0] & 0x40) == 64;
			m_channelNumber = (byte)(message[0] & 0x3F);
			m_keyCounter = message[1];
			return true;
		}
		return false;
	}

	public override void GenerateSIPCOSMessage(ref CORESTACKHeader header, ref List<byte> data)
	{
		base.GenerateSIPCOSMessage(ref header, ref data);
		data.Clear();
		byte b = 18;
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
		data.Add(1);
		data.Add(m_channelNumber);
		data.Add(0);
		if (m_lowBat)
		{
			data.Add(128);
		}
		else
		{
			data.Add(0);
		}
		data.Add(128);
	}
}
