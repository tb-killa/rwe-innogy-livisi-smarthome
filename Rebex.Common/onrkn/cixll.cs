using System;

namespace onrkn;

internal abstract class cixll : samhn
{
	public bool zuofh
	{
		get
		{
			IntPtr intPtr = weojx(0);
			IntPtr intPtr2 = weojx(IntPtr.Size);
			if (intPtr == IntPtr.Zero && 0 == 0)
			{
				return intPtr2 == IntPtr.Zero;
			}
			return false;
		}
	}

	public cixll()
		: base(IntPtr.Size * 2)
	{
		zhtek();
	}

	public void zhtek()
	{
		qurik(0, IntPtr.Zero);
		qurik(IntPtr.Size, IntPtr.Zero);
	}
}
