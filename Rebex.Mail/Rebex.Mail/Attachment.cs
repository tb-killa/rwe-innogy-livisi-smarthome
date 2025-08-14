using System;
using System.IO;
using Rebex.Mime;

namespace Rebex.Mail;

public class Attachment : AttachmentBase
{
	private const string whzfs = "ATT";

	private string qrykh;

	private string baglj;

	private bool jvasn;

	private MailMessage ymnhg;

	public string DisplayName
	{
		get
		{
			if (qrykh == null || 1 == 0)
			{
				return "";
			}
			return qrykh;
		}
	}

	public string FileName
	{
		get
		{
			if (baglj == null || 1 == 0)
			{
				return "";
			}
			return baglj;
		}
		set
		{
			if (base.jyvko.ReadOnly && 0 == 0)
			{
				throw new MailException("Cannot change read-only message.");
			}
			if (value == null || 1 == 0)
			{
				value = "";
			}
			else
			{
				AttachmentBase.wwfff(value, "value");
			}
			base.jyvko.Name = value;
			baglj = value;
			qrykh = value;
			jvasn = false;
		}
	}

	internal bool fkzie => jvasn;

	public MailMessage ContentMessage
	{
		get
		{
			if (ymnhg != null && 0 == 0)
			{
				return ymnhg;
			}
			if (base.jyvko.Kind != MimeEntityKind.Message)
			{
				return null;
			}
			MimeEntity contentMessage = base.jyvko.ContentMessage;
			if (contentMessage == null || 1 == 0)
			{
				return null;
			}
			if (ymnhg == null || 1 == 0)
			{
				ymnhg = new MailMessage(contentMessage.ToMessage());
			}
			return ymnhg;
		}
	}

	internal MailMessage pptit => ymnhg;

	internal override AttachmentBase vpeyb()
	{
		Attachment attachment = (Attachment)base.vpeyb();
		attachment.qrykh = qrykh;
		attachment.baglj = baglj;
		if (ymnhg != null && 0 == 0)
		{
			attachment.ymnhg = ymnhg.Clone();
		}
		return attachment;
	}

	public void SetContentFromFile(string fileName, string name, string mediaType)
	{
		if (fileName == null || 1 == 0)
		{
			throw new ArgumentNullException("fileName");
		}
		if (name == null || 1 == 0)
		{
			name = Path.GetFileName(fileName);
		}
		AttachmentBase.wwfff(name, "name");
		if (base.jyvko.ReadOnly && 0 == 0)
		{
			throw new MailException("Cannot change read-only message.");
		}
		if (mediaType == null || false || mediaType.Length == 0 || 1 == 0)
		{
			mediaType = "application/octet-stream";
		}
		base.jyvko.SetContentFromFile(fileName, name, mediaType);
		baglj = name;
		qrykh = name;
		jvasn = false;
		pqtxy();
	}

	public void SetContentFromFile(string fileName, string name)
	{
		SetContentFromFile(fileName, name, null);
	}

	public void SetContentFromFile(string fileName)
	{
		SetContentFromFile(fileName, null, null);
	}

	public void SetContent(Stream source, string name, string mediaType)
	{
		hampi(source, name, mediaType, p3: true);
	}

	private void hampi(Stream p0, string p1, string p2, bool p3)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("source");
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("name");
		}
		string text = p1;
		if (p3 && 0 == 0)
		{
			AttachmentBase.wwfff(p1, "name");
		}
		else
		{
			text = AttachmentBase.jficx(p1, '-');
		}
		if (base.jyvko.ReadOnly && 0 == 0)
		{
			throw new MailException("Cannot change read-only message.");
		}
		if (p2 == null || false || p2.Length == 0 || 1 == 0)
		{
			p2 = "application/octet-stream";
		}
		base.jyvko.SetContent(p0, p1, p2);
		baglj = text;
		qrykh = p1;
		jvasn = false;
		pqtxy();
	}

	public void SetContent(Stream source, string name)
	{
		SetContent(source, name, null);
	}

	internal MimeEntity akqjb()
	{
		if (ymnhg != null && 0 == 0)
		{
			base.jyvko.SetContent(ymnhg.ToMimeMessage());
			if (baglj != null && 0 == 0 && (!jvasn || 1 == 0))
			{
				base.jyvko.Name = baglj;
			}
		}
		return base.jyvko.Clone();
	}

	internal override void pqtxy()
	{
		ymnhg = null;
	}

	internal override Stream oetys(bool p0)
	{
		MimeEntity contentMessage = base.jyvko.ContentMessage;
		if (contentMessage != null && 0 == 0)
		{
			if (p0 && 0 == 0)
			{
				throw new MailException("Accessing writable content stream of attached embedded message is not supported.");
			}
			MemoryStream memoryStream = new MemoryStream();
			if (base.jyvko.Kind != MimeEntityKind.Message && base.jyvko.SignatureStyle == MimeSignatureStyle.Enveloped)
			{
				base.jyvko.SignedContentInfo.Save(memoryStream);
			}
			else
			{
				contentMessage.Save(memoryStream);
			}
			return new MemoryStream(memoryStream.GetBuffer(), 0, (int)memoryStream.Length, writable: false, publiclyVisible: false);
		}
		return base.oetys(p0);
	}

	internal override long ukaeb()
	{
		MimeEntity contentMessage = base.jyvko.ContentMessage;
		if (contentMessage != null && 0 == 0)
		{
			MemoryStream memoryStream = new MemoryStream();
			contentMessage.Save(memoryStream);
			return memoryStream.Length;
		}
		return base.ukaeb();
	}

	internal Attachment(MimeEntity entity, AttachmentCollection list)
		: base(entity)
	{
		string text = entity.Name;
		bool p;
		if (text.Length == 0 || 1 == 0)
		{
			p = true;
			text = entity.ContentDescription;
			if (text.Length > 48)
			{
				text = text.Substring(0, 48);
			}
			jvasn = true;
		}
		else
		{
			p = false;
		}
		if (text.Length == 0 || 1 == 0)
		{
			entity = entity.ContentMessage;
			if (entity != null && 0 == 0)
			{
				MimeHeader mimeHeader = entity.Headers["Subject"];
				if (mimeHeader != null && 0 == 0)
				{
					text = mimeHeader.Value.ToString();
					jvasn = true;
				}
			}
		}
		baglj = text;
		cdzcu(ymnhg, "ATT", p, list, out qrykh, ref baglj);
	}

	public Attachment()
	{
	}

	public Attachment(MailMessage message)
		: this(message, null, null)
	{
	}

	internal Attachment(MailMessage message, string fileName, AttachmentCollection list)
	{
		if (message == null || 1 == 0)
		{
			throw new ArgumentNullException("message");
		}
		ymnhg = message;
		base.jyvko.SetContent(ymnhg.ToMimeMessage());
		bool flag;
		if (!string.IsNullOrEmpty(fileName) || 1 == 0)
		{
			baglj = fileName;
			flag = false;
			if (!flag)
			{
				goto IL_0073;
			}
		}
		baglj = message.Subject;
		jvasn = true;
		flag = true;
		goto IL_0073;
		IL_0073:
		cdzcu(ymnhg, "ATT", flag, list, out qrykh, ref baglj);
	}

	public Attachment(string fileName)
	{
		SetContentFromFile(fileName);
	}

	public Attachment(string fileName, string name)
	{
		SetContentFromFile(fileName, name);
	}

	public Attachment(string fileName, string name, string mediaType)
	{
		SetContentFromFile(fileName, name, mediaType);
	}

	public Attachment(Stream contentStream, string name)
	{
		SetContent(contentStream, name);
	}

	public Attachment(Stream contentStream, string name, string mediaType)
	{
		SetContent(contentStream, name, mediaType);
	}

	public Attachment(byte[] content, string name)
	{
		if (content == null || 1 == 0)
		{
			throw new ArgumentNullException("content");
		}
		MemoryStream source = new MemoryStream(content);
		SetContent(source, name);
	}

	public Attachment(byte[] content, string name, string mediaType)
	{
		if (content == null || 1 == 0)
		{
			throw new ArgumentNullException("content");
		}
		MemoryStream source = new MemoryStream(content);
		SetContent(source, name, mediaType);
	}

	internal Attachment(byte[] content, string name, bool checkName)
	{
		if (content == null || 1 == 0)
		{
			throw new ArgumentNullException("content");
		}
		MemoryStream p = new MemoryStream(content);
		hampi(p, name, null, checkName);
	}
}
