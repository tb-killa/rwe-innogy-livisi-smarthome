using System.Text;
using Rebex.Net;

namespace onrkn;

internal class scabu : mkuxt
{
	private string gpfsz;

	public override void jfjrs(tndeg p0)
	{
		mkuxt.agnqw(p0, 5);
		mkuxt.excko(p0, gpfsz);
	}

	public scabu(string name)
	{
		gpfsz = name;
	}

	public scabu(byte[] buffer, int offset, int count, Encoding encoding)
	{
		zyppx zyppx2 = new zyppx(buffer, offset, count, encoding);
		byte b = zyppx2.sfolp();
		if (b != 5)
		{
			throw new SshException(tcpjq.svqut, brgjd.edcru("Unexpected packet {0}.", b));
		}
		gpfsz = zyppx2.mdsgo();
	}
}
