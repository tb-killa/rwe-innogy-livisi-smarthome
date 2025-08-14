using System;
using System.Reflection;

namespace RWE.SmartHome.SHC.ErrorHandling;

public class ShcException : Exception
{
	public string Assembly { get; private set; }

	public string Module { get; private set; }

	public int ErrorCode { get; private set; }

	public string[] ReplaceStrings { get; private set; }

	public ShcException(string message, Exception innerException, string module, int errorCode, string[] replaceStrings)
		: base((replaceStrings == null) ? message : string.Format(message, replaceStrings), innerException)
	{
		Assembly = System.Reflection.Assembly.GetCallingAssembly().GetName().Name;
		Module = module;
		ErrorCode = errorCode;
		ReplaceStrings = replaceStrings;
	}

	public ShcException(string message, string module, int errorCode, string[] replaceStrings)
		: this(message, null, module, errorCode, replaceStrings)
	{
	}

	public ShcException(string module, int errorCode, string[] replaceStrings)
		: this(string.Empty, null, module, errorCode, replaceStrings)
	{
	}

	public ShcException(string message, string module, int errorCode)
		: this(message, null, module, errorCode, null)
	{
	}
}
