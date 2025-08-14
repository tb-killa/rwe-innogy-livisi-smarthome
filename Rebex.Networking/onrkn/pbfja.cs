using System.Text;
using Rebex.Net;

namespace onrkn;

internal class pbfja : mkuxt
{
	private uint nxykx;

	private string iptki;

	private string ixymt;

	public tcpjq bgjgq
	{
		get
		{
			if (nxykx < 1 || nxykx > 15)
			{
				return tcpjq.yeheh;
			}
			return (tcpjq)nxykx;
		}
	}

	public string psojj => iptki;

	public override void jfjrs(tndeg p0)
	{
		mkuxt.agnqw(p0, 1);
		mkuxt.ebmel(p0, nxykx);
		mkuxt.excko(p0, iptki);
		mkuxt.excko(p0, ixymt);
	}

	public pbfja(uint reason, string description)
	{
		nxykx = reason;
		iptki = description.TrimEnd('.');
		ixymt = "";
	}

	public pbfja(byte[] buffer, int offset, int count, Encoding encoding)
	{
		zyppx zyppx2 = new zyppx(buffer, offset, count, encoding);
		byte b = zyppx2.sfolp();
		if (b != 1)
		{
			throw new SshException(tcpjq.svqut, brgjd.edcru("Unexpected packet {0}.", b));
		}
		nxykx = zyppx2.fiswn();
		iptki = zyppx2.mdsgo();
		ixymt = zyppx2.mdsgo();
	}
}
