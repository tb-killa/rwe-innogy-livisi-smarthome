using System.Collections.Generic;
using SerialAPI;

namespace SipcosCommandHandler;

public class SIPcosAnswerFrame : SIPCOSMessage
{
	private SIPcosAnswerFrameStatus m_status;

	private bool m_shouldContactSNC;

	private byte m_key_channel_number;

	private byte m_isLevel;

	private bool m_lowBat;

	private bool m_clock;

	private SIPcosAnswerCondition m_condition;

	private byte m_channelError;

	private bool m_dutyCycle;

	private byte m_rssi;

	public SIPcosAnswerFrameStatus Status
	{
		get
		{
			return m_status;
		}
		set
		{
			m_status = value;
		}
	}

	public bool ShouldContactSNC
	{
		get
		{
			return m_shouldContactSNC;
		}
		set
		{
			m_shouldContactSNC = value;
		}
	}

	public byte KeyChannelNumber
	{
		get
		{
			return m_key_channel_number;
		}
		set
		{
			m_key_channel_number = value;
		}
	}

	public byte IsLevel
	{
		get
		{
			return m_isLevel;
		}
		set
		{
			m_isLevel = value;
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

	public SIPcosAnswerCondition Condition
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

	public byte RSSI
	{
		get
		{
			return m_rssi;
		}
		set
		{
			m_rssi = value;
		}
	}

	public SIPcosAnswerFrame(SIPcosHeader header)
		: base(header)
	{
	}

	internal void parse(ref List<byte> message)
	{
		if (message.Count >= 1)
		{
			m_message.Clear();
			m_message.AddRange(message);
			m_status = (SIPcosAnswerFrameStatus)(message[0] & 0xBF);
			m_shouldContactSNC = (message[0] & 0x40) == 64;
			if (m_status == SIPcosAnswerFrameStatus.STATUSACK && message.Count >= 5)
			{
				m_key_channel_number = message[1];
				m_isLevel = message[2];
				m_lowBat = (message[3] & 0x80) == 128;
				m_clock = (message[3] & 0x40) == 64;
				m_condition = (SIPcosAnswerCondition)((message[3] >> 4) & 3);
				m_channelError = (byte)((message[3] >> 1) & 7);
				m_dutyCycle = (message[3] & 1) == 1;
				m_rssi = message[4];
			}
		}
	}
}
