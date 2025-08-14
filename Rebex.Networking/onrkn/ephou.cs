using System.Text;
using Rebex.Net;

namespace onrkn;

internal class ephou : mkuxt
{
	private readonly byte[] xiuic;

	private readonly byte[] kxfce;

	public byte[] edhny()
	{
		return xiuic;
	}

	public byte[] hhtrk()
	{
		return kxfce;
	}

	public ephou(byte[] buffer, int offset, int count, Encoding encoding)
	{
		zyppx zyppx2 = new zyppx(buffer, offset, count, encoding);
		byte b = zyppx2.sfolp();
		if (b != 31)
		{
			throw new SshException(tcpjq.svqut, brgjd.edcru("Unexpected packet {0}.", b));
		}
		xiuic = zyppx2.tebzf();
		kxfce = zyppx2.tebzf();
	}
}
