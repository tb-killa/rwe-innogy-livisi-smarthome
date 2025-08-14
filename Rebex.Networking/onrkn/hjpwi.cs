using System.Text;
using Rebex.Net;
using Rebex.Security.Cryptography;

namespace onrkn;

internal class hjpwi : mkuxt
{
	private ObjectIdentifier yzsmy;

	public ObjectIdentifier ledxm => yzsmy;

	public hjpwi(byte[] buffer, int offset, int count, Encoding encoding)
	{
		zyppx zyppx2 = new zyppx(buffer, offset, count, encoding);
		byte b = zyppx2.sfolp();
		if (b != 60)
		{
			throw new SshException(tcpjq.svqut, brgjd.edcru("Unexpected packet {0}.", b));
		}
		yzsmy = ObjectIdentifier.Parse(zyppx2.tebzf());
	}
}
