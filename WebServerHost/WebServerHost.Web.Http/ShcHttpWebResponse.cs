using System.Collections.Generic;
using System.Net;
using System.Text;

namespace WebServerHost.Web.Http;

public class ShcHttpWebResponse : ShcWebResponse
{
	private string content;

	public ShcHttpWebResponse(HttpStatusCode statusCode, string server, string redirectUrl, string content)
	{
		base.StatusCode = statusCode;
		SetHeader("Accept-Ranges", "bytes");
		SetHeader("Server", server);
		SetHeader("Content-Type", Common.GetMimeType(".htm"));
		SetHeader("Content-Length", (content != null) ? Encoding.UTF8.GetByteCount(content).ToString() : "0");
		if (!string.IsNullOrEmpty(redirectUrl))
		{
			SetHeader("Location", redirectUrl);
		}
		this.content = content;
		base.Version = "HTTP/1.1";
	}

	public override byte[] GetBytes()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append($"{base.Version} {Common.GetDescription(base.StatusCode)}\r\n");
		foreach (KeyValuePair<string, string> header in base.Headers)
		{
			stringBuilder.Append($"{header.Key}: {header.Value}\r\n");
		}
		if (!base.Headers.ContainsKey("Content-Type"))
		{
			SetHeader("Content-Type", Common.GetMimeType("*.htm"));
		}
		stringBuilder.Append("\r\n");
		if (!string.IsNullOrEmpty(content))
		{
			stringBuilder.Append(content);
		}
		return Encoding.UTF8.GetBytes(stringBuilder.ToString());
	}
}
