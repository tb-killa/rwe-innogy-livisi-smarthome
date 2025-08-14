using System.Security.Cryptography;

namespace onrkn;

internal class vgnkg : jgsra
{
	public vgnkg(ICryptoTransform encryptor)
		: base(encryptor, 4, riucd.xflkz)
	{
	}

	protected override void ctcwj(byte[] p0, byte[] p1)
	{
		tyokl();
		int num = 0;
		zxtxa(num++, p0[0] >> 4, uubgu, 0);
		hqlwz(cjdxx, aydyb(num++, p0[0] & 0xF), uubgu, 0, uubgu, 0);
		int num2 = 1;
		if (num2 == 0)
		{
			goto IL_004f;
		}
		goto IL_00a8;
		IL_004f:
		hqlwz(cjdxx, aydyb(num++, p0[num2] >> 4), uubgu, 0, uubgu, 0);
		hqlwz(cjdxx, aydyb(num++, p0[num2] & 0xF), uubgu, 0, uubgu, 0);
		num2++;
		goto IL_00a8;
		IL_00a8:
		if (num2 >= 15)
		{
			hqlwz(cjdxx, aydyb(num++, p0[15] >> 4), uubgu, 0, uubgu, 0);
			hqlwz(cjdxx, aydyb(num++, p0[15] & 0xF), uubgu, 0, p1, 0);
			return;
		}
		goto IL_004f;
	}
}
