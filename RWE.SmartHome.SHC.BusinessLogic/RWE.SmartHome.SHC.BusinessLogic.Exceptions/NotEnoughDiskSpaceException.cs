using System;

namespace RWE.SmartHome.SHC.BusinessLogic.Exceptions;

public class NotEnoughDiskSpaceException : Exception
{
	public NotEnoughDiskSpaceException(string message)
		: base(message)
	{
	}
}
