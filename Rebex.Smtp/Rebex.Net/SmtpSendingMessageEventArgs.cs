using System;
using System.IO;
using Rebex.Mime;

namespace Rebex.Net;

public class SmtpSendingMessageEventArgs : EventArgs
{
	private MimeMessage bcvpr;

	private Stream tgloa;

	public Stream Stream
	{
		get
		{
			if (tgloa.CanSeek && 0 == 0)
			{
				return tgloa;
			}
			return null;
		}
	}

	public MimeMessage Message => bcvpr;

	internal SmtpSendingMessageEventArgs(MimeMessage message, Stream stream)
	{
		bcvpr = message;
		tgloa = stream;
	}
}
