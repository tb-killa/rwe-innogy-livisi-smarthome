using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Text;
using Rebex;
using Rebex.Mail;
using Rebex.Mime;
using Rebex.Mime.Headers;
using Rebex.Security.Cryptography;

namespace onrkn;

internal class wvkad
{
	private class uebju
	{
		public string nvnqn;

		public string rnwnf;

		public MemoryStream qakeq;

		public MailMessage epdhw;

		public string cunmt;

		public string sujfd;

		public string ooufd;
	}

	private const int ovalx = 574529400;

	private const int tfbzv = 1;

	private const int xncry = 2;

	private const int uhnxx = 65535;

	private const int shqsd = 0;

	private const int ieakq = 1;

	private const int syqlz = 2;

	private const int dxuai = 3;

	private const int fhacm = 4;

	private const int jolcg = 5;

	private const int ihbfy = 6;

	private const int ijcwh = 7;

	private const int zwzlq = 8;

	private const int yknvt = 9;

	private const int ljvbh = 0;

	private const int cgbmv = 32768;

	private const int kvgkk = 98308;

	private const int yeqli = 229381;

	private const int xzctl = 229382;

	private const int rgdtb = 425991;

	private const int mrqfn = 491528;

	private const int vdzer = 98313;

	private const int hnofx = 98314;

	private const int qmlaq = 98315;

	private const int izbga = 163852;

	private const int olwxu = 294925;

	private const int hfyeb = 425999;

	private const int ryujn = 98320;

	private const int zfwxg = 426001;

	private const int yyzhn = 229394;

	private const int yelav = 229395;

	private const int zkfnk = 229408;

	private const int wyras = 430081;

	private const int gsote = 430082;

	private const int ycios = 430083;

	private const int yegsb = 430084;

	private const int ntafj = 430085;

	private const int xnmua = 561158;

	private const int ddzst = 430087;

	private const int pgnaf = 458758;

	private const int huyle = 393216;

	private const int ppmuo = 393217;

	private const int vxakz = 393218;

	private const int ldhfg = 196614;

	private const int pixaa = 196615;

	private const int vlcpn = 327688;

	private const int pbzgl = 262153;

	private const int ytzgd = 0;

	private const int rxqos = 1;

	private const int ptvqy = 10;

	private const int cmqkd = 4096;

	private const int xcdpw = 2;

	private const int xkfpq = 3;

	private const int wtoby = 4;

	private const int omnng = 5;

	private const int svddi = 6;

	private const int edpim = 7;

	private const int pgslm = 64;

	private const int nvwvz = 72;

	private const int riovo = 20;

	private const int yutbp = 11;

	private const int fzjzt = 13;

	private const int frbzh = 30;

	private const int jlkmf = 31;

	private const int mjkcb = 258;

	private readonly MailMessage kpmjh;

	private readonly bool xqsal;

	private string ngatd;

	private Encoding czfbw;

	private uebju uwixc;

	private readonly ArrayList hfqpk;

	private bool uhuyf;

	private static readonly char[] xecfj = new char[2] { '\\', '/' };

	public static bool wcrle(MailMessage p0)
	{
		if (p0.Attachments.Count != 1)
		{
			return false;
		}
		Attachment attachment = p0.Attachments[0];
		if (attachment.ContentType.MediaType != "application/ms-tnef" && 0 == 0)
		{
			return false;
		}
		Stream contentStream = attachment.GetContentStream();
		bool flag;
		try
		{
			wvkad wvkad2 = new wvkad(p0, ignoreHeaders: true);
			flag = wvkad2.lvqgm(contentStream);
		}
		finally
		{
			if (contentStream != null && 0 == 0)
			{
				((IDisposable)contentStream).Dispose();
			}
		}
		if (flag && 0 == 0 && p0.Attachments.Count > 0)
		{
			p0.Attachments.RemoveAt(0);
		}
		return true;
	}

	private wvkad(MailMessage message, bool ignoreHeaders)
	{
		kpmjh = message;
		xqsal = ignoreHeaders;
		ngatd = "";
		czfbw = EncodingTools.Default;
		hfqpk = new ArrayList();
	}

	private bool lvqgm(Stream p0)
	{
		BinaryReader binaryReader = new BinaryReader(p0, EncodingTools.ASCII);
		if (p0.Length == 0)
		{
			return true;
		}
		if (p0.Length < 6 || 574529400 != binaryReader.ReadInt32())
		{
			if (!kpmjh.Settings.IgnoreInvalidTnefMessages || 1 == 0)
			{
				throw new MailException("Invalid TNEF message.");
			}
			return true;
		}
		binaryReader.ReadInt16();
		bool flag = false;
		if (flag)
		{
			goto IL_0068;
		}
		goto IL_00fc;
		IL_0068:
		switch (binaryReader.ReadByte())
		{
		case 1:
			laexm(binaryReader);
			break;
		case 2:
			try
			{
				auvem(binaryReader);
			}
			catch (EndOfStreamException)
			{
				if (!kpmjh.Settings.IgnoreInvalidTnefMessages || 1 == 0)
				{
					throw new MailException("Invalid TNEF message (not enough data of attachment part).");
				}
				flag = true;
			}
			break;
		case 10:
		case 13:
			flag = true;
			if (flag)
			{
				break;
			}
			goto default;
		default:
			if (!kpmjh.Settings.IgnoreInvalidTnefMessages || 1 == 0)
			{
				throw new MailException("Invalid TNEF message part.");
			}
			return true;
		}
		goto IL_00fc;
		IL_00fc:
		if ((flag ? true : false) || binaryReader.PeekChar() < 0)
		{
			int num = 0;
			IEnumerator enumerator = hfqpk.GetEnumerator();
			try
			{
				while (enumerator.MoveNext() ? true : false)
				{
					uebju uebju = (uebju)enumerator.Current;
					string text = ((uebju.rnwnf != null && 0 == 0) ? uebju.rnwnf : ((uebju.nvnqn == null) ? null : uebju.nvnqn));
					if (text != null && 0 == 0)
					{
						int num2 = brgjd.pkosy(text, xecfj);
						if (num2 >= 0)
						{
							text = text.Substring(num2 + 1);
						}
					}
					string text2 = null;
					Attachment attachment = null;
					if (uebju.qakeq != null && 0 == 0)
					{
						if (uebju.ooufd == "multipart/signed" && 0 == 0)
						{
							MimeEntity mimeEntity = new MimeEntity();
							mimeEntity.Load(uebju.qakeq);
							kpmjh.AlternateViews.wzxkg.Clear();
							kpmjh.Resources.pusbf.Clear();
							kpmjh.Attachments.cmbhn.Clear();
							kpmjh.okbpw(mimeEntity);
							return false;
						}
						if (text == null || 1 == 0)
						{
							text = "att";
						}
						attachment = new Attachment();
						attachment.Options = MimeOptions.AllowAnyTextCharacters;
						string text3 = uebju.ooufd;
						if (text3 == null || false || text3.Length == 0 || 1 == 0)
						{
							string cunmt;
							if ((cunmt = uebju.cunmt) != null && 0 == 0 && cunmt == "Picture (Device Independent Bitmap)")
							{
								text += ".bmp";
								text3 = "image/bmp";
							}
							else
							{
								text3 = "application/octet-stream";
							}
						}
						text = AttachmentBase.jficx(text, '-');
						if (!ContentType.vgubl(text3) || 1 == 0)
						{
							text3 = "application/octet-stream";
						}
						attachment.SetContent(uebju.qakeq, text, text3);
						text2 = uebju.sujfd;
						if (text2 != null && 0 == 0)
						{
							attachment.ContentId = new MessageId(text2);
						}
					}
					if (uebju.epdhw != null && 0 == 0)
					{
						uebju.epdhw.phnpj(p0: true);
						attachment = new Attachment(uebju.epdhw);
					}
					if (attachment != null && 0 == 0)
					{
						string headerValue = brgjd.edcru("embedded-rtf-attachment-id{0}@rebex.net", num++);
						attachment.jyvko.Headers.Add("X-Content-ID", headerValue);
						if (text2 != null && 0 == 0 && ngatd.IndexOf(brgjd.edcru("src=\"cid:{0}\"", text2), StringComparison.OrdinalIgnoreCase) >= 0)
						{
							kpmjh.nkkmz(attachment, text2);
						}
						else
						{
							kpmjh.Attachments.Add(attachment);
						}
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
			return true;
		}
		goto IL_0068;
	}

	private void laexm(BinaryReader p0)
	{
		xhllr(p0, out var p1, out var p2);
		byte[] array = p2 as byte[];
		switch (p1)
		{
		case 430087:
			if (array.Length >= 4)
			{
				int codepage = BitConverter.ToInt32(array, 0);
				czfbw = EncodingTools.GetEncoding(codepage);
			}
			break;
		case 430083:
		{
			BinaryReader binaryReader = new BinaryReader(new MemoryStream(array, 0, array.Length, writable: false, publiclyVisible: false));
			try
			{
				gjvpw(binaryReader, p1: false);
				break;
			}
			finally
			{
				binaryReader.Close();
			}
		}
		}
	}

	private void auvem(BinaryReader p0)
	{
		xhllr(p0, out var p1, out var p2);
		byte[] array = p2 as byte[];
		switch (p1)
		{
		case 430082:
			uwixc = new uebju();
			hfqpk.Add(uwixc);
			break;
		case 98320:
			if (uwixc != null && 0 == 0 && p2 is string && 0 == 0)
			{
				uebju uebju = uwixc;
				string obj = (string)p2;
				char[] trimChars = new char[1];
				uebju.nvnqn = obj.TrimEnd(trimChars);
			}
			break;
		case 425999:
			if (uwixc != null && 0 == 0 && array != null && 0 == 0)
			{
				uwixc.qakeq = new MemoryStream(array, 0, array.Length, writable: false, publiclyVisible: false);
			}
			break;
		case 430085:
			if (uwixc != null && 0 == 0 && array != null && 0 == 0)
			{
				BinaryReader binaryReader = new BinaryReader(new MemoryStream(array, 0, array.Length, writable: false, publiclyVisible: false));
				try
				{
					gjvpw(binaryReader, p1: true);
					break;
				}
				finally
				{
					binaryReader.Close();
				}
			}
			break;
		}
	}

	private void xhllr(BinaryReader p0, out int p1, out object p2)
	{
		p1 = p0.ReadInt32();
		int count = p0.ReadInt32();
		byte[] array = p0.ReadBytes(count);
		p0.ReadInt16();
		switch (p1)
		{
		default:
			p1 = 0;
			p2 = null;
			break;
		case 32768:
		case 98308:
		case 98313:
		case 98314:
		case 98315:
		case 98320:
		case 163852:
		case 196614:
		case 196615:
		case 229381:
		case 229382:
		case 229394:
		case 229395:
		case 229408:
		case 262153:
		case 294925:
		case 327688:
		case 393216:
		case 393217:
		case 393218:
		case 425991:
		case 425999:
		case 426001:
		case 430081:
		case 430082:
		case 430083:
		case 430084:
		case 430085:
		case 430087:
		case 458758:
		case 491528:
		case 561158:
			switch (p1 >> 16)
			{
			case 6:
			case 7:
			case 8:
				p2 = array;
				break;
			case 1:
			{
				string text = czfbw.GetString(array, 0, array.Length);
				char[] trimChars = new char[1];
				p2 = text.TrimEnd(trimChars);
				break;
			}
			case 3:
				p2 = null;
				break;
			default:
				p2 = null;
				break;
			}
			break;
		}
	}

	private void gjvpw(BinaryReader p0, bool p1)
	{
		int num = p0.ReadInt32();
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_0013;
		}
		goto IL_06ed;
		IL_0013:
		int num3 = p0.ReadInt16();
		int num4 = p0.ReadUInt16();
		xbbxb xbbxb2;
		if (num4 > 32767)
		{
			xbbxb2 = (xbbxb)0;
			new Guid(p0.ReadBytes(16));
			int num5 = p0.ReadInt32();
			if (num5 == 0 || 1 == 0)
			{
				p0.ReadInt32().ToString();
			}
			else
			{
				if (num5 != 1)
				{
					throw new MailException("Unknown TNEF type.");
				}
				int i = p0.ReadInt32();
				byte[] array = p0.ReadBytes(i);
				for (; (i & 3) != 0; i++)
				{
					if (1 == 0)
					{
						break;
					}
					p0.ReadByte();
				}
				Encoding.Unicode.GetString(array, 0, array.Length);
			}
		}
		else
		{
			xbbxb2 = (xbbxb)num4;
		}
		bool flag = (num3 & 0x1000) == 4096;
		num3 &= -4097;
		object obj;
		switch (num3)
		{
		case 11:
			if (flag && 0 == 0)
			{
				throw new MailException("Invalid TNEF multi value.");
			}
			goto case 2;
		case 2:
		case 3:
		case 4:
		case 5:
		case 6:
		case 7:
		case 20:
		case 64:
		case 72:
			obj = qlrhq(p0, num3, flag, p3: true);
			break;
		case 13:
			if (flag && 0 == 0)
			{
				throw new MailException("Invalid TNEF multi value.");
			}
			goto case 30;
		case 30:
		case 31:
		case 258:
			obj = qlrhq(p0, num3, flag, p3: false);
			break;
		default:
			throw new MailException("Unknown TNEF type.");
		}
		if (p1 && 0 == 0)
		{
			if (uwixc != null && 0 == 0)
			{
				switch (xbbxb2)
				{
				case xbbxb.qmngt:
					uwixc.sujfd = obj as string;
					break;
				case xbbxb.uhqgu:
					uwixc.ooufd = obj as string;
					break;
				case xbbxb.vjfkm:
					uwixc.cunmt = obj as string;
					break;
				case xbbxb.osshe:
					uwixc.rnwnf = obj as string;
					break;
				case xbbxb.fcuav:
					if (num3 == 13 && obj is byte[] array2 && 0 == 0)
					{
						bool p2;
						MemoryStream memoryStream = qqhnd.zmjzm(array2, out p2);
						if (memoryStream == null || 1 == 0)
						{
							throw new MailException("Unknown TNEF embedded object.");
						}
						if (!p2 || 1 == 0)
						{
							uwixc.qakeq = memoryStream;
							break;
						}
						uwixc.qakeq = null;
						uwixc.epdhw = new MailMessage();
						byte[] array3 = HashingAlgorithm.ComputeHash(HashingAlgorithmId.SHA1, array2);
						string messageId = BitConverter.ToString(array3, 0, 16).Replace("-", "") + "@tnef";
						uwixc.epdhw.MessageId = new MessageId(messageId);
						wvkad wvkad2 = new wvkad(uwixc.epdhw, ignoreHeaders: false);
						wvkad2.lvqgm(memoryStream);
					}
					break;
				}
			}
		}
		else
		{
			byte[] array4 = null;
			switch (xbbxb2)
			{
			case xbbxb.fduka:
			{
				array4 = (byte[])obj;
				BinaryReader binaryReader = new BinaryReader(new MemoryStream(array4, 0, array4.Length, writable: false, publiclyVisible: false));
				try
				{
					array4 = njbal.dfgxm(binaryReader, pieou.shemm);
				}
				finally
				{
					binaryReader.Close();
				}
				Encoding encoding = EncodingTools.GetEncoding("eightbit");
				venkc venkc2 = new venkc(encoding.GetString(array4, 0, array4.Length));
				bool whhll = venkc2.whhll;
				bool flag2 = kpmjh.Settings.bxjim(whhll);
				if ((whhll ? true : false) || flag2)
				{
					if (flag2 && 0 == 0)
					{
						string name = AttachmentBase.boiem(kpmjh.Subject);
						Attachment attachment = new Attachment();
						attachment.jyvko.Options |= MimeOptions.AllowAnyTextCharacters;
						attachment.SetContent(new MemoryStream(array4, writable: false), name, "application/rtf");
						attachment.jyvko.Headers.Add("x-rebex-rtf-body", "1");
						kpmjh.Attachments.Add(attachment);
					}
					else
					{
						AlternateView alternateView = new AlternateView();
						alternateView.SetContent(new MemoryStream(array4, writable: false), "application/rtf");
						kpmjh.AlternateViews.Add(alternateView);
					}
				}
				else
				{
					hvbst(array4, "rtf");
				}
				break;
			}
			case xbbxb.izhdx:
				array4 = (byte[])obj;
				hvbst(array4, "html");
				break;
			}
			if ((!xqsal || 1 == 0) && (!uhuyf || 1 == 0))
			{
				string text = obj as string;
				if (text == null || 1 == 0)
				{
					text = "";
				}
				switch (xbbxb2)
				{
				case xbbxb.pdgnz:
					kpmjh.Subject = text;
					break;
				case xbbxb.ydbxt:
					if (obj is string && 0 == 0)
					{
						drtia(text);
					}
					break;
				case xbbxb.vxmey:
					if (kpmjh.From.Count > 0)
					{
						kpmjh.From[0] = new MailAddress(kpmjh.From[0].Address, text);
					}
					else
					{
						kpmjh.From = new MailAddress(null, text);
					}
					break;
				case xbbxb.fvopr:
					if (kpmjh.From.Count > 0)
					{
						kpmjh.From[0] = new MailAddress(text, kpmjh.From[0].DisplayName);
					}
					else
					{
						kpmjh.From = new MailAddress(text);
					}
					break;
				case xbbxb.emffb:
					kpmjh.To = text;
					break;
				case xbbxb.ekkep:
					kpmjh.CC = text;
					break;
				case xbbxb.uuriw:
					kpmjh.Bcc = text;
					break;
				case xbbxb.zkyiw:
					if (obj is long && 0 == 0)
					{
						kpmjh.Date = new MailDateTime(DateTime.FromFileTime((long)obj));
					}
					break;
				}
			}
		}
		num2++;
		goto IL_06ed;
		IL_06ed:
		if (num2 >= num)
		{
			return;
		}
		goto IL_0013;
	}

	private void drtia(string p0)
	{
		if (uhuyf)
		{
			return;
		}
		MimeEntity mimeEntity = new MimeEntity();
		mimeEntity.Options |= MimeOptions.OnlyParseHeaders;
		byte[] bytes = czfbw.GetBytes(p0);
		MemoryStream input = new MemoryStream(bytes, 0, bytes.Length, writable: false, publiclyVisible: false);
		mimeEntity.Load(input);
		kpmjh.Headers.Clear();
		IEnumerator enumerator = mimeEntity.Headers.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				MimeHeader mimeHeader = (MimeHeader)enumerator.Current;
				string text = mimeHeader.Name.ToLower(CultureInfo.InvariantCulture);
				if (!text.StartsWith("content-") || 1 == 0)
				{
					kpmjh.Headers.Add(mimeHeader);
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
		uhuyf = true;
	}

	private void hvbst(byte[] p0, string p1)
	{
		if (p0 == null)
		{
			return;
		}
		string text = czfbw.GetString(p0, 0, p0.Length);
		char[] trimChars = new char[1];
		string text2 = text.TrimEnd(trimChars);
		if (p1 == "html" && 0 == 0)
		{
			int num = text2.IndexOf("</head>", StringComparison.OrdinalIgnoreCase);
			if (num > 0)
			{
				int num2 = text2.LastIndexOf("content=\"text/html; charset=utf-8\"", num, StringComparison.OrdinalIgnoreCase);
				if (num2 >= 0)
				{
					string text3 = Encoding.UTF8.GetString(p0, 0, p0.Length);
					char[] trimChars2 = new char[1];
					text2 = text3.TrimEnd(trimChars2);
				}
			}
		}
		AlternateView alternateView = new AlternateView();
		alternateView.Options = MimeOptions.AllowAnyTextCharacters;
		alternateView.SetContent(text2, "text/" + p1);
		kpmjh.AlternateViews.Add(alternateView);
		ngatd += text2;
	}

	private object qlrhq(BinaryReader p0, int p1, bool p2, bool p3)
	{
		int num;
		if ((!p2 || 1 == 0) && p3 && 0 == 0)
		{
			num = 1;
			if (num != 0)
			{
				goto IL_0026;
			}
		}
		num = p0.ReadInt32();
		goto IL_0026;
		IL_0026:
		object[] array = new object[num];
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_0035;
		}
		goto IL_026a;
		IL_026a:
		if (num2 >= num)
		{
			if (!p2 || 1 == 0)
			{
				return array[0];
			}
			return array;
		}
		goto IL_0035;
		IL_0035:
		int num3 = 0;
		byte[] array2 = null;
		object obj;
		switch (p1)
		{
		case 11:
			obj = p0.ReadInt16() != 0;
			num3 += 2;
			break;
		case 2:
			obj = p0.ReadInt16();
			num3 += 2;
			break;
		case 3:
			obj = p0.ReadInt32();
			num3 += 4;
			break;
		case 4:
			obj = p0.ReadSingle();
			num3 += 4;
			break;
		case 5:
			obj = p0.ReadDouble();
			num3 += 8;
			break;
		case 6:
			obj = p0.ReadInt64();
			num3 += 8;
			break;
		case 7:
			throw new MailException("TNEF OLE DATE not supported yet.");
		case 64:
			obj = p0.ReadInt64();
			num3 += 8;
			break;
		case 72:
			obj = new Guid(p0.ReadBytes(16));
			num3 += 16;
			break;
		case 20:
			obj = p0.ReadInt64();
			num3 += 8;
			break;
		case 13:
		case 30:
		case 31:
		case 258:
		{
			int num4 = p0.ReadInt32();
			num3 += 4;
			array2 = p0.ReadBytes(num4);
			num3 += num4;
			obj = array2;
			break;
		}
		default:
			throw new MailException("Unknown TNEF type.");
		}
		switch (p1)
		{
		case 30:
		{
			string text = czfbw.GetString(array2, 0, array2.Length);
			string text3 = text;
			char[] trimChars2 = new char[1];
			text = text3.TrimEnd(trimChars2);
			obj = text;
			break;
		}
		case 31:
		{
			string text = Encoding.Unicode.GetString(array2, 0, array2.Length);
			string text2 = text;
			char[] trimChars = new char[1];
			text = text2.TrimEnd(trimChars);
			obj = text;
			break;
		}
		case 13:
		case 258:
			obj = array2;
			break;
		}
		array[num2] = obj;
		for (; (num3 & 3) != 0; num3++)
		{
			if (1 == 0)
			{
				break;
			}
			p0.ReadByte();
		}
		num2++;
		goto IL_026a;
	}
}
