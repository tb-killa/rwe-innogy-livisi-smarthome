using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.Entities.Extensions;
using SmartHome.Common.API.ModelTransformationService.InteractionConverters;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService;

public class InteractionConverterService : IInteractionConverterService
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(InteractionConverterService));

	private readonly IRuleConverter ruleConverter = new RuleConverter();

	public SmartHome.Common.API.Entities.Entities.Interaction FromSmartHomeInteraction(RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Interaction shInteraction)
	{
		SmartHome.Common.API.Entities.Entities.Interaction interaction = new SmartHome.Common.API.Entities.Entities.Interaction();
		interaction.Name = shInteraction.Name;
		interaction.Id = shInteraction.Id.ToString("N");
		interaction.Created = shInteraction.CreationDate;
		interaction.Modified = shInteraction.LastChangeDate;
		interaction.ValidFrom = shInteraction.ValidFrom;
		interaction.ValidTo = shInteraction.ValidTo;
		interaction.Freezetime = shInteraction.Freezetime;
		interaction.IsInternal = shInteraction.IsInternal;
		SmartHome.Common.API.Entities.Entities.Interaction interaction2 = interaction;
		if (shInteraction.Tags != null)
		{
			interaction2.Tags = shInteraction.Tags.ConvertAll((Tag t) => new SmartHome.Common.API.Entities.Entities.Property
			{
				Name = t.Name,
				Value = t.Value
			});
		}
		if (shInteraction.Rules != null)
		{
			interaction2.Rules = shInteraction.Rules.ConvertAll((RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Rule rule) => ruleConverter.FromSmartHomeRule(rule));
		}
		return interaction2;
	}

	public RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Interaction ToSmartHomeInteraction(SmartHome.Common.API.Entities.Entities.Interaction apiInteraction)
	{
		logger.DebugEnterMethod("ToSmartHomeInteraction");
		RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Interaction interaction = new RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Interaction();
		interaction.Name = apiInteraction.Name;
		interaction.Id = apiInteraction.Id.ToGuid();
		interaction.CreationDate = apiInteraction.Created;
		interaction.LastChangeDate = apiInteraction.Modified;
		interaction.ValidFrom = apiInteraction.ValidFrom;
		interaction.ValidTo = apiInteraction.ValidTo;
		interaction.Freezetime = apiInteraction.Freezetime.GetValueOrDefault();
		interaction.IsInternal = apiInteraction.IsInternal == true;
		RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Interaction interaction2 = interaction;
		if (apiInteraction.Tags != null)
		{
			logger.Debug($"Converting tags: {apiInteraction.Tags.Count}");
			interaction2.Tags = apiInteraction.Tags.ConvertAll((SmartHome.Common.API.Entities.Entities.Property t) => new Tag
			{
				Name = t.Name,
				Value = t.Value.ToString()
			});
		}
		if (apiInteraction.Rules != null)
		{
			logger.Debug($"Converting rules: {apiInteraction.Rules.Count}");
			interaction2.Rules = apiInteraction.Rules.ConvertAll((SmartHome.Common.API.Entities.Entities.Rule rule) => ruleConverter.ToSmartHomeRule(rule, apiInteraction.Id.ToGuid()));
		}
		logger.DebugExitMethod("ToSmartHomeInteraction");
		return interaction2;
	}
}
