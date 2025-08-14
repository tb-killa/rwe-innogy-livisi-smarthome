using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses.Control;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.Entities.Enumerations;
using SmartHome.Common.API.Entities.Extensions;
using SmartHome.Common.API.ModelTransformationService.GenericConverters;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.Extensions;

public static class BaseResponseExtensions
{
	private static ILogger logger = LogManager.Instance.GetLogger(typeof(BaseResponseExtensions));

	public static SmartHome.Common.API.Entities.Entities.ActionResponse GetActionResponse<T>(this BaseResponse baseResponse, Action action) where T : class
	{
		logger.DebugEnterMethod($"GetActionResponse<{typeof(T)}>");
		SmartHome.Common.API.Entities.Entities.ActionResponse actionResponse = new SmartHome.Common.API.Entities.Entities.ActionResponse();
		actionResponse.Type = action.Type;
		actionResponse.Target = action.Target;
		actionResponse.Namespace = action.Namespace;
		SmartHome.Common.API.Entities.Entities.ActionResponse actionResponse2 = actionResponse;
		if (baseResponse is ErrorResponse)
		{
			HandleErrorResponse(baseResponse, ref actionResponse2);
			return actionResponse2;
		}
		if (baseResponse is ControlResultResponse)
		{
			HandleControlResultResponse(baseResponse, ref actionResponse2);
		}
		else if (baseResponse is RWE.SmartHome.Common.ControlNodeSHCContracts.Responses.ActionResponse)
		{
			HandleShActionResponse(baseResponse, ref actionResponse2);
		}
		else
		{
			HandleResponse<T>(baseResponse, ref actionResponse2);
		}
		logger.DebugExitMethod($"GetActionResponse<{typeof(T)}>");
		return actionResponse2;
	}

	private static void HandleErrorResponse(BaseResponse baseResponse, ref SmartHome.Common.API.Entities.Entities.ActionResponse actionResponse)
	{
		logger.DebugEnterMethod("HandleErrorResponse");
		actionResponse.ResultCode = ActionResultCode.UnexpectedFailure.ToString();
		ErrorResponse errorResponse = baseResponse as ErrorResponse;
		if (errorResponse.Data != null && errorResponse.Data.Any())
		{
			List<SmartHome.Common.API.Entities.Entities.Property> list = errorResponse.Data.Select((RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property p) => new SmartHome.Common.API.Entities.Entities.Property
			{
				Name = p.Name,
				Value = p.GetValueAsString(),
				LastChanged = p.UpdateTimestamp
			}).ToList();
			if (!list.Any((SmartHome.Common.API.Entities.Entities.Property p) => p.Name.EqualsIgnoreCase("error")))
			{
				list.Add(new SmartHome.Common.API.Entities.Entities.Property
				{
					Name = "error",
					Value = errorResponse.ErrorType.ToString()
				});
			}
			actionResponse.Data = list;
		}
		logger.DebugExitMethod("HandleErrorResponse");
	}

	private static void HandleControlResultResponse(BaseResponse baseResponse, ref SmartHome.Common.API.Entities.Entities.ActionResponse actionResponse)
	{
		ControlResultResponse controlResultResponse = baseResponse as ControlResultResponse;
		actionResponse.ResultCode = ((controlResultResponse.Result == ControlResult.Ok) ? ActionResultCode.Success.ToString() : ActionResultCode.UnexpectedFailure.ToString());
	}

	private static void HandleShActionResponse(BaseResponse baseResponse, ref SmartHome.Common.API.Entities.Entities.ActionResponse actionResponse)
	{
		RWE.SmartHome.Common.ControlNodeSHCContracts.Responses.ActionResponse actionResponse2 = baseResponse as RWE.SmartHome.Common.ControlNodeSHCContracts.Responses.ActionResponse;
		actionResponse.ResultCode = ActionResultCode.Success.ToString();
		if (actionResponse2.Payload != null)
		{
			actionResponse.Data = actionResponse2.Payload.ConvertAll<SmartHome.Common.API.Entities.Entities.Property>(PropertyConverter.ToApiProperty);
		}
	}

	private static void HandleResponse<T>(BaseResponse baseResponse, ref SmartHome.Common.API.Entities.Entities.ActionResponse actionResponse) where T : class
	{
		T val = baseResponse as T;
		actionResponse.ResultCode = ((val == null) ? ActionResultCode.UnexpectedFailure.ToString() : ActionResultCode.Success.ToString());
	}
}
