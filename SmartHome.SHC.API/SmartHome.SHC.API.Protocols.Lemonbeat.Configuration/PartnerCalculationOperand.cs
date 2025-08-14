namespace SmartHome.SHC.API.Protocols.Lemonbeat.Configuration;

public class PartnerCalculationOperand
{
	public uint? ValueId { get; set; }

	public PartnerCalculation Calculation { get; set; }

	public decimal? ConstantNumber { get; set; }

	public string ConstantString { get; set; }

	public byte[] ConstantBinary { get; set; }

	public bool? IsUpdated { get; set; }
}
