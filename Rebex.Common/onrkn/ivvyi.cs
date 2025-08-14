using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace onrkn;

internal class ivvyi : IDisposable
{
	[StructLayout(LayoutKind.Auto)]
	private struct _003CAwaitPrevious_003Ed__0 : fgyyk
	{
		public int qexbc;

		public vxvbw<dvxgu<object>> bwkmq;

		public ivvyi nvvrb;

		public dvxgu<object> hlpoa;

		public exkzi xrbld;

		private kpthf vuhsz;

		private object gjaim;

		private void klqqh()
		{
			dvxgu<object> p2;
			try
			{
				bool flag = true;
				if (qexbc != 0)
				{
					nvvrb.hsmng.qncaj.uxxyi();
					hlpoa = new dvxgu<object>();
					xrbld = Interlocked.Exchange(ref nvvrb.nvzye, hlpoa.dioyl);
				}
				try
				{
					kpthf p;
					if (qexbc != 0)
					{
						p = xrbld.avdby(p1: false).vrtmi();
						if (!p.zpafv || 1 == 0)
						{
							qexbc = 0;
							vuhsz = p;
							bwkmq.xiwgo(ref p, ref this);
							flag = false;
							return;
						}
					}
					else
					{
						p = vuhsz;
						vuhsz = default(kpthf);
						qexbc = -1;
					}
					p.ekzxl();
					p = default(kpthf);
				}
				catch
				{
				}
				nvvrb.ggypl(hlpoa);
				p2 = hlpoa;
			}
			catch (Exception p3)
			{
				qexbc = -2;
				bwkmq.tudwl(p3);
				return;
			}
			qexbc = -2;
			bwkmq.vzyck(p2);
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in klqqh
			this.klqqh();
		}

		private void cucwe(fgyyk p0)
		{
			bwkmq.viwxd(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in cucwe
			this.cucwe(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CEnqueue_003Ed__5 : fgyyk
	{
		public int ldrow;

		public ljmxa eqbks;

		public ivvyi lgjmw;

		public Func<exkzi> alyuh;

		public Action<exkzi> ilmdr;

		public dvxgu<object> whwlu;

		public exkzi qhvhh;

		private xuwyj<dvxgu<object>> hfelu;

		private object jzfoe;

		private kpthf qcbyf;

		private void fqcae()
		{
			try
			{
				bool flag = true;
				dvxgu<object> obj;
				xuwyj<dvxgu<object>> p;
				dvxgu<object> dvxgu2;
				switch (ldrow)
				{
				default:
					p = lgjmw.mzubw().giftg(p1: false).vuozn();
					if (!p.hqxbj || 1 == 0)
					{
						ldrow = 0;
						hfelu = p;
						eqbks.wqiyk(ref p, ref this);
						flag = false;
						return;
					}
					goto IL_008a;
				case 0:
					p = hfelu;
					hfelu = default(xuwyj<dvxgu<object>>);
					ldrow = -1;
					goto IL_008a;
				case 1:
					break;
					IL_008a:
					obj = p.gbccf();
					p = default(xuwyj<dvxgu<object>>);
					dvxgu2 = obj;
					whwlu = dvxgu2;
					break;
				}
				try
				{
					int num = ldrow;
					kpthf p2;
					if (num != 1)
					{
						qhvhh = alyuh();
						p2 = qhvhh.avdby(p1: false).vrtmi();
						if (!p2.zpafv || 1 == 0)
						{
							ldrow = 1;
							qcbyf = p2;
							eqbks.wqiyk(ref p2, ref this);
							flag = false;
							return;
						}
					}
					else
					{
						p2 = qcbyf;
						qcbyf = default(kpthf);
						ldrow = -1;
					}
					p2.ekzxl();
					p2 = default(kpthf);
					if (ilmdr != null && 0 == 0)
					{
						ilmdr(qhvhh);
					}
				}
				finally
				{
					if (flag && 0 == 0)
					{
						whwlu.cbgge(null);
					}
				}
			}
			catch (Exception p3)
			{
				ldrow = -2;
				eqbks.iurqb(p3);
				return;
			}
			ldrow = -2;
			eqbks.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in fqcae
			this.fqcae();
		}

		private void zbgvq(fgyyk p0)
		{
			eqbks.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in zbgvq
			this.zbgvq(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CEnqueue_003Ed__b<T> : fgyyk
	{
		public int pkrzu;

		public vxvbw<T> wmqbo;

		public ivvyi rizmp;

		public Func<njvzu<T>> sjhww;

		public Action<njvzu<T>> jhrzf;

		public dvxgu<object> rhmev;

		public njvzu<T> abwny;

		private xuwyj<dvxgu<object>> jztrz;

		private object exmwa;

		private xuwyj<T> knhdv;

		private void xalrm()
		{
			T islme;
			try
			{
				bool flag = true;
				dvxgu<object> obj;
				xuwyj<dvxgu<object>> p;
				dvxgu<object> dvxgu2;
				switch (pkrzu)
				{
				default:
					p = rizmp.mzubw().giftg(p1: false).vuozn();
					if (!p.hqxbj || 1 == 0)
					{
						pkrzu = 0;
						jztrz = p;
						wmqbo.xiwgo(ref p, ref this);
						flag = false;
						return;
					}
					goto IL_008b;
				case 0:
					p = jztrz;
					jztrz = default(xuwyj<dvxgu<object>>);
					pkrzu = -1;
					goto IL_008b;
				case 1:
					break;
					IL_008b:
					obj = p.gbccf();
					p = default(xuwyj<dvxgu<object>>);
					dvxgu2 = obj;
					rhmev = dvxgu2;
					break;
				}
				try
				{
					int num = pkrzu;
					xuwyj<T> p2;
					if (num != 1)
					{
						abwny = sjhww();
						p2 = abwny.giftg(p1: false).vuozn();
						if (!p2.hqxbj || 1 == 0)
						{
							pkrzu = 1;
							knhdv = p2;
							wmqbo.xiwgo(ref p2, ref this);
							flag = false;
							return;
						}
					}
					else
					{
						p2 = knhdv;
						knhdv = default(xuwyj<T>);
						pkrzu = -1;
					}
					p2.gbccf();
					p2 = default(xuwyj<T>);
					if (jhrzf != null && 0 == 0)
					{
						jhrzf(abwny);
					}
					islme = abwny.islme;
				}
				finally
				{
					if (flag && 0 == 0)
					{
						rhmev.cbgge(null);
					}
				}
			}
			catch (Exception p3)
			{
				pkrzu = -2;
				wmqbo.tudwl(p3);
				return;
			}
			pkrzu = -2;
			wmqbo.vzyck(islme);
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in xalrm
			this.xalrm();
		}

		private void pvqut(fgyyk p0)
		{
			wmqbo.viwxd(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in pvqut
			this.pvqut(p0);
		}
	}

	private exkzi nvzye = rxpjc.iccat;

	private readonly znuay hsmng = new znuay();

	[vtsnh(typeof(_003CAwaitPrevious_003Ed__0))]
	private njvzu<dvxgu<object>> mzubw()
	{
		_003CAwaitPrevious_003Ed__0 p = default(_003CAwaitPrevious_003Ed__0);
		p.nvvrb = this;
		p.bwkmq = vxvbw<dvxgu<object>>.rdzxj();
		p.qexbc = -1;
		vxvbw<dvxgu<object>> bwkmq = p.bwkmq;
		bwkmq.vklen(ref p);
		return p.bwkmq.xieya;
	}

	private void ggypl(dvxgu<object> p0)
	{
		if (hsmng.mpjbd && 0 == 0)
		{
			p0.gxdnj();
			hsmng.qncaj.uxxyi();
		}
	}

	[vtsnh(typeof(_003CEnqueue_003Ed__5))]
	public exkzi dhzqc(Func<exkzi> p0, Action<exkzi> p1 = null)
	{
		_003CEnqueue_003Ed__5 p2 = default(_003CEnqueue_003Ed__5);
		p2.lgjmw = this;
		p2.alyuh = p0;
		p2.ilmdr = p1;
		p2.eqbks = ljmxa.nmskg();
		p2.ldrow = -1;
		ljmxa eqbks = p2.eqbks;
		eqbks.nncuo(ref p2);
		return p2.eqbks.donjp;
	}

	[vtsnh(typeof(_003CEnqueue_003Ed__b<>))]
	public njvzu<T> heurk<T>(Func<njvzu<T>> p0, Action<njvzu<T>> p1 = null)
	{
		_003CEnqueue_003Ed__b<T> p2 = default(_003CEnqueue_003Ed__b<T>);
		p2.rizmp = this;
		p2.sjhww = p0;
		p2.jhrzf = p1;
		p2.wmqbo = vxvbw<T>.rdzxj();
		p2.pkrzu = -1;
		vxvbw<T> wmqbo = p2.wmqbo;
		wmqbo.vklen(ref p2);
		return p2.wmqbo.xieya;
	}

	public void Dispose()
	{
		hsmng.pvutk();
	}
}
