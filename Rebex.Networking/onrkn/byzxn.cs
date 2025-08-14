using System;
using System.Text;
using Rebex.Net;

namespace onrkn;

internal class byzxn : mkuxt
{
	private string xnezg;

	private string vtyqu;

	private string obpxe;

	private string[] dawfb;

	private bool[] ebxja;

	public string pfpkh => xnezg;

	public string ovtqn => vtyqu;

	public string[] pjmze => dawfb;

	public bool[] nerlh => ebxja;

	public byzxn(byte[] buffer, int offset, int count, Encoding encoding)
	{
		zyppx zyppx2 = new zyppx(buffer, offset, count, encoding);
		byte b = zyppx2.sfolp();
		if (b != 60)
		{
			throw new SshException(tcpjq.svqut, brgjd.edcru("Unexpected packet {0}.", b));
		}
		xnezg = zyppx2.mdsgo();
		vtyqu = zyppx2.mdsgo();
		obpxe = zyppx2.mdsgo();
		uint num = zyppx2.fiswn();
		if (num > 1024)
		{
			throw new InvalidOperationException();
		}
		dawfb = new string[num];
		ebxja = new bool[num];
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_009a;
		}
		goto IL_00c6;
		IL_00c6:
		if (num2 >= dawfb.Length)
		{
			return;
		}
		goto IL_009a;
		IL_009a:
		dawfb[num2] = zyppx2.mdsgo();
		ebxja[num2] = zyppx2.qxurr();
		num2++;
		goto IL_00c6;
	}
}
