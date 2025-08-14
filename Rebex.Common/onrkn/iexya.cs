using System.Security.Cryptography;

namespace onrkn;

internal class iexya : jgsra
{
	public iexya(ICryptoTransform encryptor)
		: base(encryptor, 2, riucd.etfhr)
	{
	}

	protected override void ctcwj(byte[] p0, byte[] p1)
	{
		tyokl();
		int num = 0;
		zxtxa(num++, p0[0] >> 6, uubgu, 0);
		hqlwz(cjdxx, aydyb(num++, (p0[0] >> 4) & 3), uubgu, 0, uubgu, 0);
		hqlwz(cjdxx, aydyb(num++, (p0[0] >> 2) & 3), uubgu, 0, uubgu, 0);
		hqlwz(cjdxx, aydyb(num++, p0[0] & 3), uubgu, 0, uubgu, 0);
		int num2 = 1;
		if (num2 == 0)
		{
			goto IL_00a9;
		}
		goto IL_015b;
		IL_00a9:
		hqlwz(cjdxx, aydyb(num++, (p0[num2] >> 6) & 3), uubgu, 0, uubgu, 0);
		hqlwz(cjdxx, aydyb(num++, (p0[num2] >> 4) & 3), uubgu, 0, uubgu, 0);
		hqlwz(cjdxx, aydyb(num++, (p0[num2] >> 2) & 3), uubgu, 0, uubgu, 0);
		hqlwz(cjdxx, aydyb(num++, p0[num2] & 3), uubgu, 0, uubgu, 0);
		num2++;
		goto IL_015b;
		IL_015b:
		if (num2 >= 15)
		{
			hqlwz(cjdxx, aydyb(num++, (p0[15] >> 6) & 3), uubgu, 0, uubgu, 0);
			hqlwz(cjdxx, aydyb(num++, (p0[15] >> 4) & 3), uubgu, 0, uubgu, 0);
			hqlwz(cjdxx, aydyb(num++, (p0[15] >> 2) & 3), uubgu, 0, uubgu, 0);
			hqlwz(cjdxx, aydyb(num++, p0[15] & 3), uubgu, 0, p1, 0);
			return;
		}
		goto IL_00a9;
	}
}
