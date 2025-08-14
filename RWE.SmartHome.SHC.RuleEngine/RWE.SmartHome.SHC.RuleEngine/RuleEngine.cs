using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.BusinessLogicInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.CoreEvents;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.IntegrityManagement;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LogicalDeviceStateRepository;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LogicalDeviceStateRepository.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.Scheduler;
using RWE.SmartHome.SHC.Core.Scheduler.Tasks;
using RWE.SmartHome.SHC.DomainModel.Actions;
using RWE.SmartHome.SHC.DomainModel.Constants;
using RWE.SmartHome.SHC.RuleEngine.Conditions;
using RWE.SmartHome.SHC.RuleEngine.Triggers;
using RWE.SmartHome.SHC.RuleEngineInterfaces;
using RWE.SmartHome.SHC.StartupLogicInterfaces.Events;

namespace RWE.SmartHome.SHC.RuleEngine;

public class RuleEngine : IRuleEngine
{
	private class InteractionState
	{
		public bool IsFrozen { get; private set; }

		public bool IsValid { get; private set; }

		public DateTime ValidFrom { get; private set; }

		public DateTime ValidTo { get; private set; }

		public Guid InteractionId { get; private set; }

		public InteractionState(bool isFrozen, bool isValid, DateTime validFrom, DateTime validTo, Guid interactionId)
		{
			IsFrozen = isFrozen;
			IsValid = isValid;
			ValidFrom = validFrom;
			ValidTo = validTo;
			InteractionId = interactionId;
		}
	}

	private readonly IRulesRepository rulesRepository;

	private readonly IEventManager eventManager;

	private readonly IRepository configurationRepository;

	private readonly IScheduler taskScheduler;

	private readonly Dictionary<Guid, bool> profileStates = new Dictionary<Guid, bool>();

	private readonly IActionExecuter executer;

	private readonly ConditionEvaluator conditionEvaluator;

	private Dictionary<Guid, DateTime> lastExecution = new Dictionary<Guid, DateTime>();

	private object lastExecutionLock = new object();

	public RuleEngine(IEventManager eventManager, IRulesRepository repository, IProtocolMultiplexer protocolMultiplexer, ILogicalDeviceStateRepository stateRepository, IRuleEngineObjectsFactory factory, IRepository configurationRepository, IActionExecuter executer, IScheduler scheduler)
	{
		rulesRepository = repository;
		this.eventManager = eventManager;
		this.configurationRepository = configurationRepository;
		taskScheduler = scheduler;
		eventManager.GetEvent<ShcStartupCompletedEvent>().Subscribe(OnShcInitializationFinished, (ShcStartupCompletedEventArgs a) => a.Progress == StartupProgress.CompletedRound2, ThreadOption.PublisherThread, null);
		RuleEngineWorkerObjects ruleEngineWorkerObjects = factory.CreateRuleEngineWorkerObjects(stateRepository, protocolMultiplexer, configurationRepository);
		this.executer = executer;
		conditionEvaluator = ruleEngineWorkerObjects.ConditionEvaluator;
	}

	private void OnShcInitializationFinished(ShcStartupCompletedEventArgs args)
	{
		eventManager.GetEvent<DeviceEventDetectedEvent>().Subscribe(OnEventTriggerData, null, ThreadOption.BackgroundThread, null);
		eventManager.GetEvent<LogicalDeviceStateChangedEvent>().Subscribe(OnDeviceStateChanged, null, ThreadOption.BackgroundThread, null);
		eventManager.GetEvent<TriggerRulesEvent>().Subscribe(OnRuleExternallyTriggered, null, ThreadOption.BackgroundThread, null);
		eventManager.GetEvent<CustomTriggerEvent>().Subscribe(OnCustomTriggerTriggered, null, ThreadOption.BackgroundThread, null);
		eventManager.GetEvent<ShcStartupCompletedEvent>().Publish(new ShcStartupCompletedEventArgs(StartupProgress.RuleEngineRunning));
	}

	private void OnEventTriggerData(DeviceEventDetectedEventArgs eventArgs)
	{
		Log.Debug(Module.RuleEngine, $"Trying to find rule that contains trigger {eventArgs.LogicalDeviceId}");
		string[] value = (from m in eventArgs.EventDetails
			where m.Name != EventConstants.EventTypePropertyName
			select $"{m.Name}={m.GetValueAsComparable()}").ToArray();
		Log.Debug(Module.RuleEngine, string.Format("EventParams: {0}", string.Join(", ", value)));
		try
		{
			FindAndTriggerRuleProfile(new TriggerData(TriggerDataType.EventTriggerData, new LinkBinding(EntityType.LogicalDevice, eventArgs.LogicalDeviceId), null, eventArgs.EventDetails), eventArgs.EventType);
		}
		catch (Exception arg)
		{
			Log.Error(Module.RuleEngine, $"Invalid operation during finding and triggering of rule profiles: {arg}");
		}
	}

	private void OnDeviceStateChanged(LogicalDeviceStateChangedEventArgs eventArgs)
	{
		Log.Debug(Module.RuleEngine, $"Trying to find rule profile that will be triggered by {eventArgs.LogicalDeviceId}");
		if (eventArgs.OldLogicalDeviceState == null)
		{
			Log.Debug(Module.RuleEngine, $"\tOld LogicalState: Undefined");
		}
		else
		{
			Log.Debug(Module.RuleEngine, $"\tOld LogicalState: {eventArgs.OldLogicalDeviceState} ");
		}
		if (eventArgs.NewLogicalDeviceState == null)
		{
			Log.Debug(Module.RuleEngine, $"\tNew LogicalState: Undefined");
		}
		else
		{
			Log.Debug(Module.RuleEngine, $"\tNew LogicalState: {eventArgs.NewLogicalDeviceState}");
		}
		try
		{
			if (ShouldIgnoreStateChangeEvent(eventArgs))
			{
				Log.Debug(Module.RuleEngine, "Device changed it's reachability state. Ignoring state change.");
			}
			else
			{
				FindAndTriggerRuleProfile(new TriggerData(TriggerDataType.StateTriggerData, new LinkBinding(EntityType.LogicalDevice, eventArgs.LogicalDeviceId), eventArgs.OldLogicalDeviceState, eventArgs.NewLogicalDeviceState), null);
			}
		}
		catch (Exception arg)
		{
			Log.Error(Module.RuleEngine, $"Invalid operation during finding and triggering of rule profiles: {arg}");
		}
	}

	private bool ShouldIgnoreStateChangeEvent(LogicalDeviceStateChangedEventArgs eventArgs)
	{
		if (eventArgs.OldLogicalDeviceState != null)
		{
			return eventArgs.NewLogicalDeviceState == null;
		}
		return true;
	}

	private void OnRuleExternallyTriggered(TriggerRulesEventArgs eventArgs)
	{
		List<Rule> rulesPool = configurationRepository.GetInteractions().SelectMany((Interaction x) => x.Rules).ToList();
		List<Rule> list = eventArgs.RuleIds.Select((Guid ruleId) => rulesPool.FirstOrDefault((Rule rp) => rp.Id == ruleId)).ToList();
		foreach (Rule item in list)
		{
			if (item != null)
			{
				EvaluateAndExecuteRule(item, null);
			}
		}
	}

	private void OnCustomTriggerTriggered(CustomTriggerEventArgs eventArgs)
	{
		Guid triggerId = eventArgs.TriggerId;
		IEnumerable<Rule> enumerable = rulesRepository.Rules.Where((Rule r) => r.CustomTriggers != null && r.CustomTriggers.Any((CustomTrigger ct) => ct.Id == triggerId));
		foreach (Rule item in enumerable)
		{
			CustomTrigger customTrigger = item.CustomTriggers.First((CustomTrigger t) => t.Id == triggerId);
			EvaluateAndExecuteRule(item, customTrigger.Entity);
		}
	}

	private Entity GetTriggerEntity(LinkBinding link)
	{
		if (link != null)
		{
			Guid entityId = link.EntityIdAsGuid();
			return link.LinkType switch
			{
				EntityType.BaseDevice => configurationRepository.GetBaseDevices().FirstOrDefault((BaseDevice d) => d.Id == entityId), 
				EntityType.LogicalDevice => GetDevice(entityId), 
				_ => null, 
			};
		}
		return null;
	}

	private BaseDevice GetDevice(Guid? capabilityId)
	{
		if (capabilityId.HasValue)
		{
			return configurationRepository.GetLogicalDevice(capabilityId.Value)?.BaseDevice;
		}
		return null;
	}

	internal bool EvaluateTriggers(List<Trigger> triggers, TriggerData context)
	{
		return triggers?.Where((Trigger tg) => tg.Entity.EntityIdAsGuid() == context.TriggerEntity.EntityIdAsGuid()).Any((Trigger trigger) => trigger.TriggerConditions == null || trigger.TriggerConditions.Count == 0 || EvaluateTriggerConditions(trigger, context)) ?? false;
	}

	private bool EvaluateTriggerConditions(Trigger trigger, TriggerData context)
	{
		bool result = false;
		if (context != null)
		{
			EventContext eventContext = new EventContext();
			eventContext.CurrentStateProperties = context.NewStateProperties;
			eventContext.OldStateProperties = context.OldStateProperties;
			EventContext eventContext2 = eventContext;
			switch (context.TriggerDataType)
			{
			case TriggerDataType.StateTriggerData:
				result = AreStateTriggerConditionsMatched(trigger, eventContext2);
				break;
			case TriggerDataType.EventTriggerData:
				result = AreEventTriggerConditionsMatched(trigger, eventContext2);
				break;
			}
		}
		return result;
	}

	private bool AreStateTriggerConditionsMatched(Trigger trigger, EventContext eventContext)
	{
		List<Condition> triggerConditions = trigger.TriggerConditions;
		EventContext eventContext2 = new EventContext();
		eventContext2.CurrentStateProperties = eventContext.OldStateProperties;
		EventContext eventContext3 = eventContext2;
		if (!ConditionsMatchForContext(triggerConditions, eventContext3))
		{
			return ConditionsMatchForContext(triggerConditions, eventContext);
		}
		return false;
	}

	private bool AreEventTriggerConditionsMatched(Trigger trigger, EventContext eventContext)
	{
		return ConditionsMatchForContext(trigger.TriggerConditions, eventContext);
	}

	private bool ConditionsMatchForContext(List<Condition> conditions, EventContext eventContext)
	{
		return conditions?.All((Condition trgCond) => conditionEvaluator.Evaluate(trgCond, eventContext)) ?? false;
	}

	private bool EvaluateRuleConditions(Rule rule)
	{
		if (rule.Conditions != null)
		{
			return rule.Conditions.TrueForAll((Condition c) => conditionEvaluator.Evaluate(c, null));
		}
		return true;
	}

	private InteractionState GetInteractionState(Guid interactionId)
	{
		Interaction interaction = configurationRepository.GetInteraction(interactionId);
		if (interaction != null)
		{
			bool isFrozen;
			lock (lastExecutionLock)
			{
				isFrozen = lastExecution.Keys.Contains(interactionId) && interaction.IsInteractionFrozen(lastExecution[interactionId]);
			}
			bool isValid = interaction.IsInteractionValid();
			return new InteractionState(isFrozen, isValid, interaction.GetInteractionValidFromDate(), interaction.GetInteractionValidToDate(), interaction.Id);
		}
		return null;
	}

	internal void FindAndTriggerRuleProfile(TriggerData triggerData, string eventType)
	{
		Log.Debug(Module.RuleEngine, string.Format("Event type: [{0}]", string.IsNullOrEmpty(eventType) ? "Undefined" : triggerData.TriggerDataType.ToString()));
		Log.Debug(Module.RuleEngine, $"Trigger type: [{triggerData.TriggerDataType}]");
		Log.Debug(Module.RuleEngine, $"Trigger entity type: [{triggerData.TriggerEntity.LinkType}]");
		IEnumerable<Rule> rulesToBeProcessed = GetRulesToBeProcessed(triggerData.TriggerEntity.EntityIdAsGuid(), eventType);
		foreach (Rule item in rulesToBeProcessed)
		{
			Log.Debug(Module.RuleEngine, $"Rule in interaction [{item.InteractionId}] may contain the trigger source, evaluating...");
			if (EvaluateTriggers(item.Triggers, triggerData))
			{
				EvaluateAndExecuteRule(item, triggerData.TriggerEntity);
			}
		}
	}

	private void EvaluateConditionsAndExecuteRule(Rule rule, LinkBinding triggerEntity)
	{
		Entity triggerEntity2 = GetTriggerEntity(triggerEntity);
		if (triggerEntity2 == null)
		{
			Log.Debug(Module.RuleEngine, "Rule triggered externally, or trigger was deleted.");
		}
		if (EvaluateRuleConditions(rule))
		{
			ExecuteRule(rule, triggerEntity);
		}
	}

	private void EvaluateAndExecuteRule(Rule rule, LinkBinding triggerEntity)
	{
		InteractionState interactionState = GetInteractionState(rule.InteractionId);
		if (interactionState != null && (interactionState.IsFrozen || !interactionState.IsValid))
		{
			Guid ruleId = GetRuleId(rule);
			if (interactionState.IsFrozen)
			{
				Log.Debug(Module.RuleEngine, $"Rule with Id {ruleId} won't be executed, because it's parent interaction is frozen.");
			}
			if (!interactionState.IsValid)
			{
				Log.Debug(Module.RuleEngine, $"Rule with Id {ruleId} won't be executed, because it's parent interaction is valid between {interactionState.ValidFrom}-{interactionState.ValidTo}.");
			}
		}
		else if (rule.ConditionEvaluationDelay > 0)
		{
			taskScheduler.AddSchedulerTask(new FixedTimeSpanSchedulerTask(Guid.NewGuid(), delegate
			{
				EvaluateConditionsAndExecuteRule(rule, triggerEntity);
			}, TimeSpan.FromSeconds(rule.ConditionEvaluationDelay), runOnce: true));
		}
		else
		{
			EvaluateConditionsAndExecuteRule(rule, triggerEntity);
		}
	}

	private IEnumerable<Rule> GetRulesToBeProcessed(Guid deviceId, string eventType)
	{
		if (deviceId == Guid.Empty)
		{
			return GetRulesForEvent(rulesRepository.Rules, eventType);
		}
		IEnumerable<Rule> rulesForDevice = rulesRepository.GetRulesForDevice(deviceId);
		if (string.IsNullOrEmpty(eventType))
		{
			return GetRulesForDeviceTrigger(rulesForDevice, deviceId);
		}
		return GetRulesForEvent(rulesForDevice, eventType);
	}

	private IEnumerable<Rule> GetRulesForDeviceTrigger(IEnumerable<Rule> rules, Guid deviceId)
	{
		return rules.Where((Rule rule) => rule.Triggers.Any((Trigger trg) => trg.Entity.EntityIdAsGuid() == deviceId));
	}

	private IEnumerable<Rule> GetRulesForEvent(IEnumerable<Rule> rules, string eventType)
	{
		return rules.Where((Rule rule) => rule.Triggers.Any((Trigger trigger) => trigger.EventType.Contains(eventType)));
	}

	private void ExecuteRule(Rule rule, LinkBinding deviceTrigger)
	{
		Guid ruleId = GetRuleId(rule);
		Log.Debug(Module.RuleEngine, $"Executing rule {ruleId}");
		if (executer == null)
		{
			Log.Warning(Module.RuleEngine, "Could not find executer for action");
			return;
		}
		if (rule.Actions == null)
		{
			Log.Information(Module.RuleEngine, $"Rule {ruleId} has no actions. Nothing to execute.");
			return;
		}
		if (!ExecuteRuleActions(rule, deviceTrigger))
		{
			Log.Debug(Module.RuleEngine, $"Rule {ruleId} execution failed");
			eventManager.GetEvent<RuleExecutionFailedEvent>().Publish(new RuleExecutionFailedEventArgs
			{
				RuleId = ruleId
			});
		}
		lock (lastExecutionLock)
		{
			lastExecution[rule.InteractionId] = DateTime.UtcNow;
		}
	}

	public bool ExecuteRuleActions(Rule rule, LinkBinding deviceTrigger)
	{
		bool result = true;
		ActionContext executionSource = new ActionContext(ContextType.RuleExecution, GetRuleId(rule), deviceTrigger, GetInteractionName(rule.InteractionId));
		foreach (ActionDescription action in rule.Actions)
		{
			try
			{
				executer.Execute(executionSource, action);
			}
			catch
			{
				result = false;
			}
		}
		return result;
	}

	private Guid GetRuleId(Rule rule)
	{
		Guid result = rule.Id;
		if (rule.Tags != null && rule.Tags.Any((Tag m) => "SourceRuleId".Equals(m.Name)))
		{
			result = new Guid(rule.Tags.First((Tag m) => "SourceRuleId".Equals(m.Name)).Value);
		}
		return result;
	}

	private string GetInteractionName(Guid interactionId)
	{
		Interaction interaction = configurationRepository.GetInteraction(interactionId);
		if (interaction != null)
		{
			return interaction.Name;
		}
		return string.Empty;
	}
}
