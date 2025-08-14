using System;
using System.Collections.Generic;
using System.Xml;

namespace RWE.SmartHome.SHC.Core.Configuration;

public sealed class ConfigurationSection
{
	private string sectionName;

	private Dictionary<string, string> defaultSettings;

	private Dictionary<string, string> userSettings;

	public ConfigurationSection(string sectionName)
	{
		this.sectionName = sectionName;
	}

	public IEnumerable<string> GetSectionValues()
	{
		return GetSectionContent()?.Values;
	}

	public IDictionary<string, string> GetSectionContent()
	{
		if (userSettings != null)
		{
			return userSettings;
		}
		if (defaultSettings != null)
		{
			return defaultSettings;
		}
		return null;
	}

	public string GetString(string key)
	{
		if (userSettings != null && userSettings.TryGetValue(key, out var value))
		{
			return value;
		}
		return GetStringFromDefaultSettings(key);
	}

	private string GetStringFromDefaultSettings(string key)
	{
		if (defaultSettings != null && defaultSettings.TryGetValue(key, out var value))
		{
			return value;
		}
		return null;
	}

	public int? GetInt(string key)
	{
		string text = GetString(key);
		if (text == null)
		{
			return null;
		}
		return int.Parse(text);
	}

	public uint? GetUInt(string key)
	{
		string text = GetString(key);
		if (text == null)
		{
			return null;
		}
		return uint.Parse(text);
	}

	public bool? GetBool(string key)
	{
		string text = GetString(key);
		if (text == null)
		{
			return null;
		}
		return bool.Parse(text);
	}

	public byte? GetByte(string key)
	{
		string text = GetString(key);
		if (text == null)
		{
			return null;
		}
		return byte.Parse(text);
	}

	public short? GetShort(string key)
	{
		string text = GetString(key);
		if (text == null)
		{
			return null;
		}
		return short.Parse(text);
	}

	public long? GetLong(string key)
	{
		string text = GetString(key);
		if (text == null)
		{
			return null;
		}
		return long.Parse(text);
	}

	public decimal? GetDecimal(string key)
	{
		string text = GetString(key);
		if (text == null)
		{
			return null;
		}
		return decimal.Parse(text);
	}

	public DateTime? GetDate(string key)
	{
		string text = GetString(key);
		if (text == null)
		{
			return null;
		}
		return DateTime.Parse(text);
	}

	public void ResetSection()
	{
		userSettings = null;
	}

	public void Reset(string key)
	{
		if (userSettings != null && userSettings.Remove(key) && userSettings.Count == 0)
		{
			userSettings = null;
		}
	}

	public void SetString(string key, string value)
	{
		if (GetStringFromDefaultSettings(key) == value)
		{
			Reset(key);
			return;
		}
		if (userSettings == null)
		{
			userSettings = new Dictionary<string, string>();
		}
		userSettings[key] = value;
	}

	public void SetDataValue(string key, object value)
	{
		SetString(key, value.ToString());
	}

	internal void ReadDefaultConfigurationSection(XmlReader reader)
	{
		ReadConfigurationSection(reader, ref defaultSettings);
	}

	internal void ReadUserConfigurationSection(XmlReader reader)
	{
		ReadConfigurationSection(reader, ref userSettings);
	}

	internal void WriteUserConfigurationSection(XmlWriter writer)
	{
		WriteConfigurationSection(writer, userSettings);
	}

	private void ReadConfigurationSection(XmlReader reader, ref Dictionary<string, string> settings)
	{
		reader.Read();
		while (reader.IsStartElement("Property"))
		{
			string attribute = reader.GetAttribute("Key");
			string attribute2 = reader.GetAttribute("Value");
			if (settings == null)
			{
				settings = new Dictionary<string, string>();
			}
			settings[attribute] = attribute2;
			reader.Skip();
		}
	}

	private void WriteConfigurationSection(XmlWriter writer, Dictionary<string, string> settings)
	{
		if (settings == null)
		{
			return;
		}
		writer.WriteStartElement("Section");
		writer.WriteAttributeString("Name", sectionName);
		foreach (string key in settings.Keys)
		{
			string value = settings[key];
			writer.WriteStartElement("Property");
			writer.WriteAttributeString("Key", key);
			writer.WriteAttributeString("Value", value);
			writer.WriteEndElement();
		}
		writer.WriteEndElement();
	}
}
