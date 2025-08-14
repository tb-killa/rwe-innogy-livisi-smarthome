using System;
using System.IO;

namespace Org.Mentalis.Security.Ssl.Shared;

internal class XBuffer : MemoryStream
{
	public void RemoveXBytes(int aByteCount)
	{
		if (aByteCount > Length)
		{
			throw new ArgumentException("Not enough data in buffer");
		}
		if (aByteCount == Length)
		{
			SetLength(0L);
			return;
		}
		byte[] buffer = GetBuffer();
		Array.Copy(buffer, aByteCount, buffer, 0, (int)Length - aByteCount);
		SetLength(Length - aByteCount);
	}
}
