namespace SmartHome.SHC.API.Protocols.Lemonbeat.Configuration;

public class Calculation
{
	public uint CalculationId { get; set; }

	public CalculationMethod Method { get; set; }

	public CalculationOperand Left { get; set; }

	public CalculationOperand Right { get; set; }

	public Calculation()
	{
	}

	public Calculation(uint calculationId, CalculationMethod method, CalculationOperand left, CalculationOperand right)
	{
		CalculationId = calculationId;
		Method = method;
		Left = left;
		Right = right;
	}
}
