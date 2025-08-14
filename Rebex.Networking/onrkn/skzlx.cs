using System;
using Rebex.Net;

namespace onrkn;

internal class skzlx : qntin
{
	private readonly gajry nlooz;

	private long ushym;

	public skzlx(gajry decryptor, byte[] salt)
		: base(salt)
	{
		nlooz = decryptor;
	}

	public override int bvfhg(byte[] p0, int p1)
	{
		nlooz.tglzc(p0, p1 - 16, 16);
		Array.Copy(p0, 5, xudys, 4, 8);
		p1 -= 24;
		p0[3] = (byte)(p1 - 5 >> 8);
		p0[4] = (byte)((p1 - 5) & 0xFF);
		Array.Copy(p0, 0, ueiwb, 8, 5);
		jlfbq.emxnl(ueiwb, 0, ushym);
		ushym++;
		nlooz.yirig(xudys);
		nlooz.seoke(ueiwb);
		try
		{
			int num = nlooz.yajzn(p0, 13, p1 - 5, p0, 5);
			return num + 5;
		}
		catch (Exception inner)
		{
			throw new TlsException(mjddr.wdkjl, inner);
		}
	}

	public override void egphd()
	{
		nlooz.Dispose();
	}
}
