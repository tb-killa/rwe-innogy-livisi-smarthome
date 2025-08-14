using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using SmartHome.SHC.API.Settings;

namespace RWE.SmartHome.SHC.ApplicationsHost.Settings;

public class SettingsProvider : ISettingsProvider
{
	private const string AppSettingsName = "AppSettings";

	private const string SettingName = "Setting";

	private const string KeyName = "Key";

	private const string ValueName = "Value";

	private Dictionary<string, string> settings = new Dictionary<string, string>();

	private string configFilePath = "";

	private bool settingsRead;

	private string applicationId = "";

	public SettingsProvider(string appId, string appFileName)
	{
		configFilePath = Path.Combine(Path.GetDirectoryName(appFileName), appId.Replace("sh://", "") + ".config");
		applicationId = appId;
	}

	public string GetSetting(string key)
	{
		if (!settingsRead)
		{
			try
			{
				using XmlReader xmlReader = XmlReader.Create(configFilePath);
				while (!xmlReader.IsStartElement("AppSettings") && xmlReader.Read())
				{
				}
				xmlReader.Read();
				while (xmlReader.IsStartElement("Setting"))
				{
					string attribute = xmlReader.GetAttribute("Key");
					string attribute2 = xmlReader.GetAttribute("Value");
					settings[attribute] = attribute2;
					xmlReader.Skip();
				}
			}
			catch (Exception ex)
			{
				Log.Warning(Module.ApplicationsHost, $"Could not load config file {configFilePath} for application {applicationId}; {ex.Message} -- {ex.StackTrace}");
				settings = null;
			}
			settingsRead = true;
		}
		if (settings == null)
		{
			return null;
		}
		settings.TryGetValue(key, out var value);
		return value;
	}
}
