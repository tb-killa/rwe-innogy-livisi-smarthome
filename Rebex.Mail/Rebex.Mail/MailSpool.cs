using System;
using System.ComponentModel;
using System.IO;
using Rebex.Mime;
using onrkn;

namespace Rebex.Mail;

public sealed class MailSpool
{
	private MailSpool()
	{
	}

	public static void Send(MailServerType serverType, MimeMessage message, string pickupDirectory)
	{
		if (serverType != MailServerType.Iis)
		{
			throw new ArgumentException("Server type not specified.", "serverType");
		}
		if (message == null || 1 == 0)
		{
			throw new ArgumentNullException("message");
		}
		if (pickupDirectory == null || 1 == 0)
		{
			throw new ArgumentNullException("pickupDirectory", "Pickup directory cannot be null.");
		}
		string tempFileName = Path.GetTempFileName();
		string destFileName = Path.Combine(pickupDirectory, brgjd.edcru("{0:N}.eml", Guid.NewGuid()));
		Stream stream = File.Create(tempFileName);
		try
		{
			message.Save(stream);
			stream.Close();
			stream = null;
		}
		finally
		{
			if (stream != null && 0 == 0)
			{
				stream.Close();
				File.Delete(tempFileName);
			}
		}
		File.Move(tempFileName, destFileName);
	}

	public static void Send(MailServerType serverType, MimeMessage message)
	{
		if (serverType != MailServerType.Iis)
		{
			throw new ArgumentException("Server type not specified.", "serverType");
		}
		if (message == null || 1 == 0)
		{
			throw new ArgumentNullException("message");
		}
		Send(serverType, message, null);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("The SendViaIis method has been deprecated and will be removed. Use Send method instead.", true)]
	[wptwl(false)]
	public static void SendViaIis(MailServerType serverType, MailMessage message, string pickupDirectory)
	{
		Send(serverType, message, pickupDirectory);
	}

	public static void Send(MailServerType serverType, MailMessage message, string pickupDirectory)
	{
		if (serverType != MailServerType.Iis)
		{
			throw new ArgumentException("Server type not specified.", "serverType");
		}
		if (message == null || 1 == 0)
		{
			throw new ArgumentNullException("message");
		}
		if (pickupDirectory == null || 1 == 0)
		{
			throw new ArgumentNullException("pickupDirectory");
		}
		Send(serverType, message.ToMimeMessage(), pickupDirectory);
	}

	public static void Send(MailServerType serverType, MailMessage message)
	{
		if (serverType != MailServerType.Iis)
		{
			throw new ArgumentException("Server type not specified.", "serverType");
		}
		if (message == null || 1 == 0)
		{
			throw new ArgumentNullException("message");
		}
		Send(serverType, message.ToMimeMessage(), null);
	}
}
