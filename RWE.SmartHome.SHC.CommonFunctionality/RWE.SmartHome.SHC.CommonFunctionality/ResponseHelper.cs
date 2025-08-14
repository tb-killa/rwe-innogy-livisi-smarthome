using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.API.Serializer;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;

namespace RWE.SmartHome.SHC.CommonFunctionality;

public static class ResponseHelper
{
	private static BaseClassSerializer<BaseResponse> serializer = new BaseClassSerializer<BaseResponse>();

	public static string SerializeErrorResponse(Guid requestId, ErrorResponseType errorType, params Property[] data)
	{
		ErrorResponse obj = new ErrorResponse(requestId, errorType, data);
		return serializer.Serialize(obj);
	}

	public static string SerializeResponse(BaseResponse response)
	{
		return serializer.Serialize(response);
	}
}
