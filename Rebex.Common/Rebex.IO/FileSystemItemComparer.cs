using System;
using System.Collections;
using onrkn;

namespace Rebex.IO;

public class FileSystemItemComparer : IComparer
{
	private readonly SortingOrder imguh;

	private readonly FileSystemItemComparerType guxog;

	public FileSystemItemComparer()
		: this(FileSystemItemComparerType.Name)
	{
	}

	public FileSystemItemComparer(FileSystemItemComparerType comparerType)
		: this(comparerType, SortingOrder.Ascending)
	{
	}

	public FileSystemItemComparer(FileSystemItemComparerType comparerType, SortingOrder sortOrder)
	{
		guxog = comparerType;
		imguh = sortOrder;
	}

	private static int cuigb(DateTime? p0, DateTime? p1)
	{
		if (!p0.HasValue || 1 == 0)
		{
			if (!p1.HasValue || 1 == 0)
			{
				return 0;
			}
			return -1;
		}
		if (!p1.HasValue || 1 == 0)
		{
			return 1;
		}
		return p0.Value.CompareTo(p1.Value);
	}

	public int Compare(object x, object y)
	{
		FileSystemItem fileSystemItem;
		FileSystemItem fileSystemItem2;
		switch (imguh)
		{
		case SortingOrder.Ascending:
			fileSystemItem = x as FileSystemItem;
			fileSystemItem2 = y as FileSystemItem;
			break;
		case SortingOrder.Descending:
			fileSystemItem = y as FileSystemItem;
			fileSystemItem2 = x as FileSystemItem;
			break;
		default:
			throw new InvalidOperationException(brgjd.edcru("Unsupported SortOrder type '{0}'.", imguh));
		}
		if (fileSystemItem == null || 1 == 0)
		{
			throw new ArgumentException("x is null or not an FileSystemItem object.");
		}
		if (fileSystemItem2 == null || 1 == 0)
		{
			throw new ArgumentException("y is null or not an FileSystemItem object.");
		}
		switch (guxog)
		{
		case FileSystemItemComparerType.Length:
			return fileSystemItem.Length.CompareTo(fileSystemItem2.Length);
		case FileSystemItemComparerType.CreationTime:
			return cuigb(fileSystemItem.CreationTime, fileSystemItem2.CreationTime);
		case FileSystemItemComparerType.LastWriteTime:
			return cuigb(fileSystemItem.LastWriteTime, fileSystemItem2.LastWriteTime);
		case FileSystemItemComparerType.LastAccessTime:
			return cuigb(fileSystemItem.LastAccessTime, fileSystemItem2.LastAccessTime);
		case FileSystemItemComparerType.Name:
			return fileSystemItem.Name.CompareTo(fileSystemItem2.Name);
		case FileSystemItemComparerType.FileType:
			if (fileSystemItem.IsDirectory != fileSystemItem2.IsDirectory)
			{
				if (!fileSystemItem.IsDirectory || 1 == 0)
				{
					return 1;
				}
				return -1;
			}
			if (fileSystemItem.IsFile != fileSystemItem2.IsFile)
			{
				if (!fileSystemItem.IsFile || 1 == 0)
				{
					return 1;
				}
				return -1;
			}
			if (fileSystemItem.IsLink != fileSystemItem2.IsLink)
			{
				if (!fileSystemItem.IsLink || 1 == 0)
				{
					return 1;
				}
				return -1;
			}
			return 0;
		default:
			throw new InvalidOperationException(brgjd.edcru("Unsupported FileSystemItemComparer comparison type '{0}'.", guxog));
		}
	}
}
