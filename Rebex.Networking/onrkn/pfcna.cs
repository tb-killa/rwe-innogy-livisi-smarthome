using System.Text;
using Rebex.Net;

namespace onrkn;

internal class pfcna : mkuxt
{
	private uint ywvem;

	private uint hxvwt;

	public uint mcfkx => ywvem;

	public uint wdldx => hxvwt;

	public override void jfjrs(tndeg p0)
	{
		mkuxt.agnqw(p0, 93);
		mkuxt.ebmel(p0, ywvem);
		mkuxt.ebmel(p0, hxvwt);
	}

	public pfcna(uint recipientChannel, uint bytesToAdd)
	{
		ywvem = recipientChannel;
		hxvwt = bytesToAdd;
	}

	public pfcna(byte[] buffer, int offset, int count, Encoding encoding)
	{
		zyppx zyppx2 = new zyppx(buffer, offset, count, encoding);
		byte b = zyppx2.sfolp();
		if (b != 93)
		{
			throw new SshException(tcpjq.svqut, brgjd.edcru("Unexpected packet {0}.", b));
		}
		ywvem = zyppx2.fiswn();
		hxvwt = zyppx2.fiswn();
	}
}
