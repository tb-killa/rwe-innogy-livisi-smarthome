using System;
using RWE.SmartHome.SHC.Core.Authentication.Entities;

namespace RWE.SmartHome.SHC.Core.Authentication;

public interface ILocalUserManager : IService
{
	LocalUser User { get; }

	string DefaultPassword { get; }

	bool WasButtonPressed { get; set; }

	DateTime ResetPasswordWindow { get; set; }

	void UpdateUser(string username, string password, string data);

	void ResetToDefault();

	void StartResetPasswordWindowTimer();
}
