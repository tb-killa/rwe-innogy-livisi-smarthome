using System;

namespace Rebex.Net;

public class SmtpCommandSentEventArgs : EventArgs
{
	private string bqskl;

	public string Command => bqskl;

	public SmtpCommandSentEventArgs(string command)
	{
		bqskl = command;
	}
}
