using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses.Logging;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.ModelTransformationService.Extensions;

namespace SmartHome.Common.API.ModelTransformationService.ActionConverters.DeviceSpecific.SHC;

internal class ShcGetLoggingConfigActionConverter : IActionConverter
{
	public BaseRequest ToBaseRequest(Action anAction, IConversionContext context)
	{
		return new GetLogLevelRequest();
	}

	public SmartHome.Common.API.Entities.Entities.ActionResponse FromBaseResponse(Action anAction, BaseResponse aResponse, IConversionContext context)
	{
		List<Property> list = new List<Property>();
		SmartHome.Common.API.Entities.Entities.ActionResponse actionResponse = aResponse.GetActionResponse<LogLevelResponse>(anAction);
		if (aResponse is LogLevelResponse logLevelResponse)
		{
			list.Add(new Property
			{
				Name = "ExpiresAfterMinutes",
				Value = logLevelResponse.RemainingTime
			});
			actionResponse.Data = list;
		}
		return actionResponse;
	}
}
