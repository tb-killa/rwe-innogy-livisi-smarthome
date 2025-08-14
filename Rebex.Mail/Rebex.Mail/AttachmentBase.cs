using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Rebex.Mime;
using Rebex.Mime.Headers;
using onrkn;

namespace Rebex.Mail;

public abstract class AttachmentBase
{
	private MimeEntity lmhqe;

	private static char[] bxkhb;

	public TransferEncoding TransferEncoding
	{
		get
		{
			return lmhqe.TransferEncoding;
		}
		set
		{
			if (lmhqe.ReadOnly && 0 == 0)
			{
				throw new MailException("Cannot change read-only message.");
			}
			lmhqe.TransferEncoding = value;
		}
	}

	public ContentType ContentType => lmhqe.ContentType;

	public string MediaType => ContentType.MediaType;

	public ContentDisposition ContentDisposition => lmhqe.ContentDisposition;

	public string ContentDescription
	{
		get
		{
			return lmhqe.ContentDescription;
		}
		set
		{
			lmhqe.ContentDescription = value;
		}
	}

	public MessageId ContentId
	{
		get
		{
			return lmhqe.ContentId;
		}
		set
		{
			if (lmhqe.ReadOnly && 0 == 0)
			{
				throw new MailException("Cannot change read-only message.");
			}
			lmhqe.ContentId = value;
		}
	}

	public string ContentLocation
	{
		get
		{
			ContentLocation contentLocation = jyvko.ContentLocation;
			if (contentLocation == null || 1 == 0)
			{
				return null;
			}
			return contentLocation.Location;
		}
		set
		{
			if (lmhqe.ReadOnly && 0 == 0)
			{
				throw new MailException("Cannot change read-only message.");
			}
			jyvko.ContentLocation = new ContentLocation(value);
		}
	}

	public string ContentString => lmhqe.ContentString;

	public Encoding Charset => lmhqe.Charset;

	public MimeOptions Options
	{
		get
		{
			return lmhqe.Options;
		}
		set
		{
			lmhqe.Options = value;
		}
	}

	internal MimeEntity jyvko => lmhqe;

	internal void tidto(bool p0)
	{
		if (p0 && 0 == 0)
		{
			lmhqe.ReadOnly = p0;
		}
		else
		{
			lmhqe = lmhqe.Clone();
		}
	}

	internal virtual void pqtxy()
	{
	}

	internal virtual Stream oetys(bool p0)
	{
		if (lmhqe.Kind == MimeEntityKind.Enveloped)
		{
			MemoryStream memoryStream = new MemoryStream();
			lmhqe.EnvelopedContentInfo.Save(memoryStream);
			return new MemoryStream(memoryStream.GetBuffer(), 0, (int)memoryStream.Length, writable: false, publiclyVisible: false);
		}
		return lmhqe.GetContentStream(p0);
	}

	internal virtual long ukaeb()
	{
		Stream contentStream = lmhqe.GetContentStream();
		try
		{
			return contentStream.Length;
		}
		finally
		{
			if (contentStream != null && 0 == 0)
			{
				((IDisposable)contentStream).Dispose();
			}
		}
	}

	internal virtual AttachmentBase vpeyb()
	{
		AttachmentBase attachmentBase = (AttachmentBase)Activator.CreateInstance(GetType());
		attachmentBase.lmhqe = lmhqe.Clone();
		return attachmentBase;
	}

	public Stream GetContentStream(bool writable)
	{
		return oetys(writable);
	}

	public Stream GetContentStream()
	{
		return oetys(p0: false);
	}

	public long GetContentLength()
	{
		return ukaeb();
	}

	public void SetContent(string text)
	{
		if (lmhqe.ReadOnly && 0 == 0)
		{
			throw new MailException("Cannot change read-only message.");
		}
		lmhqe.SetContent(text);
		pqtxy();
	}

	public void SetContent(string text, string mediaType)
	{
		if (lmhqe.ReadOnly && 0 == 0)
		{
			throw new MailException("Cannot change read-only message.");
		}
		lmhqe.SetContent(text, mediaType);
		pqtxy();
	}

	public void SetContent(string text, string mediaType, Encoding charset)
	{
		if (lmhqe.ReadOnly && 0 == 0)
		{
			throw new MailException("Cannot change read-only message.");
		}
		lmhqe.SetContent(text, mediaType, charset);
		pqtxy();
	}

	public void SetContent(string text, string mediaType, Encoding charset, TransferEncoding transferEncoding)
	{
		if (lmhqe.ReadOnly && 0 == 0)
		{
			throw new MailException("Cannot change read-only message.");
		}
		lmhqe.SetContent(text, mediaType, charset, transferEncoding);
		pqtxy();
	}

	public void Save(string fileName)
	{
		if (fileName == null || 1 == 0)
		{
			throw new ArgumentNullException("fileName");
		}
		Stream stream = vtdxm.namiu(fileName, pieou.msydj);
		try
		{
			Save(stream);
		}
		finally
		{
			if (stream != null && 0 == 0)
			{
				((IDisposable)stream).Dispose();
			}
		}
	}

	public void Save(Stream output)
	{
		if (output == null || 1 == 0)
		{
			throw new ArgumentNullException("output");
		}
		if (!output.CanWrite || 1 == 0)
		{
			throw new IOException("Stream is not writable.");
		}
		Stream contentStream = GetContentStream();
		try
		{
			byte[] array = new byte[1024];
			while (true)
			{
				int num = contentStream.Read(array, 0, array.Length);
				if (num != 0)
				{
					output.Write(array, 0, num);
					continue;
				}
				break;
			}
		}
		finally
		{
			if (contentStream != null && 0 == 0)
			{
				((IDisposable)contentStream).Dispose();
			}
		}
	}

	protected internal AttachmentBase(MimeEntity entity)
	{
		if (entity == null || 1 == 0)
		{
			throw new ArgumentNullException("entity");
		}
		lmhqe = entity.Clone();
		int num = 0;
		if (num != 0)
		{
			goto IL_0030;
		}
		goto IL_00d4;
		IL_0030:
		string text = lmhqe.Headers[num].Name.ToLower(CultureInfo.InvariantCulture);
		if ((!text.StartsWith("content-", StringComparison.Ordinal) || 1 == 0) && (!text.StartsWith("x-rebex-", StringComparison.Ordinal) || 1 == 0) && (!(text == "disposition") || false || !(lmhqe.ContentType.MediaType == "message/disposition-notification")))
		{
			lmhqe.Headers.RemoveAt(num);
			num--;
		}
		num++;
		goto IL_00d4;
		IL_00d4:
		if (num < lmhqe.Headers.Count)
		{
			goto IL_0030;
		}
		lmhqe.ReadOnly = entity.ReadOnly;
	}

	public MimeEntity ToMimeEntity()
	{
		return lmhqe.Clone();
	}

	protected internal AttachmentBase()
	{
		lmhqe = new MimeEntity();
	}

	static AttachmentBase()
	{
		char[] array = dahxy.cexjz();
		bxkhb = new char[array.Length + 4];
		bxkhb[0] = '?';
		bxkhb[1] = '*';
		bxkhb[2] = '/';
		bxkhb[3] = '\\';
		array.CopyTo(bxkhb, 4);
	}

	internal static void wwfff(string p0, string p1)
	{
		int num = p0.IndexOfAny(bxkhb);
		if (num < 0)
		{
			return;
		}
		throw new ArgumentException(brgjd.edcru("Invalid file name (character at position {0}).", num), p1);
	}

	internal void cdzcu(MailMessage p0, string p1, bool p2, AttachmentCollection p3, out string p4, ref string p5)
	{
		if (p5 == null || 1 == 0)
		{
			p5 = "";
		}
		else
		{
			p5 = p5.Trim();
		}
		if (p5.Length == 0 || 1 == 0)
		{
			if (p3 != null && 0 == 0)
			{
				p5 = brgjd.edcru("{0}{1:D5}", p1, p3.Count + 1);
			}
			else
			{
				p5 = brgjd.edcru("{0}00000", p1);
			}
			p2 = true;
		}
		string mediaType;
		if (p2 && 0 == 0)
		{
			if (p0 == null || 1 == 0)
			{
				mediaType = MediaType;
				string key;
				if ((key = mediaType) != null && 0 == 0)
				{
					if (czzgh.nolpd == null || 1 == 0)
					{
						czzgh.nolpd = new Dictionary<string, int>(6)
						{
							{ "application/octet-stream", 0 },
							{ "message/rfc822", 1 },
							{ "text/plain", 2 },
							{ "image/gif", 3 },
							{ "image/png", 4 },
							{ "image/jpeg", 5 }
						};
					}
					if (czzgh.nolpd.TryGetValue(key, out var value))
					{
						switch (value)
						{
						case 0:
							break;
						case 1:
							goto IL_0180;
						case 2:
							goto IL_0194;
						case 3:
							goto IL_01a8;
						case 4:
							goto IL_01b9;
						case 5:
							goto IL_01ca;
						default:
							goto IL_01db;
						}
						p5 += ".bin";
						goto IL_0222;
					}
				}
				goto IL_01db;
			}
			p5 += ".eml";
		}
		goto IL_0222;
		IL_01a8:
		p5 += ".gif";
		goto IL_0222;
		IL_0180:
		p5 += ".eml";
		goto IL_0222;
		IL_0222:
		p4 = p5;
		string text = gtkha(p5);
		etqyt(text, out var p6, out var p7);
		if (p3 != null && 0 == 0)
		{
			int num = 1;
			do
			{
				bool flag = true;
				int num2 = 0;
				if (num2 != 0)
				{
					goto IL_0257;
				}
				goto IL_0282;
				IL_0282:
				if (num2 < p3.Count)
				{
					goto IL_0257;
				}
				goto IL_028d;
				IL_0257:
				if (p3[num2].FileName == text && 0 == 0)
				{
					flag = false;
					if (!flag)
					{
						goto IL_028d;
					}
				}
				num2++;
				goto IL_0282;
				IL_028d:
				if (flag ? true : false)
				{
					break;
				}
				text = brgjd.edcru("{0}({1}){2}", p6, num, p7);
				num++;
			}
			while (num < 100);
		}
		p5 = text;
		return;
		IL_01ca:
		p5 += ".jpg";
		goto IL_0222;
		IL_0194:
		p5 += ".txt";
		goto IL_0222;
		IL_01b9:
		p5 += ".png";
		goto IL_0222;
		IL_01db:
		int num3 = mediaType.IndexOf('/');
		if (num3 >= 0)
		{
			p5 = p5 + "." + mediaType.Substring(num3 + 1);
		}
		else
		{
			p5 += ".dat";
		}
		goto IL_0222;
	}

	private static void etqyt(string p0, out string p1, out string p2)
	{
		int num = p0.LastIndexOf('.');
		if (num >= 0)
		{
			p1 = p0.Substring(0, num);
			p2 = p0.Substring(num);
		}
		else
		{
			p1 = p0;
			p2 = string.Empty;
		}
	}

	internal static string boiem(string p0)
	{
		string p1 = ((!(p0 != string.Empty)) ? "body.rtf" : (p0 + ".rtf"));
		return gtkha(p1);
	}

	internal static string gtkha(string p0)
	{
		return jficx(p0, null);
	}

	internal static string jficx(string p0, char? p1)
	{
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		if (num != 0)
		{
			goto IL_0012;
		}
		goto IL_007e;
		IL_0012:
		char c = p0[num];
		if (c < ' ' || Array.IndexOf(bxkhb, c, 0, bxkhb.Length) >= 0 || (c == ':' && num != 1))
		{
			if (p1.HasValue && 0 == 0)
			{
				stringBuilder.Append(p1.Value);
			}
			else
			{
				stringBuilder.dlvlk("%{0:X2}", (int)c);
			}
		}
		else
		{
			stringBuilder.Append(c);
		}
		num++;
		goto IL_007e;
		IL_007e:
		if (num < p0.Length)
		{
			goto IL_0012;
		}
		return stringBuilder.ToString();
	}
}
