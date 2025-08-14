using System.Collections.Generic;
using SmartHome.Common.API.Entities.Enumerations;

namespace SmartHome.Common.API.Entities;

public class User
{
	public string AccountName { get; set; }

	public string DomainRole { get; set; }

	public List<string> DomainDevices { get; set; }

	public UserPermission Permissions { get; set; }

	public bool HasControllerAccess { get; set; }

	public string UserDevicePublicAddress { get; set; }

	public string StoreAccountName { get; set; }

	public string TenantName { get; set; }
}
