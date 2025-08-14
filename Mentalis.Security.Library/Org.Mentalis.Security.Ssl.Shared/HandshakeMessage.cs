using System;

namespace Org.Mentalis.Security.Ssl.Shared;

internal class HandshakeMessage
{
	public byte[] fragment;

	public HandshakeType type;

	public HandshakeMessage(HandshakeType type, byte[] bytes)
	{
		this.type = type;
		fragment = bytes;
	}

	public byte[] ToBytes()
	{
		int num = fragment.Length;
		byte[] array = new byte[num + 4];
		array[0] = (byte)type;
		array[1] = (byte)(num / 65536);
		array[2] = (byte)(num % 65536 / 256);
		array[3] = (byte)(num % 256);
		Array.Copy(fragment, 0, array, 4, num);
		return array;
	}
}
