using System.Threading;

namespace onrkn;

internal static class egqnn
{
	public static void oddei(uint[] p0, uint[] p1, uint[] p2)
	{
		ulong num = 0uL;
		ulong num2 = 0uL;
		if (p1.Length < p0.Length)
		{
			p1 = Interlocked.Exchange(ref p0, p1);
		}
		int num3 = 0;
		if (num3 != 0)
		{
			goto IL_0023;
		}
		goto IL_0068;
		IL_0068:
		if (num3 >= p0.Length)
		{
			for (int i = p0.Length; i < p1.Length; i++)
			{
				int num4 = p0.Length - 1;
				int num5 = i - num4;
				while (num4 >= 0)
				{
					ulong num6 = (ulong)p0[num4] * (ulong)p1[num5];
					num += (uint)num6;
					num2 += num6 >> 32;
					num4--;
					num5++;
				}
				p2[i] = (uint)num;
				num = (num >> 32) + num2;
				num2 = 0uL;
			}
			for (int j = p1.Length; j < p1.Length + p0.Length; j++)
			{
				int num7 = p0.Length - 1;
				for (int k = j - num7; k < p1.Length; k++)
				{
					ulong num8 = (ulong)p0[num7] * (ulong)p1[k];
					num += (uint)num8;
					num2 += num8 >> 32;
					num7--;
				}
				p2[j] = (uint)num;
				num = (num >> 32) + num2;
				num2 = 0uL;
			}
			return;
		}
		goto IL_0023;
		IL_0051:
		int num9;
		if (num9 >= 0)
		{
			goto IL_002c;
		}
		p2[num3] = (uint)num;
		num = (num >> 32) + num2;
		num2 = 0uL;
		num3++;
		goto IL_0068;
		IL_0023:
		num9 = num3;
		int num10 = 0;
		if (num10 != 0)
		{
			goto IL_002c;
		}
		goto IL_0051;
		IL_002c:
		ulong num11 = (ulong)p0[num9] * (ulong)p1[num10];
		num += (uint)num11;
		num2 += num11 >> 32;
		num9--;
		num10++;
		goto IL_0051;
	}
}
