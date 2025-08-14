using System;

namespace SmartHome.Common.API.Entities.ErrorHandling.Exceptions;

[Serializable]
public class NoChangePerformedException : Exception
{
	public NoChangePerformedException()
	{
	}

	public NoChangePerformedException(string message)
		: base(message)
	{
	}
}
