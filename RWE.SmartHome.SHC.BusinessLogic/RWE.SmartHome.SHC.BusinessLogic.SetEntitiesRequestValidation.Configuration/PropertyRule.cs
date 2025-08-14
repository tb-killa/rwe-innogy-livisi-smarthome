using System.Collections.Generic;
using System.Xml.Serialization;
using RWE.SmartHome.SHC.BusinessLogic.SetEntitiesRequestValidation.Configuration.ValueConstrains;

namespace RWE.SmartHome.SHC.BusinessLogic.SetEntitiesRequestValidation.Configuration;

public class PropertyRule
{
	[XmlAttribute("name")]
	public string Name { get; set; }

	[XmlAttribute("type")]
	public string Type { get; set; }

	[XmlAttribute("required")]
	public bool Required { get; set; }

	public List<ValueConstraint> ValueConstraints { get; set; }
}
