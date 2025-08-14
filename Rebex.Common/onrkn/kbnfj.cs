using System;

namespace onrkn;

internal struct kbnfj : uentq
{
	private readonly exkzi tqitg;

	private readonly bool lmfuj;

	public bool dvgpm => tqitg.IsCompleted;

	internal kbnfj(exkzi task, bool continueOnCapturedContext)
	{
		if (task == null || 1 == 0)
		{
			throw new ArgumentNullException("task");
		}
		tqitg = task;
		lmfuj = continueOnCapturedContext;
	}

	public void vcbew(Action p0)
	{
		ttius.thhob(tqitg, p0, lmfuj);
	}

	public void hnlil()
	{
		tqitg.fdqtm();
	}
}
