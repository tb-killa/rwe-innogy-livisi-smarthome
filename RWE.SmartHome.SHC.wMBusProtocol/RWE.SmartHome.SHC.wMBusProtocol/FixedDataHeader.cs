using System.Text;

namespace RWE.SmartHome.SHC.wMBusProtocol;

public class FixedDataHeader
{
	public byte[] ManufacturerId { get; private set; }

	public byte Version { get; set; }

	public DeviceTypeIdentification Medium { get; set; }

	public byte AccessNumber { get; set; }

	public byte Status { get; set; }

	public byte[] Signature { get; private set; }

	public int IdentNr { get; private set; }

	public string ManufacturerName
	{
		get
		{
			ushort num = (ushort)(ManufacturerId[0] << 8);
			num |= ManufacturerId[1];
			StringBuilder stringBuilder = new StringBuilder();
			byte b = (byte)(num / 1024 + 64);
			num = (ushort)(num - (b - 64) * 32 * 32);
			byte b2 = (byte)(num / 32 + 64);
			num = (ushort)(num - (b2 - 64) * 32);
			byte value = (byte)(num + 64);
			stringBuilder.Append((char)b);
			stringBuilder.Append((char)b2);
			stringBuilder.Append((char)value);
			return stringBuilder.ToString();
		}
	}

	public FixedDataHeader()
	{
		ManufacturerId = new byte[2];
		Signature = new byte[2];
	}

	public FixedDataHeader(int identificationNumber, string manufacturer, DeviceTypeIdentification medium, byte accessNumber)
	{
		IdentNr = identificationNumber;
		ManufacturerId = CreateManufacturerId(manufacturer);
		Medium = medium;
		Version = 1;
		AccessNumber = accessNumber;
		Signature = new byte[2];
	}

	public static FixedDataHeader Create(byte[] buffer, int length)
	{
		FixedDataHeader fixedDataHeader = null;
		if (length == 0)
		{
			return null;
		}
		new FixedDataHeader();
		if (length == 12)
		{
			fixedDataHeader = new FixedDataHeader();
			fixedDataHeader.IdentNr = BCDConverter.ConvertFromBcd(new byte[4]
			{
				buffer[0],
				buffer[1],
				buffer[2],
				buffer[3]
			});
			fixedDataHeader.ManufacturerId[0] = buffer[5];
			fixedDataHeader.ManufacturerId[1] = buffer[4];
			fixedDataHeader.Version = buffer[6];
			fixedDataHeader.Medium = (DeviceTypeIdentification)buffer[7];
			fixedDataHeader.AccessNumber = buffer[8];
			fixedDataHeader.Status = buffer[9];
			fixedDataHeader.Signature[0] = buffer[11];
			fixedDataHeader.Signature[1] = buffer[10];
		}
		if (length == 4)
		{
			FixedDataHeader fixedDataHeader2 = new FixedDataHeader();
			fixedDataHeader2.AccessNumber = buffer[0];
			fixedDataHeader2.Status = buffer[1];
			fixedDataHeader = fixedDataHeader2;
			fixedDataHeader.Signature[0] = buffer[3];
			fixedDataHeader.Signature[1] = buffer[2];
		}
		return fixedDataHeader;
	}

	public byte[] ToArray(int length)
	{
		switch (length)
		{
		case 0:
			return new byte[0];
		case 12:
		{
			byte[] array = new byte[12];
			byte[] array2 = DataField.ToArray(IdentNr, DataFieldCode.Bcd8Digit);
			array[0] = array2[0];
			array[1] = array2[1];
			array[2] = array2[2];
			array[3] = array2[3];
			array[4] = ManufacturerId[1];
			array[5] = ManufacturerId[0];
			array[6] = Version;
			array[7] = (byte)Medium;
			array[8] = AccessNumber;
			array[9] = Status;
			array[10] = Signature[0];
			array[11] = Signature[1];
			return array;
		}
		case 4:
			return new byte[4]
			{
				AccessNumber,
				Status,
				Signature[1],
				Signature[0]
			};
		default:
			return null;
		}
	}

	public byte[] CreateManufacturerId(string name)
	{
		byte[] array = new byte[2];
		short num = 0;
		if (name.Length > 0)
		{
			num += (short)((name.ToUpper()[0] - 64) * 32 * 32);
		}
		if (name.Length > 1)
		{
			num += (short)((name.ToUpper()[1] - 64) * 32);
		}
		if (name.Length > 2)
		{
			num += (short)(name.ToUpper()[2] - 64);
		}
		array[1] = (byte)(num << 8 >> 8);
		array[0] = (byte)(num >> 8);
		return array;
	}

	public override string ToString()
	{
		return $"{IdentNr} {ManufacturerName} {Version} {Medium} {AccessNumber} {Status} {Signature[0]} {Signature[1]}";
	}
}
