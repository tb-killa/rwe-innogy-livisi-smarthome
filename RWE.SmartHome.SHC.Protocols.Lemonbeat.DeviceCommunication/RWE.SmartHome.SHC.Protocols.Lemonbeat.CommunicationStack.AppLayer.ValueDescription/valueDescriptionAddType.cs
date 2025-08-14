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
public class valueDescriptionAddType
{
	private valueDescriptionType[] value_descriptionField;

	[XmlElement("value_description")]
	public valueDescriptionType[] value_description
	{
		get
		{
			return value_descriptionField;
		}
		set
		{
			value_descriptionField = value;
		}
	}
}
