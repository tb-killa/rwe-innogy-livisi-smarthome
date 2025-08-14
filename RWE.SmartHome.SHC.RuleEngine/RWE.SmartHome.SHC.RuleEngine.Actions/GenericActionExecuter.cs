using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer;
using RWE.SmartHome.SHC.DomainModel.Actions;
using RWE.SmartHome.SHC.DomainModel.Constants;
using RWE.SmartHome.SHC.RuleEngineInterfaces;

namespace RWE.SmartHome.SHC.RuleEngine.Actions;

internal class GenericActionExecuter : IActionExecuter
{
	private readonly IProtocolMultiplexer protocolMultiplexer;

	private readonly DynamicSettingsResolver dynamicSettingsResolver;

	private readonly IApplicationsHost applicationsHost;

	private readonly IRepository configRepository;

	private readonly ITokenCache tokenCache;

	public GenericActionExecuter(IProtocolMultiplexer transformator, DynamicSettingsResolver dynamicSettingsResolver, IApplicationsHost applicationsHost, IRepository configRepository, ITokenCache tokenCache)
	{
		protocolMultiplexer = transformator;
		this.dynamicSettingsResolver = dynamicSettingsResolver;
		this.applicationsHost = applicationsHost;
		this.configRepository = configRepository;
		this.tokenCache = tokenCache;
	}

	public ExecutionResult Execute(ActionContext context, ActionDescription action)
	{
		ExecutionResult errorResponse = null;
		if (!IsValidTarget(action.Target, out errorResponse))
		{
			return errorResponse;
		}
		ActionDescription actionDescription = new ActionDescription();
		actionDescription.ActionType = action.ActionType;
		actionDescription.Id = action.Id;
		actionDescription.Data = ((action.Data != null) ? action.Data.Select((Parameter d) => dynamicSettingsResolver.EvaluateToConstantParameter(d)).ToList() : new List<Parameter>());
		actionDescription.Tags = action.Tags;
		actionDescription.Target = action.Target;
		actionDescription.Version = action.Version;
		actionDescription.Namespace = action.Namespace;
		ActionDescription actionDescription2 = actionDescription;
		EntityType linkType = actionDescription2.Target.LinkType;
		if (linkType == EntityType.Product)
		{
			return applicationsHost.ExecuteAction("sh://" + actionDescription2.Target.EntityId, actionDescription2);
		}
		return protocolMultiplexer.DeviceController.ExecuteAction(context, actionDescription2);
	}

	private bool IsValidTarget(LinkBinding target, out ExecutionResult errorResponse)
	{
		errorResponse = null;
		switch (target.LinkType)
		{
		case EntityType.LogicalDevice:
		{
			LogicalDevice logicalDevice = configRepository.GetLogicalDevice(target.EntityIdAsGuid());
			if (logicalDevice == null)
			{
				errorResponse = CreateFailureResult(ErrorConstants.InvalidCapabilityId);
			}
			break;
		}
		case EntityType.BaseDevice:
		{
			BaseDevice baseDevice = configRepository.GetBaseDevice(target.EntityIdAsGuid());
			if (baseDevice == null)
			{
				errorResponse = CreateFailureResult(ErrorConstants.InvalidDeviceResponse);
			}
			break;
		}
		case EntityType.Product:
		{
			string appId = string.Format("{0}{1}", "sh://", target.EntityId);
			if (!tokenCache.IsAvailableApplication(appId))
			{
				errorResponse = CreateFailureResult(ErrorConstants.InvalidApplicationId);
			}
			break;
		}
		}
		return errorResponse == null;
	}

	private ExecutionResult CreateFailureResult(string property)
	{
		return new ExecutionResult(ExecutionStatus.Failure, new List<Property>
		{
			new StringProperty(ErrorConstants.ErrorResult, property)
		});
	}
}
