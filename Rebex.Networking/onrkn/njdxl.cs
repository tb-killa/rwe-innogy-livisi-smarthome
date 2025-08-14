using System.Text;
using Rebex;
using Rebex.Net;

namespace onrkn;

internal class njdxl : mkuxt
{
	private byte[] fdxzt;

	private byte[] mlrlt;

	private byte[] wgxnp;

	public byte[] ornst()
	{
		return mlrlt;
	}

	public byte[] ymtom()
	{
		if (wgxnp.Length == 40)
		{
			zyppx zyppx2 = new zyppx(fdxzt, 0, fdxzt.Length, EncodingTools.ASCII);
			string text = zyppx2.mdsgo();
			if (text == "ssh-dss" && 0 == 0)
			{
				tndeg tndeg2 = new tndeg(EncodingTools.ASCII);
				mkuxt.excko(tndeg2, text);
				mkuxt.lcbhj(tndeg2, wgxnp, p2: false);
				return tndeg2.ToArray();
			}
		}
		return wgxnp;
	}

	public byte[] mlmdb()
	{
		return fdxzt;
	}

	protected njdxl(byte[] buffer, int offset, int count, xbrcx expectedType, Encoding encoding)
	{
		zyppx zyppx2 = new zyppx(buffer, offset, count, encoding);
		byte b = zyppx2.sfolp();
		if ((uint)b != (uint)expectedType)
		{
			throw new SshException(tcpjq.svqut, brgjd.edcru("Unexpected packet {0}.", b));
		}
		fdxzt = zyppx2.tebzf();
		mlrlt = zyppx2.tebzf();
		wgxnp = zyppx2.tebzf();
	}

	public njdxl(byte[] buffer, int offset, int count, Encoding encoding)
		: this(buffer, offset, count, xbrcx.bsevy, encoding)
	{
	}
}
