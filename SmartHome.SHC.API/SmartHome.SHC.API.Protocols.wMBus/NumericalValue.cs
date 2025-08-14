using System.IO;

namespace SmartHome.SHC.API.Protocols.wMBus;

public class NumericalValue : VariableDataEntry
{
	public decimal Value { get; set; }

	public decimal ValueInBaseUnit => Value * (decimal)base.Factor;

	public byte[] ToByteArray()
	{
		using MemoryStream memoryStream = new MemoryStream();
		using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
		{
			binaryWriter.Write(Value);
		}
		return memoryStream.ToArray();
	}

	public NumericalValue()
		: this(0m)
	{
	}

	public NumericalValue(decimal value)
		: base(ValueType.Numerical)
	{
		Value = value;
	}
}
