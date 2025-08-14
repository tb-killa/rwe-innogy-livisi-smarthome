using System;

namespace Rebex.IO;

public abstract class FileSystemItem
{
	public abstract long Length { get; }

	public DateTime? LastWriteTime => GetLastWriteTime();

	public DateTime? LastAccessTime => GetLastAccessTime();

	public DateTime? CreationTime => GetCreationTime();

	public abstract bool IsFile { get; }

	public abstract bool IsDirectory { get; }

	public abstract bool IsLink { get; }

	public abstract string Name { get; }

	public abstract string Path { get; }

	protected abstract DateTime? GetLastWriteTime();

	protected abstract DateTime? GetLastAccessTime();

	protected abstract DateTime? GetCreationTime();
}
