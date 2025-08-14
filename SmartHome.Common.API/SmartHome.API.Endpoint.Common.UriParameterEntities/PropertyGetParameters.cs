namespace SmartHome.API.Endpoint.Common.UriParameterEntities;

public class PropertyGetParameters
{
	public string Name { get; set; }

	public override string ToString()
	{
		return $"Name: {Name}";
	}
}
