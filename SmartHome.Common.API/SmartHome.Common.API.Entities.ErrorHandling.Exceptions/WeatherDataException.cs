using System;

namespace SmartHome.Common.API.Entities.ErrorHandling.Exceptions;

[Serializable]
public class WeatherDataException : Exception
{
	public WeatherDataException()
	{
	}

	public WeatherDataException(string message)
		: base(message)
	{
	}
}
