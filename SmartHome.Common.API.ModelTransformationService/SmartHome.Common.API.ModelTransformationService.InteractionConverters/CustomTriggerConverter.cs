using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.ModelTransformationService.GenericConverters;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.InteractionConverters;

public class CustomTriggerConverter : ICustomTriggerConverter
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(CustomTriggerConverter));

	private readonly LinkConverter linkConverter = new LinkConverter();

	public SmartHome.Common.API.Entities.Entities.CustomTrigger FromSmartHomeCustomTrigger(RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.CustomTrigger shTrigger)
	{
		SmartHome.Common.API.Entities.Entities.CustomTrigger customTrigger = new SmartHome.Common.API.Entities.Entities.CustomTrigger();
		customTrigger.Link = linkConverter.FromSmartHomeLinkBinding(shTrigger.Entity);
		customTrigger.Type = shTrigger.Type;
		customTrigger.Namespace = shTrigger.Namespace;
		SmartHome.Common.API.Entities.Entities.CustomTrigger customTrigger2 = customTrigger;
		if (shTrigger.Properties != null && shTrigger.Properties.Any())
		{
			customTrigger2.Properties = shTrigger.Properties.ConvertAll<SmartHome.Common.API.Entities.Entities.Property>(PropertyConverter.ToApiProperty);
		}
		if (shTrigger.Tags != null && shTrigger.Tags.Any())
		{
			customTrigger2.Tags = shTrigger.Tags.ConvertAll((Tag t) => new SmartHome.Common.API.Entities.Entities.Property
			{
				Name = t.Name,
				Value = t.Value
			});
		}
		return customTrigger2;
	}

	public RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.CustomTrigger ToSmartHomeCustomTrigger(SmartHome.Common.API.Entities.Entities.CustomTrigger apiTrigger)
	{
		RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.CustomTrigger customTrigger = new RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.CustomTrigger();
		customTrigger.Entity = linkConverter.ToSmartHomeLinkBinding(apiTrigger.Link);
		customTrigger.Type = apiTrigger.Type;
		customTrigger.Namespace = apiTrigger.Namespace;
		RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.CustomTrigger customTrigger2 = customTrigger;
		if (apiTrigger.Properties != null && apiTrigger.Properties.Any())
		{
			customTrigger2.Properties = apiTrigger.Properties.ConvertAll<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property>(PropertyConverter.ToSmartHomeProperty);
		}
		if (apiTrigger.Tags != null && apiTrigger.Tags.Any())
		{
			customTrigger2.Tags = apiTrigger.Tags.ConvertAll((SmartHome.Common.API.Entities.Entities.Property t) => new Tag
			{
				Name = t.Name,
				Value = t.Value.ToString()
			});
		}
		return customTrigger2;
	}
}
