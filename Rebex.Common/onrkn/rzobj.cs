using System;

namespace onrkn;

internal class rzobj<T0> : fpwmx where T0 : class
{
	private static readonly Action<T0> uusos;

	private readonly T0 bgbyt;

	private readonly Action<T0> qttcg;

	private static Action<T0> pagzz;

	protected rzobj(int initialCount, Action<T0> destroyAction = null)
		: base(initialCount)
	{
		Action<T0> action = destroyAction;
		if (action == null || 1 == 0)
		{
			action = uusos;
		}
		qttcg = action;
		bgbyt = this as T0;
		if (bgbyt == null || 1 == 0)
		{
			throw new ArgumentException("This instance should be base class for TResource.");
		}
	}

	public rzobj(int initialCount, T0 resource, Action<T0> destroyAction)
		: base(initialCount)
	{
		if (resource == null || 1 == 0)
		{
			throw new ArgumentNullException("resource");
		}
		if (destroyAction == null || 1 == 0)
		{
			throw new ArgumentNullException("destroyAction");
		}
		bgbyt = resource;
		qttcg = destroyAction;
	}

	protected override void mvhmz()
	{
		qttcg(bgbyt);
	}

	static rzobj()
	{
		if (pagzz == null || 1 == 0)
		{
			pagzz = ypccv;
		}
		uusos = pagzz;
	}

	private static void ypccv(T0 p0)
	{
	}
}
