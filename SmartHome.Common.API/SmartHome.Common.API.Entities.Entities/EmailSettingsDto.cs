using System;
using System.Collections.Generic;
using JsonLite;

namespace SmartHome.Common.API.Entities.Entities;

[Serializable]
public class EmailSettingsDto
{
	[JsonProperty("server_address")]
	public string ServerAddress { get; set; }

	[JsonProperty("server_port_number")]
	public int? ServerPortNumber { get; set; }

	[JsonProperty("email_username")]
	public string EmailUsername { get; set; }

	[JsonProperty("email_password")]
	public string EmailPassword { get; set; }

	[JsonProperty("recipient_list")]
	public List<string> Recipients { get; set; }

	[JsonProperty("notification_device_low_battery")]
	public bool NotificationDeviceLowBattery { get; set; }

	[JsonProperty("notifications_device_unreachable")]
	public bool NotificationsDeviceUnreachable { get; set; }
}
