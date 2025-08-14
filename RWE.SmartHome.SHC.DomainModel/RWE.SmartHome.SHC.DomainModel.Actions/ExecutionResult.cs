using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using SmartHome.SHC.API;
using SmartHome.SHC.API.PropertyDefinition;

namespace RWE.SmartHome.SHC.DomainModel.Actions;

public class ExecutionResult
{
	public ExecutionStatus Status { get; private set; }

	public List<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property> Parameters { get; private set; }

	public ExecutionResult(ExecutionStatus status, List<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property> parameters)
	{
		Status = status;
		Parameters = parameters;
	}

	public static ExecutionResult UnknownFailure()
	{
		return Error("Unknown failure");
	}

	public static ExecutionResult Error(string failureText)
	{
		return new ExecutionResult(ExecutionStatus.Failure, new List<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property>
		{
			new RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.StringProperty("ErrorType", failureText ?? "Unknown failure")
		});
	}

	public static ExecutionResult Success()
	{
		return new ExecutionResult(ExecutionStatus.Success, new List<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property>());
	}

	public static ExecutionResult FromDeviceSDK(global::SmartHome.SHC.API.ExecutionResult dsdkResult)
	{
		ExecutionStatus status = dsdkResult.Status.ToDomainModel();
		List<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property> parameters = ((dsdkResult.Details == null) ? new List<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property>() : dsdkResult.Details.Select((global::SmartHome.SHC.API.PropertyDefinition.Property p) => p.To_CoreProperty()).ToList());
		return new ExecutionResult(status, parameters);
	}

	public override string ToString()
	{
		string arg = string.Empty;
		if (Parameters != null && Parameters.Count > 0)
		{
			arg = string.Join(",", Parameters.Select((RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property param) => param.Name + "=" + param.GetValueAsString()).ToArray());
		}
		return $"ExecutionResult: [Status: [{Status.ToString()}], Parameters: [{arg}]]";
	}
}
