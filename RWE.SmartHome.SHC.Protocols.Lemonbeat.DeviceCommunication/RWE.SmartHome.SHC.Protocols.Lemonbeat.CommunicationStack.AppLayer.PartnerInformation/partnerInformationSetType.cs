using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.PartnerInformation;

[Serializable]
[DebuggerStepThrough]
[XmlType(Namespace = "urn:partner_informationxsd")]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DesignerCategory("code")]
public class partnerInformationSetType
{
	private object[] itemsField;

	[XmlElement("group", typeof(groupType))]
	[XmlElement("partner", typeof(partnerType))]
	public object[] Items
	{
		get
		{
			return itemsField;
		}
		set
		{
			itemsField = value;
		}
	}
}
