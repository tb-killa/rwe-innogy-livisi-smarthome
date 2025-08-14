using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.Status;

[Serializable]
[XmlType(Namespace = "urn:statusxsd")]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class statusReportLevelType
{
	private uint levelField;

	[XmlAttribute]
	public uint level
	{
		get
		{
			return levelField;
		}
		set
		{
			levelField = value;
		}
	}
}
