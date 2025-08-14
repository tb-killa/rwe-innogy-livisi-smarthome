using System;

namespace onrkn;

internal struct sgdyb
{
	private readonly byte[] lbtpx;

	public uint this[int index]
	{
		get
		{
			int num = index * 4;
			int num2 = lbtpx.Length;
			return (uint)((lbtpx[num % num2] << 24) | (lbtpx[(num + 1) % num2] << 16) | (lbtpx[(num + 2) % num2] << 8) | lbtpx[(num + 3) % num2]);
		}
	}

	public sgdyb(byte[] array)
	{
		if (array == null || 1 == 0)
		{
			throw new ArgumentNullException("array");
		}
		if (array.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("Array cannot be empty");
		}
		lbtpx = array;
	}
}
