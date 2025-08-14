using System.Collections.Generic;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;

public class Condition
{
	public List<Tag> Tags { get; set; }

	[XmlElement(ElementName = "LHOp")]
	public DataBinding LeftHandOperand { get; set; }

	[XmlElement(ElementName = "RHOp")]
	public DataBinding RightHandOperand { get; set; }

	[XmlElement(ElementName = "Opt")]
	public ConditionOperator Operator { get; set; }

	public Condition()
	{
		Tags = new List<Tag>();
	}

	protected void TransferProperties(Condition clone)
	{
		clone.Tags.AddRange(Tags);
	}

	protected Condition CreateClone()
	{
		return new Condition();
	}
}
