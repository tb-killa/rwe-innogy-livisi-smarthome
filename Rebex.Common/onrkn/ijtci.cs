using System;

namespace onrkn;

internal class ijtci
{
	private bool bxouy;

	private int mwibw;

	private int roddv;

	private byte[] isjoe;

	private int[] infez;

	private readonly int[] gwmnl;

	private readonly int mxxhf;

	private readonly int tffla;

	private readonly int mnzir;

	private readonly int kpkbi;

	private int npztb;

	private int ydkpz;

	private int aatzl;

	private int czvqa;

	private int byqoq;

	private int cjsuk;

	private int sfwbc;

	private int tcqna;

	private int todij;

	private int lypvv;

	private static readonly int[] viltn = new int[259]
	{
		0, 0, 0, 257, 258, 259, 260, 261, 262, 263,
		264, 265, 265, 266, 266, 267, 267, 268, 268, 269,
		269, 269, 269, 270, 270, 270, 270, 271, 271, 271,
		271, 272, 272, 272, 272, 273, 273, 273, 273, 273,
		273, 273, 273, 274, 274, 274, 274, 274, 274, 274,
		274, 275, 275, 275, 275, 275, 275, 275, 275, 276,
		276, 276, 276, 276, 276, 276, 276, 277, 277, 277,
		277, 277, 277, 277, 277, 277, 277, 277, 277, 277,
		277, 277, 277, 278, 278, 278, 278, 278, 278, 278,
		278, 278, 278, 278, 278, 278, 278, 278, 278, 279,
		279, 279, 279, 279, 279, 279, 279, 279, 279, 279,
		279, 279, 279, 279, 279, 280, 280, 280, 280, 280,
		280, 280, 280, 280, 280, 280, 280, 280, 280, 280,
		280, 281, 281, 281, 281, 281, 281, 281, 281, 281,
		281, 281, 281, 281, 281, 281, 281, 281, 281, 281,
		281, 281, 281, 281, 281, 281, 281, 281, 281, 281,
		281, 281, 281, 282, 282, 282, 282, 282, 282, 282,
		282, 282, 282, 282, 282, 282, 282, 282, 282, 282,
		282, 282, 282, 282, 282, 282, 282, 282, 282, 282,
		282, 282, 282, 282, 282, 283, 283, 283, 283, 283,
		283, 283, 283, 283, 283, 283, 283, 283, 283, 283,
		283, 283, 283, 283, 283, 283, 283, 283, 283, 283,
		283, 283, 283, 283, 283, 283, 283, 284, 284, 284,
		284, 284, 284, 284, 284, 284, 284, 284, 284, 284,
		284, 284, 284, 284, 284, 284, 284, 284, 284, 284,
		284, 284, 284, 284, 284, 284, 284, 284, 285
	};

	private static readonly int[] gvwpx = new int[29]
	{
		3, 4, 5, 6, 7, 8, 9, 10, 11, 13,
		15, 17, 19, 23, 27, 31, 35, 43, 51, 59,
		67, 83, 99, 115, 131, 163, 195, 227, 258
	};

	internal static readonly int[] vxjfo = new int[29]
	{
		0, 0, 0, 0, 0, 0, 0, 0, 1, 1,
		1, 1, 2, 2, 2, 2, 3, 3, 3, 3,
		4, 4, 4, 4, 5, 5, 5, 5, 0
	};

	private static readonly byte[] neoot = new byte[767]
	{
		0, 0, 1, 2, 3, 4, 4, 5, 5, 6,
		6, 6, 6, 7, 7, 7, 7, 8, 8, 8,
		8, 8, 8, 8, 8, 9, 9, 9, 9, 9,
		9, 9, 9, 10, 10, 10, 10, 10, 10, 10,
		10, 10, 10, 10, 10, 10, 10, 10, 10, 11,
		11, 11, 11, 11, 11, 11, 11, 11, 11, 11,
		11, 11, 11, 11, 11, 12, 12, 12, 12, 12,
		12, 12, 12, 12, 12, 12, 12, 12, 12, 12,
		12, 12, 12, 12, 12, 12, 12, 12, 12, 12,
		12, 12, 12, 12, 12, 12, 12, 13, 13, 13,
		13, 13, 13, 13, 13, 13, 13, 13, 13, 13,
		13, 13, 13, 13, 13, 13, 13, 13, 13, 13,
		13, 13, 13, 13, 13, 13, 13, 13, 13, 14,
		14, 14, 14, 14, 14, 14, 14, 14, 14, 14,
		14, 14, 14, 14, 14, 14, 14, 14, 14, 14,
		14, 14, 14, 14, 14, 14, 14, 14, 14, 14,
		14, 14, 14, 14, 14, 14, 14, 14, 14, 14,
		14, 14, 14, 14, 14, 14, 14, 14, 14, 14,
		14, 14, 14, 14, 14, 14, 14, 14, 14, 14,
		14, 14, 14, 15, 15, 15, 15, 15, 15, 15,
		15, 15, 15, 15, 15, 15, 15, 15, 15, 15,
		15, 15, 15, 15, 15, 15, 15, 15, 15, 15,
		15, 15, 15, 15, 15, 15, 15, 15, 15, 15,
		15, 15, 15, 15, 15, 15, 15, 15, 15, 15,
		15, 15, 15, 15, 15, 15, 15, 15, 15, 15,
		15, 15, 15, 15, 15, 15, 15, 16, 17, 18,
		18, 19, 19, 20, 20, 20, 20, 21, 21, 21,
		21, 22, 22, 22, 22, 22, 22, 22, 22, 23,
		23, 23, 23, 23, 23, 23, 23, 24, 24, 24,
		24, 24, 24, 24, 24, 24, 24, 24, 24, 24,
		24, 24, 24, 25, 25, 25, 25, 25, 25, 25,
		25, 25, 25, 25, 25, 25, 25, 25, 25, 26,
		26, 26, 26, 26, 26, 26, 26, 26, 26, 26,
		26, 26, 26, 26, 26, 26, 26, 26, 26, 26,
		26, 26, 26, 26, 26, 26, 26, 26, 26, 26,
		26, 27, 27, 27, 27, 27, 27, 27, 27, 27,
		27, 27, 27, 27, 27, 27, 27, 27, 27, 27,
		27, 27, 27, 27, 27, 27, 27, 27, 27, 27,
		27, 27, 27, 28, 28, 28, 28, 28, 28, 28,
		28, 28, 28, 28, 28, 28, 28, 28, 28, 28,
		28, 28, 28, 28, 28, 28, 28, 28, 28, 28,
		28, 28, 28, 28, 28, 28, 28, 28, 28, 28,
		28, 28, 28, 28, 28, 28, 28, 28, 28, 28,
		28, 28, 28, 28, 28, 28, 28, 28, 28, 28,
		28, 28, 28, 28, 28, 28, 28, 29, 29, 29,
		29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
		29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
		29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
		29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
		29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
		29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
		29, 30, 30, 30, 30, 30, 30, 30, 30, 30,
		30, 30, 30, 30, 30, 30, 30, 30, 30, 30,
		30, 30, 30, 30, 30, 30, 30, 30, 30, 30,
		30, 30, 30, 30, 30, 30, 30, 30, 30, 30,
		30, 30, 30, 30, 30, 30, 30, 30, 30, 30,
		30, 30, 30, 30, 30, 30, 30, 30, 30, 30,
		30, 30, 30, 30, 30, 30, 30, 30, 30, 30,
		30, 30, 30, 30, 30, 30, 30, 30, 30, 30,
		30, 30, 30, 30, 30, 30, 30, 30, 30, 30,
		30, 30, 30, 30, 30, 30, 30, 30, 30, 30,
		30, 30, 30, 30, 30, 30, 30, 30, 30, 30,
		30, 30, 30, 30, 30, 30, 30, 30, 30, 30,
		30, 30, 30, 30, 30, 30, 30, 30, 30, 31,
		31, 31, 31, 31, 31, 31, 31, 31, 31, 31,
		31, 31, 31, 31, 31, 31, 31, 31, 31, 31,
		31, 31, 31, 31, 31, 31, 31, 31, 31, 31,
		31, 31, 31, 31, 31, 31, 31, 31, 31, 31,
		31, 31, 31, 31, 31, 31, 31, 31, 31, 31,
		31, 31, 31, 31, 31, 31, 31, 31, 31, 31,
		31, 31, 31, 31, 31, 31, 31, 31, 31, 31,
		31, 31, 31, 31, 31, 31, 31, 31, 31, 31,
		31, 31, 31, 31, 31, 31, 31, 31, 31, 31,
		31, 31, 31, 31, 31, 31, 31, 31, 31, 31,
		31, 31, 31, 31, 31, 31, 31, 31, 31, 31,
		31, 31, 31, 31, 31, 31, 31, 31, 31, 31,
		31, 31, 31, 31, 31, 31, 31
	};

	private static readonly int[] nbymi = new int[32]
	{
		1, 2, 3, 4, 5, 7, 9, 13, 17, 25,
		33, 49, 65, 97, 129, 193, 257, 385, 513, 769,
		1025, 1537, 2049, 3073, 4097, 6145, 8193, 12289, 16385, 24577,
		32769, 49153
	};

	internal static readonly int[] yoljm = new int[32]
	{
		0, 0, 0, 0, 1, 1, 2, 2, 3, 3,
		4, 4, 5, 5, 6, 6, 7, 7, 8, 8,
		9, 9, 10, 10, 11, 11, 12, 12, 13, 13,
		14, 14
	};

	public int gjqaw
	{
		get
		{
			return lypvv;
		}
		set
		{
			lypvv = value;
			switch (value)
			{
			case 1:
				ydkpz = 4;
				aatzl = 4;
				break;
			case 2:
				ydkpz = 8;
				aatzl = 8;
				break;
			case 3:
				ydkpz = 16;
				aatzl = 16;
				break;
			case 4:
				ydkpz = 32;
				aatzl = 32;
				break;
			case 5:
				ydkpz = 64;
				aatzl = 32;
				czvqa = 8;
				byqoq = 8;
				break;
			case 6:
				ydkpz = 128;
				aatzl = 64;
				czvqa = 12;
				byqoq = 16;
				break;
			case 7:
				ydkpz = 192;
				aatzl = 96;
				czvqa = 14;
				byqoq = 24;
				break;
			case 8:
				ydkpz = 512;
				aatzl = 192;
				czvqa = 24;
				byqoq = 64;
				break;
			case 9:
				ydkpz = 2048;
				aatzl = 258;
				czvqa = 32;
				byqoq = 257;
				break;
			case 10:
				ydkpz = 3192;
				aatzl = 258;
				czvqa = 48;
				byqoq = 257;
				break;
			default:
				throw hifyx.nztrs("value", value, "Compression level has to be a number between 0 and 9.");
			case 0:
				break;
			}
		}
	}

	public bool obzio
	{
		set
		{
			bxouy = value;
			if (value && 0 == 0)
			{
				roddv = kpkbi * 3;
				npztb = kpkbi + 2;
				mwibw = 65538;
			}
			else
			{
				roddv = kpkbi * 2;
				npztb = 262;
				mwibw = 258;
			}
			if (isjoe == null || false || isjoe.Length < roddv)
			{
				infez = new int[roddv];
				isjoe = new byte[roddv];
			}
		}
	}

	public ijtci(bool useEnhancedDeflate, int keysCapacityBits, int lz77WindowSizeBits, int compressionLevel)
	{
		kpkbi = 1 << lz77WindowSizeBits;
		gjqaw = compressionLevel;
		obzio = useEnhancedDeflate;
		mxxhf = 1 << keysCapacityBits;
		tffla = mxxhf - 1;
		gwmnl = new int[mxxhf];
		mnzir = (keysCapacityBits - 1) / 3 + 1;
	}

	public int onfjx(byte[] p0, int p1, int p2, int[] p3, int p4, int[] p5, int[] p6, ref int p7)
	{
		int num = p4;
		sxnga(p0, ref p1, ref p2);
		if (sfwbc >= 3)
		{
			tcqna = (isjoe[cjsuk] << mnzir) ^ isjoe[cjsuk + 1];
		}
		while (true)
		{
			if (sfwbc <= npztb)
			{
				sxnga(p0, ref p1, ref p2);
				if (sfwbc == 0 || 1 == 0)
				{
					break;
				}
			}
			if (sfwbc >= 3)
			{
				int p8 = 0;
				int p9 = 2;
				if (mootp(ydkpz, ref p8, ref p9) && 0 == 0)
				{
					int num2 = p9;
					if (p9 <= byqoq && p9 < sfwbc)
					{
						cjsuk++;
						sfwbc--;
						num2--;
						if (mootp((p9 >= czvqa) ? (ydkpz >> 2) : ydkpz, ref p8, ref p9) && 0 == 0)
						{
							num2 = p9;
							byte b = isjoe[cjsuk - 1];
							p3[p4++] = b;
							p5[b]++;
						}
					}
					int num3 = cjsuk;
					cjsuk += num2;
					sfwbc -= num2;
					int num4 = ((sfwbc >= 2) ? cjsuk : (cjsuk - 2 + sfwbc));
					if (lypvv <= 3 && num2 > 16)
					{
						switch (lypvv)
						{
						case 1:
							num3 = num4 - 4;
							break;
						case 2:
							num3 = num4 - Math.Min(16, num2 >> 1);
							break;
						case 3:
							num3 = num4 - Math.Min(32, num2 >> 1);
							break;
						}
						tcqna = (isjoe[num3 + 1] << mnzir) ^ isjoe[num3 + 2];
					}
					while (++num3 < num4)
					{
						tcqna = ((tcqna << mnzir) ^ isjoe[num3 + 2]) & tffla;
						infez[num3] = gwmnl[tcqna];
						gwmnl[tcqna] = num3 + todij;
					}
					if (p9 >= 258 && bxouy)
					{
						p3[p4++] = 285;
						p5[285]++;
						p3[p4++] = p9 - 3;
						p7 += 16;
					}
					else
					{
						int num5 = viltn[p9];
						p3[p4++] = num5;
						p5[num5]++;
						num5 -= 257;
						p3[p4++] = p9 - gvwpx[num5];
						p7 += vxjfo[num5];
					}
					int num6 = ((p8 <= 256) ? neoot[p8] : neoot[255 + (p8 - 1 >> 7)]);
					p3[p4++] = num6;
					p6[num6]++;
					p3[p4++] = p8 - nbymi[num6];
					p7 += yoljm[num6];
					continue;
				}
			}
			byte b2 = isjoe[cjsuk++];
			p3[p4++] = b2;
			p5[b2]++;
			sfwbc--;
		}
		return p4 - num;
	}

	private bool mootp(int p0, ref int p1, ref int p2)
	{
		tcqna = ((tcqna << mnzir) ^ isjoe[cjsuk + 2]) & tffla;
		int num = gwmnl[tcqna];
		infez[cjsuk] = num;
		gwmnl[tcqna] = cjsuk + todij;
		num -= todij;
		int num2 = ((cjsuk <= kpkbi) ? 1 : (cjsuk - kpkbi));
		if (num < num2)
		{
			return false;
		}
		int num3 = p2;
		int num4 = p0;
		byte b = isjoe[cjsuk];
		byte b2 = isjoe[cjsuk + 1];
		byte b3 = isjoe[cjsuk + 2];
		byte b4 = isjoe[cjsuk + p2];
		int num5 = cjsuk + ((sfwbc < mwibw) ? sfwbc : mwibw);
		do
		{
			int num6 = num;
			if (isjoe[num + p2] != b4 || isjoe[num6] != b || isjoe[++num6] != b2 || isjoe[++num6] != b3)
			{
				continue;
			}
			int num7 = cjsuk + 2;
			while (isjoe[++num6] == isjoe[++num7] && num7 < num5)
			{
			}
			int num8 = num7 - cjsuk;
			if (num8 > p2)
			{
				p1 = cjsuk - num;
				if (num8 >= sfwbc)
				{
					p2 = sfwbc;
					return true;
				}
				p2 = num8;
				if (num8 >= aatzl)
				{
					return true;
				}
				b4 = isjoe[cjsuk + num8];
			}
		}
		while ((num = infez[num] - todij) >= num2 && num4-- > 0);
		return p2 > num3;
	}

	private void sxnga(byte[] p0, ref int p1, ref int p2)
	{
		if (cjsuk + npztb >= roddv)
		{
			aktnk();
		}
		if (p2 > 0)
		{
			int num = cjsuk + sfwbc;
			int num2 = Math.Min(p2, roddv - num);
			Array.Copy(p0, p1, isjoe, num, num2);
			p1 += num2;
			p2 -= num2;
			sfwbc += num2;
		}
	}

	private void aktnk()
	{
		int num = cjsuk - kpkbi;
		Array.Copy(isjoe, num, isjoe, 0, kpkbi + sfwbc);
		cjsuk = kpkbi;
		todij += num;
		if (todij < int.MaxValue - roddv)
		{
			Array.Copy(infez, num, infez, 0, kpkbi + sfwbc);
			return;
		}
		Array.Clear(gwmnl, 0, mxxhf);
		Array.Clear(infez, 0, cjsuk);
		todij = 0;
	}
}
