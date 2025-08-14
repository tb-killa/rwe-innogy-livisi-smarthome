using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.ConfigurationTransformation;

public class TransformationException : Exception
{
	public ErrorEntry Error { get; private set; }

	public TransformationException(string message, ErrorEntry error)
		: base(message)
	{
		Error = error;
	}

	public TransformationException(string message, Exception innerException, ErrorEntry error)
		: base(message, innerException)
	{
		Error = error;
	}

	public TransformationException(ErrorEntry error)
	{
		Error = error;
	}
}
