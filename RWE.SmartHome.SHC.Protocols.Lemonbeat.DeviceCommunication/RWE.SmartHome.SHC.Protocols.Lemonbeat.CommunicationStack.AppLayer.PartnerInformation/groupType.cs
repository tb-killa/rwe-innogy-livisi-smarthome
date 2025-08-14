using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.PartnerInformation;

[Serializable]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DesignerCategory("code")]
[DebuggerStepThrough]
[XmlType(Namespace = "urn:partner_informationxsd")]
public class groupType
{
	private groupPartnerType[] partnerField;

	private uint partner_idField;

	[XmlElement("partner")]
	public groupPartnerType[] partner
	{
		get
		{
			return partnerField;
		}
		set
		{
			partnerField = value;
		}
	}

	[XmlAttribute]
	public uint partner_id
	{
		get
		{
			return partner_idField;
		}
		set
		{
			partner_idField = value;
		}
	}
}
