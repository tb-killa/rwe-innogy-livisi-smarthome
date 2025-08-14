using System;

namespace RWE.SmartHome.SHC.ApplicationsHost.Exceptions;

public class AppDownloadException : Exception
{
	public AppDownloadResult Result { get; private set; }

	public AppDownloadException(AppDownloadResult result)
	{
		Result = result;
	}
}
