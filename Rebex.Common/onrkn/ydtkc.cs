using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace onrkn;

internal class ydtkc<T0> : IDisposable
{
	[StructLayout(LayoutKind.Auto)]
	private struct _003CReadAsync_003Ed__0 : fgyyk
	{
		public int bdmja;

		public hpmhz pxcpb;

		public ydtkc<T0> xvmet;

		public qesik<T0> plgck;

		public ArraySegment<T0> gmnij;

		public int zpist;

		private xuwyj<int> uqapx;

		private object fglpx;

		private void yyhcj()
		{
			try
			{
				bool flag = true;
				xuwyj<int> p;
				if (bdmja != 0)
				{
					gmnij = xvmet.qxmie();
					p = plgck(gmnij).giftg(p1: false).vuozn();
					if (!p.hqxbj || 1 == 0)
					{
						bdmja = 0;
						uqapx = p;
						pxcpb.txsjv(ref p, ref this);
						flag = false;
						return;
					}
				}
				else
				{
					p = uqapx;
					uqapx = default(xuwyj<int>);
					bdmja = -1;
				}
				int num = p.gbccf();
				p = default(xuwyj<int>);
				int num2 = num;
				zpist = num2;
				xvmet.pzwbh(zpist);
			}
			catch (Exception p2)
			{
				bdmja = -2;
				pxcpb.vulbx(p2);
				return;
			}
			bdmja = -2;
			pxcpb.zfyvr();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in yyhcj
			this.yyhcj();
		}

		private void zhemc(fgyyk p0)
		{
			pxcpb.velvv(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in zhemc
			this.zhemc(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CWriteAsync_003Ed__5 : fgyyk
	{
		public int gjoto;

		public vxvbw<int> iigmk;

		public ydtkc<T0> tfxta;

		public tjuiy<T0> iohya;

		public ArraySegment<T0> jpssx;

		public int yzjmh;

		private xuwyj<int> hzpmv;

		private object egsvs;

		private void jvbed()
		{
			int p2;
			try
			{
				bool flag = true;
				xuwyj<int> p;
				if (gjoto != 0)
				{
					jpssx = tfxta.bpeep();
					p = iohya(jpssx).giftg(p1: false).vuozn();
					if (!p.hqxbj || 1 == 0)
					{
						gjoto = 0;
						hzpmv = p;
						iigmk.xiwgo(ref p, ref this);
						flag = false;
						return;
					}
				}
				else
				{
					p = hzpmv;
					hzpmv = default(xuwyj<int>);
					gjoto = -1;
				}
				int num = p.gbccf();
				p = default(xuwyj<int>);
				int num2 = num;
				yzjmh = num2;
				tfxta.oaaie(yzjmh);
				p2 = yzjmh;
			}
			catch (Exception p3)
			{
				gjoto = -2;
				iigmk.tudwl(p3);
				return;
			}
			gjoto = -2;
			iigmk.vzyck(p2);
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in jvbed
			this.jvbed();
		}

		private void aimmb(fgyyk p0)
		{
			iigmk.viwxd(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in aimmb
			this.aimmb(p0);
		}
	}

	private T0[] fwdyg;

	private int bkron;

	private int cjzzv;

	public int macew
	{
		get
		{
			return cjzzv;
		}
		private set
		{
			cjzzv = value;
		}
	}

	public int babva => fwdyg.Length;

	public ydtkc(int minSize)
	{
		fwdyg = sxztb<T0>.ahblv.vfhlp(minSize);
	}

	public void oaaie(int p0)
	{
		if (p0 > 0)
		{
			if (p0 > fwdyg.Length - macew - bkron)
			{
				throw new ArgumentOutOfRangeException();
			}
			macew += p0;
		}
	}

	public void btoki()
	{
		macew = 0;
	}

	public void pzwbh(int p0)
	{
		bkron += p0;
		macew -= p0;
	}

	public void Dispose()
	{
		T0[] array = Interlocked.Exchange(ref fwdyg, null);
		if (array != null && 0 == 0)
		{
			sxztb<T0>.ahblv.uqydw(array);
		}
	}

	public ihlqx<ArraySegment<T0>> zyxao()
	{
		T0[] array = Interlocked.Exchange(ref fwdyg, null);
		if (array == null || 1 == 0)
		{
			throw new ObjectDisposedException("array");
		}
		return ozsrp.rxdfr(array, bkron, macew, sxztb<T0>.ahblv);
	}

	public void velxr(int p0)
	{
		if (p0 > babva)
		{
			T0[] destinationArray = sxztb<T0>.ahblv.vfhlp(p0);
			Array.Copy(fwdyg, bkron, destinationArray, 0, macew);
			sxztb<T0>.ahblv.uqydw(fwdyg);
			bkron = 0;
			fwdyg = destinationArray;
		}
	}

	public ArraySegment<T0> qxmie()
	{
		return new ArraySegment<T0>(fwdyg, bkron, macew);
	}

	public ArraySegment<T0> rgffm(int p0)
	{
		if (p0 > macew)
		{
			throw new ArgumentOutOfRangeException("length", "Not enough items in the buffer");
		}
		return new ArraySegment<T0>(fwdyg, bkron, p0);
	}

	public nxtme<T0> jayzd(int p0)
	{
		if (p0 > macew)
		{
			throw new ArgumentOutOfRangeException("length", "Not enough items in the buffer");
		}
		return new nxtme<T0>(fwdyg, bkron, p0);
	}

	public ArraySegment<T0> bpeep()
	{
		lqnrf();
		int num = bkron + macew;
		return new ArraySegment<T0>(fwdyg, num, fwdyg.Length - num);
	}

	public int oajvl(rtbfu<T0> p0)
	{
		ArraySegment<T0> segment = qxmie();
		int num = p0(segment);
		pzwbh(num);
		return num;
	}

	[vtsnh(typeof(ydtkc<>._003CReadAsync_003Ed__0))]
	public void ojwdt(qesik<T0> p0)
	{
		_003CReadAsync_003Ed__0 p1 = default(_003CReadAsync_003Ed__0);
		p1.xvmet = this;
		p1.plgck = p0;
		p1.pxcpb = hpmhz.karxo();
		p1.bdmja = -1;
		hpmhz pxcpb = p1.pxcpb;
		pxcpb.qnkup(ref p1);
	}

	public int yfqrq(vvcsw<T0> p0)
	{
		ArraySegment<T0> segment = bpeep();
		int num = p0(segment);
		oaaie(num);
		return num;
	}

	[vtsnh(typeof(ydtkc<>._003CWriteAsync_003Ed__5))]
	public njvzu<int> flqvi(tjuiy<T0> p0)
	{
		_003CWriteAsync_003Ed__5 p1 = default(_003CWriteAsync_003Ed__5);
		p1.tfxta = this;
		p1.iohya = p0;
		p1.iigmk = vxvbw<int>.rdzxj();
		p1.gjoto = -1;
		vxvbw<int> iigmk = p1.iigmk;
		iigmk.vklen(ref p1);
		return p1.iigmk.xieya;
	}

	private void lqnrf()
	{
		if (bkron > 0)
		{
			if (macew > 0)
			{
				Array.Copy(fwdyg, bkron, fwdyg, 0, macew);
			}
			bkron = 0;
		}
	}
}
