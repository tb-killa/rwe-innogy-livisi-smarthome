using System;

namespace onrkn;

internal struct otspa : IEquatable<otspa>
{
	public byte[] mjnob;

	public sxztb<byte> kowby;

	public bool ynunp => kowby != null;

	public void ankrk()
	{
		if (kowby != null && 0 == 0)
		{
			Array.Clear(mjnob, 0, mjnob.Length);
			kowby.uqydw(mjnob);
			mjnob = null;
		}
	}

	public bool Equals(otspa other)
	{
		if (object.Equals(mjnob, other.mjnob) && 0 == 0)
		{
			return object.Equals(kowby, other.kowby);
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (object.ReferenceEquals(null, obj) && 0 == 0)
		{
			return false;
		}
		if (obj is otspa && 0 == 0)
		{
			return Equals((otspa)obj);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (((mjnob != null) ? mjnob.GetHashCode() : 0) * 397) ^ ((kowby != null) ? kowby.GetHashCode() : 0);
	}
}
