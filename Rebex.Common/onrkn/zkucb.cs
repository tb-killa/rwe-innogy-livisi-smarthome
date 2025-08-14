namespace onrkn;

internal static class zkucb
{
	public static void tnhun(uint[] p0, uint p1, out uint p2)
	{
		ulong num = p1;
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_0009;
		}
		goto IL_001e;
		IL_0009:
		num += p0[num2];
		p0[num2] = (uint)num;
		num >>= 32;
		num2++;
		goto IL_001e;
		IL_001e:
		if (num2 < p0.Length && num != 0)
		{
			goto IL_0009;
		}
		p2 = (uint)num;
	}

	public static void bkikk(uint[] p0, uint[] p1)
	{
		ulong num = 0uL;
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_0009;
		}
		goto IL_0026;
		IL_0009:
		num += (ulong)((long)p0[num2] + (long)p1[num2]);
		p0[num2] = (uint)(0xFFFFFFFFu & num);
		num >>= 32;
		num2++;
		goto IL_0026;
		IL_0026:
		if (num2 >= p1.Length)
		{
			if (num == 0)
			{
				return;
			}
			for (int i = p1.Length; i < p0.Length; i++)
			{
				if (num == 0)
				{
					break;
				}
				num += p0[i];
				p0[i] = (uint)num;
				num >>= 32;
			}
			return;
		}
		goto IL_0009;
	}
}
