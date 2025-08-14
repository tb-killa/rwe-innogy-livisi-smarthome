using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.ValueDescription;

[Serializable]
[DesignerCategory("code")]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DebuggerStepThrough]
[XmlType(Namespace = "urn:value_descriptionxsd")]
public class numberFormatType
{
	private string unitField;

	private double minField;

	private double maxField;

	private double stepField;

	[XmlAttribute]
	public string unit
	{
		get
		{
			return unitField;
		}
		set
		{
			unitField = value;
		}
	}

	[XmlAttribute]
	public double min
	{
		get
		{
			return minField;
		}
		set
		{
			minField = value;
		}
	}

	[XmlAttribute]
	public double max
	{
		get
		{
			return maxField;
		}
		set
		{
			maxField = value;
		}
	}

	[XmlAttribute]
	public double step
	{
		get
		{
			return stepField;
		}
		set
		{
			stepField = value;
		}
	}
}
