using System;
using System.Security.Cryptography;

namespace onrkn;

internal class rioxq : riucd
{
	public rioxq(ICryptoTransform encryptor)
		: base(encryptor)
	{
	}

	protected override void ctcwj(byte[] p0, byte[] p1)
	{
		cxnbe(p0, aqrum, p1);
	}

	private void cxnbe(byte[] p0, byte[] p1, byte[] p2)
	{
		int p3 = 0;
		int p4 = 128;
		if (levhn(p0, ref p3, ref p4) && 0 == 0)
		{
			Buffer.BlockCopy(p1, 0, uubgu, 0, 16);
		}
		else
		{
			Array.Clear(uubgu, 0, 16);
		}
		riucd.nagjg(p1, 0, jtste, 0);
		int num = 1;
		if (num == 0)
		{
			goto IL_0053;
		}
		goto IL_0096;
		IL_0053:
		if (levhn(p0, ref p3, ref p4) && 0 == 0)
		{
			droym(uubgu, jtste, uubgu);
		}
		riucd.nagjg(jtste, 0, jtste, 0);
		num++;
		goto IL_0096;
		IL_0096:
		if (num >= 127)
		{
			if (levhn(p0, ref p3, ref p4) && 0 == 0)
			{
				droym(uubgu, jtste, p2);
			}
			else
			{
				Buffer.BlockCopy(uubgu, 0, p2, 0, 16);
			}
			return;
		}
		goto IL_0053;
	}

	private static bool levhn(byte[] p0, ref int p1, ref int p2)
	{
		bool result = (p0[p1] & p2) != 0;
		if (p2 == 1)
		{
			p1++;
			p2 = 128;
		}
		else
		{
			p2 >>= 1;
		}
		return result;
	}
}
