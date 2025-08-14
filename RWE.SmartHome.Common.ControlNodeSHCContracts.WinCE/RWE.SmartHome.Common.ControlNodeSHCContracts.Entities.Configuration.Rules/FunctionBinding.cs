using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;

public class FunctionBinding : DataBinding
{
	[XmlArray(ElementName = "Prms")]
	[XmlArrayItem(ElementName = "Prm")]
	public List<Parameter> Parameters { get; set; }

	[XmlElement(ElementName = "Fct")]
	public FunctionIdentifier Function { get; set; }

	protected override DataBinding CreateClone()
	{
		return new FunctionBinding();
	}

	protected override void TransferProperties(DataBinding clone)
	{
		if (!(clone is FunctionBinding functionBinding))
		{
			throw new InvalidOperationException("FunctionBinding: Invalid transfer properties call");
		}
		functionBinding.Parameters = Parameters.Select((Parameter p) => new Parameter
		{
			Name = p.Name,
			Value = p.Value.Clone(p.Value.CloneTag)
		}).ToList();
		functionBinding.Function = Function;
	}

	public new FunctionBinding Clone()
	{
		return base.Clone() as FunctionBinding;
	}

	public new FunctionBinding Clone(Guid tag)
	{
		return base.Clone(tag) as FunctionBinding;
	}
}
