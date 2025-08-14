using System;
using System.IO;
using onrkn;

namespace Rebex.Mime.Headers;

public class Unstructured : IHeader
{
	private readonly string linaa;

	public string Value => linaa;

	public Unstructured(string value)
		: this(value, checkData: true)
	{
	}

	internal Unstructured(string value, bool checkData)
	{
		if (value == null || 1 == 0)
		{
			throw new ArgumentNullException("value");
		}
		if (checkData && 0 == 0)
		{
			kgbvh.zgeyl(value, "value", p2: true);
		}
		else
		{
			value = value.Replace("\r", "").Replace("\n", "\r\n");
		}
		linaa = value;
	}

	public IHeader Clone()
	{
		return new Unstructured(linaa, checkData: false);
	}

	public override string ToString()
	{
		return linaa;
	}

	public void Encode(TextWriter writer)
	{
		if (writer == null || 1 == 0)
		{
			throw new ArgumentNullException("writer");
		}
		rllhn.soadw(writer, linaa, p2: true, p3: false, 108);
	}

	internal static IHeader seqfp(stzvh p0)
	{
		string sosem = p0.sosem;
		sosem = kgbvh.ttsbq(sosem);
		return new Unstructured(sosem, checkData: false);
	}
}
