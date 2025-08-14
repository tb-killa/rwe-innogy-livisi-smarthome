using System;
using System.IO;
using Rebex.IO;

namespace onrkn;

internal static class vtdxm
{
	public delegate Exception urozv(string message, Exception innerException);

	public static string kilhc(Stream p0)
	{
		if (p0 is FileStream fileStream && 0 == 0 && (!string.IsNullOrEmpty(fileStream.Name) || 1 == 0) && fileStream.Name[0] != '[')
		{
			return smjda(fileStream.Name);
		}
		return null;
	}

	public static string zkrhb(Stream p0)
	{
		if (p0 is FileStream fileStream && 0 == 0 && (!string.IsNullOrEmpty(fileStream.Name) || 1 == 0) && fileStream.Name[0] != '[')
		{
			return uisrq(fileStream.Name);
		}
		return null;
	}

	public static string uisrq(string p0)
	{
		return Path.GetFileName(p0);
	}

	public static string sfvzf(string p0)
	{
		if (p0.Length > 3)
		{
			int num = p0.LastIndexOf('[');
			if (num > 0 && num < p0.Length - 2)
			{
				num++;
				int num2 = p0.LastIndexOf(']');
				if (num2 > num && (num2 == p0.Length - 1 || p0[num2 + 1] == '.') && dahxy.crqjb(p0.Substring(num, num2 - num), out var p1) && 0 == 0 && p1 >= 0)
				{
					return brgjd.edcru("{0}{1}{2}", p0.Substring(0, num), p1 + 1, p0.Substring(num2));
				}
			}
		}
		int num3 = p0.LastIndexOf('.');
		if (num3 > 0)
		{
			return brgjd.edcru("{0}[1]{1}", p0.Substring(0, num3), p0.Substring(num3));
		}
		return p0 + "[1]";
	}

	public static string smjda(string p0)
	{
		try
		{
			return Path.GetFullPath(p0);
		}
		catch (NotSupportedException)
		{
			throw new ArgumentException("Unsupported path format.", "path");
		}
	}

	public static bool npufh(string p0)
	{
		return File.Exists(p0);
	}

	public static bool qjbvo(string p0)
	{
		return Directory.Exists(dahxy.tfhfm(p0));
	}

	public static void ceona(string p0)
	{
		Directory.CreateDirectory(p0);
	}

	public static FileStream bolpl(string p0)
	{
		return vswch(p0, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
	}

	public static FileStream prsfm(string p0)
	{
		return vswch(p0, FileMode.Open, FileAccess.Read, FileShare.Read);
	}

	public static FileStream jtqes(string p0, urozv p1)
	{
		return xorlw(p0, FileMode.Open, FileAccess.Read, FileShare.Read, p1);
	}

	public static FileStream namiu(string p0, urozv p1)
	{
		return xorlw(p0, FileMode.Create, FileAccess.ReadWrite, FileShare.Read, p1);
	}

	public static FileStream vswch(string p0, FileMode p1, FileAccess p2, FileShare p3)
	{
		return new gnell(p0, p1, p2, p3);
	}

	public static FileStream xorlw(string p0, FileMode p1, FileAccess p2, FileShare p3, urozv p4)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("path", "Path cannot be null.");
		}
		if (p0.Length > 0)
		{
			if ((p0.Equals(".", StringComparison.OrdinalIgnoreCase) ? true : false) || p0.Equals("..", StringComparison.OrdinalIgnoreCase))
			{
				throw p4("Local path is not a file but a directory.", null);
			}
			if ((p0.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.OrdinalIgnoreCase) ? true : false) || (p0.EndsWith(Path.DirectorySeparatorChar + ".", StringComparison.OrdinalIgnoreCase) ? true : false) || p0.EndsWith(Path.DirectorySeparatorChar + "..", StringComparison.OrdinalIgnoreCase))
			{
				throw p4("Local path is not a file but a directory.", null);
			}
			if (Path.DirectorySeparatorChar != Path.AltDirectorySeparatorChar && ((p0.EndsWith(Path.AltDirectorySeparatorChar.ToString(), StringComparison.OrdinalIgnoreCase) ? true : false) || (p0.EndsWith(Path.AltDirectorySeparatorChar + ".", StringComparison.OrdinalIgnoreCase) ? true : false) || p0.EndsWith(Path.AltDirectorySeparatorChar + "..", StringComparison.OrdinalIgnoreCase)))
			{
				throw p4("Local path is not a file but a directory.", null);
			}
		}
		try
		{
			return new gnell(p0, p1, p2, p3);
		}
		catch (DirectoryNotFoundException innerException)
		{
			if (Directory.Exists(p0) && 0 == 0)
			{
				throw p4("Local path is not a file but a directory.", innerException);
			}
			throw p4("Local path is not a file path or a part of the path doesn't exists.", innerException);
		}
		catch (UnauthorizedAccessException innerException2)
		{
			if (Directory.Exists(p0) && 0 == 0)
			{
				throw p4("Local path is not a file but a directory.", innerException2);
			}
			throw p4("Local path is not a file path or you do not have permission to access it.", innerException2);
		}
		catch (IOException innerException3)
		{
			if (Directory.Exists(p0) && 0 == 0)
			{
				throw p4("Local path is not a file but a directory.", innerException3);
			}
			throw;
		}
	}

	public static Exception iwvtz(string p0, params object[] p1)
	{
		return new DirectoryNotFoundException(brgjd.edcru(p0, p1));
	}

	public static Exception pvoqa(string p0, params object[] p1)
	{
		return new FileNotFoundException(brgjd.edcru(p0, p1));
	}

	public static Exception xwfyr(string p0, params object[] p1)
	{
		return new IOException(brgjd.edcru(p0, p1));
	}

	public static Exception jpcyx(string p0, params object[] p1)
	{
		return new PathTooLongException(brgjd.edcru(p0, p1));
	}

	public static void tjuce(string p0, FileSystemItem p1, ItemDateTimes p2)
	{
		if (p2 != ItemDateTimes.None && 0 == 0)
		{
			LocalItem localItem = new LocalItem(p0);
			if ((p2 & ItemDateTimes.CreationTime) != ItemDateTimes.None && 0 == 0 && p1.CreationTime.HasValue && 0 == 0)
			{
				localItem.SetCreationTime(p1.CreationTime.Value);
			}
			if ((p2 & ItemDateTimes.LastWriteTime) != ItemDateTimes.None && 0 == 0 && p1.LastWriteTime.HasValue && 0 == 0)
			{
				localItem.SetLastWriteTime(p1.LastWriteTime.Value);
			}
			if ((p2 & ItemDateTimes.LastAccessTime) != ItemDateTimes.None && 0 == 0 && p1.LastAccessTime.HasValue && 0 == 0)
			{
				localItem.SetLastAccessTime(p1.LastAccessTime.Value);
			}
		}
	}
}
