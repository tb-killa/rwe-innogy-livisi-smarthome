using System;
using System.IO;
using System.Text;
using onrkn;

namespace Rebex.Mime.Headers;

public class ListCommandUrl : IHeader
{
	private readonly string hetwt;

	public string Url => hetwt;

	public ListCommandUrl(Uri url)
	{
		if (url == null && 0 == 0)
		{
			throw new ArgumentNullException("url");
		}
		string absoluteUri = url.AbsoluteUri;
		kgbvh.zgeyl(absoluteUri, "url", p2: false);
		hetwt = absoluteUri;
	}

	public ListCommandUrl(string url)
		: this(url, check: true)
	{
	}

	private ListCommandUrl(string url, bool check)
	{
		if (check && 0 == 0)
		{
			if (url == null || 1 == 0)
			{
				throw new ArgumentNullException("url");
			}
			kgbvh.zgeyl(url, "url", p2: false);
		}
		hetwt = url;
	}

	public static implicit operator ListCommandUrl(Uri url)
	{
		return new ListCommandUrl(url);
	}

	public IHeader Clone()
	{
		return new ListCommandUrl(hetwt, check: false);
	}

	public override string ToString()
	{
		return hetwt;
	}

	public void Encode(TextWriter writer)
	{
		if (writer == null || 1 == 0)
		{
			throw new ArgumentNullException("writer");
		}
		writer.Write("<" + hetwt + ">");
	}

	internal static ListCommandUrl jvaot(stzvh p0)
	{
		p0.hdpha();
		StringBuilder stringBuilder = new StringBuilder();
		while (!p0.zsywy)
		{
			char c = p0.pfdcf();
			if (c == '>')
			{
				break;
			}
			if (c != '\t')
			{
				if (c == '<')
				{
					stringBuilder.Length = 0;
				}
				else if (c <= ' ' || c == '\u007f')
				{
					stringBuilder.dlvlk("%{0:X2}", (byte)c);
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
		}
		return new ListCommandUrl(stringBuilder.ToString(), check: false);
	}
}
