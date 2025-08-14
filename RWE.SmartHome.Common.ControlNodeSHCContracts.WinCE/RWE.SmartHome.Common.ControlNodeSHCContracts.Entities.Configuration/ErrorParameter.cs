using System.Xml.Serialization;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

public struct ErrorParameter
{
	[XmlAttribute]
	public ErrorParameterKey Key { get; set; }

	[XmlAttribute]
	public string Value { get; set; }
}
