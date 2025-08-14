using System;
using System.Runtime.InteropServices;

namespace onrkn;

internal struct rnqdw : IEquatable<rnqdw>
{
	private const string jmodx = "Could not cast chained (non-contiguous) ArrayView to PinnedByteArraySpan. Consider using Coalesce method before cast.";

	private readonly byte[] trzqh;

	private readonly int zyyvq;

	private readonly int bfsfi;

	private GCHandle skroh;

	private IntPtr xijdm;

	public byte[] esych => trzqh;

	public int ugrtb => bfsfi;

	public int hrzld => zyyvq;

	public rnqdw(byte[] innerArray)
		: this(innerArray, 0, innerArray.Length)
	{
	}

	public rnqdw(byte[] innerArray, int startIndex)
		: this(innerArray, startIndex, innerArray.Length - startIndex)
	{
	}

	public rnqdw(byte[] innerArray, int startIndex, int length)
	{
		this = default(rnqdw);
		if (innerArray == null || 1 == 0)
		{
			throw new ArgumentNullException("innerArray");
		}
		if (startIndex < 0)
		{
			throw new ArgumentOutOfRangeException("startIndex");
		}
		if (length < 0)
		{
			throw new ArgumentOutOfRangeException("length");
		}
		bfsfi = startIndex;
		trzqh = innerArray;
		zyyvq = Math.Min(length, innerArray.Length - startIndex);
		if (zyyvq < 0)
		{
			throw new ArgumentException("startIndex, length");
		}
		miksr();
	}

	public void fbdzt()
	{
		skroh.Free();
	}

	public IntPtr peara()
	{
		return xijdm;
	}

	public bool Equals(rnqdw other)
	{
		if (object.ReferenceEquals(trzqh, other.trzqh) && 0 == 0 && zyyvq == other.zyyvq)
		{
			return bfsfi == other.bfsfi;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (object.ReferenceEquals(null, obj) && 0 == 0)
		{
			return false;
		}
		if (obj is rnqdw && 0 == 0)
		{
			return Equals((rnqdw)obj);
		}
		return false;
	}

	public override int GetHashCode()
	{
		int hashCode = trzqh.GetHashCode();
		hashCode = (hashCode * 397) ^ zyyvq;
		return (hashCode * 397) ^ bfsfi;
	}

	public static implicit operator nxtme<byte>(rnqdw arraySpan)
	{
		return new nxtme<byte>(arraySpan.esych, arraySpan.ugrtb, arraySpan.hrzld);
	}

	public static implicit operator rnqdw(nxtme<byte> arrayView)
	{
		if (arrayView.wzdji && 0 == 0)
		{
			throw new InvalidOperationException("Could not cast chained (non-contiguous) ArrayView to PinnedByteArraySpan. Consider using Coalesce method before cast.");
		}
		return new rnqdw(arrayView.lthjd, arrayView.frlfs, arrayView.tvoem);
	}

	public static bool operator ==(rnqdw left, rnqdw right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(rnqdw left, rnqdw right)
	{
		return !left.Equals(right);
	}

	private void miksr()
	{
		skroh = GCHandle.Alloc(trzqh, GCHandleType.Pinned);
		xijdm = new IntPtr(skroh.AddrOfPinnedObject().ToInt64() + ugrtb);
	}
}
