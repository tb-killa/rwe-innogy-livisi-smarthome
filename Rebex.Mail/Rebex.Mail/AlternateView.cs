using System.IO;
using Rebex.Mime;

namespace Rebex.Mail;

public class AlternateView : AttachmentBase
{
	public void SetContentFromFile(string fileName, string mediaType)
	{
		if (base.jyvko.ReadOnly && 0 == 0)
		{
			throw new MailException("Cannot change read-only message.");
		}
		base.jyvko.SetContentFromFile(fileName, null, mediaType);
		pqtxy();
	}

	public void SetContentFromFile(string fileName)
	{
		if (base.jyvko.ReadOnly && 0 == 0)
		{
			throw new MailException("Cannot change read-only message.");
		}
		base.jyvko.SetContentFromFile(fileName, null, "text/plain");
		pqtxy();
	}

	public void SetContent(Stream contentStream, string mediaType)
	{
		if (base.jyvko.ReadOnly && 0 == 0)
		{
			throw new MailException("Cannot change read-only message.");
		}
		base.jyvko.SetContent(contentStream, null, mediaType);
		pqtxy();
	}

	public void SetContent(Stream contentStream)
	{
		if (base.jyvko.ReadOnly && 0 == 0)
		{
			throw new MailException("Cannot change read-only message.");
		}
		base.jyvko.SetContent(contentStream, null, "text/plain");
		pqtxy();
	}

	internal AlternateView(MimeEntity entity)
		: base(entity)
	{
	}

	public AlternateView()
	{
	}

	public AlternateView(string fileName, string mediaType)
		: this()
	{
		SetContentFromFile(fileName, mediaType);
	}

	public AlternateView(string fileName)
		: this()
	{
		SetContentFromFile(fileName, "text/plain");
	}

	public AlternateView(Stream contentStream, string mediaType)
		: this()
	{
		SetContent(contentStream, mediaType);
	}

	public AlternateView(Stream contentStream)
		: this()
	{
		SetContent(contentStream, "text/plain");
	}
}
