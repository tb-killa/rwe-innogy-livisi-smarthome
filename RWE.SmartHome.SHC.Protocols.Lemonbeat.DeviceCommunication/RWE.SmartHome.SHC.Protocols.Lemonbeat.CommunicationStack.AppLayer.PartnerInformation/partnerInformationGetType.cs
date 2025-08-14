using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.PartnerInformation;

[Serializable]
[XmlType(Namespace = "urn:partner_informationxsd")]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class partnerInformationGetType
{
	private uint partner_idField;

	private bool partner_idFieldSpecified;

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

	[XmlIgnore]
	public bool partner_idSpecified
	{
		get
		{
			return partner_idFieldSpecified;
		}
		set
		{
			partner_idFieldSpecified = value;
		}
	}
}
