using System;
using System.Collections.Generic;
using JsonLite;
using SmartHome.Common.API.Entities.Entities;

namespace SmartHome.Common.API.Entities.Data;

public class Image
{
	public string Id { get; set; }

	public DateTime Timestamp { get; set; }

	public List<Property> Metadata { get; set; }

	public string ImageUrl { get; set; }

	[JsonIgnore]
	public bool IsLiveImage { get; set; }
}
