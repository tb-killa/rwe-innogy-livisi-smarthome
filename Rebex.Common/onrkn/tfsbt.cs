using System;
using System.Threading;

namespace onrkn;

internal struct tfsbt : IDisposable, IEquatable<tfsbt>
{
	private static int ssioh = 1;

	[lztdu]
	private znuay xugpf;

	[lztdu]
	private object ymbzb;

	[lztdu]
	private Action sybiq;

	[lztdu]
	private Action<object> zvcln;

	[lztdu]
	private int fctja;

	internal tfsbt(Action action0, znuay cts)
	{
		zvcln = null;
		sybiq = action0;
		ymbzb = null;
		fctja = Interlocked.Increment(ref ssioh);
		xugpf = cts;
	}

	internal tfsbt(Action<object> action1, object state, znuay cts)
	{
		sybiq = null;
		zvcln = action1;
		ymbzb = state;
		fctja = Interlocked.Increment(ref ssioh);
		xugpf = cts;
	}

	internal void ratek()
	{
		if (sybiq != null && 0 == 0)
		{
			sybiq();
		}
		else
		{
			zvcln(ymbzb);
		}
	}

	public void Dispose()
	{
		if (xugpf != null && 0 == 0)
		{
			xugpf.chzfq(this);
		}
	}

	public bool Equals(tfsbt other)
	{
		if (fctja == other.fctja)
		{
			return object.Equals(xugpf, other.xugpf);
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (object.ReferenceEquals(null, obj) && 0 == 0)
		{
			return false;
		}
		if (obj is tfsbt && 0 == 0)
		{
			return Equals((tfsbt)obj);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (fctja * 397) ^ ((xugpf != null) ? xugpf.GetHashCode() : 0);
	}

	public static bool operator ==(tfsbt left, tfsbt right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(tfsbt left, tfsbt right)
	{
		return !left.Equals(right);
	}
}
