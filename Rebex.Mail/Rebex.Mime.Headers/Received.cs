using System;
using System.IO;
using onrkn;

namespace Rebex.Mime.Headers;

public class Received : IHeader
{
	private readonly string qgcdt;

	public Received(string data)
		: this(data, checkData: true)
	{
	}

	private Received(string data, bool checkData)
	{
		if (data == null || 1 == 0)
		{
			throw new ArgumentNullException("data");
		}
		if (checkData && 0 == 0)
		{
			kgbvh.hdlkr(data, "data", p2: true);
		}
		qgcdt = data;
	}

	public IHeader Clone()
	{
		return new Received(qgcdt, checkData: false);
	}

	public override string ToString()
	{
		return qgcdt;
	}

	public void Encode(TextWriter writer)
	{
		if (writer == null || 1 == 0)
		{
			throw new ArgumentNullException("writer");
		}
		rllhn.bnpgt(writer, qgcdt, 76);
	}

	internal static IHeader fyzvv(stzvh p0)
	{
		string data = rllhn.jyteb(p0.lxjgt);
		return new Received(data, checkData: false);
	}
}
