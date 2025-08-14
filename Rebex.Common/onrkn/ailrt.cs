using System;

namespace onrkn;

internal class ailrt
{
	private bool xxwvt;

	private ailrt()
	{
	}

	public static ailrt dvgbg()
	{
		return new ailrt();
	}

	private void zwvay()
	{
		if (xxwvt && 0 == 0)
		{
			throw new InvalidOperationException("Operation already completed.");
		}
	}

	public void uavni<T>(bool p0, Action<T> p1, T p2)
	{
		zwvay();
		xxwvt = true;
		p1(p2);
	}

	public void icidn<T>(Action<T> p0, T p1)
	{
		zwvay();
		p0(p1);
	}

	public void lgcuq<T>(Action<T> p0, T p1)
	{
		zwvay();
		p0(p1);
	}
}
