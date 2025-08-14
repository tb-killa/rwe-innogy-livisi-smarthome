using System.Collections.Generic;
using System.Linq;

namespace WebSocketLibrary.Handlers.Handshake;

public class ReceivedHandshake
{
	private const string ServerKeyHeaderName = "Sec-WebSocket-Accept";

	private const string SetCookieHeaderName = "Set-Cookie";

	public int StatusCode { get; set; }

	public List<KeyValuePair<string, string>> Headers { get; set; }

	public string ServerKey => GetFirstValueByKey("Sec-WebSocket-Accept");

	public List<string> Cookies => GetValuesByKey("Set-Cookie");

	private string GetFirstValueByKey(string key)
	{
		if (Headers != null && Headers.Any((KeyValuePair<string, string> m) => m.Key == key))
		{
			return Headers.FirstOrDefault((KeyValuePair<string, string> m) => m.Key == key).Value;
		}
		return null;
	}

	private List<string> GetValuesByKey(string key)
	{
		List<string> list = new List<string>();
		if (Headers != null)
		{
			IEnumerable<string> collection = from m in Headers
				where m.Key == key
				select m.Value;
			list.AddRange(collection);
		}
		return list;
	}
}
