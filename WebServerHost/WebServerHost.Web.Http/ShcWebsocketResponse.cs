using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace WebServerHost.Web.Http;

public class ShcWebsocketResponse : ShcWebResponse
{
	private string responseContent = string.Empty;

	public ShcWebsocketResponse(string websocketKey)
	{
		base.StatusCode = HttpStatusCode.SwitchingProtocols;
		SetHeader("Cache-Control", "private");
		SetHeader("Server", "Server");
		SetHeader("Upgrade", "websocket");
		SetHeader("Connection", "upgrade");
		string wSAccept = GetWSAccept(websocketKey);
		SetHeader("Sec-WebSocket-Accept", wSAccept);
		base.Version = "HTTP/1.1";
	}

	private string GetWSAccept(string websocketKey)
	{
		StringBuilder stringBuilder = new StringBuilder(websocketKey.Trim());
		stringBuilder.Append("258EAFA5-E914-47DA-95CA-C5AB0DC85B11");
		return Convert.ToBase64String(SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(stringBuilder.ToString())));
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
		if (!string.IsNullOrEmpty(responseContent))
		{
			stringBuilder.Append(responseContent);
		}
		return Encoding.UTF8.GetBytes(stringBuilder.ToString());
	}
}
