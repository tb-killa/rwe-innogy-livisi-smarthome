using System.Text;
using Rebex.Net;

namespace onrkn;

internal class jgcsx : mkuxt
{
	private uint mdbqe;

	private uint gfdrw;

	private string hjjlj;

	private string pyvnr;

	public uint wwomf => mdbqe;

	public uint qbavz => gfdrw;

	public string aspgd => hjjlj;

	public override void jfjrs(tndeg p0)
	{
		mkuxt.agnqw(p0, 92);
		mkuxt.ebmel(p0, mdbqe);
		mkuxt.ebmel(p0, gfdrw);
		mkuxt.excko(p0, hjjlj);
		mkuxt.excko(p0, pyvnr);
	}

	public jgcsx(uint recipientChannel, uint reasonCode, string description)
	{
		mdbqe = recipientChannel;
		gfdrw = reasonCode;
		hjjlj = description;
		pyvnr = "";
	}

	public jgcsx(byte[] buffer, int offset, int count, Encoding encoding)
	{
		zyppx zyppx2 = new zyppx(buffer, offset, count, encoding);
		byte b = zyppx2.sfolp();
		if (b != 92)
		{
			throw new SshException(tcpjq.svqut, brgjd.edcru("Unexpected packet {0}.", b));
		}
		mdbqe = zyppx2.fiswn();
		gfdrw = zyppx2.fiswn();
		hjjlj = zyppx2.mdsgo();
		pyvnr = zyppx2.mdsgo();
	}
}
