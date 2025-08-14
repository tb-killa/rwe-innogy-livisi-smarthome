using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.Action;

[Serializable]
[DesignerCategory("code")]
[XmlType(Namespace = "urn:actionxsd")]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DebuggerStepThrough]
public class actionReportType
{
	private actionType[] actionField;

	[XmlElement("action")]
	public actionType[] action
	{
		get
		{
			return actionField;
		}
		set
		{
			actionField = value;
		}
	}
}
