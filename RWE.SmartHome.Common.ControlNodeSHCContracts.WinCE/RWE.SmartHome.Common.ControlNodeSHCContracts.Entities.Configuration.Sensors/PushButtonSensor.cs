using System;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Sensors;

public class PushButtonSensor : LogicalDevice
{
	private const string PropNameButtonCount = "PushButtons";

	[XmlIgnore]
	public int ButtonCount
	{
		get
		{
			return (int)(base.Properties.GetDecimalValue("PushButtons") ?? 0m);
		}
		set
		{
			base.Properties.SetDecimal("PushButtons", value);
		}
	}

	[XmlIgnore]
	public override string PrimaryPropertyName
	{
		get
		{
			return "Index";
		}
		set
		{
		}
	}

	protected override Entity CreateClone()
	{
		return new PushButtonSensor();
	}

	public new PushButtonSensor Clone()
	{
		return (PushButtonSensor)base.Clone();
	}

	public new PushButtonSensor Clone(Guid tag)
	{
		return (PushButtonSensor)base.Clone(tag);
	}
}
