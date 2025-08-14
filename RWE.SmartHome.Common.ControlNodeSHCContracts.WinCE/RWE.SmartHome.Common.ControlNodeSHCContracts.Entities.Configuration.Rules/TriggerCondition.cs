using System;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;

public class TriggerCondition : Entity
{
	public Property Threshold { get; set; }

	[XmlElement(ElementName = "Opt")]
	public Operator Operator { get; set; }

	protected override Entity CreateClone()
	{
		return new TriggerCondition();
	}

	protected override void TransferProperties(Entity clone)
	{
		base.TransferProperties(clone);
		if (!(clone is TriggerCondition triggerCondition))
		{
			throw new InvalidOperationException("TriggerCondition: Invalid transfer properties call");
		}
		triggerCondition.Threshold = Threshold.Clone();
		triggerCondition.Operator = Operator;
	}

	public new TriggerCondition Clone()
	{
		return base.Clone() as TriggerCondition;
	}

	public new TriggerCondition Clone(Guid tag)
	{
		return base.Clone(tag) as TriggerCondition;
	}
}
