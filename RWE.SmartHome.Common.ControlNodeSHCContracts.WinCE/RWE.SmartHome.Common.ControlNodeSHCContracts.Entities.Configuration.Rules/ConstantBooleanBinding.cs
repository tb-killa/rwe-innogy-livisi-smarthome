using System;
using System.Xml.Serialization;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;

public class ConstantBooleanBinding : DataBinding, IConstantBinding
{
	public bool Value { get; set; }

	[XmlIgnore]
	public string ValueAsString => Value.ToString();

	protected override DataBinding CreateClone()
	{
		return new ConstantBooleanBinding();
	}

	protected override void TransferProperties(DataBinding clone)
	{
		if (!(clone is ConstantBooleanBinding constantBooleanBinding))
		{
			throw new InvalidOperationException("ConstantBooleanBinding: Invalid transfer properties call");
		}
		constantBooleanBinding.Value = Value;
	}

	public new ConstantBooleanBinding Clone()
	{
		return base.Clone() as ConstantBooleanBinding;
	}

	public new ConstantBooleanBinding Clone(Guid tag)
	{
		return base.Clone(tag) as ConstantBooleanBinding;
	}
}
