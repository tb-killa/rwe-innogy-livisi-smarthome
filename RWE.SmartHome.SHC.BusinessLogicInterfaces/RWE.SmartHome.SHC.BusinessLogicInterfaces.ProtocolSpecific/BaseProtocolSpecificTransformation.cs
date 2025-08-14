using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.DomainModel.Rules;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolSpecific;

public abstract class BaseProtocolSpecificTransformation : IProtocolSpecificTransformation
{
	protected readonly IRepository configRepository;

	protected List<Guid> supportedDeviceIds;

	protected List<Guid> supportedCapabilityIds;

	public abstract ProtocolIdentifier ProtocolId { get; }

	public IList<ErrorEntry> Errors { get; protected set; }

	public IEnumerable<LogicalDeviceState> ImmediateStateChanges { get; protected set; }

	protected BaseProtocolSpecificTransformation(IRepository configRepository)
	{
		this.configRepository = configRepository;
		Errors = new List<ErrorEntry>();
		ImmediateStateChanges = new List<LogicalDeviceState>();
	}

	public abstract bool PrepareTransformation(IElementaryRuleRepository elementaryRuleRepository);

	public abstract void CommitTransformationResults(IEnumerable<Guid> devicesToDelete);

	public virtual void DiscardTransformationResults()
	{
		CleanupTransformationResults();
	}

	protected virtual void CreateLinkWithShc(Rule rule, Trigger trigger)
	{
	}

	protected virtual void CleanupTransformationResults()
	{
		Errors = new List<ErrorEntry>();
		ImmediateStateChanges = new List<LogicalDeviceState>();
	}

	protected void AddValidationError(Guid entityId, EntityType entityType, ValidationErrorCode validationError, Guid profileId)
	{
		Errors.Add(CreateErrorEntry(entityId, entityType, validationError, profileId));
	}

	protected void AddValidationError(Guid entityId, EntityType entityType, ValidationErrorCode validationError, params ErrorParameter[] parameters)
	{
		Errors.Add(CreateErrorEntry(entityId, entityType, validationError, parameters));
	}

	private static ErrorEntry CreateErrorEntry(Guid entityId, EntityType entityType, ValidationErrorCode validationError, Guid profileId)
	{
		return CreateErrorEntry(entityId, entityType, validationError, new ErrorParameter
		{
			Key = ErrorParameterKey.ProfileId,
			Value = profileId.ToString()
		});
	}

	private static ErrorEntry CreateErrorEntry(Guid entityId, EntityType entityType, ValidationErrorCode validationError, params ErrorParameter[] parameters)
	{
		ErrorEntry errorEntry = new ErrorEntry();
		errorEntry.AffectedEntity = new EntityMetadata
		{
			EntityType = entityType,
			Id = entityId
		};
		errorEntry.ErrorCode = validationError;
		ErrorEntry errorEntry2 = errorEntry;
		errorEntry2.ErrorParameters.AddRange(parameters);
		return errorEntry2;
	}

	protected virtual void UpdateTargetEntities()
	{
		supportedDeviceIds = (from ld in configRepository.GetBaseDevices()
			where ld.ProtocolId == ProtocolId
			select ld.Id).ToList();
		supportedCapabilityIds = (from ld in configRepository.GetLogicalDevices()
			where supportedDeviceIds.Contains(ld.BaseDeviceId)
			select ld.Id).ToList();
	}

	private bool SupportsLinkForAcceleration(LinkBinding binding)
	{
		bool result = false;
		if (binding != null)
		{
			switch (binding.LinkType)
			{
			case EntityType.BaseDevice:
				result = supportedDeviceIds.Contains(binding.EntityIdAsGuid());
				break;
			case EntityType.LogicalDevice:
				result = supportedCapabilityIds.Contains(binding.EntityIdAsGuid());
				break;
			}
		}
		return result;
	}

	private bool IsCandidateForAcceleration(ElementaryTrigger trigger)
	{
		if (trigger.Trigger != null)
		{
			return SupportsLinkForAcceleration(trigger.Trigger.Entity);
		}
		return true;
	}

	private bool IsCandidateForAcceleration(ElementaryRule rule)
	{
		if (!rule.IsRestricted() && !rule.Trigger.HasComplexCondition())
		{
			if (!SupportsLinkForAcceleration(rule.Action.Target))
			{
				return IsCandidateForAcceleration(rule.Trigger);
			}
			return true;
		}
		return false;
	}

	private void HandleRuleOnShc(ElementaryRule rule)
	{
		Trigger trigger = rule.Trigger.Trigger;
		if (trigger != null && SupportsLinkForAcceleration(trigger.Entity))
		{
			CreateLinkWithShc(rule.SourceRule, trigger);
		}
	}

	public void ProcessElementaryRules(IElementaryRuleRepository elementaryRulesRepository)
	{
		List<ElementaryRule> list = elementaryRulesRepository.ListUnhandledRules().ToList();
		UpdateTargetEntities();
		foreach (ElementaryRule item in list)
		{
			bool flag = false;
			if (IsCandidateForAcceleration(item))
			{
				flag = AccelerateRule(item);
			}
			if (flag)
			{
				item.MarkAsHandled();
			}
			else
			{
				HandleRuleOnShc(item);
			}
		}
	}

	protected abstract bool AccelerateRule(ElementaryRule rule);
}
