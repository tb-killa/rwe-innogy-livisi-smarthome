using System.Collections.Generic;

namespace SerialAPI.BidCoSFrames;

internal class BIDCOSSysinfoFrame : BIDCOSMessage
{
	private int itemReference;

	private byte m_version;

	private byte[] m_deviceTypeNumber;

	private byte[] m_serialNumber;

	private byte[] m_typeCode;

	private byte m_keyA;

	private byte m_keyB;

	private BIDCOSDeviceType deviceType;

	public byte SoftwareVersion
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

	public byte[] DeviceTypeNumber
	{
		get
		{
			return m_deviceTypeNumber;
		}
		set
		{
			m_deviceTypeNumber = value;
		}
	}

	public BIDCOSDeviceType DeviceType => deviceType;

	public byte[] SerialNumber
	{
		get
		{
			return m_serialNumber;
		}
		set
		{
			m_serialNumber = value;
		}
	}

	public byte[] TypeCode
	{
		get
		{
			return m_typeCode;
		}
		set
		{
			m_typeCode = value;
		}
	}

	public byte KeyA
	{
		get
		{
			return m_keyA;
		}
		set
		{
			m_keyA = value;
		}
	}

	public byte KeyB
	{
		get
		{
			return m_keyB;
		}
		set
		{
			m_keyB = value;
		}
	}

	public BIDCOSSysinfoFrame(BIDCOSHeader header)
		: base(header)
	{
		m_deviceTypeNumber = new byte[2];
		m_serialNumber = new byte[10];
		m_typeCode = new byte[2];
	}

	public bool Parse(List<byte> message)
	{
		if (message.Count >= 17 && message[1] == 0 && (message[2] == 66 || message[2] == 170 || message[2] == 249))
		{
			m_data = message;
			m_version = message[0];
			m_deviceTypeNumber[0] = message[1];
			m_deviceTypeNumber[1] = message[2];
			switch (m_deviceTypeNumber[1])
			{
			case 66:
				deviceType = BIDCOSDeviceType.Eq3BasicSmokeDetector;
				itemReference = 91419;
				break;
			case 170:
				deviceType = BIDCOSDeviceType.Eq3EncryptedSmokeDetector;
				itemReference = 91419;
				break;
			case 249:
				deviceType = BIDCOSDeviceType.Eq3EncryptedSiren;
				itemReference = 97510;
				break;
			default:
				deviceType = BIDCOSDeviceType.Unknown;
				break;
			}
			m_serialNumber[0] = message[3];
			m_serialNumber[1] = message[4];
			m_serialNumber[2] = message[5];
			m_serialNumber[3] = message[6];
			m_serialNumber[4] = message[7];
			m_serialNumber[5] = message[8];
			m_serialNumber[6] = message[9];
			m_serialNumber[7] = message[10];
			m_serialNumber[8] = message[11];
			m_serialNumber[9] = message[12];
			m_typeCode[0] = message[13];
			m_typeCode[1] = message[14];
			m_keyA = message[15];
			m_keyB = message[16];
			return true;
		}
		return false;
	}

	public byte[] generateSGTIN()
	{
		int num = itemReference;
		int num2 = (m_serialNumber[1] << 8) | m_serialNumber[2];
		int num3 = 0;
		for (int i = 0; i < 7; i++)
		{
			num3 *= 10;
			num3 += m_serialNumber[3 + i] - 48;
		}
		switch (num2)
		{
		case 21079:
			num2 = 0;
			break;
		case 22341:
			num2 = 1;
			break;
		case 17745:
			num2 = 2;
			break;
		}
		byte[] array = new byte[12]
		{
			48, 20, 247, 72, 156, 0, 0, 0, 0, 0,
			0, 0
		};
		array[4] |= (byte)((num >> 18) & 3);
		array[5] = (byte)((num >> 10) & 0xFF);
		array[6] = (byte)((num >> 2) & 0xFF);
		array[7] = (byte)((num << 6) & 0xC0);
		array[7] |= (byte)(num2 >> 3);
		array[8] = (byte)((num2 << 5) & 0xE0);
		array[8] |= (byte)(m_serialNumber[0] - 65);
		array[9] = (byte)((num3 >> 16) & 0xFF);
		array[10] = (byte)((num3 >> 8) & 0xFF);
		array[11] = (byte)(num3 & 0xFF);
		return array;
	}

	public override void GenerateSIPCOSMessage(ref CORESTACKHeader header, ref List<byte> data)
	{
		base.GenerateSIPCOSMessage(ref header, ref data);
		data.Clear();
		byte b = 0;
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
		data.Add(0);
		data.Add(64);
		data.AddRange(generateSGTIN());
		data.AddRange(new byte[2] { 0, 1 });
		data.AddRange(GetEq3DeviceType());
		data.AddRange(new byte[2] { 4, 4 });
		data.Add(m_version);
		data.Add(0);
		data.Add(1);
		data.Add(4);
		data.Add(8);
		List<byte> obj = data;
		byte[] collection = new byte[4];
		obj.AddRange(collection);
		List<byte> obj2 = data;
		byte[] collection2 = new byte[4];
		obj2.AddRange(collection2);
	}

	private byte[] GetEq3DeviceType()
	{
		byte b = 18;
		switch (DeviceType)
		{
		case BIDCOSDeviceType.Eq3BasicSmokeDetector:
		case BIDCOSDeviceType.Eq3EncryptedSmokeDetector:
			b = 18;
			break;
		case BIDCOSDeviceType.Eq3EncryptedSiren:
			b = 22;
			break;
		}
		return new byte[4] { 0, 0, 0, b };
	}
}
