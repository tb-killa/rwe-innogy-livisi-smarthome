using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.ShcType;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.Entities.Serializers;
using SmartHome.Common.API.ModelTransformationService.Extensions;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.ActionConverters.DeviceSpecific.SHC;

internal class ShcGetShcTypeInfoActionConverter : IActionConverter
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(ShcGetShcTypeInfoActionConverter));

	public BaseRequest ToBaseRequest(Action anAction, IConversionContext context)
	{
		logger.DebugEnterMethod("ToBaseRequest");
		ShcRestriction constantParameterValue = anAction.Data.GetConstantParameterValue<ShcRestriction>("Restriction");
		logger.DebugExitMethod("ToBaseRequest");
		GetShcTypeRequest getShcTypeRequest = new GetShcTypeRequest();
		getShcTypeRequest.Restriction = constantParameterValue;
		return getShcTypeRequest;
	}

	public SmartHome.Common.API.Entities.Entities.ActionResponse FromBaseResponse(Action anAction, BaseResponse aResponse, IConversionContext context)
	{
		logger.DebugEnterMethod("FromBaseResponse");
		SmartHome.Common.API.Entities.Entities.ActionResponse actionResponse = aResponse.GetActionResponse<GetShcTypeResponse>(anAction);
		if (aResponse is GetShcTypeResponse { CurrentLicensingState: not null } getShcTypeResponse && getShcTypeResponse.CurrentLicensingState.Any())
		{
			actionResponse.Data = GetResponseProperties(getShcTypeResponse);
		}
		logger.DebugExitMethod("FromBaseResponse");
		return actionResponse;
	}

	private List<Property> GetResponseProperties(GetShcTypeResponse response)
	{
		List<Property> list = new List<Property>();
		if (response.CurrentLicensingState != null)
		{
			foreach (RestrictionState item in response.CurrentLicensingState)
			{
				list.Add(new Property
				{
					Name = "restrictionState",
					Value = ApiJsonSerializer.Serialize(item)
				});
			}
		}
		return list;
	}
}
