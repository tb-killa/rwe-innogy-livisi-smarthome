using System;
using System.IO;
using System.Text;
using Rebex.Mime;

namespace onrkn;

internal class mqucj
{
	private readonly string gfzlc;

	private readonly string smjky;

	private static mqucj geanv;

	internal static mqucj veixg
	{
		get
		{
			if (geanv == null || 1 == 0)
			{
				geanv = new mqucj("", null);
			}
			return geanv;
		}
	}

	internal bool tgwcz
	{
		get
		{
			if (gfzlc.Length == 0 || 1 == 0)
			{
				return smjky == null;
			}
			return false;
		}
	}

	internal string vffsa => gfzlc;

	internal string eeaqt => smjky;

	internal mqucj(string localPart, string domain)
	{
		if (localPart == null || 1 == 0)
		{
			throw new ArgumentNullException("localPart");
		}
		gfzlc = localPart;
		smjky = domain;
	}

	public static string vhevz(string p0)
	{
		int num = 0;
		if (num != 0)
		{
			goto IL_0006;
		}
		goto IL_0037;
		IL_0006:
		char c = p0[num];
		if (c <= '\u007f' && c != '.' && (!kgbvh.ynmbt(c) || 1 == 0))
		{
			return kgbvh.rqlyl(p0, '"', '"');
		}
		num++;
		goto IL_0037;
		IL_0037:
		if (num < p0.Length)
		{
			goto IL_0006;
		}
		return p0;
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (smjky != null && 0 == 0)
		{
			stringBuilder.Append(vhevz(gfzlc));
			stringBuilder.Append('@');
			stringBuilder.Append(vhevz(smjky));
		}
		else
		{
			stringBuilder.Append(gfzlc);
		}
		return stringBuilder.ToString();
	}

	public void cygyr(TextWriter p0, bool p1)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (p1 && 0 == 0)
		{
			stringBuilder.Append('<');
		}
		if (smjky != null && 0 == 0)
		{
			if (gfzlc.Length > 0)
			{
				stringBuilder.Append(vhevz(gfzlc));
				stringBuilder.Append("@");
				stringBuilder.Append(vhevz(smjky));
			}
			else
			{
				stringBuilder.Append(smjky);
			}
		}
		else if (gfzlc.Length > 0)
		{
			stringBuilder.Append(gfzlc);
		}
		if (p1 && 0 == 0)
		{
			stringBuilder.Append('>');
		}
		p0.Write(stringBuilder.ToString());
	}

	private static string ilhsj(stzvh p0, bool p1)
	{
		int num = -1;
		StringBuilder stringBuilder = new StringBuilder();
		while (true)
		{
			if ((p0.zsywy ? true : false) || p0.havrs == ',' || p0.havrs == ';' || p0.havrs == ':')
			{
				if (p1 && 0 == 0)
				{
					p0.hdpha(',', ';');
				}
				break;
			}
			if (((!p1 || 1 == 0) && p0.havrs == '<') || (p1 && 0 == 0 && p0.havrs == '>'))
			{
				break;
			}
			if ((!p1 || 1 == 0) && p0.havrs == '(')
			{
				num = p0.bauax;
				p0.hdpha();
				continue;
			}
			if (!kgbvh.ynmbt(p0.havrs) || 1 == 0)
			{
				stringBuilder.Append(p0.havrs);
				p0.pfdcf();
			}
			else
			{
				lbexf p2 = p0.pgtcy();
				stringBuilder.arumx(p2);
			}
			num = -1;
			if (p1 && 0 == 0)
			{
				p0.hdpha();
			}
			else
			{
				p0.bwnkb();
			}
		}
		if (num >= 0)
		{
			p0.bauax = num;
		}
		return stringBuilder.ToString();
	}

	private static string tfgmu(stzvh p0)
	{
		p0.pmsxy('[');
		StringBuilder stringBuilder = new StringBuilder();
		while (true)
		{
			p0.hdpha();
			if (p0.zsywy && 0 == 0)
			{
				throw MimeException.dngxr(p0.bauax, brgjd.edcru("Header ends prematurely while character '{0}' was expected.", ']'));
			}
			char c = p0.pfdcf();
			if (c == ']')
			{
				break;
			}
			if (p0.zsywy && 0 == 0)
			{
				throw MimeException.dngxr(p0.bauax, brgjd.edcru("Header ends prematurely while character '{0}' was expected.", ']'));
			}
			if (c == '\\')
			{
				c = p0.pfdcf();
			}
			stringBuilder.Append(c);
		}
		return stringBuilder.ToString();
	}

	private static string ziakv(stzvh p0, bool p1)
	{
		p0.hdpha();
		switch (p0.havrs)
		{
		case '"':
		{
			bdhvt bdhvt2 = p0.oypkh();
			return bdhvt2.ToString();
		}
		case '[':
			return tfgmu(p0);
		default:
			return ilhsj(p0, p1);
		}
	}

	private static void ralrd(stzvh p0, bool p1)
	{
		p0.hdpha();
		if (p0.havrs != '@')
		{
			return;
		}
		p0.pfdcf();
		if (p0.zsywy)
		{
			return;
		}
		while (p0.havrs != ':')
		{
			string text = ziakv(p0, p1);
			if (text.Length == 0 || 1 == 0)
			{
				p0.pfdcf();
			}
			if (p1 && 0 == 0)
			{
				p0.hdpha('>');
			}
			p0.hdpha(',', ';', '@');
			if (p0.zsywy && 0 == 0)
			{
				return;
			}
		}
		p0.pmsxy(':');
		p0.hdpha();
	}

	private static string llzdz(stzvh p0)
	{
		StringBuilder stringBuilder = new StringBuilder();
		while ((!p0.zsywy || 1 == 0) && p0.havrs != '@' && p0.havrs != '>')
		{
			if (p0.havrs == '.')
			{
				stringBuilder.Append('.');
				p0.pfdcf();
				p0.bwnkb();
			}
			else if (p0.havrs == ':')
			{
				if (stringBuilder.Length > 0)
				{
					if (stringBuilder[0] == '[' && stringBuilder.ToString().StartsWith("[FAX"))
					{
						stringBuilder.Append(':');
					}
					else
					{
						stringBuilder.Length = 0;
					}
				}
				p0.pfdcf();
				p0.hdpha();
			}
			else if (!kgbvh.ynmbt(p0.havrs) || 1 == 0)
			{
				stringBuilder.Append(p0.havrs);
				p0.pfdcf();
			}
			else
			{
				lbexf p1 = p0.pgtcy();
				stringBuilder.arumx(p1);
				p0.hdpha();
			}
		}
		if (stringBuilder.Length > 0 && stringBuilder[stringBuilder.Length - 1] == ';')
		{
			stringBuilder.Length--;
		}
		return stringBuilder.ToString();
	}

	private static string qfcag(stzvh p0)
	{
		p0.hdpha();
		StringBuilder stringBuilder = new StringBuilder();
		while (!p0.zsywy)
		{
			switch (p0.havrs)
			{
			case '"':
			{
				bdhvt bdhvt2 = p0.oypkh();
				stringBuilder.Append(bdhvt2.ToString());
				break;
			}
			case '>':
			case '@':
				if (stringBuilder.Length > 0 && stringBuilder[stringBuilder.Length - 1] == '>')
				{
					stringBuilder.Length--;
				}
				return stringBuilder.ToString();
			default:
			{
				string text = llzdz(p0);
				stringBuilder.Append(text.ToString());
				break;
			}
			}
			p0.hdpha();
		}
		if (stringBuilder.Length > 0 && stringBuilder[stringBuilder.Length - 1] == '>')
		{
			stringBuilder.Length--;
		}
		return stringBuilder.ToString();
	}

	public static mqucj xdgws(stzvh p0)
	{
		p0.hdpha();
		bool flag = false;
		if (p0.havrs == '<')
		{
			p0.pfdcf();
			p0.hdpha();
			flag = true;
		}
		if (p0.havrs == '@')
		{
			ralrd(p0, flag);
		}
		string text = qfcag(p0);
		if (p0.zsywy && 0 == 0)
		{
			if (text.Length == 0 || 1 == 0)
			{
				return null;
			}
			return new mqucj(text, null);
		}
		if (p0.havrs == '>')
		{
			p0.pfdcf();
			if (text.Length == 0 || 1 == 0)
			{
				return null;
			}
			return new mqucj(text, null);
		}
		p0.pmsxy('@');
		if (p0.zsywy && 0 == 0)
		{
			return new mqucj(text, null);
		}
		string text2 = ziakv(p0, flag);
		if (flag && 0 == 0)
		{
			p0.avkco('>');
		}
		string text3;
		if (text2.Length == 0 || 1 == 0)
		{
			if (text.Length == 0 || 1 == 0)
			{
				return veixg;
			}
			text3 = text;
		}
		else if (text.Length > 0)
		{
			text = text.TrimEnd('@');
			text2 = text2.TrimStart('@');
			text3 = brgjd.edcru("{0}@{1}", text, text2);
		}
		else
		{
			text3 = text2;
		}
		text3 = text3.Trim('@');
		return new mqucj(text3);
	}

	internal mqucj(string address)
	{
		string[] array = address.Split('@');
		if (array.Length == 0 || 1 == 0)
		{
			gfzlc = "";
			return;
		}
		gfzlc = array[0];
		if (array.Length > 1)
		{
			smjky = array[1];
		}
	}
}
