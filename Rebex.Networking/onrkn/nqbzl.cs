using System.Text;
using Rebex.Net;

namespace onrkn;

internal class nqbzl : mkuxt
{
	private string[] upkvl;

	private bool exazc;

	public string[] kcxck => upkvl;

	public bool abbgz => exazc;

	public nqbzl(byte[] buffer, int offset, int count, Encoding encoding)
	{
		zyppx zyppx2 = new zyppx(buffer, offset, count, encoding);
		byte b = zyppx2.sfolp();
		if (b != 51)
		{
			throw new SshException(tcpjq.svqut, brgjd.edcru("Unexpected packet {0}.", b));
		}
		upkvl = zyppx2.dxxld();
		exazc = zyppx2.qxurr();
	}
}
