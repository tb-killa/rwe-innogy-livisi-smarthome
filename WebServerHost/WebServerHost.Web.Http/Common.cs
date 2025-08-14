using System.Collections.Generic;
using System.Net;
using System.Text;

namespace WebServerHost.Web.Http;

public static class Common
{
	public const string Http_1_1 = "HTTP/1.1";

	public const string Http_2 = "HTTP/2";

	public static Dictionary<string, string> MimeTypes = new Dictionary<string, string>
	{
		{ ".htm", "text/html" },
		{ ".html", "text/html" },
		{ ".png", "image/png" },
		{ ".jpg", "image/jpg" },
		{ ".gif", "image/gif" },
		{ ".bmp", "image/bmp" },
		{ ".ico", "image/vnd.microsoft.icon" },
		{ ".cgi", "text/html" },
		{ ".js", "text/javascript" },
		{ ".svg", "image/svg+xml" },
		{ ".css", "text/css" },
		{ ".eot", "application/vnd.ms-fontobject" },
		{ ".ttf", "font/ttf" },
		{ ".woff", "font/woff" },
		{ ".woff2", "font/woff2" },
		{ ".plist", "application/x-plist" },
		{ ".xml", "text/xml" },
		{ ".htc", "text/x-component" },
		{ ".json", "application/json" }
	};

	public static string GetMimeType(string extension)
	{
		string value = null;
		MimeTypes.TryGetValue(extension.ToLower(), out value);
		return value;
	}

	public static string GetDescription(HttpStatusCode statusCode)
	{
		StringBuilder stringBuilder = new StringBuilder().Append((int)statusCode).Append(' ');
		if (statusCode == HttpStatusCode.OK)
		{
			stringBuilder.Append(statusCode.ToString());
		}
		else
		{
			string text = statusCode.ToString();
			foreach (char c in text)
			{
				if (char.IsUpper(c))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append(c);
			}
		}
		return stringBuilder.ToString();
	}
}
