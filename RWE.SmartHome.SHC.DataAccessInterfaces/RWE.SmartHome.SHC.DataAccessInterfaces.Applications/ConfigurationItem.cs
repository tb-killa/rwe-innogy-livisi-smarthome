using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.DataAccessInterfaces.Applications;

public class ConfigurationItem
{
	public string ApplicationId { get; set; }

	public string Name { get; set; }

	[XmlElement(IsNullable = true)]
	public string Value { get; set; }
}
