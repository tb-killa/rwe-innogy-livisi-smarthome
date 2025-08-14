using System;
using System.Collections.Generic;

namespace onrkn;

internal class dvxgu<T0>
{
	private readonly njvzu<T0> ajzcj;

	public njvzu<T0> dioyl => ajzcj;

	public dvxgu()
	{
		ajzcj = new njvzu<T0>(gmpgj.sxstl);
	}

	public dvxgu(object state)
	{
		ajzcj = new njvzu<T0>(state, gmpgj.sxstl);
	}

	public void gxdnj()
	{
		ajzcj.ddszb(p0: true);
	}

	public bool kcuac()
	{
		return ajzcj.ddszb(p0: false);
	}

	public void dassc(Exception p0)
	{
		ajzcj.zpszo(p0, p1: true);
	}

	public void omxrj(IEnumerable<Exception> p0)
	{
		ajzcj.lyavn(p0, p1: true);
	}

	public bool ggmwx(Exception p0)
	{
		return ajzcj.zpszo(p0, p1: false);
	}

	public bool qstcu(IEnumerable<Exception> p0)
	{
		return ajzcj.lyavn(p0, p1: false);
	}

	public void cbgge(T0 p0)
	{
		ajzcj.asybg(p0, p1: true);
	}

	public bool lxwus(T0 p0)
	{
		return ajzcj.asybg(p0, p1: false);
	}
}
