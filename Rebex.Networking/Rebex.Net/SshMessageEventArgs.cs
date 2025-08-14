using System;

namespace Rebex.Net;

public class SshMessageEventArgs : EventArgs
{
	private readonly string fphjq;

	public string Message => fphjq;

	public SshMessageEventArgs(string message)
	{
		fphjq = message;
	}
}
