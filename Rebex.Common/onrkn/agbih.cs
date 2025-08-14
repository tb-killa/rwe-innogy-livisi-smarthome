using System;
using System.IO;
using System.Net;
using Rebex;

namespace onrkn;

internal static class agbih
{
	private static Uri xnoax(string p0, awngk p1, string p2)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("url");
		}
		if (p0.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("String cannot have zero length.", "url");
		}
		try
		{
			Uri uri = new Uri(p0);
			if (uri.Scheme.Equals("http", StringComparison.OrdinalIgnoreCase) && 0 == 0)
			{
				return uri;
			}
		}
		catch (FormatException ex)
		{
			p1.byfnx(LogLevel.Error, p2, "Unable to parse URL '{0}': {1}", p0, ex);
		}
		catch (ArgumentException ex2)
		{
			p1.byfnx(LogLevel.Error, p2, "Unable to parse URL '{0}': {1}", p0, ex2);
		}
		return null;
	}

	public static Stream tqzhi(string p0, awngk p1, string p2)
	{
		return mmrkx(p0, (byte[])null, p1, p2, (hcqmh<string, string>[])null);
	}

	public static Stream mmrkx(string p0, byte[] p1, awngk p2, string p3, params hcqmh<string, string>[] p4)
	{
		Uri uri = xnoax(p0, p2, p3);
		if (uri == null && 0 == 0)
		{
			return null;
		}
		try
		{
			string text = uri.AbsoluteUri;
			string text2 = text;
			int num = 0;
			while (true)
			{
				vqmjt vqmjt2 = new vqmjt(uri);
				try
				{
					vqmjt2.pezdy(p2);
					vqmjt2.pjvho = 7000;
					vqmjt2.nplxx();
					vqmjt2.qzlvp = "close";
					if (p1 == null || 1 == 0)
					{
						vqmjt2.mzzkd(uri.PathAndQuery);
					}
					else
					{
						vqmjt2.abooq("POST", uri.PathAndQuery, null);
						vqmjt2.swlkd(new MemoryStream(p1), p1: false, p1.Length, null);
					}
					int num2;
					if (p4 != null && 0 == 0)
					{
						num2 = 0;
						if (num2 != 0)
						{
							goto IL_00ba;
						}
						goto IL_00e1;
					}
					goto IL_00e9;
					IL_00e9:
					thths thths2 = vqmjt2.kvanb();
					if (thths2.xgkmt >= HttpStatusCode.MultipleChoices && thths2.xgkmt <= (HttpStatusCode)399)
					{
						p2.byfnx(LogLevel.Debug, p3, "Received HTTP status '{0}' for '{1}'.", (int)thths2.xgkmt, text);
						if (num > 10)
						{
							p2.byfnx(LogLevel.Error, p3, "Too many redirects.");
							return null;
						}
						text2 = text;
						text = thths2.virwn["Location"];
						p2.byfnx(LogLevel.Debug, p3, "New location for '{0}' is: '{1}'.", text2, text);
						if ((string.IsNullOrEmpty(text) ? true : false) || text.Equals(text2, StringComparison.Ordinal))
						{
							return null;
						}
						uri = xnoax(text, p2, p3);
						if (uri == null && 0 == 0)
						{
							return null;
						}
						num++;
						continue;
					}
					if (thths2.xgkmt != HttpStatusCode.OK)
					{
						return null;
					}
					Stream stream = thths2.vhkfm();
					try
					{
						MemoryStream memoryStream = new MemoryStream();
						stream.alskc(memoryStream);
						memoryStream.Position = 0L;
						return memoryStream;
					}
					finally
					{
						if (stream != null && 0 == 0)
						{
							((IDisposable)stream).Dispose();
						}
					}
					IL_00ba:
					hcqmh<string, string> hcqmh2 = p4[num2];
					vqmjt2.isvcv.imhki(hcqmh2.amanf, hcqmh2.cdois);
					num2++;
					goto IL_00e1;
					IL_00e1:
					if (num2 < p4.Length)
					{
						goto IL_00ba;
					}
					goto IL_00e9;
				}
				finally
				{
					if (vqmjt2 != null && 0 == 0)
					{
						((IDisposable)vqmjt2).Dispose();
					}
				}
			}
		}
		catch (ksggh ksggh2)
		{
			p2.byfnx(LogLevel.Error, p3, "Unable to download content of '{0}': {1}", p0, ksggh2);
			return null;
		}
		catch (ujepc ujepc2)
		{
			p2.byfnx(LogLevel.Error, p3, "Unable to download content of '{0}': {1}", p0, ujepc2);
			return null;
		}
	}
}
