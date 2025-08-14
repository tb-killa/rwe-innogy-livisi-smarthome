using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;

public class TriggerDefinition : Entity
{
	[XmlElement(ElementName = "Acn")]
	public TriggerAction Action { get; set; }

	[XmlArrayItem(ElementName = "TgCdt")]
	[XmlArray(ElementName = "Cdts")]
	public List<TriggerCondition> Conditions { get; set; }

	public TriggerDefinition()
	{
		Conditions = new List<TriggerCondition>();
	}

	protected override Entity CreateClone()
	{
		return new TriggerDefinition();
	}

	protected override void TransferProperties(Entity clone)
	{
		base.TransferProperties(clone);
		if (!(clone is TriggerDefinition triggerDefinition))
		{
			throw new InvalidOperationException("TriggerDefinition: Invalid transfer properties call");
		}
		triggerDefinition.Action = Action;
		triggerDefinition.Conditions = Conditions.Select((TriggerCondition p) => p.Clone()).ToList();
	}

	public new TriggerDefinition Clone()
	{
		return base.Clone() as TriggerDefinition;
	}

	public new TriggerDefinition Clone(Guid tag)
	{
		return base.Clone(tag) as TriggerDefinition;
	}
}
