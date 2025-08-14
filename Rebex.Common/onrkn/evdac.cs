using System;
using System.Collections.Generic;

namespace onrkn;

internal class evdac
{
	private readonly char[] yofej;

	private readonly char[] xpzby;

	private char fjnlw;

	private char jwzxn;

	private char zeqtz;

	private bool zakts;

	private bool jvdmf;

	public char xwsda
	{
		get
		{
			return fjnlw;
		}
		private set
		{
			fjnlw = value;
		}
	}

	public char uyupw
	{
		get
		{
			return jwzxn;
		}
		private set
		{
			jwzxn = value;
		}
	}

	public char dhpem
	{
		get
		{
			return zeqtz;
		}
		private set
		{
			zeqtz = value;
		}
	}

	public bool arswr
	{
		get
		{
			return zakts;
		}
		private set
		{
			zakts = value;
		}
	}

	public bool kfhql
	{
		get
		{
			return jvdmf;
		}
		private set
		{
			jvdmf = value;
		}
	}

	public evdac(char directorySeparatorChar, char altDirectorySeparatorChar, char volumeSeparatorChar, bool caseSensitive, bool supportsFileSync, char[] invalidPathChars)
	{
		xwsda = directorySeparatorChar;
		uyupw = altDirectorySeparatorChar;
		dhpem = volumeSeparatorChar;
		arswr = caseSensitive;
		kfhql = supportsFileSync;
		yofej = invalidPathChars;
		xpzby = new char[2] { xwsda, uyupw };
	}

	public char[] jqvwt()
	{
		return yofej;
	}

	public bool wkilu(string p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("path");
		}
		if (p0.IndexOfAny(yofej) >= 0)
		{
			return true;
		}
		return false;
	}

	public bool kaees(string p0)
	{
		if (p0 == null || 1 == 0)
		{
			return false;
		}
		if (p0.Length == 0 || 1 == 0)
		{
			return false;
		}
		if (p0[0] == xwsda || p0[0] == uyupw)
		{
			return true;
		}
		if (dhpem != xwsda && dhpem != uyupw && p0.IndexOf(dhpem) >= 0)
		{
			return true;
		}
		return false;
	}

	public string ztzip(string p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("path");
		}
		int num = p0.LastIndexOfAny(xpzby);
		if (num < 0)
		{
			return p0;
		}
		num++;
		if (num == p0.Length)
		{
			return ".";
		}
		return p0.Substring(num);
	}

	public string yqwyg(string p0, string p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("path");
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("relativePath");
		}
		if (wkilu(p0) && 0 == 0)
		{
			throw new nfcev(fvjcl.vmbsn, "Path contains invalid characters.");
		}
		if (wkilu(p1) && 0 == 0)
		{
			throw new nfcev(fvjcl.vmbsn, "Path contains invalid characters.");
		}
		List<string> list = new List<string>();
		bool flag;
		if (kaees(p1) && 0 == 0)
		{
			int num = p1.IndexOfAny(xpzby);
			if (num < 0)
			{
				throw new nfcev(fvjcl.vmbsn, "Invalid relative path.");
			}
			string item = p1.Substring(0, num);
			p0 = p1.Substring(num + 1);
			p1 = null;
			list.Add(item);
			flag = true;
			if (flag)
			{
				goto IL_011b;
			}
		}
		if (kaees(p0) && 0 == 0)
		{
			int num2 = p0.IndexOfAny(xpzby);
			if (num2 < 0)
			{
				throw new nfcev(fvjcl.vmbsn);
			}
			string item2 = p0.Substring(0, num2);
			p0 = p0.Substring(num2 + 1);
			list.Add(item2);
			flag = true;
			if (flag)
			{
				goto IL_011b;
			}
		}
		flag = false;
		goto IL_011b;
		IL_011b:
		if (p0.Length > 0)
		{
			list.AddRange(p0.Split(xpzby));
		}
		if (p1 != null && 0 == 0 && p1.Length > 0)
		{
			list.AddRange(p1.Split(xpzby));
		}
		for (int i = ((flag ? true : false) ? 1 : 0); i < list.Count; i++)
		{
			string text = list[i];
			string text2;
			if ((text2 = text) == null)
			{
				continue;
			}
			if ((!(text2 == "") || 1 == 0) && (!(text2 == ".") || 1 == 0))
			{
				if (text2 == "..")
				{
					if (i > 1)
					{
						list.RemoveRange(i - 1, 2);
						i -= 2;
					}
					else
					{
						list.RemoveAt(i);
						i--;
					}
				}
			}
			else
			{
				list.RemoveAt(i);
				i--;
			}
		}
		if (list.Count == 0 || 1 == 0)
		{
			return "";
		}
		if (list.Count == 1 && flag && 0 == 0)
		{
			list.Insert(0, "");
		}
		return string.Join(xwsda.ToString(), list.ToArray());
	}

	public string phbkz(string p0)
	{
		if (uyupw != xwsda)
		{
			p0 = p0.Replace(uyupw, xwsda);
		}
		return p0;
	}
}
