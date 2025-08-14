using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;

[XmlInclude(typeof(ConstantStringBinding))]
[XmlInclude(typeof(ConstantDateTimeBinding))]
[XmlInclude(typeof(LinkBinding))]
[XmlInclude(typeof(ConstantNumericBinding))]
[XmlInclude(typeof(ConstantBooleanBinding))]
[XmlInclude(typeof(FunctionBinding))]
public abstract class DataBinding
{
	[XmlIgnore]
	public Guid CloneTag { get; set; }

	[XmlIgnore]
	public bool IsClone => CloneTag != Guid.Empty;

	[XmlIgnore]
	public int Version { get; set; }

	public List<Tag> Tags { get; set; }

	protected DataBinding()
	{
		Tags = new List<Tag>();
	}

	protected abstract DataBinding CreateClone();

	protected virtual void TransferProperties(DataBinding clone)
	{
		clone.Tags.AddRange(Tags);
	}

	public virtual DataBinding Clone()
	{
		return Clone(Guid.Empty);
	}

	public virtual DataBinding Clone(Guid tag)
	{
		DataBinding dataBinding = CreateClone();
		dataBinding.CloneTag = tag;
		TransferProperties(dataBinding);
		return dataBinding;
	}
}
