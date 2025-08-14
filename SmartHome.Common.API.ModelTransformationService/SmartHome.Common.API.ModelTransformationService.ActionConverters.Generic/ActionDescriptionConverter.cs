using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.ModelTransformationService.InteractionConverters;

namespace SmartHome.Common.API.ModelTransformationService.ActionConverters.Generic;

public class ActionDescriptionConverter : IActionDescriptionConverter
{
	private readonly ParameterConverter parameterConverter = new ParameterConverter();

	private readonly LinkConverter linkConverter = new LinkConverter();

	public Action FromSmartHomeActionDescription(ActionDescription shActionDescription)
	{
		Action action = new Action();
		action.ActionId = shActionDescription.Id;
		action.Type = shActionDescription.ActionType;
		action.Namespace = shActionDescription.Namespace;
		action.Target = linkConverter.FromSmartHomeLinkBinding(shActionDescription.Target);
		Action action2 = action;
		if (shActionDescription.Data != null)
		{
			action2.Data = shActionDescription.Data.ConvertAll((RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Parameter p) => parameterConverter.FromSmartHomeParameter(p));
		}
		if (shActionDescription.Tags != null && shActionDescription.Tags.Any())
		{
			action2.Tags = shActionDescription.Tags.ConvertAll((Tag t) => new SmartHome.Common.API.Entities.Entities.Property
			{
				Name = t.Name,
				Value = t.Value
			});
		}
		return action2;
	}

	public ActionDescription ToSmartHomeActionDescription(Action apiAction)
	{
		ActionDescription actionDescription = new ActionDescription();
		actionDescription.ActionType = apiAction.Type;
		actionDescription.Namespace = apiAction.Namespace;
		actionDescription.Target = linkConverter.ToSmartHomeLinkBinding(apiAction.Target);
		actionDescription.Data = new List<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Parameter>();
		ActionDescription actionDescription2 = actionDescription;
		if (apiAction.Data != null)
		{
			actionDescription2.Data = apiAction.Data.ConvertAll((SmartHome.Common.API.Entities.Entities.Parameter parameter) => parameterConverter.ToSmartHomeParameter(parameter));
		}
		if (apiAction.Tags != null && apiAction.Tags.Any())
		{
			actionDescription2.Tags = apiAction.Tags.ConvertAll((SmartHome.Common.API.Entities.Entities.Property t) => new Tag
			{
				Name = t.Name,
				Value = t.Value.ToString()
			});
		}
		if (apiAction.ActionId.HasValue && apiAction.ActionId.HasValue)
		{
			actionDescription2.Id = apiAction.ActionId.Value;
		}
		return actionDescription2;
	}
}
