using System;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapterInterfaces;

public class SwitchCondition : IEquatable<SwitchCondition>
{
	public ProfileAction UnconditionalAction { get; private set; }

	public ProfileAction GreaterThanAction { get; private set; }

	public ProfileAction LowerThanAction { get; private set; }

	public int? ComparisonValue { get; private set; }

	public ProfileAction Evaluate(int? decisionValue, bool isLongPress)
	{
		if (!ComparisonValue.HasValue || !decisionValue.HasValue)
		{
			return UnconditionalAction;
		}
		if (decisionValue > ComparisonValue)
		{
			return GreaterThanAction;
		}
		if (decisionValue < ComparisonValue)
		{
			return LowerThanAction;
		}
		return ProfileAction.NoAction;
	}

	public SwitchCondition(ProfileAction unconditionalAction, ProfileAction greaterThanAction, ProfileAction lowerThanAction, int? comparisonValue)
	{
		UnconditionalAction = unconditionalAction;
		GreaterThanAction = greaterThanAction;
		LowerThanAction = lowerThanAction;
		ComparisonValue = comparisonValue;
	}

	public bool Equals(SwitchCondition other)
	{
		if (object.ReferenceEquals(null, other))
		{
			return false;
		}
		if (object.ReferenceEquals(this, other))
		{
			return true;
		}
		if (object.Equals(other.UnconditionalAction, UnconditionalAction) && object.Equals(other.GreaterThanAction, GreaterThanAction) && object.Equals(other.LowerThanAction, LowerThanAction))
		{
			return other.ComparisonValue.Equals(ComparisonValue);
		}
		return false;
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
		if ((object)obj.GetType() != typeof(SwitchCondition))
		{
			return false;
		}
		return Equals((SwitchCondition)obj);
	}

	public override int GetHashCode()
	{
		int hashCode = UnconditionalAction.GetHashCode();
		hashCode = (hashCode * 397) ^ GreaterThanAction.GetHashCode();
		hashCode = (hashCode * 397) ^ LowerThanAction.GetHashCode();
		return (hashCode * 397) ^ (ComparisonValue.HasValue ? ComparisonValue.Value : 0);
	}

	public static bool operator ==(SwitchCondition left, SwitchCondition right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(SwitchCondition left, SwitchCondition right)
	{
		return !object.Equals(left, right);
	}
}
