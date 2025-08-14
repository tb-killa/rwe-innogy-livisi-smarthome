using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Rebex.Mime.Headers;
using onrkn;

namespace Rebex.Mime;

public class MimeHeader
{
	private delegate IHeader fuapt(stzvh reader);

	internal const string jfokg = "content-location";

	internal const string lxtlf = "content-description";

	internal const string nuscf = "content-disposition";

	internal const string mwjpf = "content-id";

	internal const string jioid = "content-transfer-encoding";

	internal const string poqet = "content-type";

	internal const string messj = "from";

	internal const string imokc = "to";

	internal const string aegbj = "cc";

	internal const string ufwet = "bcc";

	internal const string tbexp = "list-unsubscribe";

	internal const string egjob = "list-subscribe";

	internal const string iyuhj = "list-archive";

	internal const string hexvo = "list-help";

	internal const string qzuxv = "list-owner";

	internal const string glalu = "disposition-notification-to";

	internal const string idhjz = "in-reply-to";

	internal const string bvlpb = "references";

	internal const string udvit = "reply-to";

	internal const string dmesa = "date";

	internal const string bmhgj = "resent-date";

	internal const string quqgf = "delivery-date";

	internal const string bwson = "keywords";

	internal const string twuma = "list-post";

	internal const string licjq = "message-id";

	internal const string ururg = "received";

	internal const string bmskj = "return-path";

	internal const string qorum = "sender";

	internal const string hlunw = "subject";

	internal const string auajg = "comments";

	internal const string dlafn = "mime-version";

	internal const string bkjhm = "return-receipt-to";

	internal const string hnjxs = "x-rebex-rtf-body";

	private readonly string asbqy;

	private IHeader avesk;

	private bool vwypz;

	private string raayd;

	private byte[] senrn;

	private readonly bool vrdhj;

	private readonly bool rzomh;

	private readonly EventHandler<MimeUnparsableHeaderEventArgs> szhxj;

	private bool xpmmb;

	private static Dictionary<string, fuapt> akabl;

	public string Name => asbqy;

	public IHeader Value
	{
		get
		{
			if (avesk == null || 1 == 0)
			{
				crblq();
				lumtt(asbqy, raayd);
			}
			return avesk;
		}
	}

	public bool Unparsable => vwypz;

	public string Raw
	{
		get
		{
			if ((raayd != null || senrn != null) && (!(avesk is qmqrp qmqrp) || false || qmqrp.tzvfe == 0))
			{
				crblq();
				if ((bdgfa(asbqy) ? true : false) || asbqy.ToLower(CultureInfo.InvariantCulture) == "received")
				{
					return rllhn.jyteb(raayd);
				}
				return raayd.Replace("\n", "");
			}
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			avesk.Encode(stringWriter);
			return stringWriter.ToString();
		}
	}

	internal bool hmmsd
	{
		get
		{
			return xpmmb;
		}
		set
		{
			xpmmb = value;
			if (avesk is qmqrp qmqrp && 0 == 0)
			{
				qmqrp.tmqvd = value;
			}
		}
	}

	internal void vysda(bool p0)
	{
		if (avesk == null || 1 == 0)
		{
			lumtt(asbqy, raayd);
		}
		if (p0 && 0 == 0)
		{
			raayd = null;
		}
	}

	private void crblq()
	{
		if ((raayd == null || 1 == 0) && senrn != null && 0 == 0)
		{
			raayd = EncodingTools.Default.GetString(senrn, 0, senrn.Length).Trim();
		}
	}

	internal bool xmhkl(Encoding p0, bool p1, MimeEntity p2, EventHandler<MimeParsingHeaderEventArgs> p3)
	{
		raayd = p0.GetString(senrn, 0, senrn.Length);
		if (p3 != null && 0 == 0)
		{
			MimeParsingHeaderEventArgs e = new MimeParsingHeaderEventArgs(asbqy, raayd);
			p3(p2, e);
			string raw = e.Raw;
			if (raw == null || 1 == 0)
			{
				return false;
			}
			raayd = raw;
		}
		if (p1 && 0 == 0)
		{
			raayd = raayd.Trim();
		}
		bool flag = true;
		int num = 0;
		if (num != 0)
		{
			goto IL_0080;
		}
		goto IL_0095;
		IL_0080:
		if (senrn[num] >= 127)
		{
			flag = false;
			if (!flag)
			{
				goto IL_00a0;
			}
		}
		num++;
		goto IL_0095;
		IL_00a0:
		if (!flag || 1 == 0)
		{
			lumtt(asbqy, raayd);
			raayd = null;
		}
		senrn = null;
		return true;
		IL_0095:
		if (num < senrn.Length)
		{
			goto IL_0080;
		}
		goto IL_00a0;
	}

	public override string ToString()
	{
		return asbqy + ": " + Raw;
	}

	internal MimeHeader(string name, IHeader val, bool canonize)
	{
		if (name == null || 1 == 0)
		{
			throw new ArgumentNullException("name");
		}
		if (val == null || 1 == 0)
		{
			throw new ArgumentNullException("val");
		}
		if (canonize && 0 == 0)
		{
			asbqy = pyzxn(name);
		}
		else
		{
			if (name == null || 1 == 0)
			{
				throw new ArgumentNullException("name");
			}
			asbqy = name;
		}
		vrdhj = true;
		avesk = val;
	}

	internal MimeHeader(MimeEntity entity, string name, byte[] data)
	{
		vrdhj = (entity.Options & MimeOptions.IgnoreUnparsableHeaders) == 0;
		szhxj = entity.ftkcv();
		rzomh = true;
		asbqy = name;
		senrn = data;
	}

	public MimeHeader(string name, IHeader value)
		: this(name, value, canonize: false)
	{
	}

	private MimeHeader(MimeHeader src)
	{
		asbqy = src.asbqy;
		if (src.avesk != null && 0 == 0)
		{
			avesk = src.avesk.Clone();
		}
		vwypz = src.vwypz;
		raayd = src.raayd;
		senrn = src.senrn;
		vrdhj = src.vrdhj;
		szhxj = src.szhxj;
	}

	public MimeHeader(string name, string value)
	{
		if (name == null || 1 == 0)
		{
			throw new ArgumentNullException("name");
		}
		if (value == null || 1 == 0)
		{
			throw new ArgumentNullException("value");
		}
		asbqy = pyzxn(name);
		vrdhj = true;
		lumtt(name, value);
	}

	public MimeHeader Clone()
	{
		return new MimeHeader(this);
	}

	public void Encode(TextWriter writer)
	{
		if (writer == null || 1 == 0)
		{
			throw new ArgumentNullException("writer");
		}
		writer.Write(asbqy);
		writer.Write(": ");
		if ((raayd != null || senrn != null) && (!(avesk is qmqrp qmqrp) || false || qmqrp.tzvfe == 0))
		{
			crblq();
			writer.Write(raayd);
			writer.Write("\r\n");
		}
		else
		{
			avesk.Encode(writer);
			writer.Write("\r\n");
		}
	}

	private static bool bdgfa(string p0)
	{
		if (p0.Length < 2 || (brgjd.rbxxu(p0[0]) == 'X' && p0[1] == '-'))
		{
			return true;
		}
		return false;
	}

	static MimeHeader()
	{
		akabl = new Dictionary<string, fuapt>(StringComparer.OrdinalIgnoreCase);
		akabl.Add("bcc", MailAddressCollection.yymbm);
		akabl.Add("cc", MailAddressCollection.yymbm);
		akabl.Add("disposition-notification-to", MailAddressCollection.yymbm);
		akabl.Add("from", MailAddressCollection.yymbm);
		akabl.Add("reply-to", MailAddressCollection.yymbm);
		akabl.Add("sender", MailAddress.kemeh);
		akabl.Add("to", MailAddressCollection.yymbm);
		akabl.Add("resent-from", MailAddressCollection.yymbm);
		akabl.Add("resent-sender", MailAddress.kemeh);
		akabl.Add("resent-to", MailAddressCollection.yymbm);
		akabl.Add("resent-cc", MailAddressCollection.yymbm);
		akabl.Add("resent-bcc", MailAddressCollection.yymbm);
		akabl.Add("resent-reply-to", MailAddressCollection.yymbm);
		akabl.Add("resent-message-id", MessageId.sarvi);
		akabl.Add("resent-date", MailDateTime.aohzm);
		akabl.Add("delivery-date", MailDateTime.aohzm);
		akabl.Add("date", MailDateTime.aohzm);
		akabl.Add("mime-version", MimeVersion.brxqb);
		akabl.Add("keywords", PhraseCollection.baxan);
		akabl.Add("subject", Unstructured.seqfp);
		akabl.Add("comments", Unstructured.seqfp);
		akabl.Add("in-reply-to", MessageIdCollection.iogdx);
		akabl.Add("list-archive", ListCommandUrlCollection.fjbra);
		akabl.Add("list-help", ListCommandUrlCollection.fjbra);
		akabl.Add("list-owner", ListCommandUrlCollection.fjbra);
		akabl.Add("list-post", ListCommandUrlCollection.fjbra);
		akabl.Add("list-subscribe", ListCommandUrlCollection.fjbra);
		akabl.Add("list-unsubscribe", ListCommandUrlCollection.fjbra);
		akabl.Add("message-id", MessageId.sarvi);
		akabl.Add("received", Received.fyzvv);
		akabl.Add("references", MessageIdCollection.iogdx);
		akabl.Add("return-path", ReturnPath.ymslu);
		akabl.Add("content-location", ContentLocation.yibms);
		akabl.Add("content-description", Unstructured.seqfp);
		akabl.Add("content-disposition", ContentDisposition.ldvap);
		akabl.Add("content-id", MessageId.sarvi);
		akabl.Add("content-transfer-encoding", ContentTransferEncoding.heqkw);
		akabl.Add("content-type", ContentType.tcamm);
		akabl.Add("x-rebex-rtf-body", Unparsed.kkzpk);
	}

	private void lumtt(string p0, string p1)
	{
		stzvh stzvh = new stzvh(p1);
		try
		{
			fuapt value;
			lock (akabl)
			{
				akabl.TryGetValue(p0, out value);
			}
			avesk = null;
			if (value != null && 0 == 0)
			{
				avesk = value(stzvh);
			}
			else if (bdgfa(p0) && 0 == 0)
			{
				avesk = Unparsed.kkzpk(stzvh);
			}
			else
			{
				avesk = Unstructured.seqfp(stzvh);
			}
		}
		catch (MimeException ex)
		{
			if (ex.Status != MimeExceptionStatus.HeaderParserError)
			{
				throw;
			}
			string text;
			if (rzomh && 0 == 0 && (text = p0.ToLower(CultureInfo.InvariantCulture)) != null && 0 == 0 && (text == "date" || text == "resent-date" || text == "delivery-date"))
			{
				avesk = Unparsed.kkzpk(stzvh);
				return;
			}
			bool flag = !vrdhj;
			IHeader header = null;
			if (szhxj != null && 0 == 0)
			{
				MimeUnparsableHeaderEventArgs e = new MimeUnparsableHeaderEventArgs(p0, p1, ex, MimeUnparsableHeaderSeverity.Error, MimeUnparsableHeaderStatus.UnableToParse);
				e.Ignore = flag;
				szhxj(this, e);
				flag = e.Ignore;
				header = e.wfcxz;
			}
			if (!flag || 1 == 0)
			{
				ex.dtyub(" in header '" + p0 + "'.");
				throw;
			}
			if (header != null && 0 == 0)
			{
				vwypz = false;
				avesk = header;
			}
			else
			{
				vwypz = true;
				string text2;
				if ((text2 = p0.ToLower(CultureInfo.InvariantCulture)) == null)
				{
					goto IL_0212;
				}
				if (!(text2 == "content-type") || 1 == 0)
				{
					if (!(text2 == "content-disposition") || 1 == 0)
					{
						goto IL_0212;
					}
					avesk = new ContentDisposition("attachment");
				}
				else
				{
					avesk = new ContentType("application/octet-stream");
				}
			}
			goto end_IL_0084;
			IL_0212:
			avesk = Unparsed.kkzpk(stzvh);
			end_IL_0084:;
		}
		if (szhxj != null && 0 == 0 && avesk is ContentType contentType && 0 == 0 && contentType.CharSet != null && 0 == 0 && (contentType.Encoding == null || 1 == 0))
		{
			MimeUnparsableHeaderEventArgs e2 = new MimeUnparsableHeaderEventArgs(p0, p1, new MimeException("Unknown charset.", MimeExceptionStatus.HeaderParserError), MimeUnparsableHeaderSeverity.Warning, MimeUnparsableHeaderStatus.UnknownCharset);
			e2.Ignore = true;
			szhxj(this, e2);
			if (e2.wfcxz != null && 0 == 0)
			{
				avesk = e2.wfcxz;
			}
		}
		if (xpmmb && 0 == 0 && avesk is qmqrp qmqrp && 0 == 0)
		{
			qmqrp.tmqvd = true;
		}
	}

	private static string pyzxn(string p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("headerName");
		}
		if (p0.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("Empty header name.", p0);
		}
		wcxxf.ibmcb(p0, p1: true);
		lock (akabl)
		{
			if (!akabl.ContainsKey(p0) || 1 == 0)
			{
				return p0;
			}
		}
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = true;
		int num = 0;
		if (num != 0)
		{
			goto IL_007e;
		}
		goto IL_00b0;
		IL_007e:
		char c = p0[num];
		stringBuilder.Append((flag ? true : false) ? brgjd.rbxxu(c) : brgjd.ewjli(c));
		flag = c == '-';
		num++;
		goto IL_00b0;
		IL_00b0:
		if (num >= p0.Length)
		{
			p0 = stringBuilder.ToString();
			if (p0.StartsWith("Mime-") && 0 == 0)
			{
				return "MIME" + p0.Substring(4);
			}
			if (p0.EndsWith("-Id") && 0 == 0)
			{
				return p0.Substring(0, p0.Length - 2) + "ID";
			}
			return p0;
		}
		goto IL_007e;
	}

	public static string DecodeMimeHeader(string value)
	{
		if (value == null || 1 == 0)
		{
			throw new ArgumentNullException("value");
		}
		return kgbvh.ttsbq(value);
	}
}
