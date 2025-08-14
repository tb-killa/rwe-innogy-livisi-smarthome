using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace SmartHome.SHC.API.Serialization.Json;

internal class JsonReader : IDisposable
{
	private readonly TextReader reader;

	private bool disposed;

	public JsonReader(TextReader input)
	{
		reader = input;
	}

	public JsonReader(Stream input)
		: this(new StreamReader(input, Encoding.UTF8))
	{
	}

	public JsonReader(string input)
		: this(new StringReader(input))
	{
	}

	public void SkipWhiteSpaces()
	{
		while (true)
		{
			bool flag = true;
			char c = Peek();
			if (!char.IsWhiteSpace(c))
			{
				break;
			}
			reader.Read();
		}
	}

	public int ReadInt32()
	{
		string text = ReadNumericValue();
		return (text != null) ? Convert.ToInt32(text) : 0;
	}

	public bool ReadBoolean()
	{
		StringBuilder stringBuilder = new StringBuilder(25);
		while (true)
		{
			bool flag = true;
			char c = Peek();
			if (c == ',' || c == '{' || c == '}')
			{
				break;
			}
			c = Read();
			stringBuilder.Append(c);
		}
		return bool.Parse(stringBuilder.ToString());
	}

	public string ReadString()
	{
		AssertAndConsume('"');
		StringBuilder stringBuilder = new StringBuilder(25);
		bool flag = false;
		while (true)
		{
			bool flag2 = true;
			char c = Read();
			if (c == '\\' && !flag)
			{
				flag = true;
				continue;
			}
			if (flag)
			{
				stringBuilder.Append(FromEscaped(c));
				flag = false;
				continue;
			}
			if (c == '"')
			{
				break;
			}
			stringBuilder.Append(c);
		}
		string text = stringBuilder.ToString();
		return (text == "null") ? null : text;
	}

	public double ReadDouble()
	{
		string text = ReadNumericValue();
		return (text == null) ? 0.0 : Convert.ToDouble(text);
	}

	public DateTime ReadDateTime()
	{
		string text = ReadString();
		return (text == null) ? DateTime.MinValue : DateTime.ParseExact(text, "G", CultureInfo.InvariantCulture);
	}

	public char ReadChar()
	{
		string text = ReadString();
		if (text == null)
		{
			return '\0';
		}
		if (text.Length > 1)
		{
			throw new JsonException("Expecting a character, but got a string");
		}
		return text[0];
	}

	public virtual int ReadEnum()
	{
		return ReadInt32();
	}

	public virtual string[] ReadList()
	{
		StringBuilder stringBuilder = new StringBuilder();
		char c;
		while ((c = Read()) != '[')
		{
			Read();
		}
		for (c = Read(); c != ']'; c = Read())
		{
			stringBuilder.Append(c);
		}
		stringBuilder.Replace("\"", "");
		return stringBuilder.ToString().Split(',');
	}

	public virtual long ReadInt64()
	{
		string text = ReadNumericValue();
		return (text == null) ? 0 : Convert.ToInt64(text);
	}

	public virtual float ReadFloat()
	{
		string text = ReadNumericValue();
		return (text == null) ? 0f : Convert.ToSingle(text);
	}

	public virtual short ReadInt16()
	{
		string text = ReadNumericValue();
		return (short)((text != null) ? Convert.ToInt16(text) : 0);
	}

	public virtual decimal ReadDecimal()
	{
		string text = ReadNumericValue();
		return (text == null) ? 0m : Convert.ToDecimal(text);
	}

	public char Peek()
	{
		int c = reader.Peek();
		return ValidateChar(c);
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected internal void AssertAndConsume(char character)
	{
		char c = Read();
		if (c != character)
		{
			throw new JsonException($"Expected character '{character}', but got: '{c}'");
		}
	}

	protected internal void AssertAndConsume(string @string)
	{
		foreach (char character in @string)
		{
			AssertAndConsume(character);
		}
	}

	protected internal void AssertAndConsumeNull()
	{
		AssertAndConsume("null");
	}

	protected internal bool AssertNextIsDelimiterOrSeparator(char endDelimiter)
	{
		char c = Read();
		if (c == endDelimiter)
		{
			return true;
		}
		if (c == ',')
		{
			return false;
		}
		throw new JsonException("Expected array separator or end of array, got: " + c);
	}

	private string ReadNumericValue()
	{
		return ReadNonStringValue('0');
	}

	private string ReadNonStringValue(char offset)
	{
		StringBuilder stringBuilder = new StringBuilder(10);
		while (true)
		{
			bool flag = true;
			char c = Peek();
			if (IsDelimiter(c))
			{
				break;
			}
			int num = reader.Read();
			if (num >= 48 && num <= 57)
			{
				stringBuilder.Append(num - offset);
			}
			else
			{
				stringBuilder.Append((char)num);
			}
		}
		string text = stringBuilder.ToString();
		return (text == "null") ? null : text;
	}

	private bool IsDelimiter(char c)
	{
		return c == '}' || c == ']' || c == ',' || IsWhiteSpace(c);
	}

	private bool IsWhiteSpace(char c)
	{
		return char.IsWhiteSpace(c);
	}

	public char Read()
	{
		int c = reader.Read();
		return ValidateChar(c);
	}

	private char ValidateChar(int c)
	{
		if (c == -1)
		{
			throw new JsonException("End of data");
		}
		return (char)c;
	}

	private string FromEscaped(char c)
	{
		return c switch
		{
			'"' => "\"", 
			'\\' => "\\", 
			'b' => "\b", 
			'f' => "\f", 
			'r' => "\r", 
			'n' => "\n", 
			't' => "\t", 
			_ => throw new JsonException("Unrecognized escape character: " + c), 
		};
	}

	private void Dispose(bool disposing)
	{
		if (!disposed)
		{
			if (disposing)
			{
				reader.Close();
			}
			disposed = true;
		}
	}

	public void DiscardObjectDefinitionStart()
	{
		SkipWhiteSpaces();
		AssertAndConsume('{');
		SkipWhiteSpaces();
	}

	public string GetNextPropertyName()
	{
		SkipWhiteSpaces();
		string result = ReadString();
		SkipWhiteSpaces();
		AssertAndConsume(':');
		SkipWhiteSpaces();
		return result;
	}

	public void DiscardFieldValue()
	{
		SkipWhiteSpaces();
		if (Peek() == '{')
		{
			DiscardObject();
		}
		else if (Peek() == '[')
		{
			DiscardArray();
		}
		else
		{
			DiscardValue();
		}
		SkipWhiteSpaces();
	}

	public void DiscardObject()
	{
		SkipWhiteSpaces();
		AssertAndConsume('{');
		while (Peek() != '}')
		{
			GetNextPropertyName();
			DiscardFieldValue();
			if (Peek() == '}')
			{
				break;
			}
			AssertAndConsume(',');
		}
		AssertAndConsume('}');
		SkipWhiteSpaces();
	}

	public void DiscardValue()
	{
		SkipWhiteSpaces();
		if (Peek() == '"')
		{
			ReadString();
			return;
		}
		char c = Peek();
		while (c != ',' && c != '}' && c != ']')
		{
			Read();
			c = Peek();
		}
	}

	public void DiscardArray()
	{
		SkipWhiteSpaces();
		AssertAndConsume('[');
		while (Peek() != ']')
		{
			DiscardFieldValue();
			SkipWhiteSpaces();
			if (Peek() == ']')
			{
				break;
			}
			AssertAndConsume(',');
		}
		AssertAndConsume(']');
		SkipWhiteSpaces();
	}
}
