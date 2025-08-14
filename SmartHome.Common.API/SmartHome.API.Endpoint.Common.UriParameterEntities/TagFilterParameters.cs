namespace SmartHome.API.Endpoint.Common.UriParameterEntities;

public class TagFilterParameters
{
	public string TKey { get; set; }

	public string TVal { get; set; }

	public override string ToString()
	{
		return string.Format("[TKey: {0}, TVal: {1}]", TKey ?? "null", TVal ?? "null");
	}
}
