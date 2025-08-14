using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace WebSocketLibrary.Common.Cookies;

public class CookiesCollection
{
	private readonly Regex cookieParserRegex = new Regex("^ *(?<name>.*?)=(?<value>.*?)(;|$)", RegexOptions.Compiled);

	private readonly List<Cookie> cookies = new List<Cookie>();

	private bool isModified;

	private string headerValue;

	public int Count => cookies.Count;

	public void AddNewCookie(Cookie cookie)
	{
		if (cookie != null)
		{
			cookies.RemoveAll((Cookie m) => m.Name == cookie.Name);
			cookies.Add(cookie);
			isModified = true;
		}
	}

	public void AddNewCookies(List<Cookie> cookies)
	{
		cookies.ForEach(AddNewCookie);
	}

	public List<Cookie> GetAllCookies()
	{
		return cookies;
	}

	public void AddCookieFromHeader(string header)
	{
		if (string.IsNullOrEmpty(header))
		{
			return;
		}
		Match match = cookieParserRegex.Match(header);
		if (match.Success)
		{
			string value = match.Groups["name"].Value;
			string value2 = match.Groups["value"].Value;
			Cookie cookie = new Cookie(value, value2);
			cookies.RemoveAll((Cookie m) => m.Name == cookie.Name);
			if (!string.IsNullOrEmpty(cookie.Value))
			{
				cookies.Add(cookie);
			}
		}
	}

	public string GetHeaderValue()
	{
		if (string.IsNullOrEmpty(headerValue) || isModified)
		{
			headerValue = GetAllCookiesHeaderValue();
			isModified = false;
		}
		return headerValue;
	}

	private string GetAllCookiesHeaderValue()
	{
		if (cookies.Count > 0)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			foreach (Cookie cookie in cookies)
			{
				if (flag)
				{
					flag = false;
				}
				else
				{
					stringBuilder.Append("; ");
				}
				stringBuilder.AppendFormat("{0}={1}", new object[2] { cookie.Name, cookie.Value });
			}
			return stringBuilder.ToString();
		}
		return string.Empty;
	}
}
