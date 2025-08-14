using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

namespace RWE.SmartHome.SHC.BusinessLogic.SetEntitiesRequestValidation.Configuration.EntityRules;

public class BaseDeviceRule : GenericDeviceRule<BaseDevice>
{
	protected override IEnumerable<Property> GetAttributesWrapper(BaseDevice entity)
	{
		List<Property> list = new List<Property>();
		list.Add(new StringProperty
		{
			Name = "Product",
			Value = entity.AppId
		});
		list.Add(new StringProperty
		{
			Name = "Type",
			Value = entity.DeviceType
		});
		list.Add(new StringProperty
		{
			Name = "Version",
			Value = entity.DeviceVersion
		});
		list.Add(new StringProperty
		{
			Name = "LocationId",
			Value = entity.LocationIdString
		});
		list.Add(new StringProperty
		{
			Name = "Manufacturer",
			Value = entity.Manufacturer
		});
		list.Add(new StringProperty
		{
			Name = "ProtocolId",
			Value = entity.ProtocolId.ToString()
		});
		list.Add(new StringProperty
		{
			Name = "SerialNumber",
			Value = entity.SerialNumber
		});
		list.Add(new GuidListProperty
		{
			Name = "LogicalDeviceIds",
			Value = (entity.LogicalDeviceIds ?? new List<Guid>()).Select((Guid x) => x).ToList()
		});
		return list;
	}

	protected override IEnumerable<Property> GetPropertiesWrapper(BaseDevice entity)
	{
		return entity.Properties.ToList();
	}
}
