using System;

namespace SmartHome.Common.API.Entities.ErrorHandling.Exceptions;

[Serializable]
public class EntityAlreadyExistsException : Exception
{
	public EntityAlreadyExistsException()
	{
	}

	public EntityAlreadyExistsException(string message)
		: base(message)
	{
	}
}
