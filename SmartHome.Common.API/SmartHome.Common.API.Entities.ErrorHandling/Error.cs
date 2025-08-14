using System.Collections.Generic;
using JsonLite;
using SmartHome.Common.API.Entities.Entities;

namespace SmartHome.Common.API.Entities.ErrorHandling;

public class Error
{
	[JsonProperty("errorcode")]
	public int ErrorCode { get; set; }

	[JsonProperty("description")]
	public string Description { get; set; }

	[JsonProperty("messages")]
	public List<string> Messages { get; set; }

	public List<Property> Data { get; set; }

	[JsonProperty("details")]
	public List<string> DebugInformation { get; set; }

	[JsonProperty("ref")]
	public string Reference { get; set; }
}
