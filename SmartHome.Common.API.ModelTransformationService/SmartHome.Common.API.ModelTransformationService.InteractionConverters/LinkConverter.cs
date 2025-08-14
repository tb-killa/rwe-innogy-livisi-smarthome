using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.ModelTransformationService.Extensions;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.InteractionConverters;

public class LinkConverter
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(LinkConverter));

	private static readonly Dictionary<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType, string> entityLinkPatternMapping = new Dictionary<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType, string>
	{
		{
			RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType.BaseDevice,
			"/device/{0}"
		},
		{
			RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType.LogicalDevice,
			"/capability/{0}"
		},
		{
			RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType.Location,
			"/location/{0}"
		},
		{
			RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType.Product,
			"/product/{0}/{1}"
		},
		{
			RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType.Interaction,
			"/interaction/{0}"
		},
		{
			RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType.Home,
			"/home/{0}"
		},
		{
			RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType.Member,
			"/home/member/{0}"
		}
	};

	private static readonly Dictionary<SmartHome.Common.API.Entities.Entities.EntityType, RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType> entityTypeMapping = new Dictionary<SmartHome.Common.API.Entities.Entities.EntityType, RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType>
	{
		{
			SmartHome.Common.API.Entities.Entities.EntityType.Device,
			RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType.BaseDevice
		},
		{
			SmartHome.Common.API.Entities.Entities.EntityType.Capability,
			RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType.LogicalDevice
		},
		{
			SmartHome.Common.API.Entities.Entities.EntityType.Location,
			RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType.Location
		},
		{
			SmartHome.Common.API.Entities.Entities.EntityType.Product,
			RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType.Product
		},
		{
			SmartHome.Common.API.Entities.Entities.EntityType.Interaction,
			RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType.Interaction
		},
		{
			SmartHome.Common.API.Entities.Entities.EntityType.Home,
			RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType.Home
		},
		{
			SmartHome.Common.API.Entities.Entities.EntityType.Member,
			RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType.Member
		}
	};

	public string FromSmartHomeLinkBinding(LinkBinding shLink)
	{
		RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType linkType = shLink.LinkType;
		if (entityLinkPatternMapping.ContainsKey(linkType))
		{
			return string.Format(entityLinkPatternMapping[linkType], shLink.EntityId);
		}
		logger.LogAndThrow<ArgumentException>($"No mapping for SH link entity of type {linkType}");
		return null;
	}

	public LinkBinding ToSmartHomeLinkBinding(string apiLink)
	{
		SmartHome.Common.API.Entities.Entities.EntityType entityType = apiLink.GetEntityType();
		if (entityTypeMapping.ContainsKey(entityType))
		{
			LinkBinding linkBinding = new LinkBinding();
			linkBinding.LinkType = entityTypeMapping[entityType];
			linkBinding.EntityId = apiLink.GetUniqueIdentifier(entityType);
			return linkBinding;
		}
		logger.LogAndThrow<ArgumentException>($"No mapping for API link entity of type {entityType}");
		return null;
	}
}
