using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.Entities.Extensions;
using SmartHome.Common.API.ModelTransformationService.Extensions;
using SmartHome.Common.API.ModelTransformationService.GenericConverters;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService;

public class HomeConverterService : IHomeConverterService
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(HomeConverterService));

	private readonly List<string> shcHomeAttributes = new List<string> { "Name" };

	public SmartHome.Common.API.Entities.Entities.Home ToApiEntity(RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots.Home shcHome)
	{
		SmartHome.Common.API.Entities.Entities.Home home = new SmartHome.Common.API.Entities.Entities.Home();
		home.Id = shcHome.Id.ToString("N");
		home.HomeSetup = string.Format("/home/setup/{0}", shcHome.HomeSetupId.ToString("N"));
		home.Tags = ToApiTags(shcHome.Tags);
		home.Members = ToApiMemberLinks(shcHome.MemberIds);
		home.Config = ToApiHomeConfig(shcHome);
		return home;
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

	private static List<string> ToApiMemberLinks(IEnumerable<Guid> memberIds)
	{
		return memberIds?.Select((Guid x) => string.Format("/home/member/{0}", x.ToString("N"))).ToList();
	}

	private static List<SmartHome.Common.API.Entities.Entities.Property> ToApiHomeConfig(RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots.Home shcHome)
	{
		List<SmartHome.Common.API.Entities.Entities.Property> list = new List<SmartHome.Common.API.Entities.Entities.Property>();
		list.Add(new SmartHome.Common.API.Entities.Entities.Property
		{
			Name = "Name",
			Value = shcHome.Name
		});
		List<SmartHome.Common.API.Entities.Entities.Property> first = list;
		List<SmartHome.Common.API.Entities.Entities.Property> second = ((shcHome.Properties == null) ? new List<SmartHome.Common.API.Entities.Entities.Property>() : shcHome.Properties.ConvertAll<SmartHome.Common.API.Entities.Entities.Property>(PropertyConverter.ToApiProperty));
		return first.Concat(second).ToList();
	}

	public RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots.Home ToShcEntity(SmartHome.Common.API.Entities.Entities.Home apiHome)
	{
		try
		{
			logger.Debug($"Converting API Home entity with Id: {apiHome.Id}");
			RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots.Home home = new RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots.Home();
			home.Id = ((apiHome.Id == null) ? Guid.Empty : apiHome.Id.ToGuid());
			home.HomeSetupId = ((apiHome.HomeSetup == null) ? Guid.Empty : apiHome.HomeSetup.GetGuid());
			home.Tags = ToShcTags(apiHome.Tags);
			home.MemberIds = ToShcMemberLinks(apiHome);
			home.Properties = ToShcHomeProperties(apiHome);
			RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots.Home home2 = home;
			if (apiHome.Config != null)
			{
				home2.Name = apiHome.Config.FirstOrDefault((SmartHome.Common.API.Entities.Entities.Property x) => x.Name.EqualsIgnoreCase("Name"))?.Value.ToString();
			}
			return home2;
		}
		catch (Exception)
		{
			logger.LogAndThrow<ArgumentException>($"Cannot convert API Home to SmartHome: {apiHome.Id}");
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

	private List<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property> ToShcHomeProperties(SmartHome.Common.API.Entities.Entities.Home apiHome)
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

	private static List<Guid> ToShcMemberLinks(SmartHome.Common.API.Entities.Entities.Home apiHome)
	{
		if (apiHome.Members == null)
		{
			return null;
		}
		return apiHome.Members.Select((string x) => x.GetGuid()).ToList();
	}

	public List<SmartHome.Common.API.Entities.Entities.Property> ToApiState(HomeState state)
	{
		if (state == null)
		{
			return null;
		}
		try
		{
			logger.Debug($"Converting Home State with Id: {state.HomeId}");
			return state.StateProperties.ConvertAll<SmartHome.Common.API.Entities.Entities.Property>(PropertyConverter.ToApiProperty);
		}
		catch (Exception)
		{
			logger.LogAndThrow<ArgumentException>($"Cannot convert SHC Home State with id {state.HomeId} to API entity.");
			return null;
		}
	}
}
