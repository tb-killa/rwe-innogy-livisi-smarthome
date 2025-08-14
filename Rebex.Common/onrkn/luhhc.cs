using System;
using System.Collections.Generic;

namespace onrkn;

internal class luhhc : shdfo
{
	private readonly string buojl;

	private readonly string upmcu;

	private readonly wcrlo vugat;

	private readonly evdac ywpuk;

	private readonly evdac ecpxu;

	public luhhc(string basePath, wcrlo baseFilesystem)
	{
		vugat = baseFilesystem;
		ywpuk = baseFilesystem.tunjy();
		ecpxu = ywuwt(ywpuk);
		if (basePath.StartsWith("\\\\") && 0 == 0)
		{
			int num = basePath.IndexOf('\\', 2);
			if (num < 0)
			{
				throw new ArgumentException("Invalid UNC path.", "basePath");
			}
			buojl = basePath.Substring(0, num);
			basePath = basePath.Substring(num);
			if (((buojl == string.Empty) ? true : false) || buojl == "?" || basePath == string.Empty)
			{
				throw new ArgumentException("Invalid UNC path.", "basePath");
			}
		}
		else
		{
			buojl = string.Empty;
		}
		upmcu = ywpuk.phbkz(basePath).TrimEnd(ywpuk.xwsda);
	}

	private string kzrsv(string p0, bool p1)
	{
		p0 = ecpxu.yqwyg(p0, "");
		if (p0.Length == 0 || false || p0[0] != '/')
		{
			if (p1 && 0 == 0)
			{
				throw new nfcev(fvjcl.nzuue);
			}
			return null;
		}
		if (p0 == "/" && 0 == 0)
		{
			p0 = upmcu;
			if ((p0.Length == 2 && p0[1] == ':') || p0.Length == 0 || 1 == 0)
			{
				p0 += '/';
			}
		}
		else
		{
			p0 = ywpuk.phbkz(p0);
			p0 = upmcu + p0;
		}
		return buojl + p0;
	}

	public override vgycx wxxmm(string p0)
	{
		string text = kzrsv(p0, p1: false);
		if (text == null || 1 == 0)
		{
			return null;
		}
		vgycx vgycx2 = vugat.wxxmm(text);
		if (vgycx2 != null && 0 == 0 && p0 == "/" && 0 == 0)
		{
			vgycx2.wyuyy = ".";
		}
		return vgycx2;
	}

	public override xwtwn ellsg(string p0, string p1)
	{
		p0 = kzrsv(p0, p1: true);
		return vugat.ellsg(p0, p1);
	}

	public override eyqzi nxqay(string p0, rdvij p1, hqxly p2)
	{
		p0 = kzrsv(p0, p1: true);
		return vugat.nxqay(p0, p1, p2);
	}

	public override void kosgg(string p0)
	{
		p0 = kzrsv(p0, p1: true);
		vugat.kosgg(p0);
	}

	public override void tkipb(string p0)
	{
		p0 = kzrsv(p0, p1: true);
		vugat.tkipb(p0);
	}

	public override void xptlv(string p0)
	{
		p0 = kzrsv(p0, p1: true);
		vugat.xptlv(p0);
	}

	public override void bwzap(string p0, vgycx p1)
	{
		p0 = kzrsv(p0, p1: true);
		vugat.bwzap(p0, p1);
	}

	public override void jcpei(string p0, string p1)
	{
		if (p0 == "/" && 0 == 0)
		{
			throw new nfcev(fvjcl.jlxwg, "Cannot copy root directory.");
		}
		p0 = kzrsv(p0, p1: true);
		p1 = kzrsv(p1, p1: true);
		vugat.jcpei(p0, p1);
	}

	public override void idbxb(string p0, string p1)
	{
		if (p0 == "/" && 0 == 0)
		{
			throw new nfcev(fvjcl.jlxwg, "Cannot rename root directory.");
		}
		p0 = kzrsv(p0, p1: true);
		p1 = kzrsv(p1, p1: true);
		vugat.idbxb(p0, p1);
	}

	public override evdac tunjy()
	{
		return ecpxu;
	}

	public evdac uitkp()
	{
		return ywpuk;
	}

	private static evdac ywuwt(evdac p0)
	{
		char c = '/';
		char altDirectorySeparatorChar = c;
		char[] array = p0.jqvwt();
		List<char> list = new List<char>();
		char[] array2 = dahxy.cexjz();
		int num = 0;
		if (num != 0)
		{
			goto IL_0024;
		}
		goto IL_0060;
		IL_0024:
		char c2 = array2[num];
		if (c2 != c && c2 != '?' && c2 != '*')
		{
			if (c2 == '\\' && Array.IndexOf(array, c2) < 0)
			{
				altDirectorySeparatorChar = c2;
			}
			else
			{
				list.Add(c2);
			}
		}
		num++;
		goto IL_0060;
		IL_0060:
		if (num < array2.Length)
		{
			goto IL_0024;
		}
		return new evdac(c, altDirectorySeparatorChar, c, caseSensitive: true, p0.kfhql, list.ToArray());
	}

	public hmvwx ziacd(string p0)
	{
		return new hmvwx(p0, upmcu, ywpuk);
	}
}
