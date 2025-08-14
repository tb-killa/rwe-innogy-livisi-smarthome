using System.Text;
using Rebex.Net;

namespace onrkn;

internal class cuapx : mkuxt
{
	private bool rasma;

	private string xppgr;

	private string hsupq;

	public string jwgky => xppgr;

	public cuapx(byte[] buffer, int offset, int count, Encoding encoding)
	{
		zyppx zyppx2 = new zyppx(buffer, offset, count, encoding);
		byte b = zyppx2.sfolp();
		if (b != 4)
		{
			throw new SshException(tcpjq.svqut, brgjd.edcru("Unexpected packet {0}.", b));
		}
		rasma = zyppx2.qxurr();
		xppgr = zyppx2.mdsgo();
		hsupq = zyppx2.mdsgo();
	}
}
