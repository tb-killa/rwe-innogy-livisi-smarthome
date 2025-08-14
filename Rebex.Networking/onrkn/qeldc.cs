using System;

namespace onrkn;

internal class qeldc : qntin
{
	private readonly fhryo cqwjb;

	private long hange;

	public qeldc(fhryo encryptor, byte[] salt)
		: base(salt)
	{
		cqwjb = encryptor;
	}

	public override int bvfhg(byte[] p0, int p1)
	{
		jlfbq.emxnl(ueiwb, 0, hange);
		hange++;
		Array.Copy(p0, 0, ueiwb, 8, 5);
		cqwjb.seoke(ueiwb);
		Array.Copy(ueiwb, 0, xudys, 4, 8);
		cqwjb.yirig(xudys);
		int num = 13;
		int num2 = cqwjb.yajzn(p0, 5, p1 - 5, p0, num);
		num2 += cqwjb.mdexb(p0, num + num2);
		num2 += num;
		p0[3] = (byte)(num2 - 5 >> 8);
		p0[4] = (byte)((num2 - 5) & 0xFF);
		Array.Copy(ueiwb, 0, p0, 5, 8);
		return num2;
	}

	public override void egphd()
	{
		cqwjb.Dispose();
	}
}
