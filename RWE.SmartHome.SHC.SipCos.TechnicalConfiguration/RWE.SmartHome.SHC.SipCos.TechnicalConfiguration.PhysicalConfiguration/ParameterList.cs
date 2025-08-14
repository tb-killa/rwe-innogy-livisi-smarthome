using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;

public class ParameterList : IEnumerable<KeyValuePair<byte, byte>>, IEnumerable
{
	private readonly SortedList<byte, byte> parameters = new SortedList<byte, byte>();

	public byte this[byte key]
	{
		get
		{
			return parameters[key];
		}
		set
		{
			parameters[key] = value;
		}
	}

	public int Count => parameters.Count;

	public ParameterList()
	{
	}

	public IEnumerator<KeyValuePair<byte, byte>> GetEnumerator()
	{
		return parameters.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public ParameterList SetAt(byte key, byte value)
	{
		parameters[key] = value;
		return this;
	}

	public ParameterList Remove(byte key)
	{
		parameters.Remove(key);
		return this;
	}

	public byte[] ToArray()
	{
		byte[] array = new byte[parameters.Count * 2];
		int num = 0;
		foreach (KeyValuePair<byte, byte> parameter in parameters)
		{
			array[num++] = parameter.Key;
			array[num++] = parameter.Value;
		}
		return array;
	}

	public string ToReadable()
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (KeyValuePair<byte, byte> parameter in parameters)
		{
			stringBuilder.AppendFormat(" {0:X2}:{1:X2}", new object[2] { parameter.Key, parameter.Value });
		}
		if (parameters.Count > 0)
		{
			stringBuilder.Remove(0, 1);
		}
		return stringBuilder.ToString();
	}

	public ParameterList CreateDiffAndMark(ParameterList newList)
	{
		ParameterList parameterList = new ParameterList();
		foreach (KeyValuePair<byte, byte> parameter in newList.parameters)
		{
			if (parameters.TryGetValue(parameter.Key, out var value))
			{
				if (parameter.Value != value)
				{
					parameterList[parameter.Key] = parameter.Value;
					parameters.Remove(parameter.Key);
				}
			}
			else
			{
				parameterList[parameter.Key] = parameter.Value;
			}
		}
		return parameterList;
	}

	public void ApplyDiff(ParameterList diff)
	{
		if (diff == null)
		{
			return;
		}
		foreach (KeyValuePair<byte, byte> item in diff)
		{
			parameters[item.Key] = item.Value;
		}
	}

	public ParameterList Clone()
	{
		ParameterList parameterList = new ParameterList();
		foreach (KeyValuePair<byte, byte> parameter in parameters)
		{
			parameterList[parameter.Key] = parameter.Value;
		}
		return parameterList;
	}

	public ParameterList(BinaryReader reader)
	{
		byte b = reader.ReadByte();
		for (int i = 0; i < b; i++)
		{
			byte key = reader.ReadByte();
			byte value = reader.ReadByte();
			parameters[key] = value;
		}
	}

	public void Save(BinaryWriter writer)
	{
		writer.Write((byte)parameters.Count);
		writer.Write(ToArray());
	}
}
