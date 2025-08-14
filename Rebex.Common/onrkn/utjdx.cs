using System;

namespace onrkn;

internal abstract class utjdx
{
	private const int vgxwz = 65521;

	private const int wfflg = 5552;

	public static uint ifqlq(uint p0, byte[] p1, int p2, int p3)
	{
		if (p3 == 0 || 1 == 0)
		{
			return p0;
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("buffer", "Buffer cannot be null.");
		}
		if (p2 < 0)
		{
			throw hifyx.nztrs("offset", p2, "Offset is negative.");
		}
		if (p3 < 0)
		{
			throw hifyx.nztrs("count", p3, "Count is negative.");
		}
		int num = p2 + p3;
		if (num > p1.Length)
		{
			throw new ArgumentException("The sum of offset and count is larger than the buffer length.");
		}
		uint num2 = p0 & 0xFFFF;
		uint num3 = (p0 >> 16) & 0xFFFF;
		while (p3 > 0)
		{
			int num4 = ((p3 < 5552) ? p3 : 5552);
			p3 -= num4;
			while (num4-- > 0)
			{
				num2 += p1[p2++];
				num3 += num2;
			}
			num2 %= 65521;
			num3 %= 65521;
		}
		return (num3 << 16) | num2;
	}
}
