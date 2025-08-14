using System.Collections.Generic;
using System.Net;
using System.Text;

namespace WebServerHost.Web.Http;

public class ShcRestResponse : ShcWebResponse
{
	private string jsonContent;

	public ShcRestResponse(HttpStatusCode statusCode, string jsonContent)
	{
		base.StatusCode = statusCode;
		SetHeader("Cache-Control", "no-store");
		SetHeader("Access-Control-Allow-Origin", "*");
		SetHeader("Access-Control-Allow-Methods", HttpMethod.AllowedMethods);
		SetHeader("Allow", HttpMethod.AllowedMethods);
		SetHeader("Access-Control-Expose-Headers", "Request-Context");
		SetHeader("Content-Type", "application/json;charset=utf-8");
		this.jsonContent = jsonContent;
		SetHeader("Content-Length", Encoding.UTF8.GetByteCount(this.jsonContent).ToString());
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
		stringBuilder.Append("\r\n");
		if (!string.IsNullOrEmpty(jsonContent))
		{
			stringBuilder.Append(jsonContent);
		}
		return Encoding.UTF8.GetBytes(stringBuilder.ToString());
	}
}
