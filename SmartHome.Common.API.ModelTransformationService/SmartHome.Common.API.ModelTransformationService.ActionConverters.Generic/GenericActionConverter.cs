using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.Entities.ErrorHandling.Exceptions;
using SmartHome.Common.API.ModelTransformationService.Extensions;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.ActionConverters.Generic;

internal class GenericActionConverter : IActionConverter
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(GenericActionConverter));

	private readonly ActionDescriptionConverter actionDescriptionConverter = new ActionDescriptionConverter();

	public BaseRequest ToBaseRequest(Action anAction, IConversionContext context)
	{
		logger.DebugEnterMethod("ToBaseRequest");
		if (anAction.Type.Contains("/"))
		{
			logger.LogAndThrow<NotSupportedActionException>($"Invalid action type format {anAction.Type}. Should be short format.");
		}
		ActionRequest actionRequest = new ActionRequest();
		actionRequest.ActionDescription = actionDescriptionConverter.ToSmartHomeActionDescription(anAction);
		ActionRequest result = actionRequest;
		logger.DebugExitMethod("ToBaseRequest");
		return result;
	}

	public SmartHome.Common.API.Entities.Entities.ActionResponse FromBaseResponse(Action anAction, BaseResponse aResponse, IConversionContext context)
	{
		return aResponse?.GetActionResponse<RWE.SmartHome.Common.ControlNodeSHCContracts.Responses.ActionResponse>(anAction);
	}
}
