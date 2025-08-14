namespace SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.ValueDescriptions;

public class StringFormat
{
	public uint MaximumLength { get; set; }

	public string[] ValidValues { get; set; }

	public StringFormat()
	{
	}

	public StringFormat(uint maximumLength, string[] validValues)
	{
		MaximumLength = maximumLength;
		ValidValues = validValues;
	}

	public StringFormat(byte maximumLength)
	{
		MaximumLength = maximumLength;
	}
}
