using System;
using System.IO;

namespace onrkn;

internal class rxjtj
{
	private readonly qwrgb lyujm;

	private readonly byte[] bmldf;

	private readonly byte[] roopn;

	public rxjtj(qwrgb deriver, byte[] H, byte[] session_id)
	{
		lyujm = deriver;
		bmldf = H;
		roopn = session_id;
	}

	public byte[] ekaai(char p0, int p1)
	{
		if (p1 == 0 || 1 == 0)
		{
			return new byte[0];
		}
		MemoryStream memoryStream = new MemoryStream();
		memoryStream.Write(bmldf, 0, bmldf.Length);
		memoryStream.WriteByte((byte)p0);
		memoryStream.Write(roopn, 0, roopn.Length);
		byte[] array = lyujm.zhupj(null, memoryStream.ToArray());
		memoryStream.SetLength(bmldf.Length);
		byte[] array2 = new byte[p1];
		int num = 0;
		while (true)
		{
			int num2 = Math.Min(array.Length, p1 - num);
			Array.Copy(array, 0, array2, num, num2);
			num += num2;
			if (num == p1)
			{
				break;
			}
			memoryStream.Write(array, 0, array.Length);
			array = lyujm.zhupj(null, memoryStream.ToArray());
		}
		return array2;
	}
}
