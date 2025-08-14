using System;

namespace onrkn;

internal static class gpkne
{
	internal class jtnhp
	{
		public static void qwsig(nxtme<byte> p0, uint p1)
		{
			jlfbq.mfljd(p0.lthjd, p0.frlfs, p1);
		}
	}

	public const int wsgqx = 1;

	public const int qnkgc = 16777215;

	public const int uxjai = 4;

	public const int zimsi = 4;

	public const int ecdoe = 3;

	public const int lfsgl = 2;

	public const int rrexf = 1024;

	public const int nceuo = 128;

	public const int zwpsg = 127;

	public const int swfbl = 1;

	public const uint bzogl = 19u;

	public const int xodpn = 64;

	public const int jkisa = 72;

	public const int ghziw = 0;

	public const int gkmfm = 1;

	public const int evlli = 2;

	public const int lohso = 3;

	public const int udvpv = 4;

	public const int iytjb = 5;

	public const int kanpb = 6;

	public const string geeai = "Argon2 is not supported on big endian platforms.";

	public const string usynr = "DegreeOfParallelim property must have a value between 1 and 16777215";

	public const string utjgk = "Argument 'outputHash' must have a length greater than 4.";

	public const string btwdl = "Iterations property must have a value greater than 0.";

	private const ulong trhpk = 4294967295uL;

	private const int qykgx = 2;

	private const int xrusx = 2;

	private const int vucrc = 3;

	private const int oioco = 1;

	public static readonly nxtme<byte> fltma;

	public static readonly nxtme<byte> placy;

	static gpkne()
	{
		if (!BitConverter.IsLittleEndian || 1 == 0)
		{
			throw new NotSupportedException("Argon2 is not supported on big endian platforms.");
		}
		placy = new byte[4];
		fltma = new byte[4];
		jtnhp.qwsig(placy.ldelr, 0u);
		jtnhp.qwsig(fltma.ldelr, 1u);
	}

	public static void zxiwe(nxtme<byte> p0, nxtme<byte> p1, nxtme<byte> p2, nxtme<byte> p3, rkpix p4, nxtme<byte> p5)
	{
		if (p4 == null || 1 == 0)
		{
			throw new ArgumentNullException("argon2Configuration");
		}
		if (p4.xoepc < 1 || p4.xoepc > 16777215)
		{
			throw new ArgumentOutOfRangeException("argon2Configuration", "DegreeOfParallelim property must have a value between 1 and 16777215");
		}
		if (p5.hvvsm < 4)
		{
			throw new ArgumentOutOfRangeException("outputHash", "Argument 'outputHash' must have a length greater than 4.");
		}
		if (p4.ngtpx < 1)
		{
			throw new ArgumentOutOfRangeException("argon2Configuration", "Iterations property must have a value greater than 0.");
		}
		int xoepc = p4.xoepc;
		int num = p4.tqhsq;
		int num2 = xoepc << 3;
		if (num < num2)
		{
			num = num2;
		}
		int num3 = xoepc << 2;
		int num4 = num / num3;
		int num5 = num4 << 2;
		int num6 = num5 * xoepc;
		int num7 = num5 >> 2;
		nxtme<ulong>[] array = sxztb<nxtme<ulong>>.ahblv.vfhlp(num6);
		nxtme<nxtme<ulong>> nxtme2 = array.myshu(0, num6);
		int num8 = 0;
		if (num8 != 0)
		{
			goto IL_00cc;
		}
		goto IL_010d;
		IL_00cc:
		nxtme2[num8] = sxztb<ulong>.ahblv.vfhlp(128).pynmq(0, 128);
		nxtme2[num8].ldelr.qkihq();
		num8++;
		goto IL_010d;
		IL_010d:
		if (num8 >= nxtme2.hvvsm)
		{
			byte[] array2 = sxztb<byte>.ahblv.vfhlp(4);
			nxtme<byte> nxtme3 = array2.plhfl(0, 4);
			byte[] array3 = sxztb<byte>.ahblv.vfhlp(72);
			nxtme<byte> p6 = array3.myshu(0, 72);
			bool flag = p4.tqhyl == ffooh.gjqsj || p4.tqhyl == ffooh.vefjh;
			ulong[] array4 = null;
			ulong[] array5 = null;
			ulong[] array6 = null;
			nxtme<ulong> nxtme4 = nxtme<ulong>.gihlo;
			nxtme<ulong> nxtme5 = nxtme<ulong>.gihlo;
			nxtme<ulong> p7 = nxtme<ulong>.gihlo;
			if (flag && 0 == 0)
			{
				array4 = sxztb<ulong>.ahblv.vfhlp(128);
				nxtme4 = array4.myshu(0, 128);
				array5 = sxztb<ulong>.ahblv.vfhlp(128);
				nxtme5 = array5.myshu(0, 128);
				array6 = sxztb<ulong>.ahblv.vfhlp(128);
				p7 = array6.myshu(0, 128);
				p7.qkihq();
			}
			luida.vczsd p8 = luida.gylwc(nxtme<byte>.gihlo, 64);
			try
			{
				jtnhp.qwsig(nxtme3, (uint)p4.xoepc);
				luida.toshr(ref p8, nxtme3);
				jtnhp.qwsig(nxtme3, (uint)p5.hvvsm);
				luida.toshr(ref p8, nxtme3);
				jtnhp.qwsig(nxtme3, (uint)p4.tqhsq);
				luida.toshr(ref p8, nxtme3);
				jtnhp.qwsig(nxtme3, (uint)p4.ngtpx);
				luida.toshr(ref p8, nxtme3);
				jtnhp.qwsig(nxtme3, 19u);
				luida.toshr(ref p8, nxtme3);
				jtnhp.qwsig(nxtme3, (uint)p4.tqhyl);
				luida.toshr(ref p8, nxtme3);
				jtnhp.qwsig(nxtme3, (uint)p0.hvvsm);
				luida.toshr(ref p8, nxtme3);
				luida.toshr(ref p8, p0);
				jtnhp.qwsig(nxtme3, (uint)p2.hvvsm);
				luida.toshr(ref p8, nxtme3);
				luida.toshr(ref p8, p2);
				jtnhp.qwsig(nxtme3, (uint)p1.hvvsm);
				luida.toshr(ref p8, nxtme3);
				luida.toshr(ref p8, p1);
				jtnhp.qwsig(nxtme3, (uint)p3.hvvsm);
				luida.toshr(ref p8, nxtme3);
				luida.toshr(ref p8, p3);
				luida.mdjru(ref p8, p6.jlxhy(0, 64));
				placy.ldelr.rjwrl(p6.xjycg(64));
				nxtme<byte> p9 = p6.xjycg(68);
				int num9 = 0;
				if (num9 != 0)
				{
					goto IL_0356;
				}
				goto IL_03cb;
				IL_0356:
				jtnhp.qwsig(p9, (uint)num9);
				nxtme<ulong> p10 = nxtme2[num9 * num5];
				otspa otspa2 = peekn.ahheu(p10, p1: true);
				try
				{
					xkgis(p6, otspa2.mjnob.myshu(0, 1024));
				}
				finally
				{
					if (otspa2.ynunp && 0 == 0)
					{
						Buffer.BlockCopy(otspa2.mjnob, 0, p10.lthjd, 0, 1024);
					}
					otspa2.ankrk();
				}
				num9++;
				goto IL_03cb;
				IL_06a4:
				int num10;
				if (num10 < 4)
				{
					goto IL_048a;
				}
				int num11 = num11 + 1;
				goto IL_06b2;
				IL_03cb:
				if (num9 < xoepc)
				{
					goto IL_0356;
				}
				fltma.ldelr.rjwrl(p6.xjycg(64));
				int num12 = 0;
				if (num12 != 0)
				{
					goto IL_03fa;
				}
				goto IL_0471;
				IL_06fe:
				ulong[] lthjd;
				int num13;
				ulong[] lthjd2;
				lthjd[num13] ^= lthjd2[num13++];
				lthjd[num13] ^= lthjd2[num13++];
				lthjd[num13] ^= lthjd2[num13++];
				lthjd[num13] ^= lthjd2[num13++];
				lthjd[num13] ^= lthjd2[num13++];
				lthjd[num13] ^= lthjd2[num13++];
				lthjd[num13] ^= lthjd2[num13++];
				lthjd[num13] ^= lthjd2[num13++];
				lthjd[num13] ^= lthjd2[num13++];
				lthjd[num13] ^= lthjd2[num13++];
				lthjd[num13] ^= lthjd2[num13++];
				lthjd[num13] ^= lthjd2[num13++];
				lthjd[num13] ^= lthjd2[num13++];
				lthjd[num13] ^= lthjd2[num13++];
				lthjd[num13] ^= lthjd2[num13++];
				lthjd[num13] ^= lthjd2[num13++];
				goto IL_08ee;
				IL_03fa:
				jtnhp.qwsig(p9, (uint)num12);
				nxtme<ulong> p11 = nxtme2[num12 * num5 + 1];
				otspa otspa3 = peekn.ahheu(p11, p1: true);
				try
				{
					xkgis(p6, otspa3.mjnob.myshu(0, 1024));
				}
				finally
				{
					if (otspa3.ynunp && 0 == 0)
					{
						Buffer.BlockCopy(otspa3.mjnob, 0, p11.lthjd, 0, 1024);
					}
					otspa3.ankrk();
				}
				num12++;
				goto IL_0471;
				IL_0900:
				int num14;
				if (num14 >= xoepc)
				{
					otspa otspa4 = peekn.ahheu(lthjd.plhfl(0, 128), p1: true);
					try
					{
						xkgis(otspa4.mjnob.plhfl(0, 1024), p5);
						return;
					}
					finally
					{
						otspa4.ankrk();
					}
				}
				goto IL_06dc;
				IL_0471:
				if (num12 < xoepc)
				{
					goto IL_03fa;
				}
				num11 = 0;
				if (num11 != 0)
				{
					goto IL_0480;
				}
				goto IL_06b2;
				IL_06dc:
				lthjd2 = array[num14 * num5 + (num5 - 1)].lthjd;
				num13 = 0;
				if (num13 != 0)
				{
					goto IL_06fe;
				}
				goto IL_08ee;
				IL_0480:
				num10 = 0;
				if (num10 != 0)
				{
					goto IL_048a;
				}
				goto IL_06a4;
				IL_048a:
				int num15 = 0;
				if (num15 != 0)
				{
					goto IL_0494;
				}
				goto IL_0696;
				IL_0494:
				bool flag2 = p4.tqhyl == ffooh.gjqsj || (p4.tqhyl == ffooh.vefjh && (num11 == 0 || 1 == 0) && num10 < 2);
				if (flag2 && 0 == 0)
				{
					nxtme4.qkihq();
					nxtme5.qkihq();
					nxtme5[0] = (ulong)num11;
					nxtme5[1] = (ulong)num15;
					nxtme5[2] = (ulong)num10;
					nxtme5[3] = (ulong)num6;
					nxtme5[4] = (ulong)p4.ngtpx;
					nxtme5[5] = (ulong)p4.tqhyl;
				}
				int num16 = 0;
				if ((num11 == 0 || 1 == 0) && (num10 == 0 || 1 == 0))
				{
					num16 = 2;
					if (flag && 0 == 0)
					{
						nxtme5[6]++;
						oqfsn(p7, nxtme5, nxtme4);
						oqfsn(p7, nxtme4, nxtme4);
					}
				}
				int num17 = num15 * num5 + num10 * num4 + num16;
				int num18 = num17 - 1;
				int num19 = num16;
				while (num19 < num7)
				{
					if ((num19 == 0 || 1 == 0) && (num10 == 0 || 1 == 0))
					{
						num18 += num5;
					}
					ulong p12;
					if (flag2 && 0 == 0)
					{
						int num20 = num19 & 0x7F;
						if (num20 == 0 || 1 == 0)
						{
							nxtme5[6]++;
							oqfsn(p7, nxtme5, nxtme4);
							oqfsn(p7, nxtme4, nxtme4);
						}
						p12 = nxtme4[num20];
					}
					else
					{
						p12 = nxtme2[num18].ldelr[0];
					}
					int num21 = uzjwg(p12, num11, num10, num15, num19, num7, num5, xoepc);
					bnvgg(array[num21].ldelr, array[num18].ldelr, array[num17].ldelr);
					num19++;
					num18 = num17++;
				}
				num15++;
				goto IL_0696;
				IL_06b2:
				if (num11 < p4.ngtpx)
				{
					goto IL_0480;
				}
				lthjd = array[num5 - 1].lthjd;
				num14 = 1;
				if (num14 == 0)
				{
					goto IL_06dc;
				}
				goto IL_0900;
				IL_08ee:
				if (num13 < 128)
				{
					goto IL_06fe;
				}
				num14++;
				goto IL_0900;
				IL_0696:
				if (num15 < xoepc)
				{
					goto IL_0494;
				}
				num10++;
				goto IL_06a4;
			}
			finally
			{
				p8.ksapj();
				Array.Clear(array3, 0, array3.Length);
				sxztb<byte>.ahblv.uqydw(array3);
				using (nxtme<nxtme<ulong>>.rrjio rrjio = nxtme2.twvtt())
				{
					while (rrjio.MoveNext() ? true : false)
					{
						nxtme<ulong> current = rrjio.Current;
						Array.Clear(current.lthjd, 0, current.lthjd.Length);
						sxztb<ulong>.ahblv.uqydw(current.lthjd);
					}
				}
				Array.Clear(array, 0, array.Length);
				sxztb<nxtme<ulong>>.ahblv.uqydw(array);
				Array.Clear(array2, 0, array2.Length);
				sxztb<byte>.ahblv.uqydw(array2);
				if (flag && 0 == 0)
				{
					Array.Clear(array4, 0, array4.Length);
					sxztb<ulong>.ahblv.uqydw(array4);
					Array.Clear(array5, 0, array5.Length);
					sxztb<ulong>.ahblv.uqydw(array5);
					Array.Clear(array6, 0, array6.Length);
					sxztb<ulong>.ahblv.uqydw(array6);
				}
			}
		}
		goto IL_00cc;
	}

	private static int uzjwg(ulong p0, int p1, int p2, int p3, int p4, int p5, int p6, int p7)
	{
		uint num = (uint)(p0 >> 32);
		bool flag = false;
		bool flag2 = p1 == 0;
		int num2 = (int)(num % p7);
		if (p3 == num2)
		{
			flag = true;
		}
		if (flag2 && 0 == 0 && (p2 == 0 || 1 == 0))
		{
			num2 = p3;
			flag = true;
		}
		int num3 = ((flag2 ? true : false) ? p2 : 3);
		int num4 = num3 * p5;
		int num5 = ((!flag2 || 1 == 0) ? (((p2 + 1) & 3) * p5) : 0);
		if ((flag ? true : false) || (flag2 && 0 == 0 && (p2 == 0 || 1 == 0)))
		{
			num4 += p4;
		}
		if ((flag ? true : false) || p4 == 0 || 1 == 0)
		{
			num4--;
		}
		ulong num6 = p0 & 0xFFFFFFFFu;
		num6 *= num6;
		num6 >>= 32;
		num6 *= (ulong)num4;
		num6 >>= 32;
		num6++;
		int num7 = num2 * p6;
		ulong num8 = (ulong)((long)num5 + (long)num4) - num6;
		return num7 + (int)(num8 % (ulong)p6);
	}

	private static void oqfsn(nxtme<ulong> p0, nxtme<ulong> p1, nxtme<ulong> p2)
	{
		ulong[] lthjd = p0.lthjd;
		ulong[] lthjd2 = p1.lthjd;
		ulong[] lthjd3 = p2.lthjd;
		ulong[] array = sxztb<ulong>.ahblv.vfhlp(128);
		ulong[] array2 = array;
		try
		{
			int num;
			if ((uint)lthjd.Length > 127u && (uint)lthjd2.Length > 127u && (uint)lthjd3.Length > 127u)
			{
				num = 0;
				if (num != 0)
				{
					goto IL_0059;
				}
				goto IL_0219;
			}
			return;
			IL_0219:
			if (num < 128)
			{
				goto IL_0059;
			}
			dvwbi(array2);
			int num2 = 0;
			if (num2 != 0)
			{
				goto IL_0236;
			}
			goto IL_0416;
			IL_0416:
			if (num2 >= 128)
			{
				return;
			}
			goto IL_0236;
			IL_0236:
			lthjd3[num2] ^= array2[num2++];
			lthjd3[num2] ^= array2[num2++];
			lthjd3[num2] ^= array2[num2++];
			lthjd3[num2] ^= array2[num2++];
			lthjd3[num2] ^= array2[num2++];
			lthjd3[num2] ^= array2[num2++];
			lthjd3[num2] ^= array2[num2++];
			lthjd3[num2] ^= array2[num2++];
			lthjd3[num2] ^= array2[num2++];
			lthjd3[num2] ^= array2[num2++];
			lthjd3[num2] ^= array2[num2++];
			lthjd3[num2] ^= array2[num2++];
			lthjd3[num2] ^= array2[num2++];
			lthjd3[num2] ^= array2[num2++];
			lthjd3[num2] ^= array2[num2++];
			lthjd3[num2] ^= array2[num2++];
			goto IL_0416;
			IL_0059:
			array2[num] = lthjd[num] ^ lthjd2[num];
			lthjd3[num] = array2[num++];
			array2[num] = lthjd[num] ^ lthjd2[num];
			lthjd3[num] = array2[num++];
			array2[num] = lthjd[num] ^ lthjd2[num];
			lthjd3[num] = array2[num++];
			array2[num] = lthjd[num] ^ lthjd2[num];
			lthjd3[num] = array2[num++];
			array2[num] = lthjd[num] ^ lthjd2[num];
			lthjd3[num] = array2[num++];
			array2[num] = lthjd[num] ^ lthjd2[num];
			lthjd3[num] = array2[num++];
			array2[num] = lthjd[num] ^ lthjd2[num];
			lthjd3[num] = array2[num++];
			array2[num] = lthjd[num] ^ lthjd2[num];
			lthjd3[num] = array2[num++];
			array2[num] = lthjd[num] ^ lthjd2[num];
			lthjd3[num] = array2[num++];
			array2[num] = lthjd[num] ^ lthjd2[num];
			lthjd3[num] = array2[num++];
			array2[num] = lthjd[num] ^ lthjd2[num];
			lthjd3[num] = array2[num++];
			array2[num] = lthjd[num] ^ lthjd2[num];
			lthjd3[num] = array2[num++];
			array2[num] = lthjd[num] ^ lthjd2[num];
			lthjd3[num] = array2[num++];
			array2[num] = lthjd[num] ^ lthjd2[num];
			lthjd3[num] = array2[num++];
			array2[num] = lthjd[num] ^ lthjd2[num];
			lthjd3[num] = array2[num++];
			array2[num] = lthjd[num] ^ lthjd2[num];
			lthjd3[num] = array2[num++];
			goto IL_0219;
		}
		finally
		{
			Array.Clear(array, 0, array.Length);
			sxztb<ulong>.ahblv.uqydw(array);
		}
	}

	private static void bnvgg(nxtme<ulong> p0, nxtme<ulong> p1, nxtme<ulong> p2)
	{
		ulong[] lthjd = p0.lthjd;
		ulong[] lthjd2 = p1.lthjd;
		ulong[] lthjd3 = p2.lthjd;
		ulong[] array = sxztb<ulong>.ahblv.vfhlp(128);
		ulong[] array2 = array;
		try
		{
			int num;
			if ((uint)lthjd.Length > 127u && (uint)lthjd2.Length > 127u && (uint)lthjd3.Length > 127u)
			{
				num = 0;
				if (num != 0)
				{
					goto IL_0059;
				}
				goto IL_0319;
			}
			return;
			IL_0319:
			if (num < 128)
			{
				goto IL_0059;
			}
			dvwbi(array2);
			int num2 = 0;
			if (num2 != 0)
			{
				goto IL_0336;
			}
			goto IL_0516;
			IL_0516:
			if (num2 >= 128)
			{
				return;
			}
			goto IL_0336;
			IL_0336:
			lthjd3[num2] ^= array2[num2++];
			lthjd3[num2] ^= array2[num2++];
			lthjd3[num2] ^= array2[num2++];
			lthjd3[num2] ^= array2[num2++];
			lthjd3[num2] ^= array2[num2++];
			lthjd3[num2] ^= array2[num2++];
			lthjd3[num2] ^= array2[num2++];
			lthjd3[num2] ^= array2[num2++];
			lthjd3[num2] ^= array2[num2++];
			lthjd3[num2] ^= array2[num2++];
			lthjd3[num2] ^= array2[num2++];
			lthjd3[num2] ^= array2[num2++];
			lthjd3[num2] ^= array2[num2++];
			lthjd3[num2] ^= array2[num2++];
			lthjd3[num2] ^= array2[num2++];
			lthjd3[num2] ^= array2[num2++];
			goto IL_0516;
			IL_0059:
			array2[num] = lthjd[num] ^ lthjd2[num];
			lthjd3[num] ^= array2[num++];
			array2[num] = lthjd[num] ^ lthjd2[num];
			lthjd3[num] ^= array2[num++];
			array2[num] = lthjd[num] ^ lthjd2[num];
			lthjd3[num] ^= array2[num++];
			array2[num] = lthjd[num] ^ lthjd2[num];
			lthjd3[num] ^= array2[num++];
			array2[num] = lthjd[num] ^ lthjd2[num];
			lthjd3[num] ^= array2[num++];
			array2[num] = lthjd[num] ^ lthjd2[num];
			lthjd3[num] ^= array2[num++];
			array2[num] = lthjd[num] ^ lthjd2[num];
			lthjd3[num] ^= array2[num++];
			array2[num] = lthjd[num] ^ lthjd2[num];
			lthjd3[num] ^= array2[num++];
			array2[num] = lthjd[num] ^ lthjd2[num];
			lthjd3[num] ^= array2[num++];
			array2[num] = lthjd[num] ^ lthjd2[num];
			lthjd3[num] ^= array2[num++];
			array2[num] = lthjd[num] ^ lthjd2[num];
			lthjd3[num] ^= array2[num++];
			array2[num] = lthjd[num] ^ lthjd2[num];
			lthjd3[num] ^= array2[num++];
			array2[num] = lthjd[num] ^ lthjd2[num];
			lthjd3[num] ^= array2[num++];
			array2[num] = lthjd[num] ^ lthjd2[num];
			lthjd3[num] ^= array2[num++];
			array2[num] = lthjd[num] ^ lthjd2[num];
			lthjd3[num] ^= array2[num++];
			array2[num] = lthjd[num] ^ lthjd2[num];
			lthjd3[num] ^= array2[num++];
			goto IL_0319;
		}
		finally
		{
			Array.Clear(array, 0, array.Length);
			sxztb<ulong>.ahblv.uqydw(array);
		}
	}

	private static void dvwbi(ulong[] p0)
	{
		int num;
		if ((uint)p0.Length > 127u)
		{
			num = 0;
			if (num != 0)
			{
				goto IL_0012;
			}
			goto IL_05d7;
		}
		return;
		IL_05ec:
		int num3;
		ulong num2 = p0[num3 + 113];
		ulong num4 = p0[num3];
		ulong num5 = p0[num3 + 1];
		ulong num6 = p0[num3 + 16];
		ulong num7 = p0[num3 + 17];
		ulong num8 = p0[num3 + 32];
		ulong num9 = p0[num3 + 33];
		ulong num10 = p0[num3 + 48];
		ulong num11 = p0[num3 + 49];
		ulong num12 = p0[num3 + 64];
		ulong num13 = p0[num3 + 65];
		ulong num14 = p0[num3 + 80];
		ulong num15 = p0[num3 + 81];
		ulong num16 = p0[num3 + 96];
		ulong num17 = p0[num3 + 97];
		ulong num18 = p0[num3 + 112];
		num4 += num8 + ((num4 & 0xFFFFFFFFu) * (num8 & 0xFFFFFFFFu) << 1);
		num16 ^= num4;
		num16 = (num16 >> 32) ^ (num16 << 32);
		num12 += num16 + ((num12 & 0xFFFFFFFFu) * (num16 & 0xFFFFFFFFu) << 1);
		num8 ^= num12;
		num8 = (num8 >> 24) ^ (num8 << 40);
		num4 += num8 + ((num4 & 0xFFFFFFFFu) * (num8 & 0xFFFFFFFFu) << 1);
		num16 ^= num4;
		num16 = (num16 >> 16) ^ (num16 << 48);
		num12 += num16 + ((num12 & 0xFFFFFFFFu) * (num16 & 0xFFFFFFFFu) << 1);
		num8 ^= num12;
		num8 = (num8 >> 63) ^ (num8 << 1);
		num5 += num9 + ((num5 & 0xFFFFFFFFu) * (num9 & 0xFFFFFFFFu) << 1);
		num17 ^= num5;
		num17 = (num17 >> 32) ^ (num17 << 32);
		num13 += num17 + ((num13 & 0xFFFFFFFFu) * (num17 & 0xFFFFFFFFu) << 1);
		num9 ^= num13;
		num9 = (num9 >> 24) ^ (num9 << 40);
		num5 += num9 + ((num5 & 0xFFFFFFFFu) * (num9 & 0xFFFFFFFFu) << 1);
		num17 ^= num5;
		num17 = (num17 >> 16) ^ (num17 << 48);
		num13 += num17 + ((num13 & 0xFFFFFFFFu) * (num17 & 0xFFFFFFFFu) << 1);
		num9 ^= num13;
		num9 = (num9 >> 63) ^ (num9 << 1);
		num6 += num10 + ((num6 & 0xFFFFFFFFu) * (num10 & 0xFFFFFFFFu) << 1);
		num18 ^= num6;
		num18 = (num18 >> 32) ^ (num18 << 32);
		num14 += num18 + ((num14 & 0xFFFFFFFFu) * (num18 & 0xFFFFFFFFu) << 1);
		num10 ^= num14;
		num10 = (num10 >> 24) ^ (num10 << 40);
		num6 += num10 + ((num6 & 0xFFFFFFFFu) * (num10 & 0xFFFFFFFFu) << 1);
		num18 ^= num6;
		num18 = (num18 >> 16) ^ (num18 << 48);
		num14 += num18 + ((num14 & 0xFFFFFFFFu) * (num18 & 0xFFFFFFFFu) << 1);
		num10 ^= num14;
		num10 = (num10 >> 63) ^ (num10 << 1);
		num7 += num11 + ((num7 & 0xFFFFFFFFu) * (num11 & 0xFFFFFFFFu) << 1);
		num2 ^= num7;
		num2 = (num2 >> 32) ^ (num2 << 32);
		num15 += num2 + ((num15 & 0xFFFFFFFFu) * (num2 & 0xFFFFFFFFu) << 1);
		num11 ^= num15;
		num11 = (num11 >> 24) ^ (num11 << 40);
		num7 += num11 + ((num7 & 0xFFFFFFFFu) * (num11 & 0xFFFFFFFFu) << 1);
		num2 ^= num7;
		num2 = (num2 >> 16) ^ (num2 << 48);
		num15 += num2 + ((num15 & 0xFFFFFFFFu) * (num2 & 0xFFFFFFFFu) << 1);
		num11 ^= num15;
		num11 = (num11 >> 63) ^ (num11 << 1);
		num4 += num9 + ((num4 & 0xFFFFFFFFu) * (num9 & 0xFFFFFFFFu) << 1);
		num2 ^= num4;
		num2 = (num2 >> 32) ^ (num2 << 32);
		num14 += num2 + ((num14 & 0xFFFFFFFFu) * (num2 & 0xFFFFFFFFu) << 1);
		num9 ^= num14;
		num9 = (num9 >> 24) ^ (num9 << 40);
		num4 += num9 + ((num4 & 0xFFFFFFFFu) * (num9 & 0xFFFFFFFFu) << 1);
		num2 ^= num4;
		num2 = (num2 >> 16) ^ (num2 << 48);
		num14 += num2 + ((num14 & 0xFFFFFFFFu) * (num2 & 0xFFFFFFFFu) << 1);
		num9 ^= num14;
		num9 = (num9 >> 63) ^ (num9 << 1);
		num5 += num10 + ((num5 & 0xFFFFFFFFu) * (num10 & 0xFFFFFFFFu) << 1);
		num16 ^= num5;
		num16 = (num16 >> 32) ^ (num16 << 32);
		num15 += num16 + ((num15 & 0xFFFFFFFFu) * (num16 & 0xFFFFFFFFu) << 1);
		num10 ^= num15;
		num10 = (num10 >> 24) ^ (num10 << 40);
		num5 += num10 + ((num5 & 0xFFFFFFFFu) * (num10 & 0xFFFFFFFFu) << 1);
		num16 ^= num5;
		num16 = (num16 >> 16) ^ (num16 << 48);
		num15 += num16 + ((num15 & 0xFFFFFFFFu) * (num16 & 0xFFFFFFFFu) << 1);
		num10 ^= num15;
		num10 = (num10 >> 63) ^ (num10 << 1);
		num6 += num11 + ((num6 & 0xFFFFFFFFu) * (num11 & 0xFFFFFFFFu) << 1);
		num17 ^= num6;
		num17 = (num17 >> 32) ^ (num17 << 32);
		num12 += num17 + ((num12 & 0xFFFFFFFFu) * (num17 & 0xFFFFFFFFu) << 1);
		num11 ^= num12;
		num11 = (num11 >> 24) ^ (num11 << 40);
		num6 += num11 + ((num6 & 0xFFFFFFFFu) * (num11 & 0xFFFFFFFFu) << 1);
		num17 ^= num6;
		num17 = (num17 >> 16) ^ (num17 << 48);
		num12 += num17 + ((num12 & 0xFFFFFFFFu) * (num17 & 0xFFFFFFFFu) << 1);
		num11 ^= num12;
		num11 = (num11 >> 63) ^ (num11 << 1);
		num7 += num8 + ((num7 & 0xFFFFFFFFu) * (num8 & 0xFFFFFFFFu) << 1);
		num18 ^= num7;
		num18 = (num18 >> 32) ^ (num18 << 32);
		num13 += num18 + ((num13 & 0xFFFFFFFFu) * (num18 & 0xFFFFFFFFu) << 1);
		num8 ^= num13;
		num8 = (num8 >> 24) ^ (num8 << 40);
		num7 += num8 + ((num7 & 0xFFFFFFFFu) * (num8 & 0xFFFFFFFFu) << 1);
		num18 ^= num7;
		num18 = (num18 >> 16) ^ (num18 << 48);
		num13 += num18 + ((num13 & 0xFFFFFFFFu) * (num18 & 0xFFFFFFFFu) << 1);
		num8 ^= num13;
		num8 = (num8 >> 63) ^ (num8 << 1);
		p0[num3] = num4;
		p0[num3 + 1] = num5;
		p0[num3 + 16] = num6;
		p0[num3 + 17] = num7;
		p0[num3 + 32] = num8;
		p0[num3 + 33] = num9;
		p0[num3 + 48] = num10;
		p0[num3 + 49] = num11;
		p0[num3 + 64] = num12;
		p0[num3 + 65] = num13;
		p0[num3 + 80] = num14;
		p0[num3 + 81] = num15;
		p0[num3 + 96] = num16;
		p0[num3 + 97] = num17;
		p0[num3 + 112] = num18;
		p0[num3 + 113] = num2;
		num3 += 2;
		goto IL_0c2b;
		IL_0012:
		ulong num19 = p0[num + 15];
		ulong num20 = p0[num];
		ulong num21 = p0[num + 1];
		ulong num22 = p0[num + 2];
		ulong num23 = p0[num + 3];
		ulong num24 = p0[num + 4];
		ulong num25 = p0[num + 5];
		ulong num26 = p0[num + 6];
		ulong num27 = p0[num + 7];
		ulong num28 = p0[num + 8];
		ulong num29 = p0[num + 9];
		ulong num30 = p0[num + 10];
		ulong num31 = p0[num + 11];
		ulong num32 = p0[num + 12];
		ulong num33 = p0[num + 13];
		ulong num34 = p0[num + 14];
		num20 += num24 + ((num20 & 0xFFFFFFFFu) * (num24 & 0xFFFFFFFFu) << 1);
		num32 ^= num20;
		num32 = (num32 >> 32) ^ (num32 << 32);
		num28 += num32 + ((num28 & 0xFFFFFFFFu) * (num32 & 0xFFFFFFFFu) << 1);
		num24 ^= num28;
		num24 = (num24 >> 24) ^ (num24 << 40);
		num20 += num24 + ((num20 & 0xFFFFFFFFu) * (num24 & 0xFFFFFFFFu) << 1);
		num32 ^= num20;
		num32 = (num32 >> 16) ^ (num32 << 48);
		num28 += num32 + ((num28 & 0xFFFFFFFFu) * (num32 & 0xFFFFFFFFu) << 1);
		num24 ^= num28;
		num24 = (num24 >> 63) ^ (num24 << 1);
		num21 += num25 + ((num21 & 0xFFFFFFFFu) * (num25 & 0xFFFFFFFFu) << 1);
		num33 ^= num21;
		num33 = (num33 >> 32) ^ (num33 << 32);
		num29 += num33 + ((num29 & 0xFFFFFFFFu) * (num33 & 0xFFFFFFFFu) << 1);
		num25 ^= num29;
		num25 = (num25 >> 24) ^ (num25 << 40);
		num21 += num25 + ((num21 & 0xFFFFFFFFu) * (num25 & 0xFFFFFFFFu) << 1);
		num33 ^= num21;
		num33 = (num33 >> 16) ^ (num33 << 48);
		num29 += num33 + ((num29 & 0xFFFFFFFFu) * (num33 & 0xFFFFFFFFu) << 1);
		num25 ^= num29;
		num25 = (num25 >> 63) ^ (num25 << 1);
		num22 += num26 + ((num22 & 0xFFFFFFFFu) * (num26 & 0xFFFFFFFFu) << 1);
		num34 ^= num22;
		num34 = (num34 >> 32) ^ (num34 << 32);
		num30 += num34 + ((num30 & 0xFFFFFFFFu) * (num34 & 0xFFFFFFFFu) << 1);
		num26 ^= num30;
		num26 = (num26 >> 24) ^ (num26 << 40);
		num22 += num26 + ((num22 & 0xFFFFFFFFu) * (num26 & 0xFFFFFFFFu) << 1);
		num34 ^= num22;
		num34 = (num34 >> 16) ^ (num34 << 48);
		num30 += num34 + ((num30 & 0xFFFFFFFFu) * (num34 & 0xFFFFFFFFu) << 1);
		num26 ^= num30;
		num26 = (num26 >> 63) ^ (num26 << 1);
		num23 += num27 + ((num23 & 0xFFFFFFFFu) * (num27 & 0xFFFFFFFFu) << 1);
		num19 ^= num23;
		num19 = (num19 >> 32) ^ (num19 << 32);
		num31 += num19 + ((num31 & 0xFFFFFFFFu) * (num19 & 0xFFFFFFFFu) << 1);
		num27 ^= num31;
		num27 = (num27 >> 24) ^ (num27 << 40);
		num23 += num27 + ((num23 & 0xFFFFFFFFu) * (num27 & 0xFFFFFFFFu) << 1);
		num19 ^= num23;
		num19 = (num19 >> 16) ^ (num19 << 48);
		num31 += num19 + ((num31 & 0xFFFFFFFFu) * (num19 & 0xFFFFFFFFu) << 1);
		num27 ^= num31;
		num27 = (num27 >> 63) ^ (num27 << 1);
		num20 += num25 + ((num20 & 0xFFFFFFFFu) * (num25 & 0xFFFFFFFFu) << 1);
		num19 ^= num20;
		num19 = (num19 >> 32) ^ (num19 << 32);
		num30 += num19 + ((num30 & 0xFFFFFFFFu) * (num19 & 0xFFFFFFFFu) << 1);
		num25 ^= num30;
		num25 = (num25 >> 24) ^ (num25 << 40);
		num20 += num25 + ((num20 & 0xFFFFFFFFu) * (num25 & 0xFFFFFFFFu) << 1);
		num19 ^= num20;
		num19 = (num19 >> 16) ^ (num19 << 48);
		num30 += num19 + ((num30 & 0xFFFFFFFFu) * (num19 & 0xFFFFFFFFu) << 1);
		num25 ^= num30;
		num25 = (num25 >> 63) ^ (num25 << 1);
		num21 += num26 + ((num21 & 0xFFFFFFFFu) * (num26 & 0xFFFFFFFFu) << 1);
		num32 ^= num21;
		num32 = (num32 >> 32) ^ (num32 << 32);
		num31 += num32 + ((num31 & 0xFFFFFFFFu) * (num32 & 0xFFFFFFFFu) << 1);
		num26 ^= num31;
		num26 = (num26 >> 24) ^ (num26 << 40);
		num21 += num26 + ((num21 & 0xFFFFFFFFu) * (num26 & 0xFFFFFFFFu) << 1);
		num32 ^= num21;
		num32 = (num32 >> 16) ^ (num32 << 48);
		num31 += num32 + ((num31 & 0xFFFFFFFFu) * (num32 & 0xFFFFFFFFu) << 1);
		num26 ^= num31;
		num26 = (num26 >> 63) ^ (num26 << 1);
		num22 += num27 + ((num22 & 0xFFFFFFFFu) * (num27 & 0xFFFFFFFFu) << 1);
		num33 ^= num22;
		num33 = (num33 >> 32) ^ (num33 << 32);
		num28 += num33 + ((num28 & 0xFFFFFFFFu) * (num33 & 0xFFFFFFFFu) << 1);
		num27 ^= num28;
		num27 = (num27 >> 24) ^ (num27 << 40);
		num22 += num27 + ((num22 & 0xFFFFFFFFu) * (num27 & 0xFFFFFFFFu) << 1);
		num33 ^= num22;
		num33 = (num33 >> 16) ^ (num33 << 48);
		num28 += num33 + ((num28 & 0xFFFFFFFFu) * (num33 & 0xFFFFFFFFu) << 1);
		num27 ^= num28;
		num27 = (num27 >> 63) ^ (num27 << 1);
		num23 += num24 + ((num23 & 0xFFFFFFFFu) * (num24 & 0xFFFFFFFFu) << 1);
		num34 ^= num23;
		num34 = (num34 >> 32) ^ (num34 << 32);
		num29 += num34 + ((num29 & 0xFFFFFFFFu) * (num34 & 0xFFFFFFFFu) << 1);
		num24 ^= num29;
		num24 = (num24 >> 24) ^ (num24 << 40);
		num23 += num24 + ((num23 & 0xFFFFFFFFu) * (num24 & 0xFFFFFFFFu) << 1);
		num34 ^= num23;
		num34 = (num34 >> 16) ^ (num34 << 48);
		num29 += num34 + ((num29 & 0xFFFFFFFFu) * (num34 & 0xFFFFFFFFu) << 1);
		num24 ^= num29;
		num24 = (num24 >> 63) ^ (num24 << 1);
		p0[num] = num20;
		p0[num + 1] = num21;
		p0[num + 2] = num22;
		p0[num + 3] = num23;
		p0[num + 4] = num24;
		p0[num + 5] = num25;
		p0[num + 6] = num26;
		p0[num + 7] = num27;
		p0[num + 8] = num28;
		p0[num + 9] = num29;
		p0[num + 10] = num30;
		p0[num + 11] = num31;
		p0[num + 12] = num32;
		p0[num + 13] = num33;
		p0[num + 14] = num34;
		p0[num + 15] = num19;
		num += 16;
		goto IL_05d7;
		IL_05d7:
		if (num < 128)
		{
			goto IL_0012;
		}
		num3 = 0;
		if (num3 != 0)
		{
			goto IL_05ec;
		}
		goto IL_0c2b;
		IL_0c2b:
		if (num3 >= 16)
		{
			return;
		}
		goto IL_05ec;
	}

	private static void xkgis(nxtme<byte> p0, nxtme<byte> p1)
	{
		byte[] array = sxztb<byte>.ahblv.vfhlp(4);
		nxtme<byte> nxtme2 = array.myshu(0, 4);
		jtnhp.qwsig(nxtme2, (uint)p1.hvvsm);
		if (p1.hvvsm <= 64)
		{
			luida.vczsd p2 = luida.gylwc(nxtme<byte>.gihlo, p1.hvvsm);
			try
			{
				luida.toshr(ref p2, nxtme2);
				luida.toshr(ref p2, p0);
				luida.mdjru(ref p2, p1);
				return;
			}
			finally
			{
				Array.Clear(array, 0, array.Length);
				sxztb<byte>.ahblv.uqydw(array);
				p2.ksapj();
			}
		}
		int num = (p1.hvvsm + 31 >> 5) - 2;
		nxtme<byte> nxtme3 = p1.jlxhy(0, 64);
		nxtme<byte> nxtme4 = p0;
		luida.vczsd p3 = luida.gylwc(nxtme<byte>.gihlo, 64);
		try
		{
			luida.toshr(ref p3, nxtme2);
			luida.toshr(ref p3, nxtme4);
			luida.mdjru(ref p3, nxtme3);
			nxtme4 = nxtme3;
			int p4 = Math.Min(64, p1.hvvsm - 32);
			nxtme3 = p1.jlxhy(32, p4);
			int num2 = 1;
			int num3 = 64;
			if (num3 == 0)
			{
				goto IL_00fa;
			}
			goto IL_0139;
			IL_00fa:
			luida.feopy(nxtme4, nxtme<byte>.gihlo, nxtme3);
			nxtme4 = nxtme3;
			int p5 = Math.Min(64, p1.hvvsm - num3);
			nxtme3 = p1.jlxhy(num3, p5);
			num2++;
			num3 += 32;
			goto IL_0139;
			IL_0139:
			if (num2 < num)
			{
				goto IL_00fa;
			}
		}
		finally
		{
			Array.Clear(array, 0, array.Length);
			sxztb<byte>.ahblv.uqydw(array);
			p3.ksapj();
		}
		luida.feopy(nxtme4, nxtme<byte>.gihlo, nxtme3);
	}
}
