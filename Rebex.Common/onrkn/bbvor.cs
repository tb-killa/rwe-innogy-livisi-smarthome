using System;

namespace onrkn;

internal class bbvor : mggni, jghfk, cyhjf, gwbla, IDisposable, maowd
{
	private readonly mggni yjfss;

	private ArraySegment<byte> dwzre;

	public bbvor(mggni inner, ArraySegment<byte> prebuffered)
	{
		yjfss = inner;
		dwzre = prebuffered;
	}

	public void Dispose()
	{
		yjfss.Dispose();
	}

	public exkzi jhbpr()
	{
		return yjfss.jhbpr();
	}

	public njvzu<int> rhjom(ArraySegment<byte> p0)
	{
		if (dwzre.Count > 0)
		{
			int num = Math.Min(dwzre.Count, p0.Count);
			Array.Copy(dwzre.Array, dwzre.Offset, p0.Array, p0.Offset, num);
			dwzre = new ArraySegment<byte>(dwzre.Array, dwzre.Offset + num, dwzre.Count - num);
			return rxpjc.caxut(num);
		}
		return yjfss.rhjom(p0);
	}

	public njvzu<int> razzy(ArraySegment<byte> p0)
	{
		return yjfss.razzy(p0);
	}

	public exkzi qxxgh()
	{
		return yjfss.qxxgh();
	}
}
