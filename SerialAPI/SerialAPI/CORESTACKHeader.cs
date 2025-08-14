using System;
using System.Collections.Generic;

namespace SerialAPI;

public class CORESTACKHeader
{
	private enum CheckIndex : byte
	{
		FrameType,
		AddressExtensionType,
		Fragmentation,
		QoS,
		LocalHopLimit,
		MacDestination,
		MacSource,
		IpDestination,
		IpSource,
		SyncWord
	}

	protected CorestackFrameType m_frameType;

	protected AddressExtensionType m_extensionType;

	protected bool m_macSecurity = true;

	protected bool m_fragmentation;

	protected bool m_ack;

	protected bool m_qos;

	protected byte m_localHopLimit = 7;

	protected byte[] m_macDestination = new byte[3];

	protected byte[] m_macSource;

	protected byte[] m_ipDestination;

	protected byte[] m_ipSource;

	protected byte[] m_mic;

	protected byte[] m_sequence_number;

	protected uint m_sequence_count;

	protected bool[] Checked;

	protected byte[] m_syncWord;

	protected int headerDataSize;

	public int HeaderSize => headerDataSize;

	public CorestackFrameType CorestackFrameType
	{
		get
		{
			return m_frameType;
		}
		set
		{
			m_frameType = value;
			Checked[0] = true;
		}
	}

	public AddressExtensionType AddressExtensionType
	{
		get
		{
			return m_extensionType;
		}
		set
		{
			m_extensionType = value;
			Checked[1] = true;
		}
	}

	public bool MacSecurity
	{
		get
		{
			return m_macSecurity;
		}
		set
		{
			m_macSecurity = value;
		}
	}

	public uint MacSecuritySequenceCount => m_sequence_count;

	public bool Fragmentation
	{
		get
		{
			return m_fragmentation;
		}
		set
		{
			m_fragmentation = value;
			Checked[2] = true;
		}
	}

	public bool QoS
	{
		get
		{
			return m_qos;
		}
		set
		{
			m_qos = value;
			Checked[3] = true;
		}
	}

	public byte LocalHopLimit
	{
		get
		{
			return m_localHopLimit;
		}
		set
		{
			m_localHopLimit = value;
			Checked[4] = true;
		}
	}

	public byte[] MacDestination
	{
		get
		{
			return m_macDestination;
		}
		set
		{
			if (value != null && value.Length > 2)
			{
				m_macDestination[0] = value[0];
				m_macDestination[1] = value[1];
				m_macDestination[2] = value[2];
				Checked[5] = true;
			}
		}
	}

	public byte[] MacSource
	{
		get
		{
			return m_macSource;
		}
		set
		{
			if (value != null && value.Length > 2)
			{
				m_macSource[0] = value[0];
				m_macSource[1] = value[1];
				m_macSource[2] = value[2];
				Checked[6] = true;
			}
		}
	}

	public byte[] IpDestination
	{
		get
		{
			return m_ipDestination;
		}
		set
		{
			m_ipDestination = value;
			Checked[7] = true;
		}
	}

	public byte[] IpSource
	{
		get
		{
			return m_ipSource;
		}
		set
		{
			m_ipSource = value;
			Checked[8] = true;
		}
	}

	public byte RSSI { get; set; }

	public byte[] SyncWord
	{
		get
		{
			return m_syncWord;
		}
		set
		{
			if (value.Length > 1)
			{
				m_syncWord[0] = value[0];
				m_syncWord[1] = value[1];
				Checked[9] = true;
			}
		}
	}

	public CORESTACKHeader()
	{
		m_macDestination = new byte[3];
		m_macSource = new byte[3];
		m_ipDestination = new byte[3];
		m_ipSource = new byte[3];
		m_mic = new byte[4];
		m_sequence_number = new byte[4];
		Checked = new bool[16];
		m_syncWord = new byte[2];
		m_syncWord[0] = 154;
		m_syncWord[1] = 125;
		Checked[9] = true;
		Checked[4] = true;
	}

	public CORESTACKHeader(CORESTACKHeader header)
	{
		m_frameType = header.m_frameType;
		m_extensionType = header.m_extensionType;
		m_macSecurity = header.m_macSecurity;
		m_fragmentation = header.m_fragmentation;
		m_ack = header.m_ack;
		m_qos = header.m_qos;
		m_localHopLimit = header.m_localHopLimit;
		m_macDestination = header.m_macDestination;
		m_macSource = header.m_macSource;
		m_ipDestination = header.m_ipDestination;
		m_ipSource = header.m_ipSource;
		m_mic = header.m_mic;
		m_sequence_number = header.m_sequence_number;
		m_sequence_count = header.m_sequence_count;
		Checked = header.Checked;
		m_syncWord = header.m_syncWord;
	}

	public void parse(List<byte> data)
	{
		m_frameType = (CorestackFrameType)((data[0] & 0x1C) >> 2);
		m_macSecurity = (data[0] & 0x20) == 32;
		m_ack = (data[0] & 0x40) == 64;
		m_qos = (data[0] & 0x80) == 128;
		m_extensionType = (AddressExtensionType)(data[1] & 0xF);
		m_localHopLimit = (byte)((data[1] & 0x70) >> 4);
		m_fragmentation = (data[1] & 0x80) == 128;
		data.CopyTo(2, m_macDestination, 0, 3);
		m_macDestination.CopyTo(m_ipDestination, 0);
		data.CopyTo(5, m_macSource, 0, 3);
		m_macSource.CopyTo(m_ipSource, 0);
		headerDataSize += 8;
		if (m_macSecurity)
		{
			data.CopyTo(8, m_sequence_number, 0, 4);
			for (int i = 0; i < m_sequence_number.Length; i++)
			{
				m_sequence_count <<= 8;
				m_sequence_count += m_sequence_number[i];
			}
			data.CopyTo(12, m_mic, 0, 4);
			headerDataSize += 8;
		}
		switch (m_extensionType)
		{
		case AddressExtensionType.FIRST_ROUTED:
			data.CopyTo(16, m_ipDestination, 0, 3);
			headerDataSize += 3;
			break;
		case AddressExtensionType.IN_PATH:
			data.CopyTo(16, m_ipDestination, 0, 3);
			data.CopyTo(19, m_ipSource, 0, 3);
			headerDataSize += 6;
			break;
		case AddressExtensionType.LAST_ROUTED:
			data.CopyTo(16, m_ipSource, 0, 3);
			headerDataSize += 3;
			break;
		}
	}

	public virtual List<byte> getHeaderAsSerial()
	{
		List<byte> list = new List<byte>();
		list.Add(0);
		int index = list.Count - 1;
		if (Checked[0])
		{
			list[index] |= (byte)((uint)m_frameType << 2);
		}
		list[index] |= (byte)(Convert.ToByte(m_macSecurity) << 5);
		if (Checked[3])
		{
			list[index] |= (byte)(Convert.ToByte(m_qos) << 7);
		}
		list.Add(0);
		index = list.Count - 1;
		if (Checked[1])
		{
			list[index] |= (byte)m_extensionType;
		}
		if (Checked[4])
		{
			list[index] |= (byte)(m_localHopLimit << 4);
		}
		if (Checked[2])
		{
			list[index] |= (byte)(Convert.ToByte(m_fragmentation) << 7);
		}
		if (Checked[5] && m_macDestination != null)
		{
			list.AddRange(m_macDestination);
		}
		else
		{
			byte[] collection = new byte[3];
			list.AddRange(collection);
		}
		if (Checked[6] && m_macSource != null)
		{
			list.AddRange(m_macSource);
		}
		else
		{
			byte[] collection2 = new byte[3];
			list.AddRange(collection2);
		}
		switch (m_extensionType)
		{
		case AddressExtensionType.FIRST_ROUTED:
			if (Checked[7] && m_ipDestination != null && m_ipDestination.Length >= 3)
			{
				list.Add(m_ipDestination[0]);
				list.Add(m_ipDestination[1]);
				list.Add(m_ipDestination[2]);
			}
			else
			{
				byte[] collection4 = new byte[3];
				list.AddRange(collection4);
			}
			break;
		case AddressExtensionType.IN_PATH:
			if (Checked[8] && m_ipSource != null && m_ipSource.Length >= 3)
			{
				list.Add(m_ipSource[0]);
				list.Add(m_ipSource[1]);
				list.Add(m_ipSource[2]);
			}
			else
			{
				byte[] collection5 = new byte[3];
				list.AddRange(collection5);
			}
			if (Checked[7] && m_ipDestination != null && m_ipDestination.Length >= 3)
			{
				list.Add(m_ipDestination[0]);
				list.Add(m_ipDestination[1]);
				list.Add(m_ipDestination[2]);
			}
			else
			{
				byte[] collection6 = new byte[3];
				list.AddRange(collection6);
			}
			break;
		case AddressExtensionType.LAST_ROUTED:
			if (Checked[8] && m_ipSource != null && m_ipSource.Length >= 3)
			{
				list.Add(m_ipSource[0]);
				list.Add(m_ipSource[1]);
				list.Add(m_ipSource[2]);
			}
			else
			{
				byte[] collection3 = new byte[3];
				list.AddRange(collection3);
			}
			break;
		}
		return list;
	}
}
