using System;
using System.Text;
using Rebex.Net;

namespace onrkn;

internal class fnofn : mkuxt
{
	public const string ugnvs = "tcpip-forward";

	public const string hhjkn = "cancel-tcpip-forward";

	private string iqmmk;

	private bool ryjqt;

	private object[] jehsl;

	public object[] uqqhd => jehsl;

	public bool tkyxc => ryjqt;

	public override void jfjrs(tndeg p0)
	{
		mkuxt.agnqw(p0, 80);
		mkuxt.excko(p0, iqmmk);
		mkuxt.duaqa(p0, ryjqt);
		mkuxt.qxras(p0, jehsl);
	}

	public fnofn(byte[] buffer, int offset, int count, Encoding encoding)
	{
		zyppx zyppx2 = new zyppx(buffer, offset, count, encoding);
		byte b = zyppx2.sfolp();
		if (b != 80)
		{
			throw new SshException(tcpjq.svqut, brgjd.edcru("Unexpected packet {0}.", b));
		}
		iqmmk = zyppx2.mdsgo();
		ryjqt = zyppx2.qxurr();
	}

	public fnofn(string requestName, bool wantReply, params object[] arguments)
	{
		iqmmk = requestName;
		ryjqt = wantReply;
		string text;
		if ((text = requestName) != null && 0 == 0 && (text == "tcpip-forward" || text == "cancel-tcpip-forward"))
		{
			jehsl = new object[2];
			jehsl[0] = (string)arguments[0];
			jehsl[1] = (uint)(int)arguments[1];
			return;
		}
		throw new NotSupportedException();
	}
}
