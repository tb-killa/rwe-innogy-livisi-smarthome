using System;
using System.Collections.Generic;
using System.Text;

namespace RWE.SmartHome.SHC.wMBusProtocol;

public class VariableDataBlock
{
	public DataInformationField Dif { get; set; }

	public List<DataInformationFieldExtension> Dife { get; private set; }

	public ValueInformationField Vif { get; set; }

	public List<ValueInformationFieldBase> Vife { get; private set; }

	public byte[] Data { get; set; }

	public uint StorageNumber
	{
		get
		{
			uint num = 0u;
			for (int num2 = Dife.Count - 1; num2 >= 0; num2--)
			{
				num |= Dife[num2].StorageNumber;
				num <<= 1;
			}
			return num | (uint)(Dif.LsbOfStorageNumber ? 1 : 0);
		}
	}

	public uint SubUnit
	{
		get
		{
			uint num = 0u;
			if (Dife.Count > 0)
			{
				num = (Dife[Dife.Count - 1].DeviceUnit ? 1u : 0u);
			}
			for (int num2 = Dife.Count - 2; num2 >= 0; num2--)
			{
				num <<= 1;
				num |= (byte)(Dife[num2].DeviceUnit ? 1 : 0);
			}
			return num;
		}
	}

	public uint Tariff
	{
		get
		{
			uint num = 0u;
			if (Dife.Count > 0)
			{
				num = Dife[Dife.Count - 1].Tariff;
			}
			for (int num2 = Dife.Count - 2; num2 >= 0; num2--)
			{
				num <<= 2;
				num |= Dife[num2].Tariff;
			}
			return num;
		}
	}

	public VariableDataBlock()
	{
		Dife = new List<DataInformationFieldExtension>();
		Vife = new List<ValueInformationFieldBase>();
	}

	public VariableDataBlock(DataInformationField dataInformationField, ValueInformationField valueInformationField, byte[] data)
		: this()
	{
		Dif = dataInformationField;
		Vif = valueInformationField;
		Data = data;
	}

	public VariableDataBlock(bool deviceUnit, byte tariff, byte storageNumber, FunctionFieldCode functionFieldCode, ValueInformationFieldCode valueInformationFieldCode, DataFieldCode dataFieldCode, long value)
		: this()
	{
		bool lsbOfStorageNumber = (storageNumber & 1) == 1;
		byte b = (byte)(storageNumber >> 1);
		bool flag = b != 0 || deviceUnit || tariff != 0;
		Dif = new DataInformationField(flag, lsbOfStorageNumber, functionFieldCode, dataFieldCode);
		if (flag)
		{
			Dife.Add(new DataInformationFieldExtension(extensionBit: false, deviceUnit, tariff, b));
		}
		Vif = new ValueInformationField(extensionBit: false, valueInformationFieldCode);
		Data = DataField.ToArray(value, dataFieldCode);
	}

	public byte[] ToArray()
	{
		int num = 0;
		byte[] array = new byte[2 + Dife.Count + Vife.Count + Data.Length];
		array[num] = Dif.GetValue();
		num++;
		foreach (DataInformationFieldExtension item in Dife)
		{
			array[num] = item.GetValue();
			num++;
		}
		array[num] = Vif.GetValue();
		num++;
		foreach (ValueInformationFieldBase item2 in Vife)
		{
			array[num] = item2.GetValue();
			num++;
		}
		byte[] data = Data;
		foreach (byte b in data)
		{
			array[num] = b;
			num++;
		}
		return array;
	}

	public static int Create(byte[] buffer, int start, out VariableDataBlock vb)
	{
		vb = new VariableDataBlock();
		int num = start;
		vb.Dif = new DataInformationField(buffer[num]);
		num++;
		bool extensionBit = vb.Dif.ExtensionBit;
		while (extensionBit)
		{
			DataInformationFieldExtension dataInformationFieldExtension = new DataInformationFieldExtension(buffer[num]);
			vb.Dife.Add(dataInformationFieldExtension);
			num++;
			extensionBit = dataInformationFieldExtension.ExtensionBit;
		}
		if (vb.Dif.DataField == DataFieldCode.NoData)
		{
			return num;
		}
		vb.Vif = new ValueInformationField(buffer[num]);
		num++;
		extensionBit = vb.Vif.ExtensionBit;
		while (extensionBit)
		{
			ValueInformationFieldBase valueInformationFieldBase = null;
			valueInformationFieldBase = ((vb.Vif.ValueInformation != ValueInformationFieldCode.ExtensionFD) ? ((ValueInformationFieldBase)new ValueInformationField(buffer[num])) : ((ValueInformationFieldBase)new ValueInformationFieldFD(buffer[num])));
			vb.Vife.Add(valueInformationFieldBase);
			num++;
			extensionBit = valueInformationFieldBase.ExtensionBit;
		}
		int num2 = DataField.LengthOf(vb.Dif.DataField);
		if (num2 == -1)
		{
			byte b = buffer[num];
			if (b >= 0 && b <= 191)
			{
				num2 = b;
			}
			else if (b >= 192 && b <= 201)
			{
				num2 = b - 192;
			}
			else if (b >= 208 && b <= 217)
			{
				num2 = b - 208;
			}
			else
			{
				if (b < 224 || b > 240)
				{
					if (b == 248)
					{
						throw new NotImplementedException("floating point number according to IEEE 754 not supported");
					}
					throw new NotImplementedException("reserved");
				}
				num2 = b - 224;
			}
			num2++;
		}
		vb.Data = new byte[num2];
		for (int i = 0; i < num2; i++)
		{
			vb.Data[i] = buffer[num + i];
		}
		return num + num2;
	}

	public static VariableDataBlock Create(byte[] buffer)
	{
		Create(buffer, 0, out var vb);
		return vb;
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine(Dif.ToString());
		foreach (DataInformationFieldExtension item in Dife)
		{
			stringBuilder.AppendLine(item.ToString());
		}
		stringBuilder.Append(Vif.ToString());
		foreach (ValueInformationFieldBase item2 in Vife)
		{
			stringBuilder.AppendLine();
			stringBuilder.Append(item2.ToString());
		}
		string text = string.Empty;
		if (Dif.DataField != DataFieldCode.VariableLength)
		{
			decimal? num = DataField.ToNumericalValue(Data, Dif.DataField);
			if (num.HasValue)
			{
				try
				{
					text = Vif.ValueInformation switch
					{
						ValueInformationFieldCode.TimePointDate => DateTimeConverter.ToDate(Data).ToShortDateString(), 
						ValueInformationFieldCode.TimePointDateTime => DateTimeConverter.ToDateTime(Data).ToString(), 
						_ => num.Value.ToString(), 
					};
				}
				catch (ArgumentOutOfRangeException)
				{
				}
			}
			else
			{
				text = "no value";
			}
		}
		else
		{
			text = DataField.ToStringValue(Data, Dif.DataField);
		}
		stringBuilder.AppendFormat(" Value: {0}", new object[1] { text });
		return stringBuilder.ToString();
	}
}
