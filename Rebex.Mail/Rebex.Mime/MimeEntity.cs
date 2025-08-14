using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Rebex.Mime.Headers;
using Rebex.Security.Certificates;
using Rebex.Security.Cryptography;
using Rebex.Security.Cryptography.Pkcs;
using onrkn;

namespace Rebex.Mime;

public class MimeEntity
{
	private sealed class cvyri
	{
		public ArrayList cpecj;

		public void dkarc(byte[] p0, int p1)
		{
			cpecj.Add(new nxtme<byte>(p0, 0, p1));
		}
	}

	private static qacxy? lwbyq;

	[NonSerialized]
	private MimeHeaderCollection movah;

	[NonSerialized]
	private MimeEntityCollection zajxz;

	[NonSerialized]
	private vdmfr derxc;

	[NonSerialized]
	private opjbe csrgm;

	[NonSerialized]
	private string wknht;

	[NonSerialized]
	private string wcfgc;

	[NonSerialized]
	private Encoding itjdj;

	[NonSerialized]
	private MimeOptions ejpcp;

	[NonSerialized]
	private EventHandler<MimeUnparsableHeaderEventArgs> tpzep;

	[NonSerialized]
	private EventHandler<MimeParsingHeaderEventArgs> vxcla;

	[NonSerialized]
	private MimeEntity gvccg;

	[NonSerialized]
	private MimeEntity laxrm;

	[NonSerialized]
	private ContentType vgqjo;

	[NonSerialized]
	private ContentTransferEncoding fpqgm;

	[NonSerialized]
	private bool wvvtb;

	[NonSerialized]
	private bool jbrce;

	[NonSerialized]
	private bool fckfg;

	[NonSerialized]
	private Stream gkypg;

	[NonSerialized]
	private bool upzzo;

	[NonSerialized]
	private SignedData mbgbb;

	[NonSerialized]
	private EnvelopedData iuwky;

	[NonSerialized]
	private opjbe qgdgd;

	[NonSerialized]
	private MimeEntity buivp;

	[NonSerialized]
	private ICertificateFinder xtbgl;

	[NonSerialized]
	private bool ounaw;

	private static Func<FormatException, Exception> ceqsz;

	private static Func<string, Exception> foadp;

	private static Func<string, Exception> qujjt;

	public ICertificateFinder CertificateFinder
	{
		get
		{
			return xtbgl;
		}
		set
		{
			if (xtbgl != value)
			{
				xtbgl = value;
				if (mbgbb != null && 0 == 0)
				{
					mbgbb.CertificateFinder = xtbgl;
				}
				if (iuwky != null && 0 == 0)
				{
					iuwky.CertificateFinder = xtbgl;
				}
			}
		}
	}

	public bool Silent
	{
		get
		{
			return ounaw;
		}
		set
		{
			ounaw = value;
		}
	}

	public MimeHeaderCollection Headers => movah;

	public MimeEntityCollection Parts => zajxz;

	public MimeEntity Parent
	{
		get
		{
			return gvccg;
		}
		set
		{
			if (fckfg && 0 == 0)
			{
				throw new MimeException("Cannot change read-only message.", MimeExceptionStatus.OperationError);
			}
			if (value != gvccg)
			{
				if (gvccg != null && 0 == 0)
				{
					gvccg.Parts.Remove(this);
				}
				if (value != null && 0 == 0)
				{
					value.Parts.Add(this);
				}
			}
		}
	}

	public string Preamble
	{
		get
		{
			return wknht;
		}
		set
		{
			if (fckfg && 0 == 0)
			{
				throw new MimeException("Cannot change read-only message.", MimeExceptionStatus.OperationError);
			}
			if (value == null || 1 == 0)
			{
				wknht = null;
				return;
			}
			value = value.Replace("\r", "");
			if (!lhovy(value) || 1 == 0)
			{
				throw new ArgumentException("Not an ASCII string.", "value");
			}
			wknht = value;
		}
	}

	public string Epilogue
	{
		get
		{
			return wcfgc;
		}
		set
		{
			if (fckfg && 0 == 0)
			{
				throw new MimeException("Cannot change read-only message.", MimeExceptionStatus.OperationError);
			}
			if (value == null || 1 == 0)
			{
				wcfgc = null;
				return;
			}
			value = value.Replace("\r", "");
			if (!lhovy(value) || 1 == 0)
			{
				throw new ArgumentException("Not an ASCII string.", "value");
			}
			wcfgc = value;
		}
	}

	public MimeOptions Options
	{
		get
		{
			return ejpcp;
		}
		set
		{
			ejpcp = value;
		}
	}

	public Encoding DefaultCharset
	{
		get
		{
			return itjdj;
		}
		set
		{
			itjdj = value;
		}
	}

	public bool ReadOnly
	{
		get
		{
			return fckfg;
		}
		set
		{
			if (fckfg != value)
			{
				if ((!value || 1 == 0) && upzzo && 0 == 0)
				{
					throw new MimeException("Signed message is always read-only.", MimeExceptionStatus.OperationError);
				}
				fckfg = value;
				zajxz.vfvpt = value;
				movah.kldya = value;
			}
		}
	}

	public MimeEntity ContentMessage => laxrm;

	public string ContentString
	{
		get
		{
			Encoding encoding = vgqjo.Encoding;
			if (encoding == null || 1 == 0)
			{
				if (!vgqjo.MediaType.StartsWith("text/"))
				{
					if (vgqjo.MediaType == "application/rtf" && 0 == 0)
					{
						return "This is a binary RTF body that cannot be represented as string.";
					}
					return null;
				}
				encoding = EncodingTools.Default;
			}
			return dhgcb(encoding, p1: true);
		}
	}

	public Encoding Charset => vgqjo.Encoding;

	public TransferEncoding TransferEncoding
	{
		get
		{
			return fpqgm.Encoding;
		}
		set
		{
			if (fckfg && 0 == 0)
			{
				throw new MimeException("Cannot change read-only message.", MimeExceptionStatus.OperationError);
			}
			ContentTransferEncoding contentTransferEncoding = new ContentTransferEncoding(value);
			ContentTransferEncoding = contentTransferEncoding;
		}
	}

	public SignedData SignedContentInfo => mbgbb;

	public EnvelopedData EnvelopedContentInfo => iuwky;

	public MimeSignatureStyle SignatureStyle
	{
		get
		{
			if (mbgbb == null || 1 == 0)
			{
				return MimeSignatureStyle.None;
			}
			if (mbgbb.Detached && 0 == 0)
			{
				return MimeSignatureStyle.Detached;
			}
			return MimeSignatureStyle.Enveloped;
		}
		set
		{
			if (fckfg && 0 == 0)
			{
				throw new MimeException("Cannot change read-only message.", MimeExceptionStatus.OperationError);
			}
			bool flag;
			switch (value)
			{
			case MimeSignatureStyle.None:
				if (mbgbb == null || 1 == 0)
				{
					return;
				}
				throw new MimeException("Specified signature style is not valid for the this entity.", MimeExceptionStatus.OperationError);
			case MimeSignatureStyle.Detached:
				flag = true;
				if (flag)
				{
					break;
				}
				goto case MimeSignatureStyle.Enveloped;
			case MimeSignatureStyle.Enveloped:
				flag = false;
				if (!flag)
				{
					break;
				}
				goto default;
			default:
				throw new ArgumentException("Specified signature style is unknown.", "value");
			}
			if (mbgbb == null || false || mbgbb.SignerInfos.Count == 0 || 1 == 0)
			{
				throw new MimeException("Specified signature style is not valid for the this entity.", MimeExceptionStatus.OperationError);
			}
			if (mbgbb.Detached != flag)
			{
				mbgbb.Detached = flag;
				hepyx(!flag, mbgbb.SignerInfos[0].ToDigestAlgorithm());
			}
		}
	}

	public MimeEntityKind Kind
	{
		get
		{
			if (mbgbb != null && 0 == 0)
			{
				return MimeEntityKind.Signed;
			}
			if (iuwky != null && 0 == 0)
			{
				return MimeEntityKind.Enveloped;
			}
			if (wvvtb && 0 == 0)
			{
				return MimeEntityKind.Multipart;
			}
			if (laxrm != null && 0 == 0)
			{
				return MimeEntityKind.Message;
			}
			return MimeEntityKind.Body;
		}
	}

	internal bool hcbrv
	{
		get
		{
			if (vgqjo == null || 1 == 0)
			{
				return false;
			}
			if (vgqjo.MediaType != "multipart/signed" && 0 == 0)
			{
				return false;
			}
			string text = vgqjo.Parameters["protocol"];
			if (text == null || 1 == 0)
			{
				return false;
			}
			string text2;
			if ((text2 = text.ToLower(CultureInfo.InvariantCulture)) != null && 0 == 0 && (text2 == "application/pkcs7-signature" || text2 == "application/x-pkcs7-signature"))
			{
				return true;
			}
			return false;
		}
	}

	internal MimeEntity ylgbh => buivp;

	public bool IsMultipart
	{
		get
		{
			if (mbgbb != null || iuwky != null)
			{
				return false;
			}
			return wvvtb;
		}
	}

	internal bool mugxz => jbrce;

	public ContentType ContentType
	{
		get
		{
			return vgqjo;
		}
		set
		{
			if (value == null || 1 == 0)
			{
				value = ((gvccg == null || false || !gvccg.wvvtb || false || !(gvccg.vgqjo.MediaType == "multipart/digest")) ? new ContentType("text/plain") : new ContentType("message/rfc822"));
			}
			tngim(derxc, value, fpqgm, p3: false, null, p5: false);
		}
	}

	public ContentTransferEncoding ContentTransferEncoding
	{
		get
		{
			return fpqgm;
		}
		set
		{
			if (value == null || 1 == 0)
			{
				throw new ArgumentNullException("value");
			}
			if (!value.IsKnown || 1 == 0)
			{
				throw new ArgumentException("Unknown transfer encoding.", "value");
			}
			tngim(derxc, vgqjo, value, p3: false, null, p5: false);
		}
	}

	public ContentDisposition ContentDisposition
	{
		get
		{
			IHeader header = movah.etuur("content-disposition");
			return header as ContentDisposition;
		}
		set
		{
			movah.tyaam("content-disposition", value);
		}
	}

	public MessageId ContentId
	{
		get
		{
			IHeader header = movah.etuur("content-id");
			return header as MessageId;
		}
		set
		{
			movah.tyaam("content-id", value);
		}
	}

	public string ContentDescription
	{
		get
		{
			IHeader header = movah.etuur("content-description");
			if (header == null || 1 == 0)
			{
				return string.Empty;
			}
			return header.ToString();
		}
		set
		{
			movah.tyaam("content-description", new Unstructured(value));
		}
	}

	public ContentLocation ContentLocation
	{
		get
		{
			IHeader header = movah.etuur("content-location");
			return header as ContentLocation;
		}
		set
		{
			movah.tyaam("content-location", value);
		}
	}

	public string Name
	{
		get
		{
			string text = null;
			ContentDisposition contentDisposition = ContentDisposition;
			if (contentDisposition != null && 0 == 0)
			{
				text = contentDisposition.FileName;
			}
			if (text == null || false || text.Length == 0 || 1 == 0)
			{
				text = ContentType.Parameters["name"];
			}
			if (text == null || false || text.Length == 0 || 1 == 0)
			{
				ContentLocation contentLocation = ContentLocation;
				if (contentLocation != null && 0 == 0)
				{
					text = contentLocation.Location;
					int num = brgjd.pkosy(text, '\\', '/');
					if (num >= 0)
					{
						text = text.Substring(num + 1);
					}
				}
			}
			if (text == null || 1 == 0)
			{
				return string.Empty;
			}
			return text;
		}
		set
		{
			kgbvh.wbwli(value, "value");
			if (fckfg && 0 == 0)
			{
				throw new MimeException("Cannot change read-only message.", MimeExceptionStatus.OperationError);
			}
			ContentDisposition p = ContentDisposition;
			iltqs(vgqjo, ref p, value);
			ContentDisposition = p;
		}
	}

	public event EventHandler<MimeUnparsableHeaderEventArgs> UnparsableHeader
	{
		add
		{
			tpzep = (EventHandler<MimeUnparsableHeaderEventArgs>)Delegate.Combine(tpzep, value);
		}
		remove
		{
			tpzep = (EventHandler<MimeUnparsableHeaderEventArgs>)Delegate.Remove(tpzep, value);
		}
	}

	public event EventHandler<MimeParsingHeaderEventArgs> ParsingHeader
	{
		add
		{
			vxcla = (EventHandler<MimeParsingHeaderEventArgs>)Delegate.Combine(vxcla, value);
		}
		remove
		{
			vxcla = (EventHandler<MimeParsingHeaderEventArgs>)Delegate.Remove(vxcla, value);
		}
	}

	private static bool lhovy(string p0)
	{
		int num = 0;
		if (num != 0)
		{
			goto IL_0006;
		}
		goto IL_002d;
		IL_0006:
		if ((p0[num] < ' ' || p0[num] > '~') && p0[num] != '\n')
		{
			return false;
		}
		num++;
		goto IL_002d;
		IL_002d:
		if (num < p0.Length)
		{
			goto IL_0006;
		}
		return true;
	}

	internal void yjlvs(MimeEntity p0)
	{
		ejpcp = p0.ejpcp;
		itjdj = p0.itjdj;
		tpzep = p0.tpzep;
		vxcla = p0.vxcla;
		ounaw = p0.ounaw;
		xtbgl = p0.xtbgl;
	}

	internal EventHandler<MimeUnparsableHeaderEventArgs> ftkcv()
	{
		return tpzep;
	}

	protected virtual void OnBrokenHeader(MimeUnparsableHeaderEventArgs e)
	{
		EventHandler<MimeUnparsableHeaderEventArgs> eventHandler = tpzep;
		if (eventHandler != null && 0 == 0)
		{
			eventHandler(this, e);
		}
	}

	internal EventHandler<MimeParsingHeaderEventArgs> czfxz()
	{
		return vxcla;
	}

	private void hhdfx()
	{
		if (upzzo && 0 == 0)
		{
			return;
		}
		fckfg = true;
		upzzo = true;
		zajxz.vfvpt = true;
		movah.kldya = true;
		if (laxrm != null && 0 == 0)
		{
			laxrm.hhdfx();
		}
		int num = 0;
		if (num != 0)
		{
			goto IL_005a;
		}
		goto IL_0075;
		IL_0075:
		if (num >= zajxz.Count)
		{
			return;
		}
		goto IL_005a;
		IL_005a:
		zajxz[num].hhdfx();
		num++;
		goto IL_0075;
	}

	public MimeEntity()
	{
		dahxy.kdslf();
		if (!lwbyq.HasValue || 1 == 0)
		{
			try
			{
				lwbyq = ongpx.bjvdq(dhuan.wekdp());
			}
			catch (fwwdw fwwdw)
			{
				throw new MimeException(fwwdw.Message);
			}
		}
		movah = new MimeHeaderCollection();
		zajxz = new MimeEntityCollection(this);
		derxc = new vdmfr();
		wcfgc = "";
		vgqjo = new ContentType("text/plain");
		fpqgm = new ContentTransferEncoding(TransferEncoding.SevenBit);
		ounaw = true;
		xtbgl = Rebex.Security.Cryptography.Pkcs.CertificateFinder.Default;
	}

	internal MimeEntity(MimeEntity src)
	{
		wknht = src.wknht;
		wcfgc = src.wcfgc;
		yjlvs(src);
		wvvtb = src.wvvtb;
		jbrce = src.jbrce;
		if (src.laxrm != null && 0 == 0)
		{
			laxrm = src.laxrm.Clone();
		}
		vgqjo = (ContentType)src.vgqjo.Clone();
		fpqgm = (ContentTransferEncoding)src.fpqgm.Clone();
		derxc = src.derxc.reffw();
		csrgm = src.csrgm;
		zajxz = new MimeEntityCollection(this);
		int num = 0;
		if (num != 0)
		{
			goto IL_00b8;
		}
		goto IL_00d9;
		IL_0118:
		int num2;
		if (num2 >= src.movah.Count)
		{
			if (src.iuwky != null && 0 == 0)
			{
				iuwky = src.iuwky.Clone();
			}
			if (src.mbgbb != null && 0 == 0)
			{
				mbgbb = src.mbgbb.Clone();
				laxrm.hhdfx();
			}
			return;
		}
		goto IL_00f7;
		IL_00f7:
		movah.Add(src.movah[num2].Clone());
		num2++;
		goto IL_0118;
		IL_00d9:
		if (num < src.zajxz.Count)
		{
			goto IL_00b8;
		}
		movah = new MimeHeaderCollection();
		num2 = 0;
		if (num2 != 0)
		{
			goto IL_00f7;
		}
		goto IL_0118;
		IL_00b8:
		zajxz.Add(src.zajxz[num].Clone());
		num++;
		goto IL_00d9;
	}

	public virtual MimeEntity Clone()
	{
		return new MimeEntity(this);
	}

	public virtual MimeMessage ToMessage()
	{
		return new MimeMessage(this);
	}

	internal void dtfjp(MimeEntity p0)
	{
		gvccg = p0;
	}

	internal void crgxv()
	{
		movah.Clear();
		zajxz.Clear();
		derxc = new vdmfr();
		csrgm = null;
		laxrm = null;
		wknht = null;
		wcfgc = null;
		vgqjo = new ContentType("text/plain");
		fpqgm = new ContentTransferEncoding(TransferEncoding.SevenBit);
		wvvtb = false;
		jbrce = false;
		fckfg = false;
		upzzo = false;
		qgdgd = null;
		buivp = null;
		mbgbb = null;
		iuwky = null;
	}

	private static string qyjzh(int p0)
	{
		byte[] array = kgbvh.sjmog();
		StringBuilder stringBuilder = new StringBuilder(70);
		stringBuilder.Append("------_=_NextPart_");
		stringBuilder.Append(p0.ToString(CultureInfo.InvariantCulture).PadLeft(3, '0'));
		stringBuilder.Append('_');
		int num = 0;
		if (num != 0)
		{
			goto IL_0047;
		}
		goto IL_005e;
		IL_0047:
		stringBuilder.dlvlk("{0:X2}", array[num]);
		num++;
		goto IL_005e;
		IL_005e:
		if (num < 4)
		{
			goto IL_0047;
		}
		stringBuilder.Append('.');
		int num2 = 4;
		if (num2 == 0)
		{
			goto IL_0070;
		}
		goto IL_0087;
		IL_0087:
		if (num2 < 8)
		{
			goto IL_0070;
		}
		return stringBuilder.ToString();
		IL_0070:
		stringBuilder.dlvlk("{0:X2}", array[num2]);
		num2++;
		goto IL_0087;
	}

	private void ewqgv(string[] p0, int p1)
	{
		int num = 0;
		if (num != 0)
		{
			goto IL_0009;
		}
		goto IL_0022;
		IL_0009:
		zajxz[num].ewqgv(p0, p1 + 1);
		num++;
		goto IL_0022;
		IL_0022:
		if (num >= zajxz.Count)
		{
			if (laxrm != null && 0 == 0)
			{
				laxrm.ewqgv(p0, p1);
			}
			if (!wvvtb || 1 == 0 || fckfg)
			{
				return;
			}
			string text = vgqjo.Boundary;
			if (text != null && 0 == 0 && (text.Length < 2 || text.Length > 70))
			{
				text = null;
			}
			if (text != null)
			{
				return;
			}
			do
			{
				if (text == null || 1 == 0)
				{
					text = qyjzh(p1);
				}
				int num2 = 0;
				if (num2 != 0)
				{
					goto IL_00be;
				}
				goto IL_00d9;
				IL_00be:
				if (p0[0].IndexOf(text) == 0 || 1 == 0)
				{
					text = null;
					continue;
				}
				num2++;
				goto IL_00d9;
				IL_00d9:
				if (num2 < p0.Length)
				{
					goto IL_00be;
				}
			}
			while (text == null);
			vgqjo.Boundary = text;
			return;
		}
		goto IL_0009;
	}

	private void oznrj(Stream p0)
	{
		if (wknht != null && 0 == 0)
		{
			string text = wknht.Replace("\r", "");
			text = text.Replace("\n", "\r\n");
			byte[] bytes = EncodingTools.Default.GetBytes(text);
			p0.WriteByte(13);
			p0.WriteByte(10);
			p0.Write(bytes, 0, bytes.Length);
		}
		if (wcfgc != null && 0 == 0)
		{
			string text2 = wcfgc.Replace("\r", "");
			text2 = text2.Replace("\n", "\r\n");
			byte[] bytes2 = EncodingTools.Default.GetBytes(wcfgc);
			p0.WriteByte(13);
			p0.WriteByte(10);
			p0.Write(bytes2, 0, bytes2.Length);
		}
		if (fpqgm.Encoding != TransferEncoding.QuotedPrintable && 0 == 0 && fpqgm.Encoding != TransferEncoding.Base64)
		{
			p0.WriteByte(13);
			p0.WriteByte(10);
			derxc.qxwca(p0);
		}
		int num = 0;
		if (num != 0)
		{
			goto IL_010a;
		}
		goto IL_0123;
		IL_0123:
		if (num >= zajxz.Count)
		{
			if (laxrm != null && 0 == 0)
			{
				laxrm.oznrj(p0);
			}
			return;
		}
		goto IL_010a;
		IL_010a:
		zajxz[num].oznrj(p0);
		num++;
		goto IL_0123;
	}

	internal void kjyki()
	{
		hpmsr hpmsr = new hpmsr();
		oznrj(hpmsr);
		hpmsr.WriteByte(13);
		hpmsr.WriteByte(10);
		hpmsr.Close();
		string[] p = hpmsr.zzcnh();
		ewqgv(p, 1);
	}

	internal object jfupv(Stream p0)
	{
		ContentType contentType = ContentType;
		if (contentType.msrzy == "multipart" && 0 == 0 && (ejpcp & MimeOptions.DoNotParseMimeTree) == 0)
		{
			return null;
		}
		bool flag = ((contentType.msrzy != "text") ? true : false) || (ejpcp & MimeOptions.AllowAnyTextCharacters) != 0;
		switch (fpqgm.Encoding)
		{
		case TransferEncoding.Base64:
			p0 = new mdefg(p0);
			break;
		case TransferEncoding.QuotedPrintable:
			p0 = new cotho(p0, flag, (ejpcp & MimeOptions.DoNotQuoteProblematicSequences) == 0);
			break;
		}
		if (iuwky != null && 0 == 0)
		{
			iuwky.Save(p0);
			p0.Close();
			return null;
		}
		if (mbgbb != null && 0 == 0)
		{
			mbgbb.Save(p0);
			p0.Close();
			return null;
		}
		if (!flag || 1 == 0)
		{
			Encoding encoding = vgqjo.Encoding;
			if (encoding == Encoding.Unicode)
			{
				p0.WriteByte(byte.MaxValue);
				p0.WriteByte(254);
			}
			else if (encoding == Encoding.BigEndianUnicode)
			{
				p0.WriteByte(254);
				p0.WriteByte(byte.MaxValue);
			}
		}
		if (!derxc.iihdy || 1 == 0)
		{
			derxc.qxwca(p0);
			p0.Close();
			return null;
		}
		Stream stream = derxc.kqazs();
		return new object[3]
		{
			stream,
			p0,
			new byte[4096]
		};
	}

	internal object deomd(object p0)
	{
		object[] array = (object[])p0;
		Stream stream = (Stream)array[0];
		Stream stream2 = (Stream)array[1];
		byte[] array2 = (byte[])array[2];
		int num = stream.Read(array2, 0, array2.Length);
		if (num == 0 || 1 == 0)
		{
			stream.Close();
			stream2.Close();
			return null;
		}
		stream2.Write(array2, 0, num);
		return p0;
	}

	internal void vsqfc(ArrayList p0, bool p1, bool p2)
	{
		Action<byte[], int> action = null;
		cvyri cvyri = new cvyri();
		cvyri.cpecj = p0;
		if (this is MimeMessage && 0 == 0)
		{
			p1 = true;
		}
		if (!movah.kldya || 1 == 0)
		{
			if (p1 && 0 == 0)
			{
				movah.tyaam("mime-version", new MimeVersion(1, 0));
			}
			movah.tyaam("content-type", vgqjo);
			if (fpqgm.Encoding == TransferEncoding.SevenBit && (!p2 || 1 == 0) && (ejpcp & MimeOptions.AlwaysWriteContentTransferEncoding) == 0)
			{
				movah.tyaam("content-transfer-encoding", null);
			}
			else
			{
				movah.tyaam("content-transfer-encoding", fpqgm);
			}
			if (p2 && 0 == 0)
			{
				MimeHeader mimeHeader = movah["content-disposition"];
				if (mimeHeader != null && 0 == 0)
				{
					mimeHeader.vysda(p0: true);
				}
			}
		}
		int num = 0;
		if (num != 0)
		{
			goto IL_00fd;
		}
		goto IL_019d;
		IL_00fd:
		if (((ejpcp & MimeOptions.DoNotWriteBcc) == 0 || string.Compare(movah[num].Name, "bcc", StringComparison.OrdinalIgnoreCase) != 0) && ((p2 ? true : false) || (ejpcp & MimeOptions.AlwaysWriteContentTransferEncoding) != 0 || ((string.Compare(movah[num].Name, "content-transfer-encoding", StringComparison.OrdinalIgnoreCase) != 0) ? true : false) || fpqgm.Encoding != TransferEncoding.SevenBit))
		{
			cvyri.cpecj.Add(movah[num]);
		}
		num++;
		goto IL_019d;
		IL_03e2:
		string boundary;
		cvyri.cpecj.Add("--" + boundary);
		int num2;
		zajxz[num2].vsqfc(cvyri.cpecj, p1: false, p2);
		num2++;
		goto IL_041b;
		IL_041b:
		if (num2 >= zajxz.Count)
		{
			cvyri.cpecj.Add("--" + boundary + "--");
			if (wcfgc != null && 0 == 0)
			{
				string text = wcfgc.Replace("\r", "");
				text = text.Replace("\n", "\r\n");
				cvyri.cpecj.Add(text);
			}
			return;
		}
		goto IL_03e2;
		IL_019d:
		if (num < movah.Count)
		{
			goto IL_00fd;
		}
		if (p1 && 0 == 0 && (ejpcp & MimeOptions.DoNotAddDateIfNoSubjectAndFrom) == 0 && (Headers["from"] == null || 1 == 0) && (Headers["subject"] == null || 1 == 0) && (Headers["date"] == null || 1 == 0))
		{
			cvyri.cpecj.Add(new MimeHeader("date", new MailDateTime(DateTime.Now), canonize: true));
		}
		cvyri.cpecj.Add(null);
		if (iuwky != null || (mbgbb != null && 0 == 0 && (!mbgbb.Detached || 1 == 0)))
		{
			cvyri.cpecj.Add(this);
			return;
		}
		if (wvvtb)
		{
			boundary = vgqjo.Boundary;
			if (boundary == null || 1 == 0)
			{
				throw new MimeException("Missing boundary parameter.", MimeExceptionStatus.UnspecifiedError);
			}
			if (wknht != null && 0 == 0)
			{
				string text2 = wknht.Replace("\r", "");
				text2 = text2.Replace("\n", "\r\n");
				cvyri.cpecj.Add(text2);
			}
			if (mbgbb != null && 0 == 0)
			{
				cvyri.cpecj.Add("--" + boundary);
				ContentInfo contentInfo = mbgbb.ContentInfo;
				if (action == null || 1 == 0)
				{
					action = cvyri.dkarc;
				}
				contentInfo.gfzrv(action);
				cvyri.cpecj.Add(null);
				cvyri.cpecj.Add("--" + boundary);
				MemoryStream memoryStream = new MemoryStream();
				mbgbb.Save(memoryStream);
				memoryStream.Position = 0L;
				MimeEntity mimeEntity = new MimeEntity();
				mimeEntity.gvccg = this;
				mimeEntity.SetContent(memoryStream, "smime.p7s", "application/pkcs7-signature", TransferEncoding.Base64);
				mimeEntity.vsqfc(cvyri.cpecj, p1: false, p2: false);
			}
			num2 = 0;
			if (num2 != 0)
			{
				goto IL_03e2;
			}
			goto IL_041b;
		}
		if (laxrm != null && 0 == 0)
		{
			laxrm.vsqfc(cvyri.cpecj, p1: true, p2);
		}
		else
		{
			cvyri.cpecj.Add(this);
		}
	}

	internal void layrr()
	{
		movah.Clear();
	}

	internal void eupxs(byte[] p0, int p1, int p2, Encoding p3)
	{
		int num = -1;
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_000b;
		}
		goto IL_001c;
		IL_000b:
		if (p0[p1 + num2] == 58)
		{
			num = num2;
			goto IL_0020;
		}
		num2++;
		goto IL_001c;
		IL_0020:
		if (num <= 0)
		{
			return;
		}
		if (p3 == null || 1 == 0)
		{
			p3 = EncodingTools.Default;
		}
		string text = p3.GetString(p0, p1, num).Trim();
		if ((text.Length != 0) ? true : false)
		{
			p1 += num + 1;
			p2 -= num + 1;
			if (p2 >= 1 && (p0[p1] == 32 || p0[p1] == 9 || p0[p1] == 10))
			{
				p1++;
				p2--;
			}
			byte[] array;
			if (p2 <= 0)
			{
				array = new byte[0];
			}
			else
			{
				array = new byte[p2];
				Array.Copy(p0, p1, array, 0, p2);
			}
			MimeHeader header = new MimeHeader(this, text, array);
			movah.Add(header);
		}
		return;
		IL_001c:
		if (num2 < p2)
		{
			goto IL_000b;
		}
		goto IL_0020;
	}

	internal void unatm(Encoding p0)
	{
		if (p0 == null || 1 == 0)
		{
			if (movah.etuur("content-type") is ContentType contentType && 0 == 0)
			{
				p0 = contentType.Encoding;
			}
			bool flag = false;
			if (p0 != null && 0 == 0)
			{
				string text = p0.WebName.ToLower(CultureInfo.InvariantCulture);
				if (text == "euc-kr" || text == "ks_c_5601-1987")
				{
					flag = true;
				}
			}
			if (p0 == null || false || p0 == EncodingTools.ASCII || ((!EncodingTools.yuhur(p0) || 1 == 0) && (!flag || 1 == 0)))
			{
				p0 = EncodingTools.Default;
			}
		}
		bool flag2 = (ejpcp & MimeOptions.ProcessAllHeaders) != 0;
		bool p1 = (ejpcp & MimeOptions.DoNotTrimHeaderValues) == 0;
		int num = 0;
		if (num != 0)
		{
			goto IL_00e8;
		}
		goto IL_013a;
		IL_00e8:
		MimeHeader mimeHeader = movah[num];
		if (!mimeHeader.xmhkl(p0, p1, this, vxcla) || 1 == 0)
		{
			movah.RemoveAt(num);
		}
		else if (flag2 && 0 == 0)
		{
			mimeHeader.vysda(p0: false);
		}
		num++;
		goto IL_013a;
		IL_013a:
		if (num < movah.Count)
		{
			goto IL_00e8;
		}
		ezreo();
	}

	private static string bgnwa(byte[] p0, int p1, int p2)
	{
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		if (num != 0)
		{
			goto IL_000c;
		}
		goto IL_002d;
		IL_000c:
		byte b = p0[p1 + num];
		if ((b >= 32 && b < 127) || b == 10)
		{
			stringBuilder.Append((char)b);
		}
		num++;
		goto IL_002d;
		IL_002d:
		if (num < p2)
		{
			goto IL_000c;
		}
		return stringBuilder.ToString();
	}

	internal void byyjx(byte[] p0, int p1, int p2)
	{
		wknht = bgnwa(p0, p1, p2);
	}

	internal void wuezd(byte[] p0, int p1, int p2)
	{
		wcfgc = bgnwa(p0, p1, p2);
	}

	internal void xebfp(MimeEntity p0)
	{
		laxrm = p0;
	}

	internal void sdfpb()
	{
		derxc = new vdmfr();
		if ((ejpcp & MimeOptions.KeepRawEntityBody) != 0)
		{
			csrgm = new opjbe();
		}
		if (IsMultipart && 0 == 0)
		{
			gkypg = null;
			return;
		}
		gkypg = derxc.ppkoc();
		ContentTransferEncoding contentTransferEncoding = ContentTransferEncoding;
		string text;
		if (contentTransferEncoding == null || false || (text = contentTransferEncoding.ToString()) == null)
		{
			return;
		}
		if (!(text == "base64") || 1 == 0)
		{
			if (text == "quoted-printable")
			{
				gkypg = new vmzeg(gkypg, ownsInner: true);
			}
			return;
		}
		Stream inner = gkypg;
		if (ceqsz == null || 1 == 0)
		{
			ceqsz = jsnmd;
		}
		gkypg = new xvufe(inner, ownsInner: true, ceqsz);
	}

	internal void ypyok(byte[] p0, int p1, int p2)
	{
		if (gkypg != null && 0 == 0)
		{
			gkypg.Write(p0, p1, p2);
		}
		if (csrgm != null && 0 == 0)
		{
			csrgm.Write(p0, p1, p2);
		}
	}

	internal void mxhog(Encoding p0)
	{
		if (gkypg != null && 0 == 0)
		{
			gkypg.Close();
			gkypg = null;
		}
		if (csrgm != null && 0 == 0)
		{
			csrgm.Position = 0L;
		}
		if (vgqjo == null || 1 == 0)
		{
			vgqjo = new ContentType("text/plain");
		}
		switch (fpqgm.Encoding)
		{
		default:
			if (vgqjo.msrzy != "text" && 0 == 0)
			{
				return;
			}
			break;
		case TransferEncoding.SevenBit:
		case TransferEncoding.EightBit:
			break;
		}
		if (vgqjo.msrzy == "multipart" && 0 == 0)
		{
			return;
		}
		int p1 = 0;
		int p2 = 0;
		int p3 = 0;
		int p4 = 0;
		int p5 = 0;
		int p6 = 0;
		zvjgu(derxc, derxc.tvoem, out p1, out p2, out p3, out p4, out p5, out p6);
		if (vgqjo.msrzy != "text" && 0 == 0)
		{
			if (p4 > 0 || p6 > 998)
			{
				fpqgm = new ContentTransferEncoding(TransferEncoding.QuotedPrintable);
				movah.tyaam("content-transfer-encoding", fpqgm);
			}
			return;
		}
		ContentDisposition contentDisposition = ContentDisposition;
		bool flag = contentDisposition != null && 0 == 0 && contentDisposition.Disposition == "attachment";
		Encoding p7 = vgqjo.Encoding;
		if (p7 != null && 0 == 0 && fpqgm.Encoding == TransferEncoding.Base64 && p3 >= 2 && p4 > 0 && (string.Compare(p7.WebName, "us-ascii", StringComparison.OrdinalIgnoreCase) == 0 || false || p7 == Encoding.BigEndianUnicode || p7 == Encoding.Unicode || p7 == Encoding.UTF8))
		{
			int tvoem = derxc.tvoem;
			if (arkdy(derxc, ref p7, out var p8) && 0 == 0)
			{
				if (!flag || 1 == 0)
				{
					derxc.jouoi(p8, tvoem - p8);
				}
				if (p7 != Encoding.UTF8)
				{
					p2 = 0;
					p3 = 0;
					p4 = 0;
				}
				vgqjo.CharSet = p7.WebName.ToLower(CultureInfo.InvariantCulture);
				movah.tyaam("content-type", vgqjo);
			}
		}
		vdmfr vdmfr;
		Stream stream;
		int tvoem2;
		int num;
		int num2;
		if (p4 > 0)
		{
			if (flag && 0 == 0)
			{
				fpqgm = new ContentTransferEncoding(TransferEncoding.Base64);
				movah.tyaam("content-transfer-encoding", fpqgm);
				vgqjo.dczab("application", "octet-stream");
				movah.tyaam("content-type", vgqjo);
				return;
			}
			if (p7 == null || false || !EncodingTools.jgvsv(p7) || 1 == 0)
			{
				p4 = 0;
				p1 = 0;
				p2 = 0;
				p3 = 0;
				p6 = 0;
				vdmfr = new vdmfr();
				stream = vdmfr.ppkoc();
				tvoem2 = derxc.tvoem;
				num = 0;
				num2 = 0;
				if (num2 != 0)
				{
					goto IL_0319;
				}
				goto IL_03c9;
			}
		}
		goto IL_03ec;
		IL_03ec:
		if (p6 > 998 && (fpqgm.Encoding == TransferEncoding.SevenBit || fpqgm.Encoding == TransferEncoding.EightBit))
		{
			fpqgm = new ContentTransferEncoding(TransferEncoding.QuotedPrintable);
			movah.tyaam("content-transfer-encoding", fpqgm);
		}
		if (p3 <= 0)
		{
			return;
		}
		if (p7 != null && 0 == 0)
		{
			if (p3 == p5 && p7.WebName.ToLower(CultureInfo.InvariantCulture) == "iso-2022-jp" && 0 == 0)
			{
				return;
			}
			if (p7 == EncodingTools.ASCII)
			{
				if (p0 != null && 0 == 0)
				{
					vgqjo.CharSet = p0.WebName.ToLower(CultureInfo.InvariantCulture);
				}
				else if (vgqjo.msrzy == "text" && 0 == 0 && vgqjo.mniut == "calendar")
				{
					byte[] array = derxc.xfpze();
					string s = Encoding.UTF8.GetString(array, 0, array.Length);
					byte[] bytes = Encoding.UTF8.GetBytes(s);
					if (bytes.Length == array.Length)
					{
						vgqjo.CharSet = Encoding.UTF8.WebName.ToLower(CultureInfo.InvariantCulture);
					}
					else
					{
						vgqjo.CharSet = EncodingTools.Default.WebName.ToLower(CultureInfo.InvariantCulture);
					}
				}
				else
				{
					int tvoem3 = derxc.tvoem;
					if (tvoem3 >= 3 && derxc.hpcfz(0) == 239 && derxc.hpcfz(1) == 187 && derxc.hpcfz(2) == 191)
					{
						vgqjo.CharSet = "utf-8";
					}
					else
					{
						vgqjo.CharSet = EncodingTools.Default.WebName.ToLower(CultureInfo.InvariantCulture);
					}
				}
				movah.tyaam("content-type", vgqjo);
			}
		}
		if (fpqgm.Encoding == TransferEncoding.SevenBit)
		{
			fpqgm = new ContentTransferEncoding(TransferEncoding.EightBit);
			movah.tyaam("content-transfer-encoding", fpqgm);
		}
		return;
		IL_0319:
		byte b = derxc.hpcfz(num2);
		switch (b)
		{
		case 10:
			p6 = Math.Max(p6, num);
			num = 0;
			stream.WriteByte(13);
			stream.WriteByte(10);
			p1 += 2;
			break;
		case 9:
			num++;
			stream.WriteByte(9);
			p1++;
			break;
		default:
			if (b >= 128)
			{
				p3++;
			}
			else if (b == 127 || b < 32)
			{
				p2++;
			}
			else
			{
				p1++;
			}
			stream.WriteByte(b);
			num++;
			break;
		case 0:
		case 13:
			break;
		}
		num2++;
		goto IL_03c9;
		IL_03c9:
		if (num2 < tvoem2)
		{
			goto IL_0319;
		}
		stream.Close();
		p6 = Math.Max(p6, num);
		derxc = vdmfr;
		goto IL_03ec;
	}

	private static bool arkdy(vdmfr p0, ref Encoding p1, out int p2)
	{
		int tvoem = p0.tvoem;
		p2 = 0;
		if (tvoem >= 2 && p0.hpcfz(0) == byte.MaxValue && p0.hpcfz(1) == 254)
		{
			p1 = Encoding.Unicode;
			p2 = 2;
			return true;
		}
		if (tvoem >= 2 && p0.hpcfz(0) == 254 && p0.hpcfz(1) == byte.MaxValue)
		{
			p1 = Encoding.BigEndianUnicode;
			p2 = 2;
			return true;
		}
		if (tvoem >= 3 && p0.hpcfz(0) == 239 && p0.hpcfz(1) == 187 && p0.hpcfz(2) == 191)
		{
			p1 = Encoding.UTF8;
			p2 = 3;
			return true;
		}
		return false;
	}

	internal void phsce(MimeEntity p0)
	{
		buivp = p0;
	}

	internal void mhdda()
	{
		qgdgd = new opjbe();
	}

	internal void xkiui(byte[] p0, int p1, int p2)
	{
		qgdgd.Write(p0, p1, p2);
	}

	internal void lcfpo()
	{
		if (qgdgd.Length > 2)
		{
			qgdgd.SetLength(qgdgd.Length - 2);
		}
		else
		{
			qgdgd.SetLength(0L);
		}
	}

	internal void fqpqf()
	{
		int num = 0;
		if (num != 0)
		{
			goto IL_0009;
		}
		goto IL_001e;
		IL_0009:
		zajxz[num].fqpqf();
		num++;
		goto IL_001e;
		IL_001e:
		if (num >= zajxz.Count)
		{
			if (laxrm != null && 0 == 0)
			{
				laxrm.fqpqf();
			}
			if (laxrm != null && 0 == 0)
			{
				if (buivp != null && 0 == 0)
				{
					string mediaType;
					if ((mediaType = buivp.vgqjo.MediaType) == null || false || (!(mediaType == "application/pkcs7-signature") && !(mediaType == "application/x-pkcs7-signature")))
					{
						return;
					}
					ContentInfo contentInfo = new ContentInfo(laxrm.qgdgd);
					laxrm.qgdgd = null;
					SignedData signedData = new SignedData(contentInfo, detached: true);
					signedData.Silent = ounaw;
					signedData.CertificateFinder = xtbgl;
					Stream contentStream = buivp.GetContentStream();
					try
					{
						signedData.Load(contentStream);
					}
					catch
					{
						if ((ejpcp & MimeOptions.IgnoreUnparsableSignatures) == 0)
						{
							throw;
						}
						signedData = null;
					}
					finally
					{
						if (contentStream != null && 0 == 0)
						{
							((IDisposable)contentStream).Dispose();
						}
					}
					mbgbb = signedData;
					if (signedData != null && 0 == 0)
					{
						laxrm.hhdfx();
						return;
					}
					zajxz.Add(laxrm);
					zajxz.Add(buivp);
					laxrm = null;
					buivp = null;
					vgqjo = new ContentType("multipart/mixed");
				}
				else if (vgqjo.MediaType == "multipart/signed" && 0 == 0)
				{
					zajxz.Add(laxrm);
					laxrm = null;
					vgqjo = new ContentType("multipart/mixed");
				}
			}
			else
			{
				string mediaType2;
				if (buivp != null || (mediaType2 = vgqjo.MediaType) == null)
				{
					return;
				}
				if (!(mediaType2 == "multipart/relative") || 1 == 0)
				{
					if (!(mediaType2 == "application/pkcs7-mime") && !(mediaType2 == "application/x-pkcs7-mime"))
					{
						return;
					}
					string text = vgqjo.Parameters["smime-type"];
					if (text != null && 0 == 0)
					{
						text = ((vgqjo.Parameters["x-smime-type"] == null) ? text.ToLower(CultureInfo.InvariantCulture) : null);
					}
					Stream contentStream2 = GetContentStream();
					try
					{
						if ((ejpcp & MimeOptions.DoNotParseMimeTree) != 0 || contentStream2.Length == 0)
						{
							return;
						}
						SignedData signedData2;
						EnvelopedData envelopedData;
						try
						{
							PkcsBase pkcsBase = PkcsBase.LoadSignedOrEnvelopedData(contentStream2, xtbgl, ounaw);
							signedData2 = pkcsBase as SignedData;
							envelopedData = pkcsBase as EnvelopedData;
						}
						catch (CryptographicException)
						{
							return;
						}
						string text2;
						if ((text2 = text) != null && 0 == 0)
						{
							if (text2 == "signed-data")
							{
								goto IL_0368;
							}
							if (text2 == "enveloped-data")
							{
								goto IL_03eb;
							}
						}
						goto IL_0404;
						IL_0404:
						if (signedData2 != null && 0 == 0)
						{
							vgqjo.Parameters["smime-type"] = "signed-data";
							vgqjo.Parameters["x-smime-type"] = null;
							goto IL_0368;
						}
						if (envelopedData != null)
						{
							vgqjo.Parameters["smime-type"] = "enveloped-data";
							vgqjo.Parameters["x-smime-type"] = null;
							goto IL_03eb;
						}
						return;
						IL_03eb:
						if (envelopedData != null && 0 == 0)
						{
							iuwky = envelopedData;
							return;
						}
						goto IL_0404;
						IL_0368:
						if (signedData2 != null && 0 == 0)
						{
							aviir<byte> p = signedData2.ContentInfo.fugju();
							if (!vyuzg(p))
							{
								return;
							}
							Stream stream = signedData2.ContentInfo.ToStream();
							try
							{
								MimeEntity mimeEntity = new MimeEntity();
								mimeEntity.yjlvs(this);
								mimeEntity.Load(stream);
								mimeEntity.hhdfx();
								laxrm = mimeEntity;
								mbgbb = signedData2;
								return;
							}
							catch (MimeException)
							{
								return;
							}
							finally
							{
								stream.Close();
							}
						}
						goto IL_0404;
					}
					finally
					{
						contentStream2.Close();
					}
				}
				vgqjo.dczab(vgqjo.msrzy, "related");
			}
			return;
		}
		goto IL_0009;
	}

	private static bool vyuzg(aviir<byte> p0)
	{
		if (p0 == null || false || p0.tvoem == 0 || 1 == 0)
		{
			return false;
		}
		int num = 0;
		int i = 0;
		int num2 = 0;
		int num3 = 0;
		bool flag = false;
		for (int num4 = Math.Min(p0.tvoem, 1024); i < num4; i++)
		{
			if (p0[i] == 58)
			{
				flag = true;
			}
			if (p0[i] == 10)
			{
				int num5 = i - num;
				if (i > num && p0[i - 1] == 13)
				{
					num5--;
				}
				if (num5 == 0 || 1 == 0)
				{
					i++;
					break;
				}
				num2++;
				if ((!flag || 1 == 0) && (!char.IsWhiteSpace((char)p0[num]) || 1 == 0))
				{
					num3++;
				}
				flag = false;
				num = i + 1;
			}
		}
		if (num3 > num2 / 2)
		{
			return false;
		}
		int p1 = 0;
		int p2 = 0;
		int p3 = 0;
		int p4 = 0;
		int p5 = 0;
		int p6 = 0;
		zvjgu(p0, i, out p1, out p2, out p3, out p4, out p5, out p6);
		int num6 = p1 / 4;
		if (p3 + p2 > num6 || p4 > num6 / 4)
		{
			return false;
		}
		return true;
	}

	public Stream GetContentStream(bool writable)
	{
		if (fckfg && 0 == 0 && writable && 0 == 0)
		{
			throw new MimeException("Cannot change read-only message.", MimeExceptionStatus.OperationError);
		}
		if (Kind != MimeEntityKind.Body && 0 == 0)
		{
			throw new MimeException("Accessing content stream of non-leaf entities is not supported.", MimeExceptionStatus.OperationError);
		}
		if (!writable || 1 == 0)
		{
			return derxc.kqazs();
		}
		derxc = derxc.xnfuu();
		return derxc.ppkoc();
	}

	public Stream GetContentStream()
	{
		return GetContentStream(writable: false);
	}

	public Stream GetRawContentStream()
	{
		if (csrgm == null || 1 == 0)
		{
			return null;
		}
		return new ctpqa(csrgm, ownsStream: true);
	}

	internal string dhgcb(Encoding p0, bool p1)
	{
		byte[] bytes = derxc.xfpze();
		int num = derxc.tvoem;
		int p2 = 0;
		if (p0 == Encoding.UTF8 || p0 == Encoding.Unicode || p0 == Encoding.BigEndianUnicode)
		{
			arkdy(derxc, ref p0, out p2);
			num -= p2;
		}
		string text = p0.GetString(bytes, p2, num);
		if (p1 && 0 == 0)
		{
			text = brgjd.mclrt(text, "\r", "");
		}
		return text;
	}

	private static void zvjgu(aviir<byte> p0, int p1, out int p2, out int p3, out int p4, out int p5, out int p6, out int p7)
	{
		p2 = 0;
		p3 = 0;
		p4 = 0;
		p5 = 0;
		p6 = 0;
		p7 = 0;
		if (p1 == 0 || 1 == 0)
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_0030;
		}
		goto IL_0114;
		IL_0030:
		num++;
		byte b = p0[num2];
		if (b >= 128)
		{
			p4++;
		}
		else if (b == 127)
		{
			p3++;
		}
		else if (b >= 32)
		{
			p2++;
		}
		else
		{
			switch (b)
			{
			case 10:
				p5++;
				break;
			case 13:
				if (num2 >= p1 - 1)
				{
					p5++;
					break;
				}
				if (p0[num2 + 1] != 10)
				{
					p5++;
				}
				else
				{
					p7 = Math.Max(p7, num - 1);
					num = 0;
					p2++;
				}
				num2++;
				break;
			case 9:
				p2++;
				break;
			case 0:
				p5++;
				break;
			case 27:
				p3++;
				p6++;
				break;
			default:
				p3++;
				break;
			}
		}
		num2++;
		goto IL_0114;
		IL_0114:
		if (num2 < p1)
		{
			goto IL_0030;
		}
		p7 = Math.Max(p7, num);
	}

	private static ContentTransferEncoding maoqt(vdmfr p0, ContentType p1, ContentTransferEncoding p2, bool p3)
	{
		TransferEncoding transferEncoding;
		if (p2 == null || 1 == 0)
		{
			transferEncoding = TransferEncoding.Unknown;
			if (transferEncoding != TransferEncoding.QuotedPrintable)
			{
				goto IL_001a;
			}
		}
		transferEncoding = p2.Encoding;
		goto IL_001a;
		IL_0311:
		return new ContentTransferEncoding(transferEncoding);
		IL_001a:
		if (p1.msrzy == "multipart" || p1.MediaType == "message/rfc822")
		{
			switch (transferEncoding)
			{
			case TransferEncoding.QuotedPrintable:
			case TransferEncoding.Base64:
				throw new MimeException(brgjd.edcru("Cannot use '{0}' encoding for '{1}' entity.", p2, p1), MimeExceptionStatus.OperationError);
			case TransferEncoding.SevenBit:
			case TransferEncoding.EightBit:
			case TransferEncoding.Binary:
				return p2;
			default:
				return new ContentTransferEncoding(TransferEncoding.SevenBit);
			}
		}
		if (p1.msrzy != "text" && 0 == 0)
		{
			switch (transferEncoding)
			{
			case TransferEncoding.QuotedPrintable:
			case TransferEncoding.Base64:
			case TransferEncoding.Binary:
				return p2;
			case TransferEncoding.Unknown:
				return new ContentTransferEncoding(TransferEncoding.Base64);
			}
		}
		int p4 = 0;
		int p5 = 0;
		int p6 = 0;
		int p7 = 0;
		int p8 = 0;
		int p9 = 0;
		zvjgu(p0, p0.tvoem, out p4, out p5, out p6, out p7, out p8, out p9);
		if (p7 > 0 || p6 > 0)
		{
			Encoding encoding = p1.Encoding;
			if (encoding != null && 0 == 0 && p1.msrzy == "text" && 0 == 0)
			{
				string text = encoding.WebName.ToLower(CultureInfo.InvariantCulture);
				if (text == "iso-2022-jp" && 0 == 0 && (p7 == 0 || 1 == 0) && p6 == p8)
				{
					return new ContentTransferEncoding(TransferEncoding.SevenBit);
				}
				if (text != "utf-8" && 0 == 0 && text != "utf-7" && 0 == 0 && (!EncodingTools.yuhur(encoding) || 1 == 0))
				{
					return new ContentTransferEncoding(TransferEncoding.Base64);
				}
			}
		}
		if (p7 > 0)
		{
			if (p3 && 0 == 0 && p1.msrzy == "text" && 0 == 0)
			{
				throw new MimeException("Body contains characters that are invalid for text.", MimeExceptionStatus.OperationError);
			}
			switch (transferEncoding)
			{
			case TransferEncoding.QuotedPrintable:
			case TransferEncoding.Base64:
			case TransferEncoding.Binary:
				return p2;
			case TransferEncoding.SevenBit:
			case TransferEncoding.EightBit:
				throw new MimeException("Body contains characters that cannot be encoded by the selected transfer encoding. Consider using Base64 or QuotedPrintable transfer encoding.", MimeExceptionStatus.OperationError);
			}
		}
		else if (p6 > 0)
		{
			switch (transferEncoding)
			{
			case TransferEncoding.QuotedPrintable:
			case TransferEncoding.Base64:
			case TransferEncoding.Binary:
				return p2;
			case TransferEncoding.EightBit:
				if (p9 <= 998)
				{
					return p2;
				}
				throw new MimeException("Body contains lines that are too long. Consider using Base64 or QuotedPrintable transfer encoding.", MimeExceptionStatus.OperationError);
			case TransferEncoding.SevenBit:
				throw new MimeException("Body contains characters that cannot be encoded by the selected transfer encoding. Consider using Base64 or QuotedPrintable transfer encoding.", MimeExceptionStatus.OperationError);
			}
		}
		else
		{
			switch (transferEncoding)
			{
			case TransferEncoding.QuotedPrintable:
			case TransferEncoding.Base64:
			case TransferEncoding.Binary:
				return p2;
			case TransferEncoding.SevenBit:
			case TransferEncoding.EightBit:
				if (p9 <= 998)
				{
					return p2;
				}
				throw new MimeException("Body contains lines that are too long. Consider using Base64 or QuotedPrintable transfer encoding.", MimeExceptionStatus.OperationError);
			}
		}
		if (p7 > 0)
		{
			transferEncoding = TransferEncoding.Base64;
			if (transferEncoding != TransferEncoding.QuotedPrintable)
			{
				goto IL_0311;
			}
		}
		if (p6 > 0 || p5 > 0)
		{
			if (p1.msrzy == "text" || p4 > (p6 + p5) * 3)
			{
				transferEncoding = TransferEncoding.QuotedPrintable;
				if (transferEncoding == TransferEncoding.QuotedPrintable)
				{
					goto IL_0311;
				}
			}
			transferEncoding = TransferEncoding.Base64;
			if (transferEncoding != TransferEncoding.QuotedPrintable)
			{
				goto IL_0311;
			}
		}
		if (p9 <= 76)
		{
			transferEncoding = TransferEncoding.SevenBit;
			if (transferEncoding != TransferEncoding.QuotedPrintable)
			{
				goto IL_0311;
			}
		}
		transferEncoding = TransferEncoding.QuotedPrintable;
		goto IL_0311;
	}

	private void iltqs(ContentType p0, ref ContentDisposition p1, string p2)
	{
		if (p2 == null || false || p2.Length == 0 || 1 == 0)
		{
			if (p1 != null && 0 == 0)
			{
				p1.FileName = null;
			}
			p0.Parameters["name"] = null;
			return;
		}
		if (p1 == null || 1 == 0)
		{
			p1 = new ContentDisposition("attachment");
		}
		p1.FileName = p2;
		p0.Parameters["name"] = p2;
	}

	private void tngim(vdmfr p0, ContentType p1, ContentTransferEncoding p2, bool p3, string p4, bool p5)
	{
		if (fckfg && 0 == 0)
		{
			throw new MimeException("Cannot change read-only message.", MimeExceptionStatus.OperationError);
		}
		bool flag = p1.msrzy == "multipart" && 0 == 0 && (ejpcp & MimeOptions.DoNotParseMimeTree) == 0;
		bool flag2 = p1.MediaType == "message/rfc822" && 0 == 0 && (ejpcp & MimeOptions.DoNotParseMimeTree) == 0;
		bool flag3 = mbgbb != null || iuwky != null;
		if ((flag ? true : false) || flag2)
		{
			p0 = new vdmfr();
		}
		if (p2 != fpqgm)
		{
			if (!p5 || 1 == 0)
			{
				p2 = maoqt(p0, p1, p2, (ejpcp & MimeOptions.AllowAnyTextCharacters) == 0);
			}
			else if (p2 == null || 1 == 0)
			{
				p2 = new ContentTransferEncoding(TransferEncoding.Base64);
			}
		}
		if (p3 && 0 == 0)
		{
			if (p4 != null && 0 == 0)
			{
				kgbvh.wbwli(p4, "name");
			}
			ContentDisposition p6 = ContentDisposition;
			iltqs(p1, ref p6, p4);
			ContentDisposition = p6;
		}
		if (!flag || 1 == 0)
		{
			zajxz.Clear();
		}
		if ((!flag2 || 1 == 0) && (!flag3 || 1 == 0))
		{
			laxrm = null;
		}
		wvvtb = flag;
		jbrce = flag2;
		if (!flag3 || 1 == 0)
		{
			qgdgd = null;
			buivp = null;
			mbgbb = null;
			iuwky = null;
		}
		ufknm(p0, p1);
		derxc = p0;
		csrgm = null;
		vgqjo = p1;
		fpqgm = p2;
		movah.tyaam("content-type", p1);
		movah.tyaam("content-transfer-encoding", p2);
	}

	private void ufknm(vdmfr p0, ContentType p1)
	{
		if (p1.msrzy != "text" || p0.iihdy)
		{
			return;
		}
		Encoding p2 = null;
		if (arkdy(p0, ref p2, out var p3) && 0 == 0)
		{
			p0.jouoi(p3, p0.tvoem - p3);
			if (string.IsNullOrEmpty(p1.CharSet) && 0 == 0)
			{
				p1.CharSet = p2.WebName.ToLower(CultureInfo.InvariantCulture);
			}
		}
	}

	private void miikc(string p0, string p1, Encoding p2, ContentTransferEncoding p3)
	{
		if (fckfg && 0 == 0)
		{
			throw new MimeException("Cannot change read-only message.", MimeExceptionStatus.OperationError);
		}
		ContentType contentType = new ContentType(p1);
		if (contentType.msrzy == "message" || contentType.MediaType == "message/rfc822")
		{
			throw new ArgumentException(brgjd.edcru("Media type '{0}' is not appropriate for text.", p1), "mediaType");
		}
		if (p3 != null && 0 == 0 && (!p3.IsKnown || 1 == 0))
		{
			throw new ArgumentException("Unknown transfer encoding.", "transferEncoding");
		}
		byte[] p4;
		if (p2 == null || 1 == 0)
		{
			p2 = kgbvh.gptax(vgqjo.Encoding, itjdj, p0, out p4);
		}
		else
		{
			p4 = p2.GetBytes(p0);
		}
		vdmfr vdmfr;
		Stream stream;
		int num;
		switch (p2.CodePage)
		{
		case 1200:
		case 1201:
		case 12000:
		case 12001:
			vdmfr = vdmfr.rcqyz(p4);
			break;
		default:
			{
				vdmfr = new vdmfr();
				stream = vdmfr.ppkoc();
				num = 0;
				if (num != 0)
				{
					goto IL_012e;
				}
				goto IL_0164;
			}
			IL_0164:
			if (num >= p4.Length)
			{
				stream.Close();
				break;
			}
			goto IL_012e;
			IL_012e:
			switch (p4[num])
			{
			case 10:
				stream.WriteByte(13);
				stream.WriteByte(10);
				break;
			default:
				stream.WriteByte(p4[num]);
				break;
			case 13:
				break;
			}
			num++;
			goto IL_0164;
		}
		contentType.CharSet = p2.WebName.ToLower(CultureInfo.InvariantCulture);
		tngim(vdmfr, contentType, p3, p3: true, null, p5: false);
	}

	public void SetContent(string text)
	{
		if (text == null || 1 == 0)
		{
			throw new ArgumentNullException("text");
		}
		miikc(text, "text/plain", null, null);
	}

	public void SetContent(string text, string mediaType)
	{
		if (text == null || 1 == 0)
		{
			throw new ArgumentNullException("text");
		}
		if (mediaType == null || 1 == 0)
		{
			throw new ArgumentNullException("mediaType");
		}
		miikc(text, mediaType, null, null);
	}

	public void SetContent(string text, string mediaType, Encoding charset)
	{
		if (text == null || 1 == 0)
		{
			throw new ArgumentNullException("text");
		}
		if (mediaType == null || 1 == 0)
		{
			throw new ArgumentNullException("mediaType");
		}
		if (charset == null || 1 == 0)
		{
			throw new ArgumentNullException("charset");
		}
		miikc(text, mediaType, charset, null);
	}

	public void SetContent(string text, string mediaType, Encoding charset, TransferEncoding transferEncoding)
	{
		if (text == null || 1 == 0)
		{
			throw new ArgumentNullException("text");
		}
		if (mediaType == null || 1 == 0)
		{
			throw new ArgumentNullException("mediaType");
		}
		ContentTransferEncoding p = new ContentTransferEncoding(transferEncoding);
		miikc(text, mediaType, charset, p);
	}

	private static bool ffgjd(MimeEntity p0, MimeEntity p1)
	{
		if (p0 == null || 1 == 0)
		{
			return false;
		}
		if (p0 == p1)
		{
			return true;
		}
		int num = 0;
		if (num != 0)
		{
			goto IL_0018;
		}
		goto IL_003a;
		IL_0018:
		if (ffgjd(p0.zajxz[num], p1) && 0 == 0)
		{
			return true;
		}
		num++;
		goto IL_003a;
		IL_003a:
		if (num >= p0.zajxz.Count)
		{
			if (ffgjd(p0.laxrm, p1) && 0 == 0)
			{
				return true;
			}
			return false;
		}
		goto IL_0018;
	}

	public void SetContent(MimeEntity entity)
	{
		if (entity == null || 1 == 0)
		{
			throw new ArgumentNullException("entity");
		}
		if (fckfg && 0 == 0)
		{
			throw new MimeException("Cannot change read-only message.", MimeExceptionStatus.OperationError);
		}
		if (ffgjd(this, entity) && 0 == 0)
		{
			throw new MimeException("Adding this entity to the list would lead to circular dependency.", MimeExceptionStatus.OperationError);
		}
		zajxz.Clear();
		laxrm = entity;
		derxc = new vdmfr();
		csrgm = null;
		vgqjo = new ContentType("message/rfc822");
		switch (entity.fpqgm.Encoding)
		{
		case TransferEncoding.QuotedPrintable:
		case TransferEncoding.Base64:
			fpqgm = new ContentTransferEncoding(TransferEncoding.SevenBit);
			break;
		default:
			fpqgm = entity.fpqgm;
			break;
		}
		movah.tyaam("content-type", vgqjo);
		movah.tyaam("content-transfer-encoding", fpqgm);
		wvvtb = false;
		jbrce = true;
		qgdgd = null;
		buivp = null;
		mbgbb = null;
		iuwky = null;
		ContentDisposition = null;
	}

	public void SetEnvelopedContent(MimeEntity entity, params Certificate[] recipients)
	{
		kpuqp(entity, null, 0, null, recipients);
	}

	public void SetEnvelopedContent(MimeEntity entity, string encryptionAlgorithm, params Certificate[] recipients)
	{
		kpuqp(entity, encryptionAlgorithm, 0, null, recipients);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[wptwl(false)]
	[Obsolete("This method has been deprecated. Please use another overload instead.", true)]
	public void SetEnvelopedContent(MimeEntity entity, string encryptionAlgorithm, int keyLength, params Certificate[] recipients)
	{
		kpuqp(entity, encryptionAlgorithm, keyLength, null, recipients);
	}

	public void SetEnvelopedContent(MimeEntity entity, string encryptionAlgorithm, EncryptionParameters encryptionParameters, params Certificate[] recipients)
	{
		kpuqp(entity, encryptionAlgorithm, 0, encryptionParameters, recipients);
	}

	public void SetEnvelopedContent(MimeEntity entity, SymmetricKeyAlgorithmId encryptionAlgorithm, EncryptionParameters encryptionParameters, params Certificate[] recipients)
	{
		SetEnvelopedContent(entity, encryptionAlgorithm, null, encryptionParameters, recipients);
	}

	public void SetEnvelopedContent(MimeEntity entity, SymmetricKeyAlgorithmId encryptionAlgorithm, int? keySize, EncryptionParameters encryptionParameters, params Certificate[] recipients)
	{
		string text = bpkgq.oiant(encryptionAlgorithm, keySize);
		if (text == null || 1 == 0)
		{
			if (encryptionAlgorithm == SymmetricKeyAlgorithmId.AES || 1 == 0)
			{
				throw new ArgumentException("Invalid AES key length.", "keyLength");
			}
			throw new ArgumentException("Invalid encryption algorithm.", "encryptionAlgorithm");
		}
		kpuqp(entity, text, (encryptionAlgorithm == SymmetricKeyAlgorithmId.ArcTwo) ? (keySize ?? 0) : 0, encryptionParameters, recipients);
	}

	private void kpuqp(MimeEntity p0, string p1, int p2, EncryptionParameters p3, params Certificate[] p4)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("entity");
		}
		if (p4 == null || 1 == 0)
		{
			throw new ArgumentNullException("recipients");
		}
		if (p4.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("One or more recipients have to be specified to be able to encrypt the message.", "recipients");
		}
		if (Array.IndexOf(p4, null, 0, p4.Length) >= 0)
		{
			throw new ArgumentException("Null item is present in the collection.", "recipients");
		}
		if (fckfg && 0 == 0)
		{
			throw new MimeException("Cannot change read-only message.", MimeExceptionStatus.OperationError);
		}
		if (ffgjd(this, p0) && 0 == 0)
		{
			throw new MimeException("Adding this entity to the list would lead to circular dependency.", MimeExceptionStatus.OperationError);
		}
		if (p1 == null || 1 == 0)
		{
			p1 = "1.2.840.113549.3.7";
		}
		EnvelopedData envelopedData = new EnvelopedData(null, new ObjectIdentifier(p1), p2);
		envelopedData.Silent = ounaw;
		envelopedData.CertificateFinder = xtbgl;
		int num = 0;
		if (num != 0)
		{
			goto IL_00ee;
		}
		goto IL_019e;
		IL_00ee:
		Certificate certificate = p4[num];
		switch (certificate.KeyAlgorithm)
		{
		case KeyAlgorithm.RSA:
			break;
		case KeyAlgorithm.DSA:
			throw new MimeException("DSA certificate cannot be used to encrypt data.", MimeExceptionStatus.OperationError);
		default:
			throw new MimeException(brgjd.edcru("Unsupported certificate key type '{0}'.", certificate.KeyAlgorithm), MimeExceptionStatus.OperationError);
		}
		int keySize = certificate.GetKeySize();
		int? p5 = envelopedData.nsmfw();
		if (foadp == null || 1 == 0)
		{
			foadp = hvkln;
		}
		EncryptionParameters.radkb(p3, AsymmetricKeyAlgorithmId.RSA, keySize, p5, foadp);
		KeyTransRecipientInfo item = new KeyTransRecipientInfo(certificate, p3, SubjectIdentifierType.IssuerAndSerialNumber);
		envelopedData.RecipientInfos.Add(item);
		num++;
		goto IL_019e;
		IL_019e:
		if (num >= p4.Length)
		{
			zajxz.Clear();
			laxrm = p0;
			derxc = new vdmfr();
			csrgm = null;
			vgqjo = new ContentType("application/pkcs7-mime");
			vgqjo.Parameters["smime-type"] = "enveloped-data";
			fpqgm = new ContentTransferEncoding(TransferEncoding.Base64);
			movah.tyaam("content-type", vgqjo);
			movah.tyaam("content-transfer-encoding", fpqgm);
			Name = "smime.p7m";
			wvvtb = false;
			jbrce = false;
			qgdgd = null;
			buivp = null;
			mbgbb = null;
			iuwky = envelopedData;
			return;
		}
		goto IL_00ee;
	}

	public void SetSignedContent(MimeEntity entity, params Certificate[] signers)
	{
		SetSignedContent(entity, MimeSignatureStyle.Detached, null, signers);
	}

	public void SetSignedContent(MimeEntity entity, MimeSignatureStyle style, params Certificate[] signers)
	{
		SetSignedContent(entity, style, null, signers);
	}

	public void SetSignedContent(MimeEntity entity, MimeSignatureStyle style, SignatureHashAlgorithm algorithm, params Certificate[] signers)
	{
		SetSignedContent(entity, style, SignatureParameters.preyi(algorithm, "algorithm"), signers);
	}

	public void SetSignedContent(MimeEntity entity, SignatureParameters signatureParameters, params Certificate[] signers)
	{
		SetSignedContent(entity, MimeSignatureStyle.Detached, signatureParameters, signers);
	}

	public void SetSignedContent(MimeEntity entity, MimeSignatureStyle style, SignatureParameters signatureParameters, params Certificate[] signers)
	{
		if (entity == null || 1 == 0)
		{
			throw new ArgumentNullException("entity");
		}
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
		if (fckfg && 0 == 0)
		{
			throw new MimeException("Cannot change read-only message.", MimeExceptionStatus.OperationError);
		}
		if (ffgjd(this, entity) && 0 == 0)
		{
			throw new MimeException("Adding this entity to the list would lead to circular dependency.", MimeExceptionStatus.OperationError);
		}
		onlmf(signatureParameters);
		bool flag;
		switch (style)
		{
		case MimeSignatureStyle.None:
			throw new MimeException("Specified signature style is not valid for the this entity.", MimeExceptionStatus.OperationError);
		case MimeSignatureStyle.Enveloped:
			flag = true;
			if (flag)
			{
				break;
			}
			goto case MimeSignatureStyle.Detached;
		case MimeSignatureStyle.Detached:
			flag = false;
			if (!flag)
			{
				break;
			}
			goto default;
		default:
			throw new ArgumentException("Specified signature style is unknown.", "style");
		}
		ContentInfo contentInfo = new ContentInfo();
		SignedData signedData = new SignedData(contentInfo, !flag);
		signedData.Silent = ounaw;
		signedData.CertificateFinder = xtbgl;
		int num = 0;
		if (num != 0)
		{
			goto IL_0122;
		}
		goto IL_01ad;
		IL_01ad:
		if (num >= signers.Length)
		{
			zajxz.Clear();
			laxrm = entity;
			derxc = new vdmfr();
			csrgm = null;
			hepyx(flag, signedData.SignerInfos[0].ToDigestAlgorithm());
			jbrce = false;
			qgdgd = null;
			buivp = null;
			iuwky = null;
			mbgbb = signedData;
			return;
		}
		goto IL_0122;
		IL_0122:
		Certificate certificate = signers[num];
		iuxwn(signatureParameters, certificate);
		if (certificate.KeyAlgorithm == KeyAlgorithm.RSA || 1 == 0)
		{
			int keySize = certificate.GetKeySize();
			if (qujjt == null || 1 == 0)
			{
				qujjt = drptr;
			}
			SignatureParameters.wdmbv(signatureParameters, AsymmetricKeyAlgorithmId.RSA, keySize, qujjt);
		}
		SignerInfo signerInfo = new SignerInfo(certificate, signatureParameters, SubjectIdentifierType.IssuerAndSerialNumber);
		if ((ejpcp & MimeOptions.DisableEncryptionKeyPreference) == 0)
		{
			signerInfo.EncryptionKeyPreference = signerInfo.SignerIdentifier;
		}
		signedData.SignerInfos.Add(signerInfo);
		num++;
		goto IL_01ad;
	}

	private static void onlmf(SignatureParameters p0)
	{
		if (p0 != null)
		{
			if (p0.Format != 0 && 0 == 0 && p0.Format != SignatureFormat.Pkcs)
			{
				string text = bpkgq.alqlw(p0.Format);
				throw new MimeException(brgjd.edcru("Unsupported signature format '{0}'.", text), MimeExceptionStatus.OperationError);
			}
			switch (p0.PaddingScheme)
			{
			case SignaturePaddingScheme.Default:
			case SignaturePaddingScheme.Pkcs1:
			case SignaturePaddingScheme.Pss:
				return;
			}
			string text2 = bpkgq.xptdi(p0.PaddingScheme);
			throw new MimeException(brgjd.edcru("The {0} padding scheme is not supported.", text2), MimeExceptionStatus.OperationError);
		}
	}

	private static void iuxwn(SignatureParameters p0, Certificate p1)
	{
		if (p0 == null)
		{
			return;
		}
		switch (p1.KeyAlgorithm)
		{
		case KeyAlgorithm.RSA:
			switch (p0.HashAlgorithm)
			{
			default:
			{
				string text2 = bpkgq.hdrmd(p0.HashAlgorithm);
				throw new MimeException(brgjd.edcru("The {0} signature hashing algorithm cannot be used with {1} certificate.", text2, p1.KeyAlgorithm), MimeExceptionStatus.OperationError);
			}
			case (HashingAlgorithmId)0:
			case HashingAlgorithmId.SHA1:
			case HashingAlgorithmId.SHA224:
			case HashingAlgorithmId.SHA256:
			case HashingAlgorithmId.SHA384:
			case HashingAlgorithmId.SHA512:
			case HashingAlgorithmId.MD4:
			case HashingAlgorithmId.MD5:
				break;
			}
			break;
		case KeyAlgorithm.DSA:
			switch (p0.HashAlgorithm)
			{
			default:
				bpkgq.hdrmd(p0.HashAlgorithm);
				throw new MimeException("Only SHA-1 is supported for DSA certificates.", MimeExceptionStatus.OperationError);
			case (HashingAlgorithmId)0:
			case HashingAlgorithmId.SHA1:
				if (p0.PaddingScheme != SignaturePaddingScheme.Default && 0 == 0)
				{
					string text = bpkgq.xptdi(p0.PaddingScheme);
					throw new MimeException(brgjd.edcru("The {0} padding scheme cannot be used with {1} certificate.", text, p1.KeyAlgorithm), MimeExceptionStatus.OperationError);
				}
				break;
			}
			break;
		default:
			throw new MimeException(brgjd.edcru("Unsupported certificate key type '{0}'.", p1.KeyAlgorithm), MimeExceptionStatus.OperationError);
		}
	}

	private void hepyx(bool p0, SignatureHashAlgorithm p1)
	{
		if (!p0 || 1 == 0)
		{
			vgqjo = new ContentType("multipart/signed");
			switch (p1)
			{
			case SignatureHashAlgorithm.MD4:
				vgqjo.Parameters["micalg"] = "MD4";
				break;
			case SignatureHashAlgorithm.MD5:
				vgqjo.Parameters["micalg"] = "MD5";
				break;
			case SignatureHashAlgorithm.SHA1:
				vgqjo.Parameters["micalg"] = "SHA1";
				break;
			case SignatureHashAlgorithm.SHA224:
				vgqjo.Parameters["micalg"] = "SHA224";
				break;
			case SignatureHashAlgorithm.SHA256:
				vgqjo.Parameters["micalg"] = "SHA256";
				break;
			case SignatureHashAlgorithm.SHA384:
				vgqjo.Parameters["micalg"] = "SHA384";
				break;
			case SignatureHashAlgorithm.SHA512:
				vgqjo.Parameters["micalg"] = "SHA512";
				break;
			default:
				throw new MimeException(brgjd.edcru("The {0} signature hashing algorithm cannot be used with {1} certificate.", p1, "this"), MimeExceptionStatus.OperationError);
			}
			vgqjo.Parameters["protocol"] = "application/pkcs7-signature";
			movah.tyaam("content-type", vgqjo);
			movah.tyaam("content-transfer-encoding", fpqgm);
			ContentDisposition = null;
			wvvtb = true;
		}
		else
		{
			vgqjo = new ContentType("application/pkcs7-mime");
			vgqjo.Parameters["smime-type"] = "signed-data";
			fpqgm = new ContentTransferEncoding(TransferEncoding.Base64);
			movah.tyaam("content-type", vgqjo);
			movah.tyaam("content-transfer-encoding", fpqgm);
			Name = "smime.p7m";
			wvvtb = false;
		}
	}

	private void vbqsm(Stream p0, string p1, string p2, ContentTransferEncoding p3)
	{
		if (fckfg && 0 == 0)
		{
			throw new MimeException("Cannot change read-only message.", MimeExceptionStatus.OperationError);
		}
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("source");
		}
		if (p2 == null || 1 == 0)
		{
			throw new ArgumentNullException("mediaType");
		}
		if (p1 == null || 1 == 0)
		{
			p1 = "";
		}
		if (p3 != null && 0 == 0 && (!p3.IsKnown || 1 == 0))
		{
			throw new ArgumentException("Unknown transfer encoding.", "transferEncoding");
		}
		ContentType contentType = new ContentType(p2);
		try
		{
			if (contentType.MediaType == "message/rfc822" && 0 == 0 && (ejpcp & MimeOptions.DoNotParseMimeTree) == 0 && 0 == 0)
			{
				MimeMessage mimeMessage = new MimeMessage();
				mimeMessage.ejpcp = ejpcp;
				mimeMessage.Load(p0);
				SetContent(mimeMessage);
			}
			else
			{
				vdmfr p4 = vdmfr.utvjv(p0);
				tngim(p4, contentType, p3, p3: true, p1, p5: false);
			}
		}
		finally
		{
			p0.Close();
		}
	}

	private void ghvjw(vdmfr p0, string p1, string p2, ContentTransferEncoding p3)
	{
		if (fckfg && 0 == 0)
		{
			throw new MimeException("Cannot change read-only message.", MimeExceptionStatus.OperationError);
		}
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("body");
		}
		if (p2 == null || 1 == 0)
		{
			throw new ArgumentNullException("mediaType");
		}
		if (p1 == null || 1 == 0)
		{
			p1 = "";
		}
		if (p3 != null && 0 == 0 && (!p3.IsKnown || 1 == 0))
		{
			throw new ArgumentException("Unknown transfer encoding.", "transferEncoding");
		}
		ContentType p4 = new ContentType(p2);
		tngim(p0, p4, p3, p3: true, p1, p5: true);
	}

	public void SetContent(Stream source, string name, string mediaType, TransferEncoding transferEncoding)
	{
		vbqsm(source, name, mediaType, new ContentTransferEncoding(transferEncoding));
	}

	public void SetContent(Stream source, string name, string mediaType)
	{
		vbqsm(source, name, mediaType, null);
	}

	public void SetContent(Stream source, string name)
	{
		vbqsm(source, name, "application/octet-stream", null);
	}

	public void SetContent(Stream source)
	{
		vbqsm(source, null, "application/octet-stream", null);
	}

	internal void qtbpa(string p0, long p1, string p2, string p3, TransferEncoding p4)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("fileName");
		}
		ContentTransferEncoding p5 = ((p4 == TransferEncoding.Unknown) ? null : new ContentTransferEncoding(p4));
		if ((ejpcp & MimeOptions.DoNotPreloadAttachments) != 0)
		{
			vdmfr p6 = new vdmfr(p0, p1);
			ghvjw(p6, p2, p3, p5);
			return;
		}
		Stream stream = File.OpenRead(p0);
		try
		{
			vbqsm(stream, p2, p3, p5);
		}
		finally
		{
			if (stream != null && 0 == 0)
			{
				((IDisposable)stream).Dispose();
			}
		}
	}

	public void SetContentFromFile(string fileName, string name, string mediaType, TransferEncoding transferEncoding)
	{
		qtbpa(fileName, 0L, name, mediaType, transferEncoding);
	}

	public void SetContentFromFile(string fileName, string name, string mediaType)
	{
		qtbpa(fileName, 0L, name, mediaType, TransferEncoding.Unknown);
	}

	public void SetContentFromFile(string fileName, string name)
	{
		qtbpa(fileName, 0L, name, "application/octet-stream", TransferEncoding.Unknown);
	}

	public void SetContentFromFile(string fileName)
	{
		qtbpa(fileName, 0L, null, "application/octet-stream", TransferEncoding.Unknown);
	}

	public void Decrypt()
	{
		if (iuwky == null || 1 == 0)
		{
			throw new MimeException("Unable to encrypt or decrypt an entity that does not have encrypted content.", MimeExceptionStatus.OperationError);
		}
		if (!iuwky.IsEncrypted || 1 == 0)
		{
			throw new MimeException("The content of this entity is not encrypted.", MimeExceptionStatus.OperationError);
		}
		iuwky.Silent = ounaw;
		iuwky.grqku(((ejpcp & MimeOptions.SkipCertificateUsageCheck) == 0) ? azsca.iuqjz : azsca.xtlsb);
		Stream stream = iuwky.ContentInfo.ToStream();
		try
		{
			laxrm = new MimeEntity();
			laxrm.yjlvs(this);
			laxrm.Load(stream);
		}
		finally
		{
			if (stream != null && 0 == 0)
			{
				((IDisposable)stream).Dispose();
			}
		}
	}

	public void Encrypt()
	{
		if (iuwky == null || 1 == 0)
		{
			throw new MimeException("Unable to encrypt or decrypt an entity that does not have encrypted content.", MimeExceptionStatus.OperationError);
		}
		if (iuwky.IsEncrypted && 0 == 0)
		{
			throw new MimeException("The content of this entity is already encrypted.", MimeExceptionStatus.OperationError);
		}
		opjbe opjbe = new opjbe();
		laxrm.izpsa(opjbe, p1: false, p2: false);
		if (laxrm.zajxz.Count == 0 || 1 == 0)
		{
			opjbe.SetLength(opjbe.Length - 2);
		}
		iuwky.ContentInfo = new ContentInfo(opjbe);
		iuwky.rgetd(((ejpcp & MimeOptions.SkipCertificateUsageCheck) == 0) ? azsca.iuqjz : azsca.xtlsb);
		laxrm = null;
	}

	public void Sign()
	{
		if (mbgbb == null || 1 == 0)
		{
			throw new MimeException("Unable to sign or validate entity that does not have signed content.", MimeExceptionStatus.OperationError);
		}
		bool flag = false;
		IEnumerator<SignerInfo> enumerator = mbgbb.SignerInfos.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				SignerInfo current = enumerator.Current;
				if (current.Signature != null && 0 == 0)
				{
					flag = true;
					if (flag)
					{
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
		if (flag && 0 == 0)
		{
			throw new MimeException("The content of this entity is already signed.", MimeExceptionStatus.OperationError);
		}
		if (!laxrm.upzzo || 1 == 0)
		{
			laxrm.wknht = "";
			laxrm.wcfgc = null;
			laxrm.kjyki();
		}
		opjbe opjbe = new opjbe();
		laxrm.izpsa(opjbe, p1: false, p2: true);
		if (laxrm.zajxz.Count == 0 || 1 == 0)
		{
			opjbe.SetLength(opjbe.Length - 2);
		}
		mbgbb.ContentInfo = new ContentInfo(opjbe);
		SignatureOptions signatureOptions = (SignatureOptions)0L;
		if ((ejpcp & MimeOptions.DisableSMimeCapabilitiesAttribute) != 0)
		{
			signatureOptions |= SignatureOptions.DisableSMimeCapabilities;
		}
		if ((ejpcp & MimeOptions.SkipCertificateUsageCheck) != 0)
		{
			signatureOptions |= SignatureOptions.SkipCertificateUsageCheck;
		}
		mbgbb.mjncq(signatureOptions, azsca.iuqjz);
		laxrm.hhdfx();
	}

	public SignatureValidationResult ValidateSignature()
	{
		return ValidateSignature(verifySignatureOnly: false, ValidationOptions.None, CertificateChainEngine.Auto);
	}

	public SignatureValidationResult ValidateSignature(bool verifySignatureOnly, ValidationOptions options)
	{
		return ValidateSignature(verifySignatureOnly, options, CertificateChainEngine.Auto);
	}

	public SignatureValidationResult ValidateSignature(bool verifySignatureOnly, ValidationOptions options, CertificateChainEngine certificateEngine)
	{
		if (mbgbb == null || 1 == 0)
		{
			throw new MimeException("Unable to sign or validate entity that does not have signed content.", MimeExceptionStatus.OperationError);
		}
		if ((ejpcp & MimeOptions.SkipCertificateUsageCheck) != 0)
		{
			options |= ValidationOptions.IgnoreWrongUsage;
		}
		return mbgbb.hezvv(verifySignatureOnly, options, certificateEngine, azsca.iuqjz);
	}

	public MimeEntity GetSignatureEntity()
	{
		if (mbgbb == null || 1 == 0)
		{
			return null;
		}
		if (buivp == null || 1 == 0)
		{
			return this;
		}
		return buivp;
	}

	private void ezreo()
	{
		vgqjo = movah.etuur("content-type") as ContentType;
		fpqgm = movah.etuur("content-transfer-encoding") as ContentTransferEncoding;
		wvvtb = false;
		jbrce = false;
		if (fpqgm != null && 0 == 0)
		{
			if (!fpqgm.IsKnown || 1 == 0)
			{
				if (vgqjo == null || 1 == 0)
				{
					vgqjo = new ContentType("text/plain");
					return;
				}
				string msrzy;
				if ((msrzy = vgqjo.msrzy) == null || false || (!(msrzy == "multipart") && !(msrzy == "message")))
				{
					return;
				}
				fpqgm = new ContentTransferEncoding(TransferEncoding.Binary);
			}
		}
		else
		{
			fpqgm = new ContentTransferEncoding(TransferEncoding.SevenBit);
		}
		if (vgqjo == null || 1 == 0)
		{
			if (gvccg != null && 0 == 0 && gvccg.vgqjo != null && 0 == 0 && gvccg.wvvtb && 0 == 0 && gvccg.vgqjo.MediaType == "multipart/digest" && 0 == 0)
			{
				vgqjo = new ContentType("message/rfc822");
				jbrce = true;
			}
			else
			{
				vgqjo = new ContentType("text/plain");
			}
		}
		else if (vgqjo.msrzy == "multipart" && 0 == 0)
		{
			string boundary = vgqjo.Boundary;
			if (boundary != null && 0 == 0 && ((boundary.Length != 0) ? true : false))
			{
				switch (fpqgm.Encoding)
				{
				case TransferEncoding.QuotedPrintable:
				case TransferEncoding.Base64:
					fpqgm = new ContentTransferEncoding(TransferEncoding.Binary);
					break;
				}
				if ((ejpcp & MimeOptions.DoNotParseMimeTree) == 0)
				{
					wvvtb = true;
				}
			}
		}
		else if (vgqjo.MediaType == "message/rfc822" && 0 == 0 && (ejpcp & MimeOptions.DoNotParseMimeTree) == 0)
		{
			jbrce = true;
		}
	}

	public void Load(string fileName)
	{
		if (fileName == null || 1 == 0)
		{
			throw new ArgumentNullException("fileName");
		}
		if (fileName.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("Path is empty.", "fileName");
		}
		if (fckfg && 0 == 0)
		{
			throw new MimeException("Cannot change read-only message.", MimeExceptionStatus.OperationError);
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

	public void Load(Stream input)
	{
		if (input == null || 1 == 0)
		{
			throw new ArgumentNullException("input");
		}
		if (fckfg && 0 == 0)
		{
			throw new MimeException("Cannot change read-only message.", MimeExceptionStatus.OperationError);
		}
		suirw(input, p1: false);
	}

	internal void suirw(Stream p0, bool p1)
	{
		hzqsb hzqsb = new hzqsb(this);
		hzqsb.ebnxt = (ejpcp & MimeOptions.OnlyParseHeaders) != 0;
		bool flag = (ejpcp & MimeOptions.IgnoreUnparsableHeaders) != 0;
		try
		{
			byte[] array = new byte[1024];
			while (!hzqsb.dqppr)
			{
				int num = p0.Read(array, 0, array.Length);
				if (num == 0)
				{
					break;
				}
				hzqsb.Write(array, 0, num);
			}
			hzqsb.Close();
			if ((movah.Count == 0 || 1 == 0) && (derxc.tvoem == 0 || 1 == 0) && (!hzqsb.ebnxt || false || !flag || 1 == 0))
			{
				throw new MimeException("Unable to parse mail message, no headers and no body found.", MimeExceptionStatus.MessageParserError);
			}
		}
		catch
		{
			crgxv();
			throw;
		}
		finally
		{
			if ((ejpcp & MimeOptions.DoNotCloseStreamAfterLoad) == 0)
			{
				p0.Close();
			}
		}
		if ((!hzqsb.ebnxt || 1 == 0) && (!p1 || 1 == 0))
		{
			fqpqf();
		}
	}

	public void Save(string fileName)
	{
		if (fileName == null || 1 == 0)
		{
			throw new ArgumentNullException("fileName");
		}
		Stream stream = vtdxm.namiu(fileName, qopzu);
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
		bool p = gvccg == null;
		izpsa(output, p, p2: false);
	}

	public Stream ToStream()
	{
		bool p = gvccg == null;
		return zvkll(p, p1: false);
	}

	private Stream zvkll(bool p0, bool p1)
	{
		if (vnibw() && 0 == 0)
		{
			throw new MimeException("One or more entities to be encrypted were not encrypted yet. Make sure to call the Encrypt method prior to saving or sending the message.", MimeExceptionStatus.OperationError);
		}
		if (sfgqd() && 0 == 0)
		{
			throw new MimeException("Signature for one or more signed entities is missing. Make sure to call the Sign method prior to saving or sending the message.", MimeExceptionStatus.OperationError);
		}
		return new pofnc(this, p0, p1, itjdj);
	}

	private void izpsa(Stream p0, bool p1, bool p2)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("output");
		}
		Stream stream = zvkll(p1, p2);
		try
		{
			byte[] array = new byte[1024];
			while (true)
			{
				int num = stream.Read(array, 0, array.Length);
				if (num != 0)
				{
					p0.Write(array, 0, num);
					continue;
				}
				break;
			}
		}
		finally
		{
			p0.Flush();
		}
	}

	private bool vnibw()
	{
		int num = 0;
		if (num != 0)
		{
			goto IL_0009;
		}
		goto IL_002a;
		IL_0009:
		if (zajxz[num].vnibw() && 0 == 0)
		{
			return true;
		}
		num++;
		goto IL_002a;
		IL_002a:
		if (num >= zajxz.Count)
		{
			if (laxrm != null && 0 == 0 && laxrm.vnibw() && 0 == 0)
			{
				return true;
			}
			if (iuwky != null && 0 == 0)
			{
				return !iuwky.IsEncrypted;
			}
			return false;
		}
		goto IL_0009;
	}

	private bool sfgqd()
	{
		int num = 0;
		if (num != 0)
		{
			goto IL_0009;
		}
		goto IL_002a;
		IL_0009:
		if (zajxz[num].sfgqd() && 0 == 0)
		{
			return true;
		}
		num++;
		goto IL_002a;
		IL_009c:
		int num2;
		if (num2 >= mbgbb.SignerInfos.Count)
		{
			goto IL_00b9;
		}
		goto IL_0074;
		IL_00b9:
		return false;
		IL_002a:
		if (num < zajxz.Count)
		{
			goto IL_0009;
		}
		if (laxrm != null && 0 == 0 && laxrm.sfgqd() && 0 == 0)
		{
			return true;
		}
		if (mbgbb != null && 0 == 0)
		{
			num2 = 0;
			if (num2 != 0)
			{
				goto IL_0074;
			}
			goto IL_009c;
		}
		goto IL_00b9;
		IL_0074:
		SignerInfo signerInfo = mbgbb.SignerInfos[num2];
		if (signerInfo.Signature == null || 1 == 0)
		{
			return true;
		}
		num2++;
		goto IL_009c;
	}

	private MimeException qopzu(string p0, Exception p1)
	{
		return new MimeException(p0, MimeExceptionStatus.OperationError, p1);
	}

	private static Exception jsnmd(FormatException p0)
	{
		return new MimeException("Base64 decoding error.", MimeExceptionStatus.MessageParserError, p0);
	}

	private static Exception hvkln(string p0)
	{
		return new MimeException(p0, MimeExceptionStatus.OperationError);
	}

	private static Exception drptr(string p0)
	{
		return new MimeException(p0, MimeExceptionStatus.OperationError);
	}
}
