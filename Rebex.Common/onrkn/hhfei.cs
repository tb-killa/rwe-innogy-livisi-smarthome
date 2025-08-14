using System;
using Rebex;

namespace onrkn;

internal class hhfei : eiuaf, mhrzn
{
	private enum tujvb
	{
		qanhr = 1,
		hdbag = 2,
		pkjaa = 3,
		zmzqx = 4,
		qpbbz = 5,
		xkxou = 6,
		pufbu = 9,
		rcpmt = 10,
		jtxmk = 11,
		pdkap = 12,
		xngsf = 13
	}

	private static readonly int[] ctzgz = new int[17]
	{
		0, 1, 3, 7, 15, 31, 63, 127, 255, 511,
		1023, 2047, 4095, 8191, 16383, 32767, 65535
	};

	private static readonly int[] isaba = new int[28]
	{
		3, 4, 5, 6, 7, 8, 9, 10, 11, 13,
		15, 17, 19, 23, 27, 31, 35, 43, 51, 59,
		67, 83, 99, 115, 131, 163, 195, 227
	};

	private static readonly int[] uclcy = new int[28]
	{
		0, 0, 0, 0, 0, 0, 0, 0, 1, 1,
		1, 1, 2, 2, 2, 2, 3, 3, 3, 3,
		4, 4, 4, 4, 5, 5, 5, 5
	};

	private static readonly int[] aogmx = new int[32]
	{
		1, 2, 3, 4, 5, 7, 9, 13, 17, 25,
		33, 49, 65, 97, 129, 193, 257, 385, 513, 769,
		1025, 1537, 2049, 3073, 4097, 6145, 8193, 12289, 16385, 24577,
		32769, 49153
	};

	private static readonly int[] exktx = new int[32]
	{
		0, 0, 0, 0, 1, 1, 2, 2, 3, 3,
		4, 4, 5, 5, 6, 6, 7, 7, 8, 8,
		9, 9, 10, 10, 11, 11, 12, 12, 13, 13,
		14, 14
	};

	private byte[] hvbzo;

	private int vwmqu;

	private int muiyq;

	private int oorcw;

	private int opclj;

	private bool veuvg;

	private yosfy zjonk = yosfy.drxjq;

	private hvgim dhiun;

	private yudos mflsx;

	private int kexqb = -1;

	private int sxgkv;

	private xgjbs rsdaq;

	private xgjbs rwvpq;

	private tujvb pbson;

	private nzvsl vlxoz;

	private bool ekhgv;

	private bool khkit;

	private readonly sjhqe socti;

	private int brsak;

	private int flzds;

	private int jfcyo;

	private int awmta;

	private int jmstw;

	private int[] xjhdj;

	private xgjbs bzedx;

	private int vsuib;

	private int lcxow;

	private int wgrcx;

	public bool lybmt
	{
		get
		{
			return ekhgv;
		}
		set
		{
			if (!khkit || 1 == 0)
			{
				throw new InvalidOperationException("Type of DEFLATE algorithm can be set only in the initial state.");
			}
			ekhgv = value;
		}
	}

	public bool wngjx => dhiun == hvgim.qhlwt;

	public yosfy lotbz => zjonk;

	public hhfei()
		: this(useEnhancedDeflate: false, null)
	{
	}

	public hhfei(bool useEnhancedDeflate)
		: this(useEnhancedDeflate, null)
	{
	}

	public hhfei(bool useEnhancedDeflate, sjhqe logger)
	{
		khkit = true;
		ekhgv = useEnhancedDeflate;
		sjhqe obj = logger;
		if (obj == null || 1 == 0)
		{
			obj = awngk.xffts;
		}
		socti = obj;
	}

	public void eanoq(byte[] p0, int p1, int p2, dzmpf p3)
	{
		if (zjonk != yosfy.drxjq)
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
		if (p1 + p2 > p0.Length)
		{
			throw new ArgumentException("The sum of offset and count is larger than the buffer length.");
		}
		if (vlxoz == null || 1 == 0)
		{
			vlxoz = new nzvsl((ekhgv ? true : false) ? 16 : 15, socti);
		}
		hvbzo = p0;
		vwmqu = p1;
		muiyq = p2;
		zjonk = yosfy.muyhp;
		khkit = false;
	}

	public int zohfz(byte[] p0, int p1, int p2)
	{
		if (zjonk != yosfy.muyhp)
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
		if (p2 == 0 || 1 == 0)
		{
			return 0;
		}
		int num = p2;
		zjonk = (yosfy)0;
		while (zjonk == (yosfy)0)
		{
			while (muiyq > 0 && opclj < 16)
			{
				oorcw |= hvbzo[vwmqu] << opclj;
				vwmqu++;
				muiyq--;
				opclj += 8;
			}
			switch (dhiun)
			{
			case hvgim.qhlwt:
			{
				int num2 = fxevu(1);
				if (num2 >= 0)
				{
					veuvg = num2 == 1;
					kexqb++;
					dhiun = hvgim.emuzf;
					socti.byfnx(LogLevel.Verbose, "DEFLATE", "Processing block {0} (final={1}).", kexqb, veuvg);
				}
				break;
			}
			case hvgim.emuzf:
				switch (fxevu(2))
				{
				case 0:
				{
					mflsx = yudos.jtxds;
					dhiun = hvgim.zpaqw;
					int num3 = opclj % 8;
					oorcw >>= num3;
					opclj -= num3;
					socti.byfnx(LogLevel.Verbose, "DEFLATE", "Using no compression in block {0}.", kexqb);
					break;
				}
				case 1:
					mflsx = yudos.pmogm;
					pbson = tujvb.pufbu;
					rsdaq = xgjbs.hjwiz;
					rwvpq = xgjbs.rrryn;
					dhiun = hvgim.gucov;
					socti.byfnx(LogLevel.Verbose, "DEFLATE", "Using fixed Huffman tree in block {0}.", kexqb);
					break;
				case 2:
					mflsx = yudos.zfqbj;
					pbson = tujvb.qanhr;
					dhiun = hvgim.reiyv;
					socti.byfnx(LogLevel.Verbose, "DEFLATE", "Using dynamic Huffman tree in block {0}.", kexqb);
					break;
				case 3:
					throw new sjylk(brgjd.edcru("Invalid compression method of the block {0}.", kexqb));
				}
				break;
			case hvgim.zpaqw:
			{
				int num2 = fxevu(16);
				if (num2 >= 0)
				{
					sxgkv = num2;
					dhiun = hvgim.zibui;
				}
				break;
			}
			case hvgim.zibui:
			{
				int num2 = fxevu(16);
				if (num2 < 0)
				{
					break;
				}
				if (sxgkv != (~num2 & 0xFFFF))
				{
					throw new sjylk(brgjd.edcru("Block length check failed in block number {0}.", kexqb));
				}
				dhiun = hvgim.gucov;
				goto case hvgim.gucov;
			}
			case hvgim.reiyv:
				oqdwn();
				break;
			case hvgim.gucov:
				switch (mflsx)
				{
				case yudos.jtxds:
					pebjm(p0, ref p1, ref p2);
					break;
				case yudos.pmogm:
				case yudos.zfqbj:
					vuwnc(p0, ref p1, ref p2);
					break;
				default:
					throw new InvalidOperationException(brgjd.edcru("Invalid compression method of the block {0}.", kexqb));
				}
				break;
			default:
				throw new InvalidOperationException("Invalid next state.");
			}
		}
		return num - p2;
	}

	public int pmhat()
	{
		if (zjonk != yosfy.aljno)
		{
			throw new InvalidOperationException("Finish method can be called only when State is CompressorState.Finish.");
		}
		if ((opclj & 7) != 0 && 0 == 0)
		{
			throw new InvalidOperationException("Invalid decompressor state.");
		}
		muiyq += opclj >> 3;
		hvbzo = null;
		vlxoz = null;
		opclj = 0;
		oorcw = 0;
		kexqb = -1;
		zjonk = yosfy.drxjq;
		khkit = true;
		return muiyq;
	}

	private int fxevu(int p0)
	{
		if (opclj < p0)
		{
			zjonk = yosfy.drxjq;
			return -1;
		}
		int result = oorcw & ((1 << p0) - 1);
		oorcw >>= p0;
		opclj -= p0;
		return result;
	}

	private void pebjm(byte[] p0, ref int p1, ref int p2)
	{
		while (opclj > 0 && sxgkv > 0 && p2 > 0)
		{
			byte b = (byte)oorcw;
			vlxoz.bttag(b);
			p0[p1] = b;
			p1++;
			p2--;
			sxgkv--;
			oorcw >>= 8;
			opclj -= 8;
			if (opclj < 0)
			{
				throw new InvalidOperationException(brgjd.edcru("Invalid decompressor state (block number {0}).", kexqb));
			}
		}
		int num = Math.Min(sxgkv, Math.Min(muiyq, p2));
		if (num > 0)
		{
			vlxoz.pjccs(hvbzo, vwmqu, num);
			Array.Copy(hvbzo, vwmqu, p0, p1, num);
			p1 += num;
			p2 -= num;
			vwmqu += num;
			muiyq -= num;
			sxgkv -= num;
		}
		if (sxgkv == 0 || 1 == 0)
		{
			dhiun = hvgim.qhlwt;
			if (veuvg && 0 == 0)
			{
				zjonk = yosfy.aljno;
				return;
			}
		}
		if (muiyq == 0 || 1 == 0)
		{
			zjonk = yosfy.drxjq;
		}
		else if (p2 == 0 || 1 == 0)
		{
			zjonk = yosfy.muyhp;
		}
	}

	private void oqdwn()
	{
		while (zjonk == (yosfy)0)
		{
			while (muiyq > 0 && opclj < 24)
			{
				oorcw |= hvbzo[vwmqu] << opclj;
				vwmqu++;
				muiyq--;
				opclj += 8;
			}
			switch (pbson)
			{
			case tujvb.qanhr:
				awmta = fxevu(5);
				if (awmta >= 0)
				{
					brsak = awmta + 257;
					if (brsak > 286)
					{
						throw new sjylk(brgjd.edcru("Invalid number of Literal/Length code lengths (block number {0}).", kexqb));
					}
					pbson = tujvb.hdbag;
				}
				break;
			case tujvb.hdbag:
				awmta = fxevu(5);
				if (awmta >= 0)
				{
					flzds = awmta + 1;
					if (flzds > ((ekhgv ? true : false) ? 32 : 30))
					{
						throw new sjylk(brgjd.edcru("Invalid number of Distance code lengths (block number {0}).", kexqb));
					}
					pbson = tujvb.pkjaa;
				}
				break;
			case tujvb.pkjaa:
				awmta = fxevu(4);
				if (awmta >= 0)
				{
					jfcyo = awmta + 4;
					if (jfcyo > xgjbs.yxjfb.Length)
					{
						throw new sjylk(brgjd.edcru("Invalid number of Dynamic tree code lengths (block number {0}).", kexqb));
					}
					if (xjhdj == null || 1 == 0)
					{
						xjhdj = new int[318];
					}
					else
					{
						Array.Clear(xjhdj, 0, xgjbs.yxjfb.Length);
					}
					jmstw = 0;
					pbson = tujvb.zmzqx;
				}
				break;
			case tujvb.zmzqx:
				awmta = fxevu(3);
				if (awmta < 0)
				{
					break;
				}
				xjhdj[xgjbs.yxjfb[jmstw++]] = awmta;
				if (jmstw >= jfcyo)
				{
					if (jmstw > jfcyo)
					{
						throw new sjylk(brgjd.edcru("Cannot build 'Code' Huffman tree (block number {0}).", kexqb));
					}
					bzedx = new xgjbs(xjhdj, xgjbs.yxjfb.Length);
					jmstw = 0;
					pbson = tujvb.qpbbz;
				}
				break;
			case tujvb.qpbbz:
				awmta = bzedx.aytpx(ref oorcw, ref opclj);
				if (awmta >= 0)
				{
					pbson = tujvb.xkxou;
				}
				else
				{
					zjonk = yosfy.drxjq;
				}
				break;
			case tujvb.xkxou:
				if (!xbege(awmta))
				{
					break;
				}
				if (jmstw >= brsak + flzds)
				{
					if (jmstw > brsak + flzds)
					{
						throw new sjylk(brgjd.edcru("Cannot build Huffman tree - too much code lengths specified (block number {0}).", kexqb));
					}
					rsdaq = new xgjbs(xjhdj, 0, brsak);
					rwvpq = new xgjbs(xjhdj, brsak, flzds);
					pbson = tujvb.pufbu;
					dhiun = hvgim.gucov;
					return;
				}
				pbson = tujvb.qpbbz;
				break;
			default:
				throw new InvalidOperationException(brgjd.edcru("Invalid 'Decoding' state ({0}) in the block {1}.", pbson, kexqb));
			}
		}
	}

	private bool xbege(int p0)
	{
		if (p0 < 16)
		{
			xjhdj[jmstw++] = p0;
			return true;
		}
		int num2;
		int num;
		switch (p0)
		{
		case 16:
			num = fxevu(2);
			if (num < 0)
			{
				break;
			}
			num += 3;
			if (jmstw == 0 || 1 == 0)
			{
				throw new sjylk(brgjd.edcru("Cannot use running length on first element (block number {0}).", kexqb));
			}
			if (jmstw + num > xjhdj.Length)
			{
				throw new sjylk(brgjd.edcru("Running length count refers beyond buffer length (block number {0}).", kexqb));
			}
			p0 = xjhdj[jmstw - 1];
			num2 = 0;
			if (num2 != 0)
			{
				goto IL_00de;
			}
			goto IL_00fd;
		case 17:
			num = fxevu(3);
			if (num >= 0)
			{
				num += 3;
				if (jmstw + num > xjhdj.Length)
				{
					throw new sjylk(brgjd.edcru("Running length count refers beyond buffer length (block number {0}).", kexqb));
				}
				Array.Clear(xjhdj, jmstw, num);
				jmstw += num;
				return true;
			}
			break;
		case 18:
			num = fxevu(7);
			if (num >= 0)
			{
				num += 11;
				if (jmstw + num > xjhdj.Length)
				{
					throw new sjylk(brgjd.edcru("Running length count refers beyond buffer length (block number {0}).", kexqb));
				}
				Array.Clear(xjhdj, jmstw, num);
				jmstw += num;
				return true;
			}
			break;
		default:
			{
				throw new InvalidOperationException(brgjd.edcru("Invalid value ({0}) supplied when building Dynamic Huffman tree (block number {1}).", pbson, kexqb));
			}
			IL_00fd:
			if (num2 >= num)
			{
				return true;
			}
			goto IL_00de;
			IL_00de:
			xjhdj[jmstw++] = p0;
			num2++;
			goto IL_00fd;
		}
		return false;
	}

	private bool uoiqt(ref int p0)
	{
		while (true)
		{
			if (opclj < 15)
			{
				oorcw |= hvbzo[vwmqu] << opclj;
				vwmqu++;
				muiyq--;
				opclj += 8;
				continue;
			}
			int num = oorcw & rsdaq.dkjky;
			int num2 = rsdaq.gmohx[num];
			int num3;
			if (num2 > 0)
			{
				num3 = rsdaq.njbsl[num];
				oorcw >>= num2;
				opclj -= num2;
			}
			else
			{
				if (rsdaq.kcznj == null || false || rsdaq.kcznj[num] == null)
				{
					throw new sjylk(brgjd.edcru("Code (0x{0:X4}) is not present in the tree.", oorcw & 0x7FFF));
				}
				int num4 = (oorcw >> rsdaq.ntgns) & rsdaq.riszl;
				num2 = rsdaq.kcznj[num][num4];
				num3 = rsdaq.bqiom[num][num4];
				if (num2 == 0 || 1 == 0)
				{
					throw new sjylk(brgjd.edcru("Code (0x{0:X4}) is not present in the tree.", oorcw & 0x7FFF));
				}
				oorcw >>= num2;
				opclj -= num2;
			}
			if (num3 < 256)
			{
				vlxoz.afnnf[vlxoz.ujzza++] = (byte)num3;
				p0--;
			}
			else
			{
				switch (num3)
				{
				case 256:
					dhiun = hvgim.qhlwt;
					if (veuvg && 0 == 0)
					{
						int num5 = opclj & 7;
						oorcw >>= num5;
						opclj -= num5;
						zjonk = yosfy.aljno;
					}
					return true;
				case 285:
					if (ekhgv && 0 == 0)
					{
						vsuib = 3;
						wgrcx = 16;
					}
					else
					{
						vsuib = 258;
						wgrcx = 0;
					}
					break;
				default:
					num3 -= 257;
					if (num3 >= isaba.Length)
					{
						throw new sjylk(brgjd.edcru("Invalid 'Literal/Length' value ({0}) was found in the input data (block number {1}).", num3, kexqb));
					}
					vsuib = isaba[num3];
					wgrcx = uclcy[num3];
					break;
				}
				if (wgrcx > 0)
				{
					while (opclj < wgrcx)
					{
						oorcw |= hvbzo[vwmqu] << opclj;
						vwmqu++;
						muiyq--;
						opclj += 8;
					}
					vsuib += oorcw & ctzgz[wgrcx];
					oorcw >>= wgrcx;
					opclj -= wgrcx;
				}
				while (opclj < 15)
				{
					oorcw |= hvbzo[vwmqu] << opclj;
					vwmqu++;
					muiyq--;
					opclj += 8;
				}
				num = oorcw & rwvpq.dkjky;
				num2 = rwvpq.gmohx[num];
				if (num2 > 0)
				{
					num3 = rwvpq.njbsl[num];
					oorcw >>= num2;
					opclj -= num2;
				}
				else
				{
					if (rwvpq.kcznj == null || false || rwvpq.kcznj[num] == null)
					{
						throw new sjylk(brgjd.edcru("Code (0x{0:X4}) is not present in the tree.", oorcw & 0x7FFF));
					}
					int num6 = (oorcw >> rwvpq.ntgns) & rwvpq.riszl;
					num2 = rwvpq.kcznj[num][num6];
					num3 = rwvpq.bqiom[num][num6];
					if (num2 == 0 || 1 == 0)
					{
						throw new sjylk(brgjd.edcru("Code (0x{0:X4}) is not present in the tree.", oorcw & 0x7FFF));
					}
					oorcw >>= num2;
					opclj -= num2;
				}
				if (num3 < 4)
				{
					lcxow = num3 + 1;
				}
				else
				{
					lcxow = aogmx[num3];
					for (wgrcx = exktx[num3]; opclj < wgrcx; opclj += 8)
					{
						oorcw |= hvbzo[vwmqu] << opclj;
						vwmqu++;
						muiyq--;
					}
					lcxow += oorcw & ctzgz[wgrcx];
					oorcw >>= wgrcx;
					opclj -= wgrcx;
				}
				if (lcxow > vlxoz.fybsd)
				{
					socti.byfnx(LogLevel.Error, "DEFLATE", "Invalid distance {0} (Is64={1}, val={2}, dist={3}, bits={4}, bmask={5:X2}).", lcxow, ekhgv, num3, aogmx[num3], exktx[num3], ctzgz[wgrcx]);
					throw new sjylk(brgjd.edcru("Invalid 'Distance' value ({0}) was found in the input data (block number {1}).", lcxow, kexqb));
				}
				if (vsuib >= p0)
				{
					if (vsuib > p0)
					{
						pbson = tujvb.xngsf;
						vsuib -= p0;
					}
					vlxoz.lvado(lcxow, p0);
					p0 = 0;
					return false;
				}
				vlxoz.lvado(lcxow, vsuib);
				p0 -= vsuib;
			}
			if (muiyq < 8 || p0 <= 0)
			{
				break;
			}
		}
		return false;
	}

	private void vuwnc(byte[] p0, ref int p1, ref int p2)
	{
		while (true)
		{
			if (opclj < 24 && muiyq > 0)
			{
				oorcw |= hvbzo[vwmqu] << opclj;
				vwmqu++;
				muiyq--;
				opclj += 8;
				continue;
			}
			switch (pbson)
			{
			case tujvb.pufbu:
			{
				if (muiyq >= 5)
				{
					if (vlxoz.ujzza == vlxoz.fybsd)
					{
						vlxoz.ujzza = 0;
					}
					int num2 = Math.Min(p2, vlxoz.fybsd - vlxoz.ujzza);
					int p3 = num2;
					bool flag = uoiqt(ref p3);
					int num3 = num2 - p3;
					vlxoz.dmrvp(p0, p1, num3);
					p1 += num3;
					p2 -= num3;
					if (flag && 0 == 0)
					{
						return;
					}
					if (p2 == 0 || 1 == 0)
					{
						zjonk = yosfy.muyhp;
						return;
					}
					break;
				}
				int num = rsdaq.aytpx(ref oorcw, ref opclj);
				if (num < 0)
				{
					zjonk = yosfy.drxjq;
					return;
				}
				if (num < 256)
				{
					if (vlxoz.ujzza == vlxoz.fybsd)
					{
						vlxoz.ujzza = 0;
					}
					vlxoz.afnnf[vlxoz.ujzza++] = (byte)num;
					p0[p1] = (byte)num;
					p1++;
					p2--;
					if (p2 == 0 || 1 == 0)
					{
						zjonk = yosfy.muyhp;
						return;
					}
					break;
				}
				if (num == 256)
				{
					dhiun = hvgim.qhlwt;
					if (veuvg && 0 == 0)
					{
						int num4 = opclj & 7;
						oorcw >>= num4;
						opclj -= num4;
						zjonk = yosfy.aljno;
					}
					return;
				}
				oemtr(num);
				break;
			}
			case tujvb.rcpmt:
				if (opclj < wgrcx)
				{
					zjonk = yosfy.drxjq;
					return;
				}
				vsuib += oorcw & ctzgz[wgrcx];
				oorcw >>= wgrcx;
				opclj -= wgrcx;
				pbson = tujvb.jtxmk;
				break;
			case tujvb.jtxmk:
			{
				int num = rwvpq.aytpx(ref oorcw, ref opclj);
				if (num < 0)
				{
					zjonk = yosfy.drxjq;
					return;
				}
				kmwtc(num);
				break;
			}
			case tujvb.pdkap:
				if (opclj < wgrcx)
				{
					zjonk = yosfy.drxjq;
					return;
				}
				lcxow += oorcw & ctzgz[wgrcx];
				oorcw >>= wgrcx;
				opclj -= wgrcx;
				pbson = tujvb.xngsf;
				goto case tujvb.xngsf;
			case tujvb.xngsf:
				if (vsuib < p2)
				{
					vlxoz.ktrjw(lcxow, vsuib, p0, p1);
					p1 += vsuib;
					p2 -= vsuib;
					pbson = tujvb.pufbu;
					break;
				}
				if (vsuib == p2)
				{
					pbson = tujvb.pufbu;
				}
				else
				{
					vsuib -= p2;
				}
				vlxoz.ktrjw(lcxow, p2, p0, p1);
				p1 += p2;
				p2 = 0;
				zjonk = yosfy.muyhp;
				return;
			default:
				throw new InvalidOperationException(brgjd.edcru("Invalid 'Decoding' state ({0}) in the block {1}.", pbson, kexqb));
			}
		}
	}

	private void oemtr(int p0)
	{
		if (p0 < 265)
		{
			vsuib = p0 - 254;
			pbson = tujvb.jtxmk;
			return;
		}
		if (p0 < 285)
		{
			vsuib = isaba[p0 - 257];
			wgrcx = p0 - 261 >> 2;
			pbson = tujvb.rcpmt;
			return;
		}
		if (p0 == 285)
		{
			if (ekhgv && 0 == 0)
			{
				vsuib = 3;
				wgrcx = 16;
				pbson = tujvb.rcpmt;
			}
			else
			{
				vsuib = 258;
				pbson = tujvb.jtxmk;
			}
			return;
		}
		throw new sjylk(brgjd.edcru("Invalid 'Literal/Length' value ({0}) was found in the input data (block number {1}).", p0, kexqb));
	}

	private void kmwtc(int p0)
	{
		if (p0 < 4)
		{
			lcxow = p0 + 1;
			pbson = tujvb.xngsf;
			return;
		}
		if (p0 < 30 || (ekhgv && 0 == 0 && p0 < 32))
		{
			lcxow = aogmx[p0];
			wgrcx = p0 - 2 >> 1;
			pbson = tujvb.pdkap;
			return;
		}
		throw new sjylk(brgjd.edcru("Invalid 'Distance' value ({0}) was found in the input data (block number {1}).", p0, kexqb));
	}
}
