using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.Configuration;

public class InvalidateTokenCacheNotification : BaseNotification
{
	public string AppId { get; set; }

	public string AppVersion { get; set; }

	public List<Property> Properties { get; set; }
}
