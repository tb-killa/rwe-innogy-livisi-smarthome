using System.Collections.Generic;
using System.Xml.Serialization;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.ShcType;

public class RestrictionState
{
	[XmlAttribute]
	public ShcRestriction Restriction { get; set; }

	[XmlAttribute]
	public bool IsRestrictionEnabled { get; set; }

	[XmlElement]
	public List<ShcTypeParameterState> Parameters { get; set; }
}
