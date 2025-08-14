using System;

namespace onrkn;

internal class wcixh : rysmc
{
	private static ulong[] ssteq = new ulong[80]
	{
		4794697086780616226uL, 8158064640168781261uL, 13096744586834688815uL, 16840607885511220156uL, 4131703408338449720uL, 6480981068601479193uL, 10538285296894168987uL, 12329834152419229976uL, 15566598209576043074uL, 1334009975649890238uL,
		2608012711638119052uL, 6128411473006802146uL, 8268148722764581231uL, 9286055187155687089uL, 11230858885718282805uL, 13951009754708518548uL, 16472876342353939154uL, 17275323862435702243uL, 1135362057144423861uL, 2597628984639134821uL,
		3308224258029322869uL, 5365058923640841347uL, 6679025012923562964uL, 8573033837759648693uL, 10970295158949994411uL, 12119686244451234320uL, 12683024718118986047uL, 13788192230050041572uL, 14330467153632333762uL, 15395433587784984357uL,
		489312712824947311uL, 1452737877330783856uL, 2861767655752347644uL, 3322285676063803686uL, 5560940570517711597uL, 5996557281743188959uL, 7280758554555802590uL, 8532644243296465576uL, 9350256976987008742uL, 10552545826968843579uL,
		11727347734174303076uL, 12113106623233404929uL, 14000437183269869457uL, 14369950271660146224uL, 15101387698204529176uL, 15463397548674623760uL, 17586052441742319658uL, 1182934255886127544uL, 1847814050463011016uL, 2177327727835720531uL,
		2830643537854262169uL, 3796741975233480872uL, 4115178125766777443uL, 5681478168544905931uL, 6601373596472566643uL, 7507060721942968483uL, 8399075790359081724uL, 8693463985226723168uL, 9568029438360202098uL, 10144078919501101548uL,
		10430055236837252648uL, 11840083180663258601uL, 13761210420658862357uL, 14299343276471374635uL, 14566680578165727644uL, 15097957966210449927uL, 16922976911328602910uL, 17689382322260857208uL, 500013540394364858uL, 748580250866718886uL,
		1242879168328830382uL, 1977374033974150939uL, 2944078676154940804uL, 3659926193048069267uL, 4368137639120453308uL, 4836135668995329356uL, 5532061633213252278uL, 6448918945643986474uL, 6902733635092675308uL, 7801388544844847127uL
	};

	private ulong cbvnd;

	private ulong hsrux;

	private ulong cdwnc;

	private ulong ubzrf;

	private ulong unrxw;

	private ulong avyaf;

	private ulong pzuxl;

	private ulong jyopq;

	private ulong[] jngog;

	private bool kwyut;

	private int sbvwf;

	public override int HashSize => sbvwf;

	protected override int rbegz => 128;

	protected override int qvxcs => 16;

	protected override byte[] tsqhg()
	{
		byte[] array = new byte[HashSize / 8];
		jqzjf(array, 0, cbvnd);
		jqzjf(array, 8, hsrux);
		jqzjf(array, 16, cdwnc);
		jqzjf(array, 24, ubzrf);
		jqzjf(array, 32, unrxw);
		jqzjf(array, 40, avyaf);
		if (kwyut && 0 == 0)
		{
			jqzjf(array, 48, pzuxl);
			jqzjf(array, 56, jyopq);
		}
		return array;
	}

	public wcixh(bool fullSize)
	{
		kwyut = fullSize;
		sbvwf = ((fullSize ? true : false) ? 512 : 384);
		jngog = new ulong[80];
		Reset();
	}

	private ulong adokc(byte[] p0, ref int p1)
	{
		return ((ulong)p0[p1++] << 56) | ((ulong)p0[p1++] << 48) | ((ulong)p0[p1++] << 40) | ((ulong)p0[p1++] << 32) | ((ulong)p0[p1++] << 24) | ((ulong)p0[p1++] << 16) | ((ulong)p0[p1++] << 8) | p0[p1++];
	}

	public override void Reset()
	{
		base.Reset();
		Array.Clear(jngog, 0, jngog.Length);
		if (kwyut && 0 == 0)
		{
			cbvnd = 7640891576956012808uL;
			hsrux = 13503953896175478587uL;
			cdwnc = 4354685564936845355uL;
			ubzrf = 11912009170470909681uL;
			unrxw = 5840696475078001361uL;
			avyaf = 11170449401992604703uL;
			pzuxl = 2270897969802886507uL;
			jyopq = 6620516959819538809uL;
		}
		else
		{
			cbvnd = 14680500436340154072uL;
			hsrux = 7105036623409894663uL;
			cdwnc = 10473403895298186519uL;
			ubzrf = 1526699215303891257uL;
			unrxw = 7436329637833083697uL;
			avyaf = 10282925794625328401uL;
			pzuxl = 15784041429090275239uL;
			jyopq = 5167115440072839076uL;
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
		jngog[num] = adokc(p0, ref p1);
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
		goto IL_008f;
		IL_00de:
		ulong num4;
		ulong num5;
		ulong num6;
		ulong num7;
		int num8;
		ulong num3 = num4 + (((num5 >> 14) | (num5 << 50)) ^ ((num5 >> 18) | (num5 << 46)) ^ ((num5 >> 41) | (num5 << 23))) + ((num5 & (num6 ^ num7)) ^ num7) + ssteq[num8] + jngog[num8];
		ulong num10;
		ulong num11;
		ulong num12;
		ulong num9 = num3 + (((num10 >> 28) | (num10 << 36)) ^ ((num10 >> 34) | (num10 << 30)) ^ ((num10 >> 39) | (num10 << 25))) + ((num10 & (num11 | num12)) | (num11 & num12));
		ulong num14;
		ulong num13 = num14 + num3;
		num8++;
		num3 = num7 + (((num13 >> 14) | (num13 << 50)) ^ ((num13 >> 18) | (num13 << 46)) ^ ((num13 >> 41) | (num13 << 23))) + ((num13 & (num5 ^ num6)) ^ num6) + ssteq[num8] + jngog[num8];
		ulong num15 = num3 + (((num9 >> 28) | (num9 << 36)) ^ ((num9 >> 34) | (num9 << 30)) ^ ((num9 >> 39) | (num9 << 25))) + ((num9 & (num10 | num11)) | (num10 & num11));
		ulong num16 = num12 + num3;
		num8++;
		num3 = num6 + (((num16 >> 14) | (num16 << 50)) ^ ((num16 >> 18) | (num16 << 46)) ^ ((num16 >> 41) | (num16 << 23))) + ((num16 & (num13 ^ num5)) ^ num5) + ssteq[num8] + jngog[num8];
		ulong num17 = num3 + (((num15 >> 28) | (num15 << 36)) ^ ((num15 >> 34) | (num15 << 30)) ^ ((num15 >> 39) | (num15 << 25))) + ((num15 & (num9 | num10)) | (num9 & num10));
		ulong num18 = num11 + num3;
		num8++;
		num3 = num5 + (((num18 >> 14) | (num18 << 50)) ^ ((num18 >> 18) | (num18 << 46)) ^ ((num18 >> 41) | (num18 << 23))) + ((num18 & (num16 ^ num13)) ^ num13) + ssteq[num8] + jngog[num8];
		ulong num19 = num3 + (((num17 >> 28) | (num17 << 36)) ^ ((num17 >> 34) | (num17 << 30)) ^ ((num17 >> 39) | (num17 << 25))) + ((num17 & (num15 | num9)) | (num15 & num9));
		ulong num20 = num10 + num3;
		num8++;
		num3 = num13 + (((num20 >> 14) | (num20 << 50)) ^ ((num20 >> 18) | (num20 << 46)) ^ ((num20 >> 41) | (num20 << 23))) + ((num20 & (num18 ^ num16)) ^ num16) + ssteq[num8] + jngog[num8];
		num14 = num3 + (((num19 >> 28) | (num19 << 36)) ^ ((num19 >> 34) | (num19 << 30)) ^ ((num19 >> 39) | (num19 << 25))) + ((num19 & (num17 | num15)) | (num17 & num15));
		num4 = num9 + num3;
		num8++;
		num3 = num16 + (((num4 >> 14) | (num4 << 50)) ^ ((num4 >> 18) | (num4 << 46)) ^ ((num4 >> 41) | (num4 << 23))) + ((num4 & (num20 ^ num18)) ^ num18) + ssteq[num8] + jngog[num8];
		num12 = num3 + (((num14 >> 28) | (num14 << 36)) ^ ((num14 >> 34) | (num14 << 30)) ^ ((num14 >> 39) | (num14 << 25))) + ((num14 & (num19 | num17)) | (num19 & num17));
		num7 = num15 + num3;
		num8++;
		num3 = num18 + (((num7 >> 14) | (num7 << 50)) ^ ((num7 >> 18) | (num7 << 46)) ^ ((num7 >> 41) | (num7 << 23))) + ((num7 & (num4 ^ num20)) ^ num20) + ssteq[num8] + jngog[num8];
		num11 = num3 + (((num12 >> 28) | (num12 << 36)) ^ ((num12 >> 34) | (num12 << 30)) ^ ((num12 >> 39) | (num12 << 25))) + ((num12 & (num14 | num19)) | (num14 & num19));
		num6 = num17 + num3;
		num8++;
		num3 = num20 + (((num6 >> 14) | (num6 << 50)) ^ ((num6 >> 18) | (num6 << 46)) ^ ((num6 >> 41) | (num6 << 23))) + ((num6 & (num7 ^ num4)) ^ num4) + ssteq[num8] + jngog[num8];
		num10 = num3 + (((num11 >> 28) | (num11 << 36)) ^ ((num11 >> 34) | (num11 << 30)) ^ ((num11 >> 39) | (num11 << 25))) + ((num11 & (num12 | num14)) | (num12 & num14));
		num5 = num19 + num3;
		num8++;
		goto IL_0536;
		IL_0029:
		ulong num21 = jngog[num2 - 2];
		ulong num22 = jngog[num2 - 15];
		jngog[num2] = (((num21 >> 19) | (num21 << 45)) ^ ((num21 >> 61) | (num21 << 3)) ^ (num21 >> 6)) + jngog[num2 - 7] + (((num22 >> 1) | (num22 << 63)) ^ ((num22 >> 8) | (num22 << 56)) ^ (num22 >> 7)) + jngog[num2 - 16];
		num2++;
		goto IL_008f;
		IL_008f:
		if (num2 < 80)
		{
			goto IL_0029;
		}
		num10 = cbvnd;
		num11 = hsrux;
		num12 = cdwnc;
		num14 = ubzrf;
		num5 = unrxw;
		num6 = avyaf;
		num7 = pzuxl;
		num4 = jyopq;
		num8 = 0;
		if (num8 != 0)
		{
			goto IL_00de;
		}
		goto IL_0536;
		IL_0536:
		if (num8 >= 80)
		{
			cbvnd = num10 + cbvnd;
			hsrux = num11 + hsrux;
			cdwnc = num12 + cdwnc;
			ubzrf = num14 + ubzrf;
			unrxw = num5 + unrxw;
			avyaf = num6 + avyaf;
			pzuxl = num7 + pzuxl;
			jyopq = num4 + jyopq;
			return;
		}
		goto IL_00de;
	}
}
