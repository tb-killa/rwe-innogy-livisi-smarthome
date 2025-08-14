using System;
using System.Collections.Generic;
using System.IO;
using onrkn;

namespace Rebex.IO;

public class LocalItem : FileSystemItem
{
	private bool yrqnr;

	private long jyrwr;

	private DateTime? wqpni;

	private DateTime? ldlkv;

	private DateTime? rubqu;

	private FileAttributes hetrt;

	private bool cvkyq;

	private bool sdgqp;

	private bool rhvtt;

	private bool qzszj;

	private string mqbap;

	private string zjisc;

	private string zgxtg;

	public bool Exists => yrqnr;

	public override long Length
	{
		get
		{
			wzizo();
			return jyrwr;
		}
	}

	public override bool IsFile
	{
		get
		{
			wzizo();
			qjjdt();
			return cvkyq;
		}
	}

	public override bool IsDirectory
	{
		get
		{
			wzizo();
			qjjdt();
			return sdgqp;
		}
	}

	public override bool IsLink
	{
		get
		{
			wzizo();
			return rhvtt;
		}
	}

	public override string Name => mqbap;

	public override string Path => zjisc;

	public string FullPath => zgxtg;

	public FileAttributes Attributes => hetrt;

	public LocalItem(string path)
	{
		if (path == null || 1 == 0)
		{
			throw new ArgumentNullException("path", "Path cannot be null.");
		}
		if (path.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("Path cannot be empty.", "path");
		}
		char? p = null;
		string text = path.TrimEnd(dahxy.awabp);
		if (text.Length > 0 && text.Length < path.Length && text[text.Length - 1] != dahxy.njlzv)
		{
			p = dahxy.ymzqe;
			path = text;
		}
		FileSystemInfo fileSystemInfo;
		try
		{
			fileSystemInfo = new FileInfo(path);
		}
		catch (NotSupportedException)
		{
			npfps(path);
			return;
		}
		if (!fileSystemInfo.Exists || 1 == 0)
		{
			fileSystemInfo = new DirectoryInfo(path);
		}
		ikkuv(fileSystemInfo, path, p);
	}

	private LocalItem(FileSystemInfo info, string path)
	{
		ikkuv(info, path, null);
	}

	private void npfps(string p0)
	{
		yrqnr = false;
		int num = p0.LastIndexOfAny(new char[2] { '\\', '/' });
		mqbap = p0.Substring(num + 1);
		zjisc = p0;
		string text = System.IO.Path.DirectorySeparatorChar.ToString();
		string text2 = "";
		text2 = "\\";
		zgxtg = text2 + text + p0;
		zgxtg = zgxtg.Replace("\\\\", text).Replace("\\/", text).Replace("/\\", text);
	}

	private void ikkuv(FileSystemInfo p0, string p1, char? p2)
	{
		FileInfo fileInfo = p0 as FileInfo;
		mqbap = p0.Name;
		zjisc = p1 + p2;
		zgxtg = p0.FullName + p2;
		yrqnr = p0.Exists;
		char? c = p2;
		if (((c.HasValue ? true : false) ? new int?(c.GetValueOrDefault()) : ((int?)null)).HasValue && 0 == 0 && fileInfo != null && 0 == 0)
		{
			yrqnr = false;
		}
		if (yrqnr && 0 == 0)
		{
			if (fileInfo != null && 0 == 0)
			{
				cvkyq = true;
				sdgqp = false;
				jyrwr = fileInfo.Length;
			}
			else
			{
				cvkyq = false;
				sdgqp = true;
				jyrwr = 0L;
			}
			rhvtt = (p0.Attributes & FileAttributes.ReparsePoint) == FileAttributes.ReparsePoint;
			wqpni = p0.LastWriteTime;
			ldlkv = p0.LastAccessTime;
			rubqu = p0.CreationTime;
			hetrt = p0.Attributes;
		}
	}

	private void wzizo()
	{
		if (!yrqnr || 1 == 0)
		{
			throw new InvalidOperationException("Local item doesn't exist.");
		}
	}

	private void qjjdt()
	{
		if (rhvtt && 0 == 0 && !qzszj)
		{
			qzszj = true;
		}
	}

	protected override DateTime? GetLastWriteTime()
	{
		wzizo();
		return wqpni;
	}

	public void SetLastWriteTime(DateTime date)
	{
		wzizo();
		iicsn(null, null, date);
		wqpni = date;
	}

	protected override DateTime? GetLastAccessTime()
	{
		wzizo();
		return ldlkv;
	}

	public void SetLastAccessTime(DateTime date)
	{
		wzizo();
		iicsn(null, date, null);
		ldlkv = date;
	}

	protected override DateTime? GetCreationTime()
	{
		wzizo();
		return rubqu;
	}

	public void SetCreationTime(DateTime date)
	{
		wzizo();
		iicsn(date, null, null);
		rubqu = date;
	}

	internal void bgruk(string p0)
	{
		zjisc = p0;
	}

	public override string ToString()
	{
		return zgxtg;
	}

	public void Delete()
	{
		wzizo();
		if (cvkyq && 0 == 0)
		{
			File.Delete(zgxtg);
		}
		else
		{
			Directory.Delete(zgxtg);
		}
	}

	public LocalItem[] GetFiles()
	{
		wzizo();
		if (!IsDirectory || 1 == 0)
		{
			throw vtdxm.iwvtz("The item is not a directory: '{0}'.", FullPath);
		}
		List<LocalItem> list = new List<LocalItem>();
		FileInfo[] files = new DirectoryInfo(zgxtg).GetFiles();
		FileInfo[] array = files;
		int num = 0;
		if (num != 0)
		{
			goto IL_0056;
		}
		goto IL_00a1;
		IL_0056:
		FileInfo fileInfo = array[num];
		LocalItem localItem = new LocalItem(fileInfo, System.IO.Path.Combine(zjisc, fileInfo.Name));
		if (localItem.Exists && 0 == 0 && localItem.IsFile && 0 == 0)
		{
			list.Add(localItem);
		}
		num++;
		goto IL_00a1;
		IL_00a1:
		if (num < array.Length)
		{
			goto IL_0056;
		}
		return list.ToArray();
	}

	public LocalItem[] GetDirectories()
	{
		wzizo();
		if (!IsDirectory || 1 == 0)
		{
			throw vtdxm.iwvtz("The item is not a directory: '{0}'.", FullPath);
		}
		List<LocalItem> list = new List<LocalItem>();
		DirectoryInfo[] directories = new DirectoryInfo(zgxtg).GetDirectories();
		DirectoryInfo[] array = directories;
		int num = 0;
		if (num != 0)
		{
			goto IL_0056;
		}
		goto IL_00a1;
		IL_0056:
		DirectoryInfo directoryInfo = array[num];
		LocalItem localItem = new LocalItem(directoryInfo, System.IO.Path.Combine(zjisc, directoryInfo.Name));
		if (localItem.Exists && 0 == 0 && localItem.IsDirectory && 0 == 0)
		{
			list.Add(localItem);
		}
		num++;
		goto IL_00a1;
		IL_00a1:
		if (num < array.Length)
		{
			goto IL_0056;
		}
		return list.ToArray();
	}

	public long ComputeCrc32()
	{
		wzizo();
		if (!IsFile || 1 == 0)
		{
			throw vtdxm.pvoqa("The item is not a file: '{0}'.", FullPath);
		}
		mecsr mecsr = new mecsr();
		Stream stream = vtdxm.prsfm(zgxtg);
		try
		{
			mecsr.cvnqk(stream);
		}
		finally
		{
			if (stream != null && 0 == 0)
			{
				((IDisposable)stream).Dispose();
			}
		}
		return mecsr.hxtqz;
	}

	public static Checksum GetChecksum(string path, ChecksumAlgorithm algorithm)
	{
		return GetChecksum(path, algorithm, 0L, -1L);
	}

	public static Checksum GetChecksum(string path, ChecksumAlgorithm algorithm, long offset, long count)
	{
		return nqamt(path, algorithm, offset, count, null);
	}

	internal static Checksum nqamt(string p0, ChecksumAlgorithm p1, long p2, long p3, vtdxm.urozv p4)
	{
		bpkgq.xqnvu(p1, "algorithm");
		FileStream fileStream = ((p4 != null) ? vtdxm.xorlw(p0, FileMode.Open, FileAccess.Read, FileShare.Read, p4) : vtdxm.vswch(p0, FileMode.Open, FileAccess.Read, FileShare.Read));
		try
		{
			if (p2 > 0)
			{
				fileStream.Seek(p2, SeekOrigin.Begin);
			}
			return xudmp(fileStream, p1, p3);
		}
		finally
		{
			fileStream.Close();
		}
	}

	internal static Checksum xudmp(Stream p0, ChecksumAlgorithm p1, long p2)
	{
		byte[] checksum = Checksum.nrrak(p0, p1, null, (ulong)((p2 < 0) ? 0 : p2), 0u);
		return new Checksum(p1, checksum);
	}

	private void iicsn(DateTime? p0, DateTime? p1, DateTime? p2)
	{
		dvfml.toapj[] lpCreationTime = ((!p0.HasValue) ? null : new dvfml.toapj[1] { dvfml.pxrli(p0.Value) });
		dvfml.toapj[] lpLastAccessTime = ((!p1.HasValue) ? null : new dvfml.toapj[1] { dvfml.pxrli(p1.Value) });
		dvfml.toapj[] lpLastWriteTime = ((!p2.HasValue) ? null : new dvfml.toapj[1] { dvfml.pxrli(p2.Value) });
		IntPtr intPtr = dvfml.CreateFile(zgxtg, 1073741824u, 1u, IntPtr.Zero, 3u, 128u, IntPtr.Zero);
		try
		{
			if (dvfml.zhwer(intPtr) && 0 == 0)
			{
				throw new IOException("Cannot open file for setting file creation, last write and/or last access times.");
			}
			if (!dvfml.SetFileTime(intPtr, lpCreationTime, lpLastAccessTime, lpLastWriteTime) || 1 == 0)
			{
				throw new IOException("Cannot set file creation, last write and/or last access times.");
			}
		}
		finally
		{
			dvfml.CloseHandle(intPtr);
		}
	}
}
