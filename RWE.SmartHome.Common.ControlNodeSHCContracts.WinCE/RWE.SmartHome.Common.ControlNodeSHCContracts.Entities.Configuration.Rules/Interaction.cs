using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;

[XmlType("INT")]
public class Interaction : Entity
{
	public string Name { get; set; }

	[XmlElement(ElementName = "CrDt")]
	public DateTime CreationDate { get; set; }

	[XmlElement(ElementName = "LChDt")]
	public DateTime LastChangeDate { get; set; }

	[XmlArray(ElementName = "Rls")]
	[XmlArrayItem(ElementName = "Rl")]
	public List<Rule> Rules { get; set; }

	[XmlElement(ElementName = "VFm")]
	public DateTime? ValidFrom { get; set; }

	[XmlElement(ElementName = "VTo")]
	public DateTime? ValidTo { get; set; }

	[XmlElement(ElementName = "FT")]
	public int Freezetime { get; set; }

	[XmlElement(ElementName = "IsItl")]
	public bool IsInternal { get; set; }

	protected override Entity CreateClone()
	{
		return new Interaction();
	}

	protected override void TransferProperties(Entity clone)
	{
		base.TransferProperties(clone);
		Interaction cloneCast = clone as Interaction;
		if (cloneCast != null)
		{
			cloneCast.Name = Name;
			cloneCast.CreationDate = CreationDate;
			cloneCast.LastChangeDate = LastChangeDate;
			cloneCast.Rules = ((Rules != null) ? (from r in Rules
				where r != null
				select r.Clone(cloneCast.CloneTag)).ToList() : null);
			cloneCast.ValidFrom = ValidFrom;
			cloneCast.ValidTo = ValidTo;
			cloneCast.Freezetime = Freezetime;
			cloneCast.IsInternal = IsInternal;
			return;
		}
		throw new InvalidOperationException("Interaction: Invalid transfer properties call");
	}

	public new Interaction Clone()
	{
		return base.Clone() as Interaction;
	}

	public new Interaction Clone(Guid tag)
	{
		return base.Clone(tag) as Interaction;
	}
}
