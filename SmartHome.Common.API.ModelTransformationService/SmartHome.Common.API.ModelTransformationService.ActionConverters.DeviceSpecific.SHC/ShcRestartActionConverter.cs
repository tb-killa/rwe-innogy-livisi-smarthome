using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.ModelTransformationService.Extensions;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.ActionConverters.DeviceSpecific.SHC;

internal class ShcRestartActionConverter : IActionConverter
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(ShcRestartActionConverter));

	public BaseRequest ToBaseRequest(Action anAction, IConversionContext context)
	{
		logger.DebugEnterMethod("ToBaseRequest");
		string constantParameterValue = anAction.Data.GetConstantParameterValue<string>("Reason");
		logger.DebugExitMethod("ToBaseRequest");
		SHCRestartRequest sHCRestartRequest = new SHCRestartRequest();
		sHCRestartRequest.Reason = constantParameterValue;
		return sHCRestartRequest;
	}

	public SmartHome.Common.API.Entities.Entities.ActionResponse FromBaseResponse(Action anAction, BaseResponse aResponse, IConversionContext context)
	{
		return aResponse.GetActionResponse<AcknowledgeResponse>(anAction);
	}
}
