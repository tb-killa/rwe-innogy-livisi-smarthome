using System;

namespace onrkn;

internal class wvrwq<T0> : IDisposable
{
	public delegate void mphde(T0 state);

	private sealed class wfgdf
	{
		public wvrwq<T0> vruaf;

		public Func<T0, mphde, exkzi> mmjlk;

		public exkzi fqmiy()
		{
			T0 estli;
			lock (vruaf.cbubo)
			{
				estli = vruaf.estli;
			}
			return mmjlk(estli, vruaf.feuib);
		}
	}

	private sealed class wxotf<T1>
	{
		public wvrwq<T0> wpizn;

		public Func<T0, mphde, njvzu<T1>> getzy;

		public njvzu<T1> aqowv()
		{
			T0 estli;
			lock (wpizn.cbubo)
			{
				estli = wpizn.estli;
			}
			return getzy(estli, wpizn.feuib);
		}
	}

	private readonly ivvyi cbubo = new ivvyi();

	private T0 estli;

	public T0 xjfxx
	{
		get
		{
			lock (cbubo)
			{
				return estli;
			}
		}
	}

	public wvrwq(T0 initState)
	{
		feuib(initState);
	}

	public void Dispose()
	{
		cbubo.Dispose();
	}

	public exkzi wvvao(Func<T0, mphde, exkzi> p0)
	{
		wfgdf wfgdf = new wfgdf();
		wfgdf.mmjlk = p0;
		wfgdf.vruaf = this;
		return cbubo.dhzqc(wfgdf.fqmiy);
	}

	public njvzu<T> jopnb<T>(Func<T0, mphde, njvzu<T>> p0)
	{
		wxotf<T> wxotf = new wxotf<T>();
		wxotf.getzy = p0;
		wxotf.wpizn = this;
		return cbubo.heurk(wxotf.aqowv);
	}

	private void feuib(T0 p0)
	{
		lock (cbubo)
		{
			estli = p0;
		}
	}
}
