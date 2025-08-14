namespace onrkn;

internal class fqgoz
{
	private static ushort[] hbufd;

	private ushort djuqd;

	public ushort utymw
	{
		get
		{
			return djuqd;
		}
		set
		{
			djuqd = value;
		}
	}

	static fqgoz()
	{
		hbufd = new ushort[256];
		int num = 0;
		if (num != 0)
		{
			goto IL_0011;
		}
		goto IL_0067;
		IL_0011:
		int num2 = 0;
		int num3 = num << 8;
		int num4 = 0;
		if (num4 != 0)
		{
			goto IL_001c;
		}
		goto IL_004d;
		IL_001c:
		if (((num2 ^ num3) & 0x8000) != 0 && 0 == 0)
		{
			num2 <<= 1;
			num3 <<= 1;
			num2 ^= 0x1021;
		}
		else
		{
			num2 <<= 1;
			num3 <<= 1;
		}
		num4++;
		goto IL_004d;
		IL_0067:
		if (num >= 256)
		{
			return;
		}
		goto IL_0011;
		IL_004d:
		if (num4 < 8)
		{
			goto IL_001c;
		}
		hbufd[num] = (ushort)num2;
		num++;
		goto IL_0067;
	}

	public fqgoz()
	{
		hdiho();
	}

	public void hdiho()
	{
		djuqd = 0;
	}

	public void vhwye(byte p0)
	{
		djuqd = (ushort)((djuqd << 8) ^ hbufd[(djuqd >> 8) ^ p0]);
	}

	public void fldkw(byte[] p0, int p1, int p2)
	{
		int num = 0;
		if (num != 0)
		{
			goto IL_0006;
		}
		goto IL_0038;
		IL_0006:
		djuqd = (ushort)((djuqd << 8) ^ hbufd[(djuqd >> 8) ^ p0[p1++]]);
		num++;
		goto IL_0038;
		IL_0038:
		if (num >= p2)
		{
			return;
		}
		goto IL_0006;
	}
}
