using System.Collections.Generic;
using System.Net;
using System.Text;

namespace WebServerHost.Web.Http;

public class ShcWebResponse
{
	private Dictionary<string, string> headers = new Dictionary<string, string>();

	public HttpStatusCode StatusCode { get; set; }

	public string Version { get; set; }

	public Dictionary<string, string> Headers => headers;

	public ShcWebResponse()
	{
		SetHeader("Cache-Control", "no-cache");
		SetHeader("Access-Control-Allow-Origin", "*");
		SetHeader("Access-Control-Allow-Methods", HttpMethod.AllowedMethods);
		SetHeader("Allow", HttpMethod.AllowedMethods);
		SetHeader("Access-Control-Expose-Headers", "Request-Context");
	}

	public virtual byte[] GetBytes()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (!string.IsNullOrEmpty(Version))
		{
			stringBuilder.Append($"{Version} {Common.GetDescription(StatusCode)}\r\n");
		}
		foreach (KeyValuePair<string, string> header in headers)
		{
			stringBuilder.Append($"{header.Key}: {header.Value}\r\n");
		}
		stringBuilder.Append("\r\n");
		return Encoding.UTF8.GetBytes(stringBuilder.ToString());
	}

	public void SetHeader(string key, string value)
	{
		headers[key] = value;
	}
}
