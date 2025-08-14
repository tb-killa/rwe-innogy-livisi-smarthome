using System;
using System.IO;
using System.Text;
using onrkn;

namespace Rebex.Mime.Headers;

public class ContentLocation : IHeader
{
	private readonly string ahvsz;

	public string Location => ahvsz;

	public ContentLocation(string location)
		: this(kgbvh.nzgih(location, "location"), checkUrl: true)
	{
	}

	private ContentLocation(string location, bool checkUrl)
	{
		if (location == null || 1 == 0)
		{
			throw new ArgumentNullException("location");
		}
		if (checkUrl && 0 == 0)
		{
			kgbvh.zgeyl(location, "location", p2: false);
		}
		ahvsz = location;
	}

	public IHeader Clone()
	{
		return new ContentLocation(ahvsz, checkUrl: false);
	}

	public override string ToString()
	{
		return ahvsz;
	}

	public void Encode(TextWriter writer)
	{
		if (writer == null || 1 == 0)
		{
			throw new ArgumentNullException("writer");
		}
		rllhn.soadw(writer, ahvsz, p2: true, p3: false, 256);
	}

	public override int GetHashCode()
	{
		return ahvsz.GetHashCode();
	}

	public override bool Equals(object obj)
	{
		if (!(obj is ContentLocation contentLocation) || 1 == 0)
		{
			return false;
		}
		return ahvsz.Equals(contentLocation.ahvsz);
	}

	internal static IHeader yibms(stzvh p0)
	{
		StringBuilder stringBuilder = new StringBuilder();
		p0.hdpha();
		while (!p0.zsywy)
		{
			char c = p0.pfdcf();
			switch (c)
			{
			default:
				stringBuilder.Append(c);
				continue;
			case '\t':
				continue;
			case '(':
				break;
			}
			break;
		}
		string p1 = stringBuilder.ToString();
		p1 = kgbvh.ttsbq(p1);
		if (p1.Length > 2)
		{
			char c2 = p1[0];
			char c3 = p1[p1.Length - 1];
			if ((c2 == '"' && c3 == '"') || (c2 == '<' && c3 == '>'))
			{
				p1 = p1.Substring(1, p1.Length - 2);
			}
		}
		stringBuilder = new StringBuilder();
		int num = 0;
		if (num != 0)
		{
			goto IL_00a3;
		}
		goto IL_00e2;
		IL_00a3:
		int num2 = p1[num];
		if (num2 <= 32 || num2 == 127)
		{
			stringBuilder.dlvlk("%{0:X2}", num2);
		}
		else
		{
			stringBuilder.Append(p1[num]);
		}
		num++;
		goto IL_00e2;
		IL_00e2:
		if (num < p1.Length)
		{
			goto IL_00a3;
		}
		return new ContentLocation(stringBuilder.ToString(), checkUrl: false);
	}
}
