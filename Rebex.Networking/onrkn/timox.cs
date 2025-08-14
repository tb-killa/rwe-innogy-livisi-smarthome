using System.Text;
using Rebex.Net;

namespace onrkn;

internal class timox : mkuxt
{
	private string gdgsp;

	public override void jfjrs(tndeg p0)
	{
		mkuxt.agnqw(p0, 6);
		mkuxt.excko(p0, gdgsp);
	}

	public timox(string name)
	{
		gdgsp = name;
	}

	public timox(byte[] buffer, int offset, int count, Encoding encoding)
	{
		zyppx zyppx2 = new zyppx(buffer, offset, count, encoding);
		byte b = zyppx2.sfolp();
		if (b != 6)
		{
			throw new SshException(tcpjq.svqut, brgjd.edcru("Unexpected packet {0}.", b));
		}
		gdgsp = zyppx2.mdsgo();
	}
}
