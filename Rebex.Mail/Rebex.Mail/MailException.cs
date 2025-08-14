using System;

namespace Rebex.Mail;

public class MailException : Exception
{
	public MailException()
		: base("Unspecified mail error.")
	{
	}

	public MailException(string message)
		: base(message)
	{
	}

	public MailException(string message, Exception inner)
		: base(message, inner)
	{
	}
}
