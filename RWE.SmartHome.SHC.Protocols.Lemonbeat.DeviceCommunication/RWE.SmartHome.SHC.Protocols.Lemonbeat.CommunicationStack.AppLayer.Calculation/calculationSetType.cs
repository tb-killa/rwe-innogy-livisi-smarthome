using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.Calculation;

[Serializable]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DesignerCategory("code")]
[XmlType(Namespace = "urn:calculationxsd")]
[DebuggerStepThrough]
public class calculationSetType
{
	private calculationType[] calculationField;

	[XmlElement("calculation")]
	public calculationType[] calculation
	{
		get
		{
			return calculationField;
		}
		set
		{
			calculationField = value;
		}
	}
}
