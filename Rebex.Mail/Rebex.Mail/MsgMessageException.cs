using System;
using onrkn;

namespace Rebex.Mail;

public class MsgMessageException : Exception
{
	public MsgMessageException(string message)
		: base(message)
	{
	}

	public MsgMessageException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	internal MsgMessageException(string messageFormat, params object[] args)
		: base(brgjd.edcru(messageFormat, args))
	{
	}

	internal static MsgMessageException jhhqd(string p0, Exception p1)
	{
		return new MsgMessageException(p0, p1);
	}
}
