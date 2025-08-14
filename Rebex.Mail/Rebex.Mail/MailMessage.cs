using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;
using Rebex.Mime;
using Rebex.Mime.Headers;
using Rebex.OutlookMessages;
using Rebex.Security.Certificates;
using Rebex.Security.Cryptography.Pkcs;
using onrkn;

namespace Rebex.Mail;

public class MailMessage
{
	private enum mmucn
	{
		nlzaq = -1,
		vjkno,
		vtxou,
		hzodg,
		hhpti,
		jmona,
		tlzsq
	}

	internal enum ihasp
	{
		ecmnp,
		prjtm,
		wyzct
	}

	private class adptk : FileStream
	{
		private const int cqako = 4;

		public long qthpd;

		private bool nhtay;

		private readonly byte[] goyvp;

		public adptk(string path)
			: base(path, FileMode.Open)
		{
			goyvp = new byte[4];
		}

		private byte bbkpg(byte[] p0, int p1)
		{
			if (p1 >= 0)
			{
				return p0[p1];
			}
			return goyvp[4 + p1];
		}

		public override int Read(byte[] array, int offset, int count)
		{
			if (nhtay && 0 == 0)
			{
				return 0;
			}
			int num = base.Read(array, offset, count);
			if (num == 0 || 1 == 0)
			{
				return num;
			}
			int num2 = 0;
			if (num2 != 0)
			{
				goto IL_0031;
			}
			goto IL_006d;
			IL_0031:
			if (array[num2] == 10)
			{
				int num3 = num2 - 1;
				byte b = bbkpg(array, num3);
				if (b == 13)
				{
					num3--;
					b = bbkpg(array, num3);
				}
				if (b == 10)
				{
					num = num2 + 1;
					nhtay = true;
					goto IL_0071;
				}
			}
			num2++;
			goto IL_006d;
			IL_006d:
			if (num2 < num)
			{
				goto IL_0031;
			}
			goto IL_0071;
			IL_0071:
			if (!nhtay || 1 == 0)
			{
				if (num >= 4)
				{
					Array.Copy(array, offset + num - 4, goyvp, 0, 4);
				}
				else
				{
					Array.Copy(goyvp, num, goyvp, 0, 3 - num);
					Array.Copy(array, 0, goyvp, 4 - num, num);
				}
			}
			qthpd += num;
			return num;
		}
	}

	[NonSerialized]
	private MailSettings nykiq;

	[NonSerialized]
	private Encoding iqmpa;

	[NonSerialized]
	private EventHandler<MimeUnparsableHeaderEventArgs> dauyl;

	[NonSerialized]
	private EventHandler<MimeParsingHeaderEventArgs> wrzkm;

	[NonSerialized]
	private string fkxmc;

	[NonSerialized]
	private MimeEntity bqtqx;

	[NonSerialized]
	private MimeMessage zidhi;

	[NonSerialized]
	private AttachmentCollection yapte;

	[NonSerialized]
	private AlternateViewCollection iccuz;

	[NonSerialized]
	private LinkedResourceCollection gxxjr;

	[NonSerialized]
	private MimeEntity uwvee;

	[NonSerialized]
	private MimeEntity xwnqq;

	[NonSerialized]
	private MimeEntity maxwa;

	[NonSerialized]
	private SubjectInfoCollection hoayu;

	[NonSerialized]
	private SubjectInfoCollection fjxio;

	[NonSerialized]
	private mmucn stzmz;

	[NonSerialized]
	private mmucn meosg;

	[NonSerialized]
	private ICertificateFinder hpvcu;

	[NonSerialized]
	private bool tnlnk;

	public ICertificateFinder CertificateFinder
	{
		get
		{
			return hpvcu;
		}
		set
		{
			if (hpvcu != value)
			{
				hpvcu = value;
				if (xwnqq != null && 0 == 0)
				{
					xwnqq.CertificateFinder = hpvcu;
				}
			}
		}
	}

	public bool Silent
	{
		get
		{
			return tnlnk;
		}
		set
		{
			tnlnk = value;
		}
	}

	public bool IsDraft
	{
		get
		{
			return Headers.GetRaw("X-Unsent") == "1";
		}
		set
		{
			if (value && 0 == 0)
			{
				Headers["X-Unsent"] = new MimeHeader("X-Unsent", "1");
			}
			else
			{
				Headers.Remove("X-Unsent");
			}
		}
	}

	private bool yxkqw
	{
		get
		{
			return Headers.GetRaw("X-Unread") != "1";
		}
		set
		{
			if (!value || 1 == 0)
			{
				Headers["X-Unread"] = new MimeHeader("X-Unread", "1");
			}
			else
			{
				Headers.Remove("X-Unread");
			}
		}
	}

	public bool ReadOnly => stzmz != mmucn.vjkno;

	public bool IsSigned
	{
		get
		{
			switch (stzmz)
			{
			case mmucn.hzodg:
			case mmucn.jmona:
				return true;
			default:
				return false;
			}
		}
	}

	public bool IsEncrypted
	{
		get
		{
			switch (stzmz)
			{
			case mmucn.vtxou:
			case mmucn.hhpti:
			case mmucn.jmona:
				return true;
			default:
				return false;
			}
		}
	}

	public bool CanDecrypt
	{
		get
		{
			if (!IsEncrypted || false || xwnqq == null)
			{
				return false;
			}
			EnvelopedData envelopedContentInfo = xwnqq.EnvelopedContentInfo;
			if (envelopedContentInfo == null || 1 == 0)
			{
				throw new MailException("Missing enveloped content info. This should never happen.");
			}
			if (envelopedContentInfo.HasPrivateKey && 0 == 0)
			{
				return true;
			}
			if (meosg == mmucn.vjkno || false || meosg == mmucn.hzodg)
			{
				return envelopedContentInfo.AcquirePrivateKey();
			}
			return false;
		}
	}

	public MailEncryptionAlgorithm EncryptionAlgorithm
	{
		get
		{
			if (!IsEncrypted || false || xwnqq == null)
			{
				return MailEncryptionAlgorithm.None;
			}
			return wcxxf.ojbcd(xwnqq.EnvelopedContentInfo);
		}
	}

	public SubjectInfoCollection Recipients => hoayu;

	public SubjectInfoCollection Signers => fjxio;

	public MailDateTime Date
	{
		get
		{
			MailDateTime mailDateTime = zidhi.Date;
			if (mailDateTime == null || 1 == 0)
			{
				mailDateTime = ReceivedDate;
			}
			if (mailDateTime == null || false || mailDateTime.UniversalTime.Year > DateTime.Now.Year + 10)
			{
				return null;
			}
			return mailDateTime;
		}
		set
		{
			zidhi.Date = value;
		}
	}

	public MailDateTime ReceivedDate => wcxxf.pxify(Headers);

	public MailAddressCollection From
	{
		get
		{
			return zidhi.From;
		}
		set
		{
			zidhi.From = value;
		}
	}

	public MailAddress Sender
	{
		get
		{
			return zidhi.Sender;
		}
		set
		{
			zidhi.Sender = value;
		}
	}

	public MailAddressCollection ReplyTo
	{
		get
		{
			return zidhi.ReplyTo;
		}
		set
		{
			zidhi.ReplyTo = value;
		}
	}

	public MailAddressCollection To
	{
		get
		{
			return zidhi.To;
		}
		set
		{
			zidhi.To = value;
		}
	}

	public MailAddressCollection CC
	{
		get
		{
			return zidhi.CC;
		}
		set
		{
			zidhi.CC = value;
		}
	}

	public MailAddressCollection Bcc
	{
		get
		{
			return zidhi.Bcc;
		}
		set
		{
			zidhi.Bcc = value;
		}
	}

	public MessageId MessageId
	{
		get
		{
			return zidhi.MessageId;
		}
		set
		{
			zidhi.MessageId = value;
		}
	}

	public MessageIdCollection InReplyTo
	{
		get
		{
			return zidhi.InReplyTo;
		}
		set
		{
			zidhi.InReplyTo = value;
		}
	}

	public string Subject
	{
		get
		{
			return zidhi.Subject.Replace('\t', ' ');
		}
		set
		{
			zidhi.Subject = value;
		}
	}

	public MimeHeaderCollection Headers => zidhi.Headers;

	public AttachmentCollection Attachments => yapte;

	public AlternateViewCollection AlternateViews => iccuz;

	public LinkedResourceCollection Resources => gxxjr;

	public MessageIdCollection References
	{
		get
		{
			return zidhi.References;
		}
		set
		{
			zidhi.References = value;
		}
	}

	public string EnvelopeId
	{
		get
		{
			return zidhi.EnvelopeId;
		}
		set
		{
			zidhi.EnvelopeId = value;
		}
	}

	public MailPriority Priority
	{
		get
		{
			return (MailPriority)zidhi.Priority;
		}
		set
		{
			zidhi.Priority = (MimePriority)value;
		}
	}

	internal ihasp wiyqw
	{
		get
		{
			MimeHeader mimeHeader = zidhi.Headers["Importance"];
			if (mimeHeader == null || 1 == 0)
			{
				mimeHeader = zidhi.Headers["X-Importance"];
			}
			if (mimeHeader == null || 1 == 0)
			{
				return ihasp.prjtm;
			}
			string text = mimeHeader.Raw.ToLower(CultureInfo.InvariantCulture);
			if (text.IndexOf("low") >= 0)
			{
				return ihasp.ecmnp;
			}
			if (text.IndexOf("hi") >= 0)
			{
				return ihasp.wyzct;
			}
			if (text.Length == 1)
			{
				if (text[0] == '2')
				{
					return ihasp.wyzct;
				}
				if (text[0] == '1')
				{
					return ihasp.wyzct;
				}
			}
			return ihasp.prjtm;
		}
		set
		{
			string value2 = value switch
			{
				ihasp.ecmnp => "Low", 
				ihasp.prjtm => "Normal", 
				ihasp.wyzct => "High", 
				_ => throw hifyx.nztrs("value", value, "Invalid importance."), 
			};
			zidhi.Headers["Importance"] = new MimeHeader("Importance", value2);
			zidhi.Headers.Remove("X-Importance");
		}
	}

	public bool HasBodyHtml
	{
		get
		{
			AlternateView alternateView = henhp("text/html");
			return alternateView != null;
		}
	}

	public bool HasBodyText
	{
		get
		{
			AlternateView alternateView = henhp("text/plain");
			return alternateView != null;
		}
	}

	public string BodyHtml
	{
		get
		{
			AlternateView alternateView = henhp("text/html");
			if (alternateView == null || 1 == 0)
			{
				return "";
			}
			return alternateView.ContentString;
		}
		set
		{
			if (bqtqx != null && 0 == 0)
			{
				throw new MailException("Modifying message content is not possible for messages loaded without parsing the MIME tree.");
			}
			AlternateView alternateView = henhp("text/html");
			if (value == null || false || value.Length == 0 || 1 == 0)
			{
				if (alternateView != null && 0 == 0)
				{
					iccuz.wzxkg.Remove(alternateView);
				}
				return;
			}
			if (alternateView == null || 1 == 0)
			{
				MimeEntity entity = new MimeEntity();
				alternateView = new AlternateView(entity);
				iccuz.Add(alternateView);
			}
			value = xvcgm(value, p1: false, null);
			if (iqmpa != null && 0 == 0)
			{
				alternateView.SetContent(value, "text/html", iqmpa);
			}
			else
			{
				alternateView.SetContent(value, "text/html");
			}
			alternateView.jyvko.DefaultCharset = iqmpa;
		}
	}

	public string BodyText
	{
		get
		{
			AlternateView alternateView = henhp("text/plain");
			if (alternateView == null || 1 == 0)
			{
				return "";
			}
			return alternateView.ContentString;
		}
		set
		{
			if (bqtqx != null && 0 == 0)
			{
				throw new MailException("Modifying message content is not possible for messages loaded without parsing the MIME tree.");
			}
			AlternateView alternateView = henhp("text/plain");
			if (value == null || 1 == 0)
			{
				if (alternateView != null && 0 == 0)
				{
					iccuz.wzxkg.Remove(alternateView);
				}
				return;
			}
			if (alternateView == null || 1 == 0)
			{
				MimeEntity entity = new MimeEntity();
				alternateView = new AlternateView(entity);
				iccuz.Insert(0, alternateView);
			}
			value = xvcgm(value, p1: false, null);
			if (iqmpa != null && 0 == 0)
			{
				alternateView.SetContent(value, "text/plain", iqmpa);
			}
			else
			{
				alternateView.SetContent(value, "text/plain");
			}
			alternateView.jyvko.DefaultCharset = iqmpa;
		}
	}

	public Encoding DefaultCharset
	{
		get
		{
			return iqmpa;
		}
		set
		{
			iqmpa = value;
		}
	}

	[wptwl(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Options property has been deprecated. Please use Settings property instead.", false)]
	public MimeOptions Options
	{
		get
		{
			return nykiq.hsrhh();
		}
		set
		{
			if (nykiq.tfuau > 1)
			{
				throw new InvalidOperationException("Invalid call of old API. Use the Settings properties instead.");
			}
			nykiq.mbcjp(value);
		}
	}

	public MailSettings Settings
	{
		get
		{
			return nykiq;
		}
		set
		{
			if (value == null || 1 == 0)
			{
				throw new ArgumentNullException("value", "Value cannot be null.");
			}
			nykiq = value;
			nykiq.tfuau++;
		}
	}

	public event EventHandler<MimeUnparsableHeaderEventArgs> UnparsableHeader
	{
		add
		{
			dauyl = (EventHandler<MimeUnparsableHeaderEventArgs>)Delegate.Combine(dauyl, value);
		}
		remove
		{
			dauyl = (EventHandler<MimeUnparsableHeaderEventArgs>)Delegate.Remove(dauyl, value);
		}
	}

	public event EventHandler<MimeParsingHeaderEventArgs> ParsingHeader
	{
		add
		{
			wrzkm = (EventHandler<MimeParsingHeaderEventArgs>)Delegate.Combine(wrzkm, value);
		}
		remove
		{
			wrzkm = (EventHandler<MimeParsingHeaderEventArgs>)Delegate.Remove(wrzkm, value);
		}
	}

	private void fxioh(bool p0)
	{
		yapte.pubjq = p0;
		iccuz.oomcm = p0;
		gxxjr.tvgwf = p0;
	}

	public MailMessage Clone()
	{
		MailMessage mailMessage = new MailMessage();
		mailMessage.nykiq = nykiq;
		mailMessage.iqmpa = iqmpa;
		mailMessage.fkxmc = fkxmc;
		mailMessage.zidhi = zidhi.ToMessage();
		mailMessage.yapte = yapte.zwpay();
		mailMessage.yapte.mhqha = mailMessage;
		mailMessage.iccuz = iccuz.momyw();
		mailMessage.gxxjr = gxxjr.euifs();
		mailMessage.stzmz = stzmz;
		mailMessage.meosg = meosg;
		MimeEntity mimeEntity;
		if (uwvee != null && 0 == 0)
		{
			mimeEntity = (mailMessage.uwvee = uwvee.Clone());
		}
		else
		{
			mimeEntity = null;
			if (maxwa != null && 0 == 0)
			{
				mailMessage.maxwa = maxwa.Clone();
				mailMessage.fjxio = new SubjectInfoCollection(mailMessage.maxwa.SignedContentInfo);
			}
		}
		while (mimeEntity != null)
		{
			switch (mimeEntity.Kind)
			{
			case MimeEntityKind.Enveloped:
				if (mailMessage.xwnqq == null || 1 == 0)
				{
					mailMessage.xwnqq = mimeEntity;
					mailMessage.hoayu = new SubjectInfoCollection(mailMessage.xwnqq.EnvelopedContentInfo);
				}
				break;
			case MimeEntityKind.Signed:
				if (mailMessage.maxwa == null || 1 == 0)
				{
					mailMessage.maxwa = mimeEntity;
					mailMessage.fjxio = new SubjectInfoCollection(mailMessage.maxwa.SignedContentInfo);
				}
				break;
			}
			mimeEntity = mimeEntity.ContentMessage;
		}
		mailMessage.tnlnk = tnlnk;
		mailMessage.hpvcu = hpvcu;
		return mailMessage;
	}

	public MailMessage CreateReply(MailAddress sender, ReplyBodyTransformation transformation)
	{
		return CreateReply(sender, transformation, replyToAll: false);
	}

	public MailMessage CreateReply(MailAddress sender, ReplyBodyTransformation transformation, bool replyToAll)
	{
		if (sender == null || 1 == 0)
		{
			throw new ArgumentNullException("sender");
		}
		MailMessage mailMessage = new MailMessage();
		switch (transformation)
		{
		case ReplyBodyTransformation.KeepOriginal:
			if (HasBodyText && 0 == 0)
			{
				mailMessage.BodyText = BodyText;
			}
			if (HasBodyHtml && 0 == 0)
			{
				mailMessage.BodyHtml = BodyHtml;
			}
			break;
		case ReplyBodyTransformation.Attachment:
			mailMessage.Attachments.Add(new Attachment(this));
			break;
		default:
			throw new ArgumentOutOfRangeException("transformation");
		case ReplyBodyTransformation.None:
			break;
		}
		mailMessage.MessageId = new MessageId();
		MessageIdCollection messageIdCollection;
		bool flag;
		int num;
		if (MessageId != null && 0 == 0)
		{
			mailMessage.InReplyTo.Add(MessageId);
			MimeHeader mimeHeader = Headers["References"];
			if (mimeHeader != null && 0 == 0)
			{
				messageIdCollection = mimeHeader.Value as MessageIdCollection;
				if (messageIdCollection != null && 0 == 0)
				{
					messageIdCollection = (MessageIdCollection)messageIdCollection.Clone();
				}
			}
			else
			{
				messageIdCollection = new MessageIdCollection();
			}
			while (messageIdCollection.Count > 9)
			{
				messageIdCollection.RemoveAt(1);
			}
			flag = true;
			num = 0;
			if (num != 0)
			{
				goto IL_0123;
			}
			goto IL_0155;
		}
		goto IL_018e;
		IL_0123:
		if (messageIdCollection[num].Id == MessageId.Id && 0 == 0)
		{
			flag = false;
			if (!flag)
			{
				goto IL_015f;
			}
		}
		num++;
		goto IL_0155;
		IL_0155:
		if (num < messageIdCollection.Count)
		{
			goto IL_0123;
		}
		goto IL_015f;
		IL_015f:
		if (flag && 0 == 0)
		{
			messageIdCollection.Add(MessageId);
		}
		mailMessage.Headers.Add(new MimeHeader("References", messageIdCollection));
		goto IL_018e;
		IL_0233:
		int num2 = 0;
		MailAddressCollection mailAddressCollection;
		int num3;
		string text = mailAddressCollection[num3].Address.ToLower(CultureInfo.InvariantCulture);
		int num4 = 0;
		if (num4 != 0)
		{
			goto IL_025a;
		}
		goto IL_02cb;
		IL_018e:
		mailMessage.From = sender;
		mailAddressCollection = new MailAddressCollection();
		if (replyToAll && 0 == 0)
		{
			mailAddressCollection.AddRange(From);
			mailAddressCollection.AddRange(ReplyTo);
			mailAddressCollection.AddRange(To);
		}
		else if (ReplyTo != null && 0 == 0 && ReplyTo.Count > 0)
		{
			mailAddressCollection.AddRange(ReplyTo);
		}
		else
		{
			mailAddressCollection.AddRange(From);
		}
		MailAddressCollection cC = CC;
		string text2 = sender.Address.ToLower(CultureInfo.InvariantCulture);
		num3 = 0;
		if (num3 != 0)
		{
			goto IL_0233;
		}
		goto IL_0357;
		IL_0357:
		if (num3 >= mailAddressCollection.Count)
		{
			if (replyToAll && 0 == 0)
			{
				mailMessage.CC = cC;
			}
			mailMessage.To = mailAddressCollection;
			mailMessage.Subject = ("RE: " + Subject).Trim();
			return mailMessage;
		}
		goto IL_0233;
		IL_02e0:
		int num5;
		string text3 = cC[num5].Address.ToLower(CultureInfo.InvariantCulture);
		if (text2 == text3 && 0 == 0)
		{
			cC.RemoveAt(num5);
			num5--;
		}
		else if (text == text3 && 0 == 0)
		{
			cC.RemoveAt(num5);
			num5--;
		}
		num5++;
		goto IL_0346;
		IL_0346:
		if (num5 < cC.Count)
		{
			goto IL_02e0;
		}
		num3++;
		goto IL_0357;
		IL_02cb:
		if (num4 < mailAddressCollection.Count)
		{
			goto IL_025a;
		}
		num5 = 0;
		if (num5 != 0)
		{
			goto IL_02e0;
		}
		goto IL_0346;
		IL_025a:
		string text4 = mailAddressCollection[num4].Address.ToLower(CultureInfo.InvariantCulture);
		if (text2 == text4 && 0 == 0)
		{
			mailAddressCollection.RemoveAt(num4);
			num4--;
		}
		else if (text == text4 && 0 == 0)
		{
			num2++;
			if (num2 > 1)
			{
				mailAddressCollection.RemoveAt(num4);
				num4--;
			}
		}
		num4++;
		goto IL_02cb;
	}

	public MailMessage()
	{
		tnlnk = true;
		hpvcu = Rebex.Security.Cryptography.Pkcs.CertificateFinder.Default;
		bamim();
		zidhi.MessageId = new MessageId();
		zidhi.Date = new MailDateTime(DateTime.Now);
		Settings = new MailSettings();
	}

	public MailMessage(MimeMessage message)
		: this()
	{
		if (message == null || 1 == 0)
		{
			throw new ArgumentNullException("message");
		}
		mmbgg(message);
		tnlnk = message.Silent;
		hpvcu = message.CertificateFinder;
	}

	private void bamim()
	{
		zidhi = new MimeMessage();
		yapte = new AttachmentCollection();
		yapte.mhqha = this;
		iccuz = new AlternateViewCollection();
		gxxjr = new LinkedResourceCollection();
		fkxmc = null;
		hoayu = new SubjectInfoCollection();
		fjxio = new SubjectInfoCollection();
		bqtqx = null;
		stzmz = mmucn.vjkno;
		meosg = mmucn.vjkno;
		maxwa = null;
		uwvee = null;
		xwnqq = null;
	}

	public static implicit operator MailMessage(MimeMessage message)
	{
		if (message == null || 1 == 0)
		{
			throw new ArgumentNullException("message");
		}
		return new MailMessage(message);
	}

	private Attachment argjn(string p0, string p1)
	{
		IEnumerator<Attachment> enumerator = yapte.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				Attachment current = enumerator.Current;
				if (current.ContentType.MediaType == p0 && 0 == 0 && (p1 == null || false || current.FileName == p1))
				{
					return current;
				}
			}
		}
		finally
		{
			if (enumerator != null && 0 == 0)
			{
				enumerator.Dispose();
			}
		}
		return null;
	}

	private AlternateView henhp(string p0)
	{
		int p1;
		return xwqtq(p0, out p1);
	}

	private AlternateView xwqtq(string p0, out int p1)
	{
		int num = 0;
		if (num != 0)
		{
			goto IL_0006;
		}
		goto IL_0040;
		IL_0006:
		if (iccuz[num].ContentType.MediaType == p0 && 0 == 0)
		{
			p1 = num;
			return iccuz[num];
		}
		num++;
		goto IL_0040;
		IL_0040:
		if (num >= iccuz.Count)
		{
			p1 = -1;
			return null;
		}
		goto IL_0006;
	}

	private AttachmentBase lrjmk()
	{
		int p;
		return yvghu(p0: true, out p);
	}

	private AttachmentBase yvghu(bool p0, out int p1)
	{
		AttachmentBase attachmentBase = xwqtq("text/rtf", out p1);
		if (attachmentBase != null && 0 == 0)
		{
			return attachmentBase;
		}
		attachmentBase = xwqtq("application/rtf", out p1);
		if (attachmentBase != null && 0 == 0)
		{
			return attachmentBase;
		}
		p1 = -1;
		Attachment attachment = null;
		int num = 0;
		if (num != 0)
		{
			goto IL_0045;
		}
		goto IL_00b8;
		IL_0045:
		Attachment attachment2 = yapte[num];
		if (!(attachment2.ContentType.MediaType != "application/rtf") || 1 == 0)
		{
			if (attachment2.jyvko.Headers.GetRaw("x-rebex-rtf-body") == "1" && 0 == 0)
			{
				return attachment2;
			}
			if ((!p0 || 1 == 0) && (attachment == null || 1 == 0))
			{
				attachment = attachment2;
			}
		}
		num++;
		goto IL_00b8;
		IL_00b8:
		if (num < yapte.Count)
		{
			goto IL_0045;
		}
		return attachment;
	}

	private static string xvcgm(string p0, bool p1, string p2)
	{
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		if (num != 0)
		{
			goto IL_0012;
		}
		goto IL_00a1;
		IL_0012:
		char c = p0[num];
		if (c >= ' ' && c != '\u007f')
		{
			stringBuilder.Append(c);
		}
		else
		{
			switch (c)
			{
			case '\t':
				stringBuilder.Append(c);
				break;
			case '\n':
				if (p1 && 0 == 0 && p2 != null && 0 == 0)
				{
					stringBuilder.Append(p2);
				}
				else
				{
					stringBuilder.Append("\r\n");
				}
				break;
			default:
				if (p2 != null && 0 == 0)
				{
					stringBuilder.Append(p2);
				}
				break;
			case '\r':
				break;
			}
		}
		num++;
		goto IL_00a1;
		IL_00a1:
		if (num < p0.Length)
		{
			goto IL_0012;
		}
		return stringBuilder.ToString();
	}

	private static Unstructured xrpej(string p0)
	{
		p0 = xvcgm(p0, p1: true, "\t");
		return new Unstructured(p0, checkData: false);
	}

	private static void myvkc(MimeEntity p0)
	{
		int num = 0;
		if (num != 0)
		{
			goto IL_0006;
		}
		goto IL_0052;
		IL_0006:
		MimeHeader mimeHeader = p0.Headers[num];
		string text = mimeHeader.Name.ToLower(CultureInfo.InvariantCulture);
		if (!text.StartsWith("content-") || 1 == 0)
		{
			p0.Headers.RemoveAt(num);
			num--;
		}
		num++;
		goto IL_0052;
		IL_0052:
		if (num >= p0.Headers.Count)
		{
			return;
		}
		goto IL_0006;
	}

	private static void pfaci(MimeEntity p0, MimeEntity p1, bool p2)
	{
		int num = 0;
		if (num != 0)
		{
			goto IL_0009;
		}
		goto IL_0050;
		IL_0009:
		MimeHeader mimeHeader = p1.Headers[num];
		string text = mimeHeader.Name.ToLower(CultureInfo.InvariantCulture);
		if (text.StartsWith("content-") && 0 == 0)
		{
			p1.Headers.RemoveAt(num);
			num--;
		}
		num++;
		goto IL_0050;
		IL_0050:
		if (num < p1.Headers.Count)
		{
			goto IL_0009;
		}
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_0066;
		}
		goto IL_00dc;
		IL_00dc:
		if (num2 >= p0.Headers.Count)
		{
			return;
		}
		goto IL_0066;
		IL_0066:
		MimeHeader mimeHeader2 = p0.Headers[num2];
		string text2 = mimeHeader2.Name.ToLower(CultureInfo.InvariantCulture);
		mimeHeader2 = mimeHeader2.Clone();
		if (text2.StartsWith("content-") && 0 == 0)
		{
			if (!p2 || 1 == 0)
			{
				p1.Headers[text2] = mimeHeader2;
			}
		}
		else
		{
			p1.Headers.Add(mimeHeader2);
		}
		num2++;
		goto IL_00dc;
	}

	private static MimeEntity oxxck(AlternateView p0, Type p1)
	{
		if ((object)p1 == typeof(MimeMessage))
		{
			return p0.jyvko.ToMessage();
		}
		return p0.jyvko.Clone();
	}

	private MimeEntity rjxml(Type p0, bool p1, bool p2)
	{
		switch (iccuz.Count)
		{
		case 0:
			return (MimeEntity)Activator.CreateInstance(p0);
		case 1:
			if ((nykiq.DisableSinglePartHtmlWorkaround ? true : false) || (p2 ? true : false) || !p1 || false || !(iccuz[0].ContentType.MediaType != "text/plain") || 1 == 0)
			{
				return oxxck(iccuz[0], p0);
			}
			break;
		}
		MimeEntity mimeEntity = (MimeEntity)Activator.CreateInstance(p0);
		mimeEntity.Preamble = "";
		mimeEntity.ContentType = new ContentType("multipart/alternative");
		int num = 0;
		if (num != 0)
		{
			goto IL_00c3;
		}
		goto IL_00ee;
		IL_00c3:
		mimeEntity.Parts.Add(oxxck(iccuz[num], typeof(MimeEntity)));
		num++;
		goto IL_00ee;
		IL_00ee:
		if (num < iccuz.Count)
		{
			goto IL_00c3;
		}
		return mimeEntity;
	}

	private MimeEntity tnllz(Type p0, bool p1, bool p2)
	{
		if (gxxjr.Count == 0 || 1 == 0)
		{
			return rjxml(p0, p1, p2);
		}
		MimeEntity mimeEntity = rjxml(typeof(MimeEntity), p1: false, p2);
		MimeEntity mimeEntity2 = (MimeEntity)Activator.CreateInstance(p0);
		mimeEntity2.ContentType = new ContentType("multipart/related");
		mimeEntity2.ContentType.Parameters["type"] = mimeEntity.ContentType.MediaType;
		mimeEntity2.Parts.Add(mimeEntity);
		int num = 0;
		if (num != 0)
		{
			goto IL_0083;
		}
		goto IL_00a9;
		IL_00a9:
		if (num < gxxjr.Count)
		{
			goto IL_0083;
		}
		return mimeEntity2;
		IL_0083:
		mimeEntity2.Parts.Add(gxxjr[num].jyvko.Clone());
		num++;
		goto IL_00a9;
	}

	public MimeMessage ToMimeMessage()
	{
		return qlivp(p0: true);
	}

	private MimeEntity qxnkb(Type p0, bool p1)
	{
		if (bqtqx != null && 0 == 0)
		{
			return bqtqx.Clone();
		}
		MimeEntity mimeEntity;
		int num;
		if (yapte.Count > 0)
		{
			MimeEntity entity = tnllz(typeof(MimeEntity), p1: false, p1);
			mimeEntity = (MimeEntity)Activator.CreateInstance(p0);
			if (fkxmc != null && 0 == 0 && iccuz.Count == 1 && yapte.Count >= 1)
			{
				mimeEntity.ContentType = new ContentType("multipart/report");
				if (fkxmc.Length > 0)
				{
					mimeEntity.ContentType.Parameters["report-type"] = fkxmc;
				}
			}
			else
			{
				mimeEntity.ContentType = new ContentType("multipart/mixed");
			}
			mimeEntity.Parts.Add(entity);
			num = 0;
			if (num != 0)
			{
				goto IL_00d8;
			}
			goto IL_00f9;
		}
		return tnllz(p0, p1: true, p1);
		IL_00f9:
		if (num < yapte.Count)
		{
			goto IL_00d8;
		}
		return mimeEntity;
		IL_00d8:
		mimeEntity.Parts.Add(yapte[num].akqjb());
		num++;
		goto IL_00f9;
	}

	private MimeMessage qlivp(bool p0)
	{
		MimeMessage mimeMessage;
		if (stzmz != mmucn.vjkno && 0 == 0)
		{
			if (uwvee == null || 1 == 0)
			{
				throw new MailException("Cannot save or encrypt the message because the signed content has been modified. Remove the signature first.");
			}
			mimeMessage = uwvee.ToMessage();
			myvkc(mimeMessage);
		}
		else
		{
			mimeMessage = (MimeMessage)qxnkb(typeof(MimeMessage), p1: false);
		}
		if (p0 && 0 == 0)
		{
			pfaci(zidhi, mimeMessage, p2: false);
			mimeMessage.EnvelopeId = zidhi.EnvelopeId;
			if (!mimeMessage.ReadOnly || 1 == 0)
			{
				mimeMessage.Preamble = zidhi.Preamble;
				mimeMessage.Epilogue = zidhi.Epilogue;
			}
		}
		mimeMessage.Options = nykiq.hsrhh();
		mimeMessage.DefaultCharset = iqmpa;
		return mimeMessage;
	}

	private bool qljus(byte[] p0, int p1, int p2)
	{
		if (p2 >= 8)
		{
			return jlfbq.epwpf(p0, p1) == -2226271756974174256L;
		}
		return false;
	}

	public void Load(Stream input)
	{
		if (input == null || 1 == 0)
		{
			throw new ArgumentNullException("input");
		}
		byte[] array = new byte[8];
		int num = 0;
		if (num != 0)
		{
			goto IL_0025;
		}
		goto IL_0042;
		IL_0048:
		if (qljus(array, 0, num) && 0 == 0)
		{
			jfxnb jfxnb = new jfxnb();
			if (!input.CanSeek || 1 == 0)
			{
				throw new NotSupportedException("Only seekable streams are supported for loading MSG files.");
			}
			input.Position = 0L;
			input.Flush();
			try
			{
				jfxnb.xewra.yedya = nykiq.pdktx;
				jfxnb.xewra.zojzk = nykiq.pdktx;
				jfxnb.nhnbp(input);
				wpfeu(jfxnb);
				return;
			}
			catch (uwkib inner)
			{
				throw new MailException("Error while parsing compound MSG file.", inner);
			}
			catch (MsgMessageException inner2)
			{
				throw new MailException("Error while parsing compound MSG file.", inner2);
			}
		}
		MimeMessage mimeMessage = new MimeMessage();
		mimeMessage.Options = nykiq.hsrhh();
		mimeMessage.DefaultCharset = iqmpa;
		mimeMessage.UnparsableHeader += dauyl;
		mimeMessage.ParsingHeader += wrzkm;
		mimeMessage.Silent = tnlnk;
		mimeMessage.CertificateFinder = hpvcu;
		mimeMessage.Load(new jiqyi(array, input, num));
		mmbgg(mimeMessage);
		return;
		IL_0042:
		if (num < array.Length)
		{
			goto IL_0025;
		}
		goto IL_0048;
		IL_0025:
		int num2 = input.Read(array, num, array.Length - num);
		if (num2 != 0 && 0 == 0)
		{
			num += num2;
			goto IL_0042;
		}
		goto IL_0048;
	}

	public void Load(string fileName)
	{
		if (fileName == null || 1 == 0)
		{
			throw new ArgumentNullException("fileName");
		}
		if (fileName.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("Path cannot be empty.", "fileName");
		}
		Stream stream = File.OpenRead(fileName);
		try
		{
			Load(stream);
		}
		finally
		{
			if (stream != null && 0 == 0)
			{
				((IDisposable)stream).Dispose();
			}
		}
	}

	public void Load(byte[] data)
	{
		MemoryStream input = new MemoryStream(data, writable: false);
		Load(input);
	}

	private void gxcxk(MimeEntity p0)
	{
		if (!p0.IsMultipart || 1 == 0)
		{
			if (p0.Kind == MimeEntityKind.Signed && p0.ContentMessage != null && 0 == 0)
			{
				gxcxk(p0.ContentMessage);
				if (p0.ylgbh != null && 0 == 0)
				{
					puurx(p0.ylgbh);
				}
			}
			else if (p0.ContentType.MediaType.StartsWith("text/") && 0 == 0)
			{
				iccuz.Add(new AlternateView(p0));
			}
			else
			{
				if ((!p0.ReadOnly || 1 == 0) && p0.ContentType.MediaType == "application/rtf" && 0 == 0)
				{
					p0.Headers.tyaam("x-rebex-rtf-body", new Unparsed("1"));
				}
				yapte.Add(new Attachment(p0, yapte));
			}
			return;
		}
		string mediaType;
		if ((mediaType = p0.ContentType.MediaType) != null && 0 == 0)
		{
			if (czzgh.obkop == null || 1 == 0)
			{
				czzgh.obkop = new Dictionary<string, int>(6)
				{
					{ "multipart/mixed", 0 },
					{ "multipart/signed", 1 },
					{ "multipart/report", 2 },
					{ "multipart/specific", 3 },
					{ "multipart/alternative", 4 },
					{ "multipart/related", 5 }
				};
			}
			if (czzgh.obkop.TryGetValue(mediaType, out var value) && 0 == 0)
			{
				int num;
				switch (value)
				{
				case 4:
					num = 0;
					if (num != 0)
					{
						goto IL_02e1;
					}
					goto IL_0306;
				case 5:
					{
						bfwpw(p0);
						return;
					}
					IL_0306:
					if (num >= p0.Parts.Count)
					{
						return;
					}
					goto IL_02e1;
					IL_02e1:
					gxcxk(p0.Parts[num]);
					num++;
					goto IL_0306;
				}
			}
		}
		bool flag;
		int num2;
		if (p0.Parts.Count > 0)
		{
			flag = false;
			num2 = 0;
			if (num2 != 0)
			{
				goto IL_01c4;
			}
			goto IL_02ab;
		}
		if (p0.ContentMessage != null && 0 == 0)
		{
			gxcxk(p0.ContentMessage);
		}
		return;
		IL_02ab:
		if (num2 >= p0.Parts.Count)
		{
			return;
		}
		goto IL_01c4;
		IL_02a7:
		num2++;
		goto IL_02ab;
		IL_01c4:
		MimeEntity mimeEntity = p0.Parts[num2];
		string text = mimeEntity.ContentDisposition?.Disposition;
		bool flag2 = text == "inline" && 0 == 0 && (mimeEntity.Kind == MimeEntityKind.Body || 1 == 0) && !nykiq.TreatMixedInlineAsAttachment;
		if (!flag || 1 == 0)
		{
			if (mimeEntity.Kind == MimeEntityKind.Multipart)
			{
				gxcxk(mimeEntity);
				flag = true;
				if (flag)
				{
					goto IL_02a7;
				}
			}
			if ((!(text == "attachment") || 1 == 0) && (!flag2 || 1 == 0))
			{
				gxcxk(mimeEntity);
				flag = true;
				if (flag)
				{
					goto IL_02a7;
				}
			}
		}
		if (flag2 && 0 == 0)
		{
			LinkedResource value2 = new LinkedResource(mimeEntity);
			gxxjr.Add(value2);
		}
		else
		{
			puurx(mimeEntity);
		}
		goto IL_02a7;
	}

	private void bfwpw(MimeEntity p0)
	{
		if (p0.Parts.Count == 0 || 1 == 0)
		{
			return;
		}
		MimeEntity mimeEntity = null;
		string text = p0.ContentType.Parameters["start"];
		if (text != null && 0 == 0)
		{
			IEnumerator enumerator = p0.Parts.GetEnumerator();
			try
			{
				while (enumerator.MoveNext() ? true : false)
				{
					MimeEntity mimeEntity2 = (MimeEntity)enumerator.Current;
					string text2 = ((mimeEntity2.ContentId != null) ? mimeEntity2.ContentId.ToString() : null);
					if (text2 == text && 0 == 0)
					{
						mimeEntity = mimeEntity2;
						break;
					}
				}
			}
			finally
			{
				if (enumerator is IDisposable disposable && 0 == 0)
				{
					disposable.Dispose();
				}
			}
		}
		if (mimeEntity == null || 1 == 0)
		{
			string text3 = p0.ContentType.Parameters["type"];
			if (text3 != null && 0 == 0)
			{
				IEnumerator enumerator2 = p0.Parts.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext() ? true : false)
					{
						MimeEntity mimeEntity3 = (MimeEntity)enumerator2.Current;
						if (mimeEntity3.ContentType.MediaType == text3 && 0 == 0)
						{
							mimeEntity = mimeEntity3;
							break;
						}
					}
				}
				finally
				{
					if (enumerator2 is IDisposable disposable2 && 0 == 0)
					{
						disposable2.Dispose();
					}
				}
			}
			if (mimeEntity == null || 1 == 0)
			{
				mimeEntity = p0.Parts[0];
				if ((text == null || 1 == 0) && (text3 == null || 1 == 0))
				{
					if (mimeEntity.IsMultipart && 0 == 0)
					{
						if (p0.Parts.Count == 1)
						{
							gxcxk(mimeEntity);
							return;
						}
					}
					else if (!mimeEntity.ContentType.MediaType.StartsWith("text/") || 1 == 0)
					{
						IEnumerator enumerator3 = p0.Parts.GetEnumerator();
						try
						{
							while (enumerator3.MoveNext() ? true : false)
							{
								MimeEntity mimeEntity4 = (MimeEntity)enumerator3.Current;
								if (mimeEntity4.ContentType.MediaType.StartsWith("text/") && 0 == 0)
								{
									mimeEntity = mimeEntity4;
									break;
								}
							}
						}
						finally
						{
							if (enumerator3 is IDisposable disposable3 && 0 == 0)
							{
								disposable3.Dispose();
							}
						}
					}
				}
			}
		}
		if (mimeEntity.IsMultipart && 0 == 0)
		{
			if (mimeEntity.ContentType.MediaType != "multipart/alternative" && 0 == 0)
			{
				puurx(p0);
				return;
			}
			gxcxk(mimeEntity);
		}
		else
		{
			if (!mimeEntity.ContentType.MediaType.StartsWith("text/"))
			{
				puurx(p0);
				return;
			}
			gxcxk(mimeEntity);
		}
		int num = 0;
		if (num != 0)
		{
			goto IL_02d1;
		}
		goto IL_03dd;
		IL_03dd:
		if (num >= p0.Parts.Count)
		{
			return;
		}
		goto IL_02d1;
		IL_02d1:
		MimeEntity mimeEntity5 = p0.Parts[num];
		if (mimeEntity5 != mimeEntity)
		{
			if (mimeEntity5.Kind != MimeEntityKind.Body && 0 == 0)
			{
				Attachment value = new Attachment(mimeEntity5.ToMessage(), mimeEntity5.Name, yapte);
				yapte.Add(value);
			}
			else
			{
				ContentDisposition contentDisposition = mimeEntity5.ContentDisposition;
				if (contentDisposition != null && 0 == 0 && contentDisposition.Disposition == "attachment" && 0 == 0 && mimeEntity5.Headers["Content-Location"] == null && mimeEntity5.Headers["Content-ID"] == null)
				{
					Attachment value2 = new Attachment(mimeEntity5, yapte);
					yapte.Add(value2);
				}
				else
				{
					LinkedResource value3 = new LinkedResource(mimeEntity5);
					gxxjr.Add(value3);
				}
			}
		}
		num++;
		goto IL_03dd;
	}

	private void puurx(MimeEntity p0)
	{
		if (!p0.IsMultipart || 1 == 0)
		{
			yapte.Add(new Attachment(p0, yapte));
			return;
		}
		int num = 0;
		if (num != 0)
		{
			goto IL_002e;
		}
		goto IL_004a;
		IL_004a:
		if (num >= p0.Parts.Count)
		{
			return;
		}
		goto IL_002e;
		IL_002e:
		puurx(p0.Parts[num]);
		num++;
		goto IL_004a;
	}

	private void mmbgg(MimeMessage p0)
	{
		bamim();
		meosg = mmucn.nlzaq;
		if (p0.ContentType.MediaType == "multipart/report" && 0 == 0)
		{
			fkxmc = p0.ContentType.Parameters["report-type"];
			if (fkxmc == null || 1 == 0)
			{
				fkxmc = "";
			}
		}
		pfaci(p0, zidhi, p2: true);
		zidhi.Preamble = p0.Preamble;
		zidhi.Epilogue = p0.Epilogue;
		zidhi.EnvelopeId = p0.EnvelopeId;
		jhayu(p0);
	}

	private void jhayu(MimeEntity p0)
	{
		okbpw(p0);
		if ((nykiq.DoNotParseMimeTree ? true : false) || (nykiq.OnlyParseHeaders ? true : false))
		{
			return;
		}
		bool pubjq = yapte.pubjq;
		if (pubjq && 0 == 0)
		{
			fxioh(p0: false);
		}
		if (!nykiq.SkipTnefMessageProcessing || 1 == 0)
		{
			try
			{
				wvkad.wcrle(this);
			}
			catch (Exception inner)
			{
				if (!nykiq.IgnoreInvalidTnefMessages || 1 == 0)
				{
					throw new MailException("Invalid TNEF message.", inner);
				}
			}
		}
		phnpj(p0: false);
		wltqx();
		if (pubjq && 0 == 0)
		{
			fxioh(p0: true);
		}
	}

	internal void phnpj(bool p0)
	{
		AlternateView alternateView = henhp("text/html");
		if (alternateView != null && 0 == 0 && (!p0 || 1 == 0))
		{
			return;
		}
		bool flag = nykiq.RtfMode != RtfProcessingMode.Legacy;
		int p1;
		AttachmentBase attachmentBase = yvghu(flag, out p1);
		if (alternateView != null && 0 == 0)
		{
			pezux(alternateView.ContentString);
			bxxon(attachmentBase, p1: true);
			return;
		}
		if (attachmentBase == null || 1 == 0)
		{
			return;
		}
		Encoding encoding = EncodingTools.GetEncoding("eightbit");
		string text = attachmentBase.jyvko.dhgcb(encoding, p1: false);
		venkc venkc = new venkc(text);
		if (!nykiq.racyr || 1 == 0)
		{
			bxxon(attachmentBase, p1: false);
			return;
		}
		bool flag2;
		string text2;
		int num;
		if (venkc.veelr && 0 == 0)
		{
			flag2 = true;
			text2 = venkc.savrf();
		}
		else
		{
			flag2 = false;
			text2 = venkc.ccduh(pieou.shemm);
			if (text2 == null || 1 == 0)
			{
				bxxon(attachmentBase, p1: false);
				return;
			}
			if (text2.IndexOf("<img border=0 src=\"cid:embedded-rtf-attachment-id", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				num = 0;
				if (num != 0)
				{
					goto IL_011e;
				}
				goto IL_022a;
			}
		}
		goto IL_023c;
		IL_022a:
		if (num < yapte.Count)
		{
			goto IL_011e;
		}
		goto IL_023c;
		IL_011e:
		Attachment attachment = yapte[num];
		string raw = attachment.jyvko.Headers.GetRaw("X-Content-ID");
		if (raw != null && 0 == 0)
		{
			attachment.jyvko.Headers.Remove("X-Content-ID");
			Stream contentStream = attachment.GetContentStream();
			bool flag3;
			try
			{
				flag3 = wyfgf.usxuw(contentStream);
			}
			finally
			{
				if (contentStream != null && 0 == 0)
				{
					((IDisposable)contentStream).Dispose();
				}
			}
			if (flag3 && 0 == 0)
			{
				if (text2.IndexOf(brgjd.edcru("<img border=0 src=\"cid:{0}\">", raw), StringComparison.OrdinalIgnoreCase) >= 0)
				{
					nkkmz(attachment, raw);
					Attachments.RemoveAt(num--);
				}
			}
			else
			{
				text2 = text2.Replace(brgjd.edcru("<img border=0 src=\"cid:{0}\">", raw), brgjd.edcru(" &lt;&lt;{0}&gt;&gt; ", attachment.FileName));
			}
		}
		num++;
		goto IL_022a;
		IL_023c:
		alternateView = new AlternateView();
		alternateView.SetContent(text2, "text/html", Encoding.UTF8);
		if (p1 < 0)
		{
			iccuz.Add(alternateView);
		}
		else
		{
			iccuz.Insert(p1, alternateView);
		}
		if ((!flag || 1 == 0) && attachmentBase is Attachment && 0 == 0)
		{
			attachmentBase.jyvko.Headers.tyaam("x-rebex-rtf-body", new Unparsed("1"));
		}
		bxxon(attachmentBase, p1: true);
		if (flag2 && 0 == 0)
		{
			pezux(text2);
		}
	}

	private void bxxon(AttachmentBase p0, bool p1)
	{
		if (p0 == null)
		{
			return;
		}
		switch (nykiq.RtfMode)
		{
		default:
			return;
		case RtfProcessingMode.Default:
			if (p1 && 0 == 0)
			{
				iccuz.wzxkg.Remove(p0);
				yapte.cmbhn.Remove(p0);
				return;
			}
			break;
		case RtfProcessingMode.Legacy:
			return;
		case RtfProcessingMode.TreatAsAttachment:
			break;
		}
		if (p0 is AlternateView && 0 == 0)
		{
			p0.jyvko.Headers.tyaam("x-rebex-rtf-body", new Unparsed("1"));
			iccuz.wzxkg.Remove(p0);
			Attachment value = new Attachment(p0.jyvko, yapte);
			yapte.Add(value);
		}
	}

	private void pezux(string p0)
	{
		if (string.IsNullOrEmpty(p0) && 0 == 0)
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_001e;
		}
		goto IL_00dc;
		IL_00dc:
		if (num >= yapte.Count)
		{
			return;
		}
		goto IL_001e;
		IL_001e:
		Attachment attachment = yapte[num];
		if (attachment.ContentId != null && 0 == 0)
		{
			string id = attachment.ContentId.Id;
			if (id != null && 0 == 0 && id.Length != 0 && 0 == 0 && p0.IndexOf("cid:" + id, StringComparison.Ordinal) >= 0)
			{
				Stream contentStream = attachment.GetContentStream();
				bool flag;
				try
				{
					flag = wyfgf.usxuw(contentStream);
				}
				finally
				{
					if (contentStream != null && 0 == 0)
					{
						((IDisposable)contentStream).Dispose();
					}
				}
				if (flag && 0 == 0)
				{
					nkkmz(attachment, id);
					Attachments.RemoveAt(num--);
				}
			}
		}
		num++;
		num2++;
		goto IL_00dc;
	}

	internal void nkkmz(Attachment p0, string p1)
	{
		LinkedResource linkedResource = new LinkedResource();
		linkedResource.Options = MimeOptions.AllowAnyTextCharacters;
		Stream contentStream = p0.GetContentStream();
		try
		{
			linkedResource.lraqa(contentStream, p0.FileName, p0.MediaType);
		}
		finally
		{
			if (contentStream != null && 0 == 0)
			{
				((IDisposable)contentStream).Dispose();
			}
		}
		linkedResource.ContentId = MessageId.nkxgc(p1);
		Resources.Add(linkedResource);
	}

	private void wltqx()
	{
		int num = 0;
		if (num != 0)
		{
			goto IL_000c;
		}
		goto IL_00c7;
		IL_000c:
		Attachment attachment = yapte[num];
		MimeEntity jyvko = attachment.jyvko;
		MimeEntity[] array2;
		int num2;
		if (jyvko != null && 0 == 0 && (!(jyvko.ContentTransferEncoding.ToString() != "uuencode") || 1 == 0))
		{
			MimeEntity[] array = nqccz.fhzrc(jyvko, p1: false);
			if (array.Length == 0 || 1 == 0)
			{
				array = nqccz.fhzrc(jyvko, p1: true);
			}
			if (array.Length > 0)
			{
				yapte.RemoveAt(num);
				num--;
				array2 = array;
				num2 = 0;
				if (num2 != 0)
				{
					goto IL_008d;
				}
				goto IL_00b3;
			}
		}
		goto IL_00bb;
		IL_00bb:
		num++;
		goto IL_00c7;
		IL_00b3:
		if (num2 < array2.Length)
		{
			goto IL_008d;
		}
		goto IL_00bb;
		IL_00c7:
		if (num >= yapte.Count)
		{
			return;
		}
		goto IL_000c;
		IL_008d:
		MimeEntity entity = array2[num2];
		yapte.Add(new Attachment(entity, yapte));
		num2++;
		goto IL_00b3;
	}

	internal void okbpw(MimeEntity p0)
	{
		switch (p0.Kind)
		{
		case MimeEntityKind.Enveloped:
			if (uwvee == null || 1 == 0)
			{
				uwvee = p0;
			}
			if (xwnqq == null || 1 == 0)
			{
				fxioh(p0: true);
				xwnqq = p0;
				hoayu = new SubjectInfoCollection(xwnqq.EnvelopedContentInfo);
				if (stzmz == mmucn.hzodg)
				{
					stzmz = mmucn.jmona;
				}
				else
				{
					stzmz = mmucn.vtxou;
				}
				return;
			}
			break;
		case MimeEntityKind.Signed:
			if (uwvee == null || 1 == 0)
			{
				uwvee = p0;
			}
			if (maxwa == null || 1 == 0)
			{
				maxwa = p0;
				fjxio = new SubjectInfoCollection(maxwa.SignedContentInfo);
				stzmz = mmucn.hzodg;
			}
			okbpw(p0.ContentMessage);
			return;
		}
		MimeEntity[] array;
		int num;
		if (p0.IsMultipart && 0 == 0)
		{
			gxcxk(p0);
		}
		else
		{
			string text = p0.ContentDisposition?.Disposition;
			string mediaType = p0.ContentType.MediaType;
			if (mediaType.StartsWith("multipart/") && 0 == 0)
			{
				bqtqx = p0.Clone();
				myvkc(bqtqx);
				iccuz.oomcm = true;
				gxxjr.tvgwf = true;
				yapte.pubjq = true;
			}
			else
			{
				if (mediaType.StartsWith("text/") && 0 == 0 && (text == null || false || text == "inline"))
				{
					array = nqccz.fhzrc(p0, p1: false);
					iccuz.Add(new AlternateView(p0));
					num = 0;
					if (num != 0)
					{
						goto IL_01d3;
					}
					goto IL_01f4;
				}
				yapte.Add(new Attachment(p0, yapte));
			}
		}
		goto IL_0215;
		IL_01f4:
		if (num < array.Length)
		{
			goto IL_01d3;
		}
		goto IL_0215;
		IL_01d3:
		yapte.Add(new Attachment(array[num], yapte));
		num++;
		goto IL_01f4;
		IL_0215:
		if (maxwa != null && 0 == 0)
		{
			fxioh(p0: true);
		}
	}

	public void Save(Stream output)
	{
		Save(output, MailFormat.Mime);
	}

	public void Save(string fileName)
	{
		Save(fileName, MailFormat.Mime);
	}

	public void Save(Stream output, MailFormat format)
	{
		if (output == null || 1 == 0)
		{
			throw new ArgumentNullException("output");
		}
		switch (format)
		{
		case MailFormat.Mime:
		{
			MimeEntity mimeEntity = ToMimeMessage();
			mimeEntity.DefaultCharset = iqmpa;
			mimeEntity.Save(output);
			break;
		}
		case MailFormat.OutlookMsg:
		case (MailFormat)2:
			ossea(output, format);
			break;
		default:
			throw hifyx.nztrs("format", format, "Invalid message format.");
		}
	}

	public void Save(string fileName, MailFormat format)
	{
		if (fileName == null || 1 == 0)
		{
			throw new ArgumentNullException("fileName");
		}
		MimeEntity mimeEntity;
		switch (format)
		{
		case MailFormat.Mime:
			mimeEntity = ToMimeMessage();
			mimeEntity.DefaultCharset = iqmpa;
			break;
		case MailFormat.OutlookMsg:
		case (MailFormat)2:
			mimeEntity = null;
			break;
		default:
			throw hifyx.nztrs("format", format, "Invalid message format.");
		}
		Stream stream = vtdxm.namiu(fileName, pieou.msydj);
		try
		{
			if (mimeEntity == null || 1 == 0)
			{
				ossea(stream, format);
			}
			else
			{
				mimeEntity.Save(stream);
			}
		}
		finally
		{
			if (stream != null && 0 == 0)
			{
				((IDisposable)stream).Dispose();
			}
		}
	}

	private void ossea(Stream p0, MailFormat p1)
	{
		try
		{
			jfxnb jfxnb = rjacl(p1);
			jfxnb.ucqrd(p0);
		}
		catch (uwkib inner)
		{
			throw new MailException("Error while constructing compound MSG file.", inner);
		}
		catch (MsgMessageException inner2)
		{
			throw new MailException("Error while constructing compound MSG file.", inner2);
		}
	}

	private jfxnb rjacl(MailFormat p0)
	{
		jfxnb jfxnb = new jfxnb();
		if (p0 == MailFormat.OutlookMsg)
		{
			jfxnb.kfurm = true;
		}
		MimeEntity mimeEntity = qlivp(p0: false);
		if (uwvee != null && 0 == 0)
		{
			if (uwvee.Kind == MimeEntityKind.Signed && uwvee.SignedContentInfo.Detached)
			{
				mimeEntity.DefaultCharset = iqmpa;
				StringWriter stringWriter = new StringWriter();
				uwvee.ContentType.Encode(stringWriter);
				jfxnb.pcmji(mimeEntity.ToStream(), stringWriter.ToString());
			}
			else
			{
				eorvm eorvm = new eorvm();
				switch (uwvee.Kind)
				{
				case MimeEntityKind.Enveloped:
					uwvee.EnvelopedContentInfo.Save(eorvm);
					break;
				case MimeEntityKind.Signed:
					uwvee.SignedContentInfo.Save(eorvm);
					break;
				default:
					throw new InvalidOperationException("Invalid message structure.");
				}
				StringWriter stringWriter2 = new StringWriter();
				uwvee.ContentType.Encode(stringWriter2);
				jfxnb.lgpbz(eorvm, stringWriter2.ToString(), uwvee.ContentTransferEncoding.ToString());
			}
		}
		else
		{
			string raw = Headers.GetRaw("X-Outlook-MessageClass");
			if (raw != null && 0 == 0 && ((raw.Equals("IPM.Post", StringComparison.OrdinalIgnoreCase) ? true : false) || raw.Equals("REPORT.IPM.Note.IPNRN", StringComparison.OrdinalIgnoreCase)))
			{
				jfxnb.icirf = raw;
			}
			AlternateView alternateView = henhp("text/plain");
			AlternateView alternateView2 = henhp("text/html");
			AttachmentBase attachmentBase = lrjmk();
			if (alternateView2 != null && 0 == 0 && alternateView2.Charset != null)
			{
				nvhaf(jfxnb, alternateView2.Charset);
			}
			else if (attachmentBase != null && 0 == 0 && attachmentBase.Charset != null)
			{
				nvhaf(jfxnb, attachmentBase.Charset);
			}
			else if (alternateView != null && 0 == 0 && alternateView.Charset != null && 0 == 0)
			{
				nvhaf(jfxnb, alternateView.Charset);
			}
			if (alternateView2 != null && 0 == 0 && alternateView2.ContentId != null)
			{
				jfxnb.suuuu.qrqdp(MsgPropertyTag.BodyContentId, alternateView2.ContentId.Id);
			}
			else if (attachmentBase != null && 0 == 0 && attachmentBase.ContentId != null)
			{
				jfxnb.suuuu.qrqdp(MsgPropertyTag.BodyContentId, attachmentBase.ContentId.Id);
			}
			else if (alternateView != null && 0 == 0 && alternateView.ContentId != null && 0 == 0)
			{
				jfxnb.suuuu.qrqdp(MsgPropertyTag.BodyContentId, alternateView.ContentId.Id);
			}
			if (alternateView2 != null && 0 == 0 && alternateView2.ContentLocation != null)
			{
				jfxnb.suuuu.qrqdp(MsgPropertyTag.BodyContentLocation, alternateView2.ContentLocation);
			}
			else if (attachmentBase != null && 0 == 0 && attachmentBase.ContentLocation != null)
			{
				jfxnb.suuuu.qrqdp(MsgPropertyTag.BodyContentLocation, attachmentBase.ContentLocation);
			}
			else if (alternateView != null && 0 == 0 && alternateView.ContentLocation != null && 0 == 0)
			{
				jfxnb.suuuu.qrqdp(MsgPropertyTag.BodyContentLocation, alternateView.ContentLocation);
			}
			if (alternateView != null && 0 == 0)
			{
				if ((alternateView2 == null || 1 == 0) && attachmentBase == null)
				{
					jfxnb.lxdmx(alternateView.ContentString);
				}
				else
				{
					jfxnb.ggdfi = alternateView.ContentString;
				}
			}
			if (alternateView2 != null && 0 == 0)
			{
				jfxnb.wslzq(alternateView2.ContentString);
			}
			if (nykiq.pjfau && 0 == 0)
			{
				jfxnb.ggdfi = null;
			}
			if (attachmentBase != null && 0 == 0 && (alternateView2 == null || 1 == 0))
			{
				if (alternateView == null || 1 == 0)
				{
					jfxnb.ggdfi = string.Empty;
				}
				if (attachmentBase.MediaType == "text/rtf" && 0 == 0)
				{
					jfxnb.pufqr = attachmentBase.ContentString;
				}
				else
				{
					int num = (int)attachmentBase.GetContentLength();
					Stream contentStream = attachmentBase.GetContentStream();
					try
					{
						byte[] array = new byte[num];
						while (num > 0)
						{
							num -= contentStream.Read(array, array.Length - num, num);
						}
						jfxnb.pufqr = EncodingTools.GetEncoding("eightbit").GetString(array, 0, array.Length);
					}
					finally
					{
						if (contentStream != null && 0 == 0)
						{
							((IDisposable)contentStream).Dispose();
						}
					}
				}
			}
			IEnumerator<LinkedResource> enumerator = gxxjr.GetEnumerator();
			try
			{
				while (enumerator.MoveNext() ? true : false)
				{
					LinkedResource current = enumerator.Current;
					cfhyk(jfxnb, current, current.FileName, null);
				}
			}
			finally
			{
				if (enumerator != null && 0 == 0)
				{
					enumerator.Dispose();
				}
			}
			IEnumerator<Attachment> enumerator2 = yapte.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext() ? true : false)
				{
					Attachment current2 = enumerator2.Current;
					if (current2 == attachmentBase)
					{
						continue;
					}
					string text = current2.DisplayName;
					MailMessage contentMessage = current2.ContentMessage;
					if (contentMessage != null && 0 == 0)
					{
						jfxnb jfxnb2 = contentMessage.rjacl(p0);
						if (string.IsNullOrEmpty(text) && 0 == 0)
						{
							text = jfxnb2.soumj;
							if (string.IsNullOrEmpty(text) && 0 == 0)
							{
								text = current2.ContentDescription;
							}
						}
						wyfgf wyfgf = jfxnb.clqou.rgcby(jfxnb2, text);
						if (!current2.fkzie || 1 == 0)
						{
							wyfgf.vbjpq.duwaw(MsgPropertyTag.AttachFilename, current2.FileName);
						}
					}
					else
					{
						if ((current2.fkzie ? true : false) || string.IsNullOrEmpty(text))
						{
							text = null;
						}
						cfhyk(jfxnb, current2, current2.FileName, text);
					}
				}
			}
			finally
			{
				if (enumerator2 != null && 0 == 0)
				{
					enumerator2.Dispose();
				}
			}
		}
		if (MessageId != null && 0 == 0)
		{
			jfxnb.takfk = MessageId.Id;
		}
		if (Headers["subject"] != null && 0 == 0)
		{
			jfxnb.soumj = Subject;
		}
		if (Headers["date"] != null && 0 == 0 && Date != null && 0 == 0)
		{
			jfxnb.ixaxw = Date.UniversalTime;
		}
		if ((!mefcv(jfxnb, MsgPropertyTag.MessageDeliveryTime, "X-Outlook-MessageDeliveryDate") || 1 == 0) && Headers["received"] != null && 0 == 0 && ReceivedDate != null && 0 == 0)
		{
			jfxnb.bkaju = ReceivedDate.UniversalTime;
		}
		if (Sender != null && 0 == 0)
		{
			jfxnb.untou = Sender.Address;
			jfxnb.kazjp = jhnhd(Sender);
		}
		else if (From.Count > 0)
		{
			jfxnb.untou = From[0].Address;
			jfxnb.kazjp = jhnhd(From[0]);
		}
		else
		{
			ijuru(jfxnb, MsgPropertyTag.SenderName, "X-Outlook-SenderName");
			ijuru(jfxnb, MsgPropertyTag.SenderAddressType, "X-Outlook-SenderAddressType");
			ijuru(jfxnb, MsgPropertyTag.SenderEmailAddress, "X-Outlook-SenderEmailAddress");
		}
		if (From.Count > 0)
		{
			jfxnb.mzhkh = From[0].Address;
			jfxnb.ofwbk = jhnhd(From[0]);
		}
		else
		{
			ijuru(jfxnb, MsgPropertyTag.SentRepresentingName, "X-Outlook-SentRepresentingName");
			ijuru(jfxnb, MsgPropertyTag.SentRepresentingAddressType, "X-Outlook-SentRepresentingAddressType");
			ijuru(jfxnb, MsgPropertyTag.SentRepresentingEmailAddress, "X-Outlook-SentRepresentingEmailAddress");
		}
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_0830;
		}
		goto IL_0868;
		IL_09ba:
		int num3;
		MimeEntity mimeEntity2;
		if (num3 < mimeEntity2.Headers.Count)
		{
			goto IL_0931;
		}
		int num4 = 0;
		if (num4 != 0)
		{
			goto IL_09d4;
		}
		goto IL_0a1f;
		IL_09d4:
		MimeHeader mimeHeader = mimeEntity.Headers[num4];
		if (mimeHeader.Name.StartsWith("content-", StringComparison.OrdinalIgnoreCase) && 0 == 0)
		{
			mimeEntity2.Headers[mimeHeader.Name] = mimeHeader.Clone();
		}
		num4++;
		goto IL_0a1f;
		IL_08b6:
		int num5;
		if (num5 < CC.Count)
		{
			goto IL_087e;
		}
		int num6 = 0;
		if (num6 != 0)
		{
			goto IL_08cc;
		}
		goto IL_0904;
		IL_0904:
		if (num6 < Bcc.Count)
		{
			goto IL_08cc;
		}
		MemoryStream memoryStream = new MemoryStream();
		mimeEntity2 = zidhi.Clone();
		num3 = 0;
		if (num3 != 0)
		{
			goto IL_0931;
		}
		goto IL_09ba;
		IL_08cc:
		jfxnb.rsaru.kobgy(zccyb.zgczd, Bcc[num6].Address, jhnhd(Bcc[num6]));
		num6++;
		goto IL_0904;
		IL_0a1f:
		if (num4 >= mimeEntity.Headers.Count)
		{
			mimeEntity2.ContentType = mimeEntity.ContentType;
			mimeEntity2.DefaultCharset = iqmpa;
			mimeEntity2.Save(memoryStream);
			string text2 = EncodingTools.Default.GetString(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
			int num7 = text2.IndexOf("\r\n\r\n");
			if (num7 >= 0)
			{
				text2 = text2.Substring(0, num7 + 4);
			}
			jfxnb.kmqzx = text2;
			jfxnb.irsrm = (oshbb)Priority;
			jfxnb.bzlqs = (asfub)wiyqw;
			jfxnb.hepvh = IsDraft;
			jfxnb.kfaju = yxkqw;
			zayhb(jfxnb, MsgPropertyTag.LastVerbExecuted, "X-Outlook-LastVerbExecuted");
			mefcv(jfxnb, MsgPropertyTag.LastVerbExecutionTime, "X-Outlook-LastVerbExecutionTime");
			zayhb(jfxnb, MsgPropertyTag.IconIndex, "X-Outlook-IconIndex");
			ijuru(jfxnb, MsgPropertyTag.OriginalSubject, "X-Outlook-OriginalSubject");
			ijuru(jfxnb, MsgPropertyTag.ReportText, "X-Outlook-ReportText");
			mefcv(jfxnb, MsgPropertyTag.ReportTime, "X-Outlook-ReportTime");
			mefcv(jfxnb, MsgPropertyTag.OriginalSubmitTime, "X-Outlook-OriginalSubmitTime");
			mefcv(jfxnb, MsgPropertyTag.OriginalDeliveryTime, "X-Outlook-OriginalDeliveryTime");
			ijuru(jfxnb, MsgPropertyTag.OriginalDisplayTo, "X-Outlook-OriginalDisplayTo");
			ijuru(jfxnb, MsgPropertyTag.OriginalDisplayCc, "X-Outlook-OriginalDisplayCc");
			ijuru(jfxnb, MsgPropertyTag.OriginalDisplayBcc, "X-Outlook-OriginalDisplayBcc");
			ijuru(jfxnb, MsgPropertyTag.OriginalDisplayName, "X-Outlook-OriginalDisplayName");
			ijuru(jfxnb, MsgPropertyTag.OriginalSentRepresentingName, "X-Outlook-OriginalSentRepresentingName");
			ijuru(jfxnb, MsgPropertyTag.OriginalSentRepresentingAddressType, "X-Outlook-OriginalRepresentingAddressType");
			ijuru(jfxnb, MsgPropertyTag.OriginalSentRepresentingEmailAddress, "X-Outlook-OriginalRepresentingEmailAddress");
			ijuru(jfxnb, MsgPropertyTag.OriginalSenderName, "X-Outlook-OriginalSenderName");
			ijuru(jfxnb, MsgPropertyTag.OriginalSenderAddressType, "X-Outlook-OriginalSenderAddressType");
			ijuru(jfxnb, MsgPropertyTag.OriginalSenderEmailAddress, "X-Outlook-OriginalSenderEmailAddress");
			if (Headers["Keywords"] != null && 0 == 0 && Headers["Keywords"].Value != null && 0 == 0)
			{
				string text3 = Headers["Keywords"].Value.ToString();
				List<string> list = new List<string>();
				int num8 = 0;
				int num9 = text3.IndexOf(',', num8);
				while (true)
				{
					if (num9 == -1)
					{
						num9 = text3.Length;
					}
					if (num9 > num8)
					{
						string text4 = text3.Substring(num8, num9 - num8).Trim();
						if (text4.Length > 0)
						{
							list.Add(text4);
						}
					}
					if (num9 == text3.Length)
					{
						break;
					}
					num8 = num9 + 1;
					num9 = text3.IndexOf(',', num8);
				}
				if (list.Count > 0)
				{
					jfxnb.hkkgy("Keywords", MsgPropertySet.PublicStrings, list.ToArray());
				}
			}
			return jfxnb;
		}
		goto IL_09d4;
		IL_0830:
		jfxnb.rsaru.kobgy(zccyb.pqplk, To[num2].Address, jhnhd(To[num2]));
		num2++;
		goto IL_0868;
		IL_0931:
		string text5 = mimeEntity2.Headers[num3].Name.ToLower(CultureInfo.InvariantCulture);
		string text6;
		if (text5.StartsWith("x-outlook-", StringComparison.Ordinal) && 0 == 0)
		{
			mimeEntity2.Headers.RemoveAt(num3--);
		}
		else if ((text6 = text5) != null && 0 == 0 && text6 == "x-unsent" && 0 == 0)
		{
			mimeEntity2.Headers.RemoveAt(num3--);
		}
		num3++;
		goto IL_09ba;
		IL_0868:
		if (num2 < To.Count)
		{
			goto IL_0830;
		}
		num5 = 0;
		if (num5 != 0)
		{
			goto IL_087e;
		}
		goto IL_08b6;
		IL_087e:
		jfxnb.rsaru.kobgy(zccyb.hfokr, CC[num5].Address, jhnhd(CC[num5]));
		num5++;
		goto IL_08b6;
	}

	private string jhnhd(MailAddress p0)
	{
		if (p0.DisplayName != null && 0 == 0 && p0.DisplayName.Length > 0)
		{
			return p0.DisplayName;
		}
		if (p0.Address != null && 0 == 0 && p0.Address.Length > 0)
		{
			return p0.Address;
		}
		return null;
	}

	private void ijuru(jfxnb p0, MsgPropertyTag p1, string p2)
	{
		string raw = Headers.GetRaw(p2);
		if (raw != null && 0 == 0)
		{
			p0.suuuu.duwaw(p1, raw);
		}
	}

	private void zayhb(jfxnb p0, MsgPropertyTag p1, string p2)
	{
		if (brgjd.bnrqx(Headers.GetRaw(p2), out var p3) && 0 == 0)
		{
			p0.suuuu.mycww(p1, p3);
		}
	}

	private bool mefcv(jfxnb p0, MsgPropertyTag p1, string p2)
	{
		if (brgjd.nbusd(Headers.GetRaw(p2), out var p3) && 0 == 0)
		{
			p0.suuuu.bvloj(p1, p3);
			return true;
		}
		return false;
	}

	private void cfhyk(jfxnb p0, AttachmentBase p1, string p2, string p3)
	{
		string mediaType = p1.MediaType;
		string charSet = p1.ContentType.CharSet;
		string text = ((p1.ContentId == null) ? null : p1.ContentId.Id);
		string contentLocation = p1.ContentLocation;
		Stream contentStream = p1.GetContentStream();
		try
		{
			if (p1 is LinkedResource && 0 == 0 && (text != null || contentLocation != null) && ((mediaType.StartsWith("image/", StringComparison.OrdinalIgnoreCase) ? true : false) || (mediaType.Equals("text/css", StringComparison.OrdinalIgnoreCase) ? true : false) || (mediaType.Equals("text/javascript", StringComparison.OrdinalIgnoreCase) ? true : false) || mediaType.Equals("application/javascript", StringComparison.OrdinalIgnoreCase)))
			{
				p0.clqou.ihdlz(contentStream, p2, mediaType, charSet, text, contentLocation);
			}
			else
			{
				p0.clqou.derde(contentStream, p2, p3, mediaType, charSet, text, contentLocation);
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

	private void nvhaf(jfxnb p0, Encoding p1)
	{
		if (p0.kfurm && 0 == 0)
		{
			p0.gpgdq(p1.CodePage);
		}
		else
		{
			p0.zxaar = p1;
		}
	}

	public byte[] ToByteArray()
	{
		MemoryStream memoryStream = new MemoryStream();
		Save(memoryStream);
		return memoryStream.ToArray();
	}

	private MimeEntity lmbyo(string p0, byte[] p1, string p2)
	{
		byte[] bytes = EncodingTools.ASCII.GetBytes(p0);
		jiqyi p3 = new jiqyi(bytes, new MemoryStream(p1, writable: false), 0L);
		MimeEntity mimeEntity = wmenc(p3);
		TransferEncoding transferEncoding = TransferEncoding.Base64;
		if (p2 != null && 0 == 0)
		{
			MimeHeader mimeHeader = new MimeHeader("content-transfer-encoding", p2);
			if (mimeHeader.Value is ContentTransferEncoding contentTransferEncoding && 0 == 0)
			{
				transferEncoding = contentTransferEncoding.Encoding;
			}
		}
		mimeEntity.TransferEncoding = transferEncoding;
		zidhi.Preamble = null;
		zidhi.Epilogue = null;
		return mimeEntity;
	}

	private MimeEntity wmenc(Stream p0)
	{
		MimeEntity mimeEntity = new MimeEntity();
		mimeEntity.Silent = tnlnk;
		mimeEntity.CertificateFinder = hpvcu;
		mimeEntity.Options = nykiq.hsrhh() & ~(MimeOptions.DoNotParseMimeTree | MimeOptions.OnlyParseHeaders | MimeOptions.KeepRawEntityBody | MimeOptions.DoNotCloseStreamAfterLoad | MimeOptions.DoNotPreloadAttachments);
		mimeEntity.DefaultCharset = iqmpa;
		mimeEntity.UnparsableHeader += dauyl;
		mimeEntity.ParsingHeader += wrzkm;
		mimeEntity.Load(p0);
		return mimeEntity;
	}

	private void wpfeu(jfxnb p0)
	{
		bamim();
		meosg = mmucn.nlzaq;
		string text = null;
		string key;
		byte[] array;
		if ((key = p0.icirf.ToLower(CultureInfo.InvariantCulture)) != null && 0 == 0)
		{
			if (czzgh.zmgbe == null || 1 == 0)
			{
				czzgh.zmgbe = new Dictionary<string, int>(6)
				{
					{ "ipm.note.smime", 0 },
					{ "ipm.note.secure", 1 },
					{ "ipm.note.secure.sign", 2 },
					{ "ipm.note.smime.multipartsigned", 3 },
					{ "ipm.note", 4 },
					{ "report.ipm.note.ipnrn", 5 }
				};
			}
			if (czzgh.zmgbe.TryGetValue(key, out var value))
			{
				switch (value)
				{
				case 0:
				case 1:
					break;
				case 2:
				case 3:
					goto IL_01d4;
				case 4:
					goto IL_020f;
				case 5:
					if (nykiq.LoadMsgProperties && 0 == 0)
					{
						noged<string>(p0.suuuu, MsgPropertyTag.OriginalSubject, "X-Outlook-OriginalSubject");
						noged<string>(p0.suuuu, MsgPropertyTag.ReportText, "X-Outlook-ReportText");
						noged<DateTime?>(p0.suuuu, MsgPropertyTag.ReportTime, "X-Outlook-ReportTime");
						noged<DateTime?>(p0.suuuu, MsgPropertyTag.OriginalSubmitTime, "X-Outlook-OriginalSubmitTime");
						noged<DateTime?>(p0.suuuu, MsgPropertyTag.OriginalDeliveryTime, "X-Outlook-OriginalDeliveryTime");
						noged<string>(p0.suuuu, MsgPropertyTag.OriginalDisplayTo, "X-Outlook-OriginalDisplayTo");
						noged<string>(p0.suuuu, MsgPropertyTag.OriginalDisplayCc, "X-Outlook-OriginalDisplayCc");
						noged<string>(p0.suuuu, MsgPropertyTag.OriginalDisplayBcc, "X-Outlook-OriginalDisplayBcc");
						noged<string>(p0.suuuu, MsgPropertyTag.OriginalDisplayName, "X-Outlook-OriginalDisplayName");
						noged<string>(p0.suuuu, MsgPropertyTag.OriginalSentRepresentingName, "X-Outlook-OriginalSentRepresentingName");
						noged<string>(p0.suuuu, MsgPropertyTag.OriginalSentRepresentingAddressType, "X-Outlook-OriginalRepresentingAddressType");
						noged<string>(p0.suuuu, MsgPropertyTag.OriginalSentRepresentingEmailAddress, "X-Outlook-OriginalRepresentingEmailAddress");
						noged<string>(p0.suuuu, MsgPropertyTag.OriginalSenderName, "X-Outlook-OriginalSenderName");
						noged<string>(p0.suuuu, MsgPropertyTag.OriginalSenderAddressType, "X-Outlook-OriginalSenderAddressType");
						noged<string>(p0.suuuu, MsgPropertyTag.OriginalSenderEmailAddress, "X-Outlook-OriginalSenderEmailAddress");
					}
					goto IL_0c59;
				default:
					goto IL_0c59;
				}
				wyfgf wyfgf = pokwk(p0);
				array = (byte[])wyfgf.rcncd;
				string p1 = p0.suuuu.hlyhv<string>("content-transfer-encoding");
				if (SignedData.IsSignedData(array, 0, array.Length) && 0 == 0)
				{
					string p2 = "Content-Type: application/pkcs7-mime; name=smime.p7m; smime-type=signed-data\r\nContent-Transfer-Encoding: binary\r\n\r\n";
					uwvee = (maxwa = lmbyo(p2, array, p1));
					fjxio = new SubjectInfoCollection(maxwa.SignedContentInfo);
					stzmz = mmucn.hzodg;
					okbpw(maxwa.ContentMessage);
				}
				else
				{
					if (!EnvelopedData.IsEnvelopedData(array, 0, array.Length))
					{
						throw new MailException("Secured content was not recognized.");
					}
					string p3 = "Content-Type: application/pkcs7-mime; name=smime.p7m; smime-type=enveloped-data\r\nContent-Transfer-Encoding: binary\r\n\r\n";
					uwvee = (xwnqq = lmbyo(p3, array, p1));
					hoayu = new SubjectInfoCollection(xwnqq.EnvelopedContentInfo);
					stzmz = mmucn.vtxou;
					fxioh(p0: true);
				}
				lmdma(p0);
				goto IL_0c65;
			}
		}
		goto IL_0c59;
		IL_020f:
		Encoding encoding = p0.chnlh(p0: true);
		if (!EncodingTools.yuhur(encoding) || 1 == 0)
		{
			encoding = Encoding.UTF8;
		}
		lmdma(p0);
		string text2 = p0.ggdfi;
		if (!p0.kfurm || false || text2 == null)
		{
			if (p0.suuuu.unzoh<bool>(MsgPropertyTag.RtfInSync) && 0 == 0 && p0.ehdmx() != null)
			{
				text2 = p0.ehdmx();
			}
			else
			{
				byte[] array2 = p0.suuuu.hlyhv<byte[]>("Internet Charset Body");
				if (array2 != null && 0 == 0)
				{
					if (!EncodingTools.bzzgu(array2, out var p4) || 1 == 0)
					{
						p4 = p0.chnlh(p0: false);
					}
					text2 = p4.GetString(array2, 0, array2.Length);
				}
			}
		}
		if (text2 != null && 0 == 0)
		{
			tyvvt(iccuz, p0, text2, "text/plain", encoding);
		}
		Dictionary<int, int> dictionary = null;
		bool flag = nykiq.RtfMode != RtfProcessingMode.Default;
		string text3 = string.Empty;
		if (p0.ljlfy != null && 0 == 0)
		{
			text3 = p0.ljlfy;
		}
		else if ((!string.IsNullOrEmpty(p0.pufqr) || 1 == 0) && (p0.ehdmx() == null || 1 == 0) && nykiq.racyr && 0 == 0)
		{
			text3 = p0.xdokw.ccduh(pieou.shemm);
			if (text3 == null || 1 == 0)
			{
				text3 = string.Empty;
				flag = true;
				if (flag)
				{
					goto IL_03b1;
				}
			}
			dictionary = tmoco(p0);
		}
		goto IL_03b1;
		IL_070f:
		bool flag2;
		ContentType contentType;
		if (!flag2 || 1 == 0)
		{
			if (array.Length >= 3 && array[0] == 239 && array[1] == 187 && array[2] == 191)
			{
				contentType.CharSet = "utf-8";
			}
			else if (iqmpa != null && 0 == 0)
			{
				contentType.CharSet = iqmpa.WebName.ToLower(CultureInfo.InvariantCulture);
			}
			else
			{
				contentType.CharSet = EncodingTools.Default.WebName.ToLower(CultureInfo.InvariantCulture);
			}
		}
		goto IL_0799;
		IL_0c65:
		if ((Headers["X-Outlook-MessageClass"] == null || 1 == 0) && text != null && 0 == 0)
		{
			Headers["X-Outlook-MessageClass"] = new MimeHeader("X-Outlook-MessageClass", xrpej(text));
		}
		IsDraft = p0.hepvh;
		yxkqw = p0.kfaju;
		noged<int?>(p0.suuuu, MsgPropertyTag.LastVerbExecuted, "X-Outlook-LastVerbExecuted");
		idovr<DateTime?>(p0.suuuu, MsgPropertyTag.LastVerbExecutionTime, "X-Outlook-LastVerbExecutionTime", "{0:r}");
		oighe<int?>(p0.suuuu, MsgPropertyTag.IconIndex, "X-Outlook-IconIndex", -1);
		int num;
		if (nykiq.LoadMsgProperties && 0 == 0)
		{
			noged<string>(p0.suuuu, MsgPropertyTag.SentRepresentingName, "X-Outlook-SentRepresentingName");
			noged<string>(p0.suuuu, MsgPropertyTag.SentRepresentingAddressType, "X-Outlook-SentRepresentingAddressType");
			noged<string>(p0.suuuu, MsgPropertyTag.SentRepresentingEmailAddress, "X-Outlook-SentRepresentingEmailAddress");
			noged<string>(p0.suuuu, MsgPropertyTag.SenderName, "X-Outlook-SenderName");
			noged<string>(p0.suuuu, MsgPropertyTag.SenderAddressType, "X-Outlook-SenderAddressType");
			noged<string>(p0.suuuu, MsgPropertyTag.SenderEmailAddress, "X-Outlook-SenderEmailAddress");
			noged<string>(p0.suuuu, MsgPropertyTag.SenderSmtpAddress, "X-Outlook-SenderSmtpAddress");
			num = 0;
			if (num != 0)
			{
				goto IL_0dc2;
			}
			goto IL_0ee2;
		}
		goto IL_0f20;
		IL_0ad6:
		int num2 = num2 + 1;
		goto IL_0adc;
		IL_06f3:
		int num3;
		if (array[num3] >= 127)
		{
			flag2 = false;
			if (!flag2)
			{
				goto IL_070f;
			}
		}
		num3++;
		goto IL_0708;
		IL_0f70:
		string[] array3;
		int num4;
		string value2 = array3[num4];
		StringBuilder stringBuilder;
		if (stringBuilder.Length > 0)
		{
			stringBuilder.Append(", ");
		}
		stringBuilder.Append(value2);
		num4++;
		goto IL_0f9e;
		IL_04fb:
		wyfgf wyfgf2 = p0.clqou[num2];
		string text4 = wyfgf2.vbjpq.unzoh<string>(MsgPropertyTag.AttachMimeTag);
		contentType = ((text4 == null) ? null : (new MimeHeader("Content-type", text4).Value as ContentType));
		if (contentType == null || 1 == 0)
		{
			contentType = new ContentType("application/octet-stream");
		}
		else if (!ContentType.vgubl(contentType.MediaType) || 1 == 0)
		{
			contentType.dczab("application", "octet-stream");
		}
		bool flag3;
		if (wyfgf2.jgqfq && 0 == 0)
		{
			array = null;
			flag3 = true;
			if (flag3)
			{
				goto IL_05d7;
			}
		}
		array = (byte[])wyfgf2.rcncd;
		flag3 = contentType.MediaType == "message/rfc822" && 0 == 0 && qljus(array, 0, array.Length);
		goto IL_05d7;
		IL_03b1:
		if (flag && 0 == 0 && (!string.IsNullOrEmpty(p0.pufqr) || 1 == 0) && (p0.ehdmx() == null || 1 == 0) && (p0.xroex() == null || 1 == 0))
		{
			bool whhll = p0.xdokw.whhll;
			bool flag4 = nykiq.bxjim(whhll);
			if ((whhll ? true : false) || flag4)
			{
				byte[] bytes = EncodingTools.GetEncoding("eightbit").GetBytes(p0.pufqr);
				if (flag4 && 0 == 0)
				{
					Attachment attachment = wvfmo(yapte, new MemoryStream(bytes, writable: false), new ContentType("application/rtf"), AttachmentBase.boiem(Subject), p0.suuuu.unzoh<string>(MsgPropertyTag.BodyContentId), p0.suuuu.unzoh<string>(MsgPropertyTag.BodyContentLocation));
					attachment.jyvko.Headers.Add("x-rebex-rtf-body", "1");
				}
				else
				{
					oodla(iccuz, p0, bytes);
				}
			}
			else
			{
				tyvvt(iccuz, p0, p0.pufqr, "text/rtf", encoding);
			}
		}
		int num5 = 0;
		num2 = 0;
		if (num2 != 0)
		{
			goto IL_04fb;
		}
		goto IL_0adc;
		IL_0ad0:
		num5++;
		goto IL_0ad6;
		IL_0f20:
		if (Headers["Keywords"] == null || 1 == 0)
		{
			string[] array4 = p0.suuuu.hlyhv<string[]>("Keywords");
			if (array4 != null && 0 == 0)
			{
				stringBuilder = new StringBuilder();
				array3 = array4;
				num4 = 0;
				if (num4 != 0)
				{
					goto IL_0f70;
				}
				goto IL_0f9e;
			}
		}
		goto IL_0fd5;
		IL_0708:
		if (num3 < array.Length)
		{
			goto IL_06f3;
		}
		goto IL_070f;
		IL_0799:
		string text5 = zloqv(wyfgf2.vbjpq, p1: true);
		if (string.IsNullOrEmpty(text5) && 0 == 0)
		{
			text5 = "Untitled";
		}
		else
		{
			text5 = AttachmentBase.gtkha(text5);
			contentType.Parameters["name"] = text5;
		}
		string text6;
		if (dictionary != null && 0 == 0)
		{
			text6 = brgjd.edcru("embedded-rtf-attachment-id{0}@rebex.net", dictionary[num2]);
			if (!wyfgf2.vgcwk || 1 == 0)
			{
				text3 = text3.Replace(brgjd.edcru("<img border=0 src=\"cid:{0}\">", text6), brgjd.edcru(" &lt;&lt;{0}&gt;&gt; ", text5));
			}
		}
		else if (string.IsNullOrEmpty(text6) && 0 == 0)
		{
			string text7 = brgjd.edcru("ATT-{0}-{1}", num5, text5);
			int num6 = text3.IndexOf(text7);
			if (num6 > 0)
			{
				text6 = text7;
				if (text3[num6 - 1] != ':')
				{
					text3 = text3.Replace(text6, brgjd.edcru("cid:{0}", text6));
				}
			}
		}
		bool flag5;
		string text8;
		if ((flag5 ? true : false) || ((!string.IsNullOrEmpty(text6) || 1 == 0) && text3.IndexOf("cid:" + text6) >= 0))
		{
			LinkedResource linkedResource = new LinkedResource();
			linkedResource.lraqa(new MemoryStream(array, writable: false), text5, "application/octet-stream");
			linkedResource.jyvko.ContentType = contentType;
			if (!string.IsNullOrEmpty(text6) || 1 == 0)
			{
				linkedResource.ContentId = MessageId.nkxgc(text6);
			}
			if (!string.IsNullOrEmpty(text8) || 1 == 0)
			{
				linkedResource.ContentLocation = pgndz(text8, p1: true, p2: true);
			}
			if (linkedResource.ContentDisposition == null || 1 == 0)
			{
				linkedResource.jyvko.ContentDisposition = new ContentDisposition("inline");
			}
			else
			{
				linkedResource.ContentDisposition.Inline = true;
			}
			gxxjr.Add(linkedResource);
		}
		else
		{
			wvfmo(yapte, new MemoryStream(array, writable: false), contentType, text5, text6, text8);
		}
		goto IL_0ad0;
		IL_0dc2:
		oabdp oabdp = p0.rsaru[num];
		string text9 = brgjd.edcru("X-Outlook-Recipient{0}-Type", num);
		Headers[text9] = new MimeHeader(text9, xrpej(aymej(oabdp.sizjo)));
		noged<string>(oabdp.plpif, MsgPropertyTag.DisplayName, brgjd.edcru("X-Outlook-Recipient{0}-Name", num));
		noged<string>(oabdp.plpif, MsgPropertyTag.AddressType, brgjd.edcru("X-Outlook-Recipient{0}-AddressType", num));
		noged<string>(oabdp.plpif, MsgPropertyTag.EmailAddress, brgjd.edcru("X-Outlook-Recipient{0}-EmailAddress", num));
		noged<string>(oabdp.plpif, MsgPropertyTag.SmtpAddress, brgjd.edcru("X-Outlook-Recipient{0}-SmtpAddress", num));
		num++;
		goto IL_0ee2;
		IL_0adc:
		if (num2 < p0.clqou.fnqqt)
		{
			goto IL_04fb;
		}
		if (!string.IsNullOrEmpty(text3) || 1 == 0)
		{
			tyvvt(iccuz, p0, text3, "text/html", encoding);
		}
		goto IL_0c65;
		IL_05d7:
		if (!flag3 || 1 == 0)
		{
			if (wyfgf2.lcpzw != xcrar.yesjh)
			{
				if (wyfgf2.lcpzw == xcrar.crqsh || (wyfgf2.lcpzw == xcrar.xkvvt && wyfgf2.rcncd == null))
				{
					goto IL_0ad6;
				}
				if (wyfgf2.lcpzw != xcrar.xkvvt || !(wyfgf2.rcncd is byte[]))
				{
					throw new MailException("Attachment content is not binary.");
				}
			}
			flag5 = wyfgf2.vbjpq.unzoh<bool>(MsgPropertyTag.AttachmentHidden);
			text6 = wyfgf2.vbjpq.unzoh<string>(MsgPropertyTag.AttachContentId);
			text8 = wyfgf2.vbjpq.unzoh<string>(MsgPropertyTag.AttachContentLocation);
			if (text8 != null && 0 == 0)
			{
				text8 = kgbvh.ttsbq(text8);
			}
			string mediaType = contentType.MediaType;
			string text10 = wyfgf2.vbjpq.unzoh<string>(MsgPropertyTag.TextAttachmentCharset);
			if (text10 != null && 0 == 0)
			{
				contentType.CharSet = text10;
			}
			else if (mediaType.StartsWith("text/") && 0 == 0)
			{
				flag2 = true;
				num3 = 0;
				if (num3 != 0)
				{
					goto IL_06f3;
				}
				goto IL_0708;
			}
			goto IL_0799;
		}
		MailMessage mailMessage = new MailMessage();
		if (wyfgf2.jgqfq && 0 == 0)
		{
			mailMessage.wpfeu(wyfgf2.atbph());
		}
		else
		{
			mailMessage.Load(array);
		}
		string text11 = myduv(wyfgf2.vbjpq);
		if (text11 != null && 0 == 0)
		{
			string text12 = text11;
			if (text12.EndsWith(".eml", StringComparison.OrdinalIgnoreCase) && 0 == 0)
			{
				text12 = text12.Substring(0, text12.Length - 4);
			}
			if (text12.Equals(mailMessage.Subject, StringComparison.OrdinalIgnoreCase) && 0 == 0)
			{
				text11 = null;
			}
		}
		Attachment attachment2 = new Attachment(mailMessage, text11, yapte);
		if (!attachment2.fkzie || 1 == 0)
		{
			attachment2.jyvko.Name = attachment2.FileName;
		}
		yapte.Add(attachment2);
		goto IL_0ad0;
		IL_01d4:
		wyfgf wyfgf3 = pokwk(p0);
		MemoryStream p5 = new MemoryStream((byte[])wyfgf3.rcncd, writable: false);
		MimeEntity p6 = wmenc(p5);
		jhayu(p6);
		lmdma(p0);
		goto IL_0c65;
		IL_0ee2:
		if (num < p0.rsaru.fnqqt)
		{
			goto IL_0dc2;
		}
		vnmrj<string>(p0.suuuu, MsgPropertyId.InternetAccountName, "X-Outlook-InternetAccountName");
		vnmrj<string>(p0.suuuu, MsgPropertyId.InternetAccountStamp, "X-Outlook-InternetAccountStamp");
		goto IL_0f20;
		IL_0fd5:
		IEnumerator<qacae> enumerator = p0.suuuu.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				qacae current = enumerator.Current;
				if (current.hmrmu != mshrw.tkmbe || current.ryfdm.ognto != MsgPropertySet.InternetHeaders)
				{
					continue;
				}
				string text13 = current.abdox.ToLower(CultureInfo.InvariantCulture);
				if ((Headers[text13] == null || 1 == 0) && text13.IndexOf(':') < 0 && (!text13.StartsWith("content-") || 1 == 0))
				{
					switch (current.pzpvc)
					{
					case xcrar.bkapb:
					case xcrar.xmyux:
						Headers[text13] = new MimeHeader(text13, xrpej(current.tgbhs.ToString()));
						break;
					}
				}
			}
			return;
		}
		finally
		{
			if (enumerator != null && 0 == 0)
			{
				enumerator.Dispose();
			}
		}
		IL_0f9e:
		if (num4 < array3.Length)
		{
			goto IL_0f70;
		}
		Headers["Keywords"] = new MimeHeader("Keywords", stringBuilder.ToString());
		goto IL_0fd5;
		IL_0c59:
		text = p0.icirf;
		goto IL_020f;
	}

	private static Dictionary<int, int> tmoco(jfxnb p0)
	{
		SortedList<int, List<int>> sortedList = new SortedList<int, List<int>>();
		int num = 0;
		if (num != 0)
		{
			goto IL_000f;
		}
		goto IL_006b;
		IL_000f:
		wyfgf wyfgf = p0.clqou[num];
		int num2 = wyfgf.vbjpq.vvzzv(MsgPropertyTag.RenderingPosition, int.MaxValue);
		if (num2 < 0)
		{
			num2 = int.MaxValue;
		}
		if (!sortedList.TryGetValue(num2, out var value) || 1 == 0)
		{
			value = (sortedList[num2] = new List<int>());
		}
		value.Add(num);
		num++;
		goto IL_006b;
		IL_006b:
		if (num < p0.clqou.fnqqt)
		{
			goto IL_000f;
		}
		int num3 = 0;
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		IEnumerator<List<int>> enumerator = sortedList.Values.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				List<int> current = enumerator.Current;
				using List<int>.Enumerator enumerator2 = current.GetEnumerator();
				while (enumerator2.MoveNext() ? true : false)
				{
					int current2 = enumerator2.Current;
					dictionary[current2] = num3++;
				}
			}
			return dictionary;
		}
		finally
		{
			if (enumerator != null && 0 == 0)
			{
				enumerator.Dispose();
			}
		}
	}

	private void lmdma(jfxnb p0)
	{
		if (p0.kmqzx != null && 0 == 0 && (!Settings.IgnoreMsgTransportHeaders || 1 == 0))
		{
			byte[] bytes = EncodingTools.GetEncoding("utf-8").GetBytes(p0.kmqzx);
			MimeMessage mimeMessage = new MimeMessage();
			mimeMessage.Silent = tnlnk;
			mimeMessage.CertificateFinder = hpvcu;
			mimeMessage.Options = nykiq.hsrhh() | MimeOptions.OnlyParseHeaders | MimeOptions.IgnoreUnparsableHeaders | (MimeOptions)4194304L;
			mimeMessage.DefaultCharset = EncodingTools.GetEncoding("utf-8");
			mimeMessage.UnparsableHeader += dauyl;
			mimeMessage.ParsingHeader += wrzkm;
			mimeMessage.Load(new MemoryStream(bytes, writable: false));
			pfaci(mimeMessage, zidhi, p2: true);
		}
		if (p0.mzhkh == null || false || p0.mzhkh.Equals(p0.untou, StringComparison.OrdinalIgnoreCase))
		{
			if (Headers["from"] == null || 1 == 0)
			{
				if (p0.untou != null && 0 == 0)
				{
					From = hrcku(p0.untou, p0.kazjp);
				}
				else
				{
					string p1 = p0.suuuu.unzoh<string>(MsgPropertyTag.LastModifierName);
					From = hrcku(p1, p0.ofwbk);
					if (Headers["from"] == null || 1 == 0)
					{
						p1 = p0.suuuu.jljfr<string>(MsgPropertyId.InternetAccountName);
						From = hrcku(p1, p0.ofwbk);
					}
				}
			}
		}
		else
		{
			if (Headers["from"] == null || 1 == 0)
			{
				From = hrcku(p0.mzhkh, p0.ofwbk);
			}
			if (Headers["sender"] == null || 1 == 0)
			{
				Sender = xbhsx(p0.untou, p0.kazjp);
			}
		}
		bool flag = Headers["to"] == null;
		bool flag2 = Headers["cc"] == null;
		bool flag3 = Headers["bcc"] == null;
		IEnumerator<oabdp> enumerator = p0.rsaru.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				oabdp current = enumerator.Current;
				MailAddress mailAddress = xbhsx(current.mrwcz, current.ubimv);
				if (mailAddress == null)
				{
					continue;
				}
				switch (current.sizjo)
				{
				case zccyb.pqplk:
					if (flag && 0 == 0)
					{
						To.Add(mailAddress);
					}
					break;
				case zccyb.hfokr:
					if (flag2 && 0 == 0)
					{
						CC.Add(mailAddress);
					}
					break;
				case zccyb.zgczd:
					if (flag3 && 0 == 0)
					{
						Bcc.Add(mailAddress);
					}
					break;
				}
			}
		}
		finally
		{
			if (enumerator != null && 0 == 0)
			{
				enumerator.Dispose();
			}
		}
		if ((Headers["subject"] == null || 1 == 0) && p0.soumj != null && 0 == 0)
		{
			Subject = pgndz(p0.soumj, p1: false, p2: false);
		}
		if ((Headers["date"] == null || 1 == 0) && p0.ixaxw.HasValue && 0 == 0)
		{
			Date = new MailDateTime(p0.ixaxw.Value.ToLocalTime());
		}
		if ((Headers["received"] == null || 1 == 0) && p0.bkaju.HasValue && 0 == 0)
		{
			idovr<DateTime?>(p0.suuuu, MsgPropertyTag.MessageDeliveryTime, "X-Outlook-MessageDeliveryDate", "{0:r}");
		}
		if ((Headers["message-id"] == null || 1 == 0) && p0.takfk != null && 0 == 0)
		{
			MessageId = MessageId.nkxgc(p0.takfk);
		}
		Priority = (MailPriority)p0.irsrm;
		wiyqw = (ihasp)p0.bzlqs;
	}

	private MailAddressCollection hrcku(string p0, string p1)
	{
		MailAddress mailAddress = xbhsx(p0, p1);
		if (mailAddress == null || 1 == 0)
		{
			return new MailAddressCollection();
		}
		return mailAddress;
	}

	private MailAddress xbhsx(string p0, string p1)
	{
		if (string.IsNullOrEmpty(p0) && 0 == 0)
		{
			return null;
		}
		p0 = p0.Trim();
		if (p0.Length == 0 || false || p0.IndexOf('@') < 0)
		{
			return null;
		}
		MailAddress mailAddress = new MailAddress(p0, p1);
		if (string.IsNullOrEmpty(mailAddress.Address) && 0 == 0 && string.IsNullOrEmpty(p1) && 0 == 0)
		{
			return null;
		}
		return mailAddress;
	}

	private string pgndz(string p0, bool p1, bool p2)
	{
		if (p0 == null || 1 == 0)
		{
			return null;
		}
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		if (num != 0)
		{
			goto IL_001e;
		}
		goto IL_007b;
		IL_001e:
		char c = p0[num];
		if (c == '"')
		{
			if (!p2 || 1 == 0)
			{
				stringBuilder.Append(c);
			}
		}
		else if (c >= ' ')
		{
			stringBuilder.Append(c);
		}
		else
		{
			switch (c)
			{
			case '\n':
				stringBuilder.Append(' ');
				break;
			case '\t':
				if (!p1 || 1 == 0)
				{
					stringBuilder.Append(c);
				}
				break;
			}
		}
		num++;
		goto IL_007b;
		IL_007b:
		if (num < p0.Length)
		{
			goto IL_001e;
		}
		return stringBuilder.ToString().Trim();
	}

	private string aymej(zccyb p0)
	{
		switch (p0)
		{
		case zccyb.pqplk:
			return "To";
		case zccyb.hfokr:
			return "Cc";
		case zccyb.zgczd:
			return "Bcc";
		default:
		{
			int num = (int)p0;
			return num.ToString();
		}
		}
	}

	private void vnmrj<T>(howhn p0, MsgPropertyId p1, string p2)
	{
		T val = p0.ocszm<T>(p1);
		if (val != null && 0 == 0)
		{
			Headers[p2] = new MimeHeader(p2, xrpej(val.ToString()));
		}
	}

	private void noged<T>(howhn p0, MsgPropertyTag p1, string p2)
	{
		roucj<T>(p0, p1, p2, "{0}", null);
	}

	private void oighe<T>(howhn p0, MsgPropertyTag p1, string p2, object p3)
	{
		roucj<T>(p0, p1, p2, "{0}", p3);
	}

	private void idovr<T>(howhn p0, MsgPropertyTag p1, string p2, string p3)
	{
		roucj<T>(p0, p1, p2, p3, null);
	}

	private void roucj<T>(howhn p0, MsgPropertyTag p1, string p2, string p3, object p4)
	{
		T val = p0.unzoh<T>(p1);
		if (val != null && 0 == 0 && (!val.Equals(p4) || 1 == 0))
		{
			Headers[p2] = new MimeHeader(p2, xrpej(brgjd.edcru(p3, val)));
		}
	}

	private AlternateView oodla(AlternateViewCollection p0, jfxnb p1, byte[] p2)
	{
		AlternateView alternateView = new AlternateView(new MemoryStream(p2, writable: false), "application/rtf");
		if (!string.IsNullOrEmpty(p1.suuuu.unzoh<string>(MsgPropertyTag.BodyContentId)) || 1 == 0)
		{
			alternateView.ContentId = MessageId.nkxgc(p1.suuuu.unzoh<string>(MsgPropertyTag.BodyContentId));
		}
		if (!string.IsNullOrEmpty(p1.suuuu.unzoh<string>(MsgPropertyTag.BodyContentLocation)) || 1 == 0)
		{
			alternateView.ContentLocation = kgbvh.ttsbq(p1.suuuu.unzoh<string>(MsgPropertyTag.BodyContentLocation));
		}
		p0.Add(alternateView);
		return alternateView;
	}

	private AlternateView tyvvt(AlternateViewCollection p0, jfxnb p1, string p2, string p3, Encoding p4)
	{
		AlternateView alternateView = new AlternateView();
		alternateView.Options |= MimeOptions.AllowAnyTextCharacters;
		char[] trimChars = new char[1];
		alternateView.SetContent(p2.TrimEnd(trimChars), p3, p4);
		if (((p3 != "text/plain") ? true : false) || p1.ljlfy == null)
		{
			if (!string.IsNullOrEmpty(p1.suuuu.unzoh<string>(MsgPropertyTag.BodyContentId)) || 1 == 0)
			{
				alternateView.ContentId = MessageId.nkxgc(p1.suuuu.unzoh<string>(MsgPropertyTag.BodyContentId));
			}
			if (!string.IsNullOrEmpty(p1.suuuu.unzoh<string>(MsgPropertyTag.BodyContentLocation)) || 1 == 0)
			{
				alternateView.ContentLocation = kgbvh.ttsbq(p1.suuuu.unzoh<string>(MsgPropertyTag.BodyContentLocation));
			}
		}
		p0.Add(alternateView);
		return alternateView;
	}

	private Attachment wvfmo(AttachmentCollection p0, Stream p1, ContentType p2, string p3, string p4, string p5)
	{
		Attachment attachment = new Attachment();
		attachment.jyvko.Options |= MimeOptions.AllowAnyTextCharacters;
		attachment.SetContent(p1, p3, p2.MediaType);
		attachment.jyvko.ContentType = p2;
		if (!string.IsNullOrEmpty(p4) || 1 == 0)
		{
			attachment.ContentId = MessageId.nkxgc(p4);
		}
		if (!string.IsNullOrEmpty(p5) || 1 == 0)
		{
			attachment.ContentLocation = pgndz(p5, p1: true, p2: true);
		}
		p0.Add(attachment);
		return attachment;
	}

	private wyfgf pokwk(jfxnb p0)
	{
		if (p0.clqou.fnqqt == 0 || 1 == 0)
		{
			throw new MailException("Secured content not found.");
		}
		wyfgf wyfgf = null;
		if (p0.clqou.fnqqt == 1)
		{
			wyfgf = p0.clqou[0];
		}
		else
		{
			IEnumerator<wyfgf> enumerator = p0.clqou.GetEnumerator();
			try
			{
				while (enumerator.MoveNext() ? true : false)
				{
					wyfgf current = enumerator.Current;
					if (current.lcpzw == xcrar.yesjh)
					{
						string text = current.vbjpq.unzoh<string>(MsgPropertyTag.AttachMimeTag);
						if (text == null || 1 == 0)
						{
							wyfgf = current;
						}
						else if ((text.Equals("application/pkcs7-mime", StringComparison.OrdinalIgnoreCase) ? true : false) || text.Equals("multipart/signed", StringComparison.OrdinalIgnoreCase))
						{
							wyfgf = current;
							break;
						}
					}
				}
			}
			finally
			{
				if (enumerator != null && 0 == 0)
				{
					enumerator.Dispose();
				}
			}
			if (wyfgf == null || 1 == 0)
			{
				throw new MailException("Secured content not found.");
			}
		}
		if (wyfgf.lcpzw != xcrar.yesjh)
		{
			throw new MailException("Secured content is not binary.");
		}
		return wyfgf;
	}

	private static string zloqv(howhn p0, bool p1)
	{
		string text = rzqqg(p0, p1);
		if (text == null || 1 == 0)
		{
			return null;
		}
		return text.Trim();
	}

	private static string rzqqg(howhn p0, bool p1)
	{
		qacae qacae = p0[MsgPropertyTag.AttachLongFilename];
		if (qacae != null && 0 == 0)
		{
			return (string)qacae.tgbhs;
		}
		qacae = p0[MsgPropertyTag.AttachLongPathname];
		if (qacae != null && 0 == 0)
		{
			return Path.GetFileName((string)qacae.tgbhs);
		}
		qacae = p0[MsgPropertyTag.AttachFilename];
		if (qacae != null && 0 == 0)
		{
			return (string)qacae.tgbhs;
		}
		qacae = p0[MsgPropertyTag.AttachPathname];
		if (qacae != null && 0 == 0)
		{
			return Path.GetFileName((string)qacae.tgbhs);
		}
		qacae = p0[MsgPropertyTag.AttachTransportName];
		if (qacae != null && 0 == 0)
		{
			return Path.GetFileName((string)qacae.tgbhs);
		}
		if (p1 && 0 == 0)
		{
			return myduv(p0);
		}
		return null;
	}

	private static string myduv(howhn p0)
	{
		string text = paphv(p0);
		if (text == null || 1 == 0)
		{
			return null;
		}
		return text.Trim();
	}

	private static string paphv(howhn p0)
	{
		qacae qacae = p0[MsgPropertyTag.DisplayName];
		if (qacae != null && 0 == 0)
		{
			string text = ((string)qacae.tgbhs).Trim();
			if ((text.EndsWith(" B)") ? true : false) || (text.EndsWith(" kB)") ? true : false) || (text.EndsWith(" KB)") ? true : false) || text.EndsWith(" MB)"))
			{
				int num = text.LastIndexOf(" (");
				if (num < 0)
				{
					return text;
				}
				text = text.Substring(0, num);
			}
			if (text.Length == 0 || 1 == 0)
			{
				return null;
			}
			return text;
		}
		return null;
	}

	public void Decrypt()
	{
		if (xwnqq == null || 1 == 0)
		{
			throw new MailException("The message is not encrypted.");
		}
		do
		{
			fxioh(p0: false);
			xwnqq.Options = qxurj();
			xwnqq.CertificateFinder = hpvcu;
			xwnqq.Silent = tnlnk;
			xwnqq.Decrypt();
			MimeEntity contentMessage = xwnqq.ContentMessage;
			uwvee = null;
			xwnqq = null;
			jhayu(contentMessage);
		}
		while (xwnqq != null);
		switch (stzmz)
		{
		case mmucn.jmona:
			meosg = stzmz;
			stzmz = mmucn.hzodg;
			uwvee = null;
			fxioh(p0: true);
			break;
		case mmucn.vtxou:
		case mmucn.hhpti:
			if (maxwa == null || false || meosg == mmucn.jmona)
			{
				meosg = stzmz;
				stzmz = mmucn.vjkno;
				uwvee = null;
				fxioh(p0: false);
			}
			else
			{
				meosg = stzmz;
				stzmz = mmucn.hzodg;
				uwvee = maxwa;
				fxioh(p0: true);
			}
			break;
		case mmucn.hzodg:
			break;
		}
	}

	public void RemoveSignature()
	{
		if (maxwa == null || 1 == 0)
		{
			throw new MailException("The message is not signed.");
		}
		meosg = stzmz;
		if (xwnqq != null && 0 == 0)
		{
			uwvee = xwnqq;
			stzmz = mmucn.vtxou;
		}
		else
		{
			uwvee = null;
			stzmz = mmucn.vjkno;
			fxioh(p0: false);
		}
		maxwa = null;
		fjxio = new SubjectInfoCollection();
	}

	public void Encrypt(IEnumerable<Certificate> recipients)
	{
		Encrypt(null, recipients);
	}

	public void Encrypt(params Certificate[] recipients)
	{
		if (recipients == null || 1 == 0)
		{
			throw new ArgumentNullException("recipients");
		}
		Encrypt(null, recipients);
	}

	public void Encrypt(MailEncryptionAlgorithm encryptionAlgorithm, IEnumerable<Certificate> recipients)
	{
		Encrypt(new MailEncryptionParameters(encryptionAlgorithm), recipients);
	}

	public void Encrypt(MailEncryptionAlgorithm encryptionAlgorithm, params Certificate[] recipients)
	{
		Encrypt(new MailEncryptionParameters(encryptionAlgorithm), recipients);
	}

	public void Encrypt(MailEncryptionParameters encryptionParameters, IEnumerable<Certificate> recipients)
	{
		if (recipients == null || 1 == 0)
		{
			throw new ArgumentNullException("recipients");
		}
		List<Certificate> list = new List<Certificate>(recipients);
		Encrypt(encryptionParameters, list.ToArray());
	}

	public void Encrypt(MailEncryptionParameters encryptionParameters, params Certificate[] recipients)
	{
		if (recipients == null || 1 == 0)
		{
			throw new ArgumentNullException("recipients");
		}
		if (recipients.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("One or more recipients have to be specified to be able to encrypt the message.", "recipients");
		}
		if (Array.IndexOf(recipients, null, 0, recipients.Length) >= 0)
		{
			throw new ArgumentException("Null item is present in the collection.", "recipients");
		}
		if (xwnqq != null || IsEncrypted)
		{
			throw new MailException("The message has already been encrypted.");
		}
		if (maxwa != null && 0 == 0 && (uwvee == null || 1 == 0))
		{
			throw new MailException("Cannot save or encrypt the message because the signed content has been modified. Remove the signature first.");
		}
		MailEncryptionParameters mailEncryptionParameters = encryptionParameters;
		if (mailEncryptionParameters == null || 1 == 0)
		{
			mailEncryptionParameters = new MailEncryptionParameters();
		}
		encryptionParameters = mailEncryptionParameters;
		string encryptionAlgorithm = MailEncryptionParameters.petat(encryptionParameters.EncryptionAlgorithm, "encryptionParameters");
		xwnqq = new MimeEntity();
		xwnqq.Options = qxurj();
		xwnqq.Silent = tnlnk;
		xwnqq.CertificateFinder = hpvcu;
		if (maxwa == null || 1 == 0)
		{
			MimeEntity entity = qxnkb(typeof(MimeEntity), p1: true);
			xwnqq.SetEnvelopedContent(entity, encryptionAlgorithm, encryptionParameters.akbbi, recipients);
			xwnqq.Encrypt();
			fxioh(p0: true);
			meosg = stzmz;
			stzmz = mmucn.vtxou;
		}
		else
		{
			xwnqq.SetEnvelopedContent(maxwa, encryptionAlgorithm, encryptionParameters.akbbi, recipients);
			xwnqq.Encrypt();
			meosg = stzmz;
			stzmz = mmucn.hhpti;
		}
		maxwa = null;
		uwvee = xwnqq;
		hoayu = new SubjectInfoCollection(xwnqq.EnvelopedContentInfo);
		fjxio = new SubjectInfoCollection();
		yapte.cmbhn.Clear();
		iccuz.wzxkg.Clear();
		gxxjr.pusbf.Clear();
	}

	private MimeOptions qxurj()
	{
		return nykiq.hsrhh() & MimeOptions.SkipCertificateUsageCheck;
	}

	public void Sign(IEnumerable<Certificate> signers)
	{
		Sign(null, signers);
	}

	public void Sign(params Certificate[] signers)
	{
		Sign(null, signers);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[wptwl(false)]
	public void Sign(SignatureHashAlgorithm algorithm, IEnumerable<Certificate> signers)
	{
		Sign(new MailSignatureParameters(algorithm), signers);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[wptwl(false)]
	public void Sign(SignatureHashAlgorithm algorithm, params Certificate[] signers)
	{
		Sign(new MailSignatureParameters(algorithm), signers);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[wptwl(false)]
	public void Sign(SignatureHashAlgorithm algorithm, MailSignatureStyle style, IEnumerable<Certificate> signers)
	{
		Sign(new MailSignatureParameters(algorithm, style), signers);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[wptwl(false)]
	public void Sign(SignatureHashAlgorithm algorithm, MailSignatureStyle style, params Certificate[] signers)
	{
		Sign(new MailSignatureParameters(algorithm, style), signers);
	}

	public void Sign(MailSignatureParameters signatureParameters, IEnumerable<Certificate> signers)
	{
		if (signers == null || 1 == 0)
		{
			throw new ArgumentNullException("signers");
		}
		List<Certificate> list = new List<Certificate>(signers);
		Sign(signatureParameters, list.ToArray());
	}

	public void Sign(MailSignatureParameters signatureParameters, params Certificate[] signers)
	{
		if (signers == null || 1 == 0)
		{
			throw new ArgumentNullException("signers");
		}
		if (signers.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("One or more signers have to be specified to be able to sign the message.", "signers");
		}
		if (Array.IndexOf(signers, null, 0, signers.Length) >= 0)
		{
			throw new ArgumentException("Null item is present in the collection.", "signers");
		}
		if (maxwa != null || (IsSigned ? true : false) || stzmz == mmucn.hhpti)
		{
			throw new MailException("The message has already been signed.");
		}
		MailSignatureParameters mailSignatureParameters = signatureParameters;
		if (mailSignatureParameters == null || 1 == 0)
		{
			mailSignatureParameters = new MailSignatureParameters();
		}
		signatureParameters = mailSignatureParameters;
		MailSignatureParameters.qfrth(signatureParameters.Style, "signatureParameters");
		MimeEntity mimeEntity = new MimeEntity();
		mimeEntity.Options = itznc();
		mimeEntity.Silent = tnlnk;
		mimeEntity.CertificateFinder = hpvcu;
		if (xwnqq == null || 1 == 0)
		{
			MimeEntity entity = qxnkb(typeof(MimeEntity), p1: true);
			mimeEntity.SetSignedContent(entity, (MimeSignatureStyle)signatureParameters.Style, signatureParameters.ftkjs, signers);
			mimeEntity.Sign();
			fxioh(p0: true);
			meosg = stzmz;
			stzmz = mmucn.hzodg;
		}
		else
		{
			mimeEntity.SetSignedContent(xwnqq, MimeSignatureStyle.Enveloped, signatureParameters.ftkjs, signers);
			mimeEntity.Sign();
			meosg = stzmz;
			stzmz = mmucn.jmona;
		}
		maxwa = (uwvee = mimeEntity);
		fjxio = new SubjectInfoCollection(mimeEntity.SignedContentInfo);
	}

	private MimeOptions itznc()
	{
		return nykiq.hsrhh() & (MimeOptions.DisableEncryptionKeyPreference | MimeOptions.DisableSMimeCapabilitiesAttribute | MimeOptions.SkipCertificateUsageCheck);
	}

	public MailSignatureValidity ValidateSignature()
	{
		return ValidateSignature((MailSignatureValidationOptions)0, ValidationOptions.None, CertificateChainEngine.Auto);
	}

	public MailSignatureValidity ValidateSignature(MailSignatureValidationOptions signatureValidationOptions)
	{
		return ValidateSignature(signatureValidationOptions, ValidationOptions.None, CertificateChainEngine.Auto);
	}

	public MailSignatureValidity ValidateSignature(bool verifySignatureOnly, ValidationOptions options)
	{
		return ValidateSignature((verifySignatureOnly ? true : false) ? MailSignatureValidationOptions.SkipCertificateCheck : ((MailSignatureValidationOptions)0), options, CertificateChainEngine.Auto);
	}

	public MailSignatureValidity ValidateSignature(bool verifySignatureOnly, ValidationOptions options, CertificateChainEngine certificateEngine)
	{
		return ValidateSignature((verifySignatureOnly ? true : false) ? MailSignatureValidationOptions.SkipCertificateCheck : ((MailSignatureValidationOptions)0), options, certificateEngine);
	}

	public MailSignatureValidity ValidateSignature(MailSignatureValidationOptions signatureValidationOptions, ValidationOptions options, CertificateChainEngine certificateEngine)
	{
		if (maxwa == null || 1 == 0)
		{
			throw new MailException("Cannot validate a message that is not signed.");
		}
		bool verifySignatureOnly = (signatureValidationOptions & MailSignatureValidationOptions.SkipCertificateCheck) != 0;
		maxwa.Options = itznc();
		maxwa.CertificateFinder = hpvcu;
		SignatureValidationResult result = maxwa.ValidateSignature(verifySignatureOnly, options, certificateEngine);
		MailSignatureStatus mailSignatureStatus = (MailSignatureStatus)0;
		ArrayList arrayList = new ArrayList();
		MailAddress[] array;
		int num;
		if (!nykiq.SkipSenderCheck || false || (signatureValidationOptions & MailSignatureValidationOptions.SkipHeaderCheck) == 0 || 1 == 0)
		{
			array = new MailAddress[zidhi.From.Count + ((zidhi.Sender != null) ? 1 : 0)];
			if (zidhi.Sender != null && 0 == 0)
			{
				array[0] = zidhi.Sender;
				zidhi.From.CopyTo(array, 1);
			}
			else
			{
				zidhi.From.CopyTo(array, 0);
			}
			if (array.Length == 0 || 1 == 0)
			{
				mailSignatureStatus |= MailSignatureStatus.MissingSender;
			}
			num = 0;
			if (num != 0)
			{
				goto IL_011f;
			}
			goto IL_020e;
		}
		goto IL_0227;
		IL_020e:
		if (num >= array.Length)
		{
			goto IL_0227;
		}
		goto IL_011f;
		IL_011f:
		MailAddress mailAddress = array[num];
		bool flag = false;
		IEnumerator enumerator = fjxio.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				SubjectInfo subjectInfo = (SubjectInfo)enumerator.Current;
				if (subjectInfo.Certificate == null)
				{
					continue;
				}
				string[] mailAddresses = subjectInfo.Certificate.GetMailAddresses();
				int num2 = 0;
				if (num2 != 0)
				{
					goto IL_016f;
				}
				goto IL_0198;
				IL_0198:
				if (num2 >= mailAddresses.Length)
				{
					continue;
				}
				goto IL_016f;
				IL_016f:
				if (string.Compare(mailAddresses[num2], mailAddress.Address, StringComparison.OrdinalIgnoreCase) == 0 || 1 == 0)
				{
					flag = true;
					if (flag)
					{
						continue;
					}
				}
				num2++;
				goto IL_0198;
			}
		}
		finally
		{
			if (enumerator is IDisposable disposable && 0 == 0)
			{
				disposable.Dispose();
			}
		}
		if (!flag || 1 == 0)
		{
			arrayList.Add(mailAddress.Address);
			if (num == 0 || false || (signatureValidationOptions & MailSignatureValidationOptions.IgnoreMissingNonSenderOriginators) == 0 || 1 == 0)
			{
				mailSignatureStatus |= MailSignatureStatus.SenderSignatureMissing;
			}
		}
		num++;
		goto IL_020e;
		IL_0227:
		return new MailSignatureValidity(result, (string[])arrayList.ToArray(typeof(string)), mailSignatureStatus);
	}
}
