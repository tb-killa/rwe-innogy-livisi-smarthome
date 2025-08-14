using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

namespace RWE.SmartHome.SHC.RuleEngine;

public class EntityIdentificationData : IComparable
{
	public string EntityId { get; set; }

	public EntityType EntityType { get; set; }

	public int CompareTo(object obj)
	{
		if (!(obj is EntityIdentificationData entityIdentificationData))
		{
			throw new InvalidOperationException($"Incompatible object types: {GetType().Name} - {obj.GetType().Name}");
		}
		if (EntityType == entityIdentificationData.EntityType && EntityId == entityIdentificationData.EntityId)
		{
			return 0;
		}
		return 1;
	}
}
