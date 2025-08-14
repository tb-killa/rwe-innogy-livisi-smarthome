using System;
using System.Runtime.InteropServices;

namespace onrkn;

internal class qtllz : vifpo
{
	[StructLayout(LayoutKind.Explicit, Size = 64)]
	private struct znibc
	{
		[FieldOffset(0)]
		public pclma ugtxf;

		[FieldOffset(4)]
		public pclma qandd;

		[FieldOffset(8)]
		public pclma zkiog;

		[FieldOffset(12)]
		public pclma lvvmd;

		[FieldOffset(16)]
		public pclma zzpup;

		[FieldOffset(20)]
		public pclma nfudg;

		[FieldOffset(24)]
		public pclma wefnl;

		[FieldOffset(28)]
		public pclma sqygu;

		[FieldOffset(32)]
		public pclma liyag;

		[FieldOffset(36)]
		public pclma xsckh;

		[FieldOffset(40)]
		public pclma illor;

		[FieldOffset(44)]
		public pclma zgtws;

		[FieldOffset(48)]
		public pclma kxffx;

		[FieldOffset(52)]
		public pclma mvjsy;

		[FieldOffset(56)]
		public pclma ikxna;

		[FieldOffset(60)]
		public pclma wvycw;

		[FieldOffset(0)]
		public hhqyy xrsab;

		public znibc(uint state0, uint state1, uint state2, uint state3, uint state4, uint state5, uint state6, uint state7, uint state8, uint state9, uint state10, uint state11, uint state12, uint state13, uint state14, uint state15)
		{
			this = default(znibc);
			ugtxf.lwlly(state0);
			qandd.lwlly(state1);
			zkiog.lwlly(state2);
			lvvmd.lwlly(state3);
			zzpup.lwlly(state4);
			nfudg.lwlly(state5);
			wefnl.lwlly(state6);
			sqygu.lwlly(state7);
			liyag.lwlly(state8);
			xsckh.lwlly(state9);
			illor.lwlly(state10);
			zgtws.lwlly(state11);
			kxffx.lwlly(state12);
			mvjsy.lwlly(state13);
			ikxna.lwlly(state14);
			wvycw.lwlly(state15);
		}
	}

	[StructLayout(LayoutKind.Explicit, Size = 32)]
	private struct hhqyy
	{
		[FieldOffset(0)]
		public byte dfrrm;

		[FieldOffset(0)]
		public uint zmlpc;

		[FieldOffset(4)]
		public uint unwwp;

		[FieldOffset(8)]
		public uint vckqi;

		[FieldOffset(12)]
		public uint efhta;

		[FieldOffset(16)]
		public uint joixh;

		[FieldOffset(20)]
		public uint zfvpq;

		[FieldOffset(24)]
		public uint ebwep;

		[FieldOffset(28)]
		public uint vublw;

		public hhqyy(uint state0, uint state1, uint state2, uint state3, uint state4, uint state5, uint state6, uint state7)
		{
			dfrrm = 0;
			zmlpc = state0;
			unwwp = state1;
			vckqi = state2;
			efhta = state3;
			joixh = state4;
			zfvpq = state5;
			ebwep = state6;
			vublw = state7;
		}
	}

	[StructLayout(LayoutKind.Explicit, Size = 4)]
	private struct pclma
	{
		[FieldOffset(0)]
		public byte idbap;

		[FieldOffset(1)]
		public byte hvaxy;

		[FieldOffset(2)]
		public byte ydpsk;

		[FieldOffset(3)]
		public byte aasqq;

		[FieldOffset(0)]
		public uint fmifq;

		public pclma(byte byte0, byte byte1, byte byte2, byte byte3)
		{
			this = default(pclma);
			idbap = byte0;
			hvaxy = byte1;
			ydpsk = byte2;
			aasqq = byte3;
		}

		public pclma(uint uintValue)
		{
			this = default(pclma);
			lwlly(uintValue);
		}

		public void lwlly(uint p0)
		{
			fmifq = ((BitConverter.IsLittleEndian ? true : false) ? p0 : (kjaiz(p0 & 0xFF00FF00u) | texrn(p0 & 0xFF00FF)));
		}

		private static uint texrn(uint p0)
		{
			return (p0 >> 8) | (p0 << 24);
		}

		private static uint kjaiz(uint p0)
		{
			return (p0 << 8) | (p0 >> 24);
		}

		public static implicit operator pclma(uint value)
		{
			return new pclma(value);
		}

		public static implicit operator uint(pclma value)
		{
			return value.fmifq;
		}
	}

	private struct uurtx
	{
		private byte[] gtoid;

		public uint this[int index]
		{
			get
			{
				int num = index * 4;
				return (uint)(gtoid[num++] | (gtoid[num++] << 8) | (gtoid[num++] << 16) | (gtoid[num] << 24));
			}
		}

		public uurtx(byte[] srcArray)
		{
			gtoid = srcArray;
		}
	}

	public const int njpww = 3;

	public const int pjyax = 255;

	public const int rcwbw = 8;

	public const int kcagg = 2;

	private uint mdtuw;

	private uint ihijf;

	private uint rtspj;

	private uint gxbkd;

	private uint pxeka;

	private uint bfmsa;

	private uint eeaah;

	private uint cmvpp;

	private uint trqpl;

	private uint yqxdr;

	private uint vskns;

	private uint wgclc;

	private uint lzzou;

	private uint llnlz;

	private uint slixg;

	private uint ismfk;

	private hhqyy webqg;

	public virtual bool smtsb => true;

	public virtual bool wbjie => true;

	public qtllz(byte[] key, byte[] nonce, int counter)
	{
		vifpo.uuala(key, nonce);
		rfnbq(key, nonce, counter, p3: true);
	}

	public int ovjyh(byte[] p0, int p1, int p2, byte[] p3, int p4)
	{
		if (p2 % 64 > 0)
		{
			throw new ArgumentException("inputCount must be divisible by " + iumzx);
		}
		return ivxhj(p0, p1, p2, p3, p4);
	}

	public byte[] higab(byte[] p0, int p1, int p2)
	{
		byte[] array = new byte[p2];
		ivxhj(p0, p1, p2, array, 0);
		return array;
	}

	public override int ivxhj(byte[] p0, int p1, int p2, byte[] p3, int p4)
	{
		vifpo.tgvgr(p0, p1, p2, p3, p4);
		rphwa(p0, p1, p2, p3, p4, p5: false);
		return p2;
	}

	private void rphwa(byte[] p0, int p1, int p2, byte[] p3, int p4, bool p5)
	{
		int num = p2 & 3;
		int num2 = p2 - num;
		int num3 = ((p0 == null) ? 16 : (p2 - num >> 2));
		if ((num3 == 0 || 1 == 0) && (num == 0 || 1 == 0))
		{
			return;
		}
		int num4 = p1 + num2;
		int num5 = p4 + num2;
		qetva qetva2 = peekn.dquvs(p0, p1, num2, p3: true);
		qetva qetva3 = peekn.dquvs(p3, p4, num2, p3: false);
		try
		{
			uint[] naxcq = qetva2.naxcq;
			uint[] naxcq2 = qetva3.naxcq;
			int num6 = ((!qetva2.azxdc || 1 == 0) ? (p1 >> 2) : 0);
			int num7 = ((!qetva3.azxdc || 1 == 0) ? (p4 >> 2) : 0);
			while (num3 > 0 || num > 0)
			{
				uint num8 = mdtuw;
				uint num9 = ihijf;
				uint num10 = rtspj;
				uint num11 = gxbkd;
				uint num12 = pxeka;
				uint num13 = bfmsa;
				uint num14 = eeaah;
				uint num15 = cmvpp;
				uint num16 = trqpl;
				uint num17 = yqxdr;
				uint num18 = vskns;
				uint num19 = wgclc;
				uint num20 = lzzou;
				uint num21 = llnlz;
				uint num22 = slixg;
				uint num23 = ismfk;
				int num24 = 0;
				if (num24 != 0)
				{
					goto IL_0130;
				}
				goto IL_0486;
				IL_0130:
				num8 += num12;
				num20 ^= num8;
				num20 = (num20 << 16) | (num20 >> 16);
				num16 += num20;
				num12 ^= num16;
				num12 = (num12 << 12) | (num12 >> 20);
				num8 += num12;
				num20 ^= num8;
				num20 = (num20 << 8) | (num20 >> 24);
				num16 += num20;
				num12 ^= num16;
				num12 = (num12 << 7) | (num12 >> 25);
				num9 += num13;
				num21 ^= num9;
				num21 = (num21 << 16) | (num21 >> 16);
				num17 += num21;
				num13 ^= num17;
				num13 = (num13 << 12) | (num13 >> 20);
				num9 += num13;
				num21 ^= num9;
				num21 = (num21 << 8) | (num21 >> 24);
				num17 += num21;
				num13 ^= num17;
				num13 = (num13 << 7) | (num13 >> 25);
				num10 += num14;
				num22 ^= num10;
				num22 = (num22 << 16) | (num22 >> 16);
				num18 += num22;
				num14 ^= num18;
				num14 = (num14 << 12) | (num14 >> 20);
				num10 += num14;
				num22 ^= num10;
				num22 = (num22 << 8) | (num22 >> 24);
				num18 += num22;
				num14 ^= num18;
				num14 = (num14 << 7) | (num14 >> 25);
				num11 += num15;
				num23 ^= num11;
				num23 = (num23 << 16) | (num23 >> 16);
				num19 += num23;
				num15 ^= num19;
				num15 = (num15 << 12) | (num15 >> 20);
				num11 += num15;
				num23 ^= num11;
				num23 = (num23 << 8) | (num23 >> 24);
				num19 += num23;
				num15 ^= num19;
				num15 = (num15 << 7) | (num15 >> 25);
				num8 += num13;
				num23 ^= num8;
				num23 = (num23 << 16) | (num23 >> 16);
				num18 += num23;
				num13 ^= num18;
				num13 = (num13 << 12) | (num13 >> 20);
				num8 += num13;
				num23 ^= num8;
				num23 = (num23 << 8) | (num23 >> 24);
				num18 += num23;
				num13 ^= num18;
				num13 = (num13 << 7) | (num13 >> 25);
				num9 += num14;
				num20 ^= num9;
				num20 = (num20 << 16) | (num20 >> 16);
				num19 += num20;
				num14 ^= num19;
				num14 = (num14 << 12) | (num14 >> 20);
				num9 += num14;
				num20 ^= num9;
				num20 = (num20 << 8) | (num20 >> 24);
				num19 += num20;
				num14 ^= num19;
				num14 = (num14 << 7) | (num14 >> 25);
				num10 += num15;
				num21 ^= num10;
				num21 = (num21 << 16) | (num21 >> 16);
				num16 += num21;
				num15 ^= num16;
				num15 = (num15 << 12) | (num15 >> 20);
				num10 += num15;
				num21 ^= num10;
				num21 = (num21 << 8) | (num21 >> 24);
				num16 += num21;
				num15 ^= num16;
				num15 = (num15 << 7) | (num15 >> 25);
				num11 += num12;
				num22 ^= num11;
				num22 = (num22 << 16) | (num22 >> 16);
				num17 += num22;
				num12 ^= num17;
				num12 = (num12 << 12) | (num12 >> 20);
				num11 += num12;
				num22 ^= num11;
				num22 = (num22 << 8) | (num22 >> 24);
				num17 += num22;
				num12 ^= num17;
				num12 = (num12 << 7) | (num12 >> 25);
				num24++;
				goto IL_0486;
				IL_0486:
				if (num24 >= 10)
				{
					num8 += mdtuw;
					num9 += ihijf;
					num10 += rtspj;
					num11 += gxbkd;
					num12 += pxeka;
					num13 += bfmsa;
					num14 += eeaah;
					num15 += cmvpp;
					num16 += trqpl;
					num17 += yqxdr;
					num18 += vskns;
					num19 += wgclc;
					num20 += lzzou;
					num21 += llnlz;
					num22 += slixg;
					num23 += ismfk;
					int num25;
					checked
					{
						lzzou++;
						if (p5 && 0 == 0)
						{
							if (BitConverter.IsLittleEndian && 0 == 0)
							{
								webqg = new hhqyy(num8, num9, num10, num11, num12, num13, num14, num15);
							}
							else
							{
								webqg = new znibc(num8, num9, num10, num11, num12, num13, num14, num15, num16, num17, num18, num19, num20, num21, num22, num23).xrsab;
							}
							return;
						}
						num25 = Math.Min(num3, 16);
					}
					num3 -= num25;
					uint num26;
					switch (num25)
					{
					case 16:
						naxcq2[num7++] = naxcq[num6++] ^ num8;
						naxcq2[num7++] = naxcq[num6++] ^ num9;
						naxcq2[num7++] = naxcq[num6++] ^ num10;
						naxcq2[num7++] = naxcq[num6++] ^ num11;
						naxcq2[num7++] = naxcq[num6++] ^ num12;
						naxcq2[num7++] = naxcq[num6++] ^ num13;
						naxcq2[num7++] = naxcq[num6++] ^ num14;
						naxcq2[num7++] = naxcq[num6++] ^ num15;
						naxcq2[num7++] = naxcq[num6++] ^ num16;
						naxcq2[num7++] = naxcq[num6++] ^ num17;
						naxcq2[num7++] = naxcq[num6++] ^ num18;
						naxcq2[num7++] = naxcq[num6++] ^ num19;
						naxcq2[num7++] = naxcq[num6++] ^ num20;
						naxcq2[num7++] = naxcq[num6++] ^ num21;
						naxcq2[num7++] = naxcq[num6++] ^ num22;
						naxcq2[num7++] = naxcq[num6++] ^ num23;
						continue;
					case 15:
						naxcq2[num7++] = naxcq[num6++] ^ num8;
						naxcq2[num7++] = naxcq[num6++] ^ num9;
						naxcq2[num7++] = naxcq[num6++] ^ num10;
						naxcq2[num7++] = naxcq[num6++] ^ num11;
						naxcq2[num7++] = naxcq[num6++] ^ num12;
						naxcq2[num7++] = naxcq[num6++] ^ num13;
						naxcq2[num7++] = naxcq[num6++] ^ num14;
						naxcq2[num7++] = naxcq[num6++] ^ num15;
						naxcq2[num7++] = naxcq[num6++] ^ num16;
						naxcq2[num7++] = naxcq[num6++] ^ num17;
						naxcq2[num7++] = naxcq[num6++] ^ num18;
						naxcq2[num7++] = naxcq[num6++] ^ num19;
						naxcq2[num7++] = naxcq[num6++] ^ num20;
						naxcq2[num7++] = naxcq[num6++] ^ num21;
						naxcq2[num7++] = naxcq[num6++] ^ num22;
						num26 = num23;
						break;
					case 14:
						naxcq2[num7++] = naxcq[num6++] ^ num8;
						naxcq2[num7++] = naxcq[num6++] ^ num9;
						naxcq2[num7++] = naxcq[num6++] ^ num10;
						naxcq2[num7++] = naxcq[num6++] ^ num11;
						naxcq2[num7++] = naxcq[num6++] ^ num12;
						naxcq2[num7++] = naxcq[num6++] ^ num13;
						naxcq2[num7++] = naxcq[num6++] ^ num14;
						naxcq2[num7++] = naxcq[num6++] ^ num15;
						naxcq2[num7++] = naxcq[num6++] ^ num16;
						naxcq2[num7++] = naxcq[num6++] ^ num17;
						naxcq2[num7++] = naxcq[num6++] ^ num18;
						naxcq2[num7++] = naxcq[num6++] ^ num19;
						naxcq2[num7++] = naxcq[num6++] ^ num20;
						naxcq2[num7++] = naxcq[num6++] ^ num21;
						num26 = num22;
						break;
					case 13:
						naxcq2[num7++] = naxcq[num6++] ^ num8;
						naxcq2[num7++] = naxcq[num6++] ^ num9;
						naxcq2[num7++] = naxcq[num6++] ^ num10;
						naxcq2[num7++] = naxcq[num6++] ^ num11;
						naxcq2[num7++] = naxcq[num6++] ^ num12;
						naxcq2[num7++] = naxcq[num6++] ^ num13;
						naxcq2[num7++] = naxcq[num6++] ^ num14;
						naxcq2[num7++] = naxcq[num6++] ^ num15;
						naxcq2[num7++] = naxcq[num6++] ^ num16;
						naxcq2[num7++] = naxcq[num6++] ^ num17;
						naxcq2[num7++] = naxcq[num6++] ^ num18;
						naxcq2[num7++] = naxcq[num6++] ^ num19;
						naxcq2[num7++] = naxcq[num6++] ^ num20;
						num26 = num21;
						break;
					case 12:
						naxcq2[num7++] = naxcq[num6++] ^ num8;
						naxcq2[num7++] = naxcq[num6++] ^ num9;
						naxcq2[num7++] = naxcq[num6++] ^ num10;
						naxcq2[num7++] = naxcq[num6++] ^ num11;
						naxcq2[num7++] = naxcq[num6++] ^ num12;
						naxcq2[num7++] = naxcq[num6++] ^ num13;
						naxcq2[num7++] = naxcq[num6++] ^ num14;
						naxcq2[num7++] = naxcq[num6++] ^ num15;
						naxcq2[num7++] = naxcq[num6++] ^ num16;
						naxcq2[num7++] = naxcq[num6++] ^ num17;
						naxcq2[num7++] = naxcq[num6++] ^ num18;
						naxcq2[num7++] = naxcq[num6++] ^ num19;
						num26 = num20;
						break;
					case 11:
						naxcq2[num7++] = naxcq[num6++] ^ num8;
						naxcq2[num7++] = naxcq[num6++] ^ num9;
						naxcq2[num7++] = naxcq[num6++] ^ num10;
						naxcq2[num7++] = naxcq[num6++] ^ num11;
						naxcq2[num7++] = naxcq[num6++] ^ num12;
						naxcq2[num7++] = naxcq[num6++] ^ num13;
						naxcq2[num7++] = naxcq[num6++] ^ num14;
						naxcq2[num7++] = naxcq[num6++] ^ num15;
						naxcq2[num7++] = naxcq[num6++] ^ num16;
						naxcq2[num7++] = naxcq[num6++] ^ num17;
						naxcq2[num7++] = naxcq[num6++] ^ num18;
						num26 = num19;
						break;
					case 10:
						naxcq2[num7++] = naxcq[num6++] ^ num8;
						naxcq2[num7++] = naxcq[num6++] ^ num9;
						naxcq2[num7++] = naxcq[num6++] ^ num10;
						naxcq2[num7++] = naxcq[num6++] ^ num11;
						naxcq2[num7++] = naxcq[num6++] ^ num12;
						naxcq2[num7++] = naxcq[num6++] ^ num13;
						naxcq2[num7++] = naxcq[num6++] ^ num14;
						naxcq2[num7++] = naxcq[num6++] ^ num15;
						naxcq2[num7++] = naxcq[num6++] ^ num16;
						naxcq2[num7++] = naxcq[num6++] ^ num17;
						num26 = num18;
						break;
					case 9:
						naxcq2[num7++] = naxcq[num6++] ^ num8;
						naxcq2[num7++] = naxcq[num6++] ^ num9;
						naxcq2[num7++] = naxcq[num6++] ^ num10;
						naxcq2[num7++] = naxcq[num6++] ^ num11;
						naxcq2[num7++] = naxcq[num6++] ^ num12;
						naxcq2[num7++] = naxcq[num6++] ^ num13;
						naxcq2[num7++] = naxcq[num6++] ^ num14;
						naxcq2[num7++] = naxcq[num6++] ^ num15;
						naxcq2[num7++] = naxcq[num6++] ^ num16;
						num26 = num17;
						break;
					case 8:
						naxcq2[num7++] = naxcq[num6++] ^ num8;
						naxcq2[num7++] = naxcq[num6++] ^ num9;
						naxcq2[num7++] = naxcq[num6++] ^ num10;
						naxcq2[num7++] = naxcq[num6++] ^ num11;
						naxcq2[num7++] = naxcq[num6++] ^ num12;
						naxcq2[num7++] = naxcq[num6++] ^ num13;
						naxcq2[num7++] = naxcq[num6++] ^ num14;
						naxcq2[num7++] = naxcq[num6++] ^ num15;
						num26 = num16;
						break;
					case 7:
						naxcq2[num7++] = naxcq[num6++] ^ num8;
						naxcq2[num7++] = naxcq[num6++] ^ num9;
						naxcq2[num7++] = naxcq[num6++] ^ num10;
						naxcq2[num7++] = naxcq[num6++] ^ num11;
						naxcq2[num7++] = naxcq[num6++] ^ num12;
						naxcq2[num7++] = naxcq[num6++] ^ num13;
						naxcq2[num7++] = naxcq[num6++] ^ num14;
						num26 = num15;
						break;
					case 6:
						naxcq2[num7++] = naxcq[num6++] ^ num8;
						naxcq2[num7++] = naxcq[num6++] ^ num9;
						naxcq2[num7++] = naxcq[num6++] ^ num10;
						naxcq2[num7++] = naxcq[num6++] ^ num11;
						naxcq2[num7++] = naxcq[num6++] ^ num12;
						naxcq2[num7++] = naxcq[num6++] ^ num13;
						num26 = num14;
						break;
					case 5:
						naxcq2[num7++] = naxcq[num6++] ^ num8;
						naxcq2[num7++] = naxcq[num6++] ^ num9;
						naxcq2[num7++] = naxcq[num6++] ^ num10;
						naxcq2[num7++] = naxcq[num6++] ^ num11;
						naxcq2[num7++] = naxcq[num6++] ^ num12;
						num26 = num13;
						break;
					case 4:
						naxcq2[num7++] = naxcq[num6++] ^ num8;
						naxcq2[num7++] = naxcq[num6++] ^ num9;
						naxcq2[num7++] = naxcq[num6++] ^ num10;
						naxcq2[num7++] = naxcq[num6++] ^ num11;
						num26 = num12;
						break;
					case 3:
						naxcq2[num7++] = naxcq[num6++] ^ num8;
						naxcq2[num7++] = naxcq[num6++] ^ num9;
						naxcq2[num7++] = naxcq[num6++] ^ num10;
						num26 = num11;
						break;
					case 2:
						naxcq2[num7++] = naxcq[num6++] ^ num8;
						naxcq2[num7++] = naxcq[num6++] ^ num9;
						num26 = num10;
						break;
					case 1:
						naxcq2[num7++] = naxcq[num6++] ^ num8;
						num26 = num9;
						break;
					case 0:
						num26 = num8;
						break;
					default:
						throw new ArgumentOutOfRangeException();
					}
					while (num > 0)
					{
						p3[num5++] = (byte)(p0[num4++] ^ (byte)(num26 & 0xFF));
						num--;
						num26 >>= 8;
					}
					continue;
				}
				goto IL_0130;
			}
			if (qetva3.azxdc && 0 == 0)
			{
				Buffer.BlockCopy(naxcq2, 0, p3, p4, num2);
			}
		}
		finally
		{
			qetva2.mymvf();
			qetva3.mymvf();
		}
	}

	protected override void maivd(byte[] p0, int p1)
	{
		rfnbq(null, p0, p1, p3: false);
	}

	protected override void rxgtt(nxtme<byte> p0, IntPtr p1)
	{
		rphwa(null, 0, 0, null, 0, p5: true);
		if (p1 != IntPtr.Zero && 0 == 0)
		{
			Marshal.StructureToPtr((object)webqg, p1, fDeleteOld: false);
			return;
		}
		rnqdw rnqdw2 = p0;
		try
		{
			Marshal.StructureToPtr((object)webqg, rnqdw2.peara(), fDeleteOld: false);
		}
		finally
		{
			rnqdw2.fbdzt();
		}
	}

	protected override void cldjt(bool p0)
	{
		if (p0 && 0 == 0)
		{
			mdtuw = 0u;
			ihijf = 0u;
			rtspj = 0u;
			gxbkd = 0u;
			pxeka = 0u;
			bfmsa = 0u;
			eeaah = 0u;
			cmvpp = 0u;
			trqpl = 0u;
			yqxdr = 0u;
			vskns = 0u;
			wgclc = 0u;
			lzzou = 0u;
			llnlz = 0u;
			slixg = 0u;
			ismfk = 0u;
			webqg = default(hhqyy);
		}
		base.cldjt(p0);
	}

	private void rfnbq(byte[] p0, byte[] p1, int p2, bool p3)
	{
		if (p3 && 0 == 0)
		{
			uurtx uurtx = new uurtx(p0);
			mdtuw = 1634760805u;
			ihijf = 857760878u;
			rtspj = 2036477234u;
			gxbkd = 1797285236u;
			pxeka = uurtx[0];
			bfmsa = uurtx[1];
			eeaah = uurtx[2];
			cmvpp = uurtx[3];
			trqpl = uurtx[4];
			yqxdr = uurtx[5];
			vskns = uurtx[6];
			wgclc = uurtx[7];
		}
		uurtx uurtx2 = new uurtx(p1);
		lzzou = (uint)p2;
		llnlz = uurtx2[0];
		slixg = uurtx2[1];
		ismfk = uurtx2[2];
	}
}
