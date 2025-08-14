using System;

namespace SmartHome.Common.API.Entities.ErrorHandling.Exceptions;

[Serializable]
public class PrerequisiteException : Exception
{
	public PrerequisiteException()
	{
	}

	public PrerequisiteException(string message)
		: base(message)
	{
	}
}
