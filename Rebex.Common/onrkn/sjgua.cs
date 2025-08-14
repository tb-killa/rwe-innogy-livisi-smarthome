using System;
using Rebex.IO;

namespace onrkn;

internal class sjgua : FileSystemItem
{
	private string aerkb;

	private string jwzkh;

	private long iveco;

	private bool lwsjy;

	public override string Name => jwzkh;

	public override string Path => aerkb;

	public override long Length => iveco;

	public override bool IsFile => lwsjy;

	public override bool IsDirectory => !lwsjy;

	public override bool IsLink => false;

	private sjgua(bool isFile, string path, string name, long length)
	{
		aerkb = path;
		jwzkh = name;
		iveco = length;
		lwsjy = isFile;
	}

	public static sjgua nbvqk(string p0, string p1, long p2)
	{
		return new sjgua(isFile: true, p0, p1, p2);
	}

	public static sjgua tzjir(string p0, string p1)
	{
		return new sjgua(isFile: false, p0, p1, 0L);
	}

	public void qofsg(long p0)
	{
		iveco = p0;
	}

	protected override DateTime? GetLastWriteTime()
	{
		return null;
	}

	protected override DateTime? GetLastAccessTime()
	{
		return null;
	}

	protected override DateTime? GetCreationTime()
	{
		return null;
	}
}
