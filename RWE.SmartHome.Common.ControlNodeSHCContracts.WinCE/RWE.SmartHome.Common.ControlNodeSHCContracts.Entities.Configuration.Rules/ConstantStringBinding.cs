using System;
using System.Xml.Serialization;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;

public class ConstantStringBinding : DataBinding, IConstantBinding
{
	public string Value { get; set; }

	[XmlIgnore]
	public string ValueAsString => Value;

	protected override DataBinding CreateClone()
	{
		return new ConstantStringBinding();
	}

	protected override void TransferProperties(DataBinding clone)
	{
		if (!(clone is ConstantStringBinding constantStringBinding))
		{
			throw new InvalidOperationException("ConstantDataBinder: Invalid transfer properties call");
		}
		constantStringBinding.Value = Value;
	}

	public new ConstantStringBinding Clone()
	{
		return base.Clone() as ConstantStringBinding;
	}

	public new ConstantStringBinding Clone(Guid tag)
	{
		return base.Clone(tag) as ConstantStringBinding;
	}
}
