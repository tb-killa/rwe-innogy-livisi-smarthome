using System;
using System.Collections.Generic;
using System.Text;

namespace JsonLite;

public class JsonParser
{
	public const string RootName = "_root";

	private readonly string name;

	private readonly int startIndex;

	private readonly int endIndex;

	private StringBuilder innerJson;

	private List<int> fieldsIndexes;

	private JsonType jsonPrimitiveType = JsonType.Null;

	public string Name => name;

	public int FieldsCount => fieldsIndexes.Count;

	public bool HasValue { get; private set; }

	public bool IsPrimitive { get; private set; }

	public bool IsObject { get; set; }

	public bool IsArray { get; set; }

	public bool IsString => jsonPrimitiveType == JsonType.String;

	public bool IsNumber => jsonPrimitiveType == JsonType.Number;

	public bool IsBoolean => jsonPrimitiveType == JsonType.Boolean;

	public JsonParser this[int index] => GetField(index);

	public JsonParser this[string fieldName] => GetField(fieldName);

	public JsonParser(string json)
	{
		name = "_root";
		innerJson = new StringBuilder(json);
		fieldsIndexes = new List<int>();
		startIndex = 0;
		endIndex = ParseJson();
	}

	protected int ParseJson()
	{
		int i;
		for (i = startIndex; char.IsWhiteSpace(innerJson[i]); i++)
		{
		}
		HasValue = true;
		switch (innerJson[i])
		{
		case '"':
			IsPrimitive = true;
			jsonPrimitiveType = JsonType.String;
			return ParseString(i + 1);
		case '{':
			IsObject = true;
			return ParseObject(i + 1);
		case '[':
			IsArray = true;
			return ParseArray(i + 1);
		default:
			IsPrimitive = true;
			if (IsNullValue(i))
			{
				HasValue = false;
				IsPrimitive = false;
			}
			return ParseType(i, out jsonPrimitiveType);
		}
	}

	private int ParseObject(int currentIndex)
	{
		bool flag = false;
		while (currentIndex < innerJson.Length)
		{
			if (!innerJson[currentIndex].IsWhiteSpace())
			{
				switch (innerJson[currentIndex])
				{
				case '"':
					if (!flag)
					{
						fieldsIndexes.Add(currentIndex);
						currentIndex = ParseFieldName(currentIndex + 1);
					}
					else
					{
						currentIndex = ParseString(currentIndex + 1);
					}
					break;
				case ':':
					flag = true;
					break;
				case ',':
					flag = false;
					break;
				case '{':
					currentIndex = ConsumeObject(currentIndex + 1);
					break;
				case '[':
					currentIndex = ConsumeArray(currentIndex + 1);
					break;
				case '}':
					return currentIndex;
				default:
					if (flag)
					{
						currentIndex = ParseType(currentIndex);
						break;
					}
					throw new FormatException(GetFormatExceptionMessage(currentIndex, "Unexpected token"));
				}
			}
			currentIndex++;
		}
		throw new FormatException(GetFormatExceptionMessage(currentIndex, "Missing Object ending"));
	}

	private int ParseFieldName(int currentIndex)
	{
		if (innerJson[currentIndex].IsLetter() || innerJson[currentIndex] == '_')
		{
			for (currentIndex++; currentIndex < innerJson.Length; currentIndex++)
			{
				if (innerJson[currentIndex] == '"')
				{
					return currentIndex;
				}
				if (!innerJson[currentIndex].IsLetter() && !innerJson[currentIndex].IsDigit() && innerJson[currentIndex] != '_')
				{
					break;
				}
			}
		}
		throw new FormatException(GetFormatExceptionMessage(currentIndex));
	}

	private int ParseString(int currentIndex)
	{
		while (currentIndex < innerJson.Length)
		{
			if (innerJson[currentIndex] == '\\')
			{
				currentIndex = ParseStringEscape(currentIndex + 1);
			}
			if (innerJson[currentIndex] == '"')
			{
				return currentIndex;
			}
			currentIndex++;
		}
		throw new FormatException(GetFormatExceptionMessage(currentIndex));
	}

	private int ParseStringEscape(int currentIndex)
	{
		switch (innerJson[currentIndex])
		{
		case '"':
		case '/':
		case '\\':
		case 'b':
		case 'f':
		case 'n':
		case 'r':
		case 't':
			return currentIndex + 1;
		case 'u':
			return ParseHexUnicode(currentIndex + 1);
		default:
			throw new FormatException(GetFormatExceptionMessage(currentIndex, "Invalid control character"));
		}
	}

	private int ParseHexUnicode(int currentIndex)
	{
		if (currentIndex + 3 < innerJson.Length)
		{
			for (int i = 0; i < 4 && currentIndex + i < innerJson.Length; i++)
			{
				if (!innerJson[currentIndex + i].IsHexDigit())
				{
					throw new FormatException(GetFormatExceptionMessage(currentIndex + i, "Invalid unicode escape sequence"));
				}
			}
			return currentIndex + 4;
		}
		throw new FormatException(GetFormatExceptionMessage(currentIndex, "Invalid unicode escape sequence"));
	}

	private int ConsumeObject(int currentIndex)
	{
		while (currentIndex < innerJson.Length)
		{
			switch (innerJson[currentIndex])
			{
			case '}':
				return currentIndex;
			case '{':
				currentIndex = ConsumeObject(currentIndex + 1);
				break;
			case '[':
				currentIndex = ConsumeArray(currentIndex + 1);
				break;
			}
			currentIndex++;
		}
		throw new FormatException(GetFormatExceptionMessage(currentIndex, "Invalid Json ending"));
	}

	private int ConsumeArray(int currentIndex)
	{
		while (currentIndex < innerJson.Length)
		{
			switch (innerJson[currentIndex])
			{
			case ']':
				return currentIndex;
			case '[':
				currentIndex = ConsumeArray(currentIndex + 1);
				break;
			case '{':
				currentIndex = ConsumeObject(currentIndex + 1);
				break;
			}
			currentIndex++;
		}
		throw new FormatException(GetFormatExceptionMessage(currentIndex, "Invalid Json ending"));
	}

	private int ParseArray(int currentIndex)
	{
		while (currentIndex < innerJson.Length)
		{
			if (!innerJson[currentIndex].IsWhiteSpace())
			{
				switch (innerJson[currentIndex])
				{
				case '"':
					fieldsIndexes.Add(currentIndex);
					currentIndex = ParseString(currentIndex + 1);
					break;
				case '{':
					fieldsIndexes.Add(currentIndex);
					currentIndex = ConsumeObject(currentIndex + 1);
					break;
				case '[':
					fieldsIndexes.Add(currentIndex);
					currentIndex = ConsumeArray(currentIndex + 1);
					break;
				case ']':
					return currentIndex;
				default:
					fieldsIndexes.Add(currentIndex);
					currentIndex = ParseType(currentIndex);
					break;
				case ',':
					break;
				}
			}
			currentIndex++;
		}
		throw new FormatException(GetFormatExceptionMessage(currentIndex, "Missing Array ending"));
	}

	private int ParseType(int currentIndex, out JsonType jsonType)
	{
		if (innerJson[currentIndex].IsDigit() || innerJson[currentIndex] == '-')
		{
			int result = ParseNumber(currentIndex);
			jsonType = JsonType.Number;
			return result;
		}
		if (innerJson[currentIndex].IsLetter())
		{
			if (innerJson[currentIndex] == 'n')
			{
				int result = ParseNull(currentIndex);
				jsonType = JsonType.Null;
				return result;
			}
			if (innerJson[currentIndex] == 't' || innerJson[currentIndex] == 'f')
			{
				int result = ParseBoolean(currentIndex);
				jsonType = JsonType.Boolean;
				return result;
			}
		}
		throw new FormatException(GetFormatExceptionMessage(currentIndex, "Invalid json type"));
	}

	private int ParseType(int currentIndex)
	{
		if (innerJson[currentIndex].IsDigit() || innerJson[currentIndex] == '-')
		{
			return ParseNumber(currentIndex);
		}
		if (innerJson[currentIndex].IsLetter())
		{
			if (innerJson[currentIndex] == 'n')
			{
				return ParseNull(currentIndex);
			}
			if (innerJson[currentIndex] == 't' || innerJson[currentIndex] == 'f')
			{
				return ParseBoolean(currentIndex);
			}
		}
		throw new FormatException(GetFormatExceptionMessage(currentIndex, "Invalid json type"));
	}

	private int ParseNumber(int currentIndex)
	{
		while (currentIndex < innerJson.Length)
		{
			if (innerJson[currentIndex] == ',' || innerJson[currentIndex] == '}' || innerJson[currentIndex] == ']')
			{
				return currentIndex - 1;
			}
			currentIndex++;
		}
		return innerJson.Length - 1;
	}

	private int ParseNull(int currentIndex)
	{
		if (IsNullValue(currentIndex))
		{
			return currentIndex + 3;
		}
		throw new FormatException(GetFormatExceptionMessage(currentIndex, "Invalid json type"));
	}

	private bool IsNullValue(int currentIndex)
	{
		if (innerJson[currentIndex] == 'n' && innerJson[currentIndex + 1] == 'u' && innerJson[currentIndex + 2] == 'l')
		{
			return innerJson[currentIndex + 3] == 'l';
		}
		return false;
	}

	private int ParseBoolean(int currentIndex)
	{
		if (innerJson[currentIndex] == 't' && innerJson[currentIndex + 1] == 'r' && innerJson[currentIndex + 2] == 'u' && innerJson[currentIndex + 3] == 'e')
		{
			return currentIndex + 3;
		}
		if (innerJson[currentIndex] == 'f' && innerJson[currentIndex + 1] == 'a' && innerJson[currentIndex + 2] == 'l' && innerJson[currentIndex + 3] == 's' && innerJson[currentIndex + 4] == 'e')
		{
			return currentIndex + 4;
		}
		throw new FormatException(GetFormatExceptionMessage(currentIndex, "Invalid json type"));
	}

	private string GetFormatExceptionMessage(int index)
	{
		return GetFormatExceptionMessage(index, string.Empty);
	}

	private string GetFormatExceptionMessage(int index, string message)
	{
		if (index < innerJson.Length)
		{
			return $"String is not a valid json: error at index = {index} character={innerJson[index]}. {message}";
		}
		return $"String is not a valid json: error at index = {index}. {message}";
	}

	public string GetRawValue()
	{
		return innerJson.ToString(startIndex, endIndex - startIndex + 1);
	}

	public JsonParser GetField(string fieldName)
	{
		if (IsObject)
		{
			foreach (int fieldsIndex in fieldsIndexes)
			{
				if (IsFieldAtIndex(fieldName, fieldsIndex))
				{
					int i;
					for (i = fieldsIndex + fieldName.Length + 2; innerJson[i].IsWhiteSpace() || innerJson[i] == ':'; i++)
					{
					}
					return new JsonParser(innerJson, i, fieldName);
				}
			}
			return null;
		}
		throw new InvalidOperationException("Underlying json must be an object");
	}

	private bool IsFieldAtIndex(string fieldName, int fieldIndex)
	{
		bool flag = false;
		int i;
		for (i = 0; i < fieldName.Length; i++)
		{
			flag = fieldName[i] == innerJson[fieldIndex + 1 + i];
			if (!flag)
			{
				break;
			}
		}
		if (flag)
		{
			return innerJson[fieldIndex + 1 + i] == '"';
		}
		return false;
	}

	public JsonParser GetField(int index)
	{
		if (IsArray)
		{
			return new JsonParser(innerJson, fieldsIndexes[index], index.ToString());
		}
		if (IsObject)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 1; innerJson[fieldsIndexes[index] + i] != '"'; i++)
			{
				stringBuilder.Append(innerJson[fieldsIndexes[index] + i]);
			}
			return GetField(stringBuilder.ToString());
		}
		throw new InvalidOperationException("Underlying json has no fields");
	}

	protected JsonParser(StringBuilder innerJson, int startIndex, string name)
	{
		this.name = name;
		this.innerJson = innerJson;
		fieldsIndexes = new List<int>();
		this.startIndex = startIndex;
		endIndex = ParseJson();
	}

	public T GetValue<T>() where T : IConvertible
	{
		try
		{
			if (HasValue)
			{
				if ((object)typeof(T) == typeof(string))
				{
					return (T)(object)GetValueAsString();
				}
				if ((object)typeof(T) == typeof(decimal))
				{
					return (T)(object)decimal.Parse(GetRawValue());
				}
				if (typeof(T).IsPrimitive)
				{
					if ((object)typeof(T) == typeof(bool))
					{
						return (T)(object)GetValueAsBoolean();
					}
					if ((object)typeof(T) == typeof(char))
					{
						return (T)(object)CharExtensions.ParseChar(GetValueAsString());
					}
					ParseNumber(startIndex);
					if ((object)typeof(T) == typeof(byte))
					{
						return (T)(object)byte.Parse(GetRawValue());
					}
					if ((object)typeof(T) == typeof(sbyte))
					{
						return (T)(object)sbyte.Parse(GetRawValue());
					}
					if ((object)typeof(T) == typeof(ushort))
					{
						return (T)(object)ushort.Parse(GetRawValue());
					}
					if ((object)typeof(T) == typeof(short))
					{
						return (T)(object)short.Parse(GetRawValue());
					}
					if ((object)typeof(T) == typeof(uint))
					{
						return (T)(object)uint.Parse(GetRawValue());
					}
					if ((object)typeof(T) == typeof(int))
					{
						return (T)(object)int.Parse(GetRawValue());
					}
					if ((object)typeof(T) == typeof(ulong))
					{
						return (T)(object)ulong.Parse(GetRawValue());
					}
					if ((object)typeof(T) == typeof(long))
					{
						return (T)(object)long.Parse(GetRawValue());
					}
					if ((object)typeof(T) == typeof(float))
					{
						return (T)(object)float.Parse(GetRawValue());
					}
					if ((object)typeof(T) == typeof(double))
					{
						return (T)(object)double.Parse(GetRawValue());
					}
				}
				throw new InvalidCastException(GetInvalidCastExceptionMessage(typeof(T).Name));
			}
			return (T)(object)null;
		}
		catch (FormatException innerException)
		{
			throw new InvalidCastException(GetInvalidCastExceptionMessage(typeof(T).Name), innerException);
		}
	}

	private bool GetValueAsBoolean()
	{
		ParseBoolean(startIndex);
		return innerJson[startIndex] == 't';
	}

	private string GetValueAsString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = startIndex + 1; i < endIndex; i++)
		{
			if (innerJson[i] == '\\')
			{
				i++;
				switch (innerJson[i])
				{
				case 'r':
					stringBuilder.Append('\r');
					break;
				case 'n':
					stringBuilder.Append('\n');
					break;
				case 'f':
					stringBuilder.Append('\f');
					break;
				case 't':
					stringBuilder.Append('\t');
					break;
				case 'b':
					stringBuilder.Append('\b');
					break;
				case 'u':
					stringBuilder.Append(GetUnicodeCar(i + 1));
					i += 4;
					break;
				default:
					stringBuilder.Append(innerJson[i]);
					break;
				}
			}
			else
			{
				stringBuilder.Append(innerJson[i]);
			}
		}
		return stringBuilder.ToString();
	}

	private char GetUnicodeCar(int currentIndex)
	{
		byte b = (byte)((innerJson[currentIndex] - 48 << 4) | (innerJson[currentIndex + 1] - 48));
		byte b2 = (byte)((innerJson[currentIndex + 2] - 48 << 4) | (innerJson[currentIndex + 3] - 48));
		return (char)((b << 8) | b2);
	}

	private string GetInvalidCastExceptionMessage(string typeName)
	{
		return $"The underlying value of {name} cannot be converted to type {typeName}";
	}

	public List<JsonParser> GetAllFields()
	{
		List<JsonParser> list = new List<JsonParser>();
		for (int i = 0; i < FieldsCount; i++)
		{
			list.Add(GetField(i));
		}
		return list;
	}
}
