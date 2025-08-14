using Rebex.Net;

namespace onrkn;

internal abstract class wvwdg : ofuit
{
	protected byte[] uqsbq;

	public byte[] rifkc => uqsbq;

	public wvwdg(nsvut type)
		: base(type)
	{
	}

	public nxtme<byte> dvlkv()
	{
		nxtme<byte> result = kqmgh(eppge.bythi);
		if (result.hvbtp && 0 == 0)
		{
			return result;
		}
		if ((((result.tvoem & 1) != 0) ? true : false) || (result[0] << 8) + result[1] + 2 != result.tvoem)
		{
			throw new TlsException(mjddr.gkkle, "Invalid extension data.");
		}
		return result;
	}

	public nxtme<byte> kqmgh(eppge p0, bool p1 = false)
	{
		if (uqsbq == null || 1 == 0)
		{
			return nxtme<byte>.gihlo;
		}
		wmbjj wmbjj2 = new wmbjj(uqsbq);
		int num = wmbjj2.tdjyr - 1;
		while (wmbjj2.hpxkw < num)
		{
			eppge eppge2 = (eppge)wmbjj2.mytfp();
			nxtme<byte> p2 = nxtme<byte>.gihlo;
			if (p1 && 0 == 0)
			{
				p2 = wmbjj2.dliku(2).liutv();
				wmbjj2.hpxkw -= 2;
			}
			int p3 = wmbjj2.mytfp();
			if (eppge2 == p0)
			{
				byte[] array = wmbjj2.dliku(p3);
				if (!p1 || 1 == 0)
				{
					return array;
				}
				return array.liutv().pyfmm(p2);
			}
			wmbjj2.xqsga(p3);
		}
		return nxtme<byte>.gihlo;
	}
}
