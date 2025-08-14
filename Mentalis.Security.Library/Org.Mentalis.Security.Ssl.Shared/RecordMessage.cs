using System;

namespace Org.Mentalis.Security.Ssl.Shared;

internal class RecordMessage
{
	public ContentType contentType;

	public byte[] fragment;

	public ushort length;

	public MessageType messageType;

	public ProtocolVersion version;

	public RecordMessage(MessageType messageType, ContentType contentType, ProtocolVersion version, byte[] bytes)
	{
		this.messageType = messageType;
		this.contentType = contentType;
		this.version = version;
		if (bytes != null)
		{
			fragment = bytes;
		}
		else
		{
			fragment = new byte[0];
		}
		length = (ushort)fragment.Length;
	}

	public RecordMessage(byte[] bytes, int offset)
	{
		if (bytes == null)
		{
			throw new ArgumentNullException();
		}
		if (offset < 0 || offset >= bytes.Length)
		{
			throw new ArgumentException();
		}
		messageType = MessageType.Encrypted;
		contentType = (ContentType)bytes[offset];
		version = new ProtocolVersion(bytes[offset + 1], bytes[offset + 2]);
		length = (ushort)(bytes[offset + 3] * 256 + bytes[offset + 4]);
		fragment = new byte[length];
		Array.Copy(bytes, offset + 5, fragment, 0, length);
	}

	public byte[] ToBytes()
	{
		byte[] array = new byte[fragment.Length + 5];
		array[0] = (byte)contentType;
		array[1] = version.major;
		array[2] = version.minor;
		array[3] = (byte)(length / 256);
		array[4] = (byte)(length % 256);
		Array.Copy(fragment, 0, array, 5, fragment.Length);
		return array;
	}
}
