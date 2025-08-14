using System;
using System.IO;
using onrkn;

namespace Rebex.Mime.Headers;

public class Unparsed : IHeader
{
	private readonly string aqhwz;

	public Unparsed(string value)
		: this(value, checkData: true)
	{
	}

	private Unparsed(string value, bool checkData)
	{
		if (value == null || 1 == 0)
		{
			throw new ArgumentNullException("value");
		}
		if (checkData && 0 == 0)
		{
			kgbvh.hdlkr(value, "value", p2: true);
		}
		aqhwz = value;
	}

	public IHeader Clone()
	{
		return new Unparsed(aqhwz, checkData: false);
	}

	public override string ToString()
	{
		return aqhwz;
	}

	public void Encode(TextWriter writer)
	{
		if (writer == null || 1 == 0)
		{
			throw new ArgumentNullException("writer");
		}
		rllhn.bnpgt(writer, aqhwz, 256);
	}

	internal static IHeader kkzpk(stzvh p0)
	{
		string value = rllhn.jyteb(p0.lxjgt);
		return new Unparsed(value, checkData: false);
	}
}
