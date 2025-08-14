using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using SmartHome.SHC.API.Configuration;

namespace RWE.SmartHome.SHC.CoreApiConverters;

public static class EntityTypeConverter
{
	public static LinkType ToApi(this EntityType linkType)
	{
		return linkType switch
		{
			EntityType.Product => LinkType.Product, 
			EntityType.BaseDevice => LinkType.Device, 
			EntityType.LogicalDevice => LinkType.Capability, 
			_ => throw new InvalidCastException("Invalid core link type"), 
		};
	}

	public static EntityType ToCore(this LinkType linkType)
	{
		return linkType switch
		{
			LinkType.Product => EntityType.Product, 
			LinkType.Device => EntityType.BaseDevice, 
			LinkType.Capability => EntityType.LogicalDevice, 
			_ => throw new InvalidCastException("Invalid api link type"), 
		};
	}
}
