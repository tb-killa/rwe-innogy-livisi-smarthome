using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;

public class ErrorResponse : BaseResponse
{
	public ErrorResponseType ErrorType { get; set; }

	public Property[] Data { get; set; }

	public ErrorResponse()
	{
	}

	public ErrorResponse(Guid requestId, ErrorResponseType errorType, params Property[] data)
	{
		base.CorrespondingRequestId = requestId;
		ErrorType = errorType;
		Data = data;
	}
}
