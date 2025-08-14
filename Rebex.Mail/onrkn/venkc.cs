using System;
using System.Collections.Generic;
using System.Text;
using Rebex;
using Rebex.Mail;

namespace onrkn;

internal class venkc
{
	private enum kcbfa
	{
		gfuhx,
		plabx
	}

	private const int lttak = 100;

	private kcbfa ohamw;

	private sbnrz naagl;

	private StringBuilder folpl = new StringBuilder();

	private Dictionary<string, Decoder> asvbo = new Dictionary<string, Decoder>();

	private bool jjdud;

	private byte[] eezlx = new byte[1];

	private char[] fjvjc = new char[4];

	private bool nmgql;

	private bool yztax;

	private bool khrjm;

	private bool bzitu;

	public sbnrz gttba => naagl;

	public bool veelr
	{
		get
		{
			return nmgql;
		}
		private set
		{
			nmgql = value;
		}
	}

	public bool rpqss
	{
		get
		{
			return yztax;
		}
		private set
		{
			yztax = value;
		}
	}

	public bool whhll
	{
		get
		{
			return khrjm;
		}
		private set
		{
			khrjm = value;
		}
	}

	public bool pgqva
	{
		get
		{
			return bzitu;
		}
		private set
		{
			bzitu = value;
		}
	}

	private void khcgl()
	{
		pgqva = true;
		ptirx(naagl);
	}

	public venkc(string text)
	{
		int num = 0;
		naagl = new sbnrz(null);
		naagl.efcpq(1);
		naagl.fpzoh(EncodingTools.Default.GetDecoder());
		sbnrz sbnrz2 = naagl;
		int length = text.Length;
		int i = 0;
		if (i != 0)
		{
			goto IL_0079;
		}
		goto IL_0525;
		IL_0079:
		char c = text[i];
		switch (ohamw)
		{
		case kcbfa.gfuhx:
			if (!npbqc(c) || 1 == 0)
			{
				if (c > '\u007f')
				{
					xtdhy(sbnrz2, ajuaj.mbsvn, ref i, length);
					folpl.Append(c);
					xtdhy(sbnrz2, ajuaj.exzyo, ref i, length);
				}
				else
				{
					folpl.Append(c);
				}
				break;
			}
			if (c == '\\' && i + 1 < text.Length && npbqc(text[i + 1]) && 0 == 0)
			{
				folpl.Append(text[++i]);
				break;
			}
			xtdhy(sbnrz2, ajuaj.mbsvn, ref i, length);
			switch (c)
			{
			case '\\':
				ohamw = kcbfa.plabx;
				break;
			case '{':
			{
				if (++num > 100)
				{
					khcgl();
					return;
				}
				sbnrz sbnrz4 = new sbnrz(sbnrz2);
				sbnrz2.khhoe.Add(new mmgqv(sbnrz4));
				sbnrz2 = sbnrz4;
				break;
			}
			case '}':
				num--;
				if (sbnrz2.hcbnv == null || 1 == 0)
				{
					khcgl();
					return;
				}
				sbnrz2 = sbnrz2.hcbnv;
				break;
			}
			break;
		case kcbfa.plabx:
			if (char.IsLetter(c) && 0 == 0)
			{
				folpl.Append(c);
				bool flag = true;
				for (i++; i < text.Length; i++)
				{
					c = text[i];
					if (flag && 0 == 0)
					{
						if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'))
						{
							folpl.Append(c);
							continue;
						}
						if ((c < '0' || c > '9') && c != '-')
						{
							break;
						}
						folpl.Append(c);
						flag = false;
						if (!flag)
						{
							continue;
						}
					}
					if (c < '0' || c > '9')
					{
						break;
					}
					folpl.Append(c);
				}
				if (i == text.Length)
				{
					c = ' ';
				}
			}
			switch (c)
			{
			case '\\':
				xtdhy(sbnrz2, ajuaj.gdmuq, ref i, length);
				if (i + 1 < text.Length && npbqc(text[i + 1]) && 0 == 0)
				{
					folpl.Append(text[++i]);
					ohamw = kcbfa.gfuhx;
				}
				break;
			case '{':
			{
				if (++num > 100)
				{
					khcgl();
					return;
				}
				xtdhy(sbnrz2, ajuaj.gdmuq, ref i, length);
				sbnrz sbnrz3 = new sbnrz(sbnrz2);
				sbnrz2.khhoe.Add(new mmgqv(sbnrz3));
				sbnrz2 = sbnrz3;
				ohamw = kcbfa.gfuhx;
				break;
			}
			case '}':
				num--;
				xtdhy(sbnrz2, ajuaj.gdmuq, ref i, length);
				if (sbnrz2.hcbnv == null || 1 == 0)
				{
					khcgl();
					return;
				}
				sbnrz2 = sbnrz2.hcbnv;
				ohamw = kcbfa.gfuhx;
				break;
			case '-':
			case '_':
			case '~':
				if (folpl.Length == 0 || 1 == 0)
				{
					if (c == '~')
					{
						c = ' ';
						if (c != 0)
						{
							goto IL_03ed;
						}
					}
					if (c == '_')
					{
						c = '-';
					}
					goto IL_03ed;
				}
				xtdhy(sbnrz2, ajuaj.gdmuq, ref i, length);
				folpl.Append(c);
				goto IL_0421;
			case '\'':
				if (folpl.Length == 0 || 1 == 0)
				{
					if (i + 2 < text.Length)
					{
						folpl.Append(text.Substring(i + 1, 2));
					}
					else
					{
						folpl.Append("00");
					}
					i += 2;
					xtdhy(sbnrz2, ajuaj.lmtqx, ref i, length);
				}
				else
				{
					xtdhy(sbnrz2, ajuaj.gdmuq, ref i, length);
					folpl.Append(c);
				}
				ohamw = kcbfa.gfuhx;
				break;
			case ' ':
				xtdhy(sbnrz2, ajuaj.gdmuq, ref i, length);
				ohamw = kcbfa.gfuhx;
				break;
			default:
				{
					if (folpl.Length == 0 || 1 == 0)
					{
						folpl.Append(c);
						xtdhy(sbnrz2, ajuaj.gdmuq, ref i, length);
					}
					else
					{
						xtdhy(sbnrz2, ajuaj.gdmuq, ref i, length);
						folpl.Append(c);
						if (c > '\u007f')
						{
							xtdhy(sbnrz2, ajuaj.exzyo, ref i, length);
						}
					}
					ohamw = kcbfa.gfuhx;
					break;
				}
				IL_0421:
				ohamw = kcbfa.gfuhx;
				break;
				IL_03ed:
				folpl.Append(c);
				xtdhy(sbnrz2, ajuaj.gdmuq, ref i, length);
				goto IL_0421;
			}
			break;
		}
		i++;
		goto IL_0525;
		IL_0525:
		if (i >= length)
		{
			if (i <= length && sbnrz2 != naagl)
			{
				xtdhy(sbnrz2, (ohamw == kcbfa.plabx) ? ajuaj.gdmuq : ajuaj.mbsvn, ref i, length);
				khcgl();
			}
			else
			{
				ptirx(naagl);
			}
			return;
		}
		goto IL_0079;
	}

	private void ptirx(sbnrz p0)
	{
		int num = 0;
		if (num != 0)
		{
			goto IL_000c;
		}
		goto IL_0371;
		IL_000c:
		mmgqv mmgqv2 = p0.khhoe[num];
		switch (mmgqv2.jprco)
		{
		case ajuaj.icyep:
			ptirx(mmgqv2.iyvve);
			break;
		case ajuaj.lmtqx:
		case ajuaj.exzyo:
		{
			string text = string.Empty;
			while (true)
			{
				if (mmgqv2.jprco == ajuaj.lmtqx)
				{
					if (!brgjd.wsiiz(mmgqv2.iaqgi, out var p3))
					{
						p0.krbty.Reset();
						break;
					}
					eezlx[0] = p3;
				}
				else
				{
					eezlx[0] = (byte)mmgqv2.iaqgi[0];
				}
				int chars = p0.krbty.GetChars(eezlx, 0, 1, fjvjc, 0, flush: false);
				if (chars > 0)
				{
					text += new string(fjvjc, 0, chars);
					break;
				}
				if (num + 1 < p0.khhoe.Count && p0.khhoe[num + 1].jprco == mmgqv2.jprco)
				{
					p0.khhoe.RemoveAt(num);
					mmgqv2 = p0.khhoe[num];
					continue;
				}
				p0.krbty.Reset();
				break;
			}
			if (text.Length > 0)
			{
				mmgqv2.jprco = ajuaj.mbsvn;
				mmgqv2.iaqgi = text;
			}
			else
			{
				p0.khhoe.RemoveAt(num);
				num--;
			}
			break;
		}
		case ajuaj.gdmuq:
		{
			if (mmgqv2.iaqgi.Length < 2)
			{
				break;
			}
			Decoder value;
			if (mmgqv2.iaqgi.StartsWith("uc") && 0 == 0 && (mmgqv2.iaqgi.Length == 2 || char.IsDigit(mmgqv2.iaqgi[2])))
			{
				xmzwv(mmgqv2.iaqgi.Substring(2), out var p1);
				p0.efcpq((p1 < 0) ? 1 : p1);
			}
			else if (mmgqv2.iaqgi[0] == 'u' && ((char.IsDigit(mmgqv2.iaqgi[1]) ? true : false) || mmgqv2.iaqgi[1] == '-'))
			{
				if (!xmzwv(mmgqv2.iaqgi.Substring(1), out var p2))
				{
					break;
				}
				mmgqv2.jprco = ajuaj.mbsvn;
				mmgqv2.iaqgi = ((char)p2).ToString();
				p2 = p0.rakkx;
				while (p2 > 0 && num + 1 < p0.khhoe.Count)
				{
					mmgqv2 = p0.khhoe[num + 1];
					switch (mmgqv2.jprco)
					{
					case ajuaj.mbsvn:
						if (mmgqv2.iaqgi.Length <= p2)
						{
							p0.khhoe.RemoveAt(num + 1);
							p2 -= mmgqv2.iaqgi.Length;
							break;
						}
						mmgqv2.iaqgi = mmgqv2.iaqgi.Substring(p2);
						p2 = 0;
						if (p2 == 0)
						{
							break;
						}
						goto case ajuaj.lmtqx;
					case ajuaj.lmtqx:
						p0.khhoe.RemoveAt(num + 1);
						p2--;
						break;
					default:
						p2 = 0;
						break;
					}
				}
			}
			else if (mmgqv2.iaqgi[0] == 'f' && char.IsDigit(mmgqv2.iaqgi[1]) && 0 == 0 && asvbo.TryGetValue(mmgqv2.iaqgi, out value) && 0 == 0)
			{
				p0.fpzoh(value);
			}
			break;
		}
		}
		num++;
		goto IL_0371;
		IL_0371:
		if (num >= p0.khhoe.Count)
		{
			return;
		}
		goto IL_000c;
	}

	private static bool npbqc(char p0)
	{
		switch (p0)
		{
		case '\\':
		case '{':
		case '}':
			return true;
		default:
			return false;
		}
	}

	private void xtdhy(sbnrz p0, ajuaj p1, ref int p2, int p3)
	{
		if (folpl.Length <= 0)
		{
			return;
		}
		mmgqv mmgqv2 = new mmgqv(p1, folpl.ToString());
		folpl.Length = 0;
		if (p1 == ajuaj.gdmuq)
		{
			int p5;
			if (mmgqv2.iaqgi.StartsWith("bin", StringComparison.Ordinal) && 0 == 0)
			{
				if (xmzwv(mmgqv2.iaqgi.Substring(3), out var p4) && 0 == 0)
				{
					p2 += p4;
					if (p2 > p3)
					{
						khcgl();
						return;
					}
					whhll = true;
				}
			}
			else if (p0.yxvxm && 0 == 0)
			{
				string iaqgi;
				if ((iaqgi = mmgqv2.iaqgi) == null)
				{
					goto IL_015b;
				}
				if (!(iaqgi == "mac") || 1 == 0)
				{
					if (!(iaqgi == "pc") || 1 == 0)
					{
						if (!(iaqgi == "pca") || 1 == 0)
						{
							goto IL_015b;
						}
						naagl.fpzoh(EncodingTools.GetEncoding(850).GetDecoder());
					}
					else
					{
						naagl.fpzoh(EncodingTools.GetEncoding(437).GetDecoder());
					}
				}
				else
				{
					naagl.fpzoh(EncodingTools.GetEncoding(10000).GetDecoder());
				}
			}
			else if (mmgqv2.iaqgi.StartsWith("fcharset") && 0 == 0 && p0.khhoe[0].jprco == ajuaj.gdmuq && p0.khhoe[0].iaqgi[0] == 'f' && p0.hcbnv.khhoe[0].jprco == ajuaj.gdmuq && p0.hcbnv.khhoe[0].iaqgi == "fonttbl" && 0 == 0 && xmzwv(mmgqv2.iaqgi.Substring(8), out p5) && 0 == 0)
			{
				int num;
				switch (p5)
				{
				case 0:
					num = 1252;
					if (num != 0)
					{
						break;
					}
					goto case 77;
				case 77:
					num = 10000;
					if (num != 0)
					{
						break;
					}
					goto case 78;
				case 78:
					num = 10001;
					if (num != 0)
					{
						break;
					}
					goto case 79;
				case 79:
					num = 10003;
					if (num != 0)
					{
						break;
					}
					goto case 80;
				case 80:
					num = 10008;
					if (num != 0)
					{
						break;
					}
					goto case 81;
				case 81:
					num = 10002;
					if (num != 0)
					{
						break;
					}
					goto case 83;
				case 83:
					num = 10005;
					if (num != 0)
					{
						break;
					}
					goto case 84;
				case 84:
					num = 10004;
					if (num != 0)
					{
						break;
					}
					goto case 85;
				case 85:
					num = 10006;
					if (num != 0)
					{
						break;
					}
					goto case 86;
				case 86:
					num = 10081;
					if (num != 0)
					{
						break;
					}
					goto case 87;
				case 87:
					num = 10021;
					if (num != 0)
					{
						break;
					}
					goto case 88;
				case 88:
					num = 10029;
					if (num != 0)
					{
						break;
					}
					goto case 89;
				case 89:
					num = 10007;
					if (num != 0)
					{
						break;
					}
					goto case 128;
				case 128:
					num = 932;
					if (num != 0)
					{
						break;
					}
					goto case 129;
				case 129:
					num = 949;
					if (num != 0)
					{
						break;
					}
					goto case 130;
				case 130:
					num = 1361;
					if (num != 0)
					{
						break;
					}
					goto case 134;
				case 134:
					num = 936;
					if (num != 0)
					{
						break;
					}
					goto case 136;
				case 136:
					num = 950;
					if (num != 0)
					{
						break;
					}
					goto case 161;
				case 161:
					num = 1253;
					if (num != 0)
					{
						break;
					}
					goto case 162;
				case 162:
					num = 1254;
					if (num != 0)
					{
						break;
					}
					goto case 163;
				case 163:
					num = 1258;
					if (num != 0)
					{
						break;
					}
					goto case 177;
				case 177:
					num = 1255;
					if (num != 0)
					{
						break;
					}
					goto case 178;
				case 178:
					num = 1256;
					if (num != 0)
					{
						break;
					}
					goto case 186;
				case 186:
					num = 1257;
					if (num != 0)
					{
						break;
					}
					goto case 204;
				case 204:
					num = 1251;
					if (num != 0)
					{
						break;
					}
					goto case 222;
				case 222:
					num = 874;
					if (num != 0)
					{
						break;
					}
					goto case 238;
				case 238:
					num = 1250;
					if (num != 0)
					{
						break;
					}
					goto case 254;
				case 254:
					num = 437;
					if (num != 0)
					{
						break;
					}
					goto case 255;
				case 255:
					num = 850;
					if (num != 0)
					{
						break;
					}
					goto default;
				default:
					num = -1;
					break;
				}
				Decoder value = naagl.krbty;
				if (num != -1)
				{
					Encoding encoding = nmtby(num);
					if (encoding == null || 1 == 0)
					{
						encoding = Encoding.ASCII;
					}
					value = encoding.GetDecoder();
				}
				asvbo[p0.khhoe[0].iaqgi] = value;
			}
		}
		goto IL_05b7;
		IL_015b:
		if (mmgqv2.iaqgi.StartsWith("ansicpg") && 0 == 0)
		{
			string p6 = mmgqv2.iaqgi.Substring(7);
			if (xmzwv(p6, out var p7) && 0 == 0)
			{
				naagl.fpzoh(EncodingTools.GetEncoding(p7).GetDecoder());
			}
		}
		else if (mmgqv2.iaqgi.StartsWith("fromhtml") && 0 == 0)
		{
			veelr = true;
		}
		else if (mmgqv2.iaqgi.StartsWith("fromtext") && 0 == 0)
		{
			rpqss = true;
		}
		goto IL_05b7;
		IL_05b7:
		p0.khhoe.Add(mmgqv2);
	}

	private static Encoding nmtby(int p0)
	{
		try
		{
			return EncodingTools.GetEncoding(p0);
		}
		catch (Exception)
		{
			return null;
		}
	}

	private bool xmzwv(string p0, out int p1)
	{
		return brgjd.bnrqx(p0, out p1);
	}

	public string ccduh(Func<string, Exception, Exception> p0)
	{
		if (naagl == null || 1 == 0)
		{
			return null;
		}
		try
		{
			mnedn mnedn2 = new mnedn();
			mnedn2.chjez(this);
			mnedn2.anypa();
			return mnedn2.ydglw();
		}
		catch (MailException)
		{
			return null;
		}
		catch (Exception arg)
		{
			throw p0("Error while converting RTF to HTML.", arg);
		}
	}

	public string savrf()
	{
		if (veelr && 0 == 0)
		{
			return vteif(p0: true);
		}
		return null;
	}

	public string ibwvu()
	{
		if (rpqss && 0 == 0)
		{
			return vteif(p0: false);
		}
		return null;
	}

	private string vteif(bool p0)
	{
		jjdud = true;
		folpl.Length = 0;
		duiro(naagl, p0);
		return folpl.ToString();
	}

	private void duiro(sbnrz p0, bool p1)
	{
		if (p0.khhoe.Count == 0 || 1 == 0)
		{
			return;
		}
		string iaqgi;
		if (p0.khhoe[0].jprco == ajuaj.gdmuq && (iaqgi = p0.khhoe[0].iaqgi) != null && 0 == 0)
		{
			if (czzgh.fbang == null || 1 == 0)
			{
				czzgh.fbang = new Dictionary<string, int>(9)
				{
					{ "footnote", 0 },
					{ "header", 1 },
					{ "footer", 2 },
					{ "pict", 3 },
					{ "info", 4 },
					{ "fonttbl", 5 },
					{ "stylesheet", 6 },
					{ "colortbl", 7 },
					{ "pntext", 8 }
				};
			}
			if (czzgh.fbang.TryGetValue(iaqgi, out var value) && 0 == 0)
			{
				switch (value)
				{
				case 0:
				case 1:
				case 2:
				case 3:
				case 4:
				case 5:
				case 6:
				case 7:
				case 8:
					return;
				}
			}
		}
		int num = 0;
		if (num != 0)
		{
			goto IL_0129;
		}
		goto IL_042d;
		IL_0129:
		mmgqv mmgqv2 = p0.khhoe[num];
		switch (mmgqv2.jprco)
		{
		case ajuaj.icyep:
			if (p1 && 0 == 0)
			{
				if (mmgqv2.iyvve.khhoe.Count > 2 && mmgqv2.iyvve.khhoe[0].iaqgi == "*" && 0 == 0 && (!mmgqv2.iyvve.khhoe[1].iaqgi.StartsWith("html") || 1 == 0))
				{
					break;
				}
			}
			else if (mmgqv2.iyvve.khhoe.Count > 1 && mmgqv2.iyvve.khhoe[0].iaqgi == "*")
			{
				break;
			}
			duiro(mmgqv2.iyvve, p1);
			break;
		case ajuaj.gdmuq:
		{
			string iaqgi2;
			if ((iaqgi2 = mmgqv2.iaqgi) == null)
			{
				break;
			}
			if (czzgh.aqjqb == null || 1 == 0)
			{
				czzgh.aqjqb = new Dictionary<string, int>(13)
				{
					{ "htmlrtf0", 0 },
					{ "htmlrtf1", 1 },
					{ "htmlrtf", 2 },
					{ "line", 3 },
					{ "par", 4 },
					{ "tab", 5 },
					{ "bullet", 6 },
					{ "endash", 7 },
					{ "emdash", 8 },
					{ "lquote", 9 },
					{ "rquote", 10 },
					{ "ldblquote", 11 },
					{ "rdblquote", 12 }
				};
			}
			if (czzgh.aqjqb.TryGetValue(iaqgi2, out var value2) && 0 == 0)
			{
				switch (value2)
				{
				case 0:
					jjdud = true;
					break;
				case 1:
				case 2:
					jjdud = false;
					break;
				case 3:
				case 4:
					ysbbx("\r\n");
					break;
				case 5:
					ysbbx("\t");
					break;
				case 6:
					ysbbx("•");
					break;
				case 7:
					ysbbx("–");
					break;
				case 8:
					ysbbx("—");
					break;
				case 9:
					ysbbx("‘");
					break;
				case 10:
					ysbbx("’");
					break;
				case 11:
					ysbbx("“");
					break;
				case 12:
					ysbbx("”");
					break;
				}
			}
			break;
		}
		case ajuaj.mbsvn:
			ysbbx(mmgqv2.iaqgi.Replace("\r", "").Replace("\n", ""));
			break;
		}
		num++;
		goto IL_042d;
		IL_042d:
		if (num >= p0.khhoe.Count)
		{
			return;
		}
		goto IL_0129;
	}

	private void ysbbx(string p0)
	{
		if (jjdud && 0 == 0)
		{
			folpl.Append(p0);
		}
	}
}
