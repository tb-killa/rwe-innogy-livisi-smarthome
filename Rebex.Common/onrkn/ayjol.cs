using System;

namespace onrkn;

internal struct ayjol : IEquatable<ayjol>
{
	public ulong[] rjqep;

	public sxztb<ulong> ciiua;

	public bool ygpwi => ciiua != null;

	public void uqjka()
	{
		if (ciiua != null && 0 == 0)
		{
			Array.Clear(rjqep, 0, rjqep.Length);
			ciiua.uqydw(rjqep);
			rjqep = null;
		}
	}

	public bool Equals(ayjol other)
	{
		if (object.Equals(rjqep, other.rjqep) && 0 == 0)
		{
			return object.Equals(ciiua, other.ciiua);
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (object.ReferenceEquals(null, obj) && 0 == 0)
		{
			return false;
		}
		if (obj is ayjol && 0 == 0)
		{
			return Equals((ayjol)obj);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (((rjqep != null) ? rjqep.GetHashCode() : 0) * 397) ^ ((ciiua != null) ? ciiua.GetHashCode() : 0);
	}
}
