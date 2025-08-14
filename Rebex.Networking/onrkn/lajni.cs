using System.Text;
using Rebex.Net;

namespace onrkn;

internal class lajni : mkuxt
{
	private int zmljo;

	private int bougj;

	private string lkyla;

	private string nfpcj;

	public string ydgsz => lkyla;

	public override void jfjrs(tndeg p0)
	{
		mkuxt.agnqw(p0, 64);
		mkuxt.ebmel(p0, (uint)zmljo);
		mkuxt.ebmel(p0, (uint)bougj);
		mkuxt.excko(p0, lkyla);
		mkuxt.excko(p0, nfpcj);
	}

	public lajni(int majorStatus, int minorStatus, string message)
	{
		zmljo = majorStatus;
		bougj = minorStatus;
		lkyla = message;
		nfpcj = "";
	}

	public lajni(byte[] buffer, int offset, int count, Encoding encoding)
	{
		zyppx zyppx2 = new zyppx(buffer, offset, count, encoding);
		byte b = zyppx2.sfolp();
		if (b != 64)
		{
			throw new SshException(tcpjq.svqut, brgjd.edcru("Unexpected packet {0}.", b));
		}
		zmljo = zyppx2.rvfya();
		bougj = zyppx2.rvfya();
		lkyla = zyppx2.mdsgo();
		nfpcj = zyppx2.mdsgo();
	}
}
