using System;
using System.IO;

namespace onrkn;

internal class eejdw : npohs
{
	private Action uxyvv;

	public eejdw(Stream baseStream, Action reaction)
		: base(baseStream)
	{
		uxyvv = reaction;
	}

	protected override void julnt()
	{
		base.julnt();
		if (uxyvv != null && 0 == 0)
		{
			uxyvv();
		}
	}
}
