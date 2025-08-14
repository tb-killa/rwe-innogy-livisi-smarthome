using System;
using Rebex.IO;

namespace onrkn;

internal class ijwiq : FileSystemItem
{
	private FileSystemItem fjyun;

	private string eumso;

	private string zssqj;

	private FileSystemItem nkyia;

	public FileSystemItem Item => fjyun;

	public bool vvhki => fjyun is sjgua;

	public FileSystemItem nnyfv
	{
		get
		{
			return nkyia;
		}
		set
		{
			nkyia = value;
		}
	}

	public override string Name => eumso;

	public override string Path => zssqj;

	public override bool IsLink
	{
		get
		{
			if (!fjyun.IsLink || 1 == 0)
			{
				return nnyfv != null;
			}
			return true;
		}
	}

	public override long Length => fjyun.Length;

	public override bool IsFile => fjyun.IsFile;

	public override bool IsDirectory => fjyun.IsDirectory;

	public ijwiq(LocalItem item)
		: this(item, item.Name, item.FullPath)
	{
	}

	public ijwiq(FileSystemItem inner, string name, string path)
	{
		fjyun = inner;
		eumso = name;
		zssqj = path;
	}

	public static ijwiq eiuvm(string p0)
	{
		return new ijwiq(sjgua.tzjir(p0, p0), p0, p0);
	}

	protected override DateTime? GetLastWriteTime()
	{
		return fjyun.LastWriteTime;
	}

	protected override DateTime? GetLastAccessTime()
	{
		return fjyun.LastAccessTime;
	}

	protected override DateTime? GetCreationTime()
	{
		return fjyun.CreationTime;
	}

	public override string ToString()
	{
		return zssqj;
	}
}
