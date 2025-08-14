using System;

namespace SmartHome.Common.API.Entities.ErrorHandling.Exceptions;

public class NoRelationshipException : Exception
{
	public NoRelationshipException()
	{
	}

	public NoRelationshipException(string message)
		: base(message)
	{
	}
}
