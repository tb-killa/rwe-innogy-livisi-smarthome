using System;
using System.Collections.Generic;

namespace onrkn;

internal struct apajk<T0>
{
	private sealed class qwvud
	{
		public apajk<T0> xhjhy;

		public apajk<T0> cweyb(T0 p0)
		{
			return xhjhy;
		}
	}

	[lztdu]
	private static apajk<T0> ifotu = default(apajk<T0>);

	[lztdu]
	private bool usdtz;

	[lztdu]
	private T0 mqved;

	private static Func<T0, T0> plmyf;

	public bool nbnot => !usdtz;

	public bool njpdj => usdtz;

	public static apajk<T0> uceou => ifotu;

	public apajk(T0 value)
	{
		usdtz = true;
		mqved = value;
	}

	public T0 mzanw()
	{
		if (nbnot && 0 == 0)
		{
			throw new InvalidOperationException("Optional is empty (none).");
		}
		return mqved;
	}

	public apajk<T0> ocgqf(Func<apajk<T0>> p0)
	{
		qwvud qwvud = new qwvud();
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("elseFunc");
		}
		qwvud.xhjhy = this;
		return jqaxc(qwvud.cweyb, p0);
	}

	public apajk<T0> baggx(apajk<T0> p0)
	{
		if (!njpdj || 1 == 0)
		{
			return p0;
		}
		return mqved;
	}

	public T0 bbtvl(Func<T0> p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("elseFunc");
		}
		if (plmyf == null || 1 == 0)
		{
			plmyf = lzsde;
		}
		return jqaxc(plmyf, p0);
	}

	public T0 xjbxa(T0 p0)
	{
		if (!njpdj || 1 == 0)
		{
			return p0;
		}
		return mqved;
	}

	public TReturn jqaxc<TReturn>(Func<T0, TReturn> p0, Func<TReturn> p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("someFunc");
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("noneFunc");
		}
		if (!njpdj || 1 == 0)
		{
			return p1();
		}
		return p0(mzanw());
	}

	public void sfqmp(Action<T0> p0, Action p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("someAction");
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("noneAction");
		}
		if (njpdj && 0 == 0)
		{
			p0(mzanw());
		}
		else
		{
			p1();
		}
	}

	public override int GetHashCode()
	{
		if (!nbnot || 1 == 0)
		{
			return EqualityComparer<T0>.Default.GetHashCode(mqved);
		}
		return 0;
	}

	public static bool operator ==(apajk<T0> left, apajk<T0> right)
	{
		return left.bhkpl(right);
	}

	public static bool operator !=(apajk<T0> left, apajk<T0> right)
	{
		return !left.bhkpl(right);
	}

	public static implicit operator apajk<T0>(T0 value)
	{
		if (value != null && 0 == 0)
		{
			return new apajk<T0>(value);
		}
		return uceou;
	}

	public static implicit operator apajk<T0>(vdffb<T0> some)
	{
		return new apajk<T0>(some.yzhuz);
	}

	public static implicit operator apajk<T0>(ampki none)
	{
		return uceou;
	}

	public bool bhkpl(apajk<T0> p0)
	{
		if (nbnot && 0 == 0 && p0.nbnot && 0 == 0)
		{
			return true;
		}
		if ((nbnot ? true : false) || p0.nbnot)
		{
			return false;
		}
		return EqualityComparer<T0>.Default.Equals(mqved, p0.mqved);
	}

	public override bool Equals(object obj)
	{
		if (object.ReferenceEquals(null, obj) && 0 == 0)
		{
			return false;
		}
		if ((object)obj.GetType() != GetType())
		{
			return false;
		}
		return bhkpl((apajk<T0>)obj);
	}

	public override string ToString()
	{
		if (!njpdj || 1 == 0)
		{
			return "<None>";
		}
		return brgjd.edcru("Some {0}", mzanw());
	}

	private static T0 lzsde(T0 p0)
	{
		return p0;
	}
}
