using System;
using System.Text;
using Rebex.Net;

namespace onrkn;

internal class yxshh : mkuxt
{
	public const string bqvbz = "session";

	public const string kamht = "direct-tcpip";

	public const string pimam = "forwarded-tcpip";

	private string nidvg;

	private uint uucow;

	private uint mslmk;

	private uint wtzey;

	private object[] ahvsi;

	public string kixek => nidvg;

	public uint iixpk => uucow;

	public uint fgjcv => mslmk;

	public uint efdcm => wtzey;

	public object[] aryxn => ahvsi;

	public override void jfjrs(tndeg p0)
	{
		mkuxt.agnqw(p0, 90);
		mkuxt.excko(p0, nidvg);
		mkuxt.ebmel(p0, uucow);
		mkuxt.ebmel(p0, mslmk);
		mkuxt.ebmel(p0, wtzey);
		mkuxt.qxras(p0, ahvsi);
	}

	public yxshh(byte[] buffer, int offset, int count, Encoding encoding)
	{
		zyppx zyppx2 = new zyppx(buffer, offset, count, encoding);
		byte b = zyppx2.sfolp();
		if (b != 90)
		{
			throw new SshException(tcpjq.svqut, brgjd.edcru("Unexpected packet {0}.", b));
		}
		nidvg = zyppx2.mdsgo();
		uucow = zyppx2.fiswn();
		mslmk = zyppx2.fiswn();
		wtzey = zyppx2.fiswn();
		string text;
		if ((text = nidvg) != null && 0 == 0)
		{
			if (text == "session")
			{
				return;
			}
			if (text == "forwarded-tcpip" || text == "direct-tcpip")
			{
				ahvsi = new object[4];
				ahvsi[0] = zyppx2.mdsgo();
				ahvsi[1] = zyppx2.fiswn();
				ahvsi[2] = zyppx2.mdsgo();
				ahvsi[3] = zyppx2.fiswn();
				return;
			}
		}
		throw new NotSupportedException();
	}

	public yxshh(SshChannel channel, int maxPacketSize, object[] arguments)
	{
		maxPacketSize = Math.Min(channel.zgwen, maxPacketSize);
		switch (channel.Type)
		{
		case SshChannelType.Session:
			nidvg = "session";
			if (arguments.Length == 0)
			{
				break;
			}
			goto default;
		case SshChannelType.DirectTcpIp:
			nidvg = "direct-tcpip";
			ahvsi = new object[4];
			ahvsi[0] = (string)arguments[0];
			ahvsi[1] = (uint)(int)arguments[1];
			ahvsi[2] = "0.0.0.0";
			ahvsi[3] = 0u;
			break;
		case SshChannelType.ForwardedTcpIp:
			nidvg = "forwarded-tcpip";
			ahvsi = new object[4];
			ahvsi[0] = (string)arguments[0];
			ahvsi[1] = (uint)(int)arguments[1];
			ahvsi[2] = "0.0.0.0";
			ahvsi[3] = 0u;
			break;
		default:
			throw new NotSupportedException();
		}
		uucow = channel.evxyv;
		mslmk = (uint)channel.hpjxu;
		wtzey = (uint)Math.Min(mslmk, maxPacketSize);
	}
}
