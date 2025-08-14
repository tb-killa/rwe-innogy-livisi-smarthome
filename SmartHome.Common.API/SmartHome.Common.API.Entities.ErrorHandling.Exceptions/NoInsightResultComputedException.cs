using System;

namespace SmartHome.Common.API.Entities.ErrorHandling.Exceptions;

[Serializable]
public class NoInsightResultComputedException : Exception
{
	public NoInsightResultComputedException()
	{
	}

	public NoInsightResultComputedException(string message)
		: base(message)
	{
	}
}
