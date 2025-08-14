using System.Text;
using Rebex.Net;

namespace onrkn;

internal class meotu : mkuxt
{
	private int tmkds;

	public override void jfjrs(tndeg p0)
	{
		mkuxt.agnqw(p0, 3);
		mkuxt.kwnor(p0, tmkds);
	}

	public meotu(int sequenceNumber)
	{
		tmkds = sequenceNumber;
	}

	public meotu(byte[] buffer, int offset, int count, Encoding encoding)
	{
		zyppx zyppx2 = new zyppx(buffer, offset, count, encoding);
		byte b = zyppx2.sfolp();
		if (b != 3)
		{
			throw new SshException(tcpjq.svqut, brgjd.edcru("Unexpected packet {0}.", b));
		}
		tmkds = zyppx2.rvfya();
	}
}
