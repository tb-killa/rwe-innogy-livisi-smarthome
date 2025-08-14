using System;
using System.Globalization;
using JsonLite;

namespace SmartHome.Common.API.Entities.Entities;

[Serializable]
public class Property
{
	[JsonProperty("name")]
	public string Name { get; set; }

	[JsonProperty("value")]
	public object Value { get; set; }

	[JsonIgnore]
	public DateTime? LastChanged { get; set; }

	[JsonProperty("lastChanged")]
	public string LastChangedStr
	{
		get
		{
			if (!LastChanged.HasValue)
			{
				return null;
			}
			return LastChanged.Value.ToUniversalTime().ToString("O", CultureInfo.InvariantCulture);
		}
		set
		{
			LastChanged = (string.IsNullOrEmpty(value) ? ((DateTime?)null) : new DateTime?(DateTime.Parse(value, CultureInfo.InvariantCulture)));
		}
	}

	public override string ToString()
	{
		return $"[Name: {Name}, Value: {Value}]";
	}
}
