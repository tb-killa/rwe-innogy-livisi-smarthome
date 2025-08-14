using System;
using System.Collections.Generic;
using System.Globalization;
using JsonLite;

namespace SmartHome.Common.API.Entities.Entities;

[Serializable]
public class Event
{
	[JsonProperty("sequenceNumber")]
	public int SequenceNumber { get; set; }

	[JsonProperty("type")]
	public virtual string Type { get; set; }

	[JsonProperty("desc")]
	public virtual string Description { get; set; }

	[JsonProperty("namespace")]
	public string Namespace { get; set; }

	[JsonIgnore]
	public virtual DateTime? Timestamp { get; set; }

	[JsonProperty("timestamp")]
	public string TimestampStr
	{
		get
		{
			if (!Timestamp.HasValue)
			{
				return null;
			}
			return Timestamp.Value.ToUniversalTime().ToString("O", CultureInfo.InvariantCulture);
		}
		set
		{
			Timestamp = (string.IsNullOrEmpty(value) ? ((DateTime?)null) : new DateTime?(DateTime.Parse(value, CultureInfo.InvariantCulture)));
		}
	}

	[JsonProperty("source")]
	public virtual string Link { get; set; }

	[JsonProperty("properties")]
	public List<Property> Properties { get; set; }

	[JsonProperty("data")]
	public object Data { get; set; }

	public override string ToString()
	{
		return string.Format("Type: {0}, Timestamp: {1}, Link: {2}", Type, Timestamp.HasValue ? Timestamp.Value.ToString("O") : string.Empty, Link);
	}

	public Event()
	{
		SequenceNumber = -1;
	}
}
