using System;
using System.Collections.Generic;
using System.Linq;

namespace onrkn;

internal class yqgve<T0>
{
	private sealed class bfkem
	{
		public string ayxqh;

		public Func<string, T0> acpts(Func<string, Func<string, T0>> p0)
		{
			return p0(ayxqh);
		}
	}

	private readonly IEnumerable<Func<string, Func<string, T0>>> xxrky;

	private readonly Func<string, Func<string, T0>> ymiex;

	private static Func<Func<string, T0>, bool> hvyua;

	private static Func<string, T0> kpkrc;

	public yqgve(IEnumerable<Func<string, Func<string, T0>>> factories)
	{
		if (factories == null || 1 == 0)
		{
			throw new ArgumentNullException("factories");
		}
		xxrky = factories.ToArray();
		ymiex = yeiic;
		ymiex = ymiex.lwhpu();
	}

	public Func<string, T0> kibze(string p0)
	{
		return ymiex(p0);
	}

	private Func<string, T0> yeiic(string p0)
	{
		bfkem bfkem = new bfkem();
		bfkem.ayxqh = p0;
		IEnumerable<Func<string, T0>> source = xxrky.Select(bfkem.acpts);
		if (hvyua == null || 1 == 0)
		{
			hvyua = lxafv;
		}
		Func<string, T0> func = source.FirstOrDefault(hvyua);
		Func<string, T0> func2 = func;
		if (func2 == null || 1 == 0)
		{
			if (kpkrc == null || 1 == 0)
			{
				kpkrc = jsohp;
			}
			func2 = kpkrc;
		}
		return func2;
	}

	private static bool lxafv(Func<string, T0> p0)
	{
		return p0 != null;
	}

	private static T0 jsohp(string p0)
	{
		return default(T0);
	}
}
