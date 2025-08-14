using System.Text;
using Rebex.Net;

namespace onrkn;

internal class yvedn : mkuxt
{
	private uint nymrv;

	private uint uqihs;

	private uint vfulc;

	private uint yisdr;

	public uint usafc => nymrv;

	public uint xiviv => uqihs;

	public uint bsbbm => vfulc;

	public int upnin
	{
		get
		{
			if (yisdr <= int.MaxValue)
			{
				return (int)yisdr;
			}
			return int.MaxValue;
		}
	}

	public override void jfjrs(tndeg p0)
	{
		mkuxt.agnqw(p0, 91);
		mkuxt.ebmel(p0, nymrv);
		mkuxt.ebmel(p0, uqihs);
		mkuxt.ebmel(p0, vfulc);
		mkuxt.ebmel(p0, yisdr);
	}

	public yvedn(byte[] buffer, int offset, int count, Encoding encoding)
	{
		zyppx zyppx2 = new zyppx(buffer, offset, count, encoding);
		byte b = zyppx2.sfolp();
		if (b != 91)
		{
			throw new SshException(tcpjq.svqut, brgjd.edcru("Unexpected packet {0}.", b));
		}
		nymrv = zyppx2.fiswn();
		uqihs = zyppx2.fiswn();
		vfulc = zyppx2.fiswn();
		yisdr = zyppx2.fiswn();
	}

	public yvedn(uint recipientChannel, uint senderChannel, uint initialWindowSize, uint maxPacketSize)
	{
		nymrv = recipientChannel;
		uqihs = senderChannel;
		vfulc = initialWindowSize;
		yisdr = maxPacketSize;
	}
}
