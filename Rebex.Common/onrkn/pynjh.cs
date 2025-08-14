using System;
using System.Runtime.InteropServices;

namespace onrkn;

internal class pynjh
{
	[StructLayout(LayoutKind.Auto)]
	private struct _003CEnsureBufferLength_003Ed__0 : fgyyk
	{
		public int lngde;

		public ljmxa xgpck;

		public pynjh mwiue;

		public int fhipk;

		private xuwyj<int> wevze;

		private object dbcmw;

		private void nivnl()
		{
			try
			{
				bool flag = true;
				if (lngde != 0)
				{
					if (fhipk > mwiue.hycpz.babva)
					{
						throw new ArgumentOutOfRangeException("length", "Length exceeded the underlying buffer size.");
					}
					goto IL_00e2;
				}
				xuwyj<int> p = wevze;
				wevze = default(xuwyj<int>);
				lngde = -1;
				goto IL_00ca;
				IL_00ca:
				p.gbccf();
				p = default(xuwyj<int>);
				goto IL_00e2;
				IL_00e2:
				if (mwiue.hycpz.macew < fhipk)
				{
					p = mwiue.hycpz.flqvi(mwiue.orfjz.rhjom).giftg(p1: false).vuozn();
					if (!p.hqxbj || 1 == 0)
					{
						lngde = 0;
						wevze = p;
						xgpck.wqiyk(ref p, ref this);
						flag = false;
						return;
					}
					goto IL_00ca;
				}
			}
			catch (Exception p2)
			{
				lngde = -2;
				xgpck.iurqb(p2);
				return;
			}
			lngde = -2;
			xgpck.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in nivnl
			this.nivnl();
		}

		private void nsbta(fgyyk p0)
		{
			xgpck.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in nsbta
			this.nsbta(p0);
		}
	}

	private sealed class rnutx<T0>
	{
		public pynjh ammbe;

		public int nbisv;

		public Func<nxtme<byte>, T0> nycsr;

		public T0 jhykf(exkzi p0)
		{
			return ammbe.idxfw(nbisv, nycsr);
		}
	}

	private sealed class wmevv<T0>
	{
		public T0 zhlzp;

		public int auvkt;

		public Func<nxtme<byte>, T0> zfwvv;

		public int cesxz(ArraySegment<byte> p0)
		{
			zhlzp = zfwvv(p0);
			return auvkt;
		}
	}

	private sealed class jgenk
	{
		public pynjh bjrpb;

		public int hhktn;

		public byte[] pahle(exkzi p0)
		{
			return bjrpb.ixnqx(hhktn);
		}
	}

	private readonly mggni orfjz;

	private ydtkc<byte> llfav;

	public ydtkc<byte> hycpz
	{
		get
		{
			return llfav;
		}
		private set
		{
			llfav = value;
		}
	}

	public pynjh(ydtkc<byte> buffer, mggni channel)
	{
		if (buffer == null || 1 == 0)
		{
			throw new ArgumentNullException("buffer");
		}
		hycpz = buffer;
		if (channel == null || 1 == 0)
		{
			throw new ArgumentNullException("channel");
		}
		orfjz = channel;
	}

	[vtsnh(typeof(_003CEnsureBufferLength_003Ed__0))]
	public exkzi wdaeu(int p0)
	{
		_003CEnsureBufferLength_003Ed__0 p1 = default(_003CEnsureBufferLength_003Ed__0);
		p1.mwiue = this;
		p1.fhipk = p0;
		p1.xgpck = ljmxa.nmskg();
		p1.lngde = -1;
		ljmxa xgpck = p1.xgpck;
		xgpck.nncuo(ref p1);
		return p1.xgpck.donjp;
	}

	public byte fosmh(byte p0, string p1 = null)
	{
		byte b = hycpz.jayzd(1)[0];
		if (p0 != b)
		{
			string text = p1;
			if (text == null || 1 == 0)
			{
				text = "Unexpected byte in message. Expected 0x" + p0.ToString("X2") + ", got 0x" + b.ToString("X2") + ".";
			}
			throw new pqotq(text);
		}
		hycpz.pzwbh(1);
		return p0;
	}

	public byte tbben(Predicate<byte> p0, string p1 = null)
	{
		byte b = hycpz.jayzd(1)[0];
		if (!p0(b) || 1 == 0)
		{
			object obj = p1;
			if (obj == null || 1 == 0)
			{
				obj = "Encountered invalid token.";
			}
			throw new pqotq((string)obj);
		}
		hycpz.pzwbh(1);
		return b;
	}

	public njvzu<T> pkqyh<T>(int p0, Func<nxtme<byte>, T> p1)
	{
		rnutx<T> rnutx = new rnutx<T>();
		rnutx.nbisv = p0;
		rnutx.nycsr = p1;
		rnutx.ammbe = this;
		return wdaeu(rnutx.nbisv).gqrnv(rnutx.jhykf);
	}

	public T idxfw<T>(int p0, Func<nxtme<byte>, T> p1)
	{
		wmevv<T> wmevv = new wmevv<T>();
		wmevv.auvkt = p0;
		wmevv.zfwvv = p1;
		wmevv.zhlzp = default(T);
		hycpz.oajvl(wmevv.cesxz);
		return wmevv.zhlzp;
	}

	public njvzu<byte[]> lgjyi(int p0)
	{
		jgenk jgenk = new jgenk();
		jgenk.hhktn = p0;
		jgenk.bjrpb = this;
		return wdaeu(jgenk.hhktn).gqrnv(jgenk.pahle);
	}

	public byte[] ixnqx(int p0)
	{
		ArraySegment<byte> arraySegment = hycpz.rgffm(p0);
		byte[] array = new byte[p0];
		Array.Copy(arraySegment.Array, arraySegment.Offset, array, 0, p0);
		hycpz.pzwbh(p0);
		return array;
	}

	public T xzoul<T>()
	{
		Type underlyingType = Enum.GetUnderlyingType(typeof(T));
		if ((object)underlyingType == typeof(byte))
		{
			nxtme<byte> nxtme2 = hycpz.jayzd(1);
			T result = (T)Enum.ToObject(typeof(T), (object)nxtme2[0]);
			hycpz.pzwbh(1);
			return result;
		}
		throw new NotSupportedException();
	}

	public njvzu<T> afgwl<T>()
	{
		Type underlyingType = Enum.GetUnderlyingType(typeof(T));
		if ((object)underlyingType == typeof(byte))
		{
			return wdaeu(1).gqrnv(arhgh<T>);
		}
		throw new NotSupportedException();
	}

	public njvzu<ushort> eejcn()
	{
		return wdaeu(2).gqrnv(kazon);
	}

	public ushort wnmjb()
	{
		ArraySegment<byte> arraySegment = hycpz.rgffm(2);
		ushort result = jlfbq.kougd(arraySegment.Array, arraySegment.Offset);
		hycpz.pzwbh(2);
		return result;
	}

	public njvzu<byte> khotx()
	{
		return wdaeu(1).gqrnv(rbuzq);
	}

	public byte uopgg()
	{
		byte result = hycpz.jayzd(1)[0];
		hycpz.pzwbh(1);
		return result;
	}

	private T arhgh<T>(exkzi p0)
	{
		return xzoul<T>();
	}

	private ushort kazon(exkzi p0)
	{
		return wnmjb();
	}

	private byte rbuzq(exkzi p0)
	{
		return uopgg();
	}
}
