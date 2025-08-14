using System;
using System.IO;
using System.Reflection;
using RWE.SmartHome.SHC.CommonFunctionality.Interfaces;
using SHCWrapper.Misc;

namespace RWE.SmartHome.SHC.CommonFunctionality.Implementations;

internal class SHCVersionImplementation : ISHCVersion
{
	private const string OS_VERSION_FILE_NAME = "/Windows/OsVersion.txt";

	private string hardwareVersion;

	private string applicationVersion;

	private string osVersion;

	public string HardwareVersion
	{
		get
		{
			if (hardwareVersion == null)
			{
				string hWVersion = HWVersion.GetHWVersion();
				hardwareVersion = hWVersion.Substring(0, 2) + "." + hWVersion.Substring(2, 2);
			}
			return hardwareVersion;
		}
	}

	public string ApplicationVersion
	{
		get
		{
			if (applicationVersion == null)
			{
				Version version = Assembly.GetExecutingAssembly().GetName().Version;
				applicationVersion = $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
			}
			return applicationVersion;
		}
	}

	public string OsVersion
	{
		get
		{
			if (osVersion == null)
			{
				osVersion = "0.0";
				if (File.Exists("/Windows/OsVersion.txt"))
				{
					using StreamReader streamReader = new StreamReader(new FileStream("/Windows/OsVersion.txt", FileMode.Open, FileAccess.Read));
					osVersion = streamReader.ReadToEnd();
					streamReader.Close();
				}
			}
			return osVersion;
		}
	}
}
