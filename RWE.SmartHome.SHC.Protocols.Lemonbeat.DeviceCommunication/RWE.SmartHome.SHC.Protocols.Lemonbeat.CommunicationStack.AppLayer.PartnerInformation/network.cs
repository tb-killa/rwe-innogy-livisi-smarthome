using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.PartnerInformation;

[Serializable]
[DesignerCategory("code")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true, Namespace = "urn:partner_informationxsd")]
[XmlRoot(Namespace = "urn:partner_informationxsd", IsNullable = false)]
[GeneratedCode("xsd", "2.0.50727.3038")]
public class network
{
	private networkDevice[] deviceField;

	private uint versionField;

	[XmlElement("device")]
	public networkDevice[] device
	{
		get
		{
			return deviceField;
		}
		set
		{
			deviceField = value;
		}
	}

	[XmlAttribute]
	public uint version
	{
		get
		{
			return versionField;
		}
		set
		{
			versionField = value;
		}
	}
}
