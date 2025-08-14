using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace onrkn;

internal class hjkmq<T0> : rwygu
{
	[StructLayout(LayoutKind.Auto)]
	private struct _003CRequestShutdown_003Ed__0 : fgyyk
	{
		public int ggymk;

		public ljmxa sxouk;

		public hjkmq<T0> dsomr;

		private kpthf vgptz;

		private object rkuwa;

		private void pglkp()
		{
			try
			{
				bool flag = true;
				kpthf p;
				if (ggymk != 0)
				{
					p = dsomr.czebc(zvcde.zymew).avdby(p1: false).vrtmi();
					if (!p.zpafv || 1 == 0)
					{
						ggymk = 0;
						vgptz = p;
						sxouk.wqiyk(ref p, ref this);
						flag = false;
						return;
					}
				}
				else
				{
					p = vgptz;
					vgptz = default(kpthf);
					ggymk = -1;
				}
				p.ekzxl();
				p = default(kpthf);
			}
			catch (Exception p2)
			{
				ggymk = -2;
				sxouk.iurqb(p2);
				return;
			}
			ggymk = -2;
			sxouk.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in pglkp
			this.pglkp();
		}

		private void flren(fgyyk p0)
		{
			sxouk.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in flren
			this.flren(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CGetChannelState_003Ed__3 : fgyyk
	{
		public int hrqlz;

		public vxvbw<lxlww> qvdri;

		public hjkmq<T0> zuvyi;

		private kpthf gszea;

		private object xzyjx;

		private void grmxq()
		{
			lxlww isepn;
			try
			{
				bool flag = true;
				kpthf p;
				if (hrqlz != 0)
				{
					p = zuvyi.bspdi.iikte().avdby(p1: false).vrtmi();
					if (!p.zpafv || 1 == 0)
					{
						hrqlz = 0;
						gszea = p;
						qvdri.xiwgo(ref p, ref this);
						flag = false;
						return;
					}
				}
				else
				{
					p = gszea;
					gszea = default(kpthf);
					hrqlz = -1;
				}
				p.ekzxl();
				p = default(kpthf);
				try
				{
					isepn = zuvyi.isepn;
				}
				finally
				{
					if (flag && 0 == 0)
					{
						zuvyi.bspdi.zvrwx();
					}
				}
			}
			catch (Exception p2)
			{
				hrqlz = -2;
				qvdri.tudwl(p2);
				return;
			}
			hrqlz = -2;
			qvdri.vzyck(isepn);
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in grmxq
			this.grmxq();
		}

		private void pldqf(fgyyk p0)
		{
			qvdri.viwxd(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in pldqf
			this.pldqf(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CRequestShutdown_003Ed__6 : fgyyk
	{
		public int yudml;

		public ljmxa xneyc;

		public hjkmq<T0> mcqnz;

		public Func<exkzi> nxtnh;

		public exkzi qlaag;

		private xuwyj<exkzi> grdiz;

		private object ihbzx;

		private kpthf trozy;

		private void ljzoo()
		{
			try
			{
				bool flag = true;
				exkzi obj;
				xuwyj<exkzi> p2;
				exkzi exkzi2;
				kpthf p;
				switch (yudml)
				{
				default:
					if (nxtnh == null || 1 == 0)
					{
						throw new ArgumentNullException("shutdownLogic");
					}
					p2 = mcqnz.kclbl(nxtnh).giftg(p1: false).vuozn();
					if (!p2.hqxbj || 1 == 0)
					{
						yudml = 0;
						grdiz = p2;
						xneyc.wqiyk(ref p2, ref this);
						flag = false;
						return;
					}
					goto IL_00ab;
				case 0:
					p2 = grdiz;
					grdiz = default(xuwyj<exkzi>);
					yudml = -1;
					goto IL_00ab;
				case 1:
					{
						p = trozy;
						trozy = default(kpthf);
						yudml = -1;
						break;
					}
					IL_00ab:
					obj = p2.gbccf();
					p2 = default(xuwyj<exkzi>);
					exkzi2 = obj;
					qlaag = exkzi2;
					p = qlaag.avdby(p1: false).vrtmi();
					if (!p.zpafv || 1 == 0)
					{
						yudml = 1;
						trozy = p;
						xneyc.wqiyk(ref p, ref this);
						flag = false;
						return;
					}
					break;
				}
				p.ekzxl();
				p = default(kpthf);
			}
			catch (Exception p3)
			{
				yudml = -2;
				xneyc.iurqb(p3);
				return;
			}
			yudml = -2;
			xneyc.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in ljzoo
			this.ljzoo();
		}

		private void hiuik(fgyyk p0)
		{
			xneyc.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in hiuik
			this.hiuik(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CRequestDispose_003Ed__b : fgyyk
	{
		public int jmijc;

		public ljmxa cwcdn;

		public hjkmq<T0> tucxc;

		public Func<exkzi> ddupn;

		public exkzi codmm;

		private xuwyj<exkzi> happr;

		private object jjcmt;

		private kpthf lraqx;

		private void mkesa()
		{
			try
			{
				bool flag = true;
				exkzi obj;
				xuwyj<exkzi> p2;
				exkzi exkzi2;
				kpthf p;
				switch (jmijc)
				{
				default:
					if (ddupn == null || 1 == 0)
					{
						throw new ArgumentNullException("disposeLogic");
					}
					p2 = tucxc.aexup(ddupn).giftg(p1: false).vuozn();
					if (!p2.hqxbj || 1 == 0)
					{
						jmijc = 0;
						happr = p2;
						cwcdn.wqiyk(ref p2, ref this);
						flag = false;
						return;
					}
					goto IL_00ab;
				case 0:
					p2 = happr;
					happr = default(xuwyj<exkzi>);
					jmijc = -1;
					goto IL_00ab;
				case 1:
					{
						p = lraqx;
						lraqx = default(kpthf);
						jmijc = -1;
						break;
					}
					IL_00ab:
					obj = p2.gbccf();
					p2 = default(xuwyj<exkzi>);
					exkzi2 = obj;
					codmm = exkzi2;
					p = codmm.avdby(p1: false).vrtmi();
					if (!p.zpafv || 1 == 0)
					{
						jmijc = 1;
						lraqx = p;
						cwcdn.wqiyk(ref p, ref this);
						flag = false;
						return;
					}
					break;
				}
				p.ekzxl();
				p = default(kpthf);
			}
			catch (Exception p3)
			{
				jmijc = -2;
				cwcdn.iurqb(p3);
				return;
			}
			jmijc = -2;
			cwcdn.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in mkesa
			this.mkesa();
		}

		private void fbama(fgyyk p0)
		{
			cwcdn.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in fbama
			this.fbama(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CRunWithStateLock_003Ed__10 : fgyyk
	{
		public int rbjne;

		public ljmxa bzfhx;

		public hjkmq<T0> mtofs;

		public Action pnmov;

		private kpthf weyky;

		private object puklj;

		private void qnakq()
		{
			try
			{
				bool flag = true;
				kpthf p;
				if (rbjne != 0)
				{
					if (pnmov == null || 1 == 0)
					{
						throw new ArgumentNullException("action");
					}
					p = mtofs.bspdi.iikte().avdby(p1: false).vrtmi();
					if (!p.zpafv || 1 == 0)
					{
						rbjne = 0;
						weyky = p;
						bzfhx.wqiyk(ref p, ref this);
						flag = false;
						return;
					}
				}
				else
				{
					p = weyky;
					weyky = default(kpthf);
					rbjne = -1;
				}
				p.ekzxl();
				p = default(kpthf);
				try
				{
					pnmov();
				}
				catch (Exception)
				{
				}
				finally
				{
					if (flag && 0 == 0)
					{
						mtofs.bspdi.zvrwx();
					}
				}
			}
			catch (Exception p2)
			{
				rbjne = -2;
				bzfhx.iurqb(p2);
				return;
			}
			rbjne = -2;
			bzfhx.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in qnakq
			this.qnakq();
		}

		private void ccqlp(fgyyk p0)
		{
			bzfhx.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in ccqlp
			this.ccqlp(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CrequestDisposeInner_003Ed__13 : fgyyk
	{
		public int lpmba;

		public vxvbw<exkzi> egzix;

		public hjkmq<T0> eovpk;

		public Func<exkzi> ukscx;

		private kpthf lwfjr;

		private object cqwot;

		private void cgqbr()
		{
			exkzi dioyl;
			try
			{
				bool flag = true;
				kpthf p;
				switch (lpmba)
				{
				default:
					p = eovpk.bspdi.iikte().avdby(p1: false).vrtmi();
					if (!p.zpafv || 1 == 0)
					{
						lpmba = 0;
						lwfjr = p;
						egzix.xiwgo(ref p, ref this);
						flag = false;
						return;
					}
					goto IL_0090;
				case 0:
					p = lwfjr;
					lwfjr = default(kpthf);
					lpmba = -1;
					goto IL_0090;
				case 1:
					break;
					IL_0090:
					p.ekzxl();
					p = default(kpthf);
					break;
				}
				try
				{
					int num = lpmba;
					_ = 1;
					try
					{
						int num2 = lpmba;
						kpthf p2;
						if (num2 == 1)
						{
							p2 = lwfjr;
							lwfjr = default(kpthf);
							lpmba = -1;
							goto IL_0166;
						}
						if (eovpk.isepn != lxlww.vilbu)
						{
							eovpk.vicru(lxlww.dzntz);
							p2 = ukscx().avdby(p1: false).vrtmi();
							if (!p2.zpafv || 1 == 0)
							{
								lpmba = 1;
								lwfjr = p2;
								egzix.xiwgo(ref p2, ref this);
								flag = false;
								return;
							}
							goto IL_0166;
						}
						dioyl = eovpk.zryly.dioyl;
						goto end_IL_00b4;
						IL_0166:
						p2.ekzxl();
						p2 = default(kpthf);
						dioyl = eovpk.zryly.dioyl;
						end_IL_00b4:;
					}
					catch (Exception)
					{
						dioyl = eovpk.zryly.dioyl;
					}
				}
				finally
				{
					if (flag && 0 == 0)
					{
						eovpk.vicru(lxlww.vilbu);
						eovpk.zryly.lxwus(null);
						eovpk.bspdi.zvrwx();
						eovpk.bspdi.Dispose();
					}
				}
			}
			catch (Exception p3)
			{
				lpmba = -2;
				egzix.tudwl(p3);
				return;
			}
			lpmba = -2;
			egzix.vzyck(dioyl);
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in cgqbr
			this.cgqbr();
		}

		private void bxxbt(fgyyk p0)
		{
			egzix.viwxd(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in bxxbt
			this.bxxbt(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CrequestShutdownInner_003Ed__16 : fgyyk
	{
		public int ifenf;

		public vxvbw<exkzi> llvsm;

		public hjkmq<T0> bcvwr;

		public Func<exkzi> uxijd;

		private kpthf wuijx;

		private object dyyix;

		private void denei()
		{
			exkzi dioyl;
			try
			{
				bool flag = true;
				kpthf p;
				switch (ifenf)
				{
				default:
					p = bcvwr.bspdi.iikte().avdby(p1: false).vrtmi();
					if (!p.zpafv || 1 == 0)
					{
						ifenf = 0;
						wuijx = p;
						llvsm.xiwgo(ref p, ref this);
						flag = false;
						return;
					}
					goto IL_0092;
				case 0:
					p = wuijx;
					wuijx = default(kpthf);
					ifenf = -1;
					goto IL_0092;
				case 1:
					break;
					IL_0092:
					p.ekzxl();
					p = default(kpthf);
					break;
				}
				try
				{
					int num = ifenf;
					_ = 1;
					try
					{
						int num2 = ifenf;
						kpthf p2;
						if (num2 == 1)
						{
							p2 = wuijx;
							wuijx = default(kpthf);
							ifenf = -1;
							goto IL_01a4;
						}
						if (bcvwr.isepn == lxlww.gbidl)
						{
							bcvwr.vicru(lxlww.dahjh);
							p2 = uxijd().avdby(p1: false).vrtmi();
							if (!p2.zpafv || 1 == 0)
							{
								ifenf = 1;
								wuijx = p2;
								llvsm.xiwgo(ref p2, ref this);
								flag = false;
								return;
							}
							goto IL_01a4;
						}
						if (!bcvwr.odwhw.dioyl.IsCompleted || 1 == 0)
						{
							InvalidOperationException p3 = new InvalidOperationException("Unexpected method call.");
							bcvwr.odwhw.ggmwx(p3);
						}
						dioyl = bcvwr.odwhw.dioyl;
						goto end_IL_00b6;
						IL_01a4:
						p2.ekzxl();
						p2 = default(kpthf);
						dioyl = bcvwr.odwhw.dioyl;
						end_IL_00b6:;
					}
					catch (Exception p4)
					{
						bcvwr.odwhw.ggmwx(p4);
						dioyl = bcvwr.odwhw.dioyl;
					}
				}
				finally
				{
					if (flag && 0 == 0)
					{
						bcvwr.vicru(lxlww.xhtmu);
						bcvwr.odwhw.lxwus(null);
						bcvwr.bspdi.zvrwx();
					}
				}
			}
			catch (Exception p5)
			{
				ifenf = -2;
				llvsm.tudwl(p5);
				return;
			}
			ifenf = -2;
			llvsm.vzyck(dioyl);
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in denei
			this.denei();
		}

		private void dthvy(fgyyk p0)
		{
			llvsm.viwxd(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in dthvy
			this.dthvy(p0);
		}
	}

	private static readonly string wedwi;

	private static readonly string owgwq;

	private static int smakp;

	private readonly dvxgu<object> zryly;

	private readonly ogeww bspdi;

	private readonly dvxgu<object> odwhw;

	private volatile lxlww isepn;

	private int zvfnd;

	private int zgnnd;

	private int dmukw;

	private string ywuds;

	public int uldlg
	{
		get
		{
			return dmukw;
		}
		private set
		{
			dmukw = value;
		}
	}

	public string knmbi
	{
		get
		{
			return ywuds;
		}
		private set
		{
			ywuds = value;
		}
	}

	public int cjcjp => Interlocked.CompareExchange(ref zvfnd, 0, 0);

	public int vyzpj => Interlocked.CompareExchange(ref zvfnd, 0, 0);

	static hjkmq()
	{
		wedwi = typeof(T0).Name;
		owgwq = brgjd.edcru("Network channel: {0} #", wedwi);
		smakp = 0;
	}

	public hjkmq()
		: this(owgwq, lxlww.gbidl)
	{
	}

	private hjkmq(string channelName, lxlww state)
	{
		if (state == lxlww.fxuxu || 1 == 0)
		{
			throw new ArgumentException("state");
		}
		uldlg = Interlocked.Increment(ref smakp);
		knmbi = ((string.IsNullOrEmpty(channelName) ? true : false) ? "Network channel: {0} #" : channelName) + uldlg;
		isepn = state;
		bspdi = new ogeww(1, 1);
		odwhw = new dvxgu<object>();
		zryly = new dvxgu<object>();
	}

	[vtsnh(typeof(hjkmq<>._003CRequestShutdown_003Ed__0))]
	internal exkzi htpbp()
	{
		_003CRequestShutdown_003Ed__0 p = default(_003CRequestShutdown_003Ed__0);
		p.dsomr = this;
		p.sxouk = ljmxa.nmskg();
		p.ggymk = -1;
		ljmxa sxouk = p.sxouk;
		sxouk.nncuo(ref p);
		return p.sxouk.donjp;
	}

	[vtsnh(typeof(hjkmq<>._003CGetChannelState_003Ed__3))]
	public njvzu<lxlww> tioft()
	{
		_003CGetChannelState_003Ed__3 p = default(_003CGetChannelState_003Ed__3);
		p.zuvyi = this;
		p.qvdri = vxvbw<lxlww>.rdzxj();
		p.hrqlz = -1;
		vxvbw<lxlww> qvdri = p.qvdri;
		qvdri.vklen(ref p);
		return p.qvdri.xieya;
	}

	public int dvyvd()
	{
		return Interlocked.Increment(ref zvfnd);
	}

	public int grhvh()
	{
		return Interlocked.Increment(ref zgnnd);
	}

	public int ivvbj()
	{
		return Interlocked.Decrement(ref zvfnd);
	}

	public int ehxaz()
	{
		return Interlocked.Decrement(ref zgnnd);
	}

	public hjkmq<T0> ikpee(lxlww p0)
	{
		return new hjkmq<T0>("Network channel: {0} #", p0);
	}

	[vtsnh(typeof(hjkmq<>._003CRequestShutdown_003Ed__6))]
	public exkzi czebc(Func<exkzi> p0)
	{
		_003CRequestShutdown_003Ed__6 p1 = default(_003CRequestShutdown_003Ed__6);
		p1.mcqnz = this;
		p1.nxtnh = p0;
		p1.xneyc = ljmxa.nmskg();
		p1.yudml = -1;
		ljmxa xneyc = p1.xneyc;
		xneyc.nncuo(ref p1);
		return p1.xneyc.donjp;
	}

	[vtsnh(typeof(hjkmq<>._003CRequestDispose_003Ed__b))]
	public exkzi bkutr(Func<exkzi> p0)
	{
		_003CRequestDispose_003Ed__b p1 = default(_003CRequestDispose_003Ed__b);
		p1.tucxc = this;
		p1.ddupn = p0;
		p1.cwcdn = ljmxa.nmskg();
		p1.jmijc = -1;
		ljmxa cwcdn = p1.cwcdn;
		cwcdn.nncuo(ref p1);
		return p1.cwcdn.donjp;
	}

	public override string ToString()
	{
		return brgjd.edcru("Id: {0}, FriendlyName: {1}, ChannelState: {2}, ReadOperationInProgressCount: {3}, SendOperationInProgressCount: {4}", uldlg, knmbi, isepn, cjcjp, vyzpj);
	}

	[vtsnh(typeof(hjkmq<>._003CRunWithStateLock_003Ed__10))]
	public exkzi wipbg(Action p0)
	{
		_003CRunWithStateLock_003Ed__10 p1 = default(_003CRunWithStateLock_003Ed__10);
		p1.mtofs = this;
		p1.pnmov = p0;
		p1.bzfhx = ljmxa.nmskg();
		p1.rbjne = -1;
		ljmxa bzfhx = p1.bzfhx;
		bzfhx.nncuo(ref p1);
		return p1.bzfhx.donjp;
	}

	public void pshha()
	{
		switch (isepn)
		{
		case lxlww.xhtmu:
			throw new InvalidOperationException("Cannot run operation after shutdown.");
		case lxlww.dahjh:
		case lxlww.dzntz:
			return;
		}
		_ = 5;
	}

	public void iqced()
	{
		lxlww lxlww2 = isepn;
		if (lxlww2 != lxlww.vilbu)
		{
			return;
		}
		throw new ObjectDisposedException(typeof(T0).FullName);
	}

	[vtsnh(typeof(hjkmq<>._003CrequestDisposeInner_003Ed__13))]
	private njvzu<exkzi> aexup(Func<exkzi> p0)
	{
		_003CrequestDisposeInner_003Ed__13 p1 = default(_003CrequestDisposeInner_003Ed__13);
		p1.eovpk = this;
		p1.ukscx = p0;
		p1.egzix = vxvbw<exkzi>.rdzxj();
		p1.lpmba = -1;
		vxvbw<exkzi> egzix = p1.egzix;
		egzix.vklen(ref p1);
		return p1.egzix.xieya;
	}

	[vtsnh(typeof(hjkmq<>._003CrequestShutdownInner_003Ed__16))]
	private njvzu<exkzi> kclbl(Func<exkzi> p0)
	{
		_003CrequestShutdownInner_003Ed__16 p1 = default(_003CrequestShutdownInner_003Ed__16);
		p1.bcvwr = this;
		p1.uxijd = p0;
		p1.llvsm = vxvbw<exkzi>.rdzxj();
		p1.ifenf = -1;
		vxvbw<exkzi> llvsm = p1.llvsm;
		llvsm.vklen(ref p1);
		return p1.llvsm.xieya;
	}

	private void vicru(lxlww p0)
	{
		_ = isepn;
		isepn = p0;
	}
}
