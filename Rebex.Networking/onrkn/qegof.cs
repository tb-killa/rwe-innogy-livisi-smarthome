using System;
using System.Text;
using Rebex.Net;

namespace onrkn;

internal class qegof : mkuxt
{
	private object[] gzxnc;

	public object[] ntypo => gzxnc;

	public override void jfjrs(tndeg p0)
	{
		mkuxt.agnqw(p0, 81);
		mkuxt.qxras(p0, gzxnc);
	}

	public qegof(byte[] buffer, int offset, int count, Encoding encoding, bvkts kind)
	{
		zyppx zyppx2 = new zyppx(buffer, offset, count, encoding);
		byte b = zyppx2.sfolp();
		if (b != 81)
		{
			throw new SshException(tcpjq.svqut, brgjd.edcru("Unexpected packet {0}.", b));
		}
		switch (kind)
		{
		case bvkts.plpbq:
		case bvkts.cpoos:
			gzxnc = new object[0];
			break;
		case bvkts.frokg:
			gzxnc = new object[1];
			gzxnc[0] = zyppx2.fiswn();
			break;
		default:
			throw new NotSupportedException();
		}
	}

	public qegof()
	{
	}
}
