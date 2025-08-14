using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.ShcSettings;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.ShcSettings;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses.ShcSettings;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.ModelTransformationService.Extensions;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.ActionConverters.DeviceSpecific.SHC;

internal class ShcSetTimeZoneActionConverter : IActionConverter
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(ShcSetTimeZoneActionConverter));

	public BaseRequest ToBaseRequest(SmartHome.Common.API.Entities.Entities.Action anAction, IConversionContext context)
	{
		logger.DebugEnterMethod("ToBaseRequest");
		if (anAction == null || anAction.Data == null)
		{
			logger.LogAndThrow<ArgumentException>("Missing parameters for TimeZone");
		}
		Parameter parameter = anAction.Data.FirstOrDefault((Parameter p) => p.Name.Equals("TimeZone", StringComparison.InvariantCultureIgnoreCase));
		if (parameter == null || parameter.Constant == null || parameter.Type != "Constant")
		{
			logger.LogAndThrow<ArgumentException>(string.Format("{0} parameter with {1} value mandatory for this action.", "TimeZone", "Constant"));
		}
		ShcTimeZone value;
		try
		{
			value = parameter.Constant.Value.ToString().ParseAsShcTimeZone(ignoreCase: true);
		}
		catch (Exception exception)
		{
			string message = string.Format("Invalid value: \"{0}\" for parameter: \"{1}\"", parameter.Constant.Value, "TimeZone");
			logger.Warn(message, exception);
			throw new ArgumentException(message);
		}
		logger.DebugExitMethod("ToBaseRequest");
		SetShcInformationRequest setShcInformationRequest = new SetShcInformationRequest();
		setShcInformationRequest.Information = new ShcInformation
		{
			TimeZone = value
		};
		return setShcInformationRequest;
	}

	public SmartHome.Common.API.Entities.Entities.ActionResponse FromBaseResponse(SmartHome.Common.API.Entities.Entities.Action anAction, BaseResponse aResponse, IConversionContext context)
	{
		List<Property> list = new List<Property>();
		SmartHome.Common.API.Entities.Entities.ActionResponse actionResponse = aResponse.GetActionResponse<ShcInformationResponse>(anAction);
		if (aResponse is ShcInformationResponse { Information: not null } shcInformationResponse)
		{
			list.Add(new Property
			{
				Name = "timezone",
				Value = (shcInformationResponse.Information.TimeZone.HasValue ? shcInformationResponse.Information.TimeZone.Value.GetStringValue() : null)
			});
			list.Add(new Property
			{
				Name = "currentShcUtcOffset",
				Value = shcInformationResponse.Information.CurrentShcUtcOffset
			});
			actionResponse.Data = list;
		}
		return actionResponse;
	}
}
