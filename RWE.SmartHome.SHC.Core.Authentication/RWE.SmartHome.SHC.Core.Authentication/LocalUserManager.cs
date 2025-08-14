using System;
using System.Threading;
using Microsoft.Practices.Mobile.ContainerModel;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.CommonFunctionality.Interfaces;
using RWE.SmartHome.SHC.Core.Authentication.Entities;

namespace RWE.SmartHome.SHC.Core.Authentication;

internal class LocalUserManager : ILocalUserManager, IService
{
	private const string DefaultUsername = "admin";

	private readonly string shcSerialNumber = SHCSerialNumber.SerialNumber();

	private Thread resetPasswordWindowTimer;

	private bool shouldExecute;

	public RWE.SmartHome.SHC.Core.Authentication.Entities.LocalUser User { get; private set; }

	public string DefaultPassword { get; private set; }

	public bool WasButtonPressed { get; set; }

	public DateTime ResetPasswordWindow { get; set; }

	public LocalUserManager(Container container)
	{
		DefaultPassword = PasswordHelper.HashPassword(shcSerialNumber);
	}

	public void UpdateUser(string username, string password, string userData)
	{
		User.Name = username;
		User.Password = password;
		User.Data = userData;
		RWE.SmartHome.SHC.CommonFunctionality.Interfaces.LocalUser localUser = new RWE.SmartHome.SHC.CommonFunctionality.Interfaces.LocalUser();
		localUser.Id = User.Id;
		localUser.Username = User.Name;
		localUser.PasswordHash = User.Password;
		localUser.Data = User.Data;
		FilePersistence.LocalUser = localUser;
	}

	public void Initialize()
	{
		User = ReadUser();
		if (User == null)
		{
			User = new RWE.SmartHome.SHC.Core.Authentication.Entities.LocalUser
			{
				Id = Guid.NewGuid()
			};
			UpdateUser("admin", DefaultPassword, string.Empty);
		}
	}

	private RWE.SmartHome.SHC.Core.Authentication.Entities.LocalUser ReadUser()
	{
		RWE.SmartHome.SHC.CommonFunctionality.Interfaces.LocalUser localUser = FilePersistence.LocalUser;
		if (localUser != null)
		{
			RWE.SmartHome.SHC.Core.Authentication.Entities.LocalUser localUser2 = new RWE.SmartHome.SHC.Core.Authentication.Entities.LocalUser();
			localUser2.Id = localUser.Id;
			localUser2.Name = localUser.Username;
			localUser2.Password = localUser.PasswordHash;
			localUser2.Data = localUser.Data;
			return localUser2;
		}
		return null;
	}

	public void Uninitialize()
	{
	}

	public void ResetToDefault()
	{
		UpdateUser("admin", DefaultPassword, string.Empty);
	}

	public void StartResetPasswordWindowTimer()
	{
		resetPasswordWindowTimer = new Thread((ThreadStart)delegate
		{
			ResetPasswordWindowTimer();
		});
		shouldExecute = true;
		resetPasswordWindowTimer.Start();
	}

	private void ResetPasswordWindowTimer()
	{
		while (shouldExecute)
		{
			if (DateTime.UtcNow > ResetPasswordWindow)
			{
				WasButtonPressed = false;
				shouldExecute = false;
			}
			Thread.Sleep(5000);
		}
	}
}
