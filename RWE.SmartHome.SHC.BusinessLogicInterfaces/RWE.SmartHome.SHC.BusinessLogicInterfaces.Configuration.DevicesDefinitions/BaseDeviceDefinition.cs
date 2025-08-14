using System.Collections.Generic;
using System.Linq;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration.DevicesDefinitions;

public class BaseDeviceDefinition
{
	public string DeviceType { get; set; }

	public string Version => GetAttribute<StringPropertyDefinition>("Version").DefaultValue;

	public FirmwareVersion MinVersion { get; set; }

	public FirmwareVersion MaxVersion { get; set; }

	public List<PropertyDefinition> Attributes { get; set; }

	public List<PropertyDefinition> ConfigurationProperties { get; set; }

	public List<LogicalDeviceDefinition> LogicalDevices { get; set; }

	public bool MatchFirmware(FirmwareVersion version)
	{
		if (MinVersion == null || version.CompareTo(MinVersion) >= 0)
		{
			if (MaxVersion != null)
			{
				return version.CompareTo(MaxVersion) < 0;
			}
			return true;
		}
		return false;
	}

	public T GetAttribute<T>(string name) where T : PropertyDefinition
	{
		return Attributes.First((PropertyDefinition x) => x.Name == name) as T;
	}

	public T GetConfigProperty<T>(string name) where T : PropertyDefinition
	{
		return ConfigurationProperties.FirstOrDefault((PropertyDefinition x) => x.Name == name) as T;
	}
}
