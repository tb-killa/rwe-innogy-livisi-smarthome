using System;

namespace Rebex.Net;

public class SshExtendedDataReceivedEventArgs : EventArgs
{
	private readonly byte[] yamdy;

	private readonly int wiawn;

	public int Length => yamdy.Length;

	public int TypeCode => wiawn;

	public byte[] GetData()
	{
		return yamdy;
	}

	internal SshExtendedDataReceivedEventArgs(int code, byte[] buffer, int offset, int count)
	{
		wiawn = code;
		yamdy = new byte[count];
		Array.Copy(buffer, offset, yamdy, 0, count);
	}
}
