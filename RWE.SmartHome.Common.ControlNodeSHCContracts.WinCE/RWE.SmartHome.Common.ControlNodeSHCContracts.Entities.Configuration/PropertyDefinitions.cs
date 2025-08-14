using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

public class PropertyDefinitions
{
	[XmlArrayItem(ElementName = "Ppt")]
	[XmlArray(ElementName = "Pmts")]
	public List<Property> Properties { get; set; }

	public PropertyDefinitions Clone()
	{
		PropertyDefinitions propertyDefinitions = new PropertyDefinitions();
		if (Properties != null)
		{
			propertyDefinitions.Properties = Properties.Select((Property ap) => ap.Clone()).ToList();
		}
		return propertyDefinitions;
	}
}
