using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace SmartHome.SHC.API.Serialization.Json;

internal class JsonWriter : IDisposable
{
	private bool disposed;

	private int currentIndentation;

	private readonly TextWriter writer;

	public JsonWriter(TextWriter output)
	{
		writer = output;
	}

	public JsonWriter(Stream output)
		: this(new StreamWriter(output, Encoding.UTF8))
	{
	}

	public JsonWriter(string file)
		: this(new FileStream(file, FileMode.Create, FileAccess.Write, FileShare.Read))
	{
	}

	public JsonWriter(StringBuilder output)
		: this(new StringWriter(output, CultureInfo.InvariantCulture))
	{
	}

	public virtual void WriteNull()
	{
		WriteRaw("null");
	}

	public virtual void WriteString(string value)
	{
		writer.Write('"' + value.Replace("\"", "\\\"") + '"');
	}

	public virtual void WriteRaw(string value)
	{
		writer.Write(value);
	}

	public virtual void WriteChar(char value)
	{
		WriteString(value.ToString());
	}

	public virtual void WriteBool(bool value)
	{
		WriteRaw(value ? "true" : "false");
	}

	public virtual void WriteDate(DateTime date)
	{
		WriteString(date.ToString("O", CultureInfo.InvariantCulture));
	}

	public virtual void WriteKey(string key)
	{
		WriteString(key);
		writer.Write(':');
	}

	public virtual void BeginObject()
	{
		writer.Write('{');
		currentIndentation++;
	}

	public virtual void EndObject()
	{
		currentIndentation--;
		writer.Write('}');
	}

	public virtual void EndArray()
	{
		writer.Write(']');
	}

	public virtual void BeginArray()
	{
		writer.Write('[');
	}

	public virtual void SeparateElements()
	{
		writer.Write(',');
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void Flush()
	{
		writer.Flush();
	}

	private void Dispose(bool disposing)
	{
		if (!disposed)
		{
			if (disposing)
			{
				Flush();
				writer.Close();
			}
			disposed = true;
		}
	}
}
