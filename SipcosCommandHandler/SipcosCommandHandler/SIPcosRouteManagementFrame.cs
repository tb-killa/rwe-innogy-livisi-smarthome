using System.Collections.Generic;
using SerialAPI;

namespace SipcosCommandHandler;

public class SIPcosRouteManagementFrame : SIPCOSMessage
{
	private RouteManagementCommand m_command;

	private RouteManagementAddressFamilyIdentifier m_addressFamily;

	private byte[] m_address;

	private byte[] m_next;

	private byte m_distance;

	private byte m_rssi_from;

	private byte m_rssi_received;

	public RouteManagementCommand Command
	{
		get
		{
			return m_command;
		}
		set
		{
			m_command = value;
		}
	}

	public RouteManagementAddressFamilyIdentifier AddressFamily
	{
		get
		{
			return m_addressFamily;
		}
		set
		{
			m_addressFamily = value;
		}
	}

	public byte[] Address
	{
		get
		{
			return m_address;
		}
		set
		{
			m_address = value;
		}
	}

	public byte[] Next
	{
		get
		{
			return m_next;
		}
		set
		{
			m_next = value;
		}
	}

	public byte Distance
	{
		get
		{
			return m_distance;
		}
		set
		{
			m_distance = value;
		}
	}

	public byte RSSI_from
	{
		get
		{
			return m_rssi_from;
		}
		set
		{
			m_rssi_from = value;
		}
	}

	public byte RSSI_received
	{
		get
		{
			return m_rssi_received;
		}
		set
		{
			m_rssi_received = value;
		}
	}

	public SIPcosRouteManagementFrame(SIPcosHeader header)
		: base(header, new List<byte>())
	{
	}

	public void parse(ref List<byte> message)
	{
		m_message = new List<byte>();
		if (message.Count > 0)
		{
			m_command = (RouteManagementCommand)message[0];
			m_message.Add(message[0]);
			message.RemoveAt(0);
		}
		if (message.Count > 0)
		{
			m_addressFamily = (RouteManagementAddressFamilyIdentifier)message[0];
			m_message.Add(message[0]);
			message.RemoveAt(0);
		}
		if (message.Count > 3)
		{
			m_address = new byte[3];
			m_address[0] = message[0];
			m_address[1] = message[1];
			m_address[2] = message[2];
			m_message.AddRange(message.GetRange(0, 3));
			message.RemoveRange(0, 3);
		}
		if (message.Count > 3)
		{
			m_next = new byte[3];
			m_next[0] = message[0];
			m_next[1] = message[1];
			m_next[2] = message[2];
			m_message.AddRange(message.GetRange(0, 3));
			message.RemoveRange(0, 3);
		}
		if (message.Count > 0)
		{
			m_distance = message[0];
			m_message.Add(message[0]);
			message.RemoveAt(0);
		}
		if (message.Count > 0)
		{
			m_rssi_from = (byte)(message[0] & 0xF0);
			m_rssi_received = (byte)((message[0] & 0xF) << 4);
			m_message.Add(message[0]);
			message.RemoveAt(0);
		}
	}
}
