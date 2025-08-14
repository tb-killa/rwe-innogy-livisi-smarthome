using System.Xml.Serialization;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.ShcType;

public class ShcTypeParameterState
{
	[XmlAttribute]
	public string ParameterName { get; set; }

	[XmlAttribute]
	public string CurrentState { get; set; }
}
