using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace onrkn;

internal static class ehncb
{
	private static class ajghw<T0>
	{
		public static readonly Action<object> nbbsj;

		private static Action<object> zjcjv;

		static ajghw()
		{
			if (zjcjv == null || 1 == 0)
			{
				zjcjv = zvrmn;
			}
			nbbsj = zjcjv;
		}

		private static void zvrmn(object p0)
		{
			((dvxgu<T0>)p0).kcuac();
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CselectImpl_003Ed__0<TA, TR> : fgyyk
	{
		public int zrfwk;

		public vxvbw<TR> vksvg;

		public njvzu<TA> asieq;

		public Func<TA, TR> nmaze;

		private xuwyj<TA> bzlyd;

		private object xzozr;

		private void hchqx()
		{
			TR p2;
			try
			{
				bool flag = true;
				Func<TA, TR> func;
				xuwyj<TA> p;
				if (zrfwk != 0)
				{
					func = nmaze;
					p = asieq.giftg(p1: false).vuozn();
					if (!p.hqxbj || 1 == 0)
					{
						Func<TA, TR> func2 = func;
						xzozr = func2;
						zrfwk = 0;
						bzlyd = p;
						vksvg.xiwgo(ref p, ref this);
						flag = false;
						return;
					}
				}
				else
				{
					Func<TA, TR> func3 = (Func<TA, TR>)xzozr;
					func = func3;
					xzozr = null;
					p = bzlyd;
					bzlyd = default(xuwyj<TA>);
					zrfwk = -1;
				}
				TA arg = p.gbccf();
				p = default(xuwyj<TA>);
				p2 = func(arg);
			}
			catch (Exception p3)
			{
				zrfwk = -2;
				vksvg.tudwl(p3);
				return;
			}
			zrfwk = -2;
			vksvg.vzyck(p2);
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in hchqx
			this.hchqx();
		}

		private void sfwqy(fgyyk p0)
		{
			vksvg.viwxd(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in sfwqy
			this.sfwqy(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CselectManyImpl_003Ed__3<TA, TR> : fgyyk
	{
		public int qldte;

		public vxvbw<TR> bgpjh;

		public njvzu<TA> atncw;

		public Func<TA, njvzu<TR>> cbcci;

		private xuwyj<TA> biqsk;

		private object lubwq;

		private xuwyj<TR> wfrnm;

		private void mceir()
		{
			TR p3;
			try
			{
				bool flag = true;
				Func<TA, njvzu<TR>> func2;
				TA arg;
				xuwyj<TA> p2;
				xuwyj<TR> p;
				switch (qldte)
				{
				default:
					func2 = cbcci;
					p2 = atncw.giftg(p1: false).vuozn();
					if (!p2.hqxbj || 1 == 0)
					{
						Func<TA, njvzu<TR>> func3 = func2;
						lubwq = func3;
						qldte = 1;
						biqsk = p2;
						bgpjh.xiwgo(ref p2, ref this);
						flag = false;
						return;
					}
					goto IL_00ac;
				case 1:
				{
					Func<TA, njvzu<TR>> func = (Func<TA, njvzu<TR>>)lubwq;
					func2 = func;
					lubwq = null;
					p2 = biqsk;
					biqsk = default(xuwyj<TA>);
					qldte = -1;
					goto IL_00ac;
				}
				case 0:
					{
						p = wfrnm;
						wfrnm = default(xuwyj<TR>);
						qldte = -1;
						break;
					}
					IL_00ac:
					arg = p2.gbccf();
					p2 = default(xuwyj<TA>);
					p = func2(arg).giftg(p1: false).vuozn();
					if (!p.hqxbj || 1 == 0)
					{
						qldte = 0;
						wfrnm = p;
						bgpjh.xiwgo(ref p, ref this);
						flag = false;
						return;
					}
					break;
				}
				TR val = p.gbccf();
				p = default(xuwyj<TR>);
				p3 = val;
			}
			catch (Exception p4)
			{
				qldte = -2;
				bgpjh.tudwl(p4);
				return;
			}
			qldte = -2;
			bgpjh.vzyck(p3);
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in mceir
			this.mceir();
		}

		private void xbjyn(fgyyk p0)
		{
			bgpjh.viwxd(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in xbjyn
			this.xbjyn(p0);
		}
	}

	private sealed class ejbtk<T0, T1, T2>
	{
		private sealed class tqinr
		{
			public ejbtk<T0, T1, T2> fhpgn;

			public T0 nyhdw;

			public njvzu<T2> sffdd(T1 p0)
			{
				return fhpgn.vcpxi(nyhdw, p0).xtyvd();
			}
		}

		public Func<T0, njvzu<T1>> akhov;

		public Func<T0, T1, T2> vcpxi;

		public njvzu<T2> oyuxb(T0 p0)
		{
			tqinr tqinr = new tqinr();
			tqinr.fhpgn = this;
			tqinr.nyhdw = p0;
			return akhov(tqinr.nyhdw).zbmmt(tqinr.sffdd);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CUnwrap_003Ed__f : fgyyk
	{
		public int awacq;

		public ljmxa qhmhi;

		public njvzu<exkzi> nrxlw;

		public exkzi wwzre;

		private xuwyj<exkzi> lxrct;

		private object cykmu;

		private kpthf xwibd;

		private void zjoas()
		{
			try
			{
				bool flag = true;
				exkzi obj;
				xuwyj<exkzi> p2;
				exkzi exkzi2;
				kpthf p;
				switch (awacq)
				{
				default:
					if (nrxlw == null || 1 == 0)
					{
						throw new ArgumentNullException("outerTask");
					}
					p2 = nrxlw.giftg(p1: false).vuozn();
					if (!p2.hqxbj || 1 == 0)
					{
						awacq = 0;
						lxrct = p2;
						qhmhi.wqiyk(ref p2, ref this);
						flag = false;
						return;
					}
					goto IL_00a0;
				case 0:
					p2 = lxrct;
					lxrct = default(xuwyj<exkzi>);
					awacq = -1;
					goto IL_00a0;
				case 1:
					{
						p = xwibd;
						xwibd = default(kpthf);
						awacq = -1;
						break;
					}
					IL_00a0:
					obj = p2.gbccf();
					p2 = default(xuwyj<exkzi>);
					exkzi2 = obj;
					wwzre = exkzi2;
					p = wwzre.avdby(p1: false).vrtmi();
					if (!p.zpafv || 1 == 0)
					{
						awacq = 1;
						xwibd = p;
						qhmhi.wqiyk(ref p, ref this);
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
				awacq = -2;
				qhmhi.iurqb(p3);
				return;
			}
			awacq = -2;
			qhmhi.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in zjoas
			this.zjoas();
		}

		private void sgcyf(fgyyk p0)
		{
			qhmhi.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in sgcyf
			this.sgcyf(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CUnwrap_003Ed__14<T> : fgyyk
	{
		public int nwxvz;

		public vxvbw<T> tbwkh;

		public njvzu<njvzu<T>> ryncn;

		public njvzu<T> najue;

		private xuwyj<njvzu<T>> bhnzk;

		private object lbxqe;

		private xuwyj<T> qbbnu;

		private void gpwfk()
		{
			T p3;
			try
			{
				bool flag = true;
				njvzu<T> obj;
				xuwyj<njvzu<T>> p2;
				njvzu<T> njvzu2;
				xuwyj<T> p;
				switch (nwxvz)
				{
				default:
					if (ryncn == null || 1 == 0)
					{
						throw new ArgumentNullException("outerTask");
					}
					p2 = ryncn.giftg(p1: false).vuozn();
					if (!p2.hqxbj || 1 == 0)
					{
						nwxvz = 0;
						bhnzk = p2;
						tbwkh.xiwgo(ref p2, ref this);
						flag = false;
						return;
					}
					goto IL_00a1;
				case 0:
					p2 = bhnzk;
					bhnzk = default(xuwyj<njvzu<T>>);
					nwxvz = -1;
					goto IL_00a1;
				case 1:
					{
						p = qbbnu;
						qbbnu = default(xuwyj<T>);
						nwxvz = -1;
						break;
					}
					IL_00a1:
					obj = p2.gbccf();
					p2 = default(xuwyj<njvzu<T>>);
					njvzu2 = obj;
					najue = njvzu2;
					p = najue.giftg(p1: false).vuozn();
					if (!p.hqxbj || 1 == 0)
					{
						nwxvz = 1;
						qbbnu = p;
						tbwkh.xiwgo(ref p, ref this);
						flag = false;
						return;
					}
					break;
				}
				T val = p.gbccf();
				p = default(xuwyj<T>);
				p3 = val;
			}
			catch (Exception p4)
			{
				nwxvz = -2;
				tbwkh.tudwl(p4);
				return;
			}
			nwxvz = -2;
			tbwkh.vzyck(p3);
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in gpwfk
			this.gpwfk();
		}

		private void vixdb(fgyyk p0)
		{
			tbwkh.viwxd(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in vixdb
			this.vixdb(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CwithCancellationTokenInner_003Ed__19<T> : fgyyk
	{
		public int hrzzs;

		public vxvbw<T> mxcsc;

		public njvzu<T> nkody;

		public ddmlv nahxc;

		public dvxgu<T> sqkcl;

		public tfsbt ctffx;

		public njvzu<T> hyynp;

		private xuwyj<njvzu<T>> azjpb;

		private object hxdgs;

		private xuwyj<T> utjds;

		private void eszcd()
		{
			T p3;
			try
			{
				bool flag = true;
				switch (hrzzs)
				{
				default:
					sqkcl = new dvxgu<T>();
					ctffx = nahxc.kjwdi(ajghw<T>.nbbsj, sqkcl);
					break;
				case 0:
				case 1:
					break;
				}
				try
				{
					njvzu<T> obj;
					xuwyj<njvzu<T>> p2;
					njvzu<T> njvzu2;
					xuwyj<T> p;
					switch (hrzzs)
					{
					default:
						p2 = rxpjc.afuut<T>(nkody, sqkcl.dioyl).giftg(p1: false).vuozn();
						if (!p2.hqxbj || 1 == 0)
						{
							hrzzs = 0;
							azjpb = p2;
							mxcsc.xiwgo(ref p2, ref this);
							flag = false;
							return;
						}
						goto IL_00e8;
					case 0:
						p2 = azjpb;
						azjpb = default(xuwyj<njvzu<T>>);
						hrzzs = -1;
						goto IL_00e8;
					case 1:
						{
							p = utjds;
							utjds = default(xuwyj<T>);
							hrzzs = -1;
							break;
						}
						IL_00e8:
						obj = p2.gbccf();
						p2 = default(xuwyj<njvzu<T>>);
						njvzu2 = obj;
						hyynp = njvzu2;
						p = hyynp.giftg(p1: false).vuozn();
						if (!p.hqxbj || 1 == 0)
						{
							hrzzs = 1;
							utjds = p;
							mxcsc.xiwgo(ref p, ref this);
							flag = false;
							return;
						}
						break;
					}
					T val = p.gbccf();
					p = default(xuwyj<T>);
					p3 = val;
				}
				finally
				{
					if (flag && 0 == 0)
					{
						tfsbt tfsbt2 = ctffx;
						((IDisposable)tfsbt2/*cast due to .constrained prefix*/).Dispose();
					}
				}
			}
			catch (Exception p4)
			{
				hrzzs = -2;
				mxcsc.tudwl(p4);
				return;
			}
			hrzzs = -2;
			mxcsc.vzyck(p3);
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in eszcd
			this.eszcd();
		}

		private void lkjtr(fgyyk p0)
		{
			mxcsc.viwxd(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in lkjtr
			this.lkjtr(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CwithCancellationTokenInner_003Ed__20 : fgyyk
	{
		public int nazlu;

		public ljmxa aovhd;

		public exkzi smxnt;

		public ddmlv napsh;

		public dvxgu<gfqoc> qvawo;

		public tfsbt nuipc;

		public exkzi qpekb;

		private xuwyj<exkzi> hvpps;

		private object gjrxy;

		private kpthf imgvd;

		private void oxolt()
		{
			try
			{
				bool flag = true;
				switch (nazlu)
				{
				default:
					qvawo = new dvxgu<gfqoc>();
					nuipc = napsh.kjwdi(ajghw<gfqoc>.nbbsj, qvawo);
					break;
				case 0:
				case 1:
					break;
				}
				try
				{
					exkzi obj;
					xuwyj<exkzi> p2;
					exkzi exkzi2;
					kpthf p;
					switch (nazlu)
					{
					default:
						p2 = rxpjc.veygc(smxnt, qvawo.dioyl).giftg(p1: false).vuozn();
						if (!p2.hqxbj || 1 == 0)
						{
							nazlu = 0;
							hvpps = p2;
							aovhd.wqiyk(ref p2, ref this);
							flag = false;
							return;
						}
						goto IL_00e6;
					case 0:
						p2 = hvpps;
						hvpps = default(xuwyj<exkzi>);
						nazlu = -1;
						goto IL_00e6;
					case 1:
						{
							p = imgvd;
							imgvd = default(kpthf);
							nazlu = -1;
							break;
						}
						IL_00e6:
						obj = p2.gbccf();
						p2 = default(xuwyj<exkzi>);
						exkzi2 = obj;
						qpekb = exkzi2;
						p = qpekb.avdby(p1: false).vrtmi();
						if (!p.zpafv || 1 == 0)
						{
							nazlu = 1;
							imgvd = p;
							aovhd.wqiyk(ref p, ref this);
							flag = false;
							return;
						}
						break;
					}
					p.ekzxl();
					p = default(kpthf);
				}
				finally
				{
					if (flag && 0 == 0)
					{
						tfsbt tfsbt2 = nuipc;
						((IDisposable)tfsbt2/*cast due to .constrained prefix*/).Dispose();
					}
				}
			}
			catch (Exception p3)
			{
				nazlu = -2;
				aovhd.iurqb(p3);
				return;
			}
			nazlu = -2;
			aovhd.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in oxolt
			this.oxolt();
		}

		private void hkulm(fgyyk p0)
		{
			aovhd.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in hkulm
			this.hkulm(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CwithTimeoutInner_003Ed__27 : fgyyk
	{
		public int syvwk;

		public ljmxa uphxn;

		public exkzi daljk;

		public TimeSpan oinyc;

		public znuay kqlfo;

		public exkzi eynpw;

		public exkzi tckrn;

		private xuwyj<exkzi> trwcg;

		private object etsxg;

		private kpthf lqyqf;

		private void lnwjq()
		{
			try
			{
				bool flag = true;
				switch (syvwk)
				{
				default:
					kqlfo = new znuay();
					break;
				case 0:
				case 1:
					break;
				}
				try
				{
					exkzi obj;
					xuwyj<exkzi> p2;
					exkzi exkzi2;
					kpthf p;
					switch (syvwk)
					{
					default:
						eynpw = rxpjc.aexnr((int)oinyc.TotalMilliseconds, kqlfo.qncaj);
						p2 = rxpjc.veygc(daljk, eynpw).giftg(p1: false).vuozn();
						if (!p2.hqxbj || 1 == 0)
						{
							syvwk = 0;
							trwcg = p2;
							uphxn.wqiyk(ref p2, ref this);
							flag = false;
							return;
						}
						goto IL_00e7;
					case 0:
						p2 = trwcg;
						trwcg = default(xuwyj<exkzi>);
						syvwk = -1;
						goto IL_00e7;
					case 1:
						{
							p = lqyqf;
							lqyqf = default(kpthf);
							syvwk = -1;
							break;
						}
						IL_00e7:
						obj = p2.gbccf();
						p2 = default(xuwyj<exkzi>);
						exkzi2 = obj;
						tckrn = exkzi2;
						if (tckrn == eynpw)
						{
							throw new TimeoutException();
						}
						kqlfo.pvutk();
						p = tckrn.avdby(p1: false).vrtmi();
						if (!p.zpafv || 1 == 0)
						{
							syvwk = 1;
							lqyqf = p;
							uphxn.wqiyk(ref p, ref this);
							flag = false;
							return;
						}
						break;
					}
					p.ekzxl();
					p = default(kpthf);
				}
				finally
				{
					if (flag && 0 == 0 && kqlfo != null && 0 == 0)
					{
						((IDisposable)kqlfo).Dispose();
					}
				}
			}
			catch (Exception p3)
			{
				syvwk = -2;
				uphxn.iurqb(p3);
				return;
			}
			syvwk = -2;
			uphxn.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in lnwjq
			this.lnwjq();
		}

		private void hdtoc(fgyyk p0)
		{
			uphxn.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in hdtoc
			this.hdtoc(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CwithTimeoutInner_003Ed__2e<T> : fgyyk
	{
		public int gtnaz;

		public vxvbw<T> kgffp;

		public njvzu<T> ikipc;

		public TimeSpan cducm;

		public znuay mnzmk;

		public exkzi oylwj;

		public exkzi ywcxl;

		private xuwyj<exkzi> ousow;

		private object guzvo;

		private xuwyj<T> tqfzs;

		private void tjgat()
		{
			T p3;
			try
			{
				bool flag = true;
				switch (gtnaz)
				{
				default:
					mnzmk = new znuay();
					break;
				case 0:
				case 1:
					break;
				}
				try
				{
					exkzi obj;
					xuwyj<exkzi> p2;
					exkzi exkzi2;
					xuwyj<T> p;
					switch (gtnaz)
					{
					default:
						oylwj = rxpjc.aexnr((int)cducm.TotalMilliseconds, mnzmk.qncaj);
						p2 = rxpjc.veygc(ikipc, oylwj).giftg(p1: false).vuozn();
						if (!p2.hqxbj || 1 == 0)
						{
							gtnaz = 0;
							ousow = p2;
							kgffp.xiwgo(ref p2, ref this);
							flag = false;
							return;
						}
						goto IL_00e9;
					case 0:
						p2 = ousow;
						ousow = default(xuwyj<exkzi>);
						gtnaz = -1;
						goto IL_00e9;
					case 1:
						{
							p = tqfzs;
							tqfzs = default(xuwyj<T>);
							gtnaz = -1;
							break;
						}
						IL_00e9:
						obj = p2.gbccf();
						p2 = default(xuwyj<exkzi>);
						exkzi2 = obj;
						ywcxl = exkzi2;
						if (ywcxl == oylwj)
						{
							throw new TimeoutException();
						}
						mnzmk.pvutk();
						p = ikipc.giftg(p1: false).vuozn();
						if (!p.hqxbj || 1 == 0)
						{
							gtnaz = 1;
							tqfzs = p;
							kgffp.xiwgo(ref p, ref this);
							flag = false;
							return;
						}
						break;
					}
					T val = p.gbccf();
					p = default(xuwyj<T>);
					p3 = val;
				}
				finally
				{
					if (flag && 0 == 0 && mnzmk != null && 0 == 0)
					{
						((IDisposable)mnzmk).Dispose();
					}
				}
			}
			catch (Exception p4)
			{
				gtnaz = -2;
				kgffp.tudwl(p4);
				return;
			}
			gtnaz = -2;
			kgffp.vzyck(p3);
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in tjgat
			this.tjgat();
		}

		private void zfuie(fgyyk p0)
		{
			kgffp.viwxd(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in zfuie
			this.zfuie(p0);
		}
	}

	private static Action<exkzi> tevmp;

	public static kbnfj jimoq(this exkzi p0)
	{
		return new kbnfj(p0, continueOnCapturedContext: true);
	}

	public static evzky<TResult> ykhdn<TResult>(this njvzu<TResult> p0)
	{
		return new evzky<TResult>(p0, continueOnCapturedContext: true);
	}

	public static kpthf avdby(this exkzi p0, bool p1)
	{
		return new kpthf(p0, p1);
	}

	public static xuwyj<TResult> giftg<TResult>(this njvzu<TResult> p0, bool p1)
	{
		return new xuwyj<TResult>(p0, p1);
	}

	public static TResult ymczw<TResult>(this njvzu<TResult> p0)
	{
		p0.fdqtm();
		return p0.islme;
	}

	public static bool zvfpz<TResult>(this njvzu<TResult> p0, int p1, out TResult p2)
	{
		if (!p0.vybqh(p1) || 1 == 0)
		{
			p2 = default(TResult);
			return false;
		}
		p2 = p0.islme;
		return true;
	}

	public static void fdqtm(this exkzi p0)
	{
		try
		{
			p0.txebj();
		}
		catch (nagsk nagsk2)
		{
			object ex = nagsk2.InnerException;
			if (ex == null || 1 == 0)
			{
				ex = nagsk2;
			}
			((Exception)ex).irjxm();
		}
		p0.ulnrr();
	}

	public static bool vybqh(this exkzi p0, int p1)
	{
		try
		{
			if (!p0.mtfep(p1) || 1 == 0)
			{
				return false;
			}
		}
		catch (nagsk nagsk2)
		{
			object ex = nagsk2.InnerException;
			if (ex == null || 1 == 0)
			{
				ex = nagsk2;
			}
			((Exception)ex).irjxm();
		}
		p0.ulnrr();
		return true;
	}

	public static exkzi kvzxl(this exkzi p0, Action<exkzi> p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("task");
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("continuation");
		}
		return p0.wszna(p1);
	}

	public static exkzi wdogv(this exkzi p0, Action<exkzi, object> p1, object p2)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("task");
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("continuation");
		}
		return p0.jyasg(p1, p2);
	}

	public static njvzu<TContinuationResult> nsehe<TContinuationResult>(this exkzi p0, Func<exkzi, TContinuationResult> p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("task");
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("continuation");
		}
		return p0.gqrnv(p1);
	}

	public static njvzu<TContinuationResult> osdty<TOriginalResult, TContinuationResult>(this njvzu<TOriginalResult> p0, Func<njvzu<TOriginalResult>, TContinuationResult> p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("task");
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("continuation");
		}
		return p0.amcit(p1);
	}

	public static njvzu<TValue> xtyvd<TValue>(this TValue p0)
	{
		return rxpjc.caxut(p0);
	}

	public static exkzi pearr(this exkzi p0)
	{
		return p0;
	}

	public static void xgngc(this exkzi p0)
	{
		try
		{
			p0.txebj();
		}
		catch (nagsk nagsk2)
		{
			throw nagsk2.mfkfw.First();
		}
	}

	public static bool pphjx(this exkzi p0, TimeSpan p1)
	{
		try
		{
			return p0.xfxjm(p1);
		}
		catch (nagsk nagsk2)
		{
			throw nagsk2.mfkfw.First();
		}
	}

	public static njvzu<TR> plqum<TA, TR>(this njvzu<TA> p0, Func<TA, TR> p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("sourceTask");
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("selectFunc");
		}
		return p0.jlyiw(p1);
	}

	[vtsnh(typeof(_003CselectImpl_003Ed__0<, >))]
	public static njvzu<TR> jlyiw<TA, TR>(this njvzu<TA> p0, Func<TA, TR> p1)
	{
		_003CselectImpl_003Ed__0<TA, TR> p2 = default(_003CselectImpl_003Ed__0<TA, TR>);
		p2.asieq = p0;
		p2.nmaze = p1;
		p2.vksvg = vxvbw<TR>.rdzxj();
		p2.zrfwk = -1;
		vxvbw<TR> vksvg = p2.vksvg;
		vksvg.vklen(ref p2);
		return p2.vksvg.xieya;
	}

	public static njvzu<TR> zbmmt<TA, TR>(this njvzu<TA> p0, Func<TA, njvzu<TR>> p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("sourceTask");
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("selectManyFunc");
		}
		return p0.erjlg(p1);
	}

	[vtsnh(typeof(_003CselectManyImpl_003Ed__3<, >))]
	public static njvzu<TR> erjlg<TA, TR>(this njvzu<TA> p0, Func<TA, njvzu<TR>> p1)
	{
		_003CselectManyImpl_003Ed__3<TA, TR> p2 = default(_003CselectManyImpl_003Ed__3<TA, TR>);
		p2.atncw = p0;
		p2.cbcci = p1;
		p2.bgpjh = vxvbw<TR>.rdzxj();
		p2.qldte = -1;
		vxvbw<TR> bgpjh = p2.bgpjh;
		bgpjh.vklen(ref p2);
		return p2.bgpjh.xieya;
	}

	public static njvzu<TR> bretd<TA1, TA2, TR>(this njvzu<TA1> p0, Func<TA1, njvzu<TA2>> p1, Func<TA1, TA2, TR> p2)
	{
		ejbtk<TA1, TA2, TR> ejbtk = new ejbtk<TA1, TA2, TR>();
		ejbtk.akhov = p1;
		ejbtk.vcpxi = p2;
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("sourceTask");
		}
		if (ejbtk.akhov == null || 1 == 0)
		{
			throw new ArgumentNullException("selectManyFunc");
		}
		if (ejbtk.vcpxi == null || 1 == 0)
		{
			throw new ArgumentNullException("projectionFunc");
		}
		return p0.zbmmt(ejbtk.oyuxb);
	}

	public static njvzu<TR> slmip<TA, TR>(this njvzu<TA> p0, Func<TA, njvzu<TR>> p1)
	{
		return p0.zbmmt(p1);
	}

	public static njvzu<T> xyjnw<T>(this njvzu<T> p0, TimeSpan p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("task");
		}
		if (!p0.IsCompleted || 1 == 0)
		{
			return zobgo(p0, p1);
		}
		return p0;
	}

	public static exkzi xzlko(this exkzi p0, TimeSpan p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("task");
		}
		if (!p0.IsCompleted || 1 == 0)
		{
			return gbtrl(p0, p1);
		}
		return p0;
	}

	public static njvzu<T> obrzd<T>(this njvzu<T> p0, int p1)
	{
		if (p1 < -1)
		{
			throw new ArgumentOutOfRangeException("milliseconds");
		}
		TimeSpan p2 = new TimeSpan(0, 0, 0, 0, p1);
		return p0.xyjnw(p2);
	}

	public static exkzi rvbbe(this exkzi p0, int p1)
	{
		if (p1 < -1)
		{
			throw new ArgumentOutOfRangeException("milliseconds");
		}
		TimeSpan p2 = new TimeSpan(0, 0, 0, 0, p1);
		return p0.xzlko(p2);
	}

	public static njvzu<T> zbvfo<T>(this njvzu<T> p0, ddmlv p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("task");
		}
		if ((p0.IsCompleted ? true : false) || p1.Equals(ddmlv.prdik))
		{
			return p0;
		}
		p1.uxxyi();
		return uutvw(p0, p1);
	}

	public static exkzi rejkf(this exkzi p0, ddmlv p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("task");
		}
		if ((p0.IsCompleted ? true : false) || p1.Equals(ddmlv.prdik))
		{
			return p0;
		}
		p1.uxxyi();
		return xzvcu(p0, p1);
	}

	public static void qaiyq(this exkzi p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("task");
		}
		if (p0.IsCompleted && 0 == 0)
		{
			_ = p0.ijeei;
			return;
		}
		if (tevmp == null || 1 == 0)
		{
			tevmp = xwjyu;
		}
		p0.kvzxl(tevmp);
	}

	[vtsnh(typeof(_003CUnwrap_003Ed__f))]
	public static exkzi htwzs(this njvzu<exkzi> p0)
	{
		_003CUnwrap_003Ed__f p1 = default(_003CUnwrap_003Ed__f);
		p1.nrxlw = p0;
		p1.qhmhi = ljmxa.nmskg();
		p1.awacq = -1;
		ljmxa qhmhi = p1.qhmhi;
		qhmhi.nncuo(ref p1);
		return p1.qhmhi.donjp;
	}

	[vtsnh(typeof(_003CUnwrap_003Ed__14<>))]
	public static njvzu<T> pyeoj<T>(this njvzu<njvzu<T>> p0)
	{
		_003CUnwrap_003Ed__14<T> p1 = default(_003CUnwrap_003Ed__14<T>);
		p1.ryncn = p0;
		p1.tbwkh = vxvbw<T>.rdzxj();
		p1.nwxvz = -1;
		vxvbw<T> tbwkh = p1.tbwkh;
		tbwkh.vklen(ref p1);
		return p1.tbwkh.xieya;
	}

	[vtsnh(typeof(_003CwithCancellationTokenInner_003Ed__19<>))]
	private static njvzu<T> uutvw<T>(njvzu<T> p0, ddmlv p1)
	{
		_003CwithCancellationTokenInner_003Ed__19<T> p2 = default(_003CwithCancellationTokenInner_003Ed__19<T>);
		p2.nkody = p0;
		p2.nahxc = p1;
		p2.mxcsc = vxvbw<T>.rdzxj();
		p2.hrzzs = -1;
		vxvbw<T> mxcsc = p2.mxcsc;
		mxcsc.vklen(ref p2);
		return p2.mxcsc.xieya;
	}

	[vtsnh(typeof(_003CwithCancellationTokenInner_003Ed__20))]
	private static exkzi xzvcu(exkzi p0, ddmlv p1)
	{
		_003CwithCancellationTokenInner_003Ed__20 p2 = default(_003CwithCancellationTokenInner_003Ed__20);
		p2.smxnt = p0;
		p2.napsh = p1;
		p2.aovhd = ljmxa.nmskg();
		p2.nazlu = -1;
		ljmxa aovhd = p2.aovhd;
		aovhd.nncuo(ref p2);
		return p2.aovhd.donjp;
	}

	[vtsnh(typeof(_003CwithTimeoutInner_003Ed__27))]
	private static exkzi gbtrl(exkzi p0, TimeSpan p1)
	{
		_003CwithTimeoutInner_003Ed__27 p2 = default(_003CwithTimeoutInner_003Ed__27);
		p2.daljk = p0;
		p2.oinyc = p1;
		p2.uphxn = ljmxa.nmskg();
		p2.syvwk = -1;
		ljmxa uphxn = p2.uphxn;
		uphxn.nncuo(ref p2);
		return p2.uphxn.donjp;
	}

	[vtsnh(typeof(_003CwithTimeoutInner_003Ed__2e<>))]
	private static njvzu<T> zobgo<T>(njvzu<T> p0, TimeSpan p1)
	{
		_003CwithTimeoutInner_003Ed__2e<T> p2 = default(_003CwithTimeoutInner_003Ed__2e<T>);
		p2.ikipc = p0;
		p2.cducm = p1;
		p2.kgffp = vxvbw<T>.rdzxj();
		p2.gtnaz = -1;
		vxvbw<T> kgffp = p2.kgffp;
		kgffp.vklen(ref p2);
		return p2.kgffp.xieya;
	}

	private static void ulnrr(this exkzi p0)
	{
		if (p0.awssf != gmpgj.pcduu)
		{
			throw new InvalidOperationException("Unexpected task state: " + p0.awssf);
		}
	}

	private static void xwjyu(exkzi p0)
	{
		_ = p0.ijeei;
	}
}
