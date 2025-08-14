using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Actuators;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.Entities.Enumerations;
using SmartHome.Common.API.Entities.ErrorHandling.Exceptions;
using SmartHome.Common.API.ModelTransformationService.Extensions;

namespace SmartHome.Common.API.ModelTransformationService.ActionConverters.CapabilitySpecific.RollerShutterActuator;

public class BaseStartStopCalibrationHandler
{
	protected BaseRequest StartCalibrationRequest(SmartHome.Common.API.Entities.Entities.Action anAction, IConversionContext context)
	{
		return SetCalibrationModeRequest(anAction, context, isCalibrating: true);
	}

	protected BaseRequest StopCalibrationRequest(SmartHome.Common.API.Entities.Entities.Action anAction, IConversionContext context)
	{
		return SetCalibrationModeRequest(anAction, context, isCalibrating: false);
	}

	protected SmartHome.Common.API.Entities.Entities.ActionResponse GetCalibrationResponse(SmartHome.Common.API.Entities.Entities.Action anAction, BaseResponse aResponse)
	{
		bool flag = false;
		if (aResponse is MultipleResponse multipleResponse && multipleResponse.ResponseList.All((BaseResponse br) => br is AcknowledgeResponse))
		{
			flag = true;
		}
		SmartHome.Common.API.Entities.Entities.ActionResponse actionResponse = new SmartHome.Common.API.Entities.Entities.ActionResponse();
		actionResponse.Type = anAction.Type;
		actionResponse.Target = anAction.Target;
		actionResponse.Namespace = anAction.Namespace;
		actionResponse.ResultCode = (flag ? ActionResultCode.Success.ToString() : ActionResultCode.UnexpectedFailure.ToString());
		return actionResponse;
	}

	private static BaseRequest SetCalibrationModeRequest(SmartHome.Common.API.Entities.Entities.Action anAction, IConversionContext context, bool isCalibrating)
	{
		Guid rollerShutterActuatorId = anAction.Target.GetGuid();
		RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Actuators.RollerShutterActuator rollerShutterActuator = null;
		if (context.LogicalDevices != null || !context.LogicalDevices.Any())
		{
			if (!(context.LogicalDevices.FirstOrDefault((LogicalDevice ld) => ld.Id == rollerShutterActuatorId) is RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Actuators.RollerShutterActuator rollerShutterActuator2))
			{
				throw new EntityDoesNotExistException($"The roller shutter actuator {rollerShutterActuatorId} could not be found in entity cache");
			}
			rollerShutterActuator = rollerShutterActuator2.Clone();
			rollerShutterActuator.IsCalibrating = isCalibrating;
			SetEntitiesRequest setEntitiesRequest = new SetEntitiesRequest();
			setEntitiesRequest.LogicalDevices = new List<LogicalDevice>(1) { rollerShutterActuator };
			return setEntitiesRequest;
		}
		throw new EntityDoesNotExistException($"The roller shutter actuator {rollerShutterActuatorId} could not be found in entity cache");
	}
}
