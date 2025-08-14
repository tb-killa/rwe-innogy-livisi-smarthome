using System;
using System.IO;
using System.Text;

namespace SmartHome.Common.Generic.Contracts.BackendShc;

internal class Base64Stream : Stream
{
	private Stream innerStream;

	private Mode streamMode;

	public override bool CanRead => streamMode == Mode.Decode;

	public override bool CanSeek => false;

	public override bool CanWrite => streamMode == Mode.Encode;

	public override long Length
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override long Position
	{
		get
		{
			throw new NotImplementedException();
		}
		set
		{
			throw new NotImplementedException();
		}
	}

	internal Base64Stream(Stream stream, Mode mode)
	{
		innerStream = stream;
		streamMode = mode;
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		if (streamMode != Mode.Decode)
		{
			throw new NotImplementedException();
		}
		if (count < 0 || buffer == null || buffer.Length < count || offset < 0 || offset + count > buffer.Length)
		{
			throw new ArgumentException();
		}
		if (count == 0)
		{
			return 0;
		}
		int num = count * 4 / 3 / 4 * 4;
		byte[] array = new byte[num];
		int i;
		int num2;
		for (i = 0; i < array.Length; array[i] = (byte)num2, i++)
		{
			num2 = innerStream.ReadByte();
			switch (num2)
			{
			case 61:
			{
				array[i++] = (byte)num2;
				while (i % 4 != 0)
				{
					array[i++] = (byte)innerStream.ReadByte();
				}
				byte[] array2 = Convert.FromBase64String(Encoding.UTF8.GetString(array, 0, i));
				Buffer.BlockCopy(array2, 0, buffer, offset, array2.Length);
				return array2.Length;
			}
			default:
				continue;
			case -1:
				break;
			}
			break;
		}
		if (i == 0)
		{
			return 0;
		}
		byte[] array3 = Convert.FromBase64String(Encoding.UTF8.GetString(array, 0, i));
		Buffer.BlockCopy(array3, 0, buffer, offset, array3.Length);
		return array3.Length;
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		if (streamMode != Mode.Encode)
		{
			throw new NotImplementedException();
		}
		if (count < 0 || buffer == null || buffer.Length < count || offset < 0 || offset + count > buffer.Length)
		{
			throw new ArgumentException();
		}
		if (count != 0)
		{
			string s = Convert.ToBase64String(buffer, offset, count);
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			innerStream.Write(bytes, 0, bytes.Length);
		}
	}

	public override void Flush()
	{
		innerStream.Flush();
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw new NotImplementedException();
	}

	public override void SetLength(long value)
	{
		throw new NotImplementedException();
	}
}
