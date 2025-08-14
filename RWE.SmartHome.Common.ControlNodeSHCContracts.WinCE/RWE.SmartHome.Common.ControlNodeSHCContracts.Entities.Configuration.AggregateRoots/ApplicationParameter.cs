namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

public class ApplicationParameter
{
	public string Key { get; set; }

	public string Value { get; set; }

	public ApplicationParameter Clone()
	{
		ApplicationParameter applicationParameter = new ApplicationParameter();
		applicationParameter.Key = Key;
		applicationParameter.Value = Value;
		return applicationParameter;
	}
}
