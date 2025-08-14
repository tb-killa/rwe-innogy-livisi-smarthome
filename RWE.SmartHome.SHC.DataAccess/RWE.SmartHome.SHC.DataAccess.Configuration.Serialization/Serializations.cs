using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;

namespace RWE.SmartHome.SHC.DataAccess.Configuration.Serialization;

public static class Serializations
{
	private static Dictionary<Type, IEntitySerialization> serializers = new Dictionary<Type, IEntitySerialization>
	{
		{
			typeof(BaseDevice),
			new BaseDeviceSerialization()
		},
		{
			typeof(LogicalDevice),
			new LogicalDeviceSerialization()
		},
		{
			typeof(Location),
			new LocationSerialization()
		},
		{
			typeof(Interaction),
			new InteractionsSerialization()
		},
		{
			typeof(HomeSetup),
			new HomeSetupSerialization()
		},
		{
			typeof(Home),
			new HomeSerialization()
		},
		{
			typeof(Member),
			new MemberSerialization()
		}
	};

	public static IEntitySerialization GetSerialization(Type type)
	{
		if (!serializers.TryGetValue(type, out var value))
		{
			throw new ArgumentException($"Unsupported serialization for type {type}");
		}
		return value;
	}
}
