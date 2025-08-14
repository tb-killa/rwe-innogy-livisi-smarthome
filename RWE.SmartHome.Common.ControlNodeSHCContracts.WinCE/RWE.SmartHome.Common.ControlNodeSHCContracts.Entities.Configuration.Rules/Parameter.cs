using System.Xml.Serialization;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;

public class Parameter
{
	public string Name { get; set; }

	[XmlElement(ElementName = "Val")]
	public DataBinding Value { get; set; }
}
