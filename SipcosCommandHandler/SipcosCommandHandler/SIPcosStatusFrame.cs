using System.Collections.Generic;
using SerialAPI;

namespace SipcosCommandHandler;

public class SIPcosStatusFrame : SIPCOSMessage
{
	private SIPcosStatusType m_type;

	private byte m_channel_number;

	private byte m_isLevel;

	private bool m_lowBat;

	private bool m_clock;

	private SIPcosAnswerCondition m_condition;

	private SIPcosStatusTimeSlotMode m_timeslot_mode;

	private byte m_channelError;

	private bool m_dutyCycle;

	private byte m_rssi;

	private bool m_freeze;

	private bool m_mold;

	private ushort m_temperature;

	private byte m_humidity;

	private byte m_setpoint;

	private byte m_control_value;

	public SIPcosStatusType Type
	{
		get
		{
			return m_type;
		}
		set
		{
			m_type = value;
		}
	}

	public byte KeyChannelNumber
	{
		get
		{
			return m_channel_number;
		}
		set
		{
			m_channel_number = value;
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

	public SIPcosStatusTimeSlotMode TimeSlotMode
	{
		get
		{
			return m_timeslot_mode;
		}
		set
		{
			m_timeslot_mode = value;
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

	public bool Freeze
	{
		get
		{
			return m_freeze;
		}
		set
		{
			m_freeze = value;
		}
	}

	public bool Mold
	{
		get
		{
			return m_mold;
		}
		set
		{
			m_mold = value;
		}
	}

	public ushort Temperature
	{
		get
		{
			return m_temperature;
		}
		set
		{
			m_temperature = value;
		}
	}

	public byte Humidity
	{
		get
		{
			return m_humidity;
		}
		set
		{
			m_humidity = value;
		}
	}

	public byte Setpoint
	{
		get
		{
			return m_setpoint;
		}
		set
		{
			m_setpoint = value;
		}
	}

	public byte ControlValue
	{
		get
		{
			return m_control_value;
		}
		set
		{
			m_control_value = value;
		}
	}

	public SIPcosStatusFrame(SIPcosHeader header)
		: base(header)
	{
	}

	internal void parse(ref List<byte> message)
	{
		if (message.Count < 2)
		{
			return;
		}
		m_message.Clear();
		m_message.AddRange(message);
		m_type = (SIPcosStatusType)message[0];
		m_channel_number = message[1];
		if (m_type == SIPcosStatusType.STATUS_REQUEST)
		{
			return;
		}
		if (m_type == SIPcosStatusType.STATUS_FRAME)
		{
			if (message.Count >= 5)
			{
				m_isLevel = message[2];
				m_lowBat = (message[3] & 0x80) == 128;
				m_clock = (message[3] & 0x40) == 64;
				m_condition = (SIPcosAnswerCondition)((message[3] >> 4) & 3);
				m_channelError = (byte)((message[3] >> 1) & 7);
				m_dutyCycle = (message[3] & 1) == 1;
				m_rssi = message[4];
			}
		}
		else if (m_type == SIPcosStatusType.STATUS_FRAME_CC_SENSOR && message.Count >= 8)
		{
			m_freeze = (message[2] & 0x80) == 128;
			m_mold = (message[2] & 0x40) == 64;
			m_temperature = 0;
			m_temperature += (ushort)(message[2] & 0xF);
			m_temperature *= 256;
			m_temperature += message[3];
			m_humidity = message[4];
			m_control_value = message[5];
			m_lowBat = (message[6] & 0x80) == 128;
			m_channelError = (byte)((message[6] >> 1) & 7);
			m_dutyCycle = (message[6] & 1) == 1;
			m_rssi = message[7];
		}
	}

	internal void parseTimeSlot(ref List<byte> message)
	{
		if (message.Count >= 7)
		{
			m_message.Clear();
			m_message.AddRange(message);
			m_type = SIPcosStatusType.STATUS_FRAME_CC_SENSOR;
			m_channel_number = 0;
			m_freeze = (message[0] & 0x80) == 128;
			m_mold = (message[0] & 0x40) == 64;
			m_temperature = 0;
			m_temperature += (ushort)(message[0] & 0xF);
			m_temperature *= 256;
			m_temperature += message[1];
			m_humidity = message[2];
			m_setpoint = message[3];
			m_control_value = message[4];
			m_timeslot_mode = (SIPcosStatusTimeSlotMode)m_message[5];
			m_lowBat = (message[6] & 0x80) == 128;
			m_channelError = (byte)((message[6] >> 1) & 7);
			m_dutyCycle = (message[6] & 1) == 1;
		}
	}
}
