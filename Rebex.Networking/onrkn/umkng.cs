using Rebex;
using Rebex.Net;

namespace onrkn;

internal class umkng : mkuxt
{
	private byte[] rmbrd;

	public byte[] wsydy => rmbrd;

	public override void jfjrs(tndeg p0)
	{
		mkuxt.agnqw(p0, 65);
		mkuxt.lcbhj(p0, rmbrd, p2: false);
	}

	public umkng(byte[] errorToken)
	{
		rmbrd = errorToken;
	}

	public umkng(byte[] buffer, int offset, int count)
	{
		zyppx zyppx2 = new zyppx(buffer, offset, count, EncodingTools.ASCII);
		byte b = zyppx2.sfolp();
		if (b != 65)
		{
			throw new SshException(tcpjq.svqut, brgjd.edcru("Unexpected packet {0}.", b));
		}
		rmbrd = zyppx2.tebzf();
	}
}
