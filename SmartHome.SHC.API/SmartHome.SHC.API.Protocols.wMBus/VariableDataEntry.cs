using System.Collections.Generic;

namespace SmartHome.SHC.API.Protocols.wMBus;

public abstract class VariableDataEntry
{
	public ValueType ValueType { get; private set; }

	public uint StorageNumber { get; set; }

	public uint SubUnit { get; set; }

	public uint Tariff { get; set; }

	public FunctionCode FunctionCode { get; set; }

	public double Factor { get; set; }

	public List<ValueInformationCode> ValueInformation { get; set; }

	public bool ExtensionBit { get; set; }

	public bool HasData { get; set; }

	protected VariableDataEntry(ValueType valueType)
	{
		Factor = 1.0;
		ValueType = valueType;
		ValueInformation = new List<ValueInformationCode>();
		HasData = true;
	}

	public bool Equals(VariableDataEntry other)
	{
		if (object.ReferenceEquals(null, other))
		{
			return false;
		}
		if (object.ReferenceEquals(this, other))
		{
			return true;
		}
		if (ValueInformation.Count != other.ValueInformation.Count)
		{
			return false;
		}
		for (int i = 0; i < ValueInformation.Count; i++)
		{
			if (!ValueInformation[i].Equals(other.ValueInformation[i]))
			{
				return false;
			}
		}
		return object.Equals(other.ValueType, ValueType) && other.StorageNumber == StorageNumber && other.SubUnit == SubUnit && object.Equals(other.FunctionCode, FunctionCode) && other.ExtensionBit == ExtensionBit && other.HasData == HasData;
	}

	public override bool Equals(object obj)
	{
		if (object.ReferenceEquals(null, obj))
		{
			return false;
		}
		if (object.ReferenceEquals(this, obj))
		{
			return true;
		}
		return Equals(obj as VariableDataEntry);
	}

	public override int GetHashCode()
	{
		int hashCode = ValueType.GetHashCode();
		hashCode = (hashCode * 397) ^ StorageNumber.GetHashCode();
		hashCode = (hashCode * 397) ^ SubUnit.GetHashCode();
		hashCode = (hashCode * 397) ^ FunctionCode.GetHashCode();
		hashCode = (hashCode * 397) ^ ((ValueInformation != null) ? ValueInformation.GetHashCode() : 0);
		hashCode = (hashCode << 1) ^ (ExtensionBit ? 1 : 0);
		return (hashCode << 1) ^ (HasData ? 1 : 0);
	}
}
