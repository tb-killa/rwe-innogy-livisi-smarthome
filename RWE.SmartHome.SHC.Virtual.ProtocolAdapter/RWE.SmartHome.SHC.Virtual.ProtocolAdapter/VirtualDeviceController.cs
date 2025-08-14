using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolSpecific;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.VirtualDevices;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.CoreApiConverters;
using RWE.SmartHome.SHC.DomainModel.Actions;
using SmartHome.SHC.API;
using SmartHome.SHC.API.Configuration;
using SmartHome.SHC.API.PropertyDefinition;

namespace RWE.SmartHome.SHC.Virtual.ProtocolAdapter;

internal class VirtualDeviceController : IProtocolSpecificDeviceController
{
	private const string sourceInteractionName = "SourceInteractionName";

	private readonly IRepository configurationRepository;

	private readonly IApplicationsHost applicationsHost;

	private readonly List<IVirtualCoreActionHandler> coreStateHandlers;

	internal VirtualDeviceController(IRepository configurationRepository, IApplicationsHost applicationsHost, List<IVirtualCoreActionHandler> coreStateHandlers)
	{
		this.configurationRepository = configurationRepository;
		this.applicationsHost = applicationsHost;
		this.coreStateHandlers = coreStateHandlers;
	}

	public List<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property> CreateTriggerEvent(LogicalDevice logicalDevice, int buttonId)
	{
		return null;
	}

	public RWE.SmartHome.SHC.DomainModel.Actions.ExecutionResult ExecuteAction(ActionContext context, RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.ActionDescription action)
	{
		BaseDevice baseDevice = GetBaseDevice(action.Target);
		if (baseDevice == null || baseDevice.ProtocolId != ProtocolIdentifier.Virtual)
		{
			return RWE.SmartHome.SHC.DomainModel.Actions.ExecutionResult.Error("Invalid base device");
		}
		RWE.SmartHome.SHC.DomainModel.Actions.ExecutionResult executionResult = ((!(baseDevice.AppId == CoreConstants.CoreAppId)) ? ExecuteVirtualDeviceAction(baseDevice.AppId, action, context) : ExecuteVirtualCoreDeviceAction(baseDevice.AppId, action, context));
		if (executionResult.Status == RWE.SmartHome.SHC.DomainModel.Actions.ExecutionStatus.Failure)
		{
			Log.Error(Module.VirtualProtocolAdapter, $"Error executing action {action.ActionType} for device [{action.Target.EntityId}]");
		}
		return executionResult;
	}

	private RWE.SmartHome.SHC.DomainModel.Actions.ExecutionResult ExecuteVirtualCoreDeviceAction(string appId, RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.ActionDescription action, ActionContext context)
	{
		RWE.SmartHome.SHC.DomainModel.Actions.ExecutionResult executionResult = null;
		foreach (IVirtualCoreActionHandler coreStateHandler in coreStateHandlers)
		{
			executionResult = coreStateHandler.ExecuteAction(context, action);
			if (executionResult.Status != RWE.SmartHome.SHC.DomainModel.Actions.ExecutionStatus.NotApplicable)
			{
				break;
			}
		}
		if (executionResult == null)
		{
			executionResult = RWE.SmartHome.SHC.DomainModel.Actions.ExecutionResult.Error("No action handler found for action type : " + action.ActionType);
		}
		return executionResult;
	}

	private RWE.SmartHome.SHC.DomainModel.Actions.ExecutionResult ExecuteVirtualDeviceAction(string appId, RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.ActionDescription action, ActionContext context)
	{
		IActionExecuterHandler customDevice = applicationsHost.GetCustomDevice<IActionExecuterHandler>(appId);
		if (customDevice == null)
		{
			Log.Warning(Module.VirtualProtocolAdapter, $"Could not call ExecuteCustomAction for custom application with AppId = {appId}; please check if app is activated");
			return RWE.SmartHome.SHC.DomainModel.Actions.ExecutionResult.Error(string.Concat(Module.VirtualProtocolAdapter, " : App inactive"));
		}
		ExecutionContext apiContext = GetApiContext(context);
		return RWE.SmartHome.SHC.DomainModel.Actions.ExecutionResult.FromDeviceSDK(customDevice.ExecuteAction(action.ToApi(), apiContext));
	}

	private ExecutionContext GetApiContext(ActionContext context)
	{
		ExecutionContext executionContext = context.ToApi();
		executionContext.Details = new global::SmartHome.SHC.API.PropertyDefinition.Property[1]
		{
			new global::SmartHome.SHC.API.PropertyDefinition.StringProperty("SourceInteractionName", context.InteractionName)
		};
		return executionContext;
	}

	private BaseDevice GetBaseDevice(LinkBinding linkBinding)
	{
		switch (linkBinding.LinkType)
		{
		case EntityType.LogicalDevice:
		{
			LogicalDevice logicalDevice = configurationRepository.GetLogicalDevice(linkBinding.EntityIdAsGuid());
			return logicalDevice.BaseDevice;
		}
		case EntityType.BaseDevice:
			return configurationRepository.GetBaseDevice(linkBinding.EntityIdAsGuid());
		default:
			return null;
		}
	}
}
