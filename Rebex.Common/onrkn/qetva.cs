using System;

namespace onrkn;

internal struct qetva : IEquatable<qetva>
{
	public uint[] naxcq;

	public sxztb<uint> lssrg;

	public bool azxdc => lssrg != null;

	public void mymvf()
	{
		if (lssrg != null && 0 == 0)
		{
			Array.Clear(naxcq, 0, naxcq.Length);
			lssrg.uqydw(naxcq);
			naxcq = null;
		}
	}

	public bool Equals(qetva other)
	{
		if (object.Equals(naxcq, other.naxcq) && 0 == 0)
		{
			return object.Equals(lssrg, other.lssrg);
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (object.ReferenceEquals(null, obj) && 0 == 0)
		{
			return false;
		}
		if (obj is qetva && 0 == 0)
		{
			return Equals((qetva)obj);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (((naxcq != null) ? naxcq.GetHashCode() : 0) * 397) ^ ((lssrg != null) ? lssrg.GetHashCode() : 0);
	}
}
