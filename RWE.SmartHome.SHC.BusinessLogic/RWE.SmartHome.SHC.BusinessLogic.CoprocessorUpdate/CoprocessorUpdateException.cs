using System;

namespace RWE.SmartHome.SHC.BusinessLogic.CoprocessorUpdate;

public class CoprocessorUpdateException : Exception
{
	public CoprocessorUpdateException()
	{
	}

	public CoprocessorUpdateException(string message)
		: base(message)
	{
	}

	public CoprocessorUpdateException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
