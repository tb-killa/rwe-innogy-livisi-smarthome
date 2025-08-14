using System;
using System.IO;

namespace RWE.SmartHome.SHC.Core.FileDownload;

internal class DecoderBase64 : IDisposable
{
	private enum State
	{
		EatingStartTag,
		EatingContent,
		EatingEndTag
	}

	private readonly char[] buffer = new char[4];

	private FileStream file;

	public DecoderBase64(string fileName)
	{
		file = new FileStream(fileName, FileMode.Create);
	}

	public void DecodeAndWrite(byte[] receiveBuffer, int offset, int count)
	{
		int num = 0;
		State state = State.EatingStartTag;
		for (int i = offset; i < offset + count; i++)
		{
			switch (state)
			{
			case State.EatingStartTag:
				if (receiveBuffer[i] == 62)
				{
					state = State.EatingContent;
				}
				break;
			case State.EatingContent:
				if (receiveBuffer[i] == 60)
				{
					if (num != 0)
					{
						throw new InvalidDataException("End tag in middle of quadruple");
					}
					state = State.EatingEndTag;
					break;
				}
				buffer[num++] = (char)receiveBuffer[i];
				if (num == 4)
				{
					byte[] array = Convert.FromBase64CharArray(buffer, 0, num);
					file.Write(array, 0, array.Length);
					num = 0;
				}
				break;
			}
		}
	}

	public void Dispose()
	{
		if (file != null)
		{
			file.Close();
			GC.SuppressFinalize(this);
			file = null;
		}
	}
}
