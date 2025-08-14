using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace onrkn;

internal static class uckgc
{
	private sealed class welei
	{
		public DecompressionMethods fpsoz;

		public bool ouwgv(hcqmh<DecompressionMethods, string> p0)
		{
			return fpsoz.qtclt(p0.amanf);
		}
	}

	private static hcqmh<DecompressionMethods, string>[] aeckd = new hcqmh<DecompressionMethods, string>[2]
	{
		new hcqmh<DecompressionMethods, string>(DecompressionMethods.GZip, "gzip"),
		new hcqmh<DecompressionMethods, string>(DecompressionMethods.Deflate, "deflate")
	};

	private static Func<hcqmh<DecompressionMethods, string>, string> kswqc;

	public static string nvpnh(this DecompressionMethods p0)
	{
		welei welei = new welei();
		welei.fpsoz = p0;
		IEnumerable<hcqmh<DecompressionMethods, string>> source = aeckd.Where(welei.ouwgv);
		if (kswqc == null || 1 == 0)
		{
			kswqc = ztsmz;
		}
		return string.Join(", ", source.Select(kswqc).ToArray());
	}

	public static bool qtclt(this DecompressionMethods p0, DecompressionMethods p1)
	{
		return (p0 & p1) == p1;
	}

	private static string ztsmz(hcqmh<DecompressionMethods, string> p0)
	{
		return p0.cdois;
	}
}
