using System;

namespace WebSocketLibrary.Handlers.Handshake;

public struct StringToken
{
	public int firstIndex;

	public int lastIndex;

	public StringToken(int firstIndex, int lastIndex)
	{
		this.firstIndex = firstIndex;
		this.lastIndex = lastIndex;
	}

	public int GetLength()
	{
		return Math.Max(0, lastIndex - firstIndex + 1);
	}

	public string GetSubstring(string text)
	{
		return text.Substring(firstIndex, GetLength());
	}
}
