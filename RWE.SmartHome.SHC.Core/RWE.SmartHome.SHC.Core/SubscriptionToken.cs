using System;

namespace RWE.SmartHome.SHC.Core;

public sealed class SubscriptionToken : IEquatable<SubscriptionToken>
{
	private readonly Guid token;

	public SubscriptionToken()
	{
		token = Guid.NewGuid();
	}

	public bool Equals(SubscriptionToken other)
	{
		if (other == null)
		{
			return false;
		}
		return object.Equals(token, other.token);
	}

	public override bool Equals(object obj)
	{
		if (object.ReferenceEquals(this, obj))
		{
			return true;
		}
		return Equals(obj as SubscriptionToken);
	}

	public override int GetHashCode()
	{
		return token.GetHashCode();
	}
}
