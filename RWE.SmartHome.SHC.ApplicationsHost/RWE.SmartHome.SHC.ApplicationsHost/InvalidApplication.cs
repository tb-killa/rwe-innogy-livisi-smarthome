using System;
using SmartHome.SHC.API;

namespace RWE.SmartHome.SHC.ApplicationsHost;

internal class InvalidApplication : IAddIn
{
	private string appId = string.Empty;

	public string ApplicationId => appId;

	public InvalidApplication(string appId)
	{
		this.appId = appId;
	}

	public void Activate(AddInConfiguration addInConfiguration)
	{
		throw new NotImplementedException();
	}

	public void Deactivate()
	{
		throw new NotImplementedException();
	}

	public void Initialize(AddInConfiguration addInConfiguration, ICoreServices environment)
	{
		throw new NotImplementedException();
	}

	public void Refresh(AddInConfiguration addInConfiguration)
	{
		throw new NotImplementedException();
	}

	public void Uninitialize(CleanupMode mode)
	{
		throw new NotImplementedException();
	}
}
