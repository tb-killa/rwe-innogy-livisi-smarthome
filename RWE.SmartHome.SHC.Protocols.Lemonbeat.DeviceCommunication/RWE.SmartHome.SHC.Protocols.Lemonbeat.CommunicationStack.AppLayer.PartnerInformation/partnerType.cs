using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.PartnerInformation;

[Serializable]
[DesignerCategory("code")]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DebuggerStepThrough]
[XmlType(Namespace = "urn:partner_informationxsd")]
public class partnerType
{
	private infoType[] infoField;

	private uint partner_idField;

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
