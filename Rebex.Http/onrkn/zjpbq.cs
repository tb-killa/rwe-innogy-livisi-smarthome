using System;
using System.Net;

namespace onrkn;

internal static class zjpbq
{
	public static WebHeaderCollection abeze(WebHeaderCollection p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("source");
		}
		WebHeaderCollection webHeaderCollection = new WebHeaderCollection();
		webHeaderCollection.Add(p0);
		return webHeaderCollection;
	}

	public static WebException entdh(ujepc p0, WebResponse p1)
	{
		return new WebException(p0.Message, p0, (WebExceptionStatus)p0.zhmeu, p1);
	}

	public static string kzafa(Uri p0, NetworkCredential p1)
	{
		string text = "";
		if (p1 != null && 0 == 0)
		{
			string text2 = text;
			text = text2 + p1.Domain + "/" + p1.UserName + "@";
		}
		object obj = text;
		return string.Concat(obj, p0.Scheme, "://", p0.Host, ":", p0.Port);
	}

	public static bool lslyj(this Uri p0)
	{
		return p0.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase);
	}
}
