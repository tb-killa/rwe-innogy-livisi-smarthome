using System;

namespace onrkn;

internal class rwfyw<T0>
{
	private readonly object cedia = new object();

	private readonly Func<T0> rvxfx;

	private volatile bool goexp;

	private T0 faxyr;

	private static Func<T0> dygwe;

	public bool cbgyf => goexp;

	public T0 avlfd
	{
		get
		{
			lock (cedia)
			{
				if (!goexp || 1 == 0)
				{
					faxyr = rvxfx();
					goexp = true;
				}
				return faxyr;
			}
		}
	}

	public rwfyw()
	{
		if (dygwe == null || 1 == 0)
		{
			dygwe = wepqi;
		}
		rvxfx = dygwe;
	}

	public rwfyw(Func<T0> creatorFunc)
		: this(creatorFunc, bakio.mmeyr)
	{
	}

	public rwfyw(Func<T0> creatorFunc, bakio mode)
	{
		if (creatorFunc == null || 1 == 0)
		{
			throw new ArgumentNullException("creatorFunc");
		}
		rvxfx = creatorFunc;
	}

	private static T0 wepqi()
	{
		return Activator.CreateInstance<T0>();
	}
}
