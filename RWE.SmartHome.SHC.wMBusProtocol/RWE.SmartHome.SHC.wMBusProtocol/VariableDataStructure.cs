using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RWE.SmartHome.SHC.wMBusProtocol;

public class VariableDataStructure
{
	public FixedDataHeader Header { get; set; }

	public List<VariableDataBlock> Records { get; private set; }

	public byte Mdh { get; set; }

	public List<byte> ManufacturerSpecificData { get; private set; }

	public VariableDataStructure()
		: this(new FixedDataHeader())
	{
		Records = new List<VariableDataBlock>();
		ManufacturerSpecificData = new List<byte>();
	}

	public VariableDataStructure(FixedDataHeader header)
	{
		Header = header;
		Records = new List<VariableDataBlock>();
		ManufacturerSpecificData = new List<byte>();
	}

	public VariableDataStructure(int identificationNumber, string manufacturer, DeviceTypeIdentification medium, byte accessNumber)
		: this(new FixedDataHeader(identificationNumber, manufacturer, medium, accessNumber))
	{
	}

	public static VariableDataStructure Create(byte[] buffer, ControlInformationCode ci)
	{
		VariableDataStructure variableDataStructure = new VariableDataStructure();
		int num = 0;
		num = GetHeaderLength(ci);
		variableDataStructure.Header = FixedDataHeader.Create(buffer, num);
		int num2 = num + 2;
		byte b = 0;
		if (buffer.Length > num + 2)
		{
			b = buffer[num + 2];
		}
		while ((b & 0xF) != 15 && num2 < buffer.Length)
		{
			VariableDataBlock vb = null;
			num2 = VariableDataBlock.Create(buffer, num2, out vb);
			if (num2 < buffer.Length)
			{
				b = buffer[num2];
			}
			variableDataStructure.Records.Add(vb);
		}
		if (num2 < buffer.Length)
		{
			variableDataStructure.Mdh = buffer[num2];
			num2++;
			for (int i = num2; i < buffer.Length; i++)
			{
				variableDataStructure.ManufacturerSpecificData.Add(buffer[i]);
			}
		}
		return variableDataStructure;
	}

	private static int GetHeaderLength(ControlInformationCode ci)
	{
		int result = 0;
		switch (ci)
		{
		case ControlInformationCode.VariableDataResponseNoHeader:
			result = 0;
			break;
		case ControlInformationCode.VariableDataResponseBigHeader:
			result = 12;
			break;
		case ControlInformationCode.VariableDataResponseShortHeader:
			result = 4;
			break;
		}
		return result;
	}

	public byte[] ToArray(ControlInformationCode ci)
	{
		IEnumerable<byte> first = Header.ToArray(GetHeaderLength(ci));
		first = first.Concat(new byte[2] { 47, 47 });
		foreach (VariableDataBlock record in Records)
		{
			first = first.Concat(record.ToArray());
		}
		if (Mdh != 0)
		{
			first.Concat(new byte[1] { Mdh });
		}
		if (ManufacturerSpecificData.Count > 0)
		{
			first.Concat(ManufacturerSpecificData);
		}
		return first.ToArray();
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine(Header.ToString());
		foreach (VariableDataBlock record in Records)
		{
			stringBuilder.AppendLine(record.ToString());
		}
		stringBuilder.Append($"Mdh: {Mdh}, Size: {ManufacturerSpecificData.Count}");
		return stringBuilder.ToString();
	}
}
