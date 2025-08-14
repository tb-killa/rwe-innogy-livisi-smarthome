using System;

namespace onrkn;

internal class kypao : mhrzn
{
	private enum ywftq
	{
		mvpsh,
		yckjx,
		mbtjr
	}

	private readonly ifjbk dhqpi;

	private readonly int jwwhk;

	private readonly bool nappn;

	private readonly bool sztpz;

	private yosfy xospy = yosfy.drxjq;

	private uint xdupt = 1u;

	private int crrai;

	private uint kkepr;

	private int ruafs;

	private ywftq qbfxs;

	private bool bfwev;

	private bool wwnuz;

	public yosfy lotbz => xospy;

	public kypao()
		: this(useEnhancedDeflate: false, 6, 49152, writeHeader: true, writeFooter: true)
	{
	}

	public kypao(int compressionLevel)
		: this(useEnhancedDeflate: false, compressionLevel, 49152, writeHeader: true, writeFooter: true)
	{
	}

	public kypao(bool useEnhancedDeflate, int compressionLevel, int blockSize, bool writeHeader, bool writeFooter)
	{
		jwwhk = compressionLevel;
		dhqpi = new ifjbk(useEnhancedDeflate, compressionLevel, blockSize, 9);
		nappn = writeHeader;
		sztpz = writeFooter;
		qbfxs = ((!writeHeader || 1 == 0) ? ywftq.yckjx : ywftq.mvpsh);
		bfwev = useEnhancedDeflate;
	}

	public void eanoq(byte[] p0, int p1, int p2, dzmpf p3)
	{
		if (xospy != yosfy.drxjq)
		{
			throw new InvalidOperationException("SetInput method can be called only when State is CompressorState.NewInput.");
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
		if (p1 + p2 > p0.Length)
		{
			throw new ArgumentException("The sum of offset and count is larger than the buffer length.");
		}
		if (sztpz && 0 == 0)
		{
			xdupt = utjdx.ifqlq(xdupt, p0, p1, p2);
		}
		dhqpi.eanoq(p0, p1, p2, p3);
		xospy = dhqpi.lotbz;
	}

	public int zohfz(byte[] p0, int p1, int p2)
	{
		if (xospy != yosfy.muyhp)
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
		crrai = 0;
		if (ruafs > 0)
		{
			int p3 = ruafs;
			ruafs = 0;
			onjof(kkepr, p3, p0, ref p1, ref p2);
		}
		while (p2 > 0)
		{
			int num3;
			int num4;
			int num6;
			int num7;
			int num8;
			int num9;
			switch (qbfxs)
			{
			case ywftq.mvpsh:
			{
				int num = ((bfwev ? true : false) ? 16 : 15) - 8;
				int num2 = 8;
				num3 = (num << 4) | num2;
				if (jwwhk < 2)
				{
					num4 = 0;
					if (num4 == 0)
					{
						goto IL_0127;
					}
				}
				if (jwwhk == 6)
				{
					num4 = 2;
					if (num4 != 0)
					{
						goto IL_0127;
					}
				}
				if (jwwhk < 9)
				{
					num4 = 1;
					if (num4 != 0)
					{
						goto IL_0127;
					}
				}
				num4 = 3;
				goto IL_0127;
			}
			case ywftq.yckjx:
			{
				int num5 = dhqpi.zohfz(p0, p1, p2);
				crrai += num5;
				if (dhqpi.lotbz == yosfy.aljno && sztpz)
				{
					p1 += num5;
					p2 -= num5;
					qbfxs = ywftq.mbtjr;
					break;
				}
				xospy = dhqpi.lotbz;
				return crrai;
			}
			case ywftq.mbtjr:
				{
					if (wwnuz && 0 == 0)
					{
						wwnuz = false;
						xospy = yosfy.aljno;
						return crrai;
					}
					if (onjof(xdupt, 4, p0, ref p1, ref p2) && 0 == 0)
					{
						xospy = yosfy.aljno;
						return crrai;
					}
					wwnuz = true;
					break;
				}
				IL_0127:
				num6 = 0;
				num7 = (num4 << 6) | (num6 << 5);
				num8 = (num3 << 8) | num7;
				num9 = num8 % 31;
				if (num9 > 0)
				{
					num8 |= 31 - num9;
				}
				qbfxs = ywftq.yckjx;
				onjof((uint)num8, 2, p0, ref p1, ref p2);
				break;
			}
		}
		xospy = yosfy.muyhp;
		return crrai;
	}

	private bool onjof(uint p0, int p1, byte[] p2, ref int p3, ref int p4)
	{
		int num = p1 - 1 << 3;
		while (p1 > 0 && p4 > 0)
		{
			p2[p3++] = (byte)(p0 >> num);
			p1--;
			p4--;
			crrai++;
			num -= 8;
		}
		if (p1 > 0)
		{
			kkepr = p0;
			ruafs = p1;
			xospy = yosfy.muyhp;
			return false;
		}
		return true;
	}

	public int pmhat()
	{
		if (xospy != yosfy.aljno)
		{
			throw new InvalidOperationException("Finish method can be called only when State is CompressorState.Finish.");
		}
		xdupt = 1u;
		qbfxs = ((!nappn || 1 == 0) ? ywftq.yckjx : ywftq.mvpsh);
		xospy = yosfy.drxjq;
		return dhqpi.pmhat();
	}
}
