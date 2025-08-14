using System.Collections.Generic;

namespace SerialAPI.BidCoSFrames;

internal class BIDCOSMessage
{
	protected BIDCOSHeader m_header;

	protected List<byte> m_data;

	public BIDCOSHeader Header
	{
		get
		{
			return m_header;
		}
		set
		{
			m_header = value;
		}
	}

	public List<byte> Data
	{
		get
		{
			return m_data;
		}
		set
		{
			m_data = value;
		}
	}

	public BIDCOSMessage(BIDCOSHeader header)
	{
		m_header = header;
		m_data = new List<byte>();
	}

	public virtual void GenerateSIPCOSMessage(ref CORESTACKHeader header, ref List<byte> data)
	{
		header.CorestackFrameType = CorestackFrameType.SIPCOS_APPLICATION;
		header.AddressExtensionType = AddressExtensionType.SINGLE_HOP;
		header.MacSecurity = false;
		header.Fragmentation = false;
		header.QoS = false;
		header.LocalHopLimit = 1;
		header.SyncWord = Core.BIDCosDefaultSync;
		header.MacDestination = m_header.Receiver;
		header.MacSource = m_header.Sender;
		header.IpDestination = m_header.Receiver;
		header.IpSource = m_header.Sender;
	}
}
