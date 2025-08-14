using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Xml;
using RWE.SmartHome.SHC.CommonFunctionality;

namespace RWE.SmartHome.SHC.Core.Configuration;

internal sealed class ConfigurationManager : IConfigurationManager, IService
{
	internal const string TagRoot = "Settings";

	internal const string TagSection = "Section";

	internal const string TagSections = "Sections";

	internal const string TagSectionName = "Name";

	internal const string TagProperty = "Property";

	internal const string TagKey = "Key";

	internal const string TagValue = "Value";

	private static string configurationDirectory = null;

	private static readonly ConfigurationManager instance = new ConfigurationManager();

	private readonly Dictionary<string, ConfigurationSection> configurationSections = new Dictionary<string, ConfigurationSection>();

	public static ConfigurationManager Instance => instance;

	public ConfigurationSection this[string sectionName]
	{
		get
		{
			ConfigurationSection configurationSection;
			if (configurationSections.ContainsKey(sectionName))
			{
				configurationSection = configurationSections[sectionName];
			}
			else
			{
				configurationSection = new ConfigurationSection(sectionName);
				configurationSections[sectionName] = configurationSection;
			}
			return configurationSection;
		}
	}

	private static string ApplicationDirectory
	{
		get
		{
			string codeBase = Assembly.GetExecutingAssembly().GetName().CodeBase;
			return Path.GetDirectoryName(new Uri(codeBase).LocalPath);
		}
	}

	private static string ConfigurationDirectory
	{
		get
		{
			if (configurationDirectory == null)
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(ApplicationDirectory);
				configurationDirectory = directoryInfo.FullName;
			}
			return configurationDirectory;
		}
	}

	private static string DefaultConfigurationPath => Path.Combine(ConfigurationDirectory, "settings.config");

	private ConfigurationManager()
	{
		Load();
	}

	private void Load()
	{
		configurationSections.Clear();
		LoadConfigurationFile(DefaultConfigurationPath, userConfiguration: false);
	}

	private void LoadConfigurationFile(string path, bool userConfiguration)
	{
		if (!File.Exists(path))
		{
			HaltFatally("No configuration file.");
			return;
		}
		using XmlReader xmlReader = XmlReader.Create(path);
		xmlReader.MoveToContent();
		while (!xmlReader.IsStartElement("Sections") && xmlReader.Read())
		{
		}
		xmlReader.Read();
		while (xmlReader.IsStartElement("Section"))
		{
			string attribute = xmlReader.GetAttribute("Name");
			ConfigurationSection cs = this[attribute];
			ReadConfigurationSection(cs, xmlReader, userConfiguration);
			xmlReader.Skip();
		}
	}

	private static void HaltFatally(string message)
	{
		Console.WriteLine(message);
		Lcd.Text = "F 0011";
		Lcd.Update();
		while (true)
		{
			Thread.Sleep(1000);
		}
	}

	private static void ReadConfigurationSection(ConfigurationSection cs, XmlReader reader, bool userConfiguration)
	{
		if (userConfiguration)
		{
			cs.ReadUserConfigurationSection(reader);
		}
		else
		{
			cs.ReadDefaultConfigurationSection(reader);
		}
	}

	public void Initialize()
	{
	}

	public void Uninitialize()
	{
	}
}
