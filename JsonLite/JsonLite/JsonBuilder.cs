using System;
using System.Text;

namespace JsonLite;

public abstract class JsonBuilder
{
	private const string Null = "null";

	private const string True = "true";

	private const string False = "false";

	private const string DateTimeFormat = "yyyy-MM-ddTHH:mm:ssK";

	private int fieldCount;

	private StringBuilder json;

	private readonly char leadingChar;

	private readonly char endingChar;

	public abstract JsonType JsonTypeBuilder { get; }

	protected JsonBuilder(char leadingChar, char endingChar)
	{
		this.leadingChar = leadingChar;
		this.endingChar = endingChar;
		json = new StringBuilder();
		json.Append(this.leadingChar);
	}

	public string Build()
	{
		json.Append(endingChar);
		return json.ToString();
	}

	protected void AddField(JsonBuilder jsonBuilder)
	{
		AddField(jsonBuilder, null);
	}

	protected void AddField(JsonBuilder jsonBuilder, string name)
	{
		if (jsonBuilder == null)
		{
			AddField<string>(null, name);
		}
		else
		{
			AddField(jsonBuilder, jsonBuilder.JsonTypeBuilder, name);
		}
	}

	protected void AddField<T>(T value) where T : IConvertible
	{
		AddField(value, null);
	}

	protected void AddField<T>(T value, string name) where T : IConvertible
	{
		if (value is DateTime dateTime)
		{
			AddField(dateTime.ToString("yyyy-MM-ddTHH:mm:ssK"), JsonType.String, name);
		}
		else if (value is Enum)
		{
			AddField(value.ToString(), JsonType.String, name);
		}
		else
		{
			AddField(value, GetJsonType(value), name);
		}
	}

	protected JsonType GetJsonType<T>(T value) where T : IConvertible
	{
		if (value == null)
		{
			return JsonType.Null;
		}
		Type type = value.GetType();
		if ((object)type == typeof(string) || (object)type == typeof(char) || value is DateTime || value is Enum)
		{
			return JsonType.String;
		}
		if (type.IsNumber())
		{
			return JsonType.Number;
		}
		if ((object)type == typeof(bool))
		{
			return JsonType.Boolean;
		}
		throw new ArgumentException("Invalid Type", "value");
	}

	private void AddField(object value, JsonType type, string name)
	{
		if (fieldCount > 0)
		{
			json.Append(',');
		}
		if (!string.IsNullOrEmpty(name))
		{
			AddStringName(name);
		}
		switch (type)
		{
		case JsonType.String:
			AddStringValue(value.ToString());
			break;
		case JsonType.Number:
			AddNumber((IConvertible)value);
			break;
		case JsonType.Boolean:
			AddBoolean((bool)value);
			break;
		case JsonType.Null:
			AddNull();
			break;
		default:
			json.Append(((JsonBuilder)value).Build());
			break;
		}
		fieldCount++;
	}

	private void AddStringName(string name)
	{
		json.Append('"').Append(name).Append('"')
			.Append(':');
	}

	private void AddStringValue(string value)
	{
		json.Append('"');
		if (value == null)
		{
			AddNull();
		}
		else
		{
			foreach (char c in value)
			{
				switch (c)
				{
				case '"':
					json.Append('\\').Append(c);
					break;
				case '\\':
					json.Append('\\').Append(c);
					break;
				case '\b':
					json.Append('\\').Append('b');
					break;
				case '\f':
					json.Append('\\').Append('f');
					break;
				case '\n':
					json.Append('\\').Append('n');
					break;
				case '\r':
					json.Append('\\').Append('r');
					break;
				case '\t':
					json.Append('\\').Append('t');
					break;
				default:
					json.Append(c);
					break;
				}
			}
		}
		json.Append('"');
	}

	private void AddNumber<T>(T value) where T : IConvertible
	{
		json.Append(value);
	}

	protected void AddBoolean(bool value)
	{
		json.Append(value ? "true" : "false");
	}

	protected void AddNull()
	{
		json.Append("null");
	}

	protected void AddRawJson(string json, string name)
	{
		if (fieldCount > 0)
		{
			this.json.Append(',');
		}
		if (!string.IsNullOrEmpty(name))
		{
			AddStringName(name);
		}
		this.json.Append(json);
		fieldCount++;
	}
}
