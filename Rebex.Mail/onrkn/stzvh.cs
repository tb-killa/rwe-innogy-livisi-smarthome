using Rebex.Mime;

namespace onrkn;

internal class stzvh
{
	private readonly string xyeao;

	private readonly string fxkca;

	private int fzuaj;

	public bool zsywy => fzuaj >= fxkca.Length;

	public int bauax
	{
		get
		{
			return fzuaj;
		}
		set
		{
			fzuaj = value;
		}
	}

	public char havrs
	{
		get
		{
			if (fzuaj < fxkca.Length)
			{
				return fxkca[fzuaj];
			}
			throw MimeException.dngxr(fzuaj, "Header ends prematurely.");
		}
	}

	public string lxjgt => xyeao;

	public string sosem => fxkca;

	public override string ToString()
	{
		return fxkca;
	}

	public char pfdcf()
	{
		if (zsywy && 0 == 0)
		{
			throw MimeException.dngxr(fzuaj, "Header ends prematurely.");
		}
		char result = havrs;
		fzuaj++;
		return result;
	}

	public stzvh(string val)
	{
		xyeao = val;
		fxkca = val.Replace("\n", "");
	}

	public void bwnkb()
	{
		while (!zsywy && (kgbvh.zhxzu(havrs) ? true : false))
		{
			pfdcf();
		}
	}

	public string vgeze()
	{
		bwnkb();
		if (havrs != '(')
		{
			return null;
		}
		int num = fzuaj + 1;
		int num2 = 1;
		bool flag = false;
		if (flag)
		{
			goto IL_0022;
		}
		goto IL_0070;
		IL_006c:
		num++;
		goto IL_0070;
		IL_0022:
		if (flag && 0 == 0)
		{
			flag = false;
			if (!flag)
			{
				goto IL_006c;
			}
		}
		switch (fxkca[num])
		{
		case '(':
			num2++;
			break;
		case ')':
			if (num2 > 0)
			{
				num2--;
			}
			break;
		case '\\':
			flag = true;
			break;
		}
		goto IL_006c;
		IL_0070:
		if (num >= fxkca.Length || ((!flag || 1 == 0) && num2 <= 0))
		{
			string result = fxkca.Substring(fzuaj, num - fzuaj);
			fzuaj = num;
			return result;
		}
		goto IL_0022;
	}

	public void hdpha(params char[] p0)
	{
		while (!zsywy)
		{
			char c = havrs;
			if (kgbvh.zhxzu(c) && 0 == 0)
			{
				pfdcf();
				continue;
			}
			if (c == '(')
			{
				vgeze();
				continue;
			}
			bool flag = false;
			int num = 0;
			if (num != 0)
			{
				goto IL_003e;
			}
			goto IL_004d;
			IL_003e:
			if (c == p0[num])
			{
				flag = true;
				if (flag)
				{
					goto IL_0053;
				}
			}
			num++;
			goto IL_004d;
			IL_004d:
			if (num < p0.Length)
			{
				goto IL_003e;
			}
			goto IL_0053;
			IL_0053:
			if (!flag || 1 == 0)
			{
				break;
			}
			pfdcf();
		}
	}

	public void pmsxy(char p0)
	{
		hdpha();
		if (zsywy && 0 == 0)
		{
			throw MimeException.dngxr(fzuaj, brgjd.edcru("Header ends prematurely while character '{0}' was expected.", p0));
		}
		if (havrs != p0)
		{
			throw MimeException.dngxr(fzuaj, brgjd.edcru("Character '{0}' read instead of expected character '{1}'.", havrs, p0));
		}
		pfdcf();
	}

	public void avkco(char p0)
	{
		while (true)
		{
			hdpha();
			if ((zsywy && 0 == 0) || havrs == p0)
			{
				break;
			}
			pfdcf();
		}
	}

	public bdhvt oypkh()
	{
		hdpha();
		string p;
		int num = kgbvh.csiln(fxkca, fzuaj, out p);
		if (num < 0)
		{
			if (zsywy && 0 == 0)
			{
				throw MimeException.dngxr(fzuaj, brgjd.edcru("Header ends prematurely while character '{0}' was expected.", '"'));
			}
			throw MimeException.dngxr(fzuaj, brgjd.edcru("Character '{0}' read instead of expected character '{1}'.", havrs, '"'));
		}
		fzuaj = num;
		return new bdhvt(p);
	}

	public dnkfp pgtcy()
	{
		hdpha();
		if (havrs == '.')
		{
			throw MimeException.dngxr(fzuaj, "Header ends prematurely.");
		}
		int i;
		for (i = fzuaj; i < fxkca.Length && ((kgbvh.ynmbt(fxkca[i]) ? true : false) || fxkca[i] == '.' || fxkca[i] >= '\u0080'); i++)
		{
		}
		if (i == fzuaj)
		{
			throw MimeException.dngxr(fzuaj, "Invalid character.");
		}
		string contents = fxkca.Substring(fzuaj, i - fzuaj);
		fzuaj = i;
		return new dnkfp(contents);
	}

	public string abglp()
	{
		string text = fxkca;
		int num = text.IndexOf("=?", fzuaj);
		if (num != fzuaj)
		{
			return null;
		}
		int num2 = text.IndexOf('?', num + 2);
		if (num2 < num + 3 || num2 == text.Length - 1)
		{
			return null;
		}
		int num3 = text.IndexOf('?', num2 + 2);
		if (num3 < num2 + 2 || num3 == text.Length - 1)
		{
			return null;
		}
		int num4 = text.IndexOf("?=", num3 + 1);
		if (num4 < num3 + 2)
		{
			return null;
		}
		for (int i = num3 + 1; i < num4; i++)
		{
			char c = text[i];
			if (c != ' ' && c != '\t' && c != '"' && (!kgbvh.ynmbt(c) || 1 == 0))
			{
				return null;
			}
		}
		return text.Substring(num, (fzuaj = num4 + 2) - num);
	}

	public lbexf iwhve()
	{
		hdpha();
		if (havrs == '=')
		{
			string text = abglp();
			if (text != null && 0 == 0)
			{
				return new dnkfp(text);
			}
		}
		if (havrs == '"')
		{
			return oypkh();
		}
		return pgtcy();
	}

	public string aqydg()
	{
		hdpha();
		int i;
		for (i = fzuaj; i < fxkca.Length && fxkca[i] > ' ' && fxkca[i] < '\u007f' && fxkca[i] != ';'; i++)
		{
		}
		string result = fxkca.Substring(fzuaj, i - fzuaj);
		fzuaj = i;
		return result;
	}

	public string wzlcv()
	{
		int i;
		for (i = fzuaj; i < fxkca.Length && (fxkca[i] > '~' || (kgbvh.uyowq(fxkca[i]) ? true : false) || fxkca[i] == '@'); i++)
		{
		}
		if (i == fzuaj)
		{
			return null;
		}
		string result = fxkca.Substring(fzuaj, i - fzuaj);
		fzuaj = i;
		return result;
	}
}
