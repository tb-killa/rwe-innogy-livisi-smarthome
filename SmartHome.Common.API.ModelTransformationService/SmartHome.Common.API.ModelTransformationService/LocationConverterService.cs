using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.Entities.Extensions;
using SmartHome.Common.API.ModelTransformationService.Extensions;

namespace SmartHome.Common.API.ModelTransformationService;

public class LocationConverterService : ILocationConverterService
{
	public SmartHome.Common.API.Entities.Entities.Location FromSmartHomeLocation(RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots.Location location)
	{
		SmartHome.Common.API.Entities.Entities.Location location2 = new SmartHome.Common.API.Entities.Entities.Location();
		location2.Id = location.Id.ToString("N");
		location2.Config = new List<SmartHome.Common.API.Entities.Entities.Property>
		{
			new SmartHome.Common.API.Entities.Entities.Property
			{
				Name = "Name",
				Value = location.Name
			},
			new SmartHome.Common.API.Entities.Entities.Property
			{
				Name = "Type",
				Value = location.RoomType.ToString()
			}
		};
		SmartHome.Common.API.Entities.Entities.Location location3 = location2;
		if (location.Tags != null && location.Tags.Any())
		{
			location3.Tags = location.Tags.ConvertAll((Tag t) => new SmartHome.Common.API.Entities.Entities.Property
			{
				Name = t.Name,
				Value = t.Value
			});
		}
		return location3;
	}

	public RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots.Location ToSmartHomeLocation(SmartHome.Common.API.Entities.Entities.Location location)
	{
		RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots.Location location2 = new RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots.Location();
		location2.Id = location.Id.ToGuid();
		location2.Name = location.Config.GetPropertyValue<string>("Name");
		location2.RoomType = location.Config.GetPropertyValue<RoomType>("Type");
		RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots.Location location3 = location2;
		if (location.Tags != null && location.Tags.Any())
		{
			location3.Tags = location.Tags.ConvertAll((SmartHome.Common.API.Entities.Entities.Property p) => new Tag
			{
				Name = p.Name,
				Value = p.Value.ToString()
			});
		}
		return location3;
	}
}
