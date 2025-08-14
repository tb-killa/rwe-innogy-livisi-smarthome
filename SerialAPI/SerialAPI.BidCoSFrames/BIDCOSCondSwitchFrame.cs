using System.Collections.Generic;
using System.Text;

namespace SerialAPI.BidCoSFrames;

internal class BIDCOSCondSwitchFrame : BIDCOSMessage
{
	private bool m_lowBat;

	private bool m_keyPressTime;

	private byte m_channelNumber;

	private byte m_keyCounter;

	private byte m_measuredValue;

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

	public bool KeyPressTime
	{
		get
		{
			return m_keyPressTime;
		}
		set
		{
			m_keyPressTime = value;
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

	public byte MeasuredValue
	{
		get
		{
			return m_measuredValue;
		}
		set
		{
			m_measuredValue = value;
		}
	}

	public BIDCOSCondSwitchFrame(BIDCOSHeader header)
		: base(header)
	{
	}

	public bool Parse(List<byte> message)
	{
		if (message.Count >= 3)
		{
			m_data = message;
			m_lowBat = (message[0] & 0x80) == 128;
			m_keyPressTime = (message[0] & 0x40) == 64;
			m_channelNumber = (byte)(message[0] & 0x3F);
			m_keyCounter = message[1];
			m_measuredValue = message[2];
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
		data.Add(m_measuredValue);
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

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("Serial API: RBIDCOSCondSwitchFrame: ");
		if (m_header != null)
		{
			stringBuilder.Append(m_header.ToString());
		}
		else
		{
			stringBuilder.Append("Header is null");
		}
		stringBuilder.Append($" Channel: {m_channelNumber}, Current Value: {m_measuredValue}, Battery Low: {m_lowBat}");
		return stringBuilder.ToString();
	}
}
