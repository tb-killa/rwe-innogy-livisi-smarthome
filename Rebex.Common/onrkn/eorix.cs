using System;
using System.Runtime.InteropServices;

namespace onrkn;

internal static class eorix
{
	[StructLayout(LayoutKind.Auto)]
	private struct _003CReceiveAllAsync_003Ed__0 : fgyyk
	{
		public int bdvfm;

		public ljmxa amhjo;

		public cyhjf ocqnn;

		public nxtme<byte> gqvso;

		public int hpkbh;

		public int mvmcc;

		private xuwyj<int> ztabq;

		private object nzduu;

		private void qzbrn()
		{
			try
			{
				bool flag = true;
				if (bdvfm != 0)
				{
					hpkbh = gqvso.tvoem;
					goto IL_00ef;
				}
				xuwyj<int> p = ztabq;
				ztabq = default(xuwyj<int>);
				bdvfm = -1;
				goto IL_00bb;
				IL_00bb:
				int num = p.gbccf();
				p = default(xuwyj<int>);
				int num2 = num;
				mvmcc = num2;
				hpkbh -= mvmcc;
				goto IL_00ef;
				IL_00ef:
				if (hpkbh > 0)
				{
					p = ocqnn.rhjom(gqvso.xjycg(gqvso.tvoem - hpkbh)).giftg(p1: false).vuozn();
					if (!p.hqxbj || 1 == 0)
					{
						bdvfm = 0;
						ztabq = p;
						amhjo.wqiyk(ref p, ref this);
						flag = false;
						return;
					}
					goto IL_00bb;
				}
			}
			catch (Exception p2)
			{
				bdvfm = -2;
				amhjo.iurqb(p2);
				return;
			}
			bdvfm = -2;
			amhjo.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in qzbrn
			this.qzbrn();
		}

		private void szxdg(fgyyk p0)
		{
			amhjo.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in szxdg
			this.szxdg(p0);
		}
	}

	private sealed class vccoj
	{
		public jghfk lerzb;

		public void ymvix(exkzi p0)
		{
			sfmwy(lerzb, p0);
		}
	}

	private sealed class tlmgj
	{
		public jghfk ohoac;

		public void aemxw(exkzi p0)
		{
			sfmwy(ohoac, p0);
		}
	}

	private sealed class mblat
	{
		public jghfk bjpah;

		public void usdwq(exkzi p0)
		{
			sfmwy(bjpah, p0);
		}
	}

	private sealed class tfubz
	{
		public jghfk dhhft;

		public void ouwrn(exkzi p0)
		{
			sfmwy(dhhft, p0);
		}
	}

	private sealed class ivixa
	{
		public cyhjf fsmqr;

		public int wgtbc(njvzu<int> p0)
		{
			return dsbpq(fsmqr, p0);
		}
	}

	private sealed class hayub
	{
		public cyhjf bhmwd;

		public int nhgbg(njvzu<int> p0)
		{
			return dsbpq(bhmwd, p0);
		}
	}

	private sealed class usrev
	{
		public cyhjf mtsob;

		public int nkuyn(njvzu<int> p0)
		{
			return dsbpq(mtsob, p0);
		}
	}

	private sealed class uhlty
	{
		public cyhjf aipha;

		public int uxgiw(njvzu<int> p0)
		{
			return dsbpq(aipha, p0);
		}
	}

	private sealed class ngkue
	{
		public jghfk swjsz;

		public int pesbh(njvzu<int> p0)
		{
			return dsbpq(swjsz, p0);
		}
	}

	private sealed class rioea
	{
		public jghfk ebsmc;

		public int etdqy(njvzu<int> p0)
		{
			return dsbpq(ebsmc, p0);
		}
	}

	private sealed class ojwzh
	{
		public jghfk tmubm;

		public int gumas(njvzu<int> p0)
		{
			return dsbpq(tmubm, p0);
		}
	}

	private sealed class ovzug
	{
		public jghfk hncjs;

		public int rkbmz(njvzu<int> p0)
		{
			return dsbpq(hncjs, p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CsendAllAsyncInner_003Ed__29 : fgyyk
	{
		public int nzwln;

		public ljmxa uoqod;

		public jghfk kttog;

		public nxtme<byte> lrpdg;

		public ddmlv nrocr;

		public int ohyng;

		private xuwyj<int> jjplc;

		private object yihxw;

		private void vpvdz()
		{
			try
			{
				bool flag = true;
				if (nzwln != 0)
				{
					goto IL_00cb;
				}
				xuwyj<int> p = jjplc;
				jjplc = default(xuwyj<int>);
				nzwln = -1;
				goto IL_009b;
				IL_009b:
				int num = p.gbccf();
				p = default(xuwyj<int>);
				int num2 = num;
				ohyng = num2;
				lrpdg = lrpdg.xjycg(ohyng);
				goto IL_00cb;
				IL_00cb:
				if (!lrpdg.hvbtp)
				{
					nrocr.uxxyi();
					p = kttog.razzy(lrpdg).giftg(p1: false).vuozn();
					if (!p.hqxbj || 1 == 0)
					{
						nzwln = 0;
						jjplc = p;
						uoqod.wqiyk(ref p, ref this);
						flag = false;
						return;
					}
					goto IL_009b;
				}
			}
			catch (Exception p2)
			{
				nzwln = -2;
				uoqod.iurqb(p2);
				return;
			}
			nzwln = -2;
			uoqod.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in vpvdz
			this.vpvdz();
		}

		private void bryrf(fgyyk p0)
		{
			uoqod.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in bryrf
			this.bryrf(p0);
		}
	}

	[vtsnh(typeof(_003CReceiveAllAsync_003Ed__0))]
	public static exkzi ltkks(this cyhjf p0, nxtme<byte> p1)
	{
		_003CReceiveAllAsync_003Ed__0 p2 = default(_003CReceiveAllAsync_003Ed__0);
		p2.ocqnn = p0;
		p2.gqvso = p1;
		p2.amhjo = ljmxa.nmskg();
		p2.bdvfm = -1;
		ljmxa amhjo = p2.amhjo;
		amhjo.nncuo(ref p2);
		return p2.amhjo.donjp;
	}

	public static exkzi zykkj(this jghfk p0, nxtme<byte> p1)
	{
		return p0.giogq(p1, ddmlv.prdik);
	}

	public static exkzi ldyol(this jghfk p0, nxtme<byte> p1, int p2)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("channel");
		}
		return p0.zykkj(p1).rvbbe(p2);
	}

	public static exkzi iayjv(this jghfk p0, nxtme<byte> p1, TimeSpan p2)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("channel");
		}
		return p0.zykkj(p1).xzlko(p2);
	}

	public static exkzi giogq(this jghfk p0, ArraySegment<byte> p1, ddmlv p2)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("channel");
		}
		if (p1.Array == null || 1 == 0)
		{
			throw new ArgumentException("Array cannot be null.", "data");
		}
		return ujgli(p0, p1, p2).rejkf(p2);
	}

	public static exkzi enoco(this jghfk p0, nxtme<byte> p1, TimeSpan p2, ddmlv p3)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("channel");
		}
		return p0.giogq(p1, p3).xzlko(p2).rejkf(p3);
	}

	public static exkzi kuvjk(this jghfk p0, nxtme<byte> p1, int p2, ddmlv p3)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("channel");
		}
		return p0.giogq(p1, p3).rvbbe(p2).rejkf(p3);
	}

	public static exkzi ybcxe(this jghfk p0, ArraySegment<byte> p1, int p2, ddmlv p3)
	{
		vccoj vccoj = new vccoj();
		vccoj.lerzb = p0;
		if (vccoj.lerzb == null || 1 == 0)
		{
			throw new ArgumentNullException("channel");
		}
		return vccoj.lerzb.giogq(p1, p3).rejkf(p3).rvbbe(p2)
			.kvzxl(vccoj.ymvix);
	}

	public static exkzi lqqzn(this jghfk p0, ArraySegment<byte> p1, TimeSpan p2, ddmlv p3)
	{
		tlmgj tlmgj = new tlmgj();
		tlmgj.ohoac = p0;
		if (tlmgj.ohoac == null || 1 == 0)
		{
			throw new ArgumentNullException("channel");
		}
		return tlmgj.ohoac.giogq(p1, p3).rejkf(p3).xzlko(p2)
			.kvzxl(tlmgj.aemxw);
	}

	public static exkzi gnbdx(this jghfk p0, ArraySegment<byte> p1, TimeSpan p2)
	{
		mblat mblat = new mblat();
		mblat.bjpah = p0;
		if (mblat.bjpah == null || 1 == 0)
		{
			throw new ArgumentNullException("channel");
		}
		return mblat.bjpah.zykkj(p1).xzlko(p2).kvzxl(mblat.usdwq);
	}

	public static exkzi fduof(this jghfk p0, ArraySegment<byte> p1, int p2)
	{
		tfubz tfubz = new tfubz();
		tfubz.dhhft = p0;
		if (tfubz.dhhft == null || 1 == 0)
		{
			throw new ArgumentNullException("channel");
		}
		return tfubz.dhhft.zykkj(p1).rvbbe(p2).kvzxl(tfubz.ouwrn);
	}

	public static njvzu<int> tyatw(this cyhjf p0, ArraySegment<byte> p1, int p2)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("channel");
		}
		return p0.rhjom(p1).obrzd(p2);
	}

	public static njvzu<int> tfymm(this cyhjf p0, ArraySegment<byte> p1, TimeSpan p2)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("channel");
		}
		return p0.rhjom(p1).xyjnw(p2);
	}

	public static njvzu<int> adedu(this cyhjf p0, ArraySegment<byte> p1, ddmlv p2)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("channel");
		}
		return p0.rhjom(p1).zbvfo(p2);
	}

	public static njvzu<int> nkwco(this cyhjf p0, ArraySegment<byte> p1, int p2, ddmlv p3)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("channel");
		}
		return p0.rhjom(p1).zbvfo(p3).obrzd(p2);
	}

	public static njvzu<int> htmqv(this cyhjf p0, ArraySegment<byte> p1, TimeSpan p2, ddmlv p3)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("channel");
		}
		return p0.rhjom(p1).zbvfo(p3).xyjnw(p2);
	}

	public static njvzu<int> mbzbf(this jghfk p0, ArraySegment<byte> p1, int p2)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("channel");
		}
		return p0.razzy(p1).obrzd(p2);
	}

	public static njvzu<int> irdch(this jghfk p0, ArraySegment<byte> p1, TimeSpan p2)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("channel");
		}
		return p0.razzy(p1).xyjnw(p2);
	}

	public static njvzu<int> pfgly(this jghfk p0, ArraySegment<byte> p1, ddmlv p2)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("channel");
		}
		return p0.razzy(p1).zbvfo(p2);
	}

	public static njvzu<int> jxphb(this jghfk p0, ArraySegment<byte> p1, TimeSpan p2, ddmlv p3)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("channel");
		}
		return p0.razzy(p1).zbvfo(p3).xyjnw(p2);
	}

	public static njvzu<int> ubvkn(this jghfk p0, ArraySegment<byte> p1, int p2, ddmlv p3)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("channel");
		}
		return p0.razzy(p1).zbvfo(p3).obrzd(p2);
	}

	public static njvzu<int> ltsgo(this cyhjf p0, ArraySegment<byte> p1, int p2, ddmlv p3)
	{
		ivixa ivixa = new ivixa();
		ivixa.fsmqr = p0;
		if (ivixa.fsmqr == null || 1 == 0)
		{
			throw new ArgumentNullException("channel");
		}
		return ivixa.fsmqr.rhjom(p1).zbvfo(p3).obrzd(p2)
			.osdty(ivixa.wgtbc);
	}

	public static njvzu<int> ckrkc(this cyhjf p0, ArraySegment<byte> p1, TimeSpan p2, ddmlv p3)
	{
		hayub hayub = new hayub();
		hayub.bhmwd = p0;
		if (hayub.bhmwd == null || 1 == 0)
		{
			throw new ArgumentNullException("channel");
		}
		return hayub.bhmwd.rhjom(p1).zbvfo(p3).xyjnw(p2)
			.osdty(hayub.nhgbg);
	}

	public static njvzu<int> hnvvm(this cyhjf p0, ArraySegment<byte> p1, int p2)
	{
		usrev usrev = new usrev();
		usrev.mtsob = p0;
		if (usrev.mtsob == null || 1 == 0)
		{
			throw new ArgumentNullException("channel");
		}
		return usrev.mtsob.rhjom(p1).obrzd(p2).osdty(usrev.nkuyn);
	}

	public static njvzu<int> bmapc(this cyhjf p0, ArraySegment<byte> p1, TimeSpan p2)
	{
		uhlty uhlty = new uhlty();
		uhlty.aipha = p0;
		if (uhlty.aipha == null || 1 == 0)
		{
			throw new ArgumentNullException("channel");
		}
		return uhlty.aipha.rhjom(p1).xyjnw(p2).osdty(uhlty.uxgiw);
	}

	public static njvzu<int> chxgw(this jghfk p0, ArraySegment<byte> p1, int p2, ddmlv p3)
	{
		ngkue ngkue = new ngkue();
		ngkue.swjsz = p0;
		if (ngkue.swjsz == null || 1 == 0)
		{
			throw new ArgumentNullException("channel");
		}
		return ngkue.swjsz.razzy(p1).zbvfo(p3).obrzd(p2)
			.osdty(ngkue.pesbh);
	}

	public static njvzu<int> vqbax(this jghfk p0, ArraySegment<byte> p1, TimeSpan p2, ddmlv p3)
	{
		rioea rioea = new rioea();
		rioea.ebsmc = p0;
		if (rioea.ebsmc == null || 1 == 0)
		{
			throw new ArgumentNullException("channel");
		}
		return rioea.ebsmc.razzy(p1).zbvfo(p3).xyjnw(p2)
			.osdty(rioea.etdqy);
	}

	public static njvzu<int> djbnz(this jghfk p0, ArraySegment<byte> p1, TimeSpan p2)
	{
		ojwzh ojwzh = new ojwzh();
		ojwzh.tmubm = p0;
		if (ojwzh.tmubm == null || 1 == 0)
		{
			throw new ArgumentNullException("channel");
		}
		return ojwzh.tmubm.razzy(p1).xyjnw(p2).osdty(ojwzh.gumas);
	}

	public static njvzu<int> rpnce(this jghfk p0, ArraySegment<byte> p1, int p2)
	{
		ovzug ovzug = new ovzug();
		ovzug.hncjs = p0;
		if (ovzug.hncjs == null || 1 == 0)
		{
			throw new ArgumentNullException("channel");
		}
		return ovzug.hncjs.razzy(p1).obrzd(p2).osdty(ovzug.rkbmz);
	}

	private static int dsbpq(IDisposable p0, njvzu<int> p1)
	{
		if (suudb(p1) && 0 == 0)
		{
			p0.Dispose();
		}
		p1.xgngc();
		return p1.islme;
	}

	private static void sfmwy(IDisposable p0, exkzi p1)
	{
		if (suudb(p1) && 0 == 0)
		{
			p0.Dispose();
		}
	}

	private static bool suudb(exkzi p0)
	{
		if (p0.ijeei && 0 == 0 && p0.mnscz != null && 0 == 0 && p0.mnscz.mfkfw != null && 0 == 0 && p0.mnscz.mfkfw.Count > 0)
		{
			return p0.mnscz.mfkfw[0] is TimeoutException;
		}
		return false;
	}

	[vtsnh(typeof(_003CsendAllAsyncInner_003Ed__29))]
	private static exkzi ujgli(jghfk p0, nxtme<byte> p1, ddmlv p2)
	{
		_003CsendAllAsyncInner_003Ed__29 p3 = default(_003CsendAllAsyncInner_003Ed__29);
		p3.kttog = p0;
		p3.lrpdg = p1;
		p3.nrocr = p2;
		p3.uoqod = ljmxa.nmskg();
		p3.nzwln = -1;
		ljmxa uoqod = p3.uoqod;
		uoqod.nncuo(ref p3);
		return p3.uoqod.donjp;
	}
}
