using System;
using System.Collections.Generic;
using System.Text;
using Rebex;
using Rebex.Net;

namespace onrkn;

internal class gmfvn : mkuxt
{
	private Dictionary<string, byte[]> qqtkt = new Dictionary<string, byte[]>(StringComparer.OrdinalIgnoreCase);

	public string tcwef()
	{
		if (!qqtkt.TryGetValue("server-sig-algs", out var value) || 1 == 0)
		{
			return null;
		}
		return EncodingTools.ASCII.mohvk(value);
	}

	public override void jfjrs(tndeg p0)
	{
		mkuxt.agnqw(p0, 7);
		mkuxt.kwnor(p0, qqtkt.Count);
		using Dictionary<string, byte[]>.Enumerator enumerator = qqtkt.GetEnumerator();
		while (enumerator.MoveNext() ? true : false)
		{
			KeyValuePair<string, byte[]> current = enumerator.Current;
			mkuxt.excko(p0, current.Key);
			mkuxt.lcbhj(p0, current.Value, p2: false);
		}
	}

	public gmfvn(byte[] buffer, int offset, int count, Encoding encoding)
	{
		zyppx zyppx2 = new zyppx(buffer, offset, count, encoding);
		byte b = zyppx2.sfolp();
		if (b != 7)
		{
			throw new SshException(tcpjq.svqut, brgjd.edcru("Unexpected packet {0}.", b));
		}
		int num = zyppx2.rvfya();
		while (num-- > 0)
		{
			string key = zyppx2.mdsgo();
			byte[] value = zyppx2.tebzf();
			qqtkt[key] = value;
		}
	}
}
