using SmartHome.SHC.API.PropertyDefinition;

namespace SmartHome.SHC.API.Configuration;

public class TriggerCondition
{
	public ComparisonOperator Operator { get; private set; }

	public Property Threshold { get; private set; }

	public TriggerCondition(ComparisonOperator comparisonOperator, Property threshold)
	{
		Operator = comparisonOperator;
		Threshold = threshold;
	}
}
