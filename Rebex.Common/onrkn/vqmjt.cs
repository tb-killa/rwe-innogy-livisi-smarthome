using System;

namespace onrkn;

internal class vqmjt : udlmn
{
	public vqmjt(Uri baseUri)
		: base(baseUri)
	{
	}

	protected override phvuu dkczn(string p0, int p1, bool p2)
	{
		if (p2 && 0 == 0)
		{
			throw new NotSupportedException("HTTPS not supported.");
		}
		wuzaj wuzaj2 = new wuzaj();
		wuzaj2.ktyok(p0, p1);
		return wuzaj2;
	}
}
