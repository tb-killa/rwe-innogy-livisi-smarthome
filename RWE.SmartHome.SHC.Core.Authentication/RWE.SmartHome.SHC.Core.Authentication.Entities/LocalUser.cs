using System;

namespace RWE.SmartHome.SHC.Core.Authentication.Entities;

public class LocalUser
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public string Password { get; set; }

	public string Data { get; set; }
}
