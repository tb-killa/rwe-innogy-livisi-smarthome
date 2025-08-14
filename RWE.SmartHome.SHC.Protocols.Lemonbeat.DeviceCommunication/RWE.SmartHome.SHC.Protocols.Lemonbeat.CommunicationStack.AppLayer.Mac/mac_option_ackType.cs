using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.Mac;

[Serializable]
[DesignerCategory("code")]
[XmlType(Namespace = "urn:macxsd")]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DebuggerStepThrough]
public class mac_option_ackType
{
	private uint rssiField;

	[XmlAttribute]
	public uint rssi
	{
		get
		{
			return rssiField;
		}
		set
		{
			rssiField = value;
		}
	}
}
