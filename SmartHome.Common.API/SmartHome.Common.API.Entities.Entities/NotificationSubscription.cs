using System;
using System.Collections.Generic;
using JsonLite;

namespace SmartHome.Common.API.Entities.Entities;

[Serializable]
public class NotificationSubscription
{
	[JsonProperty("id")]
	public string Id { get; set; }

	[JsonProperty("account")]
	public string AccountName { get; set; }

	[JsonProperty("channel")]
	public string Channel { get; set; }

	[JsonProperty("channelProperties")]
	public List<Property> ChannelProperties { get; set; }

	[JsonProperty("filters")]
	public List<Property> Filters { get; set; }

	[JsonIgnore]
	public string ClientId { get; set; }
}
