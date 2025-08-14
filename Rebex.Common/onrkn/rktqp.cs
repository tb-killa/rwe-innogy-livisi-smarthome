using System.Security.Cryptography;

namespace onrkn;

internal class rktqp : jgsra
{
	public rktqp(ICryptoTransform encryptor)
		: base(encryptor, 8, riucd.zgtcb)
	{
	}

	protected override void ctcwj(byte[] p0, byte[] p1)
	{
		tyokl();
		zxtxa(0, p0[0], uubgu, 0);
		int num = 1;
		if (num == 0)
		{
			goto IL_0019;
		}
		goto IL_0041;
		IL_0019:
		hqlwz(cjdxx, aydyb(num, p0[num]), uubgu, 0, uubgu, 0);
		num++;
		goto IL_0041;
		IL_0041:
		if (num < 15)
		{
			goto IL_0019;
		}
		hqlwz(cjdxx, aydyb(15, p0[15]), uubgu, 0, p1, 0);
	}
}
