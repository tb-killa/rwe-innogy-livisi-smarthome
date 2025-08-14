using System;
using System.Runtime.InteropServices;

namespace onrkn;

internal class ndewz<T0> : wqlni<T0>, IDisposable
{
	[StructLayout(LayoutKind.Auto)]
	private struct _003CtakeAsyncInner_003Ed__3 : fgyyk
	{
		public int xxdns;

		public vxvbw<T0> ektrg;

		public ndewz<T0> glsqg;

		public ddmlv kslnz;

		public exkzi bqhzr;

		private kpthf uxpsj;

		private object padqz;

		private void zhjzx()
		{
			T0 p2;
			try
			{
				bool flag = true;
				kpthf p;
				if (xxdns != 0)
				{
					bqhzr = glsqg.kdfse.qlvza(kslnz);
					p = bqhzr.avdby(p1: false).vrtmi();
					if (!p.zpafv || 1 == 0)
					{
						xxdns = 0;
						uxpsj = p;
						ektrg.xiwgo(ref p, ref this);
						flag = false;
						return;
					}
				}
				else
				{
					p = uxpsj;
					uxpsj = default(kpthf);
					xxdns = -1;
				}
				p.ekzxl();
				p = default(kpthf);
				p2 = glsqg.tfdix();
			}
			catch (Exception p3)
			{
				xxdns = -2;
				ektrg.tudwl(p3);
				return;
			}
			xxdns = -2;
			ektrg.vzyck(p2);
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in zhjzx
			this.zhjzx();
		}

		private void vwpwz(fgyyk p0)
		{
			ektrg.viwxd(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in vwpwz
			this.vwpwz(p0);
		}
	}

	private const string leodm = "Could not add item to the collection.";

	private const string alvlb = "Could not take item from the collection.";

	private readonly ogeww kdfse;

	private readonly rrckb<T0> pzppr;

	private bool auumq;

	private readonly Func<exkzi, T0> awmlb;

	public ndewz()
		: this((rrckb<T0>)new juyln<T0>())
	{
	}

	public ndewz(rrckb<T0> innerCollection)
	{
		Func<exkzi, T0> func = null;
		base._002Ector();
		if (innerCollection == null || 1 == 0)
		{
			throw new ArgumentNullException("innerCollection");
		}
		pzppr = innerCollection;
		kdfse = new ogeww(0, int.MaxValue);
		auumq = false;
		if (func == null || 1 == 0)
		{
			func = kpdnl;
		}
		awmlb = func;
	}

	public void ypdbu(T0 p0)
	{
		okzle();
		yndhi(p0);
	}

	public exkzi prhrm(T0 p0)
	{
		okzle();
		yndhi(p0);
		return rxpjc.iccat;
	}

	public njvzu<T0> ehuhm(ddmlv p0)
	{
		okzle();
		return xhpdz(p0);
	}

	public T0[] lujor()
	{
		okzle();
		return pzppr.ocxkr();
	}

	public void Dispose()
	{
		if (!auumq)
		{
			zvcde.amdpj(hmbnf);
			auumq = true;
		}
	}

	protected void okzle()
	{
		if (auumq && 0 == 0)
		{
			throw new ObjectDisposedException("SimpleAsyncProducerConsumerCollection");
		}
	}

	[vtsnh(typeof(ndewz<>._003CtakeAsyncInner_003Ed__3))]
	private njvzu<T0> xhpdz(ddmlv p0)
	{
		_003CtakeAsyncInner_003Ed__3 p1 = default(_003CtakeAsyncInner_003Ed__3);
		p1.glsqg = this;
		p1.kslnz = p0;
		p1.ektrg = vxvbw<T0>.rdzxj();
		p1.xxdns = -1;
		vxvbw<T0> ektrg = p1.ektrg;
		ektrg.vklen(ref p1);
		return p1.ektrg.xieya;
	}

	private T0 tfdix()
	{
		if (!pzppr.ztnzd(out var p) || 1 == 0)
		{
			throw new InvalidOperationException("Could not take item from the collection.");
		}
		return p;
	}

	private void yndhi(T0 p0)
	{
		if (!pzppr.pbpvr(p0) || 1 == 0)
		{
			throw new InvalidOperationException("Could not add item to the collection.");
		}
		kdfse.zvrwx();
	}

	private T0 kpdnl(exkzi p0)
	{
		return tfdix();
	}

	private void hmbnf()
	{
		kdfse.Dispose();
	}
}
