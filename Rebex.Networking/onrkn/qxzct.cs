using System.Text;
using Rebex.Net;

namespace onrkn;

internal class qxzct : mkuxt
{
	public override void jfjrs(tndeg p0)
	{
		mkuxt.agnqw(p0, 21);
	}

	public qxzct()
	{
	}

	public qxzct(byte[] buffer, int offset, int count, Encoding encoding)
	{
		byte b = new zyppx(buffer, offset, count, encoding).sfolp();
		if (b == 21)
		{
			return;
		}
		throw new SshException(tcpjq.svqut, brgjd.edcru("Unexpected packet {0}.", b));
	}
}
