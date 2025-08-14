using System;

namespace SmartHome.Common.API.Entities.ErrorHandling.Exceptions;

[Serializable]
public class MetadataValidationException : Exception
{
	public MetadataValidationException()
	{
	}

	public MetadataValidationException(string message)
		: base(message)
	{
	}

	public MetadataValidationException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
