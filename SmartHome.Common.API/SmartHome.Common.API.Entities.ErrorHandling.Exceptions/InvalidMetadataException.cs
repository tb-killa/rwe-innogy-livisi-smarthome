using System;

namespace SmartHome.Common.API.Entities.ErrorHandling.Exceptions;

[Serializable]
public class InvalidMetadataException : Exception
{
	public InvalidMetadataException()
	{
	}

	public InvalidMetadataException(string message)
		: base(message)
	{
	}

	public InvalidMetadataException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
