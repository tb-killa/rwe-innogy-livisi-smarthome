using System;

namespace Rebex.IO;

[Flags]
public enum ItemDateTimes
{
	None = 0,
	CreationTime = 1,
	LastWriteTime = 2,
	LastAccessTime = 4,
	All = 7
}
