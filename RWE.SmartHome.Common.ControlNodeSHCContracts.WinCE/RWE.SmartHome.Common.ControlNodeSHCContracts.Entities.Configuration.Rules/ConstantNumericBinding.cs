using System;
using System.Globalization;
using System.Xml.Serialization;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;

public class ConstantNumericBinding : DataBinding, IConstantBinding
{
	public decimal Value { get; set; }

	[XmlIgnore]
	public string ValueAsString => Value.ToString(CultureInfo.InvariantCulture);

	protected override DataBinding CreateClone()
	{
		return new ConstantNumericBinding();
	}

	protected override void TransferProperties(DataBinding clone)
	{
		if (!(clone is ConstantNumericBinding constantNumericBinding))
		{
			throw new InvalidOperationException("ConstantDataBinder: Invalid transfer properties call");
		}
		constantNumericBinding.Value = Value;
	}

	public new ConstantNumericBinding Clone()
	{
		return base.Clone() as ConstantNumericBinding;
	}

	public new ConstantNumericBinding Clone(Guid tag)
	{
		return base.Clone(tag) as ConstantNumericBinding;
	}
}
