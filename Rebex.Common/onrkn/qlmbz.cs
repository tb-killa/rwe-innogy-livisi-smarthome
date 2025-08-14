using System;

namespace onrkn;

internal class qlmbz : rysmc
{
	private static uint[] bahdc = new uint[64]
	{
		1116352408u, 1899447441u, 3049323471u, 3921009573u, 961987163u, 1508970993u, 2453635748u, 2870763221u, 3624381080u, 310598401u,
		607225278u, 1426881987u, 1925078388u, 2162078206u, 2614888103u, 3248222580u, 3835390401u, 4022224774u, 264347078u, 604807628u,
		770255983u, 1249150122u, 1555081692u, 1996064986u, 2554220882u, 2821834349u, 2952996808u, 3210313671u, 3336571891u, 3584528711u,
		113926993u, 338241895u, 666307205u, 773529912u, 1294757372u, 1396182291u, 1695183700u, 1986661051u, 2177026350u, 2456956037u,
		2730485921u, 2820302411u, 3259730800u, 3345764771u, 3516065817u, 3600352804u, 4094571909u, 275423344u, 430227734u, 506948616u,
		659060556u, 883997877u, 958139571u, 1322822218u, 1537002063u, 1747873779u, 1955562222u, 2024104815u, 2227730452u, 2361852424u,
		2428436474u, 2756734187u, 3204031479u, 3329325298u
	};

	private uint hojzs;

	private uint pbtbq;

	private uint nnqtg;

	private uint oriuh;

	private uint xkzmv;

	private uint kvuvy;

	private uint kycgj;

	private uint cmhez;

	private uint[] gxbym;

	private bool oyfqa;

	private int xwpww;

	public override int HashSize => xwpww;

	protected override int rbegz => 64;

	protected override int qvxcs => 8;

	protected override byte[] tsqhg()
	{
		byte[] array = new byte[HashSize / 8];
		nkmjd(array, 0, hojzs);
		nkmjd(array, 4, pbtbq);
		nkmjd(array, 8, nnqtg);
		nkmjd(array, 12, oriuh);
		nkmjd(array, 16, xkzmv);
		nkmjd(array, 20, kvuvy);
		nkmjd(array, 24, kycgj);
		if (oyfqa && 0 == 0)
		{
			nkmjd(array, 28, cmhez);
		}
		return array;
	}

	public qlmbz(bool fullSize)
	{
		oyfqa = fullSize;
		xwpww = ((fullSize ? true : false) ? 256 : 224);
		gxbym = new uint[64];
		Reset();
	}

	private uint aquxi(byte[] p0, ref int p1)
	{
		return (uint)((p0[p1++] << 24) | (p0[p1++] << 16) | (p0[p1++] << 8) | p0[p1++]);
	}

	public override void Reset()
	{
		base.Reset();
		Array.Clear(gxbym, 0, gxbym.Length);
		if (oyfqa && 0 == 0)
		{
			hojzs = 1779033703u;
			pbtbq = 3144134277u;
			nnqtg = 1013904242u;
			oriuh = 2773480762u;
			xkzmv = 1359893119u;
			kvuvy = 2600822924u;
			kycgj = 528734635u;
			cmhez = 1541459225u;
		}
		else
		{
			hojzs = 3238371032u;
			pbtbq = 914150663u;
			nnqtg = 812702999u;
			oriuh = 4144912697u;
			xkzmv = 4290775857u;
			kvuvy = 1750603025u;
			kycgj = 1694076839u;
			cmhez = 3204075428u;
		}
	}

	protected override void ygxqo(byte[] p0, int p1)
	{
		int num = 0;
		if (num != 0)
		{
			goto IL_0009;
		}
		goto IL_001e;
		IL_0009:
		gxbym[num] = aquxi(p0, ref p1);
		num++;
		goto IL_001e;
		IL_001e:
		if (num < 16)
		{
			goto IL_0009;
		}
		int num2 = 16;
		if (num2 == 0)
		{
			goto IL_0029;
		}
		goto IL_0092;
		IL_00e1:
		uint num4;
		uint num5;
		uint num6;
		uint num7;
		int num8;
		uint num3 = num4 + (((num5 >> 6) | (num5 << 26)) ^ ((num5 >> 11) | (num5 << 21)) ^ ((num5 >> 25) | (num5 << 7))) + ((num5 & (num6 ^ num7)) ^ num7) + bahdc[num8] + gxbym[num8];
		uint num10;
		uint num11;
		uint num12;
		uint num9 = num3 + (((num10 >> 2) | (num10 << 30)) ^ ((num10 >> 13) | (num10 << 19)) ^ ((num10 >> 22) | (num10 << 10))) + ((num10 & (num11 | num12)) | (num11 & num12));
		uint num14;
		uint num13 = num14 + num3;
		num8++;
		num3 = num7 + (((num13 >> 6) | (num13 << 26)) ^ ((num13 >> 11) | (num13 << 21)) ^ ((num13 >> 25) | (num13 << 7))) + ((num13 & (num5 ^ num6)) ^ num6) + bahdc[num8] + gxbym[num8];
		uint num15 = num3 + (((num9 >> 2) | (num9 << 30)) ^ ((num9 >> 13) | (num9 << 19)) ^ ((num9 >> 22) | (num9 << 10))) + ((num9 & (num10 | num11)) | (num10 & num11));
		uint num16 = num12 + num3;
		num8++;
		num3 = num6 + (((num16 >> 6) | (num16 << 26)) ^ ((num16 >> 11) | (num16 << 21)) ^ ((num16 >> 25) | (num16 << 7))) + ((num16 & (num13 ^ num5)) ^ num5) + bahdc[num8] + gxbym[num8];
		uint num17 = num3 + (((num15 >> 2) | (num15 << 30)) ^ ((num15 >> 13) | (num15 << 19)) ^ ((num15 >> 22) | (num15 << 10))) + ((num15 & (num9 | num10)) | (num9 & num10));
		uint num18 = num11 + num3;
		num8++;
		num3 = num5 + (((num18 >> 6) | (num18 << 26)) ^ ((num18 >> 11) | (num18 << 21)) ^ ((num18 >> 25) | (num18 << 7))) + ((num18 & (num16 ^ num13)) ^ num13) + bahdc[num8] + gxbym[num8];
		uint num19 = num3 + (((num17 >> 2) | (num17 << 30)) ^ ((num17 >> 13) | (num17 << 19)) ^ ((num17 >> 22) | (num17 << 10))) + ((num17 & (num15 | num9)) | (num15 & num9));
		uint num20 = num10 + num3;
		num8++;
		num3 = num13 + (((num20 >> 6) | (num20 << 26)) ^ ((num20 >> 11) | (num20 << 21)) ^ ((num20 >> 25) | (num20 << 7))) + ((num20 & (num18 ^ num16)) ^ num16) + bahdc[num8] + gxbym[num8];
		num14 = num3 + (((num19 >> 2) | (num19 << 30)) ^ ((num19 >> 13) | (num19 << 19)) ^ ((num19 >> 22) | (num19 << 10))) + ((num19 & (num17 | num15)) | (num17 & num15));
		num4 = num9 + num3;
		num8++;
		num3 = num16 + (((num4 >> 6) | (num4 << 26)) ^ ((num4 >> 11) | (num4 << 21)) ^ ((num4 >> 25) | (num4 << 7))) + ((num4 & (num20 ^ num18)) ^ num18) + bahdc[num8] + gxbym[num8];
		num12 = num3 + (((num14 >> 2) | (num14 << 30)) ^ ((num14 >> 13) | (num14 << 19)) ^ ((num14 >> 22) | (num14 << 10))) + ((num14 & (num19 | num17)) | (num19 & num17));
		num7 = num15 + num3;
		num8++;
		num3 = num18 + (((num7 >> 6) | (num7 << 26)) ^ ((num7 >> 11) | (num7 << 21)) ^ ((num7 >> 25) | (num7 << 7))) + ((num7 & (num4 ^ num20)) ^ num20) + bahdc[num8] + gxbym[num8];
		num11 = num3 + (((num12 >> 2) | (num12 << 30)) ^ ((num12 >> 13) | (num12 << 19)) ^ ((num12 >> 22) | (num12 << 10))) + ((num12 & (num14 | num19)) | (num14 & num19));
		num6 = num17 + num3;
		num8++;
		num3 = num20 + (((num6 >> 6) | (num6 << 26)) ^ ((num6 >> 11) | (num6 << 21)) ^ ((num6 >> 25) | (num6 << 7))) + ((num6 & (num7 ^ num4)) ^ num4) + bahdc[num8] + gxbym[num8];
		num10 = num3 + (((num11 >> 2) | (num11 << 30)) ^ ((num11 >> 13) | (num11 << 19)) ^ ((num11 >> 22) | (num11 << 10))) + ((num11 & (num12 | num14)) | (num12 & num14));
		num5 = num19 + num3;
		num8++;
		goto IL_0521;
		IL_0029:
		uint num21 = gxbym[num2 - 2];
		uint num22 = gxbym[num2 - 15];
		gxbym[num2] = (((num21 >> 17) | (num21 << 15)) ^ ((num21 >> 19) | (num21 << 13)) ^ (num21 >> 10)) + gxbym[num2 - 7] + (((num22 >> 7) | (num22 << 25)) ^ ((num22 >> 18) | (num22 << 14)) ^ (num22 >> 3)) + gxbym[num2 - 16];
		num2++;
		goto IL_0092;
		IL_0092:
		if (num2 < 64)
		{
			goto IL_0029;
		}
		num10 = hojzs;
		num11 = pbtbq;
		num12 = nnqtg;
		num14 = oriuh;
		num5 = xkzmv;
		num6 = kvuvy;
		num7 = kycgj;
		num4 = cmhez;
		num8 = 0;
		if (num8 != 0)
		{
			goto IL_00e1;
		}
		goto IL_0521;
		IL_0521:
		if (num8 >= 64)
		{
			hojzs = num10 + hojzs;
			pbtbq = num11 + pbtbq;
			nnqtg = num12 + nnqtg;
			oriuh = num14 + oriuh;
			xkzmv = num5 + xkzmv;
			kvuvy = num6 + kvuvy;
			kycgj = num7 + kycgj;
			cmhez = num4 + cmhez;
			return;
		}
		goto IL_00e1;
	}
}
