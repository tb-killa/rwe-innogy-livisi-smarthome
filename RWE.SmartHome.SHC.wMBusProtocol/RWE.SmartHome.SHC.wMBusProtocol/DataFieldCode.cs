namespace RWE.SmartHome.SHC.wMBusProtocol;

public enum DataFieldCode : byte
{
	NoData,
	Integer8Bit,
	Integer16Bit,
	Integer24Bit,
	Integer32Bit,
	Real32Bit,
	Integer48Bit,
	Integer64Bit,
	SelectionForReadout,
	Bcd2Digit,
	Bcd4Digit,
	Bcd6Digit,
	Bcd8Digit,
	VariableLength,
	Bcd12Digit,
	SpecialFunctions
}
