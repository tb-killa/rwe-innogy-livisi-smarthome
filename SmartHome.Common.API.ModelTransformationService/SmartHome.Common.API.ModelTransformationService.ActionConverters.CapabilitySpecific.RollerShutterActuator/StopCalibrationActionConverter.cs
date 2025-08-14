using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;
using SmartHome.Common.API.Entities.Entities;

namespace SmartHome.Common.API.ModelTransformationService.ActionConverters.CapabilitySpecific.RollerShutterActuator;

public class StopCalibrationActionConverter : BaseStartStopCalibrationHandler, IActionConverter
{
	public BaseRequest ToBaseRequest(Action anAction, IConversionContext context)
	{
		return StopCalibrationRequest(anAction, context);
	}

	public SmartHome.Common.API.Entities.Entities.ActionResponse FromBaseResponse(Action anAction, BaseResponse aResponse, IConversionContext context)
	{
		return GetCalibrationResponse(anAction, aResponse);
	}
}
