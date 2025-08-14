using System;

namespace SmartHome.Common.API.Entities.Entities;

[Serializable]
public class UserDataItemIdentifier
{
	public string key { get; set; }

	public string partition { get; set; }
}
