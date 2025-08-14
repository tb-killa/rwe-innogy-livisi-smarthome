using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using onrkn;

namespace Rebex.IO;

public class FileSet
{
	private class ydnvl : ReadOnlyCollection<ezewr>
	{
		public ydnvl()
			: base((IList<ezewr>)new List<ezewr>())
		{
		}

		public void mjvon(string p0, TraversalMode p1)
		{
			base.Items.Add(new ezewr(p0, p1));
		}
	}

	private class ezewr
	{
		private string jrqtj;

		private TraversalMode kjdky;

		public string tfavb => jrqtj;

		public TraversalMode wunwl => kjdky;

		public override bool Equals(object obj)
		{
			if (obj == null || 1 == 0)
			{
				throw new ArgumentNullException("obj", "Argument cannot be null.");
			}
			if (!(obj is ezewr ezewr) || 1 == 0)
			{
				throw new ArgumentException("obj", "Argument type mismatch.");
			}
			if (object.Equals(wunwl, ezewr.wunwl) && 0 == 0)
			{
				return object.Equals(tfavb, ezewr.tfavb);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return jrqtj.GetHashCode() ^ (int)kjdky;
		}

		internal ezewr(string pattern, TraversalMode mode)
		{
			jrqtj = pattern;
			kjdky = mode;
		}

		public override string ToString()
		{
			return brgjd.edcru("{0} ({1})", jrqtj, kjdky);
		}
	}

	private string dqymf;

	private bool kvndz;

	private bool uoxhv;

	private bool fdedj;

	private bool ptbtg;

	private string prtlv;

	private char zafdv;

	private char[] cyguh;

	private List<Regex> xailk;

	private List<Regex> zncrf;

	private List<Regex> nxmzm;

	private List<Regex> jochm;

	private List<Regex> azqgc;

	private List<Regex> mxgwd;

	private ydnvl sajpd;

	private ydnvl vbctx;

	private static char[] fbbet;

	private static char[] vjbqz
	{
		get
		{
			if (fbbet == null || 1 == 0)
			{
				if (dahxy.nmnrp == dahxy.ymzqe)
				{
					fbbet = new char[1] { dahxy.ymzqe };
				}
				else
				{
					fbbet = new char[2]
					{
						dahxy.ymzqe,
						dahxy.nmnrp
					};
				}
			}
			return fbbet;
		}
	}

	private char dscmo => zafdv;

	public string BasePath
	{
		get
		{
			return dqymf;
		}
		set
		{
			if (value == null || 1 == 0)
			{
				throw new ArgumentNullException("value", "Path cannot be null.");
			}
			dqymf = value;
		}
	}

	public bool IsCaseSensitive
	{
		get
		{
			return kvndz;
		}
		set
		{
			if (kvndz != value)
			{
				kvndz = value;
				mbghw();
			}
		}
	}

	public bool EmptyDirectoriesIncluded
	{
		get
		{
			return uoxhv;
		}
		set
		{
			uoxhv = value;
		}
	}

	public bool ContainingDirectoriesIncluded
	{
		get
		{
			return fdedj;
		}
		set
		{
			fdedj = value;
		}
	}

	public bool Flatten
	{
		get
		{
			return ptbtg;
		}
		set
		{
			ptbtg = value;
		}
	}

	public FileSet()
		: this(string.Empty)
	{
	}

	public FileSet(string basePath)
		: this(basePath, vjbqz)
	{
	}

	public FileSet(params char[] directorySeparators)
		: this(string.Empty, directorySeparators)
	{
	}

	public FileSet(string basePath, params char[] directorySeparators)
	{
		if (directorySeparators == null || 1 == 0)
		{
			throw new ArgumentNullException("directorySeparators", "Collection cannot be null.");
		}
		if (directorySeparators.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("Collection cannot be empty.", "directorySeparators");
		}
		BasePath = basePath;
		EmptyDirectoriesIncluded = true;
		ContainingDirectoriesIncluded = true;
		xailk = new List<Regex>();
		zncrf = new List<Regex>();
		nxmzm = new List<Regex>();
		jochm = new List<Regex>();
		azqgc = new List<Regex>();
		mxgwd = new List<Regex>();
		sajpd = new ydnvl();
		vbctx = new ydnvl();
		jhetk(directorySeparators);
	}

	public FileSet(string basePath, string pattern)
		: this(basePath)
	{
		Include(pattern);
	}

	public FileSet(string basePath, string pattern, TraversalMode mode)
		: this(basePath)
	{
		Include(pattern, mode);
	}

	private void jhetk(params char[] p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("directorySeparators", "Collection cannot be null.");
		}
		if (p0.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("Collection cannot be empty.", "directorySeparators");
		}
		cyguh = new char[p0.Length];
		Array.Copy(p0, cyguh, p0.Length);
		zafdv = cyguh[0];
		prtlv = cyguh[0].ToString();
		mbghw();
	}

	public void Include(string pattern)
	{
		Include(pattern, TraversalMode.Recursive);
	}

	public virtual void Include(string pattern, TraversalMode mode)
	{
		vbood(pattern, mode, p2: true);
	}

	private void vbood(string p0, TraversalMode p1, bool p2)
	{
		wcmzo(p0, p1);
		bool p3;
		string p4;
		bool p5;
		string p6;
		bool p7;
		string text = miirf(p0, p1, out p3, out p4, out p5, out p6, out p7);
		if (p2 && 0 == 0)
		{
			sajpd.mjvon(p0, p1);
		}
		if (p3 && 0 == 0)
		{
			xailk.Add(idsra(text, p1: true, p2: true));
		}
		if (p4 != null && 0 == 0)
		{
			xailk.Add(idsra(text + p4, p1: true, p2: true));
		}
		if (p6 != null && 0 == 0)
		{
			text += p6;
			zncrf.Add(idsra(text, p1: false, p2: true));
		}
		else if (p5 && 0 == 0)
		{
			zncrf.Add(idsra(text, p1: false, p2: true));
		}
		int num = text.IndexOf("**", StringComparison.Ordinal);
		if (num >= 0)
		{
			text = text.Substring(0, num + 2);
			num--;
			nxmzm.Add(idsra(text, p1: false, p2: true));
		}
		int startIndex = 0;
		int num2 = text.IndexOf(dscmo, startIndex);
		while (num2 > 0 && num2 != num)
		{
			nxmzm.Add(idsra(text.Substring(0, num2), p1: false, p2: true));
			startIndex = num2 + 1;
			num2 = text.IndexOf(dscmo, startIndex);
		}
	}

	public void Exclude(string pattern)
	{
		Exclude(pattern, TraversalMode.Recursive);
	}

	public virtual void Exclude(string pattern, TraversalMode mode)
	{
		ildwo(pattern, mode, p2: true);
	}

	private void ildwo(string p0, TraversalMode p1, bool p2)
	{
		wcmzo(p0, p1);
		bool p3;
		string p4;
		bool p5;
		string p6;
		bool p7;
		string text = miirf(p0, p1, out p3, out p4, out p5, out p6, out p7);
		if (p2 && 0 == 0)
		{
			vbctx.mjvon(p0, p1);
		}
		if (p3 && 0 == 0)
		{
			jochm.Add(idsra(text, p1: true, p2: true));
		}
		if (p4 != null && 0 == 0)
		{
			jochm.Add(idsra(text + p4, p1: true, p2: true));
		}
		if (p5 && 0 == 0)
		{
			azqgc.Add(idsra(text, p1: false, p2: false));
		}
		if (p6 != null && 0 == 0)
		{
			azqgc.Add(idsra(text + p6, p1: false, p2: false));
		}
		if (p7 && 0 == 0)
		{
			mxgwd.Add(idsra(text + p6, p1: false, p2: true));
		}
	}

	public virtual bool IsMatch(string relativePath, FileSetMatchMode mode)
	{
		if (relativePath == null || 1 == 0)
		{
			throw new ArgumentNullException("relativePath", "Path cannot be null.");
		}
		if (relativePath.Trim().Length == 0 || 1 == 0)
		{
			throw new ArgumentException("Path cannot be empty.", "relativePath");
		}
		if (wibzc(relativePath) && 0 == 0)
		{
			throw new ArgumentException("Argument has to be a relative path.", "relativePath");
		}
		string text = spcvu(relativePath);
		if (text.Length > 1 && ((ccazi(text, ".") ? true : false) || ccazi(text, "..")))
		{
			throw new ArgumentException(brgjd.edcru("Path cannot contain special directories '.' and '..' ({0}).", relativePath), "relativePath");
		}
		List<Regex> list;
		List<Regex> list2;
		switch (mode)
		{
		case FileSetMatchMode.MatchFile:
			if (text.Length == 1 && text[0] == '.')
			{
				return false;
			}
			if (zenlx(text) && 0 == 0)
			{
				return false;
			}
			list = xailk;
			list2 = jochm;
			break;
		case FileSetMatchMode.MatchDirectory:
			if (text.Length == 1 && text[0] == '.')
			{
				return false;
			}
			list = zncrf;
			list2 = azqgc;
			break;
		case FileSetMatchMode.TraverseDirectory:
			if (text.Length == 1 && text[0] == '.')
			{
				return sajpd.Count != 0;
			}
			list = nxmzm;
			list2 = mxgwd;
			break;
		default:
			throw new ArgumentException("Invalid mode.", "mode");
		}
		if (list.Count == 0 || 1 == 0)
		{
			return false;
		}
		using (List<Regex>.Enumerator enumerator = list2.GetEnumerator())
		{
			while (enumerator.MoveNext() ? true : false)
			{
				Regex current = enumerator.Current;
				if (current.IsMatch(text) && 0 == 0)
				{
					return false;
				}
			}
		}
		using (List<Regex>.Enumerator enumerator2 = list.GetEnumerator())
		{
			while (enumerator2.MoveNext() ? true : false)
			{
				Regex current2 = enumerator2.Current;
				if (current2.IsMatch(text) && 0 == 0)
				{
					return true;
				}
			}
		}
		return false;
	}

	public LocalItemCollection GetLocalItems()
	{
		string path = ((string.IsNullOrEmpty(dqymf) ? true : false) ? "." : dqymf);
		LocalItem localItem = new LocalItem(path);
		localItem.bgruk(string.Empty);
		if (!localItem.Exists || 1 == 0)
		{
			throw vtdxm.iwvtz("Could not find a part of the path '{0}'.", dqymf);
		}
		if (!localItem.IsDirectory || 1 == 0)
		{
			throw vtdxm.iwvtz("Specified base path is not a directory '{0}'.", dqymf);
		}
		LocalItemCollection localItemCollection = new LocalItemCollection();
		if (IsMatch(".", FileSetMatchMode.TraverseDirectory) && 0 == 0)
		{
			jkaia(localItemCollection, localItem);
		}
		return localItemCollection;
	}

	private void jkaia(LocalItemCollection p0, LocalItem p1)
	{
		LocalItem[] files = p1.GetFiles();
		LocalItem[] array = files;
		int num = 0;
		if (num != 0)
		{
			goto IL_0015;
		}
		goto IL_003f;
		IL_0015:
		LocalItem localItem = array[num];
		if (IsMatch(localItem.Path, FileSetMatchMode.MatchFile) && 0 == 0)
		{
			p0.Add(localItem);
		}
		num++;
		goto IL_003f;
		IL_005b:
		LocalItem[] array2;
		int num2;
		LocalItem localItem2 = array2[num2];
		int count = p0.Count;
		if (IsMatch(localItem2.Path, FileSetMatchMode.TraverseDirectory) && 0 == 0)
		{
			jkaia(p0, localItem2);
		}
		bool flag = count != p0.Count;
		if (ContainingDirectoriesIncluded && 0 == 0)
		{
			if ((flag ? true : false) || (EmptyDirectoriesIncluded && 0 == 0 && IsMatch(localItem2.Path, FileSetMatchMode.MatchDirectory)))
			{
				p0.rswev(count, localItem2);
			}
		}
		else if (((flag ? true : false) || EmptyDirectoriesIncluded) && IsMatch(localItem2.Path, FileSetMatchMode.MatchDirectory) && 0 == 0)
		{
			p0.rswev(count, localItem2);
		}
		num2++;
		goto IL_0132;
		IL_0132:
		if (num2 >= array2.Length)
		{
			return;
		}
		goto IL_005b;
		IL_003f:
		if (num < array.Length)
		{
			goto IL_0015;
		}
		LocalItem[] directories = p1.GetDirectories();
		array2 = directories;
		num2 = 0;
		if (num2 != 0)
		{
			goto IL_005b;
		}
		goto IL_0132;
	}

	private void wcmzo(string p0, TraversalMode p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("pattern", "Pattern cannot be null.");
		}
		if (p0.Trim().Length == 0 || 1 == 0)
		{
			throw new ArgumentException("Pattern cannot be empty.", "pattern");
		}
		if (wibzc(p0) && 0 == 0)
		{
			throw new ArgumentException("Pattern has to be a relative path or mask.", "pattern");
		}
		xnozj(p1);
	}

	private string miirf(string p0, TraversalMode p1, out bool p2, out string p3, out bool p4, out string p5, out bool p6)
	{
		string text = tgwtx(spcvu(p0));
		if ((ccazi(text, ".") ? true : false) || ccazi(text, ".."))
		{
			throw new ArgumentException("Pattern cannot contain special directory names ('.' and '..').", "pattern");
		}
		p6 = text.EndsWith("**", StringComparison.Ordinal);
		if (text.EndsWith(prtlv, StringComparison.Ordinal) && 0 == 0)
		{
			p2 = false;
			text = text.TrimEnd(dscmo);
			if (text.Length == 0 || 1 == 0)
			{
				throw new ArgumentException("Pattern cannot be empty.", "pattern");
			}
		}
		else
		{
			p2 = true;
		}
		switch (p1)
		{
		case TraversalMode.Recursive:
			p4 = true;
			if (p6 && 0 == 0)
			{
				p3 = (p5 = null);
			}
			else
			{
				p3 = (p5 = dscmo + "**");
			}
			p6 = true;
			break;
		case TraversalMode.NonRecursive:
			p4 = true;
			p3 = (p5 = null);
			break;
		case TraversalMode.MatchFilesShallow:
			p4 = false;
			p3 = (p5 = null);
			p6 = false;
			if (!p2 || 1 == 0)
			{
				throw new ArgumentException("Ambiguous usage of pattern and mode.", "pattern");
			}
			break;
		case TraversalMode.MatchFilesDeep:
			p4 = false;
			p3 = (p5 = null);
			p6 = false;
			if (p2)
			{
				text = yymgj(text);
				break;
			}
			throw new ArgumentException("Ambiguous usage of pattern and mode.", "pattern");
		default:
			throw new ArgumentException("Invalid directory traversal mode.", "mode");
		}
		return text;
	}

	private string yymgj(string p0)
	{
		int num = p0.LastIndexOf(dscmo);
		if (num < 0)
		{
			return brgjd.edcru("**{0}{1}", dscmo, p0);
		}
		return brgjd.edcru("{0}**{1}", p0.Substring(0, num + 1), p0.Substring(num));
	}

	private Regex idsra(string p0, bool p1, bool p2)
	{
		string text = Regex.Escape(prtlv);
		p0 = ofstl(p0, brgjd.edcru("**{0}**{0}", dscmo), brgjd.edcru("**{0}", dscmo));
		p0 = ofstl(p0, brgjd.edcru("{0}**{0}**", dscmo), brgjd.edcru("{0}**", dscmo));
		bool flag = p0.StartsWith("**" + dscmo, StringComparison.Ordinal);
		if (flag && 0 == 0)
		{
			p0 = p0.Remove(0, 3);
		}
		bool flag2 = p0.EndsWith(dscmo + "**", StringComparison.Ordinal);
		if (flag2 && 0 == 0)
		{
			p0 = p0.Remove(p0.Length - 3, 3);
		}
		p0 = Regex.Escape(p0);
		p0 = p0.Replace(brgjd.edcru("{0}\\*\\*{0}", text), brgjd.edcru("(({0}.*{0})|({0}))", text));
		p0 = p0.Replace("\\*\\*", ".*");
		p0 = p0.Replace("\\*", brgjd.edcru("[^{0}]*", text));
		p0 = p0.Replace("\\?", ".");
		p0 = ((!flag) ? ('^' + p0) : brgjd.edcru("((^)|({0})){1}", text, p0));
		p0 = ((p1 && 0 == 0) ? ((!flag2) ? (p0 + '$') : (p0 + text)) : ((!flag2) ? brgjd.edcru("{0}{1}?$", p0, text) : ((!p2) ? brgjd.edcru("{0}{1}", p0, text) : brgjd.edcru("(({0}{1})|({0}{1}?$))", p0, text))));
		return new Regex(p0, (!kvndz || 1 == 0) ? RegexOptions.IgnoreCase : RegexOptions.None);
	}

	private void xnozj(TraversalMode p0)
	{
		switch (p0)
		{
		case TraversalMode.Recursive:
		case TraversalMode.NonRecursive:
		case TraversalMode.MatchFilesShallow:
		case TraversalMode.MatchFilesDeep:
			return;
		}
		throw new ArgumentException("Invalid traversal mode.", "mode");
	}

	private void mbghw()
	{
		xailk.Clear();
		zncrf.Clear();
		nxmzm.Clear();
		jochm.Clear();
		azqgc.Clear();
		mxgwd.Clear();
		int num = 0;
		if (num != 0)
		{
			goto IL_0047;
		}
		goto IL_0074;
		IL_0047:
		vbood(sajpd[num].tfavb, sajpd[num].wunwl, p2: false);
		num++;
		goto IL_0074;
		IL_0074:
		if (num < sajpd.Count)
		{
			goto IL_0047;
		}
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_0087;
		}
		goto IL_00c0;
		IL_00c0:
		if (num2 >= vbctx.Count)
		{
			return;
		}
		goto IL_0087;
		IL_0087:
		ildwo(vbctx[num2].tfavb, vbctx[num2].wunwl, p2: false);
		num2++;
		goto IL_00c0;
	}

	private bool ccazi(string p0, string p1)
	{
		if (p0.Length < p1.Length)
		{
			return false;
		}
		if (p0.Length == p1.Length)
		{
			return p0.Equals(p1, StringComparison.Ordinal);
		}
		if ((!p0.EndsWith(dscmo + p1, StringComparison.Ordinal) || 1 == 0) && (!p0.StartsWith(p1 + dscmo, StringComparison.Ordinal) || 1 == 0))
		{
			return p0.IndexOf(dscmo + p1 + dscmo, StringComparison.Ordinal) >= 0;
		}
		return true;
	}

	private string tgwtx(string p0)
	{
		return ofstl(p0, "***", "**");
	}

	private string spcvu(string p0)
	{
		int num = 1;
		if (num == 0)
		{
			goto IL_0006;
		}
		goto IL_0022;
		IL_0006:
		p0 = p0.Replace(cyguh[num], dscmo);
		num++;
		goto IL_0022;
		IL_0022:
		if (num < cyguh.Length)
		{
			goto IL_0006;
		}
		return ofstl(p0, prtlv + prtlv, prtlv);
	}

	private static string ofstl(string p0, string p1, string p2)
	{
		string text = p0.Replace(p1, p2);
		while (text.Length != p0.Length)
		{
			p0 = text;
			text = p0.Replace(p1, p2);
		}
		return p0;
	}

	private bool zenlx(string p0)
	{
		if (p0.Length != 0 && 0 == 0)
		{
			return Array.IndexOf(cyguh, p0[p0.Length - 1]) >= 0;
		}
		return false;
	}

	private bool wibzc(string p0)
	{
		if (p0.Length != 0 && 0 == 0)
		{
			return Array.IndexOf(cyguh, p0[0]) >= 0;
		}
		return false;
	}

	private bool anwfx(string p0)
	{
		return !wibzc(p0);
	}
}
