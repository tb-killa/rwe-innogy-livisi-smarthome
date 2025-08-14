using System.Collections.Generic;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;

public class Trigger : Entity
{
	[XmlElement(ElementName = "EvTp")]
	public string EventType { get; set; }

	[XmlElement(ElementName = "Ety")]
	public LinkBinding Entity { get; set; }

	[XmlArray(ElementName = "TgCdts")]
	[XmlArrayItem(ElementName = "TgCdt")]
	public List<Condition> TriggerConditions { get; set; }

	protected override Entity CreateClone()
	{
		return new Trigger();
	}
}
