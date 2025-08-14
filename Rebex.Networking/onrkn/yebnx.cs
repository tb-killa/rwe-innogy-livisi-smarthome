using System;

namespace onrkn;

internal class yebnx : tzzmw
{
	private readonly fhryo nibhu;

	private long mucag;

	public yebnx(fhryo encryptor, byte[] salt)
		: base(salt)
	{
		nibhu = encryptor;
	}

	public override int bvfhg(byte[] p0, int p1)
	{
		jlfbq.emxnl(lchcv, 0, mucag);
		mucag++;
		Array.Copy(p0, 0, lchcv, 8, 5);
		nibhu.seoke(lchcv);
		pvycf.CopyTo(urart, 0);
		int num = 0;
		if (num != 0)
		{
			goto IL_0058;
		}
		goto IL_007f;
		IL_0058:
		urart[num + 4] ^= lchcv[num];
		num++;
		goto IL_007f;
		IL_007f:
		if (num >= 8)
		{
			nibhu.yirig(urart);
			int num2 = 5;
			int num3 = nibhu.yajzn(p0, 5, p1 - 5, p0, num2);
			num3 += nibhu.mdexb(p0, num2 + num3);
			num3 += num2;
			p0[3] = (byte)(num3 - 5 >> 8);
			p0[4] = (byte)((num3 - 5) & 0xFF);
			return num3;
		}
		goto IL_0058;
	}

	public override void egphd()
	{
		nibhu.Dispose();
	}
}
