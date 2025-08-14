using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.Messages;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.ModelTransformationService.Extensions;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.ActionConverters.DeviceSpecific.SHC;

internal class ShcMarkMessageAsReadActionConverter : IActionConverter
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(ShcMarkMessageAsReadActionConverter));

	public BaseRequest ToBaseRequest(SmartHome.Common.API.Entities.Entities.Action anAction, IConversionContext context)
	{
		logger.DebugEnterMethod("ToBaseRequest");
		Guid guid = anAction.Target.GetGuid();
		bool constantParameterValue = anAction.Data.GetConstantParameterValue<bool>("IsRead");
		logger.DebugExitMethod("ToBaseRequest");
		ChangeMessageStateRequest changeMessageStateRequest = new ChangeMessageStateRequest();
		changeMessageStateRequest.MessageId = guid;
		changeMessageStateRequest.NewState = (constantParameterValue ? MessageState.Read : MessageState.New);
		changeMessageStateRequest.RequestId = Guid.NewGuid();
		return changeMessageStateRequest;
	}

	public SmartHome.Common.API.Entities.Entities.ActionResponse FromBaseResponse(SmartHome.Common.API.Entities.Entities.Action anAction, BaseResponse aResponse, IConversionContext context)
	{
		return aResponse.GetActionResponse<AcknowledgeResponse>(anAction);
	}
}
