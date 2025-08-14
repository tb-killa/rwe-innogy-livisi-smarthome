using System;
using System.Collections.Generic;
using System.IO;

namespace onrkn;

internal class zypui : jxtqv, IDisposable
{
	private Stream siygt;

	private byte[] ujaki = new byte[16];

	private long tuifl;

	private int lgyln;

	private int onhqv;

	private int bbygg;

	private int hbzqw;

	private int ywsih;

	private int pzrox;

	private int lcefe;

	private int xvilb;

	private int abgyj;

	private List<int> ulfxx;

	private List<int> nwouh;

	private List<int> oxeal;

	private jnkze mfjnl;

	private Dictionary<string, jnkze> qbynb = new Dictionary<string, jnkze>(StringComparer.OrdinalIgnoreCase);

	internal override Stream kzier => siygt;

	internal override xxolr ulhnp => mfjnl.edlkl;

	internal override Dictionary<string, jnkze> vsjhd => qbynb;

	internal override jnkze hsold => mfjnl;

	internal override List<int> vtwew => ulfxx;

	internal override List<int> iqsju => oxeal;

	internal override List<int> jhtad => nwouh;

	internal override long bogao => tuifl;

	internal override int xgzqc => lgyln;

	internal override int yoagm => onhqv;

	internal override int nrrwk => bbygg;

	internal override int mhpfn => hbzqw;

	internal override int ngrnv => ywsih;

	internal override int imrfe => lcefe;

	internal override int ngjzc
	{
		get
		{
			return abgyj;
		}
		set
		{
			abgyj = value;
		}
	}

	public zypui(string filePath)
		: base(filePath)
	{
		siygt = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
		bool flag = true;
		try
		{
			kivst();
			flag = false;
		}
		finally
		{
			if (flag && 0 == 0)
			{
				kzier.Close();
			}
		}
	}

	public zypui(Stream fileStream, bool leaveOpen)
		: base(fileStream, leaveOpen)
	{
		siygt = fileStream;
		tuifl = fileStream.Position;
		kivst();
	}

	private void kivst()
	{
		if (siygt.Length - tuifl < 512)
		{
			throw new uwkib("File is too short to be a valid Compound file.");
		}
		if (pbjum() != -2226271756974174256L)
		{
			throw new uwkib("File identifier check failed.");
		}
		lhwao(16L);
		eixbq();
		eixbq();
		if (xmvos() != 65534)
		{
			throw new uwkib("Unsupported byte order identifier.");
		}
		bbygg = eixbq();
		if (bbygg < 7)
		{
			throw new uwkib("Invalid sector size.");
		}
		lgyln = 1 << bbygg;
		onhqv = lgyln >> 2;
		ywsih = eixbq();
		if (ywsih <= 0 && ywsih > bbygg)
		{
			throw new uwkib("Invalid short sector size.");
		}
		hbzqw = 1 << ywsih;
		lhwao(6L);
		int num = sywrx();
		if (num < 0)
		{
			throw new uwkib("Invalid directory stream total sector count.");
		}
		int num2 = sywrx();
		if (num2 <= 0)
		{
			throw new uwkib("Invalid sector total count.");
		}
		ulfxx = new List<int>(num2 << bbygg - 2);
		pzrox = sywrx();
		if (pzrox < 0 || pzrox >= ulfxx.Capacity)
		{
			throw new uwkib("Invalid Directory sector ID.");
		}
		sywrx();
		lcefe = sywrx();
		xvilb = sywrx();
		if ((xvilb < 0 && xvilb != -2) || xvilb >= ulfxx.Capacity)
		{
			throw new uwkib("Invalid Short sector ID.");
		}
		int num3 = sywrx();
		if (num3 < 0 || (num3 > 0 && xvilb < 0))
		{
			throw new uwkib("Invalid Short sector total count.");
		}
		oxeal = new List<int>(num3 << bbygg - 2);
		abgyj = sywrx();
		if ((abgyj < 0 && abgyj != -2) || abgyj >= ulfxx.Capacity)
		{
			throw new uwkib("Invalid Master sector ID.");
		}
		int num4 = sywrx();
		if (num4 < 0 || (num4 > 0 && abgyj < 0))
		{
			throw new uwkib("Invalid Master sector total count.");
		}
		nwouh = new List<int>(109 + num4 * (onhqv - 1));
		int num5 = 0;
		if (num5 != 0)
		{
			goto IL_027a;
		}
		goto IL_0291;
		IL_0291:
		if (num5 < 109)
		{
			goto IL_027a;
		}
		if (nwouh.Capacity > 109)
		{
			cwrht(abgyj);
		}
		int num6 = 0;
		if (num6 != 0)
		{
			goto IL_02bc;
		}
		goto IL_036e;
		IL_02bc:
		if (nwouh[num6] < 0)
		{
			if (ulfxx.Count != ulfxx.Capacity)
			{
				throw new uwkib("Invalid MSAT chain definition.");
			}
			goto IL_0380;
		}
		if (ulfxx.Count == ulfxx.Capacity)
		{
			throw new uwkib("Invalid MSAT chain definition.");
		}
		siygt.Seek(bjbna(nwouh[num6]), SeekOrigin.Begin);
		siygt.Flush();
		int num7 = 0;
		if (num7 != 0)
		{
			goto IL_0347;
		}
		goto IL_035e;
		IL_0380:
		if (oxeal.Capacity > 0)
		{
			xxolr xxolr2 = new lofzx(this, xvilb, oxeal.Capacity << 2, isShortStream: false);
			try
			{
				int num8 = 0;
				if (num8 != 0)
				{
					goto IL_03c3;
				}
				goto IL_03ee;
				IL_03c3:
				xxolr2.hxmiq(ujaki, 0, 4);
				oxeal.Add(swxox(ujaki));
				num8++;
				goto IL_03ee;
				IL_03ee:
				if (num8 < oxeal.Capacity)
				{
					goto IL_03c3;
				}
				if (xxolr2.Read(ujaki, 0, 1) > 0)
				{
					throw new uwkib("Invalid SSAT chain definition.");
				}
			}
			finally
			{
				if (xxolr2 != null && 0 == 0)
				{
					((IDisposable)xxolr2).Dispose();
				}
			}
		}
		lofzx lofzx2 = new lofzx(this, pzrox, -1L, isShortStream: false);
		try
		{
			mfjnl = new jnkze(this, lofzx2, 0, string.Empty);
			if ((mfjnl.gvryk < 0 && mfjnl.gvryk != -2) || mfjnl.gvryk >= ulfxx.Capacity)
			{
				throw new uwkib("Invalid Root directory definition ({0}/{1}).", mfjnl.gvryk, ulfxx.Capacity);
			}
			if (mfjnl.hucha >= 0)
			{
				ymnmb(lofzx2, mfjnl.hucha, mfjnl);
			}
			return;
		}
		finally
		{
			if (lofzx2 != null && 0 == 0)
			{
				((IDisposable)lofzx2).Dispose();
			}
		}
		IL_0347:
		ulfxx.Add(sywrx());
		num7++;
		goto IL_035e;
		IL_027a:
		nwouh.Add(sywrx());
		num5++;
		goto IL_0291;
		IL_035e:
		if (num7 < onhqv)
		{
			goto IL_0347;
		}
		num6++;
		goto IL_036e;
		IL_036e:
		if (num6 < nwouh.Count)
		{
			goto IL_02bc;
		}
		goto IL_0380;
	}

	private void cwrht(int p0)
	{
		siygt.Seek(bjbna(p0), SeekOrigin.Begin);
		siygt.Flush();
		int num = onhqv - 1;
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_0031;
		}
		goto IL_0046;
		IL_0031:
		nwouh.Add(sywrx());
		num2++;
		goto IL_0046;
		IL_0046:
		if (num2 >= num)
		{
			p0 = sywrx();
			if (nwouh.Count == nwouh.Capacity)
			{
				if (p0 != -2 && p0 != -1)
				{
					throw new uwkib("Invalid MSAT definition ending ({0}).", p0);
				}
				return;
			}
			if (p0 < 0)
			{
				throw new uwkib("Invalid MSAT definition ({0}).", p0);
			}
			cwrht(p0);
			return;
		}
		goto IL_0031;
	}

	private void ymnmb(lofzx p0, int p1, jnkze p2)
	{
		jnkze jnkze2 = new jnkze(this, p0, p1, p2.ucwew);
		if (jnkze2.ikomv >= 0)
		{
			ymnmb(p0, jnkze2.ikomv, p2);
		}
		p2.jfvgo(jnkze2);
		qbynb.Add(jnkze2.ucwew, jnkze2);
		if (jnkze2.hucha >= 0)
		{
			ymnmb(p0, jnkze2.hucha, jnkze2);
		}
		if (jnkze2.awjpu >= 0)
		{
			ymnmb(p0, jnkze2.awjpu, p2);
		}
	}

	internal ushort xmvos()
	{
		mdikq(2);
		return (ushort)(ujaki[0] | (ujaki[1] << 8));
	}

	internal uint odrne()
	{
		mdikq(4);
		return (uint)(ujaki[0] | (ujaki[1] << 8) | (ujaki[2] << 16) | (ujaki[3] << 24));
	}

	internal ulong qwnvc()
	{
		return odrne() | ((ulong)odrne() << 32);
	}

	internal short eixbq()
	{
		mdikq(2);
		return (short)(ujaki[0] | (ujaki[1] << 8));
	}

	internal int sywrx()
	{
		mdikq(4);
		return ujaki[0] | (ujaki[1] << 8) | (ujaki[2] << 16) | (ujaki[3] << 24);
	}

	internal long pbjum()
	{
		return (long)(odrne() | ((ulong)odrne() << 32));
	}

	private void mdikq(int p0)
	{
		xftfn(ujaki, p0);
	}

	internal void xftfn(byte[] p0, int p1)
	{
		int num = 0;
		if (num != 0)
		{
			goto IL_0006;
		}
		goto IL_0034;
		IL_0006:
		int num2 = siygt.Read(p0, num, p1);
		if (num2 <= 0)
		{
			throw new uwkib("Not enough data in the file.");
		}
		num += num2;
		p1 -= num2;
		goto IL_0034;
		IL_0034:
		if (p1 <= 0)
		{
			return;
		}
		goto IL_0006;
	}

	internal void lhwao(long p0)
	{
		siygt.Seek(p0, SeekOrigin.Current);
		siygt.Flush();
	}

	internal static int swxox(byte[] p0)
	{
		return p0[0] | (p0[1] << 8) | (p0[2] << 16) | (p0[3] << 24);
	}
}
