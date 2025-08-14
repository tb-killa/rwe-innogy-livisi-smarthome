namespace RWE.SmartHome.SHC.WebSocketsService.Collections;

public struct NameValuePair
{
	public string Name { get; private set; }

	public string Value { get; private set; }

	public NameValuePair(string name, string value)
	{
		this = default(NameValuePair);
		Name = name;
		Value = value;
	}
}
