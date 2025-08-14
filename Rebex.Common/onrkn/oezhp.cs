using System;
using System.Runtime.InteropServices;

namespace onrkn;

internal class oezhp : bvxhw
{
	private struct cslbi
	{
		public ulong vyorf;

		public ulong wjbtt;
	}

	[StructLayout(LayoutKind.Explicit, Size = 16)]
	private struct qudof
	{
		[FieldOffset(0)]
		public ulong xfkkt;

		[FieldOffset(8)]
		public ulong bhhzc;

		[FieldOffset(0)]
		public byte hxngt;

		[FieldOffset(1)]
		public byte wztzs;

		[FieldOffset(2)]
		public byte fzsnh;

		[FieldOffset(3)]
		public byte ojbsn;

		[FieldOffset(4)]
		public byte xwhux;

		[FieldOffset(5)]
		public byte ekvqe;

		[FieldOffset(6)]
		public byte hykqq;

		[FieldOffset(7)]
		public byte xdyff;

		[FieldOffset(8)]
		public byte zletb;

		[FieldOffset(9)]
		public byte nitas;

		[FieldOffset(10)]
		public byte vrjow;

		[FieldOffset(11)]
		public byte gyqej;

		[FieldOffset(12)]
		public byte gkhwx;

		[FieldOffset(13)]
		public byte odyyo;

		[FieldOffset(14)]
		public byte zueye;

		[FieldOffset(15)]
		public byte jedfw;

		public qudof(ulong low8Bytes, ulong high8Bytes)
		{
			this = default(qudof);
			xfkkt = low8Bytes;
			bhhzc = high8Bytes;
		}

		public void etvik(nxtme<byte> p0)
		{
			if (BitConverter.IsLittleEndian && 0 == 0)
			{
				rnqdw rnqdw2 = p0;
				try
				{
					Marshal.StructureToPtr((object)this, rnqdw2.peara(), fDeleteOld: false);
					return;
				}
				finally
				{
					rnqdw2.fbdzt();
				}
			}
			p0[0] = xdyff;
			p0[1] = hykqq;
			p0[2] = ekvqe;
			p0[3] = xwhux;
			p0[4] = ojbsn;
			p0[5] = fzsnh;
			p0[6] = wztzs;
			p0[7] = hxngt;
			p0[8] = jedfw;
			p0[9] = zueye;
			p0[10] = odyyo;
			p0[11] = gkhwx;
			p0[12] = gyqej;
			p0[13] = vrjow;
			p0[14] = nitas;
			p0[15] = zletb;
		}
	}

	private struct pjnrg
	{
		public ulong iszfi;

		public ulong srnzr;
	}

	private struct umawm
	{
		public ulong qdbxu;

		public ulong ttbkm;
	}

	private struct cqaqz
	{
		private nxtme<byte> mzvrf;

		private int oamdl;

		private int uelli;

		public ulong this[int index]
		{
			get
			{
				byte[] lthjd = mzvrf.lthjd;
				int num = uelli + index * 8;
				uint num2 = (uint)(lthjd[num++] | (lthjd[num++] << 8) | (lthjd[num++] << 16) | (lthjd[num++] << 24));
				uint num3 = (uint)(lthjd[num++] | (lthjd[num++] << 8) | (lthjd[num++] << 16) | (lthjd[num] << 24));
				return ((ulong)num3 << 32) | num2;
			}
		}

		public int pafiz => oamdl;

		public cqaqz(nxtme<byte> srcData)
		{
			this = default(cqaqz);
			mzvrf = srcData;
			oamdl = srcData.hvvsm << 8;
			uelli = srcData.frlfs;
		}
	}

	private const uint cbehm = uint.MaxValue;

	private const int xcvev = 32;

	private const ulong nwibh = 3uL;

	private const ulong fcxjc = 18446744073709551612uL;

	public const int bvrkt = 3;

	private ulong eouja;

	private ulong fpwqk;

	private ulong thote;

	private ulong ndfsn;

	private ulong ntbkk;

	private ulong wbude;

	private ulong bkohv;

	private byte[] dkalp;

	private int vdhqw;

	private bool pglwi;

	private qudof eaqvu;

	public override bool txhsi => pglwi;

	public oezhp(nxtme<byte> key)
	{
		ghfgj(key);
	}

	public override void Initialize()
	{
	}

	public override byte[] GetHash()
	{
		byte[] array = new byte[16];
		zzsom(array);
		return array;
	}

	public override void Reset()
	{
	}

	protected override void vkjuk(nxtme<byte> p0)
	{
		ghfgj(p0);
	}

	protected override void olrhm(ulong p0, ulong p1)
	{
		ppjka(p0, p1, p2: true);
	}

	protected override void qjveg(byte[] p0, int p1, int p2, bool p3)
	{
		gfvsy(p0, p1, p2, p3);
	}

	private void gfvsy(byte[] p0, int p1, int p2, bool p3)
	{
		nxtme<byte> nxtme2 = p0.myshu(p1, p2);
		bool flag = nxtme2.hvvsm + vdhqw >= 16;
		if ((flag ? true : false) || p3)
		{
			if (flag && 0 == 0 && vdhqw > 0)
			{
				int num = 16 - vdhqw;
				nxtme2.jlxhy(0, num).rjwrl(dkalp.myshu(vdhqw, num));
				nxtme2 = nxtme2.xjycg(num);
				tqhgz(dkalp.myshu(0, 16), p1: true);
				uhlsj();
				vdhqw = 0;
			}
			int num2 = nxtme2.hvvsm & 0xF;
			if (nxtme2.hvvsm >= 16)
			{
				tqhgz(nxtme2.jlxhy(0, nxtme2.hvvsm - num2), p1: true);
			}
			if (num2 > 0)
			{
				nxtme2.jlxhy(nxtme2.hvvsm - num2, num2).rjwrl(dkalp.xtagy());
				if (p3 && 0 == 0)
				{
					tqhgz(dkalp.myshu(0, 16), p1: true);
					uhlsj();
					vdhqw = 0;
				}
				else
				{
					vdhqw += num2;
				}
			}
		}
		else
		{
			nxtme2.rjwrl(dkalp.lafmy(vdhqw));
			vdhqw += nxtme2.hvvsm;
		}
	}

	protected override void HashCore(byte[] array, int ibStart, int cbSize)
	{
		dahxy.valft(array, "array", ibStart, "ibStart", cbSize, "cbSize");
		qjveg(array, ibStart, cbSize, p3: false);
	}

	protected override byte[] HashFinal()
	{
		return GetHash();
	}

	protected override void emwui(nxtme<byte> p0)
	{
		woasq(p0);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && 0 == 0 && dkalp != null && 0 == 0)
		{
			uhlsj();
			sxztb<byte>.ahblv.uqydw(dkalp);
		}
		base.Dispose(disposing);
	}

	private void uhlsj()
	{
		Array.Clear(dkalp, 0, dkalp.Length);
	}

	private void ghfgj(nxtme<byte> p0)
	{
		if (p0.hvvsm != 32)
		{
			throw new ArgumentException("Key must have 32 bytes", "key");
		}
		pglwi = false;
		cqaqz cqaqz = new cqaqz(p0);
		eouja = cqaqz[0] & 0xFFFFFFC0FFFFFFFL;
		fpwqk = cqaqz[1] & 0xFFFFFFC0FFFFFFCL;
		thote = cqaqz[2];
		ndfsn = cqaqz[3];
		ntbkk = 0uL;
		wbude = 0uL;
		bkohv = 0uL;
		vdhqw = 0;
		if (dkalp == null || 1 == 0)
		{
			dkalp = sxztb<byte>.ahblv.vfhlp(InputBlockSize);
		}
		uhlsj();
	}

	private void tqhgz(nxtme<byte> p0, bool p1)
	{
		ayjol ayjol2 = peekn.nmjtk(p0.lthjd, p0.frlfs, p0.tvoem, p3: true);
		try
		{
			ulong[] rjqep = ayjol2.rjqep;
			int num = p0.hvvsm >> 3;
			int num2 = 0;
			if (!ayjol2.ygpwi || 1 == 0)
			{
				num2 = p0.frlfs >> 3;
			}
			while (num > 0)
			{
				ulong p2 = rjqep[num2++];
				ulong p3 = rjqep[num2++];
				ppjka(p2, p3, p1);
				num -= 2;
			}
		}
		finally
		{
			ayjol2.uqjka();
		}
	}

	private void ppjka(ulong p0, ulong p1, bool p2)
	{
		ulong p3 = ntbkk;
		ulong p4 = wbude;
		ulong num = bkohv;
		thwnk(p3, p0, 0uL, out var p5);
		p3 = p5.iszfi;
		thwnk(p4, p1, p5.srnzr, out var p6);
		p4 = p6.iszfi;
		num = ((!p2) ? (num + p6.srnzr) : (num + (p6.srnzr + 1)));
		ctivw(p3, eouja, out var p7);
		ctivw(p4, eouja, out var p8);
		ctivw(num, eouja, out var p9);
		ctivw(p3, fpwqk, out var p10);
		ctivw(p4, fpwqk, out var p11);
		ctivw(num, fpwqk, out var p12);
		dcvgp(ref p8, ref p10, out var p13);
		dcvgp(ref p9, ref p11, out var p14);
		ulong vyorf = p7.vyorf;
		thwnk(p13.vyorf, p7.wjbtt, 0uL, out var p15);
		thwnk(p14.vyorf, p13.wjbtt, p15.srnzr, out var p16);
		thwnk(p12.vyorf, p14.wjbtt, p16.srnzr, out var p17);
		ulong num2 = p16.iszfi & 3;
		cslbi p18 = default(cslbi);
		p18.vyorf = p16.iszfi & 0xFFFFFFFFFFFFFFFCuL;
		p18.wjbtt = p17.iszfi;
		thwnk(vyorf, p18.vyorf, 0uL, out var p19);
		thwnk(p15.iszfi, p18.wjbtt, p19.srnzr, out var p20);
		num2 += p20.srnzr;
		ycosg(ref p18);
		thwnk(p19.iszfi, p18.vyorf, 0uL, out p19);
		thwnk(p20.iszfi, p18.wjbtt, p19.srnzr, out p20);
		num2 += p20.srnzr;
		ntbkk = p19.iszfi;
		wbude = p20.iszfi;
		bkohv = num2;
	}

	private void woasq(nxtme<byte> p0)
	{
		if (pglwi && 0 == 0)
		{
			eaqvu.etvik(p0);
			return;
		}
		if (vdhqw > 0)
		{
			dkalp[vdhqw] = 1;
			tqhgz(dkalp, p1: false);
		}
		umawm umawm = zzbhu(ntbkk, 18446744073709551611uL, 0uL);
		umawm umawm2 = zzbhu(wbude, ulong.MaxValue, umawm.ttbkm);
		if (zzbhu(bkohv, 3uL, umawm2.ttbkm).ttbkm == 0)
		{
			ntbkk = umawm.qdbxu;
			wbude = umawm2.qdbxu;
		}
		thwnk(ntbkk, thote, 0uL, out var p1);
		thwnk(wbude, ndfsn, p1.srnzr, out var p2);
		eaqvu = new qudof(p1.iszfi, p2.iszfi);
		pglwi = true;
		eaqvu.etvik(p0);
	}

	private static void thwnk(ulong p0, ulong p1, ulong p2, out pjnrg p3)
	{
		p3.iszfi = p0 + p1 + p2;
		p3.srnzr = ((p0 & p1) | ((p0 | p1) & ~p3.iszfi)) >> 63;
	}

	private static umawm zzbhu(ulong p0, ulong p1, ulong p2)
	{
		umawm result = default(umawm);
		result.qdbxu = p0 - p1 - p2;
		result.ttbkm = ((~p0 & p1) | (~(p0 ^ p1) & result.qdbxu)) >> 63;
		return result;
	}

	private static void dcvgp(ref cslbi p0, ref cslbi p1, out cslbi p2)
	{
		thwnk(p0.vyorf, p1.vyorf, 0uL, out var p3);
		thwnk(p0.wjbtt, p1.wjbtt, p3.srnzr, out var p4);
		p2.vyorf = p3.iszfi;
		p2.wjbtt = p4.iszfi;
	}

	private static void ctivw(ulong p0, ulong p1, out cslbi p2)
	{
		ulong num = p0 & 0xFFFFFFFFu;
		ulong num2 = p0 >> 32;
		ulong num3 = p1 & 0xFFFFFFFFu;
		ulong num4 = p1 >> 32;
		ulong num5 = num * num3;
		ulong num6 = num * num4;
		ulong num7 = num2 * num3;
		ulong num8 = num2 * num4;
		ulong num9 = ((num5 >> 32) + (num6 & 0xFFFFFFFFu) + (num7 & 0xFFFFFFFFu) >> 32) & 0xFFFFFFFFu;
		ulong vyorf = num5 + (num6 << 32) + (num7 << 32);
		ulong wjbtt = num8 + (num6 >> 32) + (num7 >> 32) + num9;
		p2.vyorf = vyorf;
		p2.wjbtt = wjbtt;
	}

	private void ycosg(ref cslbi p0)
	{
		ulong vyorf = (p0.vyorf >> 2) | ((p0.wjbtt & 3) << 62);
		ulong wjbtt = p0.wjbtt >> 2;
		p0.wjbtt = wjbtt;
		p0.vyorf = vyorf;
	}
}
