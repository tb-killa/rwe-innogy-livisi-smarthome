using System;
using System.Security.Cryptography;

namespace onrkn;

internal abstract class jgsra : riucd
{
	protected delegate void bbeku(byte[] X, int offsetX, byte[] output, int offset);

	protected byte[] cjdxx;

	protected readonly int aqgxn;

	protected readonly int jbvqf;

	protected readonly int vgiim;

	protected readonly int skypy;

	protected readonly bbeku xqdis;

	protected jgsra(ICryptoTransform encryptor, int M0_table_bits, bbeku multN)
		: base(encryptor)
	{
		aqgxn = M0_table_bits;
		jbvqf = 128 / M0_table_bits;
		vgiim = 1 << M0_table_bits;
		skypy = M0_table_bits + 4;
		xqdis = multN;
	}

	protected void tyokl()
	{
		if (cjdxx != null && 0 == 0)
		{
			return;
		}
		cjdxx = new byte[jbvqf * vgiim * 16];
		int num = 0;
		if (num != 0)
		{
			goto IL_0037;
		}
		goto IL_0138;
		IL_00cd:
		int num2 = 2;
		if (num2 == 0)
		{
			goto IL_00d2;
		}
		goto IL_011d;
		IL_0138:
		if (num >= jbvqf)
		{
			return;
		}
		goto IL_0037;
		IL_0037:
		if (num == 0 || 1 == 0)
		{
			mdeev(aqrum, 0, num, vgiim / 2);
			for (int num3 = vgiim / 4; num3 > 0; num3 >>= 1)
			{
				riucd.nagjg(cjdxx, aydyb(num, num3 << 1), cjdxx, aydyb(num, num3));
			}
			goto IL_00cd;
		}
		int num4 = 1;
		if (num4 == 0)
		{
			goto IL_0095;
		}
		goto IL_00c2;
		IL_00d9:
		int num5;
		jlfbq.cfvhy(cjdxx, aydyb(num, num2), cjdxx, aydyb(num, num5), cjdxx, aydyb(num, num2 + num5), 16);
		num5++;
		goto IL_0114;
		IL_0114:
		if (num5 < num2)
		{
			goto IL_00d9;
		}
		num2 <<= 1;
		goto IL_011d;
		IL_0095:
		xqdis(cjdxx, aydyb(num - 1, num4), cjdxx, aydyb(num, num4));
		num4 <<= 1;
		goto IL_00c2;
		IL_00c2:
		if (num4 <= vgiim / 2)
		{
			goto IL_0095;
		}
		goto IL_00cd;
		IL_00d2:
		num5 = 1;
		if (num5 == 0)
		{
			goto IL_00d9;
		}
		goto IL_0114;
		IL_011d:
		if (num2 <= vgiim / 2)
		{
			goto IL_00d2;
		}
		num++;
		goto IL_0138;
	}

	protected int aydyb(int p0, int p1)
	{
		return (p0 << skypy) + (p1 << 4);
	}

	protected void mdeev(byte[] p0, int p1, int p2, int p3)
	{
		Buffer.BlockCopy(p0, p1, cjdxx, aydyb(p2, p3), 16);
	}

	protected void zxtxa(int p0, int p1, byte[] p2, int p3)
	{
		Buffer.BlockCopy(cjdxx, aydyb(p0, p1), p2, p3, 16);
	}

	protected override void bevpu(bool p0)
	{
		if (p0 && 0 == 0 && (!yhlms || 1 == 0) && cjdxx != null && 0 == 0)
		{
			Array.Clear(cjdxx, 0, cjdxx.Length);
			cjdxx = null;
		}
		base.bevpu(p0);
	}
}
