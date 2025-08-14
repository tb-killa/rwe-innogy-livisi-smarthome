using System.Text;
using Rebex.Net;

namespace onrkn;

internal class scooe : mkuxt
{
	private string lpvlm;

	public string mezfo => lpvlm;

	public scooe(byte[] buffer, int offset, int count, Encoding encoding)
	{
		zyppx zyppx2 = new zyppx(buffer, offset, count, encoding);
		byte b = zyppx2.sfolp();
		if (b != 53)
		{
			throw new SshException(tcpjq.svqut, brgjd.edcru("Unexpected packet {0}.", b));
		}
		string text = zyppx2.mdsgo();
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		if (num != 0)
		{
			goto IL_0056;
		}
		goto IL_008c;
		IL_008c:
		if (num >= text.Length)
		{
			lpvlm = stringBuilder.ToString();
			zyppx2.mdsgo();
			return;
		}
		goto IL_0056;
		IL_0056:
		char c = text[num];
		if ((c >= ' ' && c != '\u007f') || c == '\n')
		{
			stringBuilder.Append(c);
		}
		else
		{
			stringBuilder.Append(' ');
		}
		num++;
		goto IL_008c;
	}
}
