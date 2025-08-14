using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.DeviceDescription;

[Serializable]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DebuggerStepThrough]
[XmlType(Namespace = "urn:device_descriptionxsd")]
[DesignerCategory("code")]
public class deviceDescriptionType
{
	private infoType[] infoField;

	[XmlElement("info")]
	public infoType[] info
	{
		get
		{
			return infoField;
		}
		set
		{
			infoField = value;
		}
	}
}
