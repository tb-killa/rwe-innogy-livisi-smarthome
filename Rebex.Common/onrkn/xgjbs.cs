using System;

namespace onrkn;

internal class xgjbs
{
	private struct djjbt : IComparable
	{
		public int qgbed;

		public int almyl;

		public int wkoqp;

		public int CompareTo(object obj)
		{
			djjbt djjbt = (djjbt)obj;
			if (qgbed < djjbt.qgbed)
			{
				return -1;
			}
			if (qgbed > djjbt.qgbed)
			{
				return 1;
			}
			return 0;
		}
	}

	private struct ppawx
	{
		public int imkcj;

		public int yyeqi;
	}

	private class zfzjh : IComparable<zfzjh>
	{
		public int trisw;

		public int kywyv;

		public zfzjh(int value, int codeLength)
		{
			trisw = value;
			kywyv = codeLength;
		}

		public int CompareTo(zfzjh obj)
		{
			if (kywyv < obj.kywyv)
			{
				return -1;
			}
			if (kywyv > obj.kywyv)
			{
				return 1;
			}
			if (trisw < obj.trisw)
			{
				return -1;
			}
			if (trisw > obj.trisw)
			{
				return 1;
			}
			return 0;
		}
	}

	private const int ctvgz = 7;

	public static readonly int[] mkfjy = new int[288]
	{
		8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
		8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
		8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
		8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
		8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
		8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
		8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
		8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
		8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
		8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
		8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
		8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
		8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
		8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
		8, 8, 8, 8, 9, 9, 9, 9, 9, 9,
		9, 9, 9, 9, 9, 9, 9, 9, 9, 9,
		9, 9, 9, 9, 9, 9, 9, 9, 9, 9,
		9, 9, 9, 9, 9, 9, 9, 9, 9, 9,
		9, 9, 9, 9, 9, 9, 9, 9, 9, 9,
		9, 9, 9, 9, 9, 9, 9, 9, 9, 9,
		9, 9, 9, 9, 9, 9, 9, 9, 9, 9,
		9, 9, 9, 9, 9, 9, 9, 9, 9, 9,
		9, 9, 9, 9, 9, 9, 9, 9, 9, 9,
		9, 9, 9, 9, 9, 9, 9, 9, 9, 9,
		9, 9, 9, 9, 9, 9, 9, 9, 9, 9,
		9, 9, 9, 9, 9, 9, 7, 7, 7, 7,
		7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
		7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
		8, 8, 8, 8, 8, 8, 8, 8
	};

	public static readonly int[] vqcba = new int[288]
	{
		12, 140, 76, 204, 44, 172, 108, 236, 28, 156,
		92, 220, 60, 188, 124, 252, 2, 130, 66, 194,
		34, 162, 98, 226, 18, 146, 82, 210, 50, 178,
		114, 242, 10, 138, 74, 202, 42, 170, 106, 234,
		26, 154, 90, 218, 58, 186, 122, 250, 6, 134,
		70, 198, 38, 166, 102, 230, 22, 150, 86, 214,
		54, 182, 118, 246, 14, 142, 78, 206, 46, 174,
		110, 238, 30, 158, 94, 222, 62, 190, 126, 254,
		1, 129, 65, 193, 33, 161, 97, 225, 17, 145,
		81, 209, 49, 177, 113, 241, 9, 137, 73, 201,
		41, 169, 105, 233, 25, 153, 89, 217, 57, 185,
		121, 249, 5, 133, 69, 197, 37, 165, 101, 229,
		21, 149, 85, 213, 53, 181, 117, 245, 13, 141,
		77, 205, 45, 173, 109, 237, 29, 157, 93, 221,
		61, 189, 125, 253, 19, 275, 147, 403, 83, 339,
		211, 467, 51, 307, 179, 435, 115, 371, 243, 499,
		11, 267, 139, 395, 75, 331, 203, 459, 43, 299,
		171, 427, 107, 363, 235, 491, 27, 283, 155, 411,
		91, 347, 219, 475, 59, 315, 187, 443, 123, 379,
		251, 507, 7, 263, 135, 391, 71, 327, 199, 455,
		39, 295, 167, 423, 103, 359, 231, 487, 23, 279,
		151, 407, 87, 343, 215, 471, 55, 311, 183, 439,
		119, 375, 247, 503, 15, 271, 143, 399, 79, 335,
		207, 463, 47, 303, 175, 431, 111, 367, 239, 495,
		31, 287, 159, 415, 95, 351, 223, 479, 63, 319,
		191, 447, 127, 383, 255, 511, 0, 64, 32, 96,
		16, 80, 48, 112, 8, 72, 40, 104, 24, 88,
		56, 120, 4, 68, 36, 100, 20, 84, 52, 116,
		3, 131, 67, 195, 35, 163, 99, 227
	};

	public static readonly int[] jtsmd = new int[32]
	{
		5, 5, 5, 5, 5, 5, 5, 5, 5, 5,
		5, 5, 5, 5, 5, 5, 5, 5, 5, 5,
		5, 5, 5, 5, 5, 5, 5, 5, 5, 5,
		5, 5
	};

	public static readonly int[] ydmaw = new int[32]
	{
		0, 16, 8, 24, 4, 20, 12, 28, 2, 18,
		10, 26, 6, 22, 14, 30, 1, 17, 9, 25,
		5, 21, 13, 29, 3, 19, 11, 27, 7, 23,
		15, 31
	};

	public static readonly int[] yxjfb = new int[19]
	{
		16, 17, 18, 0, 8, 7, 9, 6, 10, 5,
		11, 4, 12, 3, 13, 2, 14, 1, 15
	};

	private static xgjbs tnsir;

	private static xgjbs qjunj;

	public readonly int ntgns;

	public readonly int dkjky;

	public readonly int[] njbsl;

	public readonly int[] gmohx;

	public readonly int ifrfi;

	public readonly int riszl;

	public readonly int[][] bqiom;

	public readonly int[][] kcznj;

	public static xgjbs hjwiz
	{
		get
		{
			if (tnsir == null || 1 == 0)
			{
				lock (mkfjy)
				{
					if (tnsir == null || 1 == 0)
					{
						tnsir = new xgjbs(mkfjy, mkfjy.Length);
					}
				}
			}
			return tnsir;
		}
	}

	public static xgjbs rrryn
	{
		get
		{
			if (qjunj == null || 1 == 0)
			{
				lock (jtsmd)
				{
					if (qjunj == null || 1 == 0)
					{
						qjunj = new xgjbs(jtsmd, jtsmd.Length);
					}
				}
			}
			return qjunj;
		}
	}

	public xgjbs(int[] codeLengths, int count)
		: this(codeLengths, 0, count)
	{
	}

	public xgjbs(int[] codeLengths, int offset, int count)
	{
		int[] array = new int[17];
		int num = 17;
		int num2 = 0;
		for (int i = offset; i < offset + count; i++)
		{
			if (codeLengths[i] > 0)
			{
				if (codeLengths[i] >= array.Length)
				{
					throw new sjylk(brgjd.edcru("Invalid 'Code length' ({0}) found in the input data.", codeLengths[i]));
				}
				array[codeLengths[i]]++;
				if (codeLengths[i] > num2)
				{
					num2 = codeLengths[i];
				}
				else if (codeLengths[i] < num)
				{
					num = codeLengths[i];
				}
			}
		}
		int num3 = 0;
		array[0] = 0;
		int[] array2 = new int[num2 + 1];
		int num4 = 1;
		if (num4 == 0)
		{
			goto IL_0099;
		}
		goto IL_00b3;
		IL_00b3:
		if (num4 <= num2)
		{
			goto IL_0099;
		}
		int[] array3 = new int[count];
		int num5 = 0;
		if (num5 != 0)
		{
			goto IL_00c7;
		}
		goto IL_0101;
		IL_0312:
		int num6;
		if (num6 >= count)
		{
			return;
		}
		goto IL_019d;
		IL_0300:
		num6++;
		goto IL_0312;
		IL_01dc:
		int num8;
		int num9;
		int num7 = num3 | (num8 << num9);
		njbsl[num7] = num6;
		gmohx[num7] = num9;
		num8++;
		goto IL_0205;
		IL_019d:
		num9 = codeLengths[offset + num6];
		int num10;
		int num11;
		int num12;
		int num13;
		int num14;
		int num15;
		if (num9 > 0)
		{
			num3 = mkqxg(array3[num6], num9);
			if (num9 < ntgns)
			{
				num10 = 1 << ntgns - num9;
				num8 = 0;
				if (num8 != 0)
				{
					goto IL_01dc;
				}
				goto IL_0205;
			}
			if (num9 == ntgns)
			{
				njbsl[num3] = num6;
				gmohx[num3] = num9;
			}
			else
			{
				num11 = num3 & dkjky;
				if (bqiom[num11] == null || 1 == 0)
				{
					bqiom[num11] = new int[num12];
					kcznj[num11] = new int[num12];
				}
				num3 >>= ntgns;
				if (num9 < ifrfi)
				{
					num13 = 1 << ifrfi - num9;
					num14 = num9 - ntgns;
					num15 = 0;
					if (num15 != 0)
					{
						goto IL_02ad;
					}
					goto IL_02dc;
				}
				bqiom[num11][num3] = num6;
				kcznj[num11][num3] = num9;
			}
		}
		goto IL_0300;
		IL_02dc:
		if (num15 < num13)
		{
			goto IL_02ad;
		}
		goto IL_0300;
		IL_0101:
		if (num5 < count)
		{
			goto IL_00c7;
		}
		ntgns = Math.Min(num + 7, num2);
		int num16 = 1 << ntgns;
		dkjky = num16 - 1;
		njbsl = new int[num16];
		gmohx = new int[num16];
		ifrfi = num2;
		num12 = 1 << ifrfi - ntgns;
		if (ifrfi > ntgns)
		{
			riszl = num12 - 1;
			bqiom = new int[num16][];
			kcznj = new int[num16][];
		}
		num6 = 0;
		if (num6 != 0)
		{
			goto IL_019d;
		}
		goto IL_0312;
		IL_02ad:
		int num17 = num3 | (num15 << num14);
		bqiom[num11][num17] = num6;
		kcznj[num11][num17] = num9;
		num15++;
		goto IL_02dc;
		IL_0205:
		if (num8 < num10)
		{
			goto IL_01dc;
		}
		goto IL_0300;
		IL_00c7:
		num9 = codeLengths[offset + num5];
		if (num9 != 0 && 0 == 0)
		{
			array3[num5] = array2[num9];
			array2[num9]++;
		}
		num5++;
		goto IL_0101;
		IL_0099:
		num3 = (array2[num4] = num3 + array[num4 - 1] << 1);
		num4++;
		goto IL_00b3;
	}

	private static int mkqxg(int p0, int p1)
	{
		int num = 0;
		int num2 = 1 << p1 - 1;
		int num3 = 1;
		if (num3 == 0)
		{
			goto IL_0011;
		}
		goto IL_0023;
		IL_0011:
		if ((p0 & num2) > 0)
		{
			num |= num3;
		}
		num3 <<= 1;
		num2 >>= 1;
		goto IL_0023;
		IL_0023:
		if (num2 > 0)
		{
			goto IL_0011;
		}
		return num;
	}

	public int aytpx(ref int p0, ref int p1)
	{
		int num = p0 & dkjky;
		int num2 = gmohx[num];
		if (num2 > 0)
		{
			if (num2 > p1)
			{
				return -1;
			}
			p0 >>= num2;
			p1 -= num2;
			return njbsl[num];
		}
		if (p1 < ntgns)
		{
			return -1;
		}
		if (kcznj == null || false || kcznj[num] == null)
		{
			throw new sjylk(brgjd.edcru("Code (0x{0:X4}) is not present in the tree.", p0 & 0x7FFF));
		}
		int num3 = (p0 >> ntgns) & riszl;
		num2 = kcznj[num][num3];
		if (num2 == 0 || 1 == 0)
		{
			throw new sjylk(brgjd.edcru("Code (0x{0:X4}) is not present in the tree.", p0 & 0x7FFF));
		}
		if (num2 > p1)
		{
			return -1;
		}
		p0 >>= num2;
		p1 -= num2;
		return bqiom[num][num3];
	}

	public static int fvvzz(int[] p0, int[] p1, int[] p2, int p3)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		if (num3 != 0)
		{
			goto IL_000d;
		}
		goto IL_0024;
		IL_000d:
		if (p0[num3] != 0 && 0 == 0)
		{
			num2++;
			num = num3;
		}
		num3++;
		goto IL_0024;
		IL_0024:
		if (num3 >= p0.Length)
		{
			num++;
			if (num2 == 0 || 1 == 0)
			{
				return 1;
			}
			int num17;
			ppawx[] array;
			int p4;
			djjbt[] array2;
			int num4;
			djjbt[] array3;
			int num5;
			int num6;
			int imkcj;
			int num7;
			zfzjh[] array4;
			int num8;
			djjbt[] array5;
			int num9;
			int num10;
			int num11;
			int num12;
			int num13;
			int num14;
			int kywyv;
			int num15;
			int num16;
			int num18;
			switch (num2)
			{
			case 1:
				p1[num - 1] = 1;
				return num;
			case 2:
				num17 = 0;
				if (num17 != 0)
				{
					goto IL_0050;
				}
				goto IL_0067;
			default:
				{
					array = new ppawx[num2 * p3 + 1];
					p4 = 1;
					array2 = new djjbt[num2];
					num2 = 0;
					num4 = 0;
					if (num4 != 0)
					{
						goto IL_0099;
					}
					goto IL_00f6;
				}
				IL_00f6:
				if (num4 < p0.Length)
				{
					goto IL_0099;
				}
				Array.Sort(array2);
				array3 = (djjbt[])array2.Clone();
				num5 = fulyc(array3, array3.Length, array);
				num6 = 0;
				if (num6 != 0)
				{
					goto IL_0128;
				}
				goto IL_014e;
				IL_028a:
				imkcj = array[array2[num7].almyl].imkcj;
				array4[num7] = new zfzjh(imkcj, p1[imkcj]);
				num7++;
				goto IL_02bc;
				IL_014e:
				if (num6 < p3 - 2)
				{
					goto IL_0128;
				}
				num8 = num2 - 1 << 1;
				array5 = new djjbt[num8];
				num9 = 0;
				num10 = 0;
				num11 = 0;
				if (num11 != 0)
				{
					goto IL_0175;
				}
				goto IL_020d;
				IL_021d:
				num12 = array5[num13].almyl;
				while ((num12 != 0) ? true : false)
				{
					p1[array[num12].imkcj]++;
					num12 = array[num12].yyeqi;
				}
				num13++;
				goto IL_0272;
				IL_020d:
				if (num9 < num8)
				{
					goto IL_0175;
				}
				num13 = 0;
				if (num13 != 0)
				{
					goto IL_021d;
				}
				goto IL_0272;
				IL_0128:
				array3 = tbbnl(array3, num5, array2, array, ref p4);
				num5 = fulyc(array3, array3.Length, array);
				num6++;
				goto IL_014e;
				IL_0272:
				if (num13 < num8)
				{
					goto IL_021d;
				}
				array4 = new zfzjh[array2.Length];
				num7 = 0;
				if (num7 != 0)
				{
					goto IL_028a;
				}
				goto IL_02bc;
				IL_0099:
				if (p0[num4] != 0 && 0 == 0)
				{
					array[p4].imkcj = num4;
					array2[num2].almyl = p4;
					array2[num2].wkoqp = p4;
					array2[num2].qgbed = p0[num4];
					p4++;
					num2++;
				}
				num4++;
				goto IL_00f6;
				IL_02bc:
				if (num7 < array2.Length)
				{
					goto IL_028a;
				}
				Array.Sort(array4);
				p2[array4[0].trisw] = 0;
				num14 = 0;
				kywyv = array4[0].kywyv;
				num15 = 1 << kywyv - 1;
				num16 = 1;
				if (num16 == 0)
				{
					goto IL_02f7;
				}
				goto IL_0363;
				IL_0175:
				if (num11 == num5 || (num10 < array2.Length && array2[num10].qgbed < array3[num11].qgbed))
				{
					array[p4].imkcj = array[array2[num10].almyl].imkcj;
					array5[num9].almyl = p4++;
					num10++;
				}
				else
				{
					ref djjbt reference = ref array5[num9];
					reference = array3[num11++];
				}
				num9++;
				goto IL_020d;
				IL_0363:
				if (num16 < array4.Length)
				{
					goto IL_02f7;
				}
				return num;
				IL_0050:
				if (p0[num17] != 0)
				{
					p1[num17] = 1;
					goto IL_006b;
				}
				num17++;
				goto IL_0067;
				IL_02f7:
				num18 = num15;
				while (((num14 & num18) != 0) ? true : false)
				{
					num14 ^= num18;
					num18 >>= 1;
				}
				num14 |= num18;
				if (array4[num16].kywyv != kywyv)
				{
					num15 <<= array4[num16].kywyv - kywyv;
					kywyv = array4[num16].kywyv;
				}
				p2[array4[num16].trisw] = num14;
				num16++;
				goto IL_0363;
				IL_0067:
				if (num17 < num)
				{
					goto IL_0050;
				}
				goto IL_006b;
				IL_006b:
				p1[num - 1] = 1;
				p2[num - 1] = 1;
				return num;
			}
		}
		goto IL_000d;
	}

	private static int fulyc(djjbt[] p0, int p1, ppawx[] p2)
	{
		int num = 0;
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_000e;
		}
		goto IL_0094;
		IL_000e:
		p0[num].qgbed = p0[num2].qgbed + p0[num2 + 1].qgbed;
		p2[p0[num2].wkoqp].yyeqi = p0[num2 + 1].almyl;
		p0[num].almyl = p0[num2].almyl;
		p0[num].wkoqp = p0[num2 + 1].wkoqp;
		num++;
		num2 += 2;
		goto IL_0094;
		IL_0094:
		if (num2 < p1 - 1)
		{
			goto IL_000e;
		}
		return num;
	}

	private static djjbt[] tbbnl(djjbt[] p0, int p1, djjbt[] p2, ppawx[] p3, ref int p4)
	{
		djjbt[] array = new djjbt[p2.Length + p1];
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		if (num3 != 0)
		{
			goto IL_001b;
		}
		goto IL_00ce;
		IL_001b:
		if (num3 == p1 || (num2 < p2.Length && p2[num2].qgbed < p0[num3].qgbed))
		{
			p3[p4].imkcj = p3[p2[num2].almyl].imkcj;
			array[num].qgbed = p2[num2].qgbed;
			array[num].almyl = (array[num].wkoqp = p4++);
			num2++;
		}
		else
		{
			ref djjbt reference = ref array[num];
			reference = p0[num3++];
		}
		num++;
		goto IL_00ce;
		IL_00ce:
		if (num < array.Length)
		{
			goto IL_001b;
		}
		return array;
	}

	public static void urils(byte[] p0, out int p1, out int p2, int[] p3, int p4, int[] p5, int p6)
	{
		int[] array = new int[19];
		int[] array2 = new int[p4 + p6];
		int p7 = 0;
		tihch(array2, ref p7, array, p3, p4);
		tihch(array2, ref p7, array, p5, p6);
		int[] array3 = new int[19];
		int[] array4 = new int[19];
		fvvzz(array, array3, array4, 7);
		int[] array5 = new int[19];
		int num = 0;
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_0061;
		}
		goto IL_0082;
		IL_0061:
		array5[num2] = array3[yxjfb[num2]];
		if (array5[num2] > 0)
		{
			num = num2;
		}
		num2++;
		goto IL_0082;
		IL_00fa:
		int num4;
		int num3 = array2[num4];
		bfabj(p0, ref p1, ref p2, array4[num3], array3[num3]);
		switch (num3)
		{
		case 16:
			bfabj(p0, ref p1, ref p2, array2[++num4], 2);
			break;
		case 17:
			bfabj(p0, ref p1, ref p2, array2[++num4], 3);
			break;
		case 18:
			bfabj(p0, ref p1, ref p2, array2[++num4], 7);
			break;
		}
		num4++;
		goto IL_0175;
		IL_0175:
		if (num4 >= p7)
		{
			return;
		}
		goto IL_00fa;
		IL_0082:
		if (num2 < 19)
		{
			goto IL_0061;
		}
		if (num < 4)
		{
			num = 4;
			if (num != 0)
			{
				goto IL_009a;
			}
		}
		num++;
		goto IL_009a;
		IL_009a:
		p1 = (p2 = 0);
		bfabj(p0, ref p1, ref p2, p4 - 257, 5);
		bfabj(p0, ref p1, ref p2, p6 - 1, 5);
		bfabj(p0, ref p1, ref p2, num - 4, 4);
		int num5 = 0;
		if (num5 != 0)
		{
			goto IL_00d6;
		}
		goto IL_00ea;
		IL_00ea:
		if (num5 < num)
		{
			goto IL_00d6;
		}
		num4 = 0;
		if (num4 != 0)
		{
			goto IL_00fa;
		}
		goto IL_0175;
		IL_00d6:
		bfabj(p0, ref p1, ref p2, array5[num5], 3);
		num5++;
		goto IL_00ea;
	}

	private static void tihch(int[] p0, ref int p1, int[] p2, int[] p3, int p4)
	{
		int num = 0;
		if (num != 0)
		{
			goto IL_000c;
		}
		goto IL_0174;
		IL_000c:
		int num2 = p3[num];
		if (num2 == 0 || 1 == 0)
		{
			int num3 = 1;
			int num4 = num + 1;
			while (num4 < p4 && p3[num4] == num2 && num3 < 138)
			{
				num4++;
				num3++;
			}
			if (num3 >= 11)
			{
				p0[p1++] = 18;
				p0[p1++] = num3 - 11;
				p2[18]++;
				num += num3 - 1;
			}
			else
			{
				if (num3 < 3)
				{
					goto IL_0146;
				}
				p0[p1++] = 17;
				p0[p1++] = num3 - 3;
				p2[17]++;
				num += num3 - 1;
			}
			goto IL_0168;
		}
		if (num > 0 && num2 == p3[num - 1])
		{
			int num5 = 1;
			int num6 = num + 1;
			while (num6 < p4 && p3[num6] == num2 && num5 < 6)
			{
				num6++;
				num5++;
			}
			if (num5 >= 3)
			{
				p0[p1++] = 16;
				p0[p1++] = num5 - 3;
				p2[16]++;
				num += num5 - 1;
				goto IL_0168;
			}
		}
		goto IL_0146;
		IL_0168:
		num++;
		goto IL_0174;
		IL_0174:
		if (num >= p4)
		{
			return;
		}
		goto IL_000c;
		IL_0146:
		p0[p1++] = num2;
		p2[num2]++;
		goto IL_0168;
	}

	private static void bfabj(byte[] p0, ref int p1, ref int p2, int p3, int p4)
	{
		p0[p1] |= (byte)(p3 << p2);
		p2 += p4;
		while (p2 > 8)
		{
			p2 -= 8;
			p1++;
			p0[p1] = (byte)(p3 >> p4 - p2);
		}
		if (p2 == 8)
		{
			p1++;
			p2 = 0;
		}
	}
}
