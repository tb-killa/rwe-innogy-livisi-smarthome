namespace SmartHome.SHC.API.Protocols.Lemonbeat.Configuration;

public class PartnerCalculation
{
	public CalculationMethod Method { get; set; }

	public PartnerCalculationOperand Left { get; set; }

	public PartnerCalculationOperand Right { get; set; }

	public PartnerCalculation()
	{
	}

	public PartnerCalculation(CalculationMethod method, PartnerCalculationOperand left, PartnerCalculationOperand right)
	{
		Right = right;
		Left = left;
		Method = method;
	}
}
