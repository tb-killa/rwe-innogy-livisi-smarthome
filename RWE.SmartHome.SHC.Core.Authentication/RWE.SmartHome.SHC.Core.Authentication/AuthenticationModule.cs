using System;
using Microsoft.Practices.Mobile.ContainerModel;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.Core.Authentication;

public class AuthenticationModule : IModule
{
	public void Configure(Container container)
	{
		try
		{
			container.Register((Func<Container, IAuthenticationDataAccess>)((Container c) => new AuthenticationPersistence(container))).InitializedBy(delegate(Container c, IAuthenticationDataAccess v)
			{
				v.Initialize();
			}).ReusedWithin(ReuseScope.Container);
			container.Resolve<IAuthenticationDataAccess>();
		}
		catch (Exception ex)
		{
			Log.Error(Module.Authentication, $"Error in module loader to initially configure the module after loading. {ex.Message}, {ex.StackTrace}");
		}
		_ = string.Empty;
		try
		{
			container.Register((Func<Container, IUserManager>)((Container c) => new UserManager(container))).InitializedBy(delegate(Container c, IUserManager v)
			{
				v.Initialize();
			}).ReusedWithin(ReuseScope.Container);
			container.Register((Func<Container, ILocalUserManager>)((Container c) => new LocalUserManager(container))).InitializedBy(delegate(Container c, ILocalUserManager v)
			{
				v.Initialize();
			}).ReusedWithin(ReuseScope.Container);
			Log.Information(Module.Authentication, "Initialization of UserManager successful");
		}
		catch (Exception ex2)
		{
			Log.Error(Module.Authentication, $"Initialization of UserManager failed: {ex2.Message}");
		}
	}
}
