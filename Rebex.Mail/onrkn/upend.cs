using System.Collections;
using System.Collections.Generic;

namespace onrkn;

internal class upend<T0> : IEnumerable<T0>, IEnumerable
{
	private readonly jfxnb vwvyi;

	private readonly List<T0> hvgkh;

	private bool qxtfi;

	internal bool akmjj
	{
		get
		{
			return qxtfi;
		}
		set
		{
			qxtfi = value;
		}
	}

	protected internal jfxnb bpotr => vwvyi;

	protected List<T0> cdyle => hvgkh;

	public int fnqqt => cdyle.Count;

	public T0 this[int index] => cdyle[index];

	protected internal upend(jfxnb owner)
	{
		vwvyi = owner;
		hvgkh = new List<T0>();
	}

	internal static qacae gxdxg(upend<qacae> p0)
	{
		if (p0 == null || false || p0.fnqqt == 0 || 1 == 0)
		{
			return null;
		}
		return p0[0];
	}

	internal static string nowso(upend<qacae> p0)
	{
		qacae qacae2 = gxdxg(p0);
		if (qacae2 == null || 1 == 0)
		{
			return null;
		}
		return qacae2.tgbhs as string;
	}

	internal virtual T0 oaesx(T0 p0)
	{
		cdyle.Add(p0);
		akmjj = true;
		return p0;
	}

	internal virtual bool plpge(T0 p0)
	{
		bool flag = cdyle.Remove(p0);
		if (flag && 0 == 0)
		{
			akmjj = true;
		}
		return flag;
	}

	internal virtual T0 rckpv(int p0)
	{
		T0 result = cdyle[p0];
		cdyle.RemoveAt(p0);
		akmjj = true;
		return result;
	}

	internal virtual void vmjwa()
	{
		cdyle.Clear();
		akmjj = true;
	}

	public IEnumerator<T0> GetEnumerator()
	{
		return cdyle.GetEnumerator();
	}

	private IEnumerator fpagr()
	{
		return cdyle.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		//ILSpy generated this explicit interface implementation from .override directive in fpagr
		return this.fpagr();
	}
}
