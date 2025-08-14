using System;
using RWE.SmartHome.SHC.DomainModel.Actions;
using SmartHome.SHC.API.Configuration;
using SmartHome.SHC.API.PropertyDefinition;

namespace RWE.SmartHome.SHC.CoreApiConverters;

public static class ContextConverters
{
	public static ExecutionContext ToApi(this ActionContext context)
	{
		if (context == null)
		{
			return null;
		}
		ExecutionContext executionContext = new ExecutionContext();
		executionContext.Source = context.Type.ToApi();
		executionContext.Details = new Property[0];
		return executionContext;
	}

	private static ExecutionSource ToApi(this ContextType contextType)
	{
		switch (contextType)
		{
		case ContextType.ClientRequest:
			return ExecutionSource.DirectExecution;
		case ContextType.ConfigurationCommit:
			return ExecutionSource.ConfigurationCommit;
		case ContextType.RuleExecution:
		case ContextType.ClimateControlSync:
			return ExecutionSource.Interaction;
		default:
			throw new InvalidCastException("Invalid contextType: [{0}]." + contextType);
		}
	}
}
