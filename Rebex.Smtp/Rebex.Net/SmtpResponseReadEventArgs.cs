using System;

namespace Rebex.Net;

public class SmtpResponseReadEventArgs : EventArgs
{
	private string xwsmj;

	public string Response => xwsmj;

	public SmtpResponseReadEventArgs(string response)
	{
		xwsmj = response;
	}
}
