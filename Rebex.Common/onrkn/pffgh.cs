using System;

namespace onrkn;

internal class pffgh : eiuaf, mhrzn
{
	private enum iyclk
	{
		iwtsc,
		zndpv,
		aqrlm
	}

	private readonly hhfei steai;

	private bool rspbp;

	private bool klzqv;

	private bool gvlwo;

	private byte[] binhy;

	private int lzclc;

	private int fvpan;

	private yosfy pdgfl;

	private uint lzpzu;

	private byte[] tjidg = new byte[4];

	private int ksvta;

	private iyclk snxbu;

	private bool qlxys;

	protected bool upsxr
	{
		get
		{
			return rspbp;
		}
		set
		{
			if (!lhoiv || 1 == 0)
			{
				throw new InvalidOperationException("Cannot change this property in current state.");
			}
			rspbp = value;
		}
	}

	protected bool cpeok
	{
		get
		{
			return klzqv;
		}
		set
		{
			if (!lhoiv || 1 == 0)
			{
				throw new InvalidOperationException("Cannot change this property in current state.");
			}
			klzqv = value;
		}
	}

	protected bool lhoiv => gvlwo;

	public bool wngjx => steai.wngjx;

	public yosfy lotbz => pdgfl;

	public pffgh()
		: this(useEnhancedDeflate: false, checkHeader: true, checkFooter: true)
	{
	}

	public pffgh(bool useEnhancedDeflate, bool checkHeader, bool checkFooter)
	{
		qlxys = useEnhancedDeflate;
		steai = new hhfei(useEnhancedDeflate);
		rspbp = checkHeader;
		klzqv = checkFooter;
		ucubh();
	}

	private void ucubh()
	{
		gvlwo = true;
		binhy = null;
		lzpzu = 1u;
		pdgfl = yosfy.drxjq;
	}

	protected void hyxlx(byte[] p0, int p1, int p2, dzmpf p3)
	{
		if (pdgfl != yosfy.drxjq)
		{
			throw new InvalidOperationException("SetInput method can be called only when State is CompressorState.NewInput.");
		}
		if (p3 != dzmpf.iksen && 0 == 0)
		{
			throw new ArgumentException("No flags are required in Decompressor.", "flag");
		}
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("input", "Input buffer cannot be null.");
		}
		if (p1 < 0)
		{
			throw hifyx.nztrs("offset", p1, "Offset is negative.");
		}
		if (p2 < 0)
		{
			throw hifyx.nztrs("count", p2, "Count is negative.");
		}
		if (p1 + p2 <= p0.Length)
		{
			return;
		}
		throw new ArgumentException("The sum of offset and count is larger than the buffer length.");
	}

	public void eanoq(byte[] p0, int p1, int p2, dzmpf p3)
	{
		hyxlx(p0, p1, p2, p3);
		ywatp(p0, p1, p2, p3);
	}

	protected void ywatp(byte[] p0, int p1, int p2, dzmpf p3)
	{
		if (gvlwo && 0 == 0)
		{
			if (rspbp && 0 == 0)
			{
				snxbu = iyclk.iwtsc;
				ksvta = 2;
			}
			else
			{
				snxbu = iyclk.zndpv;
				ksvta = 0;
			}
		}
		gvlwo = false;
		while (ksvta > 0 && p2 > 0)
		{
			tjidg[--ksvta] = p0[p1++];
			p2--;
		}
		switch (snxbu)
		{
		case iyclk.iwtsc:
			if (ksvta == 0 || 1 == 0)
			{
				byte b = tjidg[1];
				byte b2 = tjidg[0];
				if (!dhtel(b, b2) || 1 == 0)
				{
					throw new sjylk("Invalid ZLIB header check.");
				}
				if (!qdjqa(b) || 1 == 0)
				{
					throw new sjylk("Invalid compression method in ZLIB header.");
				}
				int num = b >> 4;
				if (num > ((qlxys ? true : false) ? 8 : 7))
				{
					throw new sjylk("Invalid compression info in ZLIB header.");
				}
				if (((b2 >> 5) & 1) != 0 && 0 == 0)
				{
					throw new sjylk("Compression with preset dictionary is not supported.");
				}
				snxbu = iyclk.zndpv;
				goto case iyclk.zndpv;
			}
			break;
		case iyclk.zndpv:
			if (p2 > 0)
			{
				binhy = p0;
				lzclc = p1;
				fvpan = p2;
				steai.eanoq(p0, p1, p2, p3);
				pdgfl = steai.lotbz;
			}
			break;
		case iyclk.aqrlm:
			if (ksvta == 0 || 1 == 0)
			{
				if (((tjidg[3] << 24) | (tjidg[2] << 16) | (tjidg[1] << 8) | tjidg[0]) != (int)lzpzu)
				{
					throw new sjylk("Adler-32 check failed.");
				}
				pdgfl = yosfy.aljno;
				fvpan = p2;
			}
			break;
		}
	}

	public int zohfz(byte[] p0, int p1, int p2)
	{
		if (pdgfl != yosfy.muyhp)
		{
			throw new InvalidOperationException("Process method can be called only when State is CompressorState.MoreOutput.");
		}
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("output", "Output buffer cannot be null.");
		}
		if (p1 < 0)
		{
			throw hifyx.nztrs("offset", p1, "Offset is negative.");
		}
		if (p2 < 0)
		{
			throw hifyx.nztrs("count", p2, "Count is negative.");
		}
		if (p1 + p2 > p0.Length)
		{
			throw new ArgumentException("The sum of offset and count is larger than the buffer length.");
		}
		int num = steai.zohfz(p0, p1, p2);
		pdgfl = steai.lotbz;
		if (klzqv && 0 == 0)
		{
			lzpzu = utjdx.ifqlq(lzpzu, p0, p1, num);
		}
		if (pdgfl == yosfy.aljno)
		{
			int num2 = steai.pmhat();
			if (klzqv && 0 == 0)
			{
				snxbu = iyclk.aqrlm;
				ksvta = 4;
				pdgfl = yosfy.drxjq;
				eanoq(binhy, lzclc + fvpan - num2, num2, dzmpf.iksen);
			}
			else
			{
				lzclc = lzclc + fvpan - num2;
				fvpan = num2;
			}
		}
		return num;
	}

	public int pmhat()
	{
		if (pdgfl != yosfy.aljno)
		{
			throw new InvalidOperationException("Finish method can be called only when State is CompressorState.Finish.");
		}
		ucubh();
		return fvpan;
	}

	internal static bool dhtel(byte p0, byte p1)
	{
		return ((p0 << 8) | p1) % 31 == 0;
	}

	internal static bool qdjqa(byte p0)
	{
		return (p0 & 0xF) == 8;
	}
}
