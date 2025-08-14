using System;
using System.Collections.Generic;
using System.IO;

namespace onrkn;

internal class duqmg : jxtqv, IDisposable
{
	private List<jnkze> ijdor = new List<jnkze>();

	private Stream bmhii;

	private byte[] qyaqr = new byte[16];

	private long vtefs;

	private int dqrbz;

	private int rdkme;

	private int lwozm;

	private int yftlt;

	private int vbidn;

	private int fmfja;

	private int oeinh;

	private int slbpy;

	private int nosgi;

	private List<int> bgcmj;

	private List<int> ljmpl;

	private List<int> hftqd;

	private jnkze opicz;

	private Dictionary<string, jnkze> gqyyr = new Dictionary<string, jnkze>(StringComparer.OrdinalIgnoreCase);

	internal List<jnkze> kuwso => ijdor;

	internal override Stream kzier => bmhii;

	internal override xxolr ulhnp => opicz.edlkl;

	internal override Dictionary<string, jnkze> vsjhd => gqyyr;

	internal override jnkze hsold => opicz;

	internal override List<int> vtwew => bgcmj;

	internal override List<int> iqsju => hftqd;

	internal override List<int> jhtad => ljmpl;

	internal override long bogao => vtefs;

	internal override int xgzqc => dqrbz;

	internal override int yoagm => rdkme;

	internal override int nrrwk => lwozm;

	internal override int mhpfn => yftlt;

	internal override int ngrnv => vbidn;

	internal override int imrfe => oeinh;

	internal override int ngjzc
	{
		get
		{
			return nosgi;
		}
		set
		{
			nosgi = value;
		}
	}

	public duqmg(string filePath)
		: base(filePath)
	{
		bmhii = File.Open(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
		nnbcx();
	}

	public duqmg(Stream fileStream, bool leaveOpen)
		: base(fileStream, leaveOpen)
	{
		bmhii = fileStream;
		vtefs = fileStream.Position;
		nnbcx();
	}

	public jnkze rrigt(string p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("path", "Path cannot be null.");
		}
		p0 = jxtqv.ddkiu(p0);
		if (p0.Length == 0 || 1 == 0)
		{
			throw new InvalidOperationException("Cannot create directory because specified path refers to the Root directory.");
		}
		jnkze jnkze2 = rjhpb(p0, p1: false);
		if (jnkze2 != null && 0 == 0)
		{
			throw new InvalidOperationException("Item with the same name already exists.");
		}
		return bktfo(p0);
	}

	private jnkze bktfo(string p0)
	{
		jnkze jnkze2 = jnkze.dmvtn(this, p0);
		p0 = jxtqv.wklvv(p0);
		jnkze jnkze3 = rjhpb(p0, p1: false);
		if (jnkze3 == null || 1 == 0)
		{
			jnkze3 = bktfo(p0);
		}
		jnkze3.jfvgo(jnkze2);
		return jnkze2;
	}

	public void bwhtj(string p0, byte[] p1)
	{
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("buffer", "Buffer cannot be null.");
		}
		beicd(p0, p1, 0, p1.Length);
	}

	public void beicd(string p0, byte[] p1, int p2, int p3)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("path", "Path cannot be null.");
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("buffer", "Buffer cannot be null.");
		}
		if (p2 < 0 || p2 >= p1.Length)
		{
			throw hifyx.nztrs("offset", p2, "Invalid offset.");
		}
		if (p3 < 0 || p2 + p3 > p1.Length)
		{
			throw hifyx.nztrs("count", p3, "Invalid count.");
		}
		p0 = jxtqv.ddkiu(p0);
		if (p0.Length == 0 || 1 == 0)
		{
			throw new InvalidOperationException("Cannot add file because specified path refers to the Root directory.");
		}
		MemoryStream memoryStream = new MemoryStream(p1, p2, p3, writable: false);
		try
		{
			unyyx(p0, memoryStream, p3);
		}
		finally
		{
			if (memoryStream != null && 0 == 0)
			{
				((IDisposable)memoryStream).Dispose();
			}
		}
	}

	public void unyyx(string p0, Stream p1, long p2)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("path", "Path cannot be null.");
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("stream", "Stream cannot be null.");
		}
		if (p2 < 0)
		{
			throw hifyx.nztrs("maxCount", p2, "Invalid count.");
		}
		p0 = jxtqv.ddkiu(p0);
		if (p0.Length == 0 || 1 == 0)
		{
			throw new InvalidOperationException("Cannot add file because specified path refers to the Root directory.");
		}
		jnkze jnkze2 = rjhpb(p0, p1: false);
		if (jnkze2 != null && 0 == 0)
		{
			throw new InvalidOperationException("Item with the same name already exists.");
		}
		jnkze2 = jnkze.icssj(this, p0, p2);
		string p3 = jxtqv.wklvv(p0);
		jnkze jnkze3 = rjhpb(p3, p1: false);
		if (jnkze3 == null || 1 == 0)
		{
			jnkze3 = bktfo(p3);
		}
		jnkze3.jfvgo(jnkze2);
		if (p2 != 0)
		{
			if (qyaqr.Length < xgzqc)
			{
				qyaqr = new byte[xgzqc];
			}
			while (p2 > 0)
			{
				int count = (int)((p2 < qyaqr.Length) ? p2 : qyaqr.Length);
				count = p1.Read(qyaqr, 0, count);
				jnkze2.edlkl.Write(qyaqr, 0, count);
				p2 -= count;
			}
		}
	}

	public void svqqc()
	{
		balkr();
	}

	private void nnbcx()
	{
		lwozm = 9;
		dqrbz = 1 << lwozm;
		rdkme = dqrbz >> 2;
		vbidn = 6;
		yftlt = 1 << vbidn;
		oeinh = 4096;
		bgcmj = new List<int>();
		hftqd = new List<int>();
		ljmpl = new List<int>();
		bgcmj.Add(-3);
		ljmpl.Add(0);
		nosgi = -2;
		opicz = jnkze.ilurr(this);
	}

	private void balkr()
	{
		if (hftqd.Count > 0)
		{
			xnxbw xnxbw2 = new xnxbw(this, isShortStream: false);
			try
			{
				int num = 0;
				if (num != 0)
				{
					goto IL_001e;
				}
				goto IL_0034;
				IL_001e:
				xnxbw2.fptyf(hftqd[num]);
				num++;
				goto IL_0034;
				IL_0034:
				if (num < hftqd.Count)
				{
					goto IL_001e;
				}
				xnxbw2.fmgjd(rdkme - (hftqd.Count & (rdkme - 1)) << 2);
				slbpy = xnxbw2.zqmsy;
			}
			finally
			{
				if (xnxbw2 != null && 0 == 0)
				{
					((IDisposable)xnxbw2).Dispose();
				}
			}
		}
		else
		{
			slbpy = -2;
		}
		xnxbw xnxbw3 = new xnxbw(this, isShortStream: false);
		try
		{
			kxmbr(opicz);
			int num2 = 0;
			if (num2 != 0)
			{
				goto IL_00a6;
			}
			goto IL_00bc;
			IL_00a6:
			kuwso[num2].tdhod(xnxbw3);
			num2++;
			goto IL_00bc;
			IL_00bc:
			if (num2 < kuwso.Count)
			{
				goto IL_00a6;
			}
			while ((xnxbw3.Length & (dqrbz - 1)) != 0)
			{
				jnkze.ffcqb(xnxbw3);
			}
			fmfja = xnxbw3.zqmsy;
		}
		finally
		{
			if (xnxbw3 != null && 0 == 0)
			{
				((IDisposable)xnxbw3).Dispose();
			}
		}
		bmhii.Seek(vtefs, SeekOrigin.Begin);
		bmhii.Flush();
		jfkpr(-2226271756974174256L);
		tkpeb(16);
		kvnsh(62);
		kvnsh(3);
		kvnsh(65534);
		kvnsh((ushort)lwozm);
		kvnsh((ushort)vbidn);
		tkpeb(6);
		tkpeb(4);
		kcjbp(ftaih(bgcmj.Count, rdkme));
		kcjbp(fmfja);
		tkpeb(4);
		kcjbp(oeinh);
		kcjbp(slbpy);
		kcjbp(ftaih(hftqd.Count, rdkme));
		kcjbp(nosgi);
		kcjbp(ftaih(ljmpl.Count - 109, rdkme - 1));
		int num3 = 0;
		if (num3 != 0)
		{
			goto IL_0217;
		}
		goto IL_0257;
		IL_025d:
		if (ljmpl.Count > 109)
		{
			jruyo(109, nosgi);
		}
		edugi();
		return;
		IL_0217:
		if (num3 == ljmpl.Count)
		{
			jvhqw(109 - ljmpl.Count << 2);
			goto IL_025d;
		}
		kcjbp(ljmpl[num3]);
		num3++;
		goto IL_0257;
		IL_0257:
		if (num3 < 109)
		{
			goto IL_0217;
		}
		goto IL_025d;
	}

	private int ftaih(int p0, int p1)
	{
		if (p0 <= 0)
		{
			return 0;
		}
		return (p0 - 1) / p1 + 1;
	}

	private void edugi()
	{
		lngir(-1);
		int num = 0;
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_0011;
		}
		goto IL_00a3;
		IL_0011:
		bmhii.Seek(bjbna(ljmpl[num2]), SeekOrigin.Begin);
		bmhii.Flush();
		int num3 = 0;
		if (num3 != 0)
		{
			goto IL_0040;
		}
		goto IL_0096;
		IL_0040:
		kcjbp(bgcmj[num++]);
		if (num == bgcmj.Count)
		{
			if (num2 != ljmpl.Count - 1)
			{
				throw new uwkib("Invalid SAT definition.");
			}
			jvhqw(rdkme - num3 - 1 << 2);
			return;
		}
		num3++;
		goto IL_0096;
		IL_00a3:
		if (num2 < ljmpl.Count)
		{
			goto IL_0011;
		}
		throw new uwkib("Invalid MSAT definition.");
		IL_0096:
		if (num3 < rdkme)
		{
			goto IL_0040;
		}
		num2++;
		goto IL_00a3;
	}

	private void jruyo(int p0, int p1)
	{
		if (bgcmj[p1] != -4)
		{
			throw new uwkib("Invalid SAT definition.");
		}
		bmhii.Seek(bjbna(p1), SeekOrigin.Begin);
		bmhii.Flush();
		int num = rdkme - 1;
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_004b;
		}
		goto IL_008c;
		IL_008c:
		if (num2 >= num)
		{
			p1 = ljmpl[p0] + 1;
			kcjbp(p1);
			jruyo(p0, p1);
			return;
		}
		goto IL_004b;
		IL_004b:
		kcjbp(ljmpl[p0++]);
		if (p0 == ljmpl.Count)
		{
			jvhqw(num - num2 - 1 << 2);
			kcjbp(-2);
			return;
		}
		num2++;
		goto IL_008c;
	}

	private void kxmbr(jnkze p0)
	{
		int num;
		if (p0.msfii && 0 == 0 && p0.ucvon.Count > 0)
		{
			p0.hcsdt.ykbcs = efxug(p0.ucvon, 0, p0.ucvon.Count - 1);
			num = 0;
			if (num != 0)
			{
				goto IL_0048;
			}
			goto IL_0064;
		}
		return;
		IL_0064:
		if (num >= p0.ucvon.Count)
		{
			return;
		}
		goto IL_0048;
		IL_0048:
		kxmbr(p0.ucvon[num]);
		num++;
		goto IL_0064;
	}

	private ptpnz efxug(List<jnkze> p0, int p1, int p2)
	{
		if (p1 > p2)
		{
			return null;
		}
		if (p1 == p2)
		{
			return p0[p1].hcsdt;
		}
		int num = p1 + p2 >> 1;
		p0[num].hcsdt.txwje = efxug(p0, p1, num - 1);
		p0[num].hcsdt.vaabt = efxug(p0, num + 1, p2);
		return p0[num].hcsdt;
	}

	private void jtvsy(short p0)
	{
		qyaqr[0] = (byte)p0;
		qyaqr[1] = (byte)(p0 >> 8);
		bmhii.Write(qyaqr, 0, 2);
	}

	private void kcjbp(int p0)
	{
		qyaqr[0] = (byte)p0;
		qyaqr[1] = (byte)(p0 >> 8);
		qyaqr[2] = (byte)(p0 >> 16);
		qyaqr[3] = (byte)(p0 >> 24);
		bmhii.Write(qyaqr, 0, 4);
	}

	private void jfkpr(long p0)
	{
		qyaqr[0] = (byte)p0;
		qyaqr[1] = (byte)(p0 >> 8);
		qyaqr[2] = (byte)(p0 >> 16);
		qyaqr[3] = (byte)(p0 >> 24);
		qyaqr[4] = (byte)(p0 >> 32);
		qyaqr[5] = (byte)(p0 >> 40);
		qyaqr[6] = (byte)(p0 >> 48);
		qyaqr[7] = (byte)(p0 >> 56);
		bmhii.Write(qyaqr, 0, 8);
	}

	private void kvnsh(ushort p0)
	{
		qyaqr[0] = (byte)p0;
		qyaqr[1] = (byte)(p0 >> 8);
		bmhii.Write(qyaqr, 0, 2);
	}

	private void rjiug(uint p0)
	{
		qyaqr[0] = (byte)p0;
		qyaqr[1] = (byte)(p0 >> 8);
		qyaqr[2] = (byte)(p0 >> 16);
		qyaqr[3] = (byte)(p0 >> 24);
		bmhii.Write(qyaqr, 0, 4);
	}

	private void wzjcv(ulong p0)
	{
		qyaqr[0] = (byte)p0;
		qyaqr[1] = (byte)(p0 >> 8);
		qyaqr[2] = (byte)(p0 >> 16);
		qyaqr[3] = (byte)(p0 >> 24);
		qyaqr[4] = (byte)(p0 >> 32);
		qyaqr[5] = (byte)(p0 >> 40);
		qyaqr[6] = (byte)(p0 >> 48);
		qyaqr[7] = (byte)(p0 >> 56);
		bmhii.Write(qyaqr, 0, 8);
	}

	private void tkpeb(int p0)
	{
		if (qyaqr.Length < p0)
		{
			qyaqr = new byte[p0];
		}
		else
		{
			Array.Clear(qyaqr, 0, p0);
		}
		bmhii.Write(qyaqr, 0, p0);
	}

	private void jvhqw(int p0)
	{
		while (p0 > 0)
		{
			int num = Math.Min(p0, rnsvi.khisd.Length);
			bmhii.Write(rnsvi.khisd, 0, num);
			p0 -= num;
		}
	}
}
