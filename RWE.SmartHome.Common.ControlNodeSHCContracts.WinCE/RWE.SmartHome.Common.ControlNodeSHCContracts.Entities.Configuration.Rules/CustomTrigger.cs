using System.Collections.Generic;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;

public class CustomTrigger : Entity
{
	[XmlElement(ElementName = "Ety")]
	public LinkBinding Entity { get; set; }

	[XmlElement(ElementName = "Tp")]
	public string Type { get; set; }

	[XmlArray(ElementName = "Ppts")]
	[XmlArrayItem(ElementName = "Ppt")]
	public List<Property> Properties { get; set; }

	[XmlElement(ElementName = "Ns")]
	public string Namespace { get; set; }

	protected override Entity CreateClone()
	{
		return new CustomTrigger();
	}
}
