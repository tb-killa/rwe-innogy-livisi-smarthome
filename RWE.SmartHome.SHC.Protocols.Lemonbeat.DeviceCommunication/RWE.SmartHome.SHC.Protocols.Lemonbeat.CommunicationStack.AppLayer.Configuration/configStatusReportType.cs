using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.Configuration;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("xsd", "2.0.50727.3038")]
[XmlType(Namespace = "urn:configurationxsd")]
[DesignerCategory("code")]
public class configStatusReportType
{
	private uint statusField;

	[XmlAttribute]
	public uint status
	{
		get
		{
			return statusField;
		}
		set
		{
			statusField = value;
		}
	}
}
