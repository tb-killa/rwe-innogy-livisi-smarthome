using System;
using Rebex;

namespace onrkn;

internal class ifjbk : mhrzn
{
	private enum yacvk
	{
		nyxzx,
		sdqpb,
		edzxb,
		gfrla
	}

	public const int rkxvu = 6;

	public const int walwg = 9;

	public const int tuulo = 49152;

	private const int lowtp = 15;

	private const int ycbom = 64;

	private const int dqwes = 512;

	private const int dwhcl = 256;

	private const int bnfqy = 256;

	private const int dlmit = 2;

	private const int ivbpi = 10;

	private byte[] cevmp;

	private int xhcck;

	private int plpjb;

	private byte[] gecet;

	private int qigqi;

	private int ynpii;

	private byte[] ovhph;

	private int hsyck;

	private int dxsgd;

	private int laqol;

	private int bsglg;

	private int blzxm;

	private int chsxb;

	private int[] flnkt;

	private int[] sugfj;

	private int[] zhvhe;

	private int pwlke;

	private int pirxc;

	private byte[] ysmmc;

	private int vacna;

	private int tclcs;

	private yosfy locaq = yosfy.drxjq;

	private int uprcy;

	private int xhywb;

	private int ehhgd;

	private bool kogvg;

	private bool nyxmu;

	private bool ijyvd;

	private bool zcoyn;

	private hvgim umzxq;

	private yudos jaugj;

	private int[] qflzr;

	private int[] zsxna;

	private int[] uyuux;

	private int[] aasvd;

	private ijtci aphsj;

	private int pvypc;

	private bool lkfrf;

	private bool amvgb;

	private int tvpht = -1;

	private readonly sjhqe zqjgd;

	private int rrhlo;

	private int qehur;

	private int qlbow;

	private int qqcjj;

	private yacvk bxjcd;

	public int ppzjg
	{
		get
		{
			return pvypc;
		}
		set
		{
			if (!amvgb || 1 == 0)
			{
				throw new InvalidOperationException("Compression level can be set only in initial state.");
			}
			if (value < 0 || value > 9)
			{
				throw hifyx.nztrs("value", value, "Invalid compression level. Possible values are 0-9.");
			}
			pvypc = value;
			if (aphsj != null && 0 == 0)
			{
				aphsj.gjqaw = value;
			}
		}
	}

	public bool mseup
	{
		get
		{
			return lkfrf;
		}
		set
		{
			if (!amvgb || 1 == 0)
			{
				throw new InvalidOperationException("Deflate method can be set only in initial state.");
			}
			lkfrf = value;
			if (aphsj != null && 0 == 0)
			{
				aphsj.obzio = value;
			}
		}
	}

	public yosfy lotbz => locaq;

	public ifjbk()
		: this(useEnhancedDeflate: false, 6, 49152, 9)
	{
	}

	public ifjbk(int compressionLevel)
		: this(useEnhancedDeflate: false, compressionLevel, 49152, 9)
	{
	}

	public ifjbk(bool useEnhancedDeflate, int compressionLevel, int blockSize, int maxCompressionLevel)
		: this(useEnhancedDeflate, compressionLevel, blockSize, maxCompressionLevel, null)
	{
	}

	public ifjbk(bool useEnhancedDeflate, int compressionLevel, int blockSize, int maxCompressionLevel, sjhqe logger)
	{
		if (blockSize < 4096 || blockSize > 65535)
		{
			throw hifyx.nztrs("blockSize", blockSize, "Invalid block size. Possible values are 0x1000-0xFFFF.");
		}
		if (compressionLevel < 0 || compressionLevel > maxCompressionLevel)
		{
			throw hifyx.nztrs("compressionLevel", compressionLevel, "Invalid compression level. Possible values are 0-" + 9 + ".");
		}
		pvypc = compressionLevel;
		uprcy = blockSize;
		lkfrf = useEnhancedDeflate;
		sjhqe obj = logger;
		if (obj == null || 1 == 0)
		{
			obj = awngk.xffts;
		}
		zqjgd = obj;
		amvgb = true;
	}

	public void eanoq(byte[] p0, int p1, int p2, dzmpf p3)
	{
		if (locaq != yosfy.drxjq)
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
		if (cevmp == null || 1 == 0)
		{
			cevmp = new byte[uprcy];
			aphsj = new ijtci(lkfrf, 15, (lkfrf ? true : false) ? 16 : 15, pvypc);
		}
		ehhgd = plpjb + p2;
		nyxmu = (p3 & dzmpf.vgiar) == dzmpf.vgiar;
		kogvg = (p3 & dzmpf.dqswj) == dzmpf.dqswj;
		ijyvd = (p3 & dzmpf.aitwo) == dzmpf.aitwo;
		if ((!kogvg || 1 == 0) && ehhgd < uprcy)
		{
			Array.Copy(p0, p1, cevmp, plpjb, p2);
			plpjb += p2;
		}
		else
		{
			gecet = p0;
			qigqi = p1;
			ynpii = p2;
			umzxq = hvgim.mkxdi;
			locaq = yosfy.muyhp;
		}
		amvgb = false;
	}

	public int zohfz(byte[] p0, int p1, int p2)
	{
		if (locaq != yosfy.muyhp)
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
		ovhph = p0;
		hsyck = p1;
		dxsgd = p2;
		laqol = 0;
		locaq = (yosfy)0;
		if (chsxb > 0 && dxsgd > 0)
		{
			int p3 = chsxb;
			chsxb = 0;
			cblvq(bsglg, p3);
		}
		while (dxsgd > 0)
		{
			switch (umzxq)
			{
			case hvgim.mkxdi:
				tvpht++;
				zqjgd.byfnx(LogLevel.Verbose, "DEFLATE", "Analyzing block {0}.", tvpht);
				ioegb();
				umzxq = hvgim.qhlwt;
				break;
			case hvgim.qhlwt:
				cblvq((zcoyn ? true : false) ? 1 : 0, 1);
				zqjgd.byfnx(LogLevel.Verbose, "DEFLATE", "Processing block {0} (final={1}).", tvpht, zcoyn);
				umzxq = hvgim.emuzf;
				break;
			case hvgim.emuzf:
				cblvq((int)jaugj, 2);
				switch (jaugj)
				{
				case yudos.jtxds:
					zqjgd.byfnx(LogLevel.Verbose, "DEFLATE", "Using no compression in block {0}.", tvpht);
					umzxq = hvgim.zpaqw;
					break;
				case yudos.pmogm:
					zqjgd.byfnx(LogLevel.Verbose, "DEFLATE", "Using fixed Huffman tree in block {0}.", tvpht);
					umzxq = hvgim.gucov;
					break;
				case yudos.zfqbj:
					zqjgd.byfnx(LogLevel.Verbose, "DEFLATE", "Using dynamic Huffman tree in block {0}.", tvpht);
					umzxq = hvgim.reiyv;
					break;
				default:
					throw new InvalidOperationException("Invalid compression method.");
				}
				break;
			case hvgim.zpaqw:
				if (blzxm != 0 && 0 == 0)
				{
					blzxm = 0;
					hsyck++;
					dxsgd--;
					laqol++;
				}
				if (dxsgd > 0)
				{
					cblvq(rrhlo, 16);
					umzxq = hvgim.zibui;
				}
				break;
			case hvgim.zibui:
				cblvq(qehur, 16);
				umzxq = hvgim.gucov;
				break;
			case hvgim.reiyv:
				ummnb();
				break;
			case hvgim.gucov:
				switch (jaugj)
				{
				case yudos.jtxds:
					xiknn();
					break;
				case yudos.pmogm:
				case yudos.zfqbj:
					myhpx();
					break;
				default:
					throw new InvalidOperationException("Invalid compression method.");
				}
				if (locaq != 0 && 0 == 0)
				{
					return laqol;
				}
				break;
			default:
				throw new InvalidOperationException("Invalid next state.");
			}
		}
		locaq = yosfy.muyhp;
		return laqol;
	}

	public int pmhat()
	{
		if (locaq != yosfy.aljno)
		{
			throw new InvalidOperationException("Finish method can be called only when State is CompressorState.Finish.");
		}
		zryyz();
		return 0;
	}

	public void zryyz()
	{
		gecet = (ovhph = (cevmp = null));
		aphsj = null;
		flnkt = (sugfj = (zhvhe = null));
		ysmmc = null;
		qflzr = (zsxna = (uyuux = (aasvd = null)));
		locaq = yosfy.drxjq;
		tvpht = -1;
		amvgb = true;
	}

	private void cblvq(int p0, int p1)
	{
		if (blzxm == 0 || 1 == 0)
		{
			ovhph[hsyck] = (byte)p0;
		}
		else
		{
			ovhph[hsyck] |= (byte)(p0 << blzxm);
		}
		blzxm += p1;
		while (blzxm > 8)
		{
			blzxm -= 8;
			hsyck++;
			dxsgd--;
			laqol++;
			if (dxsgd == 0 || 1 == 0)
			{
				bsglg = p0 >> p1 - blzxm;
				chsxb = blzxm;
				blzxm = 0;
				return;
			}
			ovhph[hsyck] = (byte)(p0 >> p1 - blzxm);
		}
		if (blzxm == 8)
		{
			blzxm = 0;
			hsyck++;
			dxsgd--;
			laqol++;
		}
	}

	private void ioegb()
	{
		zcoyn = false;
		xhywb = Math.Min(ehhgd, uprcy);
		int num = xhywb - plpjb;
		if (pvypc != 0 && 0 == 0 && (!nyxmu || false || ehhgd != 0))
		{
			int p = 0;
			int num2 = 0;
			int num3 = 0;
			pwlke = (pirxc = 0);
			jaugj = yudos.pmogm;
			if (xhywb > 0)
			{
				if (flnkt == null || 1 == 0)
				{
					flnkt = new int[uprcy * 4 / 3 + 4];
					sugfj = new int[286];
					zhvhe = new int[32];
				}
				else
				{
					Array.Clear(sugfj, 0, sugfj.Length);
					Array.Clear(zhvhe, 0, zhvhe.Length);
				}
				if (plpjb > 0)
				{
					pirxc = aphsj.onfjx(cevmp, xhcck, plpjb, flnkt, pirxc, sugfj, zhvhe, ref p);
				}
				if (num > 0)
				{
					pirxc += aphsj.onfjx(gecet, qigqi, num, flnkt, pirxc, sugfj, zhvhe, ref p);
				}
				if (xhywb > 64)
				{
					if (qflzr == null || 1 == 0)
					{
						qflzr = new int[286];
						zsxna = new int[286];
						uyuux = new int[32];
						aasvd = new int[32];
					}
					else
					{
						Array.Clear(qflzr, 0, qflzr.Length);
						Array.Clear(zsxna, 0, zsxna.Length);
						Array.Clear(uyuux, 0, uyuux.Length);
						Array.Clear(aasvd, 0, aasvd.Length);
					}
					sugfj[256] = 1;
					int p2 = xgjbs.fvvzz(sugfj, qflzr, zsxna, 15);
					int p3 = xgjbs.fvvzz(zhvhe, uyuux, aasvd, 15);
					num3 = xhywb + 5 << 3;
					num2 = p + jdspf(sugfj, xgjbs.mkfjy) + jdspf(zhvhe, xgjbs.jtsmd);
					int num4 = p + jdspf(sugfj, qflzr) + jdspf(zhvhe, uyuux);
					if (num4 < num2 - 512)
					{
						if (ysmmc == null || 1 == 0)
						{
							ysmmc = new byte[256];
						}
						else
						{
							Array.Clear(ysmmc, 0, ysmmc.Length);
						}
						xgjbs.urils(ysmmc, out vacna, out tclcs, qflzr, p2, uyuux, p3);
						num4 += (vacna << 3) + tclcs;
						if (num4 < num2 - 256)
						{
							jaugj = yudos.zfqbj;
							num2 = num4;
						}
					}
				}
			}
			if (xhywb <= 64 || num2 < num3 - 256)
			{
				if (plpjb > xhywb)
				{
					plpjb -= xhywb;
					xhcck += xhywb;
				}
				else
				{
					qigqi += num;
					ynpii -= num;
					xhcck = (plpjb = 0);
				}
				ehhgd -= xhywb;
				if ((ehhgd == 0 || 1 == 0) && ijyvd && 0 == 0)
				{
					zcoyn = true;
				}
				return;
			}
		}
		jaugj = yudos.jtxds;
		xhywb = Math.Min(xhywb, 65535);
		rrhlo = xhywb;
		qehur = ~rrhlo;
		if (ehhgd == xhywb && ijyvd && 0 == 0)
		{
			zcoyn = true;
		}
	}

	private static int jdspf(int[] p0, int[] p1)
	{
		int num = 0;
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_0008;
		}
		goto IL_0016;
		IL_0008:
		num += p0[num2] * p1[num2];
		num2++;
		goto IL_0016;
		IL_0016:
		if (num2 < p0.Length)
		{
			goto IL_0008;
		}
		return num;
	}

	private void ummnb()
	{
		while (dxsgd > 0)
		{
			if (qlbow < vacna)
			{
				cblvq(ysmmc[qlbow], 8);
			}
			else
			{
				if (qlbow != vacna || tclcs <= 0)
				{
					qlbow = 0;
					umzxq = hvgim.gucov;
					break;
				}
				cblvq(ysmmc[qlbow], tclcs);
			}
			qlbow++;
		}
	}

	private void xiknn()
	{
		int num = Math.Min(rrhlo, dxsgd);
		int num2 = 0;
		if (plpjb > 0)
		{
			num2 = ztqjr(cevmp, ref xhcck, ref plpjb, num);
			num -= num2;
		}
		if (num > 0)
		{
			num2 += ztqjr(gecet, ref qigqi, ref ynpii, num);
		}
		rrhlo -= num2;
		ehhgd -= num2;
		laqol += num2;
		if (rrhlo == 0 || 1 == 0)
		{
			bxcgx();
		}
		else
		{
			locaq = yosfy.muyhp;
		}
	}

	private int ztqjr(byte[] p0, ref int p1, ref int p2, int p3)
	{
		if (p2 <= p3)
		{
			p3 = p2;
			Array.Copy(p0, p1, ovhph, hsyck, p3);
			p1 = 0;
			p2 = 0;
		}
		else
		{
			Array.Copy(p0, p1, ovhph, hsyck, p3);
			p1 += p3;
			p2 -= p3;
		}
		hsyck += p3;
		dxsgd -= p3;
		return p3;
	}

	private void myhpx()
	{
		while (dxsgd > 0)
		{
			switch (bxjcd)
			{
			case yacvk.nyxzx:
				if (pwlke < pirxc)
				{
					qqcjj = flnkt[pwlke];
					if (jaugj == yudos.pmogm)
					{
						cblvq(xgjbs.vqcba[qqcjj], xgjbs.mkfjy[qqcjj]);
					}
					else
					{
						cblvq(zsxna[qqcjj], qflzr[qqcjj]);
					}
					if (qqcjj > 256)
					{
						bxjcd = yacvk.sdqpb;
					}
				}
				else
				{
					if (pwlke != pirxc)
					{
						bxcgx();
						return;
					}
					if (jaugj == yudos.pmogm)
					{
						cblvq(xgjbs.vqcba[256], xgjbs.mkfjy[256]);
					}
					else
					{
						cblvq(zsxna[256], qflzr[256]);
					}
				}
				break;
			case yacvk.sdqpb:
			{
				int num2 = ((lkfrf && 0 == 0 && qqcjj == 285) ? 16 : ijtci.vxjfo[qqcjj - 257]);
				if (num2 > 0)
				{
					cblvq(flnkt[pwlke], num2);
				}
				bxjcd = yacvk.edzxb;
				break;
			}
			case yacvk.edzxb:
				qqcjj = flnkt[pwlke];
				if (jaugj == yudos.pmogm)
				{
					cblvq(xgjbs.ydmaw[qqcjj], xgjbs.jtsmd[qqcjj]);
				}
				else
				{
					cblvq(aasvd[qqcjj], uyuux[qqcjj]);
				}
				bxjcd = yacvk.gfrla;
				break;
			case yacvk.gfrla:
			{
				int num = ijtci.yoljm[qqcjj];
				if (num > 0)
				{
					cblvq(flnkt[pwlke], num);
				}
				bxjcd = yacvk.nyxzx;
				break;
			}
			}
			pwlke++;
		}
		locaq = yosfy.muyhp;
	}

	private void bxcgx()
	{
		if (ehhgd == 0 || 1 == 0)
		{
			if (ijyvd && 0 == 0)
			{
				locaq = yosfy.aljno;
				if (blzxm > 0)
				{
					blzxm = 0;
					hsyck++;
					dxsgd--;
					laqol++;
				}
				return;
			}
			if (nyxmu && 0 == 0)
			{
				if (blzxm == 0 || false || (10 + blzxm) % 8 == 0 || 1 == 0)
				{
					if (blzxm > 0)
					{
						cblvq(2, 10);
					}
					if (chsxb > 0)
					{
						locaq = yosfy.muyhp;
					}
					else
					{
						locaq = yosfy.drxjq;
					}
				}
				else
				{
					umzxq = hvgim.mkxdi;
				}
				return;
			}
			locaq = yosfy.drxjq;
			if (blzxm > 0)
			{
				if (kogvg && 0 == 0)
				{
					int num = 8 - blzxm;
					cblvq(2, num);
					bsglg = 2 >> num;
					chsxb = 10 - num;
				}
				else
				{
					bsglg = ovhph[hsyck];
					chsxb = blzxm;
					blzxm = 0;
				}
			}
		}
		else if ((kogvg ? true : false) || ehhgd >= uprcy)
		{
			umzxq = hvgim.mkxdi;
		}
		else
		{
			locaq = yosfy.drxjq;
			if (blzxm > 0)
			{
				bsglg = ovhph[hsyck];
				chsxb = blzxm;
				blzxm = 0;
			}
			if (plpjb > 0)
			{
				Array.Copy(cevmp, xhcck, cevmp, 0, plpjb);
				xhcck = 0;
				Array.Copy(gecet, qigqi, cevmp, plpjb, ynpii);
			}
			else
			{
				Array.Copy(gecet, qigqi, cevmp, 0, ynpii);
			}
			ynpii = 0;
			plpjb = ehhgd;
		}
	}
}
