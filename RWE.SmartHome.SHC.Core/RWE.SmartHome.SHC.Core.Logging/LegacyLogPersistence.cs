using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;

namespace RWE.SmartHome.SHC.Core.Logging;

internal class LegacyLogPersistence
{
	public List<LegacyLogEntry> LogEntries { get; set; }

	internal LegacyLogPersistence()
	{
		LogEntries = new List<LegacyLogEntry>();
	}

	internal void LoadFile(string logPersistenceFile)
	{
		if (!File.Exists(logPersistenceFile))
		{
			return;
		}
		using XmlReader xmlReader = new XmlTextReader(logPersistenceFile);
		xmlReader.MoveToContent();
		try
		{
			while (xmlReader.Read())
			{
				if (!(xmlReader.Name == "LogEntry"))
				{
					continue;
				}
				LegacyLogEntry legacyLogEntry = new LegacyLogEntry();
				if (!xmlReader.MoveToAttribute("Severity"))
				{
					continue;
				}
				legacyLogEntry.Severity = byte.Parse(xmlReader.Value);
				if (!xmlReader.MoveToAttribute("Source"))
				{
					continue;
				}
				legacyLogEntry.Source = xmlReader.Value;
				if (xmlReader.MoveToAttribute("Timestamp"))
				{
					legacyLogEntry.Timestamp = DateTime.Parse(xmlReader.Value, CultureInfo.InvariantCulture);
					if (xmlReader.MoveToAttribute("Message"))
					{
						legacyLogEntry.Message = xmlReader.Value;
						LogEntries.Add(legacyLogEntry);
					}
				}
			}
		}
		catch (XmlException)
		{
			try
			{
				File.Delete(logPersistenceFile);
			}
			catch
			{
			}
		}
	}
}
