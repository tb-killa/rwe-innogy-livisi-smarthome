using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

namespace RWE.SmartHome.SHC.BusinessLogic.SetEntitiesRequestValidation;

public class GuidListProperty : Property
{
	public List<Guid> Value { get; set; }

	protected override Property CreateClone()
	{
		GuidListProperty guidListProperty = new GuidListProperty();
		guidListProperty.Value = Value.Select((Guid x) => x).ToList();
		return guidListProperty;
	}

	public override bool Equals(Property other)
	{
		if (object.ReferenceEquals(this, other))
		{
			return true;
		}
		GuidListProperty guidListProperty = other as GuidListProperty;
		if (object.ReferenceEquals(null, guidListProperty))
		{
			return false;
		}
		if (guidListProperty.Name == base.Name)
		{
			if (guidListProperty.Value != null || Value != null)
			{
				return guidListProperty.Value.SequenceEqual(Value);
			}
			return true;
		}
		return false;
	}

	public override IComparable GetValueAsComparable()
	{
		throw new NotImplementedException("no comparable for this list");
	}

	public override string GetValueAsString()
	{
		if (Value != null)
		{
			return string.Join(",", Value.Select((Guid x) => x.ToString()).ToArray());
		}
		return "null";
	}
}
