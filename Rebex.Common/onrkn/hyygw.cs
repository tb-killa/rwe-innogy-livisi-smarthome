using System;
using System.IO;
using Rebex.IO;

namespace onrkn;

internal abstract class hyygw
{
	private FileSet ralti;

	private string abqqr;

	public bool begld;

	public bool zgxzt;

	public string hlzcy;

	private bool tealc;

	public bool zbbnn
	{
		get
		{
			return tealc;
		}
		private set
		{
			tealc = value;
		}
	}

	public string dfgud
	{
		get
		{
			string basePath = abqqr;
			if (basePath == null || 1 == 0)
			{
				basePath = ralti.BasePath;
			}
			return basePath;
		}
		set
		{
			abqqr = value;
		}
	}

	public FileSet mkziv => ralti;

	protected abstract char[] uorhi { get; }

	protected abstract string uykvy { get; }

	protected hyygw(FileSet set)
	{
		ralti = set;
	}

	protected hyygw(bool fromOldApi)
	{
		zbbnn = fromOldApi;
	}

	public bool omemp(string p0, char[] p1, FileSetMatchMode p2)
	{
		if (p0.Length <= dfgud.Length && 0 == 0)
		{
			return p2 switch
			{
				FileSetMatchMode.MatchFile => true, 
				FileSetMatchMode.MatchDirectory => true, 
				FileSetMatchMode.TraverseDirectory => ralti.IsMatch(".", p2), 
				_ => throw hifyx.nztrs("mode", p2, "Invalid enum value."), 
			};
		}
		string relativePath = p0.Substring(dfgud.Length).TrimStart(p1);
		return ralti.IsMatch(relativePath, p2);
	}

	protected void hfiod(string p0, TraversalMode p1, TransferAction p2)
	{
		char[] array = kpfse(p2);
		string p3 = cvywk.wbzis(p0, array, p2 == TransferAction.Uploading, out var p4, out var p5);
		if ((p1 == TraversalMode.MatchFilesDeep || p1 == TraversalMode.MatchFilesShallow) && ((p4 ? true : false) || p5))
		{
			throw new ArgumentException("Ambiguous usage of path and mode.", "pattern");
		}
		p3 = cvywk.rwapr(p3, "**", "*");
		ralti = new FileSet(p3, array);
		int num = p0.IndexOfAny(dahxy.fmacz);
		if (num >= 0)
		{
			int num2 = p3.LastIndexOf(array[0]);
			string pattern;
			if (num2 < 0)
			{
				pattern = p3;
				p3 = ".";
			}
			else
			{
				if (num < num2)
				{
					throw new ArgumentException("Illegal use of wildcards in path.", "pattern");
				}
				pattern = p3.Substring(num2 + 1);
				p3 = (((num2 != 0) ? true : false) ? p3.Substring(0, num2) : p3.Substring(0, 1));
			}
			ralti.BasePath = p3;
			ralti.Include(pattern, p1);
			if (p1 == TraversalMode.NonRecursive && zbbnn && 0 == 0 && (p2 == TransferAction.Uploading || p2 == TransferAction.Downloading))
			{
				ralti.EmptyDirectoriesIncluded = false;
			}
			return;
		}
		switch (p1)
		{
		case TraversalMode.NonRecursive:
			switch (p2)
			{
			case TransferAction.Uploading:
			case TransferAction.Downloading:
				if (zbbnn && 0 == 0)
				{
					ralti.Include("*", TraversalMode.NonRecursive);
					ralti.EmptyDirectoriesIncluded = false;
				}
				if ((!p5 || 1 == 0) && (!p4 || 1 == 0))
				{
					begld = true;
				}
				if (!p5 || 1 == 0)
				{
					zgxzt = !xtpmk(p3, p2);
				}
				break;
			case TransferAction.Deleting:
				zgxzt = (!xtpmk(p3, p2) || 1 == 0) && !p5;
				if ((!p5 || 1 == 0) && (!p4 || 1 == 0))
				{
					begld = true;
				}
				break;
			case TransferAction.Listing:
				if (zbbnn && 0 == 0)
				{
					ralti.Include("*", TraversalMode.NonRecursive);
				}
				else
				{
					zgxzt = true;
				}
				if ((!p5 || 1 == 0) && (!p4 || 1 == 0))
				{
					begld = true;
				}
				break;
			default:
				throw hifyx.nztrs("action", p2, "Invalid enum value.");
			}
			break;
		case TraversalMode.Recursive:
			ralti.Include("*", TraversalMode.Recursive);
			switch (p2)
			{
			case TransferAction.Uploading:
			case TransferAction.Downloading:
				if ((!p5 || 1 == 0) && (!p4 || 1 == 0))
				{
					begld = true;
				}
				if (!p5 || 1 == 0)
				{
					zgxzt = !xtpmk(p3, p2);
				}
				break;
			case TransferAction.Deleting:
				zgxzt = (!xtpmk(p3, p2) || 1 == 0) && !p5;
				if ((!p5 || 1 == 0) && (!p4 || 1 == 0))
				{
					begld = true;
				}
				break;
			case TransferAction.Listing:
				if ((!p5 || 1 == 0) && (!p4 || 1 == 0))
				{
					begld = true;
				}
				break;
			default:
				throw hifyx.nztrs("action", p2, "Invalid enum value.");
			}
			break;
		case TraversalMode.MatchFilesShallow:
		case TraversalMode.MatchFilesDeep:
		{
			int num3 = p3.LastIndexOf(array[0]);
			if (num3 < 0)
			{
				ralti.BasePath = ".";
				ralti.Include(p3, p1);
			}
			else if (num3 == 0 || 1 == 0)
			{
				ralti.BasePath = p3.Substring(0, 1);
				ralti.Include(p3.Substring(num3 + 1), p1);
			}
			else
			{
				ralti.BasePath = p3.Substring(0, num3);
				ralti.Include(p3.Substring(num3 + 1), p1);
			}
			break;
		}
		default:
			throw hifyx.nztrs("mode", p1, "Invalid enum value.");
		}
	}

	private bool xtpmk(string p0, TransferAction p1)
	{
		if (p1 == TransferAction.Uploading)
		{
			if (p0.Length == 1 && p0[0] == dahxy.ymzqe)
			{
				return true;
			}
			p0 = vtdxm.smjda(p0);
			return p0 == Path.GetPathRoot(p0);
		}
		return p0 == uykvy;
	}

	protected char[] kpfse(TransferAction p0)
	{
		if (p0 != TransferAction.Uploading)
		{
			return uorhi;
		}
		return dahxy.awabp;
	}
}
