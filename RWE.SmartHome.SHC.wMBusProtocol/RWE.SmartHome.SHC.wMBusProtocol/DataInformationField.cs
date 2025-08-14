namespace RWE.SmartHome.SHC.wMBusProtocol;

public class DataInformationField
{
	public bool ExtensionBit { get; set; }

	public bool LsbOfStorageNumber { get; set; }

	public FunctionFieldCode FunctionField { get; set; }

	public DataFieldCode DataField { get; set; }

	public DataInformationField()
	{
	}

	public DataInformationField(bool extensionBit, bool lsbOfStorageNumber, FunctionFieldCode functionFieldCode, DataFieldCode dataFieldCode)
	{
		ExtensionBit = extensionBit;
		LsbOfStorageNumber = lsbOfStorageNumber;
		FunctionField = functionFieldCode;
		DataField = dataFieldCode;
	}

	public DataInformationField(byte field)
	{
		ExtensionBit = (field & 0x80) == 128;
		LsbOfStorageNumber = (field & 0x40) == 64;
		DataField = (DataFieldCode)((byte)(field << 4) >> 4);
		FunctionField = (FunctionFieldCode)((byte)(field << 2) >> 6);
	}

	public byte GetValue()
	{
		byte b = (byte)((uint)FunctionField << 4);
		if (ExtensionBit)
		{
			b |= 0x80;
		}
		if (LsbOfStorageNumber)
		{
			b |= 0x40;
		}
		return (byte)((uint)b | (uint)DataField);
	}

	public override string ToString()
	{
		return $"Extension: {(ExtensionBit ? 1 : 0)}, LSB: {(LsbOfStorageNumber ? 1 : 0)}, Function: {FunctionField}, Data: {DataField}";
	}
}
