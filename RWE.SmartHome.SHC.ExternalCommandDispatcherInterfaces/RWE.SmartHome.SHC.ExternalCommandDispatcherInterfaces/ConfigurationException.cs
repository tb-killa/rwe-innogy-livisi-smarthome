using System;

namespace RWE.SmartHome.SHC.ExternalCommandDispatcherInterfaces;

public class ConfigurationException : Exception
{
	public ConfigurationException()
	{
	}

	public ConfigurationException(string message)
		: base(message)
	{
	}

	public ConfigurationException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
