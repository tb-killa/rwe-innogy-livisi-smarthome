using System;
using System.Collections.Generic;

namespace SerialAPI;

public class SIPcosHeader : CORESTACKHeader
{
	private SIPcosFrameType m_sipcosFrameType = SIPcosFrameType.ANSWER;

	private bool m_stayAwake;

	private bool m_bidi;

	private byte m_sequenceNumber;

	public SIPcosFrameType FrameType
	{
		get
		{
			return m_sipcosFrameType;
		}
		set
		{
			m_sipcosFrameType = value;
		}
	}

	public bool StayAwake
	{
		get
		{
			return m_stayAwake;
		}
		set
		{
			m_stayAwake = value;
		}
	}

	public bool BiDi
	{
		get
		{
			return m_bidi;
		}
		set
		{
			m_bidi = value;
		}
	}

	public byte SequenceNumber
	{
		get
		{
			return m_sequenceNumber;
		}
		set
		{
			m_sequenceNumber = value;
		}
	}

	public byte[] Destination
	{
		get
		{
			return base.MacDestination;
		}
		set
		{
			base.MacDestination = value;
		}
	}

	public byte[] Source
	{
		get
		{
			return base.IpSource;
		}
		set
		{
			base.IpSource = value;
		}
	}

	public SIPcosHeader()
	{
		m_frameType = CorestackFrameType.SIPCOS_APPLICATION;
	}

	public SIPcosHeader(SIPcosHeader header)
		: base(header)
	{
		m_sipcosFrameType = header.m_sipcosFrameType;
		m_stayAwake = header.m_stayAwake;
		m_bidi = header.m_bidi;
		m_sequenceNumber = header.m_sequenceNumber;
	}

	public SIPcosHeader(CORESTACKHeader header)
		: base(header)
	{
		m_frameType = CorestackFrameType.SIPCOS_APPLICATION;
	}

	public void parseHeader(List<byte> data)
	{
		if (data.Count > 0)
		{
			m_sipcosFrameType = (SIPcosFrameType)(data[0] & 0x3F);
			m_stayAwake = (data[0] & 0x40) == 64;
			m_bidi = (data[0] & 0x80) == 128;
			m_sequenceNumber = data[1];
			headerDataSize += 2;
		}
	}

	public override List<byte> getHeaderAsSerial()
	{
		List<byte> headerAsSerial = base.getHeaderAsSerial();
		headerAsSerial.Add((byte)((uint)FrameType | (uint)(byte)(Convert.ToByte(StayAwake) << 6) | (byte)(Convert.ToByte(BiDi) << 7)));
		headerAsSerial.Add(SequenceNumber);
		return headerAsSerial;
	}

	public override bool Equals(object obj)
	{
		if (obj is SIPcosHeader)
		{
			SIPcosHeader sIPcosHeader = (SIPcosHeader)obj;
			return getHeaderAsSerial().Equals(sIPcosHeader.getHeaderAsSerial());
		}
		return false;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
