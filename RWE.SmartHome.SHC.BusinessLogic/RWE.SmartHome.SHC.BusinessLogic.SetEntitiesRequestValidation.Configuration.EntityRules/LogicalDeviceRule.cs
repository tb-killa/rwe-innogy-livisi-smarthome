using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

namespace RWE.SmartHome.SHC.BusinessLogic.SetEntitiesRequestValidation.Configuration.EntityRules;

public class LogicalDeviceRule : GenericDeviceRule<LogicalDevice>
{
	protected override IEnumerable<Property> GetAttributesWrapper(LogicalDevice entity)
	{
		List<Property> list = new List<Property>();
		list.Add(new StringProperty
		{
			Name = "BaseDeviceId",
			Value = entity.BaseDeviceId.ToString()
		});
		list.Add(new StringProperty
		{
			Name = "Type",
			Value = entity.DeviceType
		});
		list.Add(new StringProperty
		{
			Name = "PrimaryPropertyName",
			Value = entity.PrimaryPropertyName
		});
		List<Property> list2 = list;
		if (entity.ActivityLogActive.HasValue)
		{
			list2.Add(new BooleanProperty
			{
				Name = "ActivityLogActive",
				Value = entity.ActivityLogActive
			});
		}
		return list2;
	}

	protected override IEnumerable<Property> GetPropertiesWrapper(LogicalDevice entity)
	{
		return entity.Properties.ToList();
	}
}
