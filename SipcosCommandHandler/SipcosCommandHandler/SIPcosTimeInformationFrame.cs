using System;
using System.Collections.Generic;
using SerialAPI;

namespace SipcosCommandHandler;

public class SIPcosTimeInformationFrame : SIPCOSMessage
{
	private SIPcosTimeInforamtionMode m_time_mode;

	private DateTime m_time;

	public SIPcosTimeInforamtionMode TimeMode
	{
		get
		{
			return m_time_mode;
		}
		set
		{
			m_time_mode = value;
		}
	}

	public DateTime Time
	{
		get
		{
			return m_time;
		}
		set
		{
			m_time = value;
		}
	}

	public SIPcosTimeInformationFrame(SIPcosHeader header)
		: base(header)
	{
	}

	internal void parse(ref List<byte> message)
	{
		if (message.Count > 0)
		{
			m_time_mode = (SIPcosTimeInforamtionMode)message[0];
			message.RemoveAt(0);
		}
		if (message.Count > 5)
		{
			int year = 2000 + message[0];
			int month = Math.Max(1, (int)message[1]);
			int day = Math.Max(1, message[2] % 32);
			int hour = message[3];
			int minute = message[4];
			int second = message[5];
			m_time = new DateTime(year, month, day, hour, minute, second);
			message.RemoveRange(0, 6);
		}
	}
}
