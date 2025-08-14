using System.Collections.Generic;

namespace SerialAPI;

public class ICMPMessage : CORESTACKMessage
{
	protected ICMP_type m_icmp_type = ICMP_type.ECHO_REPLY;

	protected byte m_icmp_rssi;

	protected byte m_icmp_operation_mode;

	public ICMP_type Type
	{
		get
		{
			return m_icmp_type;
		}
		set
		{
			m_icmp_type = value;
		}
	}

	public byte RSSI
	{
		get
		{
			return m_icmp_rssi;
		}
		set
		{
			m_icmp_rssi = value;
		}
	}

	public byte OperationMode
	{
		get
		{
			return m_icmp_operation_mode;
		}
		set
		{
			m_icmp_operation_mode = value;
		}
	}

	public ICMPMessage()
	{
	}

	public ICMPMessage(CORESTACKHeader header)
		: base(header, new List<byte>())
	{
	}

	public void Parse(ref List<byte> message)
	{
		if (message.Count > 0)
		{
			m_icmp_operation_mode = message[0];
			message.RemoveAt(0);
		}
		if (message.Count > 0)
		{
			m_icmp_rssi = message[0];
			message.RemoveAt(0);
		}
		if (message.Count > 0)
		{
			m_icmp_type = (ICMP_type)message[0];
			message.RemoveAt(0);
		}
	}

	public override string ToString()
	{
		string text = $"{m_core_header.MacSource[0]:X2} {m_core_header.MacSource[1]:X2} {m_core_header.MacSource[2]:X2}";
		string text2 = text;
		return text2 + ": " + m_icmp_type.ToString() + " RSSI: " + m_icmp_rssi + " dB <--> " + base.Header.RSSI + " dB";
	}
}
