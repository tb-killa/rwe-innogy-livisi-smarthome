using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Rebex;
using Rebex.Mail;
using Rebex.OutlookMessages;

namespace onrkn;

internal class jfxnb : IDisposable
{
	private static readonly char[] cbiun = new char[2] { '\r', '\n' };

	private static readonly char[] dvggz = ": 0123456789".ToCharArray();

	private readonly iodup jqkgu;

	private int oukgg;

	private zypui sdjjh;

	private jfxnb fodvr;

	private bool kvcsy;

	private string iuykk;

	private venkc jnqyd;

	private string nmygk;

	private string wuwuv;

	private string adsrh;

	private List<Guid> jneya;

	private Dictionary<Guid, int> gvtym;

	private vhzbi ncsut;

	private howhn fnwil;

	private zxixl rztrj;

	private jjvcf gudtu;

	private Encoding prybe;

	private Encoding wgrri;

	private Encoding bzwbo;

	private Encoding pqvir;

	internal zypui eulhi => sdjjh;

	internal int meqdt => oukgg;

	internal vhzbi ccfhn => ncsut;

	internal List<Guid> pwleq => jneya;

	internal Dictionary<Guid, int> fenpp => gvtym;

	internal Encoding bzoqu
	{
		get
		{
			return prybe;
		}
		private set
		{
			prybe = value;
		}
	}

	internal Encoding ifzxe
	{
		get
		{
			return wgrri;
		}
		private set
		{
			wgrri = value;
		}
	}

	internal Encoding haost
	{
		get
		{
			return bzwbo;
		}
		private set
		{
			bzwbo = value;
		}
	}

	internal Encoding mxgar
	{
		get
		{
			return pqvir;
		}
		private set
		{
			pqvir = value;
		}
	}

	public iodup xewra => jqkgu;

	public bool iwdcx => kvcsy;

	public jfxnb alhns => fodvr;

	public howhn suuuu => fnwil;

	public zxixl rsaru => rztrj;

	public jjvcf clqou => gudtu;

	public Encoding zxaar
	{
		get
		{
			return haost;
		}
		set
		{
			if (value == null || 1 == 0)
			{
				throw new ArgumentNullException("value", "Encoding cannot be null.");
			}
			if (kfurm && 0 == 0)
			{
				throw new InvalidOperationException("Cannot set an Encoding because the message has explicitly set the Unicode encoding.");
			}
			Encoding encoding = (mxgar = value);
			haost = encoding;
		}
	}

	public string icirf
	{
		get
		{
			if (iuykk == null || 1 == 0)
			{
				iuykk = zhcnu(MsgPropertyTag.MessageClass);
				if (iuykk == null || 1 == 0)
				{
					icirf = "IPM.Note";
				}
			}
			return iuykk;
		}
		internal set
		{
			nwwhs(MsgPropertyTag.MessageClass, value);
			iuykk = value;
		}
	}

	public bool kfurm
	{
		get
		{
			return (fnwil.unzoh<vsnwr>(MsgPropertyTag.StoreSupportMask) & vsnwr.uzflr) == vsnwr.uzflr;
		}
		set
		{
			if (value && 0 == 0)
			{
				gvyfa(MsgPropertyTag.StoreSupportMask, ansme(MsgPropertyTag.StoreSupportMask) | 0x40000);
			}
			else if (fnwil.unzoh<vsnwr>(MsgPropertyTag.StoreSupportMask) == vsnwr.uzflr)
			{
				gvyfa(MsgPropertyTag.StoreSupportMask, null);
			}
			else
			{
				gvyfa(MsgPropertyTag.StoreSupportMask, ansme(MsgPropertyTag.StoreSupportMask) & -262145);
			}
			haost = chnlh(p0: true);
			mxgar = bucji(p0: true);
		}
	}

	public bool kfaju
	{
		get
		{
			return (fnwil.unzoh<gzwtp>(MsgPropertyTag.MessageFlags) & gzwtp.fucsw) == gzwtp.fucsw;
		}
		set
		{
			if (value && 0 == 0)
			{
				gvyfa(MsgPropertyTag.MessageFlags, ansme(MsgPropertyTag.MessageFlags) | 1);
			}
			else
			{
				gvyfa(MsgPropertyTag.MessageFlags, ansme(MsgPropertyTag.MessageFlags) & -2);
			}
		}
	}

	public bool hepvh
	{
		get
		{
			return (fnwil.unzoh<gzwtp>(MsgPropertyTag.MessageFlags) & gzwtp.clycs) == gzwtp.clycs;
		}
		set
		{
			if (value && 0 == 0)
			{
				gvyfa(MsgPropertyTag.MessageFlags, ansme(MsgPropertyTag.MessageFlags) | 8);
			}
			else
			{
				gvyfa(MsgPropertyTag.MessageFlags, ansme(MsgPropertyTag.MessageFlags) & -9);
			}
		}
	}

	public string takfk
	{
		get
		{
			return zhcnu(MsgPropertyTag.InternetMessageId);
		}
		set
		{
			nwwhs(MsgPropertyTag.InternetMessageId, value);
		}
	}

	public oshbb irsrm
	{
		get
		{
			return (oshbb)llwyn(MsgPropertyTag.Priority, 0);
		}
		set
		{
			gvyfa(MsgPropertyTag.Priority, (int)value);
		}
	}

	public asfub bzlqs
	{
		get
		{
			return (asfub)llwyn(MsgPropertyTag.Importance, 1);
		}
		set
		{
			gvyfa(MsgPropertyTag.Importance, (int)value);
		}
	}

	public briwi djuqi
	{
		get
		{
			return (briwi)llwyn(MsgPropertyTag.Sensitivity, 0);
		}
		set
		{
			gvyfa(MsgPropertyTag.Sensitivity, (int)value);
		}
	}

	public DateTime? ixaxw
	{
		get
		{
			return otmmj(MsgPropertyTag.ClientSubmitTime);
		}
		set
		{
			zdbks(MsgPropertyTag.ClientSubmitTime, value);
		}
	}

	public DateTime? bkaju
	{
		get
		{
			return otmmj(MsgPropertyTag.MessageDeliveryTime);
		}
		set
		{
			zdbks(MsgPropertyTag.MessageDeliveryTime, value);
		}
	}

	public string ggdfi
	{
		get
		{
			string text = ((!xewra.yedya) ? zhcnu(MsgPropertyTag.Body) : ehdmx());
			if (text != null && 0 == 0)
			{
				return text;
			}
			if (xewra.yedya && 0 == 0)
			{
				return zhcnu(MsgPropertyTag.Body);
			}
			return ehdmx();
		}
		set
		{
			nwwhs(MsgPropertyTag.Body, value);
		}
	}

	public string ljlfy
	{
		get
		{
			if (xewra.zojzk && 0 == 0 && xroex() != null && 0 == 0)
			{
				return xroex();
			}
			string text = null;
			qacae qacae2 = fnwil[MsgPropertyTag.BodyHtml];
			if (qacae2 != null && 0 == 0)
			{
				if (dzwgu.dovit(qacae2.pzpvc) && 0 == 0)
				{
					text = (string)qacae2.tgbhs;
				}
				else if (qacae2.pzpvc == xcrar.yesjh)
				{
					byte[] array = (byte[])qacae2.tgbhs;
					text = chnlh(p0: false).GetString(array, 0, array.Length);
				}
			}
			if (text != null && 0 == 0)
			{
				return text;
			}
			if (xewra.zojzk && 0 == 0)
			{
				return null;
			}
			return xroex();
		}
		set
		{
			if (value == null || 1 == 0)
			{
				nwwhs(MsgPropertyTag.BodyHtml, value);
				return;
			}
			qacae qacae2 = fnwil[MsgPropertyTag.BodyHtml];
			if (qacae2 != null && 0 == 0)
			{
				if (qacae2.pzpvc == xcrar.yesjh)
				{
					mfrjw(MsgPropertyTag.BodyHtml, chnlh(p0: false).GetBytes(value));
					return;
				}
				if (!dzwgu.dovit(qacae2.pzpvc) || 1 == 0)
				{
					fnwil.gdqdd(MsgPropertyTag.BodyHtml);
				}
			}
			nwwhs(MsgPropertyTag.BodyHtml, value);
		}
	}

	public string pufqr
	{
		get
		{
			if (nmygk == null || 1 == 0)
			{
				byte[] array = fidhr(MsgPropertyTag.RtfCompressed);
				if (array == null || false || array.Length == 0 || 1 == 0)
				{
					return null;
				}
				array = njbal.ozsrm(array, MsgMessageException.jhhqd);
				nmygk = EncodingTools.GetEncoding("eightbit").GetString(array, 0, array.Length);
			}
			return nmygk;
		}
		set
		{
			if (nmygk == null || false || nmygk != value)
			{
				mfrjw(MsgPropertyTag.RtfCompressed, (value == null) ? null : njbal.iohjj(EncodingTools.GetEncoding("eightbit").GetBytes(value)));
				jnqyd = null;
			}
			nmygk = value;
			wuwuv = (adsrh = null);
		}
	}

	internal venkc xdokw
	{
		get
		{
			if ((jnqyd == null || 1 == 0) && pufqr != null && 0 == 0)
			{
				jnqyd = new venkc(pufqr);
			}
			return jnqyd;
		}
	}

	public string soumj
	{
		get
		{
			string text = zhcnu(MsgPropertyTag.Subject);
			if (text != null && 0 == 0)
			{
				return text;
			}
			text = zhcnu(MsgPropertyTag.NormalizedSubject);
			if (text == null || 1 == 0)
			{
				text = zhcnu(MsgPropertyTag.ConversationTopic);
			}
			if (text == null || 1 == 0)
			{
				return null;
			}
			string text2 = zhcnu(MsgPropertyTag.SubjectPrefix);
			if (text2 == null || 1 == 0)
			{
				return text;
			}
			return text2 + text;
		}
		set
		{
			nwwhs(MsgPropertyTag.Subject, value);
			nagrs(value, out var p, out var p2);
			nwwhs(MsgPropertyTag.SubjectPrefix, p);
			nwwhs(MsgPropertyTag.NormalizedSubject, p2);
			nwwhs(MsgPropertyTag.ConversationTopic, p2);
		}
	}

	public string zmmud
	{
		get
		{
			string text = zhcnu(MsgPropertyTag.ConversationTopic);
			if (text != null && 0 == 0 && text.Length != 0 && 0 == 0)
			{
				return text;
			}
			text = zhcnu(MsgPropertyTag.NormalizedSubject);
			if (text != null && 0 == 0 && text.Length != 0 && 0 == 0)
			{
				return text;
			}
			nagrs(zhcnu(MsgPropertyTag.Subject), out var _, out var p2);
			return p2;
		}
	}

	internal string gvzeg
	{
		get
		{
			string text = zhcnu(MsgPropertyTag.SenderEmailAddress);
			if (text != null && 0 == 0)
			{
				return text;
			}
			return zhcnu(MsgPropertyTag.SenderSmtpAddress);
		}
	}

	public string untou
	{
		get
		{
			string a = fnwil.vvzzv(MsgPropertyTag.SenderAddressType, "SMTP");
			string text = zhcnu(MsgPropertyTag.SenderEmailAddress);
			if (string.Equals(a, "SMTP", StringComparison.OrdinalIgnoreCase) && 0 == 0 && dzwgu.kqeon(text) && 0 == 0)
			{
				return text;
			}
			text = zhcnu(MsgPropertyTag.SenderSmtpAddress);
			if (dzwgu.kqeon(text) && 0 == 0)
			{
				return text;
			}
			return null;
		}
		set
		{
			nwwhs(MsgPropertyTag.SenderSmtpAddress, value);
			nwwhs(MsgPropertyTag.SenderEmailAddress, value);
			if (value == null || 1 == 0)
			{
				nwwhs(MsgPropertyTag.SenderAddressType, null);
			}
			else
			{
				nwwhs(MsgPropertyTag.SenderAddressType, "SMTP");
			}
			mzcqi(MsgPropertyTag.SenderEntryId);
		}
	}

	public string kazjp
	{
		get
		{
			return zhcnu(MsgPropertyTag.SenderName);
		}
		set
		{
			nwwhs(MsgPropertyTag.SenderName, value);
			mzcqi(MsgPropertyTag.SenderEntryId);
		}
	}

	internal string uyaxo => zhcnu(MsgPropertyTag.SentRepresentingEmailAddress);

	public string mzhkh
	{
		get
		{
			string a = fnwil.vvzzv(MsgPropertyTag.SentRepresentingAddressType, "SMTP");
			string text = zhcnu(MsgPropertyTag.SentRepresentingEmailAddress);
			if (string.Equals(a, "SMTP", StringComparison.OrdinalIgnoreCase) && 0 == 0 && dzwgu.kqeon(text) && 0 == 0)
			{
				return text;
			}
			return null;
		}
		set
		{
			nwwhs(MsgPropertyTag.SentRepresentingEmailAddress, value);
			if (value == null || 1 == 0)
			{
				nwwhs(MsgPropertyTag.SentRepresentingAddressType, null);
			}
			else
			{
				nwwhs(MsgPropertyTag.SentRepresentingAddressType, "SMTP");
			}
			mzcqi(MsgPropertyTag.SentRepresentingEntryId);
		}
	}

	public string ofwbk
	{
		get
		{
			return zhcnu(MsgPropertyTag.SentRepresentingName);
		}
		set
		{
			nwwhs(MsgPropertyTag.SentRepresentingName, value);
			mzcqi(MsgPropertyTag.SentRepresentingEntryId);
		}
	}

	public string kmqzx
	{
		get
		{
			return zhcnu(MsgPropertyTag.TransportMessageHeaders);
		}
		set
		{
			nwwhs(MsgPropertyTag.TransportMessageHeaders, value);
		}
	}

	internal Encoding chnlh(bool p0)
	{
		if (!p0 || false || !kfurm || 1 == 0)
		{
			return ykxeg(llwyn(MsgPropertyTag.InternetCodepage, -1));
		}
		return Encoding.Unicode;
	}

	internal Encoding bucji(bool p0)
	{
		if (!p0 || false || !kfurm || 1 == 0)
		{
			return ykxeg(llwyn(MsgPropertyTag.MessageCodepage, -1));
		}
		return Encoding.Unicode;
	}

	internal void gpgdq(int p0)
	{
		gvyfa(MsgPropertyTag.InternetCodepage, p0);
	}

	public jfxnb()
	{
		jqkgu = new iodup();
		pmioj(null);
	}

	internal jfxnb(jfxnb parentMessage)
		: this()
	{
		xcprm(parentMessage);
		sdjjh = parentMessage.eulhi;
		jqkgu = parentMessage.xewra;
		ncsut = parentMessage.ccfhn;
	}

	private void pmioj(zypui p0)
	{
		if (sdjjh != null && 0 == 0)
		{
			sdjjh.mvpfe();
		}
		oukgg++;
		sdjjh = p0;
		fodvr = null;
		kvcsy = false;
		iuykk = null;
		Encoding encoding = (mxgar = EncodingTools.Default);
		haost = encoding;
		Encoding encoding3 = (ifzxe = EncodingTools.Default);
		bzoqu = encoding3;
		nmygk = (wuwuv = (adsrh = null));
		if (jneya == null || false || jneya.Count > 0)
		{
			jneya = new List<Guid>();
		}
		if (gvtym == null || false || gvtym.Count > 0)
		{
			gvtym = new Dictionary<Guid, int>();
		}
		if (ncsut == null || false || ncsut.fnqqt > 0)
		{
			ncsut = new vhzbi(this);
		}
		if (fnwil == null || false || fnwil.fnqqt > 0)
		{
			fnwil = new howhn(this);
		}
		if (rztrj == null || false || rztrj.fnqqt > 0)
		{
			rztrj = new zxixl(this);
		}
		if (gudtu == null || false || gudtu.fnqqt > 0)
		{
			gudtu = new jjvcf(this);
		}
	}

	internal void xcprm(jfxnb p0)
	{
		if (kvcsy && 0 == 0)
		{
			if (fodvr == p0)
			{
				throw new InvalidOperationException("Message is already embedded within specified parent message.");
			}
			throw new InvalidOperationException("Message is already embedded within another MSG message.");
		}
		fodvr = p0;
		kvcsy = true;
	}

	private void nagrs(string p0, out string p1, out string p2)
	{
		bool p3 = zhcnu(MsgPropertyTag.MessageClass)?.ToUpper(CultureInfo.InvariantCulture).StartsWith("IPM.NOTE") ?? true;
		jeyzw(p3, p0, out p1, out p2);
	}

	private void jeyzw(bool p0, string p1, out string p2, out string p3)
	{
		p2 = null;
		p3 = p1;
		if (p1 == null)
		{
			return;
		}
		int num = p1.IndexOf(':');
		if (num >= 1 && (!p0 || false || num <= 3))
		{
			string text = p1.Substring(0, num);
			if (text.IndexOfAny(dvggz) < 0)
			{
				p2 = text + ": ";
				p3 = p1.Remove(0, num + 1).TrimStart();
			}
		}
	}

	private string zhcnu(MsgPropertyTag p0)
	{
		return fnwil.unzoh<string>(p0);
	}

	private void nwwhs(MsgPropertyTag p0, string p1)
	{
		fnwil.qrqdp(p0, p1);
	}

	private byte[] fidhr(MsgPropertyTag p0)
	{
		return fnwil.unzoh<byte[]>(p0);
	}

	private void mfrjw(MsgPropertyTag p0, byte[] p1)
	{
		fnwil.qrqdp(p0, p1);
	}

	private void mrifd(MsgPropertyTag p0, bool? p1)
	{
		fnwil.qrqdp(p0, p1);
	}

	private int ansme(MsgPropertyTag p0)
	{
		return fnwil.unzoh<int>(p0);
	}

	private int llwyn(MsgPropertyTag p0, int p1)
	{
		return fnwil.vvzzv(p0, p1);
	}

	private int? owvly(MsgPropertyTag p0, int? p1)
	{
		return fnwil.vvzzv(p0, p1);
	}

	private void gvyfa(MsgPropertyTag p0, int? p1)
	{
		fnwil.qrqdp(p0, p1);
	}

	private DateTime? otmmj(MsgPropertyTag p0)
	{
		return fnwil.unzoh<DateTime?>(p0);
	}

	private void zdbks(MsgPropertyTag p0, DateTime? p1)
	{
		fnwil.qrqdp(p0, p1);
	}

	private Encoding ykxeg(int p0)
	{
		if (p0 < 0)
		{
			return EncodingTools.Default;
		}
		try
		{
			return EncodingTools.GetEncoding(p0);
		}
		catch (NotSupportedException)
		{
			return EncodingTools.Default;
		}
	}

	internal void mzcqi(MsgPropertyTag p0)
	{
		switch (p0)
		{
		case MsgPropertyTag.SenderEntryId:
		{
			string p2 = gvzeg;
			object obj2 = kazjp;
			if (obj2 == null || 1 == 0)
			{
				obj2 = untou;
				if (obj2 == null || 1 == 0)
				{
					obj2 = "";
				}
			}
			mfrjw(p0, kukyb(p2, MsgPropertyTag.SenderAddressType, (string)obj2));
			break;
		}
		case MsgPropertyTag.SentRepresentingEntryId:
		{
			string p1 = uyaxo;
			object obj = ofwbk;
			if (obj == null || 1 == 0)
			{
				obj = mzhkh;
				if (obj == null || 1 == 0)
				{
					obj = "";
				}
			}
			mfrjw(p0, kukyb(p1, MsgPropertyTag.SentRepresentingAddressType, (string)obj));
			break;
		}
		default:
			throw new InvalidOperationException("Invalid EntryId tag.");
		}
	}

	private byte[] kukyb(string p0, MsgPropertyTag p1, string p2)
	{
		if (string.IsNullOrEmpty(p0) && 0 == 0)
		{
			return null;
		}
		object obj = zhcnu(p1);
		if (obj == null || 1 == 0)
		{
			obj = "SMTP";
		}
		object obj2 = p2;
		if (obj2 == null || 1 == 0)
		{
			obj2 = "";
		}
		return oabdp.xngmq(p0, (string)obj, (string)obj2);
	}

	internal string xroex()
	{
		if ((wuwuv == null || 1 == 0) && (adsrh == null || 1 == 0) && xdokw != null && 0 == 0)
		{
			wuwuv = xdokw.savrf();
		}
		return wuwuv;
	}

	internal string ehdmx()
	{
		if ((adsrh == null || 1 == 0) && (wuwuv == null || 1 == 0) && xdokw != null && 0 == 0)
		{
			adsrh = xdokw.ibwvu();
		}
		return adsrh;
	}

	public void lxdmx(string p0)
	{
		if (p0 == null || 1 == 0)
		{
			pufqr = null;
			ggdfi = null;
		}
		else
		{
			pufqr = njbal.anqsx(p0);
			ggdfi = p0;
		}
		mrifd(MsgPropertyTag.RtfInSync, true);
		adsrh = p0;
		wuwuv = null;
	}

	public void wslzq(string p0)
	{
		if (p0 == null || 1 == 0)
		{
			pufqr = null;
			ljlfy = null;
		}
		else
		{
			pufqr = njbal.rzoyw(p0, ggdfi);
			ljlfy = null;
		}
		mrifd(MsgPropertyTag.RtfInSync, true);
		wuwuv = p0;
		adsrh = null;
	}

	public qacae omxqf(string p0, MsgPropertySet p1, string p2)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("name", "Name cannot be null.");
		}
		if (p2 == null || 1 == 0)
		{
			throw new ArgumentNullException("value", "Value cannot be null.");
		}
		return fnwil.tfexu(ncsut.lvhsu(p0, p1), p2);
	}

	public qacae hkkgy(string p0, MsgPropertySet p1, string[] p2)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("name", "Name cannot be null.");
		}
		if (p2 == null || 1 == 0)
		{
			throw new ArgumentNullException("multiValue", "Value cannot be null.");
		}
		return fnwil.oomjq(ncsut.lvhsu(p0, p1), p2);
	}

	public qacae izpnm(string p0, MsgPropertySet p1, int p2)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("name", "Name cannot be null.");
		}
		return fnwil.cghwx(ncsut.lvhsu(p0, p1), p2);
	}

	public qacae rmqys(MsgPropertyId p0, MsgPropertySet p1, string p2)
	{
		if (p2 == null || 1 == 0)
		{
			throw new ArgumentNullException("value", "Value cannot be null.");
		}
		return fnwil.tfexu(ncsut.ccmvk(p0, p1), p2);
	}

	public qacae ngqus(MsgPropertyId p0, MsgPropertySet p1, int p2)
	{
		return fnwil.cghwx(ncsut.ccmvk(p0, p1), p2);
	}

	public void pcmji(Stream p0, string p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("signedMimeContent", "Stream cannot be null.");
		}
		qutug(p0, "IPM.Note.SMIME.MultipartSigned", "multipart/signed", p1, null);
	}

	public void lgpbz(Stream p0, string p1, string p2)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("encryptedMimeContent", "Stream cannot be null.");
		}
		qutug(p0, "IPM.Note.SMIME", "application/pkcs7-mime", p1, p2);
	}

	private void qutug(Stream p0, string p1, string p2, string p3, string p4)
	{
		icirf = p1;
		ljrjo ljrjo2 = new ljrjo(p3);
		jjvcf obj = gudtu;
		object obj2 = ljrjo2["name"];
		if (obj2 == null || 1 == 0)
		{
			obj2 = "smime.p7m";
		}
		wyfgf wyfgf2 = obj.kppar(p0, (string)obj2, null);
		howhn vbjpq = wyfgf2.vbjpq;
		string text = ljrjo2["media-type"];
		if (text == null || 1 == 0)
		{
			text = p2;
		}
		vbjpq.duwaw(MsgPropertyTag.AttachMimeTag, text);
		if (p3 != null && 0 == 0)
		{
			fnwil.tfexu(ncsut.lvhsu("content-type", MsgPropertySet.InternetHeaders), p3);
		}
		if (p4 != null && 0 == 0)
		{
			fnwil.tfexu(ncsut.lvhsu("content-transfer-encoding", MsgPropertySet.InternetHeaders), p4);
		}
	}

	public void azikx(string p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("filePath", "Path cannot be null.");
		}
		rqzxm(new zypui(p0));
	}

	public void nhnbp(Stream p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("fileStream", "Stream cannot be null.");
		}
		rqzxm(new zypui(p0, leaveOpen: true));
	}

	private void rqzxm(zypui p0)
	{
		if (kvcsy && 0 == 0)
		{
			throw new InvalidOperationException("Cannot perform operation on embedded message.");
		}
		pmioj(p0);
		try
		{
			grkqa();
			sxxyz("/", uxcyg.ieqrf);
			if (!jqkgu.dqdgz || 1 == 0)
			{
				sdjjh.mvpfe();
				sdjjh = null;
			}
		}
		catch
		{
			pmioj(null);
			throw;
		}
	}

	private void grkqa()
	{
		byte[] array = new byte[16];
		if (eulhi["__nameid_version1.0/__substg1.0_00020102"] != null && 0 == 0)
		{
			xxolr xxolr2 = eulhi.xbrtt("__nameid_version1.0/__substg1.0_00020102");
			try
			{
				if (xxolr2.Length % 16 != 0)
				{
					throw new MsgMessageException("Invalid length of the Guid stream.");
				}
				while (xxolr2.mdqyq(array, 0, 16) ? true : false)
				{
					Guid guid = new Guid(array);
					if (!gvtym.ContainsKey(guid) || 1 == 0)
					{
						gvtym.Add(guid, gvtym.Count);
					}
					jneya.Add(guid);
				}
			}
			finally
			{
				if (xxolr2 != null && 0 == 0)
				{
					((IDisposable)xxolr2).Dispose();
				}
			}
		}
		if (eulhi["__nameid_version1.0/__substg1.0_00030102"] == null)
		{
			return;
		}
		xxolr xxolr3 = eulhi.xbrtt("__nameid_version1.0/__substg1.0_00030102");
		try
		{
			if (xxolr3.Length % 8 != 0)
			{
				throw new MsgMessageException("Invalid length of the named property stream.");
			}
			while (xxolr3.mdqyq(array, 0, 8) ? true : false)
			{
				xrprs xrprs2 = new xrprs(this, array);
				if (xrprs2.zcrap != ncsut.fnqqt)
				{
					throw new MsgMessageException("Invalid index of the named property {0}.", xrprs2.kmaxh);
				}
				ncsut.oaesx(xrprs2);
			}
			ncsut.akmjj = false;
		}
		finally
		{
			if (xxolr3 != null && 0 == 0)
			{
				((IDisposable)xxolr3).Dispose();
			}
		}
	}

	private void sxxyz(string p0, uxcyg p1)
	{
		if (eulhi[jxtqv.motnn(p0, "__properties_version1.0")] == null || 1 == 0)
		{
			throw new MsgMessageException("MSG file doesn't contain property stream for '{0}'.", p0);
		}
		fnwil.gixkz = true;
		xxolr xxolr2 = eulhi.xbrtt(jxtqv.motnn(p0, "__properties_version1.0"));
		try
		{
			int num;
			switch (p1)
			{
			case uxcyg.ieqrf:
				num = 32;
				if (num != 0)
				{
					break;
				}
				goto case uxcyg.xjapg;
			case uxcyg.xjapg:
				num = 24;
				if (num != 0)
				{
					break;
				}
				goto case uxcyg.doiya;
			case uxcyg.doiya:
			case uxcyg.naylt:
				num = 8;
				if (num != 0)
				{
					break;
				}
				goto default;
			default:
				throw new InvalidOperationException("Unknown kind.");
			}
			byte[] array = new byte[Math.Max(num, 16)];
			int num2;
			int num3;
			if (xxolr2.Length > 0 || eulhi.gqvpk(p0).Length > 1)
			{
				if (xxolr2.Length < num)
				{
					throw new MsgMessageException("Property stream is too short.");
				}
				num2 = (int)((xxolr2.Length - num) % 16);
				if (num2 > 0)
				{
					xxolr2.Position = xxolr2.Length - num2;
					xxolr2.hxmiq(array, 0, num2);
					num3 = 0;
					if (num3 != 0)
					{
						goto IL_010e;
					}
					goto IL_012d;
				}
				goto IL_013a;
			}
			goto IL_0143;
			IL_0143:
			int num5;
			howhn howhn3;
			int num6;
			jnkze[] array2;
			int num4;
			switch (p1)
			{
			case uxcyg.ieqrf:
			case uxcyg.xjapg:
			{
				while (xxolr2.mdqyq(array, 0, 16) ? true : false)
				{
					qacae p4 = new qacae(fnwil, p0, array, readVariableLengthData: false);
					fnwil.oaesx(p4);
				}
				fnwil.akmjj = false;
				Encoding encoding = (haost = chnlh(p0: true));
				bzoqu = encoding;
				Encoding encoding3 = (mxgar = bucji(p0: true));
				ifzxe = encoding3;
				if (!jqkgu.dqdgz || 1 == 0)
				{
					num5 = 0;
					if (num5 != 0)
					{
						goto IL_01ec;
					}
					goto IL_0220;
				}
				goto IL_022f;
			}
			case uxcyg.naylt:
				howhn3 = new howhn(this, allowMultipleTags: true);
				while (xxolr2.mdqyq(array, 0, 16) ? true : false)
				{
					qacae p3 = new qacae(howhn3, p0, array, readVariableLengthData: false);
					howhn3.oaesx(p3);
				}
				howhn3.akmjj = false;
				if (!jqkgu.dqdgz || 1 == 0)
				{
					num6 = 0;
					if (num6 != 0)
					{
						goto IL_032c;
					}
					goto IL_0358;
				}
				goto IL_0363;
			case uxcyg.doiya:
			{
				howhn howhn2 = new howhn(this, allowMultipleTags: true);
				while (xxolr2.mdqyq(array, 0, 16) ? true : false)
				{
					qacae p2 = new qacae(howhn2, p0, array, !jqkgu.dqdgz);
					howhn2.oaesx(p2);
				}
				howhn2.akmjj = false;
				rztrj.oaesx(new oabdp(howhn2));
				rztrj.akmjj = false;
				break;
			}
			default:
				{
					throw new InvalidOperationException("Unknown kind.");
				}
				IL_022f:
				array2 = eulhi.gqvpk(p0);
				num4 = 0;
				if (num4 != 0)
				{
					goto IL_0247;
				}
				goto IL_02c3;
				IL_0220:
				if (num5 < fnwil.fnqqt)
				{
					goto IL_01ec;
				}
				goto IL_022f;
				IL_0247:
				if (array2[num4].msfii && 0 == 0)
				{
					if (array2[num4].dclzl.StartsWith("__recip_version1.0_#", StringComparison.OrdinalIgnoreCase) && 0 == 0)
					{
						sxxyz(array2[num4].ucwew, uxcyg.doiya);
					}
					else if (array2[num4].dclzl.StartsWith("__attach_version1.0_#", StringComparison.OrdinalIgnoreCase) && 0 == 0)
					{
						sxxyz(array2[num4].ucwew, uxcyg.naylt);
					}
				}
				num4++;
				goto IL_02c3;
				IL_01ec:
				if (!fnwil[num5].fehzf || 1 == 0)
				{
					fnwil[num5].jvnbe();
				}
				num5++;
				goto IL_0220;
				IL_02c3:
				if (num4 >= array2.Length)
				{
					break;
				}
				goto IL_0247;
				IL_0358:
				if (num6 < howhn3.fnqqt)
				{
					goto IL_032c;
				}
				goto IL_0363;
				IL_0363:
				gudtu.oaesx(new wyfgf(howhn3));
				gudtu.akmjj = false;
				break;
				IL_032c:
				if (!howhn3[num6].fehzf || 1 == 0)
				{
					howhn3[num6].jvnbe();
				}
				num6++;
				goto IL_0358;
			}
			return;
			IL_012d:
			if (num3 < num2)
			{
				goto IL_010e;
			}
			xxolr2.Position = 0L;
			goto IL_013a;
			IL_013a:
			xxolr2.hxmiq(array, 0, num);
			goto IL_0143;
			IL_010e:
			if (array[num3] != 0 && 0 == 0)
			{
				throw new MsgMessageException("Property stream has invalid length and extra bytes are non-zero.");
			}
			num3++;
			goto IL_012d;
		}
		finally
		{
			if (xxolr2 != null && 0 == 0)
			{
				((IDisposable)xxolr2).Dispose();
			}
		}
	}

	internal void wvcnp(string p0)
	{
		sxxyz(p0, uxcyg.xjapg);
	}

	public void wlbwq(string p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("filePath", "Path cannot be null.");
		}
		duqmg duqmg2 = new duqmg(p0);
		try
		{
			ivbpt(duqmg2);
		}
		finally
		{
			if (duqmg2 != null && 0 == 0)
			{
				((IDisposable)duqmg2).Dispose();
			}
		}
	}

	public void ucqrd(Stream p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("fileStream", "Stream cannot be null.");
		}
		ivbpt(new duqmg(p0, leaveOpen: true));
	}

	private void ivbpt(duqmg p0)
	{
		p0["/"].fqtok = jnkze.wiftm;
		p0.rrigt("__nameid_version1.0");
		uuipu uuipu2 = new uuipu();
		try
		{
			xnnxx(p0, uuipu2);
			lrklp(p0, uuipu2, "/", p3: false);
		}
		finally
		{
			if (uuipu2 != null && 0 == 0)
			{
				((IDisposable)uuipu2).Dispose();
			}
		}
		p0.svqqc();
	}

	private void xnnxx(duqmg p0, uuipu p1)
	{
		List<Guid> list = new List<Guid>();
		Dictionary<Guid, int> dictionary = new Dictionary<Guid, int>();
		dictionary.Add(dzwgu.sazkf[MsgPropertySet.Mapi], 1);
		dictionary.Add(dzwgu.sazkf[MsgPropertySet.PublicStrings], 2);
		int num = 0;
		if (num != 0)
		{
			goto IL_0038;
		}
		goto IL_00bb;
		IL_0038:
		if (ncsut[num].ognto != MsgPropertySet.Mapi && ncsut[num].ognto != MsgPropertySet.PublicStrings && (!dictionary.ContainsKey(ncsut[num].oaixu) || 1 == 0))
		{
			dictionary.Add(ncsut[num].oaixu, list.Count + 3);
			list.Add(ncsut[num].oaixu);
		}
		num++;
		goto IL_00bb;
		IL_0288:
		int num2;
		if (num2 < ncsut.fnqqt)
		{
			goto IL_01e4;
		}
		long fyrnh = p1.fyrnh;
		p0.unyyx("__nameid_version1.0/__substg1.0_00030102", p1.BaseStream, fyrnh);
		Dictionary<string, List<int>> dictionary2 = new Dictionary<string, List<int>>();
		int num3 = 0;
		if (num3 != 0)
		{
			goto IL_02c4;
		}
		goto IL_0388;
		IL_01e4:
		List<int> list2;
		int num4;
		if (ncsut[num2].jisdp && 0 == 0)
		{
			p1.Write((int)ncsut[num2].hnnrt);
		}
		else
		{
			p1.Write(list2[num4++]);
		}
		int num5 = (num2 << 16) | (dictionary[ncsut[num2].oaixu] << 1) | ((!ncsut[num2].jisdp || 1 == 0) ? 1 : 0);
		p1.Write(num5);
		List<int> list3;
		list3.Add(num5);
		num2++;
		goto IL_0288;
		IL_01a5:
		int num6;
		if (num6 < ncsut.fnqqt)
		{
			goto IL_012c;
		}
		fyrnh = p1.fyrnh;
		p0.unyyx("__nameid_version1.0/__substg1.0_00040102", p1.BaseStream, fyrnh);
		list3 = new List<int>();
		num2 = 0;
		num4 = 0;
		if (num4 != 0)
		{
			goto IL_01e4;
		}
		goto IL_0288;
		IL_0388:
		if (num3 < ncsut.fnqqt)
		{
			goto IL_02c4;
		}
		using (Dictionary<string, List<int>>.Enumerator enumerator = dictionary2.GetEnumerator())
		{
			while (enumerator.MoveNext() ? true : false)
			{
				KeyValuePair<string, List<int>> current = enumerator.Current;
				int num7 = 0;
				if (num7 != 0)
				{
					goto IL_03c3;
				}
				goto IL_03dd;
				IL_03dd:
				if (num7 >= current.Value.Count)
				{
					fyrnh = p1.fyrnh;
					p0.unyyx(current.Key, p1.BaseStream, fyrnh);
					continue;
				}
				goto IL_03c3;
				IL_03c3:
				p1.Write(current.Value[num7]);
				num7++;
				goto IL_03dd;
			}
			return;
		}
		IL_02c4:
		string key = jxtqv.motnn("__nameid_version1.0", ncsut[num3].xmtnb(dictionary[ncsut[num3].oaixu]));
		if (!dictionary2.TryGetValue(key, out var value) || 1 == 0)
		{
			value = new List<int>();
			dictionary2.Add(key, value);
		}
		if (ncsut[num3].jisdp && 0 == 0)
		{
			value.Add((int)ncsut[num3].hnnrt);
		}
		else
		{
			value.Add((int)ncsut[num3].ljmua);
		}
		value.Add(list3[num3]);
		num3++;
		goto IL_0388;
		IL_00bb:
		if (num < ncsut.fnqqt)
		{
			goto IL_0038;
		}
		int num8 = 0;
		if (num8 != 0)
		{
			goto IL_00d3;
		}
		goto IL_00f0;
		IL_012c:
		byte[] buffer;
		if (!ncsut[num6].jisdp || 1 == 0)
		{
			list2.Add((int)p1.fyrnh);
			p1.Write(ncsut[num6].lcvww.Length);
			p1.Write(ncsut[num6].lcvww);
			if ((p1.fyrnh & 3) != 0)
			{
				p1.Write(buffer, 0, 2);
			}
		}
		num6++;
		goto IL_01a5;
		IL_00d3:
		p1.Write(list[num8].ToByteArray());
		num8++;
		goto IL_00f0;
		IL_00f0:
		if (num8 < list.Count)
		{
			goto IL_00d3;
		}
		fyrnh = p1.fyrnh;
		p0.unyyx("__nameid_version1.0/__substg1.0_00020102", p1.BaseStream, fyrnh);
		list2 = new List<int>();
		buffer = new byte[2];
		num6 = 0;
		if (num6 != 0)
		{
			goto IL_012c;
		}
		goto IL_01a5;
	}

	private void lrklp(duqmg p0, uuipu p1, string p2, bool p3)
	{
		if (!fnwil.ptrsn(MsgPropertyTag.MessageClass) || 1 == 0)
		{
			fnwil.duwaw(MsgPropertyTag.MessageClass, "IPM.Note");
		}
		if (!kfurm || 1 == 0)
		{
			gvyfa(MsgPropertyTag.InternetCodepage, haost.CodePage);
			gvyfa(MsgPropertyTag.MessageCodepage, mxgar.CodePage);
		}
		if (gudtu.fnqqt == 0 || 1 == 0)
		{
			mrifd(MsgPropertyTag.HasAttachments, null);
			gvyfa(MsgPropertyTag.MessageFlags, ansme(MsgPropertyTag.MessageFlags) & -17);
		}
		else
		{
			mrifd(MsgPropertyTag.HasAttachments, true);
			gvyfa(MsgPropertyTag.MessageFlags, ansme(MsgPropertyTag.MessageFlags) | 0x10);
		}
		if (rztrj.akmjj && 0 == 0)
		{
			nwwhs(MsgPropertyTag.DisplayTo, rztrj.plesp(zccyb.pqplk));
			nwwhs(MsgPropertyTag.DisplayCc, rztrj.plesp(zccyb.hfokr));
			nwwhs(MsgPropertyTag.DisplayBcc, rztrj.plesp(zccyb.zgczd));
		}
		mzcfs(p0, p1, p2, fnwil, (p3 ? true : false) ? uxcyg.xjapg : uxcyg.ieqrf);
		int num = 0;
		if (num != 0)
		{
			goto IL_016c;
		}
		goto IL_01f7;
		IL_01f7:
		if (num < rztrj.fnqqt)
		{
			goto IL_016c;
		}
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_0210;
		}
		goto IL_02ca;
		IL_02ca:
		if (num2 >= gudtu.fnqqt)
		{
			return;
		}
		goto IL_0210;
		IL_016c:
		if (rztrj.akmjj && 0 == 0)
		{
			rztrj[num].plpif.qrqdp(MsgPropertyTag.RowId, num);
			rztrj[num].plpif.qrqdp(MsgPropertyTag.RecipientOrder, num);
		}
		mzcfs(p0, p1, jxtqv.motnn(p2, "__recip_version1.0_#" + num.ToString("X8")), rztrj[num].plpif, uxcyg.doiya);
		num++;
		goto IL_01f7;
		IL_0210:
		if (gudtu.akmjj && 0 == 0)
		{
			gudtu[num2].vbjpq.qrqdp(MsgPropertyTag.AttachNumber, num2);
		}
		string text = jxtqv.motnn(p2, "__attach_version1.0_#" + num2.ToString("X8"));
		mzcfs(p0, p1, text, gudtu[num2].vbjpq, uxcyg.naylt);
		if (gudtu[num2].jgqfq && 0 == 0)
		{
			gudtu[num2].atbph().lrklp(p0, p1, jxtqv.motnn(text, "__substg1.0_3701000D"), p3: true);
		}
		num2++;
		goto IL_02ca;
	}

	private void mzcfs(duqmg p0, uuipu p1, string p2, howhn p3, uxcyg p4)
	{
		byte[] array = new byte[8];
		p1.Write(array, 0, 8);
		switch (p4)
		{
		case uxcyg.ieqrf:
		case uxcyg.xjapg:
			p1.Write(BitConverter.GetBytes(rztrj.fnqqt), 0, 4);
			p1.Write(BitConverter.GetBytes(gudtu.fnqqt), 0, 4);
			p1.Write(BitConverter.GetBytes(rztrj.fnqqt), 0, 4);
			p1.Write(BitConverter.GetBytes(gudtu.fnqqt), 0, 4);
			if (p4 == uxcyg.ieqrf || 1 == 0)
			{
				p1.Write(array, 0, 8);
			}
			break;
		default:
			throw new InvalidOperationException("Unknown kind.");
		case uxcyg.doiya:
		case uxcyg.naylt:
			break;
		}
		int num = 0;
		if (num != 0)
		{
			goto IL_00bb;
		}
		goto IL_00ce;
		IL_012f:
		int num2;
		if (num2 >= p3.fnqqt)
		{
			return;
		}
		goto IL_00fc;
		IL_00ce:
		if (num < p3.fnqqt)
		{
			goto IL_00bb;
		}
		long fyrnh = p1.fyrnh;
		p0.unyyx(jxtqv.motnn(p2, "__properties_version1.0"), p1.BaseStream, fyrnh);
		num2 = 0;
		if (num2 != 0)
		{
			goto IL_00fc;
		}
		goto IL_012f;
		IL_00bb:
		p3[num].jagfb(p1, array);
		num++;
		goto IL_00ce;
		IL_00fc:
		if (!p3[num2].fehzf || 1 == 0)
		{
			p3[num2].nazyn(p0, p1, p2);
		}
		num2++;
		goto IL_012f;
	}

	protected virtual void ziuqy(bool p0)
	{
		if (p0 && 0 == 0 && sdjjh != null && 0 == 0)
		{
			sdjjh.mvpfe();
		}
	}

	public void Dispose()
	{
		ziuqy(p0: true);
		GC.SuppressFinalize(this);
	}
}
