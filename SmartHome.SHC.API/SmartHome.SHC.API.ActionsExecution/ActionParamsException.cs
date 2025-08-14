using System;

namespace SmartHome.SHC.API.ActionsExecution;

public class ActionParamsException : Exception
{
	public string ParamError { get; private set; }

	public ActionParamsException(string paramError)
	{
		ParamError = paramError;
	}
}
