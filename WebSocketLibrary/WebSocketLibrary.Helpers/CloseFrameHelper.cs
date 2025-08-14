using System;
using System.Text;

namespace WebSocketLibrary.Helpers;

public static class CloseFrameHelper
{
	public static ulong GetPayloadLength(ushort code, string text)
	{
		ulong num = 0uL;
		if (code > 0)
		{
			num += 2;
			if (!string.IsNullOrEmpty(text))
			{
				num += (ulong)Encoding.UTF8.GetByteCount(text);
			}
		}
		return num;
	}

	public static ushort GetCodeFromPayload(ArraySegment<byte> data)
	{
		if (data.Count < 2)
		{
			return 0;
		}
		return ConverterHelper.GetUShortFromBuffer(data.Array, data.Offset);
	}

	public static string GetTextFromPayload(ArraySegment<byte> data)
	{
		if (data.Count < 3)
		{
			return string.Empty;
		}
		return Encoding.UTF8.GetString(data.Array, data.Offset + 2, data.Count - 2);
	}
}
