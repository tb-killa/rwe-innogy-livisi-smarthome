using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.InteractionConverters;

internal class TriggerConverter : ITriggerConverter
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(TriggerConverter));

	private readonly IConditionConverter conditionConverter = new ConditionConverter();

	private readonly LinkConverter linkConverter = new LinkConverter();

	public SmartHome.Common.API.Entities.Entities.Trigger FromSmartHomeTrigger(RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Trigger shTrigger)
	{
		SmartHome.Common.API.Entities.Entities.Trigger trigger = new SmartHome.Common.API.Entities.Entities.Trigger();
		trigger.Type = "Event";
		trigger.Link = linkConverter.FromSmartHomeLinkBinding(shTrigger.Entity);
		trigger.EventType = shTrigger.EventType;
		SmartHome.Common.API.Entities.Entities.Trigger trigger2 = trigger;
		if (shTrigger.TriggerConditions != null)
		{
			trigger2.Conditions = shTrigger.TriggerConditions.ConvertAll((RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Condition condition) => conditionConverter.FromSmartHomeCondition(condition));
		}
		if (shTrigger.Tags != null && shTrigger.Tags.Any())
		{
			trigger2.Tags = shTrigger.Tags.ConvertAll((Tag t) => new SmartHome.Common.API.Entities.Entities.Property
			{
				Name = t.Name,
				Value = t.Value
			});
		}
		return trigger2;
	}

	public RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Trigger ToSmartHomeTrigger(SmartHome.Common.API.Entities.Entities.Trigger apiTrigger)
	{
		RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Trigger trigger = new RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Trigger();
		trigger.Entity = linkConverter.ToSmartHomeLinkBinding(apiTrigger.Link);
		trigger.EventType = apiTrigger.EventType;
		RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Trigger trigger2 = trigger;
		if (apiTrigger.Conditions != null)
		{
			trigger2.TriggerConditions = apiTrigger.Conditions.ConvertAll((SmartHome.Common.API.Entities.Entities.Condition condition) => conditionConverter.ToSmartHomeCondition(condition));
		}
		if (apiTrigger.Tags != null && apiTrigger.Tags.Any())
		{
			trigger2.Tags = apiTrigger.Tags.ConvertAll((SmartHome.Common.API.Entities.Entities.Property t) => new Tag
			{
				Name = t.Name,
				Value = t.Value.ToString()
			});
		}
		return trigger2;
	}
}
