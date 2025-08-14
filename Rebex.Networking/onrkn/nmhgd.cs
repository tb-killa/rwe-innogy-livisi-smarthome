using System;
using System.IO;

namespace onrkn;

internal class nmhgd : MemoryStream
{
	public byte this[int n] => GetBuffer()[n];

	public nmhgd()
	{
	}

	public nmhgd(int capacity)
		: base(capacity)
	{
	}

	public void ejbiu(int p0)
	{
		if (p0 < 0)
		{
			throw new ArgumentException();
		}
		if (p0 > Length)
		{
			throw new ArgumentException();
		}
		int num = (int)Length - p0;
		if (num == 0 || 1 == 0)
		{
			SetLength(0L);
			return;
		}
		byte[] buffer = GetBuffer();
		Array.Copy(buffer, p0, buffer, 0, num);
		SetLength(num);
	}

	public byte[] rjmxb(int p0, int p1)
	{
		if (p0 + p1 > Length)
		{
			throw new ArgumentException();
		}
		byte[] buffer = GetBuffer();
		byte[] array = new byte[p1];
		Array.Copy(buffer, p0, array, 0, p1);
		return array;
	}

	public void nqnih()
	{
		SetLength(0L);
	}
}
