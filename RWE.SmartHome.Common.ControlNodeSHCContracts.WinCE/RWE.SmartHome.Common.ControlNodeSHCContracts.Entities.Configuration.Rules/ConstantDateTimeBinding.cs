using System;
using System.Globalization;
using System.Xml.Serialization;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;

public class ConstantDateTimeBinding : DataBinding, IConstantBinding
{
	public DateTime Value { get; set; }

	[XmlIgnore]
	public string ValueAsString => Value.ToString(CultureInfo.InvariantCulture);

	protected override DataBinding CreateClone()
	{
		return new ConstantDateTimeBinding();
	}

	protected override void TransferProperties(DataBinding clone)
	{
		if (!(clone is ConstantDateTimeBinding constantDateTimeBinding))
		{
			throw new InvalidOperationException("ConstantDataBinder: Invalid transfer properties call");
		}
		constantDateTimeBinding.Value = Value;
	}

	public new ConstantDateTimeBinding Clone()
	{
		return base.Clone() as ConstantDateTimeBinding;
	}

	public new ConstantDateTimeBinding Clone(Guid tag)
	{
		return base.Clone(tag) as ConstantDateTimeBinding;
	}
}
