using System.Collections.Generic;
using SerialAPI;

namespace SipcosCommandHandler;

public class FirmwareInfoFrame : SIPCOSMessage
{
	private byte[] m_manufactorCode = new byte[2];

	private byte[] m_deviceType = new byte[4];

	private byte m_version;

	private byte[] m_size = new byte[4];

	public byte[] ManufactorCode
	{
		get
		{
			return m_manufactorCode;
		}
		set
		{
			m_manufactorCode = value;
		}
	}

	public byte[] DeviceType
	{
		get
		{
			return m_deviceType;
		}
		set
		{
			m_deviceType = value;
		}
	}

	public byte Version
	{
		get
		{
			return m_version;
		}
		set
		{
			m_version = value;
		}
	}

	public byte[] Size
	{
		get
		{
			return m_size;
		}
		set
		{
			m_size = value;
		}
	}

	public FirmwareInfoFrame(SIPcosHeader header)
		: base(header)
	{
	}

	public void parse(List<byte> message)
	{
		if (message.Count > 11)
		{
			m_manufactorCode[0] = message[1];
			m_manufactorCode[1] = message[2];
			m_deviceType[0] = message[3];
			m_deviceType[1] = message[4];
			m_deviceType[2] = message[5];
			m_deviceType[3] = message[6];
			m_version = message[7];
			m_size[0] = message[8];
			m_size[1] = message[9];
			m_size[2] = message[10];
			m_size[3] = message[11];
		}
	}
}
