using System;

namespace Rebex.IO.FileSystem;

[Flags]
public enum NodeContentAccess
{
	Read = 1,
	Write = 2,
	ReadWrite = 3
}
