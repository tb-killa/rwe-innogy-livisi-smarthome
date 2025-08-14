using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;

public class ActionDescription : Entity
{
	[XmlElement(ElementName = "ActnTp")]
	public string ActionType { get; set; }

	[XmlElement(ElementName = "Tgt")]
	public LinkBinding Target { get; set; }

	[XmlArrayItem(ElementName = "Prm")]
	[XmlArray(ElementName = "Prms")]
	public List<Parameter> Data { get; set; }

	[XmlElement(ElementName = "Ns")]
	public string Namespace { get; set; }

	protected override Entity CreateClone()
	{
		return new ActionDescription();
	}

	protected override void TransferProperties(Entity clone)
	{
		base.TransferProperties(clone);
		ActionDescription cloneCast = clone as ActionDescription;
		if (cloneCast != null)
		{
			cloneCast.ActionType = ActionType;
			cloneCast.Namespace = Namespace;
			cloneCast.Target = Target;
			cloneCast.Tags = base.Tags.ToList();
			cloneCast.Data = ((Data != null) ? (from p in Data
				where p != null
				select new Parameter
				{
					Name = p.Name,
					Value = p.Value.Clone(cloneCast.CloneTag)
				}).ToList() : null);
			return;
		}
		throw new InvalidOperationException("RuleAction: Invalid transfer properties call");
	}

	public new ActionDescription Clone()
	{
		return base.Clone() as ActionDescription;
	}

	public new ActionDescription Clone(Guid tag)
	{
		return base.Clone(tag) as ActionDescription;
	}
}
