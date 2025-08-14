using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;

public class Rule : Entity
{
	[XmlElement(ElementName = "INTId")]
	public Guid InteractionId { get; set; }

	[XmlArrayItem(ElementName = "Tg")]
	[XmlArray(ElementName = "Tgs")]
	public List<Trigger> Triggers { get; set; }

	[XmlArrayItem(ElementName = "CtTg")]
	[XmlArray(ElementName = "CtTgs")]
	public List<CustomTrigger> CustomTriggers { get; set; }

	[XmlArray(ElementName = "Cdts")]
	[XmlArrayItem(ElementName = "Cdt")]
	public List<Condition> Conditions { get; set; }

	[XmlArrayItem(ElementName = "Actn")]
	[XmlArray(ElementName = "Actns")]
	public List<ActionDescription> Actions { get; set; }

	[XmlElement(ElementName = "CED")]
	public int ConditionEvaluationDelay { get; set; }

	protected override Entity CreateClone()
	{
		return new Rule();
	}

	protected override void TransferProperties(Entity clone)
	{
		base.TransferProperties(clone);
		Rule cloneCast = clone as Rule;
		if (cloneCast != null)
		{
			cloneCast.InteractionId = InteractionId;
			cloneCast.Triggers = Triggers;
			cloneCast.CustomTriggers = CustomTriggers;
			cloneCast.Conditions = Conditions;
			cloneCast.Tags = ((base.Tags != null) ? base.Tags.ToList() : null);
			cloneCast.Actions = ((Actions != null) ? (from a in Actions
				where a != null
				select a.Clone(cloneCast.CloneTag)).ToList() : null);
			cloneCast.ConditionEvaluationDelay = ConditionEvaluationDelay;
			return;
		}
		throw new InvalidOperationException("Rule: Invalid transfer properties call");
	}

	public new Rule Clone()
	{
		return base.Clone() as Rule;
	}

	public new Rule Clone(Guid tag)
	{
		return base.Clone(tag) as Rule;
	}
}
