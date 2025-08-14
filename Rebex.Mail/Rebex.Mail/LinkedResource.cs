using System.IO;
using Rebex.Mime;
using Rebex.Mime.Headers;

namespace Rebex.Mail;

public class LinkedResource : AttachmentBase
{
	private const string iwnxr = "RES";

	private string tdhtu;

	private string eawoh;

	public string FileName
	{
		get
		{
			if (eawoh == null || 1 == 0)
			{
				return "";
			}
			return eawoh;
		}
	}

	public void SetContentFromFile(string fileName, string mediaType)
	{
		if (base.jyvko.ReadOnly && 0 == 0)
		{
			throw new MailException("Cannot change read-only message.");
		}
		base.jyvko.SetContentFromFile(fileName, null, mediaType);
		cdzcu(null, "RES", p2: true, null, out tdhtu, ref eawoh);
		pqtxy();
	}

	internal void lraqa(Stream p0, string p1, string p2)
	{
		if (base.jyvko.ReadOnly && 0 == 0)
		{
			throw new MailException("Cannot change read-only message.");
		}
		base.jyvko.SetContent(p0, p1, p2);
		if (p1 != null && 0 == 0)
		{
			eawoh = p1;
			tdhtu = p1;
		}
		else
		{
			cdzcu(null, "RES", p2: true, null, out tdhtu, ref eawoh);
		}
		pqtxy();
	}

	public void SetContent(Stream contentStream, string mediaType)
	{
		lraqa(contentStream, null, mediaType);
	}

	public LinkedResource()
	{
	}

	internal LinkedResource(MimeEntity entity)
		: base(entity)
	{
		string text = entity.Name;
		bool p;
		if (text.Length == 0 || 1 == 0)
		{
			p = true;
			MessageId contentId = entity.ContentId;
			if (contentId != null && 0 == 0)
			{
				text = contentId.Id;
				if (text.Length > 48)
				{
					text = text.Substring(0, 48);
				}
			}
		}
		else
		{
			p = false;
		}
		eawoh = text;
		cdzcu(null, "RES", p, null, out tdhtu, ref eawoh);
	}

	public LinkedResource(string fileName, string mediaType)
	{
		SetContentFromFile(fileName, mediaType);
	}

	public LinkedResource(Stream contentStream, string mediaType)
	{
		SetContent(contentStream, mediaType);
	}

	public LinkedResource(Stream contentStream, string name, string mediaType)
	{
		lraqa(contentStream, name, mediaType);
	}

	internal override void pqtxy()
	{
		if (base.jyvko.ContentDisposition != null && 0 == 0)
		{
			ContentDisposition contentDisposition = new ContentDisposition("inline");
			contentDisposition.FileName = base.ContentType.Parameters["name"];
			base.jyvko.ContentDisposition = contentDisposition;
		}
	}
}
