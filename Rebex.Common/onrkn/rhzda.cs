using System;

namespace onrkn;

internal abstract class rhzda<T0> : IDisposable
{
	private int baxcp;

	private readonly T0[] lsoxi;

	private readonly object lszuh = new object();

	protected rhzda(int maxSize)
	{
		lsoxi = new T0[maxSize];
	}

	protected virtual void gdvxj(T0 p0)
	{
	}

	protected abstract T0 qbuja();

	protected virtual void cjyoc(T0 p0)
	{
	}

	public void Dispose()
	{
		lock (lszuh)
		{
			while (baxcp > 0)
			{
				T0 p = lsoxi[--baxcp];
				cjyoc(p);
			}
		}
	}

	public T0 tbdcs()
	{
		lock (lszuh)
		{
			if (baxcp > 0)
			{
				return lsoxi[--baxcp];
			}
		}
		return qbuja();
	}

	public ihlqx<T0> ynzkw()
	{
		T0 value = tbdcs();
		return new ihlqx<T0>(value, wkkog);
	}

	public void wkkog(T0 p0)
	{
		gdvxj(p0);
		bool flag = false;
		lock (lszuh)
		{
			if (baxcp < lsoxi.Length)
			{
				lsoxi[baxcp++] = p0;
			}
			else
			{
				flag = true;
			}
		}
		if (flag && 0 == 0)
		{
			cjyoc(p0);
		}
	}

	public bool myplq(out T0 p0)
	{
		lock (lszuh)
		{
			if (baxcp > 0)
			{
				p0 = lsoxi[--baxcp];
				return true;
			}
			p0 = default(T0);
			return false;
		}
	}

	public bool sycwr(out ihlqx<T0> p0)
	{
		T0 p1;
		bool flag = myplq(out p1);
		p0 = ((flag ? true : false) ? new ihlqx<T0>(p1, wkkog) : null);
		return flag;
	}
}
