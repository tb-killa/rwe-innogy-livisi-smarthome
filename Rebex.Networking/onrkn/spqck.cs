using System;
using Rebex.Net;

namespace onrkn;

internal class spqck : tzzmw
{
	private readonly gajry urvhm;

	private long asxty;

	public spqck(gajry decryptor, byte[] salt)
		: base(salt)
	{
		urvhm = decryptor;
	}

	public override int bvfhg(byte[] p0, int p1)
	{
		urvhm.tglzc(p0, p1 - 16, 16);
		p1 -= 16;
		p0[3] = (byte)(p1 - 5 >> 8);
		p0[4] = (byte)((p1 - 5) & 0xFF);
		Array.Copy(p0, 0, lchcv, 8, 5);
		jlfbq.emxnl(lchcv, 0, asxty);
		asxty++;
		urvhm.seoke(lchcv);
		pvycf.CopyTo(urart, 0);
		int num = 0;
		if (num != 0)
		{
			goto IL_0088;
		}
		goto IL_00af;
		IL_0088:
		urart[num + 4] ^= lchcv[num];
		num++;
		goto IL_00af;
		IL_00af:
		if (num >= 8)
		{
			urvhm.yirig(urart);
			try
			{
				int num2 = urvhm.yajzn(p0, 5, p1 - 5, p0, 5);
				return num2 + 5;
			}
			catch (Exception inner)
			{
				throw new TlsException(mjddr.wdkjl, inner);
			}
		}
		goto IL_0088;
	}

	public override void egphd()
	{
		urvhm.Dispose();
	}
}
