using System.Collections.Generic;
using System.Linq;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration.DevicesDefinitions;

public class LogicalDeviceDefinition
{
	public string DeviceType { get; set; }

	public List<PropertyDefinition> ConfigurationProperties { get; set; }

	public T GetConfigProperty<T>(string name) where T : PropertyDefinition
	{
		return ConfigurationProperties.FirstOrDefault((PropertyDefinition x) => x.Name == name) as T;
	}
}
