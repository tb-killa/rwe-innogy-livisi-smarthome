using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.ModelTransformationService.Extensions;

namespace SmartHome.Common.API.ModelTransformationService.ActionConverters.DeviceSpecific.SHC;

internal class ShcUploadLogActionConverter : IActionConverter
{
	public BaseRequest ToBaseRequest(Action anAction, IConversionContext context)
	{
		return new UploadLogRequest();
	}

	public SmartHome.Common.API.Entities.Entities.ActionResponse FromBaseResponse(Action anAction, BaseResponse aResponse, IConversionContext context)
	{
		return aResponse.GetActionResponse<AcknowledgeResponse>(anAction);
	}
}
