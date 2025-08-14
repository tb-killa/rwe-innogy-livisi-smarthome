using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.PartnerInformation;

[Serializable]
[DesignerCategory("code")]
[XmlType(Namespace = "urn:partner_informationxsd")]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DebuggerStepThrough]
public class groupPartnerType
{
	private uint partner_idField;

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
