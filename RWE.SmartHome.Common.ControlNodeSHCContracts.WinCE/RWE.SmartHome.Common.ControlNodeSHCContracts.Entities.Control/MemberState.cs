using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;

public class MemberState : IEquatable<MemberState>
{
	public List<Property> StateProperties;

	public Guid MemberId { get; set; }

	public MemberState()
	{
		StateProperties = new List<Property>();
	}

	public bool Equals(MemberState other)
	{
		if (object.ReferenceEquals(null, other))
		{
			return false;
		}
		if (object.ReferenceEquals(this, other))
		{
			return true;
		}
		return other.StateProperties.Equals(StateProperties);
	}

	public override bool Equals(object obj)
	{
		if (object.ReferenceEquals(null, obj))
		{
			return false;
		}
		if (object.ReferenceEquals(this, obj))
		{
			return true;
		}
		if ((object)obj.GetType() != typeof(MemberState))
		{
			return false;
		}
		return Equals((MemberState)obj);
	}

	public override int GetHashCode()
	{
		int result = 0;
		if (StateProperties != null)
		{
			StateProperties.ForEach(delegate(Property p)
			{
				result = (result * 397) ^ p.GetHashCode();
			});
		}
		return result;
	}
}
