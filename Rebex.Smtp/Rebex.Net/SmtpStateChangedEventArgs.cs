using System;

namespace Rebex.Net;

public class SmtpStateChangedEventArgs : EventArgs
{
	private SmtpState khpbs;

	private SmtpState zlels;

	public SmtpState OldState => khpbs;

	public SmtpState NewState => zlels;

	public SmtpStateChangedEventArgs(SmtpState oldState, SmtpState newState)
	{
		khpbs = oldState;
		zlels = newState;
	}
}
