using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;

namespace RWE.SmartHome.SHC.SHCRelayDriver.Exceptions;

public class OperationFailedException : Exception
{
	public BaseResponse Response { get; private set; }

	internal OperationFailedException(BaseResponse response)
	{
		Response = response;
	}
}
