using System;
using System.Collections.Generic;
using System.IO;
using WebServerHost.Web.Extensions;

namespace WebServerHost.Web.Http;

public class ShcWebRequest
{
	private int contentLength;

	private Dictionary<string, string> headers;

	private string method;

	private string requestContent;

	private string uri;

	private string protocol;

	private string queryString;

	private List<KeyValuePair<string, string>> reqParams;

	public Dictionary<string, string> Headers => headers;

	public List<KeyValuePair<string, string>> Parameters => reqParams;

	public int ContentLength => contentLength;

	public string Method => method;

	public string RequestContent => requestContent;

	public string RequestUri => uri;

	public string QueryString => queryString;

	public string ProtocolVersion => protocol;

	internal ShcWebRequest(string method, string pathLine, string conent)
	{
		queryString = "";
		this.method = method;
		uri = "";
		protocol = "HTTP 1.1";
		reqParams = new List<KeyValuePair<string, string>>();
		requestContent = "";
		headers = new Dictionary<string, string>();
		contentLength = 0;
		string[] array = pathLine.Trim().Split('?');
		uri = array[0].Trim().ToLower();
		if (array.Length > 1)
		{
			queryString = array[1].Trim();
			string[] array2 = queryString.Split('&');
			foreach (string text in array2)
			{
				if (text.Contains("="))
				{
					string[] array3 = text.Split('=');
					reqParams.Add(new KeyValuePair<string, string>(array3[0], Uri.UnescapeDataString(array3[1])));
				}
			}
		}
		requestContent = conent;
	}

	public ShcWebRequest(StreamReader streamReader)
	{
		queryString = "";
		method = "";
		uri = "";
		protocol = "";
		reqParams = new List<KeyValuePair<string, string>>();
		requestContent = string.Empty;
		headers = new Dictionary<string, string>();
		contentLength = 0;
		string text = streamReader.ReadLine();
		if (text != null)
		{
			string[] array = text.Split(' ');
			if (array.Length < 3)
			{
				throw new ArgumentException("Invalid http request");
			}
			method = array[0].Trim();
			string[] array2 = array[1].Trim().Split('?');
			uri = array2[0].Trim().ToLower();
			if (array2.Length > 1)
			{
				queryString = array2[1].Trim();
				string[] array3 = queryString.Split('&');
				foreach (string text2 in array3)
				{
					if (text2.Contains("="))
					{
						string[] array4 = text2.SplitBy('=');
						reqParams.Add(new KeyValuePair<string, string>(array4[0], Uri.UnescapeDataString(array4[1])));
					}
				}
			}
			protocol = array[2].Trim();
			string text3 = streamReader.ReadLine();
			do
			{
				string text4 = text3;
				text3 = streamReader.ReadLine();
				while (text3.Length > 0 && (text3[0] == ' ' || text3[0] == '\t'))
				{
					text4 += text3.Trim();
					text3 = streamReader.ReadLine();
				}
				int num = text4.IndexOf(':');
				if (num != -1)
				{
					string key = text4.Substring(0, num).ToLower();
					string value = text4.Substring(num + 1).Trim();
					headers[key] = value;
				}
			}
			while (text3.Trim() != string.Empty);
			string value2 = string.Empty;
			if (headers.TryGetValue("content-length", out value2))
			{
				ReadContent(streamReader);
			}
			else
			{
				requestContent = string.Empty;
			}
			return;
		}
		throw new ArgumentException("Invalid http request");
	}

	public void ReadContent(StreamReader streamReader)
	{
		try
		{
			contentLength = int.Parse(headers["content-length"]);
			char[] array = new char[contentLength];
			int i;
			int num;
			for (i = 0; i < contentLength; i += num)
			{
				num = streamReader.Read(array, i, contentLength - i);
				if (num <= 0)
				{
					break;
				}
			}
			requestContent = new string(array, 0, i);
		}
		catch (Exception)
		{
		}
	}

	public string GetHeaderValue(string header)
	{
		string value = string.Empty;
		headers.TryGetValue(header.ToLower(), out value);
		return value;
	}

	public List<KeyValuePair<string, string>> GetRequestParamsByKey(string key)
	{
		return reqParams.FindAll((KeyValuePair<string, string> p) => p.Key == key);
	}
}
