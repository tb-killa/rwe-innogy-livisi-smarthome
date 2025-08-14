using System;
using System.Collections.Generic;
using System.Data.SqlTypes;

namespace RWE.SmartHome.SHC.Core.Authentication.Entities;

public class User
{
	public const string USER_ID_COLUMN = "Id";

	public const string USERNAME_COLUMN = "Name";

	public const string PASSWORD_COLUMN = "Password";

	public const string CREATEDATE_COLUMN = "CreateDate";

	public const string LASTLOGINDATE_COLUMN = "LastLoginDate";

	public const string TABLE = "Users";

	public const string PRIMARY_KEY = "Users_PKey";

	public Guid Id { get; set; }

	public string Name { get; set; }

	public string Password { get; set; }

	public DateTime CreateDate { get; set; }

	public DateTime LastLoginDate { get; set; }

	public List<Role> Roles { get; set; }

	public User()
	{
		Id = Guid.NewGuid();
		CreateDate = (DateTime)SqlDateTime.MinValue;
		LastLoginDate = (DateTime)SqlDateTime.MinValue;
		Roles = new List<Role>();
	}
}
