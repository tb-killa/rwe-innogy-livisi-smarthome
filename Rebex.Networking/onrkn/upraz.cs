using System.Text;
using Rebex.Net;

namespace onrkn;

internal class upraz : mkuxt
{
	private byte[] jchqx;

	public static readonly upraz phfxy = new upraz(new byte[0]);

	public override void jfjrs(tndeg p0)
	{
		mkuxt.agnqw(p0, 2);
		if (jchqx != null && 0 == 0)
		{
			mkuxt.lcbhj(p0, jchqx, p2: false);
		}
		else
		{
			mkuxt.ebmel(p0, 0u);
		}
	}

	public upraz(byte[] data)
	{
		jchqx = data;
	}

	public upraz(byte[] buffer, int offset, int count, Encoding encoding)
	{
		zyppx zyppx2 = new zyppx(buffer, offset, count, encoding);
		byte b = zyppx2.sfolp();
		if (b != 2)
		{
			throw new SshException(tcpjq.svqut, brgjd.edcru("Unexpected packet {0}.", b));
		}
		jchqx = zyppx2.tebzf();
	}
}
