using System;

namespace SmartHome.Common.API.Entities.ErrorHandling.Exceptions;

[Serializable]
public class DeviceCommunicationException : Exception
{
	public DeviceCommunicationException()
	{
	}

	public DeviceCommunicationException(string message)
		: base(message)
	{
	}
}
