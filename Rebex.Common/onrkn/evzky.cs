using System;

namespace onrkn;

internal struct evzky<T0> : uentq
{
	private readonly njvzu<T0> xxbrl;

	private readonly bool wijpd;

	public bool dqcgi => xxbrl.IsCompleted;

	internal evzky(njvzu<T0> task, bool continueOnCapturedContext)
	{
		if (task == null || 1 == 0)
		{
			throw new ArgumentNullException("task");
		}
		xxbrl = task;
		wijpd = continueOnCapturedContext;
	}

	public void vcbew(Action p0)
	{
		ttius.thhob(xxbrl, p0, wijpd);
	}

	public T0 obwsj()
	{
		return xxbrl.ymczw();
	}
}
