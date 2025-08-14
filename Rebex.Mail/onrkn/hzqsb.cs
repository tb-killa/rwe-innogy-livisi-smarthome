using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Rebex;
using Rebex.Mime;
using Rebex.Mime.Headers;

namespace onrkn;

internal class hzqsb : xlbfv
{
	private enum lqbqp
	{
		wahul,
		kuqtp,
		dvinf,
		axsnk
	}

	private class jcxkg
	{
		private readonly MimeEntity kyfgt;

		private readonly string lsvdy;

		private readonly bool tipuf;

		public MimeEntity kibdt => kyfgt;

		public string rempd => lsvdy;

		public bool dudrx => tipuf;

		public jcxkg(MimeEntity entity, string boundary, bool copy)
		{
			kyfgt = entity;
			lsvdy = boundary;
			tipuf = copy;
		}
	}

	private static readonly byte[] efgkw = new byte[2] { 13, 10 };

	private static readonly byte[] wrmhl = new byte[8] { 208, 207, 17, 224, 161, 177, 26, 225 };

	private MemoryStream xybln;

	private lqbqp eojks;

	private readonly List<jcxkg> kcacj = new List<jcxkg>();

	private MimeEntity mgysk;

	private MimeEntity eypwl;

	private MimeEntity yqwrs;

	private Encoding xqeuw;

	private bool yxqbh;

	private bool tufaf;

	private bool lrubp;

	private bool bvpcn;

	private bool xzoct;

	public bool ebnxt
	{
		get
		{
			return xzoct;
		}
		set
		{
			xzoct = value;
		}
	}

	public bool dqppr => bvpcn;

	public hzqsb(MimeEntity entity)
		: base(gqmay(entity))
	{
		eojks = lqbqp.wahul;
		xybln = new MemoryStream();
		mgysk = entity;
		yqwrs = entity;
		yqwrs.crgxv();
		ugfbq();
		lrubp = true;
	}

	private static int gqmay(MimeEntity p0)
	{
		if (p0 != null && 0 == 0 && (p0.Options & MimeOptions.AllowOversizedLines) != 0)
		{
			return 16777216;
		}
		return 65536;
	}

	private void ytluo(int p0)
	{
		for (int i = p0; i < kcacj.Count; i++)
		{
			jcxkg jcxkg = kcacj[i];
			if (jcxkg.dudrx && 0 == 0)
			{
				jcxkg.kibdt.lcfpo();
			}
		}
		kcacj.RemoveRange(p0, kcacj.Count - p0);
	}

	private bool outqm(byte[] p0, int p1, int p2)
	{
		if (p2 < 3)
		{
			return false;
		}
		if (p0[p1] != 45 || p0[p1 + 1] != 45)
		{
			return false;
		}
		for (int num = kcacj.Count - 1; num >= 0; num--)
		{
			jcxkg jcxkg = kcacj[num];
			if (jcxkg.rempd != null && 0 == 0 && p2 >= jcxkg.rempd.Length + 2)
			{
				string text = EncodingTools.Default.GetString(p0, p1 + 2, jcxkg.rempd.Length);
				if (!(text != jcxkg.rempd) || 1 == 0)
				{
					if (eojks == lqbqp.wahul || 1 == 0)
					{
						jgufa(p0, 0, 0);
					}
					yrytm(p0: false);
					eypwl = jcxkg.kibdt;
					int num2 = p1 + 2 + jcxkg.rempd.Length;
					int num3 = p2 - 2 - jcxkg.rempd.Length;
					if (num3 >= 2 && p0[num2] == 45 && p0[num2 + 1] == 45)
					{
						ytluo(num);
						tdvwx(p0, p1, p2, p3: true);
						yqwrs = null;
						eojks = lqbqp.axsnk;
						yxqbh = true;
					}
					else
					{
						if (num + 1 < kcacj.Count)
						{
							ytluo(num + 1);
						}
						tdvwx(p0, p1, p2, p3: true);
						yqwrs = new MimeEntity();
						yqwrs.crgxv();
						if (eypwl.hcbrv && 0 == 0)
						{
							if (eypwl.ContentMessage == null || 1 == 0)
							{
								kcacj.Add(new jcxkg(yqwrs, null, copy: true));
								yqwrs.mhdda();
								eypwl.xebfp(yqwrs);
							}
							else if (eypwl.ylgbh == null || 1 == 0)
							{
								eypwl.phsce(yqwrs);
							}
						}
						else
						{
							yqwrs.Parent = eypwl;
						}
						yqwrs.yjlvs(eypwl);
						ugfbq();
						eojks = lqbqp.wahul;
					}
					return true;
				}
			}
		}
		return false;
	}

	private void uhtyt(byte[] p0, int p1, int p2, bool p3)
	{
		if (yxqbh && 0 == 0)
		{
			yxqbh = false;
		}
		else if (!p3 || 1 == 0)
		{
			xybln.Write(efgkw, 0, efgkw.Length);
		}
		xybln.Write(p0, p1, p2);
	}

	private void cucjg(byte[] p0, int p1, int p2, bool p3)
	{
		if (yxqbh && 0 == 0)
		{
			yxqbh = false;
		}
		else if (!p3 || 1 == 0)
		{
			xybln.Write(efgkw, 0, efgkw.Length);
		}
		xybln.Write(p0, p1, p2);
	}

	private void dwebx(byte[] p0, int p1, int p2, bool p3)
	{
		if (yxqbh && 0 == 0)
		{
			yxqbh = false;
		}
		else if (!p3 || 1 == 0)
		{
			yqwrs.ypyok(efgkw, 0, efgkw.Length);
		}
		yqwrs.ypyok(p0, p1, p2);
	}

	private void jgufa(byte[] p0, int p1, int p2)
	{
		if (p2 == 0 || 1 == 0)
		{
			if (xybln.Length > 0)
			{
				ewxml(xybln.GetBuffer(), 0, (int)xybln.Length);
				xybln.SetLength(0L);
			}
			wkrop();
			if (xzoct && 0 == 0)
			{
				bvpcn = true;
			}
			if (yqwrs.IsMultipart && 0 == 0)
			{
				eypwl = yqwrs;
				kcacj.Add(new jcxkg(eypwl, yqwrs.ContentType.Boundary, copy: false));
				yqwrs = null;
				eojks = lqbqp.kuqtp;
				yxqbh = true;
			}
			else if (yqwrs.mugxz && 0 == 0)
			{
				switch (yqwrs.ContentTransferEncoding.Encoding)
				{
				case TransferEncoding.SevenBit:
				case TransferEncoding.EightBit:
				case TransferEncoding.Binary:
					eypwl = yqwrs;
					yqwrs = new MimeMessage();
					eypwl.xebfp(yqwrs);
					yqwrs.crgxv();
					yqwrs.yjlvs(eypwl);
					ugfbq();
					eojks = lqbqp.wahul;
					break;
				default:
					yqwrs.sdfpb();
					eojks = lqbqp.dvinf;
					yxqbh = true;
					break;
				}
			}
			else
			{
				yqwrs.sdfpb();
				eojks = lqbqp.dvinf;
				yxqbh = true;
				if (yqwrs.ContentTransferEncoding.Encoding == TransferEncoding.Binary)
				{
					base.ydwoh = true;
				}
			}
		}
		else if (p0[p1] == 32 || p0[p1] == 9)
		{
			if (xybln.Length == 0)
			{
				ewxml(p0, p1, p2);
				return;
			}
			xybln.WriteByte(10);
			xybln.Write(p0, p1, p2);
		}
		else
		{
			if (xybln.Length > 0)
			{
				ewxml(xybln.GetBuffer(), 0, (int)xybln.Length);
				xybln.SetLength(0L);
			}
			if (!outqm(p0, p1, p2))
			{
				xybln.Write(p0, p1, p2);
			}
		}
	}

	protected override void iabst(byte[] p0, int p1, int p2, bool p3)
	{
		if (bvpcn && 0 == 0)
		{
			return;
		}
		int num;
		if (lrubp && 0 == 0)
		{
			lrubp = false;
			if (p2 >= wrmhl.Length)
			{
				num = 0;
				if (num != 0)
				{
					goto IL_003a;
				}
				goto IL_0062;
			}
		}
		goto IL_006c;
		IL_0062:
		if (num < wrmhl.Length)
		{
			goto IL_003a;
		}
		goto IL_006c;
		IL_006c:
		if ((!tufaf || 1 == 0) && outqm(p0, p1, p2))
		{
			return;
		}
		tdvwx(p0, p1, p2, p3);
		switch (eojks)
		{
		case lqbqp.kuqtp:
			uhtyt(p0, p1, p2, tufaf);
			break;
		case lqbqp.axsnk:
			cucjg(p0, p1, p2, tufaf);
			break;
		case lqbqp.dvinf:
			dwebx(p0, p1, p2, tufaf);
			break;
		case lqbqp.wahul:
			if (!p3 || 1 == 0)
			{
				if (p2 > base.hmokd / 2)
				{
					throw new MimeException("Unable to parse mail message, headers contains a line that is too long.", MimeExceptionStatus.MessageParserError);
				}
				if ((yqwrs.Options & MimeOptions.OnlyParseHeaders) == 0)
				{
					throw new MimeException("Unable to parse mail message, headers end with an invalid line.", MimeExceptionStatus.MessageParserError);
				}
			}
			jgufa(p0, p1, p2);
			break;
		}
		tufaf = !p3;
		return;
		IL_003a:
		if (p0[p1 + num] == wrmhl[num])
		{
			num++;
			if (num >= wrmhl.Length)
			{
				throw new MimeException("The mail message is not in MIME format, but uses Outlook .MSG format. Use MailMessage object instead.", MimeExceptionStatus.MessageParserError);
			}
			goto IL_0062;
		}
		goto IL_006c;
	}

	protected override void julnt()
	{
		base.julnt();
		yrytm(p0: true);
	}

	private void yrytm(bool p0)
	{
		switch (eojks)
		{
		case lqbqp.kuqtp:
			if (eypwl != null && 0 == 0 && (!yxqbh || false || xybln.Length > 0))
			{
				eypwl.byyjx(xybln.GetBuffer(), 0, (int)xybln.Length);
			}
			xybln.SetLength(0L);
			break;
		case lqbqp.axsnk:
			if (eypwl != null && 0 == 0 && (!yxqbh || false || xybln.Length > 0))
			{
				eypwl.wuezd(xybln.GetBuffer(), 0, (int)xybln.Length);
			}
			xybln.SetLength(0L);
			break;
		case lqbqp.dvinf:
			yqwrs.mxhog(xqeuw);
			base.ydwoh = false;
			if (!yqwrs.mugxz)
			{
				break;
			}
			switch (yqwrs.ContentTransferEncoding.Encoding)
			{
			case TransferEncoding.QuotedPrintable:
			case TransferEncoding.Base64:
			{
				MimeMessage mimeMessage = new MimeMessage();
				mimeMessage.crgxv();
				mimeMessage.yjlvs(yqwrs);
				try
				{
					Stream contentStream = yqwrs.GetContentStream();
					try
					{
						mimeMessage.suirw(contentStream, p1: true);
					}
					finally
					{
						if (contentStream != null && 0 == 0)
						{
							((IDisposable)contentStream).Dispose();
						}
					}
				}
				catch (MimeException ex)
				{
					if (ex.Status == MimeExceptionStatus.HeaderParserError || ex.Status == MimeExceptionStatus.MessageParserError)
					{
						string text = yqwrs.ContentType.Parameters["name"];
						yqwrs.ContentType = new ContentType("application/octet-stream");
						if (text != null && 0 == 0)
						{
							yqwrs.ContentType.Parameters.Add("name", text);
						}
						break;
					}
					throw;
				}
				yqwrs.SetContent(mimeMessage);
				break;
			}
			case TransferEncoding.SevenBit:
			case TransferEncoding.EightBit:
			case TransferEncoding.Binary:
				break;
			}
			break;
		case lqbqp.wahul:
			if (xybln.Length == 0)
			{
				MimeEntity parent = yqwrs.Parent;
				if (parent == null || 1 == 0)
				{
					if (!p0 || 1 == 0)
					{
						throw new MimeException("Unable to parse mail message, no body was found.", MimeExceptionStatus.MessageParserError);
					}
				}
				else
				{
					parent.Parts.Remove(yqwrs);
				}
			}
			else
			{
				ewxml(xybln.GetBuffer(), 0, (int)xybln.Length);
			}
			wkrop();
			break;
		}
	}

	private void ugfbq()
	{
		if ((mgysk.Options & (MimeOptions)4194304L) != 0)
		{
			xqeuw = mgysk.DefaultCharset;
		}
		else
		{
			xqeuw = null;
		}
		yqwrs.layrr();
	}

	private void ewxml(byte[] p0, int p1, int p2)
	{
		if ((xqeuw == null || 1 == 0) && p2 >= 3 && p0[p1] == 239 && p0[p1 + 1] == 187 && p0[p1 + 2] == 191)
		{
			xqeuw = Encoding.UTF8;
			p1 += 3;
			p2 -= 3;
		}
		yqwrs.eupxs(p0, p1, p2, xqeuw);
	}

	private void wkrop()
	{
		yqwrs.unatm(xqeuw);
	}

	private void tdvwx(byte[] p0, int p1, int p2, bool p3)
	{
		int num = 0;
		if (num != 0)
		{
			goto IL_0006;
		}
		goto IL_005f;
		IL_0006:
		jcxkg jcxkg = kcacj[num];
		if (jcxkg.dudrx && 0 == 0)
		{
			MimeEntity kibdt = jcxkg.kibdt;
			if (p2 > 0)
			{
				kibdt.xkiui(p0, p1, p2);
			}
			if (p3 && 0 == 0)
			{
				kibdt.xkiui(efgkw, 0, efgkw.Length);
			}
		}
		num++;
		goto IL_005f;
		IL_005f:
		if (num >= kcacj.Count)
		{
			return;
		}
		goto IL_0006;
	}
}
