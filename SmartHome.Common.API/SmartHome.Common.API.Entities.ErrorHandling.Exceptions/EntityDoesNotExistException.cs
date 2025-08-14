using System;

namespace SmartHome.Common.API.Entities.ErrorHandling.Exceptions;

[Serializable]
public class EntityDoesNotExistException : Exception
{
	public EntityDoesNotExistException()
	{
	}

	public EntityDoesNotExistException(string message)
		: base(message)
	{
	}
}
