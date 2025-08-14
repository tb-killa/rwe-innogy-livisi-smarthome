using System;
using System.Collections.Generic;

namespace WebSocketLibrary.Handlers.Handshake;

public static class HeadersParser
{
	private const string NewLine = "\r\n";

	public static ReceivedHandshake ParseResultHandshake(string handshake)
	{
		StringToken nextLine = GetNextLine(handshake, 0);
		int statusCodeFromStatusLine = GetStatusCodeFromStatusLine(handshake, nextLine);
		ReceivedHandshake receivedHandshake = new ReceivedHandshake();
		receivedHandshake.StatusCode = statusCodeFromStatusLine;
		receivedHandshake.Headers = GetHeaders(handshake, nextLine.lastIndex + 1);
		return receivedHandshake;
	}

	private static int GetStatusCodeFromStatusLine(string handshake, StringToken token)
	{
		int num = handshake.IndexOf(' ', token.firstIndex, token.GetLength());
		int num2 = ((num > -1) ? handshake.IndexOf(' ', num + 1) : (-1));
		if (num >= 0 && num2 >= 0 && num2 > num)
		{
			return int.Parse(new StringToken(num + 1, num2 - 1).GetSubstring(handshake));
		}
		throw new ArgumentException($"Status line is invalid: |{token.GetSubstring(handshake)}|");
	}

	private static List<KeyValuePair<string, string>> GetHeaders(string handshake, int startPosition)
	{
		StringToken nextLine = GetNextLine(handshake, startPosition);
		List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
		while (nextLine.GetLength() > "\r\n".Length)
		{
			KeyValuePair<string, string> headerFromLine = GetHeaderFromLine(handshake, nextLine);
			list.Add(new KeyValuePair<string, string>(headerFromLine.Key, headerFromLine.Value));
			nextLine = GetNextLine(handshake, nextLine.lastIndex + 1);
		}
		return list;
	}

	private static StringToken GetNextLine(string text, int startPosition)
	{
		int num = text.IndexOf("\r\n", startPosition);
		int lastIndex = ((num > -1) ? (num + "\r\n".Length - 1) : (text.Length - 1));
		return new StringToken(startPosition, lastIndex);
	}

	private static KeyValuePair<string, string> GetHeaderFromLine(string handshake, StringToken lineToken)
	{
		int num = handshake.IndexOf(':', lineToken.firstIndex, lineToken.GetLength());
		if (num > -1)
		{
			StringToken stringToken = new StringToken(lineToken.firstIndex, num - 1);
			StringToken stringToken2 = new StringToken(num + 1, lineToken.lastIndex);
			string key = stringToken.GetSubstring(handshake).Trim();
			string value = stringToken2.GetSubstring(handshake).Trim();
			return new KeyValuePair<string, string>(key, value);
		}
		throw new ArgumentException($"Header parse exception: {lineToken.GetSubstring(handshake)}");
	}
}
