using System.Text;
using Rebex.Net;

namespace onrkn;

internal class ldkdl : mkuxt
{
	private byte[] ayqua;

	public byte[] qcnlq => ayqua;

	public override void jfjrs(tndeg p0)
	{
		mkuxt.agnqw(p0, 61);
		mkuxt.lcbhj(p0, ayqua, p2: false);
	}

	public ldkdl(byte[] message)
	{
		ayqua = message;
	}

	public ldkdl(byte[] buffer, int offset, int count, Encoding encoding)
	{
		zyppx zyppx2 = new zyppx(buffer, offset, count, encoding);
		byte b = zyppx2.sfolp();
		if (b != 61)
		{
			throw new SshException(tcpjq.svqut, brgjd.edcru("Unexpected packet {0}.", b));
		}
		ayqua = zyppx2.tebzf();
	}
}
