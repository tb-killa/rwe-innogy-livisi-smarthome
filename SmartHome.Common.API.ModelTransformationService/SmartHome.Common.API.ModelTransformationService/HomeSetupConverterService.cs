using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.Entities.Extensions;
using SmartHome.Common.API.ModelTransformationService.GenericConverters;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService;

public class HomeSetupConverterService : IHomeSetupConverterService
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(HomeConverterService));

	private readonly List<string> shcHomeAttributes = new List<string> { "Name" };

	public SmartHome.Common.API.Entities.Entities.HomeSetup ToApiEntity(RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots.HomeSetup shcHomeSetup)
	{
		SmartHome.Common.API.Entities.Entities.HomeSetup homeSetup = new SmartHome.Common.API.Entities.Entities.HomeSetup();
		homeSetup.Id = shcHomeSetup.Id.ToString("N");
		homeSetup.HomeId = shcHomeSetup.HomeId.ToString("N");
		homeSetup.Tags = ToApiTags(shcHomeSetup.Tags);
		homeSetup.Config = ToApiHomeSetupConfig(shcHomeSetup);
		return homeSetup;
	}

	private static List<SmartHome.Common.API.Entities.Entities.Property> ToApiTags(List<Tag> tags)
	{
		if (tags == null)
		{
			return new List<SmartHome.Common.API.Entities.Entities.Property>();
		}
		return tags.ConvertAll((Tag x) => new SmartHome.Common.API.Entities.Entities.Property
		{
			Name = x.Name,
			Value = x.Value
		});
	}

	private static List<SmartHome.Common.API.Entities.Entities.Property> ToApiHomeSetupConfig(RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots.HomeSetup shcHomeSetup)
	{
		List<SmartHome.Common.API.Entities.Entities.Property> list = new List<SmartHome.Common.API.Entities.Entities.Property>();
		list.Add(new SmartHome.Common.API.Entities.Entities.Property
		{
			Name = "Name",
			Value = shcHomeSetup.Name
		});
		List<SmartHome.Common.API.Entities.Entities.Property> list2 = list;
		if (shcHomeSetup.Properties != null)
		{
			IEnumerable<SmartHome.Common.API.Entities.Entities.Property> collection = shcHomeSetup.Properties.Select((RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property x) => PropertyConverter.ToApiProperty(x));
			list2.AddRange(collection);
		}
		return list2;
	}

	public RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots.HomeSetup ToShcEntity(SmartHome.Common.API.Entities.Entities.HomeSetup apiHomeSetup)
	{
		try
		{
			logger.Debug($"Converting API HomeSetup entity with Id: {apiHomeSetup.Id}");
			RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots.HomeSetup homeSetup = new RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots.HomeSetup();
			homeSetup.Id = ((apiHomeSetup.Id == null) ? Guid.Empty : apiHomeSetup.Id.ToGuid());
			homeSetup.HomeId = ((apiHomeSetup.HomeId == null) ? Guid.Empty : apiHomeSetup.HomeId.ToGuid());
			homeSetup.Properties = ToShcHomeProperties(apiHomeSetup);
			homeSetup.Tags = ToShcTags(apiHomeSetup.Tags);
			RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots.HomeSetup homeSetup2 = homeSetup;
			if (apiHomeSetup.Config != null)
			{
				homeSetup2.Name = apiHomeSetup.Config.FirstOrDefault((SmartHome.Common.API.Entities.Entities.Property x) => x.Name.EqualsIgnoreCase("Name"))?.Value.ToString();
			}
			return homeSetup2;
		}
		catch (Exception)
		{
			logger.LogAndThrow<ArgumentException>(string.Format("Cannot convert API HomeSetup to SmartHome: {0", apiHomeSetup.Id));
			return null;
		}
	}

	private static List<Tag> ToShcTags(List<SmartHome.Common.API.Entities.Entities.Property> properties)
	{
		if (properties == null)
		{
			return new List<Tag>();
		}
		return properties.ConvertAll((SmartHome.Common.API.Entities.Entities.Property x) => new Tag
		{
			Name = x.Name,
			Value = x.Value.ToString()
		});
	}

	private List<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property> ToShcHomeProperties(SmartHome.Common.API.Entities.Entities.HomeSetup apiHome)
	{
		if (apiHome.Config == null)
		{
			return null;
		}
		try
		{
			return (from x in apiHome.Config
				where !shcHomeAttributes.Contains(x.Name, StringComparer.InvariantCultureIgnoreCase)
				select PropertyConverter.ToSmartHomeProperty(x)).ToList();
		}
		catch (MissingFieldException ex)
		{
			logger.Error(ex.Message);
			throw;
		}
	}
}
