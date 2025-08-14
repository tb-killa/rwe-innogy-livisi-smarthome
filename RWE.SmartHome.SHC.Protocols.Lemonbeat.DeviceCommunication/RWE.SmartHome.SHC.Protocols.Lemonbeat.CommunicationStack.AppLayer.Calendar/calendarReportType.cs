using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.Calendar;

[Serializable]
[XmlType(Namespace = "urn:calendarxsd")]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class calendarReportType
{
	private taskType[] taskField;

	[XmlElement("task")]
	public taskType[] task
	{
		get
		{
			return taskField;
		}
		set
		{
			taskField = value;
		}
	}
}
