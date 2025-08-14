using System;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using SmartHome.SHC.API.Logging;

namespace RWE.SmartHome.SHC.ApplicationsHost.Logging;

public class ApplicationLogger : ILogger
{
	private readonly string applicationId;

	public ApplicationLogger(string appId)
	{
		applicationId = appId;
	}

	public void Debug(string message, params object[] args)
	{
		Log.DebugFormat(Module.CustomApp, applicationId, isPersisted: true, message, args);
	}

	public void Error(string message, params object[] args)
	{
		Log.ErrorFormat(Module.CustomApp, applicationId, isPersisted: true, message, args);
	}

	public void Information(string message, params object[] args)
	{
		Log.InformationFormat(Module.CustomApp, applicationId, isPersisted: true, message, args);
	}

	public void Warning(string message, params object[] args)
	{
		Log.WarningFormat(Module.CustomApp, applicationId, isPersisted: true, message, args);
	}

	public void Exception(Exception exception, string message, params object[] args)
	{
		Log.Exception(Module.CustomApp, exception, message, args);
	}
}
