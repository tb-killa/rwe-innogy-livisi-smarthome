using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.Calibration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.ModelTransformationService.Extensions;

namespace SmartHome.Common.API.ModelTransformationService.ActionConverters.CapabilitySpecific.RollerShutterActuator;

public class ResetCalibrationActionConverter : IActionConverter
{
	public BaseRequest ToBaseRequest(SmartHome.Common.API.Entities.Entities.Action anAction, IConversionContext context)
	{
		Guid guid = anAction.Target.GetGuid();
		ResetCalibrationRequest resetCalibrationRequest = new ResetCalibrationRequest();
		resetCalibrationRequest.LogicalDeviceId = guid;
		return resetCalibrationRequest;
	}

	public SmartHome.Common.API.Entities.Entities.ActionResponse FromBaseResponse(SmartHome.Common.API.Entities.Entities.Action anAction, BaseResponse aResponse, IConversionContext context)
	{
		return aResponse.GetActionResponse<AcknowledgeResponse>(anAction);
	}
}
