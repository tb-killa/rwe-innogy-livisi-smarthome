using System;

namespace Rebex.Net;

public class SmtpRejectedRecipientEventArgs : EventArgs
{
	private string yqbrk;

	private SmtpResponse efjqh;

	private bool mekas;

	public string Recipient => yqbrk;

	public SmtpResponse Response => efjqh;

	public bool Ignore
	{
		get
		{
			return mekas;
		}
		set
		{
			mekas = value;
		}
	}

	public SmtpRejectedRecipientEventArgs(string recipient, SmtpResponse response, bool ignore)
	{
		yqbrk = recipient;
		efjqh = response;
		mekas = ignore;
	}
}
