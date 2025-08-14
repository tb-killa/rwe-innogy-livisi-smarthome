using System;

namespace onrkn;

internal struct vsxbg : IEquatable<vsxbg>
{
	public const int wuvgb = 3;

	[lztdu]
	private byte rhelc;

	[lztdu]
	private byte aicnj;

	[lztdu]
	private byte lpwrb;

	public byte fyibn => rhelc;

	public byte hddrw => aicnj;

	public byte glxts => lpwrb;

	public vsxbg(byte byte0, byte byte1, byte byte2)
	{
		rhelc = byte0;
		aicnj = byte1;
		lpwrb = byte2;
	}

	public static implicit operator int(vsxbg uint24Value)
	{
		int num = uint24Value.rhelc;
		num |= uint24Value.aicnj << 8;
		return num | (uint24Value.glxts << 16);
	}

	public static explicit operator vsxbg(int val)
	{
		return new vsxbg((byte)val, (byte)(val >> 8), (byte)(val >> 16));
	}

	public static implicit operator vsxbg(ushort val)
	{
		return new vsxbg((byte)val, (byte)(val >> 8), 0);
	}

	public bool Equals(vsxbg other)
	{
		if (rhelc == other.rhelc && aicnj == other.aicnj)
		{
			return lpwrb == other.lpwrb;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (object.ReferenceEquals(null, obj) && 0 == 0)
		{
			return false;
		}
		if (obj is vsxbg && 0 == 0)
		{
			return Equals((vsxbg)obj);
		}
		return false;
	}

	public override int GetHashCode()
	{
		int hashCode = rhelc.GetHashCode();
		hashCode = (hashCode * 397) ^ aicnj.GetHashCode();
		return (hashCode * 397) ^ lpwrb.GetHashCode();
	}

	public override string ToString()
	{
		return brgjd.edcru("Uint24: {0}", (int)this);
	}
}
