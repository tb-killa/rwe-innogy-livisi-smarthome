using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses.Control;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.ModelTransformationService.Extensions;
using SmartHome.Common.API.ModelTransformationService.InteractionConverters;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.ActionConverters.CapabilitySpecific;

internal class SignalLocationActionConverter : IActionConverter
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(SignalLocationActionConverter));

	public BaseRequest ToBaseRequest(SmartHome.Common.API.Entities.Entities.Action anAction, IConversionContext context)
	{
		logger.DebugEnterMethod("ToBaseRequest");
		try
		{
			Guid guid = anAction.Target.GetGuid();
			List<SmartHome.Common.API.Entities.Entities.Parameter> data = anAction.Data;
			if (data == null || data.Count == 0 || data.Any((SmartHome.Common.API.Entities.Entities.Parameter p) => p.Type != "Constant"))
			{
				logger.LogAndThrow<ArgumentException>(string.Format("Parameters with {0} type only mandatory for this action.", "Constant"));
				return null;
			}
			ParameterConverter parameterConverter = new ParameterConverter();
			ActionRequest actionRequest = new ActionRequest();
			actionRequest.ActionDescription = new ActionDescription
			{
				ActionType = anAction.Type,
				Data = data.ConvertAll<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Parameter>(parameterConverter.ToSmartHomeParameter),
				Target = new LinkBinding
				{
					LinkType = RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType.Home,
					EntityId = guid.ToString("N")
				}
			};
			return actionRequest;
		}
		finally
		{
			logger.DebugExitMethod("ToBaseRequest");
		}
	}

	public SmartHome.Common.API.Entities.Entities.ActionResponse FromBaseResponse(SmartHome.Common.API.Entities.Entities.Action anAction, BaseResponse aResponse, IConversionContext context)
	{
		return aResponse.GetActionResponse<ControlResultResponse>(anAction);
	}
}
