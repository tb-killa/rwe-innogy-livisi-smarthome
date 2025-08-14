using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.ModelTransformationService.Extensions;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.ActionConverters.DeviceSpecific.SHC;

internal class ShcSetLoggingConfigActionConverter : IActionConverter
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(ShcSetLoggingConfigActionConverter));

	public BaseRequest ToBaseRequest(Action anAction, IConversionContext context)
	{
		logger.DebugEnterMethod("FromNotification");
		AdjustLogLevelRequest adjustLogLevelRequest = new AdjustLogLevelRequest();
		adjustLogLevelRequest.ExpireAfterMinutes = GetExpiresAfterMinutes(anAction.Data);
		adjustLogLevelRequest.Reason = GetReason(anAction.Data);
		AdjustLogLevelRequest result = adjustLogLevelRequest;
		logger.DebugExitMethod("FromNotification");
		return result;
	}

	public SmartHome.Common.API.Entities.Entities.ActionResponse FromBaseResponse(Action anAction, BaseResponse aResponse, IConversionContext context)
	{
		return aResponse.GetActionResponse<AcknowledgeResponse>(anAction);
	}

	private int GetExpiresAfterMinutes(List<Parameter> parameters)
	{
		return parameters.GetConstantParameterValue<int>("ExpiresAfterMinutes");
	}

	private string GetReason(List<Parameter> parameters)
	{
		return parameters.GetConstantParameterValue<string>("Reason");
	}
}
