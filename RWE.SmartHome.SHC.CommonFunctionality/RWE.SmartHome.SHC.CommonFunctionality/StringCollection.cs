using System.Collections.Generic;
using System.Text;

namespace RWE.SmartHome.SHC.CommonFunctionality;

public class StringCollection
{
	private List<string> values;

	public StringCollection()
	{
		values = new List<string>();
	}

	public StringCollection(string value)
	{
		values = new List<string>();
		if (!string.IsNullOrEmpty(value))
		{
			values.Add(value);
		}
	}

	public void Add(string value)
	{
		if (!string.IsNullOrEmpty(value))
		{
			values.Add(value);
		}
	}

	public void AddCollection(StringCollection stringCollection)
	{
		if (stringCollection != null)
		{
			values.AddRange(stringCollection.values);
		}
	}

	public void InsertTop(string value)
	{
		if (!string.IsNullOrEmpty(value))
		{
			values.Insert(0, value);
		}
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (string value in values)
		{
			stringBuilder.Append(value);
		}
		return stringBuilder.ToString();
	}

	public int Count()
	{
		return values.Count;
	}
}
