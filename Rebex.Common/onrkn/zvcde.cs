using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace onrkn;

internal static class zvcde
{
	private sealed class qmykz<T0>
	{
		public bool aufjz;

		public T0 btggm;

		public Func<T0> bhlaj;

		public T0 umcoq()
		{
			if (aufjz && 0 == 0)
			{
				return btggm;
			}
			T0 result = (btggm = bhlaj());
			aufjz = true;
			return result;
		}
	}

	private sealed class qxxxx<T0, T1>
	{
		public sroer<T1, T0> nbpqd;

		public Func<T1, T0> igstb;

		public T0 stvcq(T1 p0)
		{
			if (nbpqd.TryGetValue(p0, out var value) && 0 == 0)
			{
				return value;
			}
			T0 p1 = igstb(p0);
			return nbpqd.wsggz(p0, p1);
		}
	}

	private sealed class npzbu<T0> where T0 : class
	{
		public Func<T0> mfaqv;

		public IEnumerable<Func<T0>> gejzb;

		public T0 oxkzm()
		{
			T0 val = mfaqv();
			if (val == null || 1 == 0)
			{
				val = gkzik(gejzb.ToArray());
			}
			return val;
		}
	}

	private sealed class kpegc<T0> where T0 : class
	{
		public bool qhhnh;

		public Func<T0> qdlcr;

		public Func<T0>[] fsjqu;

		public T0 ubbcy()
		{
			T0 val = qdlcr();
			if (val == null || 1 == 0)
			{
				if (!qhhnh || 1 == 0)
				{
					return null;
				}
				val = fsjqu[0].hgyxz(fsjqu.Skip(1))();
			}
			return val;
		}
	}

	private sealed class saqpm<T0, T1> where T1 : class
	{
		private sealed class ewuye
		{
			public saqpm<T0, T1> kdkns;

			public T0 dkhgw;

			public Func<T1> fdgmd(Func<T0, T1> p0)
			{
				return p0.vcxhp(dkhgw);
			}
		}

		public Func<T0, T1> nujih;

		public Func<T0, T1>[] qwjbg;

		public T1 nfkib(T0 p0)
		{
			ewuye ewuye = new ewuye();
			ewuye.kdkns = this;
			ewuye.dkhgw = p0;
			return nujih.vcxhp(ewuye.dkhgw).hgyxz(qwjbg.Select(ewuye.fdgmd))();
		}
	}

	private sealed class yqpcz<T0, T1>
	{
		public Func<T0, T1> talxi;

		public T0 rekzz;

		public T1 ysyxe()
		{
			return talxi(rekzz);
		}
	}

	private sealed class fzngz
	{
		public Action xtvfk;

		public gfqoc qmlkh()
		{
			xtvfk();
			return gfqoc.jusmc;
		}
	}

	private sealed class mhzio<T0>
	{
		public Action<T0> ynttd;

		public gfqoc zexja(T0 p0)
		{
			ynttd(p0);
			return gfqoc.jusmc;
		}
	}

	private sealed class uzuvu<T0, T1, T2>
	{
		public Func<T0, T1> tggan;

		public Func<T1, T2> qwznv;

		public T2 onhwc(T0 p0)
		{
			return qwznv(tggan(p0));
		}
	}

	private sealed class rblnp<T0, T1, T2>
	{
		public Func<T0, T1, T2> yjumt;

		public T0 xsgkx;

		public T2 tjgij(T1 p0)
		{
			return yjumt(xsgkx, p0);
		}
	}

	private sealed class elbfr<T0, T1, T2, T3>
	{
		public Func<T0, T1, T2, T3> hisuf;

		public T0 wqxkb;

		public T3 vdlqy(T1 p0, T2 p1)
		{
			return hisuf(wqxkb, p0, p1);
		}
	}

	private sealed class euaii<T0, T1, T2, T3, T4>
	{
		public Func<T0, T1, T2, T3, T4> rusig;

		public T0 cvrjz;

		public T4 bzfgr(T1 p0, T2 p1, T3 p2)
		{
			return rusig(cvrjz, p0, p1, p2);
		}
	}

	private sealed class xndjd<T0, T1, T2, T3, T4, T5>
	{
		public qsdrh<T0, T1, T2, T3, T4, T5> bvkur;

		public T0 plemm;

		public T5 abink(T1 p0, T2 p1, T3 p2, T4 p3)
		{
			return bvkur(plemm, p0, p1, p2, p3);
		}
	}

	private sealed class mzrsz
	{
		public Func<bool> lkilb;

		public Func<bool> wynia;

		public bool nlfro()
		{
			if (lkilb() && 0 == 0)
			{
				return wynia();
			}
			return false;
		}
	}

	private sealed class bobqm<T0>
	{
		public Func<T0, bool> dhlvf;

		public Func<T0, bool> fskyd;

		public bool sliov(T0 p0)
		{
			if (dhlvf(p0) && 0 == 0)
			{
				return fskyd(p0);
			}
			return false;
		}
	}

	private sealed class iqlpi
	{
		public Func<bool> eoaye;

		public Func<bool> faomy;

		public bool osgyf()
		{
			if (!eoaye() || 1 == 0)
			{
				return faomy();
			}
			return true;
		}
	}

	private sealed class ldpoa<T0>
	{
		public Func<T0> zkzbq;

		public njvzu<T0> lpuxs()
		{
			dvxgu<T0> dvxgu2 = new dvxgu<T0>();
			try
			{
				T0 p = zkzbq();
				dvxgu2.lxwus(p);
			}
			catch (Exception p2)
			{
				dvxgu2.ggmwx(p2);
			}
			return dvxgu2.dioyl;
		}
	}

	private sealed class cwfua<T0>
	{
		public Func<T0> keagu;

		public Func<Exception, T0> yijdv;

		public T0 kqavj()
		{
			return keagu.cqxhx(yijdv);
		}
	}

	private sealed class nxhyy<T0>
	{
		public Func<gfqoc> cvgnj;

		public Func<Exception, gfqoc> yezce;

		public gfqoc znlhd()
		{
			return cvgnj.cqxhx(yezce);
		}
	}

	private sealed class zpnze<T0>
	{
		public Action tercs;

		public Action<Exception> edkju;

		public gfqoc tfhtv()
		{
			return tercs.iyswc().cqxhx(edkju.kbiid());
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CWithTaskExceptionGuard_003Ed__41<TA> : fgyyk
	{
		public int wrdyv;

		public ljmxa nuugv;

		public Func<TA, exkzi> drpgs;

		public TA hflgy;

		public Func<Exception, exkzi> isqeg;

		public Exception iizzr;

		private kpthf msvls;

		private object cqqct;

		private void tokmz()
		{
			try
			{
				bool flag = true;
				kpthf p;
				switch (wrdyv)
				{
				default:
					if (drpgs == null || 1 == 0)
					{
						throw new ArgumentNullException("originalFunc");
					}
					if (isqeg == null || 1 == 0)
					{
						throw new ArgumentNullException("exceptionalFunc");
					}
					iizzr = null;
					goto case 0;
				case 0:
					try
					{
						kpthf p2;
						if (wrdyv != 0)
						{
							p2 = drpgs(hflgy).avdby(p1: false).vrtmi();
							if (!p2.zpafv || 1 == 0)
							{
								wrdyv = 0;
								msvls = p2;
								nuugv.wqiyk(ref p2, ref this);
								flag = false;
								return;
							}
						}
						else
						{
							p2 = msvls;
							msvls = default(kpthf);
							wrdyv = -1;
						}
						p2.ekzxl();
						p2 = default(kpthf);
					}
					catch (Exception ex)
					{
						iizzr = ex;
						goto IL_00f9;
					}
					goto end_IL_0000;
				case 1:
					{
						p = msvls;
						msvls = default(kpthf);
						wrdyv = -1;
						break;
					}
					IL_00f9:
					p = isqeg(iizzr).avdby(p1: false).vrtmi();
					if (!p.zpafv || 1 == 0)
					{
						wrdyv = 1;
						msvls = p;
						nuugv.wqiyk(ref p, ref this);
						flag = false;
						return;
					}
					break;
				}
				p.ekzxl();
				p = default(kpthf);
				end_IL_0000:;
			}
			catch (Exception p3)
			{
				wrdyv = -2;
				nuugv.iurqb(p3);
				return;
			}
			wrdyv = -2;
			nuugv.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in tokmz
			this.tokmz();
		}

		private void gjvqp(fgyyk p0)
		{
			nuugv.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in gjvqp
			this.gjvqp(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CWithTaskExceptionGuard_003Ed__45<TA1, TA2> : fgyyk
	{
		public int xhkwn;

		public ljmxa lypzl;

		public Func<TA1, TA2, exkzi> exruj;

		public TA1 wound;

		public TA2 xhrbo;

		public Func<Exception, exkzi> vmcns;

		public Exception ohnih;

		private kpthf ewyaq;

		private object nsnjg;

		private void ifcse()
		{
			try
			{
				bool flag = true;
				kpthf p;
				switch (xhkwn)
				{
				default:
					if (exruj == null || 1 == 0)
					{
						throw new ArgumentNullException("originalFunc");
					}
					if (vmcns == null || 1 == 0)
					{
						throw new ArgumentNullException("exceptionalFunc");
					}
					ohnih = null;
					goto case 0;
				case 0:
					try
					{
						kpthf p2;
						if (xhkwn != 0)
						{
							p2 = exruj(wound, xhrbo).avdby(p1: false).vrtmi();
							if (!p2.zpafv || 1 == 0)
							{
								xhkwn = 0;
								ewyaq = p2;
								lypzl.wqiyk(ref p2, ref this);
								flag = false;
								return;
							}
						}
						else
						{
							p2 = ewyaq;
							ewyaq = default(kpthf);
							xhkwn = -1;
						}
						p2.ekzxl();
						p2 = default(kpthf);
					}
					catch (Exception ex)
					{
						ohnih = ex;
						goto IL_00ff;
					}
					goto end_IL_0000;
				case 1:
					{
						p = ewyaq;
						ewyaq = default(kpthf);
						xhkwn = -1;
						break;
					}
					IL_00ff:
					p = vmcns(ohnih).avdby(p1: false).vrtmi();
					if (!p.zpafv || 1 == 0)
					{
						xhkwn = 1;
						ewyaq = p;
						lypzl.wqiyk(ref p, ref this);
						flag = false;
						return;
					}
					break;
				}
				p.ekzxl();
				p = default(kpthf);
				end_IL_0000:;
			}
			catch (Exception p3)
			{
				xhkwn = -2;
				lypzl.iurqb(p3);
				return;
			}
			xhkwn = -2;
			lypzl.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in ifcse
			this.ifcse();
		}

		private void mvops(fgyyk p0)
		{
			lypzl.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in mvops
			this.mvops(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CwithTaskExceptionGuardInner_003Ed__4d : fgyyk
	{
		public int qnqbm;

		public ljmxa kvwoe;

		public Func<exkzi> bcrtg;

		public Func<Exception, exkzi> zozba;

		public Exception tnazd;

		private kpthf jaelq;

		private object vfmuz;

		private void thocn()
		{
			try
			{
				bool flag = true;
				kpthf p;
				switch (qnqbm)
				{
				default:
					tnazd = null;
					goto case 0;
				case 0:
					try
					{
						kpthf p2;
						if (qnqbm != 0)
						{
							p2 = bcrtg().avdby(p1: false).vrtmi();
							if (!p2.zpafv || 1 == 0)
							{
								qnqbm = 0;
								jaelq = p2;
								kvwoe.wqiyk(ref p2, ref this);
								flag = false;
								return;
							}
						}
						else
						{
							p2 = jaelq;
							jaelq = default(kpthf);
							qnqbm = -1;
						}
						p2.ekzxl();
						p2 = default(kpthf);
					}
					catch (Exception ex)
					{
						tnazd = ex;
						goto IL_00bd;
					}
					goto end_IL_0000;
				case 1:
					{
						p = jaelq;
						jaelq = default(kpthf);
						qnqbm = -1;
						break;
					}
					IL_00bd:
					p = zozba(tnazd).avdby(p1: false).vrtmi();
					if (!p.zpafv || 1 == 0)
					{
						qnqbm = 1;
						jaelq = p;
						kvwoe.wqiyk(ref p, ref this);
						flag = false;
						return;
					}
					break;
				}
				p.ekzxl();
				p = default(kpthf);
				end_IL_0000:;
			}
			catch (Exception p3)
			{
				qnqbm = -2;
				kvwoe.iurqb(p3);
				return;
			}
			qnqbm = -2;
			kvwoe.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in thocn
			this.thocn();
		}

		private void pqqsu(fgyyk p0)
		{
			kvwoe.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in pqqsu
			this.pqqsu(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CwithTaskExceptionGuardInner_003Ed__51<TR> : fgyyk
	{
		public int uorgk;

		public vxvbw<TR> eljgn;

		public Func<njvzu<TR>> cmabc;

		public Func<Exception, njvzu<TR>> mwdvq;

		public Exception sklyq;

		private xuwyj<TR> nmbca;

		private object ttylg;

		private void xtryo()
		{
			TR p3;
			try
			{
				bool flag = true;
				xuwyj<TR> p;
				switch (uorgk)
				{
				default:
					sklyq = null;
					goto case 0;
				case 0:
					try
					{
						xuwyj<TR> p2;
						if (uorgk != 0)
						{
							p2 = cmabc().giftg(p1: false).vuozn();
							if (!p2.hqxbj || 1 == 0)
							{
								uorgk = 0;
								nmbca = p2;
								eljgn.xiwgo(ref p2, ref this);
								flag = false;
								return;
							}
						}
						else
						{
							p2 = nmbca;
							nmbca = default(xuwyj<TR>);
							uorgk = -1;
						}
						TR val = p2.gbccf();
						p2 = default(xuwyj<TR>);
						p3 = val;
					}
					catch (Exception ex)
					{
						sklyq = ex;
						goto IL_00c0;
					}
					goto end_IL_0000;
				case 1:
					{
						p = nmbca;
						nmbca = default(xuwyj<TR>);
						uorgk = -1;
						break;
					}
					IL_00c0:
					p = mwdvq(sklyq).giftg(p1: false).vuozn();
					if (!p.hqxbj || 1 == 0)
					{
						uorgk = 1;
						nmbca = p;
						eljgn.xiwgo(ref p, ref this);
						flag = false;
						return;
					}
					break;
				}
				TR val2 = p.gbccf();
				p = default(xuwyj<TR>);
				p3 = val2;
				end_IL_0000:;
			}
			catch (Exception p4)
			{
				uorgk = -2;
				eljgn.tudwl(p4);
				return;
			}
			uorgk = -2;
			eljgn.vzyck(p3);
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in xtryo
			this.xtryo();
		}

		private void lfosy(fgyyk p0)
		{
			eljgn.viwxd(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in lfosy
			this.lfosy(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CinnerFinally_003Ed__55 : fgyyk
	{
		public int fhddz;

		public ljmxa boiky;

		public exkzi vtoyw;

		public Func<exkzi> jnpmr;

		private kpthf omhxx;

		private object solvi;

		private void eguut()
		{
			try
			{
				bool flag = true;
				kpthf p2;
				kpthf p;
				switch (fhddz)
				{
				default:
					try
					{
						kpthf p3;
						if (fhddz != 0)
						{
							p3 = vtoyw.avdby(p1: false).vrtmi();
							if (!p3.zpafv || 1 == 0)
							{
								fhddz = 0;
								omhxx = p3;
								boiky.wqiyk(ref p3, ref this);
								flag = false;
								return;
							}
						}
						else
						{
							p3 = omhxx;
							omhxx = default(kpthf);
							fhddz = -1;
						}
						p3.ekzxl();
						p3 = default(kpthf);
					}
					catch (Exception p4)
					{
						wyido(p4);
					}
					p2 = jnpmr().avdby(p1: false).vrtmi();
					if (!p2.zpafv || 1 == 0)
					{
						fhddz = 1;
						omhxx = p2;
						boiky.wqiyk(ref p2, ref this);
						flag = false;
						return;
					}
					goto IL_0121;
				case 1:
					p2 = omhxx;
					omhxx = default(kpthf);
					fhddz = -1;
					goto IL_0121;
				case 2:
					{
						p = omhxx;
						omhxx = default(kpthf);
						fhddz = -1;
						break;
					}
					IL_0121:
					p2.ekzxl();
					p2 = default(kpthf);
					p = vtoyw.avdby(p1: false).vrtmi();
					if (!p.zpafv || 1 == 0)
					{
						fhddz = 2;
						omhxx = p;
						boiky.wqiyk(ref p, ref this);
						flag = false;
						return;
					}
					break;
				}
				p.ekzxl();
				p = default(kpthf);
			}
			catch (Exception p5)
			{
				fhddz = -2;
				boiky.iurqb(p5);
				return;
			}
			fhddz = -2;
			boiky.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in eguut
			this.eguut();
		}

		private void rohjp(fgyyk p0)
		{
			boiky.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in rohjp
			this.rohjp(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CinnerFinally_003Ed__58<TR> : fgyyk
	{
		public int soctk;

		public vxvbw<TR> txcrh;

		public njvzu<TR> uqgkn;

		public Func<exkzi> eehzi;

		private xuwyj<TR> ewibe;

		private object bheff;

		private kpthf kqdgy;

		private void gnwyk()
		{
			TR p5;
			try
			{
				bool flag = true;
				kpthf p2;
				xuwyj<TR> p;
				switch (soctk)
				{
				default:
					try
					{
						xuwyj<TR> p3;
						if (soctk != 0)
						{
							p3 = uqgkn.giftg(p1: false).vuozn();
							if (!p3.hqxbj || 1 == 0)
							{
								soctk = 0;
								ewibe = p3;
								txcrh.xiwgo(ref p3, ref this);
								flag = false;
								return;
							}
						}
						else
						{
							p3 = ewibe;
							ewibe = default(xuwyj<TR>);
							soctk = -1;
						}
						p3.gbccf();
						p3 = default(xuwyj<TR>);
					}
					catch (Exception p4)
					{
						wyido(p4);
					}
					p2 = eehzi().avdby(p1: false).vrtmi();
					if (!p2.zpafv || 1 == 0)
					{
						soctk = 1;
						kqdgy = p2;
						txcrh.xiwgo(ref p2, ref this);
						flag = false;
						return;
					}
					goto IL_0124;
				case 1:
					p2 = kqdgy;
					kqdgy = default(kpthf);
					soctk = -1;
					goto IL_0124;
				case 2:
					{
						p = ewibe;
						ewibe = default(xuwyj<TR>);
						soctk = -1;
						break;
					}
					IL_0124:
					p2.ekzxl();
					p2 = default(kpthf);
					p = uqgkn.giftg(p1: false).vuozn();
					if (!p.hqxbj || 1 == 0)
					{
						soctk = 2;
						ewibe = p;
						txcrh.xiwgo(ref p, ref this);
						flag = false;
						return;
					}
					break;
				}
				TR val = p.gbccf();
				p = default(xuwyj<TR>);
				p5 = val;
			}
			catch (Exception p6)
			{
				soctk = -2;
				txcrh.tudwl(p6);
				return;
			}
			soctk = -2;
			txcrh.vzyck(p5);
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in gnwyk
			this.gnwyk();
		}

		private void tterk(fgyyk p0)
		{
			txcrh.viwxd(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in tterk
			this.tterk(p0);
		}
	}

	public static readonly Func<exkzi> zymew;

	private static Func<Exception, exkzi> zirzr;

	private static Func<exkzi> fhsgd;

	public static Func<T> ryipk<T>(this Func<T> p0)
	{
		qmykz<T> qmykz = new qmykz<T>();
		qmykz.bhlaj = p0;
		if (qmykz.bhlaj == null || 1 == 0)
		{
			throw new ArgumentNullException("originalFunction");
		}
		qmykz.aufjz = false;
		qmykz.btggm = default(T);
		return qmykz.umcoq;
	}

	public static Func<TA1, TR> lwhpu<TR, TA1>(this Func<TA1, TR> p0)
	{
		qxxxx<TR, TA1> qxxxx = new qxxxx<TR, TA1>();
		qxxxx.igstb = p0;
		qxxxx.nbpqd = new sroer<TA1, TR>();
		return qxxxx.stvcq;
	}

	public static Func<TR> hgyxz<TR>(this Func<TR> p0, IEnumerable<Func<TR>> p1) where TR : class
	{
		npzbu<TR> npzbu = new npzbu<TR>();
		npzbu.mfaqv = p0;
		npzbu.gejzb = p1;
		if (npzbu.mfaqv == null || 1 == 0)
		{
			throw new ArgumentNullException("firstFunc");
		}
		if (npzbu.gejzb == null || 1 == 0)
		{
			throw new ArgumentNullException("otherFuncs");
		}
		return npzbu.oxkzm;
	}

	public static Func<TR> aruze<TR>(this Func<TR> p0, params Func<TR>[] p1) where TR : class
	{
		kpegc<TR> kpegc = new kpegc<TR>();
		kpegc.qdlcr = p0;
		kpegc.fsjqu = p1;
		if (kpegc.qdlcr == null || 1 == 0)
		{
			throw new ArgumentNullException("firstFunc");
		}
		if (kpegc.fsjqu == null || 1 == 0)
		{
			throw new ArgumentNullException("otherFuncs");
		}
		kpegc.qhhnh = kpegc.fsjqu.Length > 0;
		return kpegc.ubbcy;
	}

	public static Func<TA1, TR> lkbmr<TA1, TR>(this Func<TA1, TR> p0, params Func<TA1, TR>[] p1) where TR : class
	{
		saqpm<TA1, TR> saqpm = new saqpm<TA1, TR>();
		saqpm.nujih = p0;
		saqpm.qwjbg = p1;
		if (saqpm.nujih == null || 1 == 0)
		{
			throw new ArgumentNullException("firstFunc");
		}
		if (saqpm.qwjbg == null || 1 == 0)
		{
			throw new ArgumentNullException("otherFuncs");
		}
		return saqpm.nfkib;
	}

	public static Func<TR> vcxhp<TA1, TR>(this Func<TA1, TR> p0, TA1 p1)
	{
		yqpcz<TA1, TR> yqpcz = new yqpcz<TA1, TR>();
		yqpcz.talxi = p0;
		yqpcz.rekzz = p1;
		if (yqpcz.talxi == null || 1 == 0)
		{
			throw new ArgumentNullException("originalFunc");
		}
		return yqpcz.ysyxe;
	}

	public static Func<gfqoc> iyswc(this Action p0)
	{
		fzngz fzngz = new fzngz();
		fzngz.xtvfk = p0;
		if (fzngz.xtvfk == null || 1 == 0)
		{
			throw new ArgumentNullException("action");
		}
		return fzngz.qmlkh;
	}

	public static Func<TA, gfqoc> kbiid<TA>(this Action<TA> p0)
	{
		mhzio<TA> mhzio = new mhzio<TA>();
		mhzio.ynttd = p0;
		if (mhzio.ynttd == null || 1 == 0)
		{
			throw new ArgumentNullException("action");
		}
		return mhzio.zexja;
	}

	public static Func<TA, TC> npory<TA, TB, TC>(this Func<TA, TB> p0, Func<TB, TC> p1)
	{
		uzuvu<TA, TB, TC> uzuvu = new uzuvu<TA, TB, TC>();
		uzuvu.tggan = p0;
		uzuvu.qwznv = p1;
		if (uzuvu.tggan == null || 1 == 0)
		{
			throw new ArgumentNullException("firstFunc");
		}
		if (uzuvu.qwznv == null || 1 == 0)
		{
			throw new ArgumentNullException("secondFunc");
		}
		return uzuvu.onhwc;
	}

	public static TR ggney<TA1, TR>(this Func<TA1, TR> p0, TA1 p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("func");
		}
		return p0(p1);
	}

	public static Func<TA2, TR> cmdve<TA1, TA2, TR>(this Func<TA1, TA2, TR> p0, TA1 p1)
	{
		rblnp<TA1, TA2, TR> rblnp = new rblnp<TA1, TA2, TR>();
		rblnp.yjumt = p0;
		rblnp.xsgkx = p1;
		if (rblnp.yjumt == null || 1 == 0)
		{
			throw new ArgumentNullException("func");
		}
		return rblnp.tjgij;
	}

	public static Func<TA2, TA3, TR> yjvko<TA1, TA2, TA3, TR>(this Func<TA1, TA2, TA3, TR> p0, TA1 p1)
	{
		elbfr<TA1, TA2, TA3, TR> elbfr = new elbfr<TA1, TA2, TA3, TR>();
		elbfr.hisuf = p0;
		elbfr.wqxkb = p1;
		if (elbfr.hisuf == null || 1 == 0)
		{
			throw new ArgumentNullException("func");
		}
		return elbfr.vdlqy;
	}

	public static Func<TA2, TA3, TA4, TR> jubkg<TA1, TA2, TA3, TA4, TR>(this Func<TA1, TA2, TA3, TA4, TR> p0, TA1 p1)
	{
		euaii<TA1, TA2, TA3, TA4, TR> euaii = new euaii<TA1, TA2, TA3, TA4, TR>();
		euaii.rusig = p0;
		euaii.cvrjz = p1;
		if (euaii.rusig == null || 1 == 0)
		{
			throw new ArgumentNullException("func");
		}
		return euaii.bzfgr;
	}

	public static Func<TA2, TA3, TA4, TA5, TR> fkrid<TA1, TA2, TA3, TA4, TA5, TR>(this qsdrh<TA1, TA2, TA3, TA4, TA5, TR> p0, TA1 p1)
	{
		xndjd<TA1, TA2, TA3, TA4, TA5, TR> xndjd = new xndjd<TA1, TA2, TA3, TA4, TA5, TR>();
		xndjd.bvkur = p0;
		xndjd.plemm = p1;
		if (xndjd.bvkur == null || 1 == 0)
		{
			throw new ArgumentNullException("func");
		}
		return xndjd.abink;
	}

	public static Func<bool> ntity(this Func<bool> p0, Func<bool> p1)
	{
		mzrsz mzrsz = new mzrsz();
		mzrsz.lkilb = p0;
		mzrsz.wynia = p1;
		if (mzrsz.lkilb == null || 1 == 0)
		{
			throw new ArgumentNullException("originalCondition");
		}
		if (mzrsz.wynia == null || 1 == 0)
		{
			throw new ArgumentNullException("nextCondition");
		}
		return mzrsz.nlfro;
	}

	public static Func<TA1, bool> zrwgu<TA1>(this Func<TA1, bool> p0, Func<TA1, bool> p1)
	{
		bobqm<TA1> bobqm = new bobqm<TA1>();
		bobqm.dhlvf = p0;
		bobqm.fskyd = p1;
		if (bobqm.dhlvf == null || 1 == 0)
		{
			throw new ArgumentNullException("originalCondition");
		}
		if (bobqm.fskyd == null || 1 == 0)
		{
			throw new ArgumentNullException("nextCondition");
		}
		return bobqm.sliov;
	}

	public static Func<bool> mllra(this Func<bool> p0, Func<bool> p1)
	{
		iqlpi iqlpi = new iqlpi();
		iqlpi.eoaye = p0;
		iqlpi.faomy = p1;
		if (iqlpi.eoaye == null || 1 == 0)
		{
			throw new ArgumentNullException("originalCondition");
		}
		if (iqlpi.faomy == null || 1 == 0)
		{
			throw new ArgumentNullException("nextCondition");
		}
		return iqlpi.osgyf;
	}

	public static Func<njvzu<T>> satar<T>(this Func<T> p0)
	{
		ldpoa<T> ldpoa = new ldpoa<T>();
		ldpoa.zkzbq = p0;
		if (ldpoa.zkzbq == null || 1 == 0)
		{
			throw new ArgumentNullException("originalFunction");
		}
		return ldpoa.lpuxs;
	}

	public static TR cqxhx<TR>(this Func<TR> p0, Func<Exception, TR> p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("originalFunc");
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("exceptionalFunc");
		}
		try
		{
			return p0();
		}
		catch (Exception arg)
		{
			return p1(arg);
		}
	}

	public static TR ebltf<TR>(this Func<TR> p0)
	{
		return p0.cqxhx(hqmtg<TR>);
	}

	public static void amdpj(this Action p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("action");
		}
		p0.iyswc().ebltf();
	}

	public static Func<TR> cqupj<TR>(this Func<TR> p0, Func<Exception, TR> p1)
	{
		cwfua<TR> cwfua = new cwfua<TR>();
		cwfua.keagu = p0;
		cwfua.yijdv = p1;
		if (cwfua.keagu == null || 1 == 0)
		{
			throw new ArgumentNullException("originalFunc");
		}
		if (cwfua.yijdv == null || 1 == 0)
		{
			throw new ArgumentNullException("exceptionalFunc");
		}
		return cwfua.kqavj;
	}

	public static Func<TR> qzlny<TR>(this Func<TR> p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("originalFunc");
		}
		return p0.ebltf;
	}

	public static Func<TA, exkzi> nlkfe<TA>(this Func<TA, exkzi> p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("originalFunc");
		}
		return p0.cjtrd;
	}

	public static Func<TA1, TA2, exkzi> aggax<TA1, TA2>(this Func<TA1, TA2, exkzi> p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("originalFunc");
		}
		return p0.hcjoj;
	}

	public static Func<gfqoc> rlzij<TR>(this Func<gfqoc> p0, Func<Exception, gfqoc> p1)
	{
		nxhyy<TR> nxhyy = new nxhyy<TR>();
		nxhyy.cvgnj = p0;
		nxhyy.yezce = p1;
		if (nxhyy.cvgnj == null || 1 == 0)
		{
			throw new ArgumentNullException("originalFunc");
		}
		if (nxhyy.yezce == null || 1 == 0)
		{
			throw new ArgumentNullException("exceptionalFunc");
		}
		return nxhyy.znlhd;
	}

	public static Func<gfqoc> ekvdb<TR>(this Action p0, Action<Exception> p1)
	{
		zpnze<TR> zpnze = new zpnze<TR>();
		zpnze.tercs = p0;
		zpnze.edkju = p1;
		if (zpnze.tercs == null || 1 == 0)
		{
			throw new ArgumentNullException("originalAction");
		}
		if (zpnze.edkju == null || 1 == 0)
		{
			throw new ArgumentNullException("exceptionalAction");
		}
		return zpnze.tfhtv;
	}

	public static exkzi qlzna(this Func<exkzi> p0, Func<Exception, exkzi> p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("originalFunc");
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("exceptionalFunc");
		}
		return bmkth(p0, p1);
	}

	public static njvzu<TR> vucbs<TR>(this Func<njvzu<TR>> p0, Func<Exception, njvzu<TR>> p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("originalFunc");
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("exceptionalFunc");
		}
		return shjca(p0, p1);
	}

	public static njvzu<TR> mjspu<TR>(this Func<njvzu<TR>> p0)
	{
		return p0.vucbs(kfugc<TR>);
	}

	[vtsnh(typeof(_003CWithTaskExceptionGuard_003Ed__41<>))]
	public static exkzi amlsh<TA>(this Func<TA, exkzi> p0, TA p1, Func<Exception, exkzi> p2)
	{
		_003CWithTaskExceptionGuard_003Ed__41<TA> p3 = default(_003CWithTaskExceptionGuard_003Ed__41<TA>);
		p3.drpgs = p0;
		p3.hflgy = p1;
		p3.isqeg = p2;
		p3.nuugv = ljmxa.nmskg();
		p3.wrdyv = -1;
		ljmxa nuugv = p3.nuugv;
		nuugv.nncuo(ref p3);
		return p3.nuugv.donjp;
	}

	[vtsnh(typeof(_003CWithTaskExceptionGuard_003Ed__45<, >))]
	public static exkzi oolrq<TA1, TA2>(this Func<TA1, TA2, exkzi> p0, TA1 p1, TA2 p2, Func<Exception, exkzi> p3)
	{
		_003CWithTaskExceptionGuard_003Ed__45<TA1, TA2> p4 = default(_003CWithTaskExceptionGuard_003Ed__45<TA1, TA2>);
		p4.exruj = p0;
		p4.wound = p1;
		p4.xhrbo = p2;
		p4.vmcns = p3;
		p4.lypzl = ljmxa.nmskg();
		p4.xhkwn = -1;
		ljmxa lypzl = p4.lypzl;
		lypzl.nncuo(ref p4);
		return p4.lypzl.donjp;
	}

	public static exkzi cjtrd<TA>(this Func<TA, exkzi> p0, TA p1)
	{
		return p0.amlsh(p1, ptzoz<TA>);
	}

	public static exkzi hcjoj<TA1, TA2>(this Func<TA1, TA2, exkzi> p0, TA1 p1, TA2 p2)
	{
		return p0.oolrq(p1, p2, qzbax<TA1, TA2>);
	}

	public static exkzi jrrmx(this Func<exkzi> p0)
	{
		if (zirzr == null || 1 == 0)
		{
			zirzr = jyipy;
		}
		return p0.qlzna(zirzr);
	}

	public static exkzi orims(this exkzi p0, Func<exkzi> p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("task");
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("finallyAction");
		}
		return kihgh(p0, p1);
	}

	public static njvzu<TR> tctma<TR>(this njvzu<TR> p0, Func<exkzi> p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("task");
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("finallyAction");
		}
		return yxfks(p0, p1);
	}

	[vtsnh(typeof(_003CwithTaskExceptionGuardInner_003Ed__4d))]
	private static exkzi bmkth(Func<exkzi> p0, Func<Exception, exkzi> p1)
	{
		_003CwithTaskExceptionGuardInner_003Ed__4d p2 = default(_003CwithTaskExceptionGuardInner_003Ed__4d);
		p2.bcrtg = p0;
		p2.zozba = p1;
		p2.kvwoe = ljmxa.nmskg();
		p2.qnqbm = -1;
		ljmxa kvwoe = p2.kvwoe;
		kvwoe.nncuo(ref p2);
		return p2.kvwoe.donjp;
	}

	[vtsnh(typeof(_003CwithTaskExceptionGuardInner_003Ed__51<>))]
	private static njvzu<TR> shjca<TR>(Func<njvzu<TR>> p0, Func<Exception, njvzu<TR>> p1)
	{
		_003CwithTaskExceptionGuardInner_003Ed__51<TR> p2 = default(_003CwithTaskExceptionGuardInner_003Ed__51<TR>);
		p2.cmabc = p0;
		p2.mwdvq = p1;
		p2.eljgn = vxvbw<TR>.rdzxj();
		p2.uorgk = -1;
		vxvbw<TR> eljgn = p2.eljgn;
		eljgn.vklen(ref p2);
		return p2.eljgn.xieya;
	}

	[vtsnh(typeof(_003CinnerFinally_003Ed__55))]
	private static exkzi kihgh(exkzi p0, Func<exkzi> p1)
	{
		_003CinnerFinally_003Ed__55 p2 = default(_003CinnerFinally_003Ed__55);
		p2.vtoyw = p0;
		p2.jnpmr = p1;
		p2.boiky = ljmxa.nmskg();
		p2.fhddz = -1;
		ljmxa boiky = p2.boiky;
		boiky.nncuo(ref p2);
		return p2.boiky.donjp;
	}

	[vtsnh(typeof(_003CinnerFinally_003Ed__58<>))]
	private static njvzu<TR> yxfks<TR>(njvzu<TR> p0, Func<exkzi> p1)
	{
		_003CinnerFinally_003Ed__58<TR> p2 = default(_003CinnerFinally_003Ed__58<TR>);
		p2.uqgkn = p0;
		p2.eehzi = p1;
		p2.txcrh = vxvbw<TR>.rdzxj();
		p2.soctk = -1;
		vxvbw<TR> txcrh = p2.txcrh;
		txcrh.vklen(ref p2);
		return p2.txcrh.xieya;
	}

	private static TR gkzik<TR>(Func<TR>[] p0) where TR : class
	{
		int num = 0;
		if (num != 0)
		{
			goto IL_000a;
		}
		goto IL_0030;
		IL_000a:
		Func<TR> func = p0[num];
		TR val = func();
		if (val != null && 0 == 0)
		{
			return val;
		}
		num++;
		goto IL_0030;
		IL_0030:
		if (num >= p0.Length)
		{
			return null;
		}
		goto IL_000a;
	}

	private static void wyido(Exception p0)
	{
	}

	private static TR hqmtg<TR>(Exception p0)
	{
		wyido(p0);
		return default(TR);
	}

	private static njvzu<TR> kfugc<TR>(Exception p0)
	{
		wyido(p0);
		return rxpjc.caxut(default(TR));
	}

	private static exkzi ptzoz<TA>(Exception p0)
	{
		wyido(p0);
		return rxpjc.iccat;
	}

	private static exkzi qzbax<TA1, TA2>(Exception p0)
	{
		wyido(p0);
		return rxpjc.iccat;
	}

	private static exkzi jyipy(Exception p0)
	{
		wyido(p0);
		return rxpjc.iccat;
	}

	static zvcde()
	{
		if (fhsgd == null || 1 == 0)
		{
			fhsgd = ecbpq;
		}
		zymew = fhsgd;
	}

	private static exkzi ecbpq()
	{
		return rxpjc.iccat;
	}
}
