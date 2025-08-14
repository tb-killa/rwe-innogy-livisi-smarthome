using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LogicalDeviceStateRepository;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.RuleEngine.Triggers;

namespace RWE.SmartHome.SHC.RuleEngine.DataBinders;

public class StatePropertyResolver
{
	private readonly List<IDataBinder> operandBinders;

	private readonly IPhysicalStateHandler physicalStateHandler;

	private readonly ILogicalDeviceStateRepository repository;

	private readonly IRepository configurationRepository;

	public StatePropertyResolver(List<IDataBinder> operandBinders, IPhysicalStateHandler physicalStateHandler, ILogicalDeviceStateRepository repository, IRepository configurationRepository)
	{
		this.operandBinders = operandBinders;
		this.physicalStateHandler = physicalStateHandler;
		this.repository = repository;
		this.configurationRepository = configurationRepository;
	}

	public IComparable GetStateProperty(IList<Parameter> operands, EventContext context)
	{
		return GetComparableFromProperty(operands, context, FunctionIdentifier.GetEntityStateProperty, (Property p) => p.GetValueAsComparable());
	}

	public IComparable GetMinutesSinceLastChangeProperty(IList<Parameter> operands, EventContext context)
	{
		return GetComparableFromProperty(operands, context, FunctionIdentifier.GetMinutesSinceLastChange, (Property p) => (decimal)DateTime.UtcNow.Subtract(p.UpdateTimestamp.GetValueOrDefault()).TotalMinutes);
	}

	private IComparable GetComparableFromProperty(IList<Parameter> operands, EventContext context, FunctionIdentifier fIdentifier, Func<Property, IComparable> getComparable)
	{
		Parameter linkBinding = operands.FirstOrDefault((Parameter p) => p.Name == "EntityId");
		Parameter parameter = operands.FirstOrDefault((Parameter p) => p.Name == "TargetPropertyName");
		if (!IsValid(linkBinding, parameter))
		{
			Log.Error(Module.RuleEngine, $"Could not execute \"{fIdentifier}\" function. Invalid arguments.");
			return null;
		}
		EntityIdentificationData entityIdentification = GetEntityIdentification(linkBinding, context);
		if (entityIdentification == null)
		{
			Log.Error(Module.RuleEngine, "Could not resolve LinkBinding.");
			return null;
		}
		IComparable result = null;
		EntityType entityType = entityIdentification.EntityType;
		Guid id = new Guid(entityIdentification.EntityId);
		string value = (parameter.Value as ConstantStringBinding).Value;
		Property statePropertyFromEntity = GetStatePropertyFromEntity(entityType, id, value);
		if (statePropertyFromEntity != null)
		{
			result = getComparable(statePropertyFromEntity);
		}
		else
		{
			Log.Error(Module.RuleEngine, $"Could not retrieve state for entity type {entityType}");
		}
		return result;
	}

	private bool IsValid(Parameter linkBinding, Parameter targetProperty)
	{
		if (linkBinding != null && linkBinding.Value != null && linkBinding.Value is LinkBinding && targetProperty != null && targetProperty.Value != null)
		{
			return targetProperty.Value is ConstantStringBinding;
		}
		return false;
	}

	private EntityIdentificationData GetEntityIdentification(Parameter linkBinding, EventContext context)
	{
		return operandBinders.Where((IDataBinder b) => b.CanEvaluate(linkBinding.Value)).First().GetValue(linkBinding.Value, context) as EntityIdentificationData;
	}

	private Property GetStatePropertyFromEntity(EntityType type, Guid id, string propertyName)
	{
		return type switch
		{
			EntityType.BaseDevice => GetBaseDeviceStateProperty(id, propertyName), 
			EntityType.LogicalDevice => GetLogicalDeviceStateProperty(id, propertyName), 
			_ => null, 
		};
	}

	private Property GetBaseDeviceStateProperty(Guid baseDeviceId, string statePropertyName)
	{
		return physicalStateHandler.Get(baseDeviceId)?.DeviceProperties.Properties.Get<Property>(statePropertyName);
	}

	private Property GetLogicalDeviceStateProperty(Guid logicalDeviceId, string statePropertyName)
	{
		LogicalDeviceState logicalDeviceState = repository.GetLogicalDeviceState(logicalDeviceId);
		if (logicalDeviceState == null)
		{
			Log.Debug(Module.RuleEngine, $"PropertyBinder: LogicalState for logical device {logicalDeviceId} not found in the repository. Device might be unreachable");
			return null;
		}
		return logicalDeviceState.GetProperties().Get<Property>(statePropertyName);
	}
}
