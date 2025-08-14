using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.Entities.Extensions;
using SmartHome.Common.API.ModelTransformationService.GenericConverters;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService;

internal class MemberConverterService : IMemberConverterService
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(MemberConverterService));

	private readonly List<string> shcMemberAttributes = new List<string> { "Name", "PresenceDeviceId" };

	public SmartHome.Common.API.Entities.Entities.Member ToApiEntity(RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots.Member shcMember)
	{
		logger.Debug($"Converting SHC Member entity with Id: {shcMember.Id}");
		SmartHome.Common.API.Entities.Entities.Member member = new SmartHome.Common.API.Entities.Entities.Member();
		member.Id = shcMember.Id.ToString("N");
		member.HomeId = shcMember.HomeId.ToString("N");
		member.Tags = ToApiTags(shcMember.Tags);
		member.Config = ToApiMemberConfig(shcMember);
		return member;
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

	private static List<SmartHome.Common.API.Entities.Entities.Property> ToApiMemberConfig(RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots.Member shcMember)
	{
		List<SmartHome.Common.API.Entities.Entities.Property> list = new List<SmartHome.Common.API.Entities.Entities.Property>();
		list.Add(new SmartHome.Common.API.Entities.Entities.Property
		{
			Name = "Name",
			Value = shcMember.Name
		});
		list.Add(new SmartHome.Common.API.Entities.Entities.Property
		{
			Name = "PresenceDeviceId",
			Value = shcMember.PresenceDeviceId.ToString("N")
		});
		List<SmartHome.Common.API.Entities.Entities.Property> first = list;
		List<SmartHome.Common.API.Entities.Entities.Property> second = ((shcMember.Properties == null) ? new List<SmartHome.Common.API.Entities.Entities.Property>() : shcMember.Properties.ConvertAll<SmartHome.Common.API.Entities.Entities.Property>(PropertyConverter.ToApiProperty));
		return first.Concat(second).ToList();
	}

	public RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots.Member ToShcEntity(SmartHome.Common.API.Entities.Entities.Member apiMember)
	{
		try
		{
			logger.Debug($"Converting API Member entity with Id: {apiMember.Id}");
			RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots.Member member = new RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots.Member();
			member.Id = ((apiMember.Id == null) ? Guid.Empty : apiMember.Id.ToGuid());
			member.HomeId = ((apiMember.HomeId == null) ? Guid.Empty : apiMember.HomeId.ToGuid());
			member.Properties = ToShcMemberProperties(apiMember);
			member.Tags = ToShcTags(apiMember.Tags);
			RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots.Member member2 = member;
			if (apiMember.Config != null)
			{
				member2.Name = apiMember.Config.FirstOrDefault((SmartHome.Common.API.Entities.Entities.Property x) => x.Name.EqualsIgnoreCase("Name"))?.Value.ToString();
				SmartHome.Common.API.Entities.Entities.Property property = apiMember.Config.FirstOrDefault((SmartHome.Common.API.Entities.Entities.Property x) => x.Name.EqualsIgnoreCase("PresenceDeviceId"));
				member2.PresenceDeviceId = ((property == null) ? Guid.Empty : new Guid(property.Value.ToString()));
			}
			return member2;
		}
		catch (Exception ex)
		{
			logger.LogAndThrow<ArgumentException>($"Cannot convert API Member to SmartHome: {apiMember.Id}. Error: {ex.Message}");
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

	private List<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property> ToShcMemberProperties(SmartHome.Common.API.Entities.Entities.Member apiMember)
	{
		if (apiMember.Config == null)
		{
			return null;
		}
		try
		{
			return (from x in apiMember.Config
				where !shcMemberAttributes.Contains(x.Name, StringComparer.InvariantCultureIgnoreCase)
				select PropertyConverter.ToSmartHomeProperty(x)).ToList();
		}
		catch (MissingFieldException ex)
		{
			logger.Error(ex.Message);
			throw;
		}
	}

	public List<SmartHome.Common.API.Entities.Entities.Property> ToApiState(MemberState state)
	{
		if (state == null)
		{
			return null;
		}
		try
		{
			logger.Debug($"Converting Member State with Id: {state.MemberId}");
			return state.StateProperties.ConvertAll<SmartHome.Common.API.Entities.Entities.Property>(PropertyConverter.ToApiProperty);
		}
		catch (Exception)
		{
			logger.LogAndThrow<ArgumentException>($"Cannot convert SHC Member State with id {state.MemberId} to API entity.");
			return null;
		}
	}
}
