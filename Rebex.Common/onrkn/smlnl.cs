using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

namespace onrkn;

internal class smlnl<T0> : wqlni<T0>, IDisposable, uentq
{
	public struct mmakm
	{
		[lztdu]
		private T0 qbzdw;

		[lztdu]
		private bool gjlsf;

		public T0 Item
		{
			get
			{
				if (mlchw && 0 == 0)
				{
					throw new lmqll();
				}
				return qbzdw;
			}
		}

		public bool mlchw => !gjlsf;

		public bool knudq => gjlsf;

		public mmakm(T0 item)
		{
			qbzdw = item;
			gjlsf = true;
		}

		public static implicit operator mmakm(T0 item)
		{
			return new mmakm(item);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CTakeAsync_003Ed__0 : fgyyk
	{
		public int zhlvu;

		public vxvbw<T0> rwrnf;

		public smlnl<T0> vjbjo;

		public ddmlv cjbpe;

		public mmakm aytgw;

		private object aatsf;

		private object fhcus;

		private void movfe()
		{
			T0 item;
			try
			{
				bool flag = true;
				smlnl<T0> p;
				if (zhlvu != 0)
				{
					vjbjo.jkeuh();
					p = vjbjo.bltja(cjbpe).qqfch();
					if (!p.kodym || 1 == 0)
					{
						zhlvu = 0;
						aatsf = p;
						rwrnf.xiwgo(ref p, ref this);
						flag = false;
						return;
					}
				}
				else
				{
					p = (smlnl<T0>)aatsf;
					aatsf = null;
					zhlvu = -1;
				}
				mmakm mmakm = p.qrnzk();
				p = null;
				mmakm mmakm2 = mmakm;
				aytgw = mmakm2;
				item = aytgw.Item;
			}
			catch (Exception p2)
			{
				zhlvu = -2;
				rwrnf.tudwl(p2);
				return;
			}
			zhlvu = -2;
			rwrnf.vzyck(item);
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in movfe
			this.movfe();
		}

		private void zzzxo(fgyyk p0)
		{
			rwrnf.viwxd(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in zzzxo
			this.zzzxo(p0);
		}
	}

	private const string zygas = "Awaiter operation failed.";

	private readonly bool amxpp;

	private static readonly WaitCallback tpxsx;

	private static readonly Action<object> xydwo;

	private ddmlv dsxjr;

	private Action ompvt;

	private tfsbt hdkel;

	private readonly Queue<T0> nbwld;

	private bool hszwt;

	private Exception ouxmv;

	private static WaitCallback mapso;

	private static Action<object> ekdgx;

	public bool kodym
	{
		get
		{
			jkeuh();
			lock (nbwld)
			{
				return nbwld.Count > 0;
			}
		}
	}

	public smlnl()
		: this(tryInlineContinuations: false)
	{
	}

	public smlnl(bool tryInlineContinuations)
	{
		amxpp = tryInlineContinuations;
		nbwld = new Queue<T0>();
		dsxjr = ddmlv.prdik;
		ompvt = null;
		hszwt = false;
		ouxmv = null;
	}

	public void ypdbu(T0 p0)
	{
		jkeuh();
		dqxau(p0);
	}

	public exkzi prhrm(T0 p0)
	{
		jkeuh();
		dqxau(p0);
		return rxpjc.iccat;
	}

	[vtsnh(typeof(smlnl<>._003CTakeAsync_003Ed__0))]
	public njvzu<T0> ehuhm(ddmlv p0)
	{
		_003CTakeAsync_003Ed__0 p1 = default(_003CTakeAsync_003Ed__0);
		p1.vjbjo = this;
		p1.cjbpe = p0;
		p1.rwrnf = vxvbw<T0>.rdzxj();
		p1.zhlvu = -1;
		vxvbw<T0> rwrnf = p1.rwrnf;
		rwrnf.vklen(ref p1);
		return p1.rwrnf.xieya;
	}

	public smlnl<T0> bltja(ddmlv p0)
	{
		jkeuh();
		dsxjr = p0;
		return this;
	}

	public T0[] lujor()
	{
		jkeuh();
		lock (nbwld)
		{
			return nbwld.ToArray();
		}
	}

	public void Dispose()
	{
		if (!hszwt)
		{
			zvcde.amdpj(kqfom);
			hszwt = true;
		}
	}

	protected void jkeuh()
	{
		if (hszwt && 0 == 0)
		{
			throw new ObjectDisposedException("SimpleAsyncProducerConsumerCollection");
		}
	}

	private void dqxau(T0 p0)
	{
		tyxcs(this, p0, p2: true);
	}

	public smlnl<T0> qqfch()
	{
		return this;
	}

	public mmakm qrnzk()
	{
		jkeuh();
		try
		{
			lock (nbwld)
			{
				if (ouxmv != null && 0 == 0)
				{
					throw new InvalidOperationException("Awaiter operation failed.", ouxmv);
				}
				if (dsxjr.bhxda && 0 == 0)
				{
					return default(mmakm);
				}
				return mbmtt();
			}
		}
		catch (lmqll)
		{
			throw;
		}
		catch (Exception)
		{
			throw;
		}
		finally
		{
			ouxmv = null;
			if (dsxjr.mjrqr && 0 == 0)
			{
				hdkel.Dispose();
			}
		}
	}

	public void vcbew(Action p0)
	{
		jkeuh();
		bool flag = false;
		lock (nbwld)
		{
			if (nbwld.Count > 0)
			{
				flag = true;
			}
		}
		if (flag && 0 == 0)
		{
			p0();
			return;
		}
		lock (nbwld)
		{
			try
			{
				ompvt = p0;
				if (dsxjr.mjrqr && 0 == 0)
				{
					hdkel = dsxjr.kjwdi(xydwo, this);
				}
			}
			catch (Exception ex)
			{
				ouxmv = ex;
			}
		}
		tyxcs(this, default(T0), p2: false);
	}

	private static void tyxcs(smlnl<T0> p0, T0 p1, bool p2)
	{
		Action action = null;
		bool flag = false;
		try
		{
			lock (p0.nbwld)
			{
				try
				{
					flag = p0.ompvt != null && 0 == 0 && (p0.nbwld.Count > 0 || p2);
					action = p0.ompvt;
					if (p2 && 0 == 0)
					{
						p0.nbwld.Enqueue(p1);
					}
					if (action == null || false || p0.nbwld.Count == 0 || 1 == 0)
					{
						return;
					}
				}
				catch (Exception)
				{
					flag = true;
					throw;
				}
				finally
				{
					if (flag && 0 == 0)
					{
						p0.ompvt = null;
					}
				}
			}
			if (p0.amxpp && 0 == 0)
			{
				Action action2 = action;
				action = null;
				action2();
			}
			else
			{
				ThreadPool.QueueUserWorkItem(tpxsx, action);
			}
		}
		catch (Exception ex2)
		{
			p0.ouxmv = ex2;
			if (action != null && 0 == 0)
			{
				ThreadPool.QueueUserWorkItem(tpxsx, action);
			}
		}
	}

	private T0 mbmtt()
	{
		return nbwld.Dequeue();
	}

	private static void mmuzt(smlnl<T0> p0)
	{
		Action action = null;
		bool flag = false;
		try
		{
			lock (p0.nbwld)
			{
				try
				{
					flag = p0.ompvt != null;
					action = p0.ompvt;
					if (action != null)
					{
						if (p0.amxpp && 0 == 0)
						{
							Action action2 = action;
							action = null;
							action2();
						}
						else
						{
							ThreadPool.QueueUserWorkItem(tpxsx, action);
						}
					}
				}
				finally
				{
					if (flag && 0 == 0)
					{
						p0.ompvt = null;
					}
				}
			}
		}
		catch (Exception ex)
		{
			p0.ouxmv = ex;
			if (action != null && 0 == 0)
			{
				ThreadPool.QueueUserWorkItem(tpxsx, action);
			}
		}
	}

	private void kqfom()
	{
		hdkel.Dispose();
	}

	static smlnl()
	{
		if (mapso == null || 1 == 0)
		{
			mapso = emvny;
		}
		tpxsx = mapso;
		if (ekdgx == null || 1 == 0)
		{
			ekdgx = vueml;
		}
		xydwo = ekdgx;
	}

	private static void emvny(object p0)
	{
		((Action)p0)();
	}

	private static void vueml(object p0)
	{
		mmuzt((smlnl<T0>)p0);
	}
}
