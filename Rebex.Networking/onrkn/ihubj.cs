using System.Text;
using Rebex.Net;

namespace onrkn;

internal class ihubj : mkuxt
{
	private byte[] oishc;

	private byte[] mmdzc;

	private byte[] gisfj;

	public byte[] ddvbs()
	{
		return mmdzc;
	}

	public byte[] afokd()
	{
		return gisfj;
	}

	public byte[] ernko()
	{
		return oishc;
	}

	protected ihubj(byte[] buffer, int offset, int count, xbrcx expectedType, Encoding encoding)
	{
		zyppx zyppx2 = new zyppx(buffer, offset, count, encoding);
		byte b = zyppx2.sfolp();
		if ((uint)b != (uint)expectedType)
		{
			throw new SshException(tcpjq.svqut, brgjd.edcru("Unexpected packet {0}.", b));
		}
		oishc = zyppx2.tebzf();
		mmdzc = zyppx2.tebzf();
		gisfj = zyppx2.tebzf();
	}

	public ihubj(byte[] buffer, int offset, int count, Encoding encoding)
		: this(buffer, offset, count, xbrcx.bsevy, encoding)
	{
	}
}
