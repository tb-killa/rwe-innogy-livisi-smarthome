using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.BusinessLogicInterfaces;
using RWE.SmartHome.SHC.DomainModel.Actions;

namespace RWE.SmartHome.SHC.BusinessLogic.VirtualDevices.SHCBaseDevice;

internal class SoftwareUpdateActionExecutor
{
	private readonly ISoftwareUpdateProcessor swuProcessor;

	public SoftwareUpdateActionExecutor(ISoftwareUpdateProcessor swuProcessor)
	{
		this.swuProcessor = swuProcessor;
	}

	public ExecutionResult HandleRequest(ActionDescription action)
	{
		return action.ActionType switch
		{
			"CheckForSoftwareUpdate" => HandleCheckForSWU(), 
			"TriggerSoftwareUpdate" => HandleTriggerSWU(), 
			_ => new ExecutionResult(ExecutionStatus.NotApplicable, new List<Property>()), 
		};
	}

	private ExecutionResult GetExecutionResult(UpdateCheckResult result)
	{
		List<Property> list = new List<Property>();
		ExecutionStatus status = ExecutionStatus.NotApplicable;
		if (result != null)
		{
			switch (result.ResultCode)
			{
			case UpdateCheckResultCode.UpdateAvailable:
				list.Add(new StringProperty(SoftwareUpdateActionConstants.UpdateStatusProperty, SoftwareUpdateActionConstants.UpdateAvailableState));
				list.Add(new StringProperty(SoftwareUpdateActionConstants.UpdateVersionProperty, result.UpdateVersion));
				status = ExecutionStatus.Success;
				break;
			case UpdateCheckResultCode.AlreadyLatest:
				list.Add(new StringProperty(SoftwareUpdateActionConstants.UpdateStatusProperty, SoftwareUpdateActionConstants.UpToDateState));
				status = ExecutionStatus.Success;
				break;
			case UpdateCheckResultCode.ErrorServiceNotAccessible:
				list.Add(new StringProperty(SoftwareUpdateActionConstants.ErrorState, SoftwareUpdateActionConstants.ErrorServiceNotAvail));
				status = ExecutionStatus.Failure;
				break;
			case UpdateCheckResultCode.ErrorUnknown:
				list.Add(new StringProperty(SoftwareUpdateActionConstants.ErrorState, SoftwareUpdateActionConstants.ErrorUnknown));
				status = ExecutionStatus.Failure;
				break;
			default:
				status = ExecutionStatus.NotApplicable;
				break;
			}
		}
		return new ExecutionResult(status, list);
	}

	private ExecutionResult HandleCheckForSWU()
	{
		UpdateCheckResult result = swuProcessor.CheckForUpdate();
		return GetExecutionResult(result);
	}

	private ExecutionResult HandleTriggerSWU()
	{
		return new ExecutionResult((!swuProcessor.DoUpdate(SoftwareUpdateModifier.ForceUpdate, usePersonalCertificate: true, saveDatabase: true, suppressErrorOutput: true, downloadOnly: false)) ? ExecutionStatus.Failure : ExecutionStatus.Success, new List<Property>());
	}
}
