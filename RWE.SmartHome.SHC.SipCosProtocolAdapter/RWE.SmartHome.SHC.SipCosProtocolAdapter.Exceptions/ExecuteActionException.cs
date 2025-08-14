using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter.Exceptions;

internal class ExecuteActionException : Exception
{
	public List<Property> Properties { get; set; }

	public ExecuteActionException(string message)
		: base(message)
	{
		Properties = new List<Property>
		{
			new StringProperty("FailureCode", message)
		};
	}

	public ExecuteActionException()
	{
		Properties = new List<Property>();
	}
}
