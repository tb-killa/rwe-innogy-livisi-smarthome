using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.Calculation;

[Serializable]
[GeneratedCode("xsd", "2.0.50727.3038")]
[XmlType(Namespace = "urn:calculationxsd")]
[DesignerCategory("code")]
[DebuggerStepThrough]
public class calculationGetType
{
	private uint calculation_idField;

	private bool calculation_idFieldSpecified;

	[XmlAttribute]
	public uint calculation_id
	{
		get
		{
			return calculation_idField;
		}
		set
		{
			calculation_idField = value;
		}
	}

	[XmlIgnore]
	public bool calculation_idSpecified
	{
		get
		{
			return calculation_idFieldSpecified;
		}
		set
		{
			calculation_idFieldSpecified = value;
		}
	}
}
