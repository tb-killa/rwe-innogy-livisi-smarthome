using System;

namespace onrkn;

internal static class ohmbc
{
	public static void tcunw(uint[] p0, ref int p1, uint[] p2, int p3, uint[] p4, out int p5)
	{
		if (p3 == 0 || 1 == 0)
		{
			throw new DivideByZeroException();
		}
		if (p1 == 0 || 1 == 0)
		{
			p5 = 0;
		}
		else if (p1 < p3)
		{
			p5 = 0;
		}
		else if (p3 == 1)
		{
			xayhp(p0, ref p1, p2[0], p4, out p5);
		}
		else
		{
			fogcl(p0, ref p1, p2, p3, p4, out p5);
		}
	}

	private static void xayhp(uint[] p0, ref int p1, uint p2, uint[] p3, out int p4)
	{
		if (p1 == 1)
		{
			uint num = p0[0];
			uint num2 = (p3[0] = num / p2);
			p4 = 1;
			if ((p0[0] = num - num2 * p2) == 0 || 1 == 0)
			{
				p1 = 0;
			}
			return;
		}
		p4 = 0;
		for (int num3 = p1; num3 > 0; num3--)
		{
			ulong num4 = uocgn((num3 != p1) ? p0[num3] : 0u, p0[num3 - 1]);
			ulong num5 = num4 / p2;
			if (num5 != 0)
			{
				ulong num6 = num5 * p2;
				p0[num3 - 1] -= (uint)(int)num6;
				if (p3 != null && 0 == 0)
				{
					p3[num3 - 1] = (uint)num5;
					if (p4 == 0 || 1 == 0)
					{
						p4 = num3;
					}
				}
			}
		}
		p1 = ((p0[0] != 0 && 0 == 0) ? 1 : 0);
	}

	private static void fogcl(uint[] p0, ref int p1, uint[] p2, int p3, uint[] p4, out int p5)
	{
		int num = ((royph(p0, p1 - 1, p2, p3 - 1) >= 0) ? 1 : 0);
		p5 = p1 - p3 + num;
		uint num2 = p2[p3 - 1];
		uint num3 = p2[p3 - 2];
		int num4 = wgeda.njbpp(num2);
		int num5 = 31 - num4;
		uint num6;
		uint num7;
		if (num5 == 0 || 1 == 0)
		{
			num6 = num2;
			num7 = num3;
		}
		else
		{
			num6 = (num2 << num5) | (num3 >> 32 - num5);
			num7 = (num3 << num5) | ((p3 >= 3) ? (p2[p3 - 3] >> 32 - num5) : 0);
		}
		int num8 = ((num == 0) ? (p1 - 1) : p1);
		int num9 = p5 - 1;
		int num10 = num8;
		while (num9 >= 0)
		{
			uint num11 = ((num10 < p1) ? p0[num10] : 0u);
			uint num12 = p0[num10 - 1];
			uint num13 = ((num10 - 2 >= 0) ? p0[num10 - 2] : 0u);
			ulong num14 = num11;
			ulong num15;
			uint p6;
			if (num5 == 0 || 1 == 0)
			{
				num15 = (num14 << 32) | num12;
				p6 = num13;
			}
			else
			{
				uint num16 = ((num10 - 3 >= 0) ? p0[num10 - 3] : 0u);
				num15 = (num14 << 32 + num5) | ((ulong)num12 << num5) | ((ulong)num13 >> 32 - num5);
				p6 = ((num10 - 2 >= 0) ? (num13 << num5) : 0) | (num16 >> 32 - num5);
			}
			ulong num17 = num15 / num6;
			ulong num18 = num15 % num6;
			if (num17 >= uint.MaxValue)
			{
				num18 += (num17 - uint.MaxValue) * num6;
				num17 = 4294967295uL;
			}
			while (num18 <= uint.MaxValue && num17 * num7 > uocgn((uint)num18, p6))
			{
				num18 += num6;
				num17--;
			}
			int num20;
			int num21;
			ulong num19;
			if (num17 != 0)
			{
				num19 = 0uL;
				num20 = num9;
				num21 = 0;
				if (num21 != 0)
				{
					goto IL_01b1;
				}
				goto IL_01fb;
			}
			goto IL_024e;
			IL_0218:
			int num22;
			num19 += (ulong)((long)p2[num22] + (long)p0[num9 + num22]);
			p0[p5 + num22] = (uint)num19;
			num19 >>= 32;
			num22++;
			goto IL_0243;
			IL_01b1:
			num19 += num17 * p2[num21];
			uint num23 = (uint)num19;
			num19 >>= 32;
			if (p0[num20] < num23)
			{
				num19++;
			}
			p0[num20] -= num23;
			num20++;
			num21++;
			goto IL_01fb;
			IL_024e:
			if (p4 != null && 0 == 0)
			{
				p4[num9] = (uint)num17;
			}
			num9--;
			num10--;
			continue;
			IL_0248:
			p1 = num9 + p3;
			goto IL_024e;
			IL_0243:
			if (num22 < p3)
			{
				goto IL_0218;
			}
			goto IL_0248;
			IL_01fb:
			if (num21 < p3)
			{
				goto IL_01b1;
			}
			if (num19 > num14)
			{
				num17--;
				num19 = 0uL;
				num22 = 0;
				if (num22 != 0)
				{
					goto IL_0218;
				}
				goto IL_0243;
			}
			goto IL_0248;
		}
		p1 = p3;
	}

	private static ulong uocgn(uint p0, uint p1)
	{
		return ((ulong)p0 << 32) | p1;
	}

	private static int royph(uint[] p0, int p1, uint[] p2, int p3)
	{
		int num = p1;
		int num2 = p3;
		while (num >= 0 && num2 >= 0)
		{
			uint num3 = p0[num];
			uint num4 = p2[num2];
			if (num3 < num4)
			{
				return -1;
			}
			if (num3 > num4)
			{
				return 1;
			}
			num--;
			num2--;
		}
		return 0;
	}
}
