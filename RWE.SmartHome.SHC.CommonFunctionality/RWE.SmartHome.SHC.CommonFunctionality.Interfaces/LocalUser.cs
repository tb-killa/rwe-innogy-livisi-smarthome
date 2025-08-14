using System;

namespace RWE.SmartHome.SHC.CommonFunctionality.Interfaces;

public class LocalUser
{
	public Guid Id { get; set; }

	public string Username { get; set; }

	public string PasswordHash { get; set; }

	public string Data { get; set; }
}
