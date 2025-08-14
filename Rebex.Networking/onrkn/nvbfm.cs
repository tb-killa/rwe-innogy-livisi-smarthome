using System;
using System.Text;
using Rebex;
using Rebex.Net;

namespace onrkn;

internal class nvbfm : mkuxt
{
	private uint suexo;

	private string bmbtm;

	private byte[] ssgpw;

	private bool oylhh;

	public string iivww => bmbtm;

	public bool fsffd => oylhh;

	public void ohtwy(SshChannelExitStatus p0)
	{
		zyppx zyppx2 = new zyppx(ssgpw, 0, ssgpw.Length, EncodingTools.ASCII);
		string text;
		if ((text = bmbtm) == null)
		{
			return;
		}
		if (!(text == "exit-status") || 1 == 0)
		{
			if (text == "exit-signal")
			{
				string p1 = zyppx2.mdsgo();
				bool p2 = zyppx2.qxurr();
				string p3 = zyppx2.mdsgo();
				p0.pwabz(p1, p2, p3);
			}
		}
		else
		{
			uint p4 = zyppx2.fiswn();
			p0.jxfol(p4);
		}
	}

	public override void jfjrs(tndeg p0)
	{
		mkuxt.agnqw(p0, 98);
		mkuxt.ebmel(p0, suexo);
		mkuxt.excko(p0, bmbtm);
		mkuxt.duaqa(p0, oylhh);
		if (ssgpw != null && 0 == 0)
		{
			mkuxt.lcbhj(p0, ssgpw, p2: true);
		}
	}

	public nvbfm(uint recipientChannel, string name, bool wantReply, Encoding encoding, params object[] parameters)
	{
		suexo = recipientChannel;
		bmbtm = name;
		oylhh = wantReply;
		tndeg tndeg2;
		int num;
		if (parameters != null && 0 == 0 && parameters.Length > 0)
		{
			tndeg2 = new tndeg(encoding);
			num = 0;
			if (num != 0)
			{
				goto IL_0041;
			}
			goto IL_008c;
		}
		return;
		IL_008c:
		if (num < parameters.Length)
		{
			goto IL_0041;
		}
		ssgpw = tndeg2.ToArray();
		return;
		IL_0041:
		object obj = parameters[num];
		if (obj is string && 0 == 0)
		{
			mkuxt.excko(tndeg2, (string)obj);
		}
		else
		{
			if (!(obj is uint))
			{
				throw new NotSupportedException();
			}
			mkuxt.ebmel(tndeg2, (uint)obj);
		}
		num++;
		goto IL_008c;
	}

	public nvbfm(byte[] buffer, int offset, int count, Encoding encoding)
	{
		zyppx zyppx2 = new zyppx(buffer, offset, count, encoding);
		byte b = zyppx2.sfolp();
		if (b != 98)
		{
			throw new SshException(tcpjq.svqut, brgjd.edcru("Unexpected packet {0}.", b));
		}
		suexo = zyppx2.fiswn();
		bmbtm = zyppx2.mdsgo();
		oylhh = zyppx2.qxurr();
		ssgpw = zyppx2.tblgh();
	}
}
