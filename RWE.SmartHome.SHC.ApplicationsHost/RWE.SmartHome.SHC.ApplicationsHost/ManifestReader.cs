using System;
using System.Xml;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.ApplicationsHost;

internal class ManifestReader
{
	private const string AppIdName = "AppId";

	private const string VersionName = "Version";

	private const string Package = "Package";

	private const string CheckSum = "CheckSum";

	private const string Shc = "SHC";

	private const string MainAssembly = "MainAssembly";

	private const string ApiVersionAttributeName = "ApiVersion";

	internal string ApiVersion { get; private set; }

	internal string AppId { get; private set; }

	internal string PackageFileName { get; private set; }

	internal byte[] PackageCheckSum { get; private set; }

	internal string PackageCheckSumString { get; private set; }

	internal string MainAssemblyName { get; private set; }

	internal string Version { get; private set; }

	internal ManifestReader(string fileName)
	{
		using XmlReader xmlReader = XmlReader.Create(fileName);
		xmlReader.MoveToContent();
		while (!xmlReader.IsStartElement("SHC") && xmlReader.Read())
		{
			if (xmlReader.IsStartElement("AppId"))
			{
				xmlReader.Read();
				AppId = xmlReader.Value;
			}
			if (xmlReader.IsStartElement("Version"))
			{
				xmlReader.Read();
				Version = xmlReader.Value;
			}
		}
		xmlReader.Read();
		int depth = xmlReader.Depth;
		while (xmlReader.Read())
		{
			if (xmlReader.NodeType == XmlNodeType.Element)
			{
				switch (xmlReader.Name)
				{
				case "MainAssembly":
					MainAssemblyName = xmlReader.ReadElementString("MainAssembly");
					break;
				case "Package":
				{
					string attribute = xmlReader.GetAttribute("CheckSum");
					if (!string.IsNullOrEmpty(attribute))
					{
						PackageCheckSumString = attribute.Trim();
						PackageCheckSum = StringToByteArray(PackageCheckSumString);
					}
					ApiVersion = xmlReader.GetAttribute("ApiVersion");
					if (string.IsNullOrEmpty(ApiVersion))
					{
						ApiVersion = "3.00";
						Log.Warning(Module.ApplicationsHost, $"ApiVersion attribute not found in file {fileName}. Will default to {ApiVersion}");
					}
					PackageFileName = xmlReader.ReadElementString("Package");
					break;
				}
				}
			}
			if (xmlReader.Depth < depth)
			{
				break;
			}
		}
	}

	private static byte[] StringToByteArray(string hex)
	{
		if (hex.Length % 2 != 0)
		{
			Log.Warning(Module.ApplicationsHost, "Cannot parse apps manifest.");
			return null;
		}
		int length = hex.Length;
		byte[] array = new byte[length / 2];
		for (int i = 0; i < length; i += 2)
		{
			array[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
		}
		return array;
	}
}
