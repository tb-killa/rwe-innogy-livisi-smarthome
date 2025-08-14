using System;

namespace RWE.SmartHome.SHC.Core.Authentication.Entities;

public class Role
{
	public const string ROLE_ID_COLUMN = "Id";

	public const string ROLENAME_COLUMN = "Name";

	public const string TABLE = "Roles";

	public const string PRIMARY_KEY = "Roles_PKey";

	public Guid Id { get; set; }

	public string Name { get; set; }

	public Role()
	{
		Id = Guid.NewGuid();
	}
}
