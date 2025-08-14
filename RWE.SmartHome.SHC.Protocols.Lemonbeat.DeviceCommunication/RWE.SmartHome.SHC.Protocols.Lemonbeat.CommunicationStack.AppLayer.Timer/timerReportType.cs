using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.Timer;

[Serializable]
[GeneratedCode("xsd", "2.0.50727.3038")]
[XmlType(Namespace = "urn:timerxsd")]
[DesignerCategory("code")]
[DebuggerStepThrough]
public class timerReportType
{
	private executeType[] executeField;

	[XmlElement("execute")]
	public executeType[] execute
	{
		get
		{
			return executeField;
		}
		set
		{
			executeField = value;
		}
	}
}
