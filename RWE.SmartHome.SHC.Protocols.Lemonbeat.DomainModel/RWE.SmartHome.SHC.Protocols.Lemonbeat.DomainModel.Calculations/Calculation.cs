namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations;

public class Calculation : ConfigurationItem
{
	public CalculationMethod Method { get; set; }

	public CalculationOperand Left { get; set; }

	public CalculationOperand Right { get; set; }

	public override bool Equals(ConfigurationItem other)
	{
		if (other is Calculation calculation && base.Id == calculation.Id && Method == calculation.Method && Left.Equals(calculation.Left))
		{
			return Right.Equals(calculation.Right);
		}
		return false;
	}

	public override int GetHashCode()
	{
		int num = (base.Id.GetHashCode() * 937) ^ Method.GetHashCode();
		num = (937 * num) ^ ((Left != null) ? Left.GetHashCode() : 0);
		return (937 * num) ^ ((Right != null) ? Right.GetHashCode() : 0);
	}
}
